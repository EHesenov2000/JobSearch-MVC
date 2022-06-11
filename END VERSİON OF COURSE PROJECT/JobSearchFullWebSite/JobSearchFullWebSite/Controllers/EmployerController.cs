using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.DTOs;
using JobSearchFullWebSite.Enums;
using JobSearchFullWebSite.Models;
using JobSearchFullWebSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Controllers
{
    public class EmployerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public EmployerController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }
        public IActionResult Index(string search,int? cityId,int? categoryId, int page = 1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.CityId = cityId;
            ViewBag.CategoryId = categoryId;
            ViewBag.Search = search;
            List<Employer> employers = _context.Employers.Include(x=>x.City).Include(x=>x.AppUser).Include(x=>x.Category).Where(x => x.IsDeleted == false).ToList();
            if (cityId != null)
            {
                employers = _context.Employers.Include(x => x.City).Include(x => x.AppUser).Include(x => x.Category).Where(x => x.IsDeleted == false && x.City.Id == cityId).ToList();
            }
            if (categoryId != null)
            {
                employers = employers.Where(x => x.Category.Id == categoryId).ToList();
            }
            ViewBag.TotalPageCount = Math.Ceiling(employers.Count() / 9m);
            EmployerIndexViewModel employerVM = new EmployerIndexViewModel
            {
                Employers = _context.Employers.Include(x => x.EmployerImages).Include(X => X.City).Include(X => X.Category).Include(x => x.Jobs).Where(x=>x.IsDeleted==false).ToList(),
                Cities=_context.Cities.ToList(),
                Categories=_context.Categories.ToList(),
            };
            if (search!=null)
            {
                employerVM.Employers = _context.Employers.Include(x => x.EmployerImages).Include(X => X.City).Include(X => X.Category).Include(x => x.Jobs).Where(x => x.IsDeleted == false && x.Name.ToUpper().Contains(search.ToUpper())).ToList();
            }
     
                if (cityId != 0)
                {
                    City city = _context.Cities.FirstOrDefault(x => x.Id == cityId);
                    if (city != null)
                    {
                        employerVM.Employers = employerVM.Employers.Where(x => x.City.CityName == city.CityName).ToList();

                    }
                }
 
                if (categoryId != 0)
                {
                    Category category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
                    if (category != null)
                    {
                        employerVM.Employers = employerVM.Employers.Where(x => x.Category.Name == category.Name).ToList();

                    }
                }
            employerVM.Employers = employerVM.Employers.Skip((page - 1) * 9).Take(9).ToList();

            return View(employerVM);
        }
        public IActionResult Detail(int id)
        {
            if (!_context.Employers.Any(x => x.Id == id && x.IsDeleted==false))
            {
                return RedirectToAction("index", "home");
            }
            EmployerDetailViewModel employerDetailViewModel = new EmployerDetailViewModel
            {
                Employer = _context.Employers.Include(x => x.City).Include(x => x.EmployerImages).Include(x=>x.AppUser).Include(x => x.Category).FirstOrDefault(x => x.Id == id && x.IsDeleted==false),
                Jobs = _context.Jobs.Include(x=>x.JobCategory).Include(x=>x.JobImages).Include(x=>x.City).Include(x => x.Employer).Where(x => x.EmployerId == id && x.IsDeleted==false && x.ApplyStatus==Enums.ApplyStatus.Accepted).ToList(),
                Candidate = (_context.Candidates.Include(x => x.AppUser).FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name) != null ? _context.Candidates.Include(x => x.AppUser).FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name && x.IsDeleted==false) : null)

            };
            return View(employerDetailViewModel);
        }
        [Authorize(Roles = "Employer")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(int id,ApplyStatus  status)
        {
            Apply apply = _context.Applies.Include(x=>x.Job).Include(x=>x.AppUser).ThenInclude(x=>x.Candidate).FirstOrDefault(x => x.Id == id && x.AppUser.Candidate.IsDeleted==false && x.Job.IsDeleted==false && x.Job.ApplyStatus==Enums.ApplyStatus.Accepted);
            if (apply == null)
            {
                return RedirectToAction("index");
            }
            apply.ApplyStatus = status;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Employer")]

        public IActionResult EmployerDashboard(int id)
        {
            Employer employer = _context.Employers.Include(x=>x.Followers).Include(x=>x.AppUser).ThenInclude(x=>x.Applies).ThenInclude(x=>x.Job).Include(x=>x.EmployerContacts).Include(x=>x.City).Include(x=>x.EmployerImages).Include(x=>x.Jobs).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            if (employer == null) return RedirectToAction("index");
            return View(employer);
        }
        [Authorize(Roles = "Employer")]

        public IActionResult EmployerProileEdit(int id)
        {
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Employer existEmployer = _context.Employers.Include(x => x.EmployerImages).Include(x=>x.City).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            if (existEmployer == null) return RedirectToAction("index");
            EmployerEditDto employerEditDto = new EmployerEditDto();
            employerEditDto.Id = existEmployer.Id;
            employerEditDto.Name = existEmployer.Name;
            employerEditDto.FoundedDate = existEmployer.FoundedDate;
            employerEditDto.IsFeatured = false;
            employerEditDto.CreatedAt = existEmployer.CreatedAt;
            employerEditDto.PhoneNumber = existEmployer.PhoneNumber;
            employerEditDto.Email = existEmployer.Email;
            employerEditDto.FacebookUrl = existEmployer.FacebookUrl;
            employerEditDto.InstagramUrl = existEmployer.InstagramUrl;
            employerEditDto.LinkedinUrl = existEmployer.LinkedinUrl;
            employerEditDto.TwitterUrl = existEmployer.TwitterUrl;
            employerEditDto.CompanyContent = existEmployer.CompanyContent;
            employerEditDto.Website=existEmployer.Website;
            if (existEmployer.Category != null)
            {
                employerEditDto.CategoryId = existEmployer.Category.Id;
            }
            else
            {
                employerEditDto.CategoryId = 0;
            }
            if (existEmployer.City != null)
            {
                employerEditDto.CityId = existEmployer.City.Id;
                employerEditDto.City = existEmployer.City;
            }
            else
            {
                employerEditDto.CityId = 0;
            }
            if (existEmployer.EmployerImages.Count() != 0)
            {
                employerEditDto.EmployerImages = existEmployer.EmployerImages;
                if (existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster)!=null)
                {
                    employerEditDto.PosterImage = existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster).Image;

                }
                else
                {
                    employerEditDto.PosterImage = "";
                }
            }
            else
            {
                employerEditDto.PosterImage = "";
            }
            return View(employerEditDto);
        }
        [Authorize(Roles = "Employer")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmployerProileEdit(int id,EmployerEditDto employerEditDto)
        {
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Employer existEmployer = _context.Employers.Include(x => x.EmployerImages).FirstOrDefault(x => x.Id == id);
            if (existEmployer == null) return RedirectToAction("index");
            if (employerEditDto.CityId != 0)
            {
                if (!_context.Cities.Any(x => x.Id == employerEditDto.CityId)) return RedirectToAction("index");
            }
            if (employerEditDto.CategoryId != 0)
            {
                if (!_context.Categories.Any(x => x.Id == employerEditDto.CategoryId)) return RedirectToAction("index");
            }
            if (employerEditDto.CompanyContent == null)
            {
                ModelState.AddModelError("CompanyContent", "Context daxil edilmelidir");
                return View(employerEditDto);
            }
            if (employerEditDto.CompanyContent.Length > 1500)
            {
                ModelState.AddModelError("CompanyContent", "Maksimum uzunluq 1000-dir");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(employerEditDto);
            }
            if (employerEditDto.PosterImageFile != null)
            {
                if (employerEditDto.PosterImageFile.ContentType != "image/png" && employerEditDto.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "Jpeg ve ya png formatinda file daxil edilmelidir");
                    return View();
                }
                if (employerEditDto.PosterImageFile.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("PosterImageFile", "File olcusu 5mb-dan cox olmaz!");
                    return View();
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + employerEditDto.PosterImageFile.FileName;
                var path = Path.Combine(rootPath, "images/employerImage", fileName);




                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    employerEditDto.PosterImageFile.CopyTo(stream);
                }
                if (existEmployer.EmployerImages.Count != 0)
                {
                    if (existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster)!=null)
                    {
                        if (existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster).Image != null)
                        {
                            string existPath = Path.Combine(_env.WebRootPath, "images/employerImage", existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster).Image);
                            if (System.IO.File.Exists(existPath))
                            {
                                System.IO.File.Delete(existPath);
                            }
                        }
                    }
                }

                if (existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster) != null)
                {
                    existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster).Image = fileName;
                }
                else
                {
                    EmployerImage employerImage = new EmployerImage()
                    {
                        EmployerId = existEmployer.Id,
                        IsPoster = true,
                        Image = fileName,
                    };
                    _context.EmployerImages.Add(employerImage);
                    employerEditDto.PosterImage = fileName;
                }
            }
            if (employerEditDto.PosterImage == null)
            {
                //candidateEditDto.Image = "";
                if (existEmployer.EmployerImages.Any(x => x.IsPoster))
                {
                    string rootPath = _env.WebRootPath;
                    var fileName = existEmployer.EmployerImages.FirstOrDefault(x => x.IsPoster).Image; ;
                    var path = Path.Combine(rootPath, "images/employerImage", fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);

                    }
                    _context.EmployerImages.Remove(_context.EmployerImages.FirstOrDefault(x => x.EmployerId == existEmployer.Id && x.IsPoster));

                }

            }
            if (employerEditDto.Images != null)
            {
                foreach (var item in employerEditDto.Images)
                {
                    if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("Images", "Mime type yanlisdir!");
                        return View(employerEditDto);
                    }

                    if (item.Length > (1024 * 1024) * 5)
                    {
                        ModelState.AddModelError("Images", "Faly olcusu 5MB-dan cox ola bilmez!");
                        return View(employerEditDto);
                    }

                    string rootPath = _env.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + item.FileName;
                    var path = Path.Combine(rootPath, "images/employerImage", fileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    EmployerImage employerImage = new EmployerImage
                    {
                        Image = fileName,
                        IsPoster = false,
                        EmployerId = existEmployer.Id
                    };
                    _context.EmployerImages.Add(employerImage);
                }
            }
            if (existEmployer.EmployerImages != null)
            {
                foreach (var item in existEmployer.EmployerImages)
                {
                    if (employerEditDto.ImagesId != null)
                    {
                        if (!employerEditDto.ImagesId.Contains(item.Id) && item.Id != 0)
                        {
                            if (item.IsPoster == false)
                            {
                                string rootPath = _env.WebRootPath;
                                var fileName = item.Image;
                                var path = Path.Combine(rootPath, "images/employerImage", fileName);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                _context.EmployerImages.Remove(item);
                            }
                        }
     
                    }
                    else
                    {
                        if (employerEditDto.Images == null)
                        {
                            if (item.IsPoster == false)
                            {
                                string rootPath = _env.WebRootPath;
                                var fileName = item.Image;
                                var path = Path.Combine(rootPath, "images/employerImage", fileName);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                _context.EmployerImages.Remove(item);
                            }
                        }
        
                    }
                }
            }
            existEmployer.Name = employerEditDto.Name;
            existEmployer.FoundedDate = employerEditDto.FoundedDate;
            existEmployer.IsFeatured = false;
            existEmployer.CreatedAt = DateTime.UtcNow.AddHours(4);
            existEmployer.PhoneNumber = employerEditDto.PhoneNumber;
            existEmployer.Email = employerEditDto.Email;
            existEmployer.FacebookUrl = employerEditDto.FacebookUrl;
            existEmployer.InstagramUrl = employerEditDto.InstagramUrl;
            existEmployer.LinkedinUrl = employerEditDto.LinkedinUrl;
            existEmployer.TwitterUrl = employerEditDto.TwitterUrl;
            existEmployer.CompanyContent = employerEditDto.CompanyContent;
            existEmployer.Website = employerEditDto.Website;
            existEmployer.CityId = employerEditDto.CityId;
            existEmployer.CategoryId = employerEditDto.CategoryId;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Employer")]

        public IActionResult GetJobs(string search,int id,int page=1)
        {
            ViewBag.Search = search;

            if (search!=null)
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.Jobs.Where(x => x.EmployerId == id && x.Name.ToUpper().Contains(search.ToUpper()) && x.IsDeleted==false && x.ApplyStatus==Enums.ApplyStatus.Accepted).Count() / 10m);
            }
            else
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.Jobs.Where(x => x.EmployerId == id && x.IsDeleted == false && x.ApplyStatus == Enums.ApplyStatus.Accepted).Count() / 10m);
            }
            ViewBag.SelectedPage = page;
            Employer employer = _context.Employers.Include(x=>x.City).Include(x=>x.EmployerImages).Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            if (employer == null) return RedirectToAction("index");
            List<Job> jobs;
            if (search!=null)
            {
                jobs = _context.Jobs.Include(x => x.JobImages).Include(x => x.JobCategory).Include(x => x.Employer).ThenInclude(x => x.Category).Include(x => x.City).Where(x => x.EmployerId == id && x.Name.ToUpper().Contains(search.ToUpper()) && !x.IsDeleted && x.ApplyStatus==Enums.ApplyStatus.Accepted).Skip((page - 1) * 10).Take(10).ToList();
            }
            else
            {
                jobs = _context.Jobs.Include(x => x.JobImages).Include(x => x.JobCategory).Include(x => x.Employer).ThenInclude(x => x.Category).Include(x => x.City).Where(x => x.EmployerId == id && !x.IsDeleted && x.ApplyStatus == Enums.ApplyStatus.Accepted).Skip((page - 1) * 10).Take(10).ToList();
            }
            if (jobs == null) return RedirectToAction("index");
            JobListAndEmployerViewModel jobListAndEmployerViewModel = new JobListAndEmployerViewModel
            {
                Jobs = jobs,
                Employer = employer,
            };
            return View(jobListAndEmployerViewModel);
        }
        [Authorize(Roles = "Employer")]

        public IActionResult GetFollowers(string search,int id,int page=1)
        {
            ViewBag.Search = search;
            if (search!=null)
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.Followers.Include(x=>x.Employer).Include(x=>x.Candidate).Where(x => x.EmployerId == id && x.Candidate.FullName.ToUpper().Contains(search.ToUpper()) && x.Candidate.IsDeleted==false && x.Employer.IsDeleted==false).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.Followers.Include(x => x.Employer).Include(x => x.Candidate).Where(x => x.EmployerId == id && x.Candidate.IsDeleted == false && x.Employer.IsDeleted == false).Count() / 6m);
            }
            ViewBag.SelectedPage = page;
            Employer employer = _context.Employers.Include(x=>x.City).Include(x=>x.EmployerImages).Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            if (employer == null) return RedirectToAction("index");
            List<Follower> followers;
            if (search!=null)
            {
                followers = _context.Followers.Include(x => x.Candidate).ThenInclude(x => x.CandidateImages).Include(x => x.Employer).Where(x => x.EmployerId == id && x.Candidate.FullName.ToUpper().Contains(search.ToUpper()) && x.Candidate.IsDeleted==false && x.Employer.IsDeleted==false).Skip((page - 1) * 6).Take(6).ToList();
            }
            else
            {
                followers = _context.Followers.Include(x => x.Candidate).ThenInclude(x => x.CandidateImages).Include(x => x.Employer).Where(x => x.EmployerId == id && x.Candidate.IsDeleted == false && x.Employer.IsDeleted == false).Skip((page - 1) * 6).Take(6).ToList();
            }
            if (followers==null)
            {
                return RedirectToAction("index");
            }
            FollowerListAndEmployerViewModel followerListAndEmployerViewModel = new FollowerListAndEmployerViewModel
            {
                Followers = followers,
                Employer = employer,
            };
            return View(followerListAndEmployerViewModel);
        }
        [Authorize(Roles = "Employer")]

        public IActionResult SubmitJob(int newId)
        {
            ViewBag.Categories = _context.JobCategories.ToList();
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Languages = _context.Languages.ToList();
            Employer employer = _context.Employers.Include(x=>x.EmployerImages).Include(x=>x.AppUser).Include(x=>x.City).FirstOrDefault(x => x.Id == newId && x.IsDeleted==false);
            if (employer == null) return RedirectToAction("index");
            ViewBag.Employer = employer;

            return View();
        }
        [Authorize(Roles = "Employer")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitJob(int newId,Job createJob)
        {
            ViewBag.Categories = _context.JobCategories.ToList();
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Languages = _context.Languages.ToList();
            Employer employer = _context.Employers.Include(x=>x.EmployerImages).Include(x=>x.AppUser).Include(x=>x.City).FirstOrDefault(x => x.Id == newId && x.IsDeleted==false);
            if (employer == null) return RedirectToAction("index");
            ViewBag.Employer = employer;
            if (!ModelState.IsValid)
            {
                if (createJob.CityId == 0)
                {
                    ModelState.AddModelError("CityId", "Seher secilmelidir");
                }
                if (createJob.JobCategoryId == 0)
                {
                    ModelState.AddModelError("JobCategoryId", "Kateqoriya secilmelidir");

                }
                if (createJob.RequiredLanguageIds == null)
                {
                    ModelState.AddModelError("RequiredLanguageIds", "Language secilmelidir");
                }
                return View(createJob);
            }
            if (createJob.CityId == 0)
            {
                ModelState.AddModelError("CityId", "Seher secilmelidir");
                return View();
            }
            if (createJob.JobCategoryId == 0)
            {
                ModelState.AddModelError("JobCategoryId", "Kateqoriya secilmelidir");

                return View();
            }
            if (createJob.RequiredLanguageIds.Count() == 0)
            {
                ModelState.AddModelError("RequiredLanguageIds", "Language secilmelidir");
                return View();
            }
            createJob.EmployerId = newId;
            _context.Jobs.Add(createJob);
            _context.SaveChanges();
            if (createJob.File != null)
            {
                if (createJob.File.ContentType != "image/png" && createJob.File.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("File", "Jpeg ve ya png formatinda file daxil edilmelidir");
                    return View();
                }
                if (createJob.File.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("File", "File olcusu 5mb-dan cox olmaz!");
                    return View();
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + createJob.File.FileName;
                var path = Path.Combine(rootPath, "images/jobImage", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    createJob.File.CopyTo(stream);
                }
                JobImage jobImage = new JobImage
                {
                    Image = fileName,
                    IsPoster = true,
                    JobId = createJob.Id,
                };
                _context.JobImages.Add(jobImage);
            }
            if (createJob.Images != null)
            {
                foreach (var item in createJob.Images)
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
                        Image=fileName,
                        IsPoster=false,
                        JobId=createJob.Id,

                    };
                    _context.JobImages.Add(jobImage);
                }
            }
            if (createJob.RequiredLanguageIds != null)
            {
                foreach (var item in createJob.RequiredLanguageIds)
                {
                    if (_context.Languages.FirstOrDefault(x => x.Id == item) != null)
                    {
                        JobRequiredLanguage jobRequiredLanguage = new JobRequiredLanguage
                        {
                            Language = _context.Languages.FirstOrDefault(x => x.Id == item).Name,
                            JobId=createJob.Id
                        };
                        _context.JobRequiredLanguages.Add(jobRequiredLanguage);
                    }
                }
            }
            createJob.CreatedAt = DateTime.UtcNow.AddHours(4);
            createJob.IsFeatured = false;
            createJob.IsDeleted = false;
            createJob.ApplyStatus = Enums.ApplyStatus.Pending;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        //public IActionResult EditJob(int jobId)
        //{
        //    ViewBag.Categories = _context.JobCategories.ToList();
        //    ViewBag.Cities = _context.Cities.ToList();
        //    ViewBag.Languages = _context.Languages.ToList();
        //    Job job = _context.Jobs.Include(x=>x.JobImages).Include(x=>x.JobCategory).Include(x=>x.City).Include(x=>x.Employer).Include(x=>x.RequiredLanguages).FirstOrDefault(x => x.Id == jobId);
        //    if (job == null) return RedirectToAction("index");

        //    return View(job);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult EditJob(int jobId,Job editJob)
        //{
        //    ViewBag.Categories = _context.JobCategories.ToList();
        //    ViewBag.Cities = _context.Cities.ToList();
        //    ViewBag.Languages = _context.Languages.ToList();
        //    Job existJob = _context.Jobs.Include(x => x.JobImages).Include(x => x.JobCategory).Include(x => x.City).Include(x => x.Employer).Include(x => x.RequiredLanguages).FirstOrDefault(x => x.Id == jobId);
        //    if (existJob == null) return RedirectToAction("index");
        //    if (editJob.File != null)
        //    {
        //        if (editJob.File.ContentType != "image/png" && editJob.File.ContentType != "image/jpeg")
        //        {
        //            ModelState.AddModelError("File", "Jpeg ve ya png formatinda file daxil edilmelidir");
        //            return View();
        //        }
        //        if (editJob.File.Length > (1024 * 1024) * 5)
        //        {
        //            ModelState.AddModelError("File", "File olcusu 5mb-dan cox olmaz!");
        //            return View();
        //        }
        //        string rootPath = _env.WebRootPath;
        //        var fileName = Guid.NewGuid().ToString() + editJob.File.FileName;
        //        var path = Path.Combine(rootPath, "images/jobImage", fileName);
        //        using (FileStream stream = new FileStream(path, FileMode.Create))
        //        {
        //            editJob.File.CopyTo(stream);
        //        }
        //        if(existJob.JobImages.FirstOrDefault(x => x.IsPoster) != null)
        //        {
        //            if (existJob.JobImages.FirstOrDefault(x => x.IsPoster).Image!=null)
        //            {
        //                existJob.JobImages.FirstOrDefault(x => x.IsPoster).Image = fileName;

        //            }
        //        }
        //        else
        //        {
        //            JobImage jobImage = new JobImage
        //            {
        //                Image = fileName,
        //                IsPoster = true,
        //                JobId = jobId,

        //            };
        //            _context.JobImages.Add(jobImage);
        //        }
        //    }
        //    if (editJob.Images != null)
        //    {
        //        foreach (var item in editJob.Images)
        //        {
        //            if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
        //            {
        //                ModelState.AddModelError("File", "Jpeg ve ya png formatinda file daxil edilmelidir");
        //                return View();
        //            }
        //            if (item.Length > (1024 * 1024) * 5)
        //            {
        //                ModelState.AddModelError("File", "File olcusu 5mb-dan cox olmaz!");
        //                return View();
        //            }
        //            string rootPath = _env.WebRootPath;
        //            var fileName = Guid.NewGuid().ToString() + item.FileName;
        //            var path = Path.Combine(rootPath, "images/jobImage", fileName);
        //            using (FileStream stream = new FileStream(path, FileMode.Create))
        //            {
        //                item.CopyTo(stream);
        //            }
        //            JobImage jobImage = new JobImage
        //            {
        //                Image = fileName,
        //                IsPoster = false,
        //                JobId =jobId,

        //            };
        //            _context.JobImages.Add(jobImage);
        //        }
        //    }
        //    if (existJob.JobImages != null)
        //    {
        //        foreach (var item in existJob.JobImages)
        //        {
        //            if (editJob.ImageIds != null)
        //            {
        //                if (!editJob.ImageIds.Contains(item.Id) && item.Id != 0)
        //                {
        //                    //if (item.IsPoster == false)
        //                    //{
        //                        string rootPath = _env.WebRootPath;
        //                        var fileName = item.Image;
        //                        var path = Path.Combine(rootPath, "images/jobImage", fileName);
        //                        if (System.IO.File.Exists(path))
        //                        {
        //                            System.IO.File.Delete(path);
        //                        }
        //                        _context.JobImages.Remove(item);
        //                    //}
        //                }
        //            }
        //            else
        //            {
        //                if (editJob.Images == null)
        //                {
        //                    string rootPath = _env.WebRootPath;
        //                    var fileName = item.Image;
        //                    var path = Path.Combine(rootPath, "images/jobImage", fileName);
        //                    if (System.IO.File.Exists(path))
        //                    {
        //                        System.IO.File.Delete(path);
        //                    }
        //                    _context.JobImages.Remove(item);
        //                }

        //            }
        //        }
        //    }

        //    if (existJob.RequiredLanguages != null)
        //    {
        //        foreach (var item in existJob.RequiredLanguages)
        //        {
        //            _context.JobRequiredLanguages.Remove(item);
        //        }
        //    }
        //    foreach (var item in editJob.RequiredLanguageIds)
        //    {
        //        if (_context.Languages.FirstOrDefault(x => x.Id == item) != null)
        //        {
        //            JobRequiredLanguage jobRequiredLanguage = new JobRequiredLanguage
        //            {
        //                Language = _context.Languages.FirstOrDefault(x => x.Id == item).Name,
        //                JobId = jobId
        //            };
        //            _context.JobRequiredLanguages.Add(jobRequiredLanguage);
        //        }
        //    }
        //    existJob.CreatedAt = DateTime.UtcNow.AddHours(4);
        //    _context.SaveChanges();
        //    return RedirectToAction("index");
        //}
        [Authorize(Roles = "Employer")]

        public IActionResult DeleteJob(int jobId)
        {
            Job job = _context.Jobs.FirstOrDefault(x => x.Id == jobId && x.IsDeleted==false && x.ApplyStatus==Enums.ApplyStatus.Accepted);
            if (job == null) return RedirectToAction("index");
            job.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Employer")]

        public IActionResult RejectJob(int jobId)
        {
            Job job = _context.Jobs.FirstOrDefault(x => x.Id == jobId && x.IsDeleted==false && x.ApplyStatus==Enums.ApplyStatus.Accepted);
            if (job == null) return RedirectToAction("index");
            //job.IsDeleted = true;
            job.ApplyStatus = Enums.ApplyStatus.UserReject;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewContact(EmployerContact employerContact)
        {
            if (employerContact == null || employerContact.EmployerId==0 || !ModelState.IsValid) return RedirectToAction("detail");
            if (!_context.Employers.Any(x=>x.Id==employerContact.EmployerId && x.IsDeleted==false))
            {
                return RedirectToAction("detail");
            }
            _context.EmployerContacts.Add(employerContact);
            _context.SaveChanges();
            return RedirectToAction("detail");
        }
        [Authorize(Roles = "Employer")]

        public IActionResult GetContacts(string search,int page=1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.Search = search;
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null) return RedirectToAction("index");
            Employer employer = _context.Employers.Include(x=>x.AppUser).Include(x=>x.EmployerImages).Include(x=>x.City).FirstOrDefault(x => x.AppUser.UserName == user.UserName && x.IsDeleted==false);
            if(employer==null) return RedirectToAction("index");
            List<EmployerContact> contacts;
            if (search != null)
            {
                contacts = _context.EmployerContacts.Include(x=>x.Employer).Where(x => x.EmployerId == employer.Id && x.Email.ToUpper().Contains(search.ToUpper()) && x.Employer.IsDeleted==false).Skip((page - 1) * 5).Take(5).ToList();
            }
            else
            {
                contacts = _context.EmployerContacts.Include(x=>x.Employer).Where(x => x.EmployerId == employer.Id && x.Employer.IsDeleted == false).Skip((page - 1) * 5).Take(5).ToList();
            }
            if (contacts==null) return RedirectToAction("index");
            ContactsAndEmployerViewModel employerViewModel = new ContactsAndEmployerViewModel
            {
                Employer = employer,
                EmployerContacts = contacts,
            };
            if (search != null)
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.EmployerContacts.Where(x => x.EmployerId == employer.Id && x.Email.ToUpper().Contains(search.ToUpper())).Count() / 5m);
            }
            else
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.EmployerContacts.Where(x => x.EmployerId == employer.Id).Count() / 5m);
            }
            return View(employerViewModel);
        }
        [Authorize(Roles = "Employer")]

        public IActionResult GetJobContacts(string search,int id,int page=1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.Search = search;
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null) return RedirectToAction("index");
            Employer employer = _context.Employers.Include(x => x.AppUser).Include(x=>x.EmployerImages).Include(x=>x.City).FirstOrDefault(x => x.AppUser.UserName == user.UserName && x.IsDeleted==false);
            if (employer == null) return RedirectToAction("index");
            Job job = _context.Jobs.FirstOrDefault(x => x.Id == id && x.IsDeleted==false && x.ApplyStatus==Enums.ApplyStatus.Accepted);
            if(job==null) return RedirectToAction("index");
            List<JobContact> contacts;
            if (search!=null)
            {
                contacts = _context.JobContacts.Include(x=>x.Job).Where(x => x.JobId == job.Id && x.Email.ToUpper().Contains(search.ToUpper()) && x.Job.ApplyStatus==Enums.ApplyStatus.Accepted && x.Job.IsDeleted==false).Skip((page - 1) * 5).Take(5).ToList();
            }
            else
            {
                contacts = _context.JobContacts.Include(x=>x.Job).Where(x => x.JobId == job.Id && x.Job.ApplyStatus == Enums.ApplyStatus.Accepted && x.Job.IsDeleted == false).Skip((page - 1) * 5).Take(5).ToList();
            }
            if (contacts==null) return RedirectToAction("index");
            if (search!=null)
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.JobContacts.Where(x => x.JobId == job.Id && x.Email.ToUpper().Contains(search.ToUpper()) && x.Job.ApplyStatus == Enums.ApplyStatus.Accepted && x.Job.IsDeleted == false).Count() / 5m);

            }
            else
            {
                ViewBag.TotalPageCount = Math.Ceiling(_context.JobContacts.Where(x => x.JobId == job.Id && x.Job.ApplyStatus == Enums.ApplyStatus.Accepted && x.Job.IsDeleted == false).Count() / 5m);
            }
            JobContactsAndEmployerViewModel jobContactsAndEmployer = new JobContactsAndEmployerViewModel
            {
                JobContacts = contacts,
                Employer = employer,
            };
            return View(jobContactsAndEmployer);
        }
        [Authorize(Roles = "Employer")]

        public IActionResult GetJobApplies(string search,int id)
        {
            ViewBag.Search = search;
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null) return RedirectToAction("index");
            Employer employer = _context.Employers.Include(x=>x.AppUser).Include(x=>x.EmployerImages).Include(x=>x.City).FirstOrDefault(x => x.AppUser.Id == user.Id && x.IsDeleted==false);
            if (employer == null) return RedirectToAction("index");
            Job job = _context.Jobs.Include(x=>x.Employer).FirstOrDefault(x => x.Id == id && x.Employer.Id==employer.Id && x.ApplyStatus == Enums.ApplyStatus.Accepted && x.IsDeleted == false);
            if (job == null) return RedirectToAction("index");
            List<Apply> applies;
            if (search!=null)
            {
                applies = _context.Applies.Include(x => x.Job).ThenInclude(x=>x.JobImages).Include(x => x.AppUser).ThenInclude(x => x.Candidate).ThenInclude(x=>x.Position).Include(x => x.AppUser).ThenInclude(x => x.Candidate).ThenInclude(x=>x.CandidateImages).Include(x => x.AppUser).ThenInclude(x => x.Candidate).ThenInclude(x => x.City).Where(x => x.JobId == job.Id && x.AppUser.Candidate.FullName.ToUpper().Contains(search.ToUpper()) && x.Job.ApplyStatus == Enums.ApplyStatus.Accepted && x.Job.IsDeleted == false).ToList();

            }
            else
            {
                applies = _context.Applies.Include(x => x.Job).ThenInclude(x => x.JobImages).Include(x => x.AppUser).ThenInclude(x => x.Candidate).ThenInclude(x => x.Position).Include(x => x.AppUser).ThenInclude(x => x.Candidate).ThenInclude(x => x.CandidateImages).Include(x => x.AppUser).ThenInclude(x => x.Candidate).ThenInclude(x => x.City).Where(x => x.JobId == job.Id && x.Job.ApplyStatus == Enums.ApplyStatus.Accepted && x.Job.IsDeleted == false).ToList();
            }
            if (applies == null) return RedirectToAction("index");
            ApplyListAndEmployer applyListAndEmployer = new ApplyListAndEmployer
            {
                Applies = applies,
                Employer = employer,
            };
            return View(applyListAndEmployer);
        }
        [Authorize(Roles = "Employer")]

        public IActionResult EmployerApplies()
        {
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null) return RedirectToAction("index");
            Employer employer = _context.Employers.Include(x=>x.AppUser).Include(x=>x.EmployerImages).Include(x=>x.City).FirstOrDefault(x => x.AppUserId == user.Id && x.IsDeleted == false) ;
            if(employer==null) return RedirectToAction("index");
            List<Job> jobs = _context.Jobs.Include(x=>x.JobImages).Include(x=>x.JobCategory).Include(x=>x.City).Where(x => x.EmployerId == employer.Id && x.IsDeleted==false && x.ApplyStatus!=Enums.ApplyStatus.Accepted && x.ApplyStatus != Enums.ApplyStatus.Block ).ToList();
            if (jobs == null) return RedirectToAction("index");
            JobListAndEmployerViewModel jobListAndEmployerViewModel = new JobListAndEmployerViewModel
            {
                Jobs = jobs,
                Employer = employer
            };
            return View(jobListAndEmployerViewModel);
        }
        public List<Apply> GetApplies(int id)
        {
            Employer employer = _context.Employers.Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id);
            if (employer == null) return null;
            return _context.Applies.Include(x=>x.Job).Include(x => x.AppUser).ThenInclude(x => x.Employer).Where(x => x.RequestDate.Year <= DateTime.UtcNow.Year && x.RequestDate.Year >= DateTime.UtcNow.Year - 10 && x.AppUserId == employer.AppUser.Id).ToList();
        }
    }
}
