using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.Enums;
using JobSearchFullWebSite.Models;
using JobSearchFullWebSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Controllers
{
    public class JobController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public JobController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(string search,int? cityId,int? categoryId,string[] types,string[] experiences,string[] careerLevels,string[] qualifications, int page = 1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.Search = search;
            ViewBag.CityId = cityId;
            ViewBag.CategoryId = categoryId;
            List<Job> Jobs = _context.Jobs.Where(x=> x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted).ToList();
            if (search!=null)
            {
                Jobs = Jobs.Where(x =>  x.Name.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (cityId != null)
            {
                Jobs = Jobs.Where(x => x.CityId == cityId).ToList();
            }
            if (categoryId != null)
            {
                Jobs = Jobs.Where(x => x.JobCategoryId == categoryId).ToList();
            }
            //if (job.City!=null)
            //{
            //    if (job.City.Id!=0)
            //    {
            //        Jobs = Jobs.Where(x => x.CityId == job.City.Id).ToList();
            //        ViewBag.CityId = job.City.Id;
            //    }
            //}
            ViewBag.TotalPageCount = Math.Ceiling(Jobs.Count() / 8m);
            JobIndexViewModel jobVM = new JobIndexViewModel
            {
                Jobs = _context.Jobs.Include(x => x.JobImages).Include(x=>x.Employer).ThenInclude(x=>x.Category).Include(x => x.JobCategory).Include(X => X.City).Where(x=> x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted ).ToList(),
                Cities = _context.Cities.ToList(),
                JobCategories = _context.JobCategories.ToList(),
                Candidate= (_context.Candidates.Include(x => x.AppUser).FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name) != null ? _context.Candidates.Include(x => x.AppUser).FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name && x.IsDeleted==false) : null)

            };
            if (search != null)
            {
                jobVM.Jobs = jobVM.Jobs.Where(x => x.Name.ToUpper().Contains(search.ToUpper().Trim())).ToList();
            }

    
                if (cityId != 0)
                {
                    City city = _context.Cities.FirstOrDefault(x => x.Id == cityId);
                    if (city!=null)
                    {
                        jobVM.Jobs = jobVM.Jobs.Where(x => x.City.CityName == city.CityName).ToList();
                    }
                }
                if (categoryId != 0)
                {
                    JobCategory jobCategory = _context.JobCategories.FirstOrDefault(x => x.Id == categoryId);
                    if (jobCategory!=null)
                    {
                        jobVM.Jobs = jobVM.Jobs.Where(x => x.JobCategory.Name == jobCategory.Name).ToList();
                    }
                }

            List<Job> sendJobs = new List<Job>();
            if (types != null)
            {
                DescriptionAttribute Freelance = (DescriptionAttribute)(((typeof(JobType)).GetField(Enums.JobType.Freelance.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute FullTime = (DescriptionAttribute)(((typeof(JobType)).GetField(Enums.JobType.FullTime.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute PartTime = (DescriptionAttribute)(((typeof(JobType)).GetField(Enums.JobType.PartTime.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Internship  = (DescriptionAttribute)(((typeof(JobType)).GetField(Enums.JobType.Internship.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Temporary = (DescriptionAttribute)(((typeof(JobType)).GetField(Enums.JobType.Temporary.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];

                foreach (var item in types)
                {
                    if (item == Freelance.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.JobType == Enums.JobType.Freelance).ToList());
                    }
                    else if (item == FullTime.Description)
                    {
                        sendJobs.AddRange( jobVM.Jobs.Where(x => x.JobType == Enums.JobType.FullTime).ToList());
                    }
                    else if (item == PartTime.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.JobType == Enums.JobType.PartTime).ToList());
                    }
                    else if (item == Internship.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.JobType == Enums.JobType.Internship).ToList());
                    }
                    else if (item == Temporary.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.JobType == Enums.JobType.Temporary).ToList());
                    }
                }
            }
            if (experiences != null)
            {
                DescriptionAttribute Fresh = (DescriptionAttribute)(((typeof(RequiredExperience)).GetField(Enums.RequiredExperience.Fresh.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Year_1 = (DescriptionAttribute)(((typeof(RequiredExperience)).GetField(Enums.RequiredExperience.Year_1.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Year_2 = (DescriptionAttribute)(((typeof(RequiredExperience)).GetField(Enums.RequiredExperience.Year_2.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Year_3 = (DescriptionAttribute)(((typeof(RequiredExperience)).GetField(Enums.RequiredExperience.Year_3.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Year_4 = (DescriptionAttribute)(((typeof(RequiredExperience)).GetField(Enums.RequiredExperience.Year_4.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute More = (DescriptionAttribute)(((typeof(RequiredExperience)).GetField(Enums.RequiredExperience.More.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                foreach (var item in experiences)
                {
                    if (item == Fresh.Description)
                    {
                        sendJobs.AddRange( jobVM.Jobs.Where(x => x.RequiredExperience == Enums.RequiredExperience.Fresh).ToList());
                    }
                    else if (item == Year_1.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredExperience == Enums.RequiredExperience.Year_1).ToList());
                    }
                    else if (item == Year_2.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredExperience == Enums.RequiredExperience.Year_2).ToList());
                    }
                    else if (item == Year_3.Description)
                    {
                        sendJobs.AddRange( jobVM.Jobs.Where(x => x.RequiredExperience == Enums.RequiredExperience.Year_3).ToList());
                    }
                    else if (item == Year_4.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredExperience == Enums.RequiredExperience.Year_4).ToList());
                    }
                    else if (item == More.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredExperience == Enums.RequiredExperience.More).ToList());
                    }
                }
            }
            if (careerLevels != null)
            {
                DescriptionAttribute Manager = (DescriptionAttribute)(((typeof(CareerLevel)).GetField(Enums.CareerLevel.Manager.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Officer = (DescriptionAttribute)(((typeof(CareerLevel)).GetField(Enums.CareerLevel.Officer.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Student = (DescriptionAttribute)(((typeof(CareerLevel)).GetField(Enums.CareerLevel.Student.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Executive = (DescriptionAttribute)(((typeof(CareerLevel)).GetField(Enums.CareerLevel.Executive.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Others = (DescriptionAttribute)(((typeof(CareerLevel)).GetField(Enums.CareerLevel.Others.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                foreach (var item in careerLevels)
                {
                    if (item == Manager.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.CareerLevel == Enums.CareerLevel.Manager).ToList());
                    }
                    else if (item == Officer.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.CareerLevel == Enums.CareerLevel.Officer).ToList());
                    }
                    else if (item == Student.Description)
                    {
                        sendJobs.AddRange( jobVM.Jobs.Where(x => x.CareerLevel == Enums.CareerLevel.Student).ToList());
                    }
                    else if (item == Executive.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.CareerLevel == Enums.CareerLevel.Executive).ToList());
                    }
                    else if (item == Others.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.CareerLevel == Enums.CareerLevel.Others).ToList());
                    }
                }
            }
            if (qualifications != null)
            {
                DescriptionAttribute Certificate = (DescriptionAttribute)(((typeof(Qualification)).GetField(Enums.Qualification.Certificate.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Associate = (DescriptionAttribute)(((typeof(Qualification)).GetField(Enums.Qualification.Associate.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Bachelor_Degree = (DescriptionAttribute)(((typeof(Qualification)).GetField(Enums.Qualification.Bachelor_Degree.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Master_Degree = (DescriptionAttribute)(((typeof(Qualification)).GetField(Enums.Qualification.Master_Degree.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                DescriptionAttribute Doctorate_Degree = (DescriptionAttribute)(((typeof(Qualification)).GetField(Enums.Qualification.Doctorate_Degree.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];
                foreach (var item in qualifications)
                {
                    if (item == Certificate.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredQualificationr == Enums.Qualification.Certificate).ToList());
                    }
                    else if (item == Associate.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredQualificationr == Enums.Qualification.Associate).ToList());
                    }
                    else if (item == Bachelor_Degree.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredQualificationr == Enums.Qualification.Bachelor_Degree).ToList());
                    }
                    else if (item == Master_Degree.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredQualificationr == Enums.Qualification.Master_Degree).ToList());
                    }
                    else if (item == Doctorate_Degree.Description)
                    {
                        sendJobs.AddRange(jobVM.Jobs.Where(x => x.RequiredQualificationr == Enums.Qualification.Doctorate_Degree).ToList());
                    }
                }
            }
            if (sendJobs.Count()!=0)
            {
                jobVM.Jobs = sendJobs;
                
            }
            jobVM.Jobs = jobVM.Jobs.Distinct().ToList();
            if(types.Count() == 0 && experiences.Count() == 0 && careerLevels.Count() == 0 && qualifications.Count() == 0)
            {
                jobVM.Jobs = jobVM.Jobs.Skip((page - 1) * 8).Take(8).ToList();
                ViewBag.Pagination = true;
            }
            else
            {
                ViewBag.Pagination = false;
            }
            return View(jobVM);
        }
        public IActionResult Detail(int id)
        {
            ViewBag.TotalCommentPage = Math.Ceiling(_context.JobComments.Include(x=>x.Job).Where(x => x.JobId == id && x.Job.IsDeleted==false && x.Job.ApplyStatus==Enums.ApplyStatus.Accepted).Count() / 4m);

            if (!_context.Jobs.Any(x => x.Id == id && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted))
            {
                return RedirectToAction("index", "home");
            }

            Job existjob = _context.Jobs.Include(x=>x.Employer).FirstOrDefault(x => x.Id == id && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted && x.Employer.IsDeleted==false);
            JobDetailViewModel JobVM = new JobDetailViewModel
            {
                Job = _context.Jobs.Include(x => x.JobCategory).Include(x => x.JobImages).Include(x=>x.JobComments).ThenInclude(x=>x.AppUser).ThenInclude(x=>x.Candidate).ThenInclude(x=>x.CandidateImages).Include(x => x.City).Include(x => x.Employer).ThenInclude(x => x.Category).Include(x => x.Employer).ThenInclude(x=>x.City).FirstOrDefault(x => x.Id == id && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted),
                EmployerImage = _context.EmployerImages.Include(x=>x.Employer).FirstOrDefault(x => x.IsPoster && x.Employer.IsDeleted==false),
                RelatedJobs = _context.Jobs.Include(x=>x.JobImages).Include(x => x.JobCategory).Include(x => x.City).Where(x => x.Employer.Name == existjob.Employer.Name && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted).ToList(),
                AppUser= _context.Users.Include(x=>x.Candidate).FirstOrDefault(x => x.UserName == User.Identity.Name && x.Candidate.IsDeleted==false),
            };

            return View(JobVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetContact(JobContact jobContact,int id)
        {
            if (!_context.Jobs.Any(x=>x.Id==id && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted))
            {
                return RedirectToAction("index");
            }
            jobContact.JobId = id;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Sehv askarlandi");
                return RedirectToAction("detail","job",id);
            }
            _context.JobContacts.Add(jobContact);
            _context.SaveChanges();
            return RedirectToAction("index","job");
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult AddJobToShortlist(int jobId,int candidateId)
        {
            Candidate candidate = _context.Candidates.FirstOrDefault(x => x.Id == candidateId && x.IsDeleted==false);
            Job job = _context.Jobs.FirstOrDefault(x => x.Id == jobId && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted);
            if (job == null || candidate == null) return RedirectToAction("index");
            ShortList shortList = new ShortList
            {
                CandidateId = candidateId,
                JobId = jobId
            };
            if (_context.ShortLists.Any(x=>x.CandidateId==candidateId && x.JobId==jobId))
            {
                return RedirectToAction("detail");
            }
            _context.ShortLists.Add(shortList);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Candidate")]
        public IActionResult RemoveJobFromShortlist(int id)
        {
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Candidate candidate = _context.Candidates.Include(x=>x.AppUser).FirstOrDefault(x => x.AppUser.Id == user.Id && x.IsDeleted==false);
            if (user == null || candidate == null) return RedirectToAction("index");
            ShortList shortList = _context.ShortLists.Include(x=>x.Candidate).Include(x=>x.Job).FirstOrDefault(x => x.JobId == id && x.Candidate.Id==candidate.Id && x.Candidate.IsDeleted==false && x.Job.IsDeleted == false && x.Job.ApplyStatus == Enums.ApplyStatus.Accepted);
            if (shortList==null)
            {
                return RedirectToAction("index");
            }
            _context.ShortLists.Remove(shortList);
            _context.SaveChanges();
            return RedirectToAction();
        }
        [Authorize(Roles = "Candidate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(JobComment coment)
        {
            if(coment.JobId==0 || coment.AppUserId==null ||coment.Comment==null || coment.Comment.Length>500) return RedirectToAction("index");
            Job job = _context.Jobs.Include(x => x.JobComments).FirstOrDefault(x => x.Id == coment.JobId && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted);

            if (job == null) return RedirectToAction("index");

            var user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && x.Candidate.IsDeleted==false);
            if (user == null) return RedirectToAction("index");
            coment.AppUserId = user.Id;

            //if (_context.JobComments.Any(x => x.JobId == coment.JobId && x.AppUserId == user.Id))        bunu acsam yalniz 1 rey yaza biler her isrifadeci
            //{
            //    return RedirectToAction("index");
            //}

            coment.CreatedAt = DateTime.UtcNow.AddHours(4);
            JobComment newComment = new JobComment
            {
                AppUserId=coment.AppUserId,
                JobId=coment.JobId,
                Comment=coment.Comment,
                CreatedAt=coment.CreatedAt,
            };
            await _context.JobComments.AddAsync(newComment);

            await _context.SaveChangesAsync();

            return RedirectToAction("detail", new { id = coment.JobId });
        }
        //[Authorize(Roles = "Member")]
        public IActionResult LoadComment(int id, int page = 1)
        {
            ViewBag.TotalCommentPage = Math.Ceiling(_context.JobComments.Where(x => x.JobId == id).Count() / 4m);
            ViewBag.SelectedPage = page;
            List<JobComment> comments = _context.JobComments.Include(x=>x.Job).Include(x => x.AppUser).ThenInclude(x=>x.Candidate).ThenInclude(x=>x.CandidateImages).Where(x => x.JobId == id && x.Job.IsDeleted==false && x.Job.ApplyStatus==Enums.ApplyStatus.Accepted && x.AppUser.Candidate.IsDeleted==false).OrderByDescending(x => x.CreatedAt).Skip((page - 1) * 4).Take(4).ToList();

            return PartialView("JobCommentPartialView", comments);
        }
        [Authorize(Roles = "Employer")]

        public IActionResult Edit(int jobId)
        {
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.JobCategories.ToList();
            ViewBag.Employers = _context.Employers.ToList();
            ViewBag.Languages = _context.Languages.ToList();
            Job existJob = _context.Jobs.Include(x => x.JobCategory).Include(x => x.Employer).ThenInclude(x=>x.EmployerImages).Include(x => x.Employer).ThenInclude(x => x.City).Include(x => x.Employer).ThenInclude(x => x.AppUser).Include(x => x.RequiredLanguages).Include(x => x.City).Include(x => x.JobImages).FirstOrDefault(x => x.Id == jobId && x.IsDeleted==false  && x.Employer.IsDeleted==false);
            if (existJob == null) return RedirectToAction("index");
            AppUser user = _context.Users.Include(x=>x.Employer).ThenInclude(x=>x.Jobs).FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null) return RedirectToAction("index");
            if(!user.Employer.Jobs.Any(x=>x.Id==jobId)) return RedirectToAction("index");

            return View(existJob);
        }
        [Authorize(Roles = "Employer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int jobId, Job editJob)
        {
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.JobCategories.ToList();
            ViewBag.Employers = _context.Employers.ToList();
            ViewBag.Languages = _context.Languages.ToList();
     
            Job existJob = _context.Jobs.Include(x => x.JobCategory).Include(x => x.Employer).ThenInclude(x=>x.EmployerImages).Include(x => x.RequiredLanguages).Include(x => x.Employer).ThenInclude(x => x.AppUser).Include(x => x.City).Include(x => x.JobImages).FirstOrDefault(x => x.Id == jobId && x.IsDeleted==false && x.Employer.IsDeleted==false);
            if (existJob == null) return RedirectToAction("index");

            if (ModelState.IsValid == false)
            {
                ModelState.AddModelError("", "Yeni isin yaradilmasi zamani problem askarlandi");
                return View(existJob);
            }
            if (editJob.File != null)
            {
                if (editJob.File.ContentType != "image/png" && editJob.File.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("File", "Jpeg ve ya png formatinda file daxil edilmelidir");
                    return View();
                }
                if (editJob.File.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("File", "File olcusu 5mb-dan cox olmaz!");
                    return View();
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + editJob.File.FileName;
                var path = Path.Combine(rootPath, "images/jobImage", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    editJob.File.CopyTo(stream);
                }
                if (existJob.JobImages.FirstOrDefault(x => x.IsPoster) != null)
                {
                    if (existJob.JobImages.FirstOrDefault(x => x.IsPoster).Image != null)
                    {
                        string rootPath2 = _env.WebRootPath;
                        var fileName2 = existJob.JobImages.FirstOrDefault(x => x.IsPoster).Image;
                        var path2 = Path.Combine(rootPath2, "images/jobImage", fileName2);
                        if (System.IO.File.Exists(path2))
                        {
                            System.IO.File.Delete(path2);
                        }
                        existJob.JobImages.FirstOrDefault(x => x.IsPoster).Image = fileName;
                    }
                }
                else
                {
                    JobImage jobImage = new JobImage
                    {
                        Image = fileName,
                        IsPoster = true,
                        JobId = jobId,
                    };
                    _context.JobImages.Add(jobImage);
                }
            }
            if(editJob.File==null && editJob.FileId == null)
            {
                string rootPath2 = _env.WebRootPath;
                JobImage jobImage = existJob.JobImages.FirstOrDefault(x => x.IsPoster);
                if (jobImage != null)
                {
                    var path2 = Path.Combine(rootPath2, "images/jobImage", jobImage.Image);
                    if (System.IO.File.Exists(path2))
                    {
                        System.IO.File.Delete(path2);
                    }
                    _context.JobImages.Remove(jobImage);
                }

            }
            if (editJob.Images != null)
            {
                foreach (var item in editJob.Images)
                {
                    if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("File", "Jpeg ve ya png formatinda file daxil edilmelidir");
                        return View();
                    }
                    if (item.Length > (1024 * 1024) * 5)
                    {
                        ModelState.AddModelError("File", "File olcusu 5mb-dan cox olmaz!");
                        return View();
                    }
                    string rootPath = _env.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + item.FileName;
                    var path = Path.Combine(rootPath, "images/jobImage", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    JobImage jobImage = new JobImage
                    {
                        Image = fileName,
                        IsPoster = false,
                        JobId = jobId,

                    };
                    _context.JobImages.Add(jobImage);
                }
            }
            if (existJob.JobImages.Where(x=>x.IsPoster==false) != null)
            {
                foreach (var item in existJob.JobImages.Where(x=>x.IsPoster==false))
                {
                    if (editJob.ImageIds != null)
                    {
                        if (!editJob.ImageIds.Contains(item.Id) && item.Id != 0)
                        {
                            //if (item.IsPoster == false)
                            //{
                            string rootPath = _env.WebRootPath;
                            var fileName = item.Image;
                            var path = Path.Combine(rootPath, "images/jobImage", fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            _context.JobImages.Remove(item);
                            //}
                        }
                    }
                    else
                    {
                        if (editJob.Images == null)
                        {
                            string rootPath = _env.WebRootPath;
                            var fileName = item.Image;
                            var path = Path.Combine(rootPath, "images/jobImage", fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            _context.JobImages.Remove(item);
                        }

                    }
                }
            }


            if (existJob.RequiredLanguages != null)
            {
                foreach (var item in existJob.RequiredLanguages)
                {
                    _context.JobRequiredLanguages.Remove(item);
                }
            }
            if (editJob.RequiredLanguageIds!=null)
            {
                foreach (var item in editJob.RequiredLanguageIds)
                {
                    if (_context.Languages.FirstOrDefault(x => x.Id == item) != null)
                    {
                        JobRequiredLanguage jobRequiredLanguage = new JobRequiredLanguage
                        {
                            Language = _context.Languages.FirstOrDefault(x => x.Id == item).Name,
                            JobId = jobId
                        };
                        _context.JobRequiredLanguages.Add(jobRequiredLanguage);
                    }
                }
            }
            existJob.Name = editJob.Name;
            existJob.JobType = editJob.JobType;
            existJob.OfferedMaxSalary = editJob.OfferedMaxSalary;
            existJob.OfferedMinSalary = editJob.OfferedMinSalary;
            existJob.IsUrgent = editJob.IsUrgent;
            existJob.IsFeatured = editJob.IsFeatured;
            existJob.OfferedSalaryForTime = editJob.OfferedSalaryForTime;
            existJob.RequiredGender = editJob.RequiredGender;
            existJob.RequiredQualificationr = editJob.RequiredQualificationr;
            existJob.RequiredExperience = editJob.RequiredExperience;
            existJob.CareerLevel = editJob.CareerLevel;
            existJob.ExpirationDate = editJob.ExpirationDate;
            existJob.CityId = editJob.CityId;
            existJob.JobCategoryId = editJob.JobCategoryId;
            existJob.JobContentTextEditor = editJob.JobContentTextEditor;
            existJob.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
