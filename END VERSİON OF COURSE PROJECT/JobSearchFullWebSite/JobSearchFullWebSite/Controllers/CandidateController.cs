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
using System.Net;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Controllers
{
    public class CandidateController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public CandidateController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(string search, int? cityId,int? positionId, int page = 1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.Search = search;
            ViewBag.CityId = cityId;
            ViewBag.PositionId = positionId;
            List<Candidate> candidates = _context.Candidates.Include(x=>x.City).Include(x=>x.Position).Where(x => x.IsDeleted == false).ToList();
            if (search != null)
            {
                candidates = candidates.Where(x => x.FullName.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (cityId != null)
            {
                candidates = candidates.Where(x => x.City.Id==cityId).ToList();
            }
            if (positionId != null)
            {
                candidates = candidates.Where(x => x.Position.Id == positionId).ToList();
            }
            ViewBag.TotalPageCount = Math.Ceiling(candidates.Count() / 9m);
            CandidateIndexViewModel candidateVM = new CandidateIndexViewModel
            {
                Candidates = _context.Candidates.Include(x => x.CandidateImages).Include(x=>x.CandidateSkills).Include(X => X.City).Include(X => X.Position).Include(x=>x.AppUser).Where(x=>x.IsDeleted==false).ToList(),
                Cities = _context.Cities.ToList(),
                Positions = _context.Positions.ToList(),

            };
            if (search != null)
            {
                candidateVM.Candidates = candidateVM.Candidates.Where(x => x.FullName.ToUpper().Contains(search.ToUpper())).ToList();
            }

    
            if (cityId != 0)
            {
                City city = _context.Cities.FirstOrDefault(x => x.Id == cityId);
                if (city != null)
                {
                    candidateVM.Candidates = candidateVM.Candidates.Where(x => x.City.CityName == city.CityName).ToList();

                }
            }

            if (positionId != 0)
            {
                Position position = _context.Positions.FirstOrDefault(x => x.Id == positionId);
                if (position!=null)
                {
                    candidateVM.Candidates = candidateVM.Candidates.Where(x => x.Position.PositionName == position.PositionName).ToList();

                }
            }
            candidateVM.Candidates = candidateVM.Candidates.Skip((page - 1) * 9).Take(9).ToList();
            return View(candidateVM);
        }
        public IActionResult Detail(int id)
        {
            if (!_context.Candidates.Any(x => x.Id == id && x.IsDeleted==false))
            {
                return RedirectToAction("index", "home");
            }
            Candidate candidate = _context.Candidates.Include(x=>x.Position).Include(x => x.City).Include(x => x.CandidateSkills).Include(x => x.CandidateImages).Include(x => x.KnowingLanguages).ThenInclude(x=>x.Language).Include(x => x.CandidateCVs).ThenInclude(x=>x.Candidate).ThenInclude(x=>x.AppUser).Include(x => x.CandidateAwardItems).Include(x => x.CandidateEducationItems).Include(x => x.CandidateWorkItems).FirstOrDefault(x=>x.Id==id && x.IsDeleted==false);
            return View(candidate);
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult CandidateDashboard(int id)
        {
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Candidate candidate = _context.Candidates.Include(x=>x.AppUser).ThenInclude(x=>x.Applies).Include(x=>x.ShortLists).Include(x=>x.Followers).Include(x=>x.CandidateContacts).Include(x=>x.CandidateImages).Include(x=>x.City).FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (candidate == null) return RedirectToAction("index");
            if (candidate.AppUser.UserName != user.UserName) return RedirectToAction("index");
            return View(candidate);
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult CandidateApplieds(string search,int id)
        {

                ViewBag.Search = search;
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Candidate candidate = _context.Candidates.Include(x=>x.AppUser).ThenInclude(x=>x.Applies).ThenInclude(x=>x.Job).ThenInclude(x=>x.JobCategory).Include(x => x.AppUser).ThenInclude(x => x.Applies).ThenInclude(x => x.Job).ThenInclude(x=>x.JobImages).Include(x => x.AppUser).ThenInclude(x => x.Applies).ThenInclude(x => x.Job).ThenInclude(x=>x.City).Include(x=>x.City).Include(x=>x.CandidateImages).FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (candidate.AppUser.UserName != user.UserName) return RedirectToAction("index");
            return View(candidate);
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult CandidateShortList(string search,int id)
        {
                ViewBag.Search = search;
            Candidate candidate = _context.Candidates.Include(x=>x.AppUser).Include(x=>x.CandidateImages).Include(x=>x.ShortLists).ThenInclude(x=>x.Job).ThenInclude(x=>x.JobImages).Include(x => x.ShortLists).ThenInclude(x => x.Job).ThenInclude(x => x.JobCategory).Include(x => x.ShortLists).ThenInclude(x => x.Job).ThenInclude(x => x.City).Include(x=>x.City).FirstOrDefault(x => x.Id==id && x.IsDeleted==false);
            if (candidate == null) return RedirectToAction("index");
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null) return RedirectToAction("index");
            if (candidate.AppUser.UserName != user.UserName) return RedirectToAction("index");
            return View(candidate);
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult CandidateFollowings(string search,int id)
        {
            ViewBag.Search = search;
            ViewBag.CandidateId = id;
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Candidate candidate = _context.Candidates.FirstOrDefault(x =>x.Id == id && x.IsDeleted==false);
            if (candidate.AppUser.UserName != user.UserName) return RedirectToAction("index");
            List<Follower> followers;
            if (search!=null)
            {
                followers = _context.Followers.Include(x => x.Employer).ThenInclude(x => x.City).Include(x => x.Employer).ThenInclude(x => x.EmployerImages).Include(x => x.Employer).ThenInclude(x => x.Category).Include(x => x.Employer).ThenInclude(x => x.Jobs).Include(x => x.Candidate).ThenInclude(x => x.CandidateImages).Include(x => x.Candidate).ThenInclude(x => x.City).Include(x => x.Candidate).ThenInclude(x => x.AppUser).Where(x => x.CandidateId == candidate.Id && x.Employer.Name.ToUpper().Contains(search.ToUpper()) && x.Employer.IsDeleted==false).ToList();
            }
            else
            {
                followers = _context.Followers.Include(x => x.Employer).ThenInclude(x => x.City).Include(x => x.Employer).ThenInclude(x => x.EmployerImages).Include(x => x.Employer).ThenInclude(x => x.Category).Include(x => x.Employer).ThenInclude(x => x.Jobs).Include(x => x.Candidate).ThenInclude(x => x.CandidateImages).Include(x => x.Candidate).ThenInclude(x => x.City).Include(x => x.Candidate).ThenInclude(x => x.AppUser).Where(x => x.CandidateId == candidate.Id && x.Employer.IsDeleted == false).ToList();
            }
            return View(followers);
        }
        //public IActionResult CandidateAlerts()
        //{
        //    return View();
        //}
        [Authorize(Roles = "Candidate")]

        public IActionResult CandidateProfileEdit(int id)
        {

            ViewBag.Languages =_context.Languages.ToList(); 
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Cities = _context.Cities.ToList();
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Candidate candidate = _context.Candidates.Include(x=>x.AppUser).Include(x=>x.CandidateImages).Include(x => x.KnowingLanguages).Include(x=>x.City).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            if (candidate.AppUser.UserName != user.UserName) return RedirectToAction("index");
            
            if (candidate == null)
            {
                return RedirectToAction("index");
            }
            if(user==null)   return RedirectToAction("index");
            if (candidate.AppUser.UserName != user.UserName) return RedirectToAction("index");
            CandidateEditDto candidateEditDto = new CandidateEditDto();

            candidateEditDto.Id = candidate.Id;
            candidateEditDto.FullName = candidate.FullName;
            candidateEditDto.WaitingSalary = candidate.WaitingSalary;
            candidateEditDto.SalaryForTime = candidate.SalaryForTime;
            candidateEditDto.BirthdayDate = candidate.BirthdayDate;
            candidateEditDto.AboutCandidateTextEditor = candidate.AboutCandidateTextEditor;
            candidateEditDto.Experience = candidate.Experience;
            candidateEditDto.Gender = candidate.Gender;
            candidateEditDto.Age = candidate.Age;
            candidateEditDto.Qualification = candidate.Qualification;
            candidateEditDto.Email = candidate.Email;
            candidateEditDto.PhoneNumber = candidate.PhoneNumber ;
            candidateEditDto.FacebookUrl = candidate.FacebookUrl;
            candidateEditDto.TwitterUrl = candidate.TwitterUrl ;
            candidateEditDto.LinkedinUrl = candidate.LinkedinUrl;
            candidateEditDto.InstagramUrl = candidate.InstagramUrl;
         

            if (candidate.Position != null)
            {
                candidateEditDto.PositionId = candidate.Position.Id;
                candidateEditDto.Position = candidate.Position;
            }
            else
            {
                candidateEditDto.PositionId = 0;
            }
            if (candidate.City != null)
            {
                candidateEditDto.CityId = candidate.City.Id;
                candidateEditDto.City = candidate.City;
            }
            else
            {
                candidateEditDto.CityId = 0;
            }
            candidateEditDto.KnowingLanguages = candidate.KnowingLanguages;
            if(candidate.CandidateImages.Count != 0){
                if (candidate.CandidateImages.FirstOrDefault(x => x.IsPoster)!=null)
                {
                    if (candidate.CandidateImages.FirstOrDefault(x => x.IsPoster).Image!=null)
                    {
                        candidateEditDto.Image = candidate.CandidateImages.FirstOrDefault(x => x.IsPoster).Image;

                    }
                    else
                    {
                        candidateEditDto.Image = null;
                    }
                }
                else
                {
                    candidateEditDto.Image = null;

                }
            }
            else
            {
                candidateEditDto.Image = null;
            }

            return View(candidateEditDto);
        }
        [Authorize(Roles = "Candidate")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CandidateProfileEdit(int id,CandidateEditDto candidateEditDto)
        {
            ViewBag.Languages = _context.Languages.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Cities = _context.Cities.ToList();
            Candidate existCandidate = _context.Candidates.Include(x=>x.AppUser).Include(x=>x.CandidateImages).Include(x => x.KnowingLanguages).Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);

            if (existCandidate == null) return RedirectToAction("index");
            if (candidateEditDto.CityId != 0)
            {
                if (!_context.Cities.Any(x => x.Id == candidateEditDto.CityId)) return RedirectToAction("index");
            }
            if (candidateEditDto.PositionId != 0)
            {
                if (!_context.Positions.Any(x => x.Id == candidateEditDto.PositionId)) return RedirectToAction("index");
            }
            if (candidateEditDto.AboutCandidateTextEditor == null)
            {
                ModelState.AddModelError("", "Context daxil edilmelidir");
                return RedirectToAction("CandidateProfileEdit");
            }
            if (candidateEditDto.AboutCandidateTextEditor.Length > 1000)
            {
                ModelState.AddModelError("AboutCandidateTextEditor", "Maksimum uzunluq 1000-dir");
                return RedirectToAction("CandidateProfileEdit");
            }
            if (!ModelState.IsValid)
            {
                RedirectToAction("CandidateProfileEdit");
            }
            if (candidateEditDto.Image == null && candidateEditDto.File==null)
            {
                CandidateImage candidate = _context.CandidateImages.FirstOrDefault(x => x.IsPoster && x.CandidateId == existCandidate.Id);
                _context.CandidateImages.Remove(candidate);
                string existPath = Path.Combine(_env.WebRootPath, "images/candidateImage", existCandidate.CandidateImages.FirstOrDefault(x => x.IsPoster).Image);
                if (System.IO.File.Exists(existPath))
                {
                    System.IO.File.Delete(existPath);
                }
            }

            if (candidateEditDto.File != null)
            {
              
              
                if (candidateEditDto.File.ContentType != "image/png" && candidateEditDto.File.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("Image", "Jpeg ve ya png formatinda file daxil edilmelidir");
                    RedirectToAction("CandidateProfileEdit");
                }
                if (candidateEditDto.File.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("Image", "File olcusu 5mb-dan cox olmaz!");
                    RedirectToAction("CandidateProfileEdit");
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + candidateEditDto.File.FileName;
                var path = Path.Combine(rootPath, "images/candidateImage", fileName);

                if (existCandidate.CandidateImages.Count() != 0 && existCandidate.CandidateImages.FirstOrDefault(x => x.IsPoster) != null && existCandidate.CandidateImages.FirstOrDefault(x => x.IsPoster).Image != null)
                {
                            string existPath = Path.Combine(_env.WebRootPath, "images/candidateImage", existCandidate.CandidateImages.FirstOrDefault(x => x.IsPoster).Image);
                            if (System.IO.File.Exists(existPath))
                            {
                                System.IO.File.Delete(existPath);
                            }
                            existCandidate.CandidateImages.FirstOrDefault(x => x.IsPoster).Image = fileName;

                }
                else
                {
                    CandidateImage candidateImage = new CandidateImage
                    {
                        CandidateId = existCandidate.Id,
                        Image = fileName,
                        IsPoster = true,
                    };
                    _context.CandidateImages.Add(candidateImage);
                }



                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    candidateEditDto.File.CopyTo(stream);
                }

             
            }

            candidateEditDto.Id = id;

            
            existCandidate.FullName = candidateEditDto.FullName;
            existCandidate.IsFeatured = false;
            existCandidate.WaitingSalary = candidateEditDto.WaitingSalary;
            existCandidate.SalaryForTime = candidateEditDto.SalaryForTime;
            existCandidate.BirthdayDate = candidateEditDto.BirthdayDate;
            existCandidate.CreatedAt = DateTime.UtcNow.AddHours(4);
            existCandidate.AboutCandidateTextEditor = candidateEditDto.AboutCandidateTextEditor;
            existCandidate.Experience = candidateEditDto.Experience;
            existCandidate.Gender = candidateEditDto.Gender;
            existCandidate.Age = candidateEditDto.Age;
            existCandidate.Qualification = candidateEditDto.Qualification;
            existCandidate.Email = candidateEditDto.Email;
            existCandidate.PhoneNumber = candidateEditDto.PhoneNumber;
            existCandidate.FacebookUrl = candidateEditDto.FacebookUrl;
            existCandidate.TwitterUrl = candidateEditDto.TwitterUrl;
            existCandidate.LinkedinUrl = candidateEditDto.LinkedinUrl;
            existCandidate.InstagramUrl = candidateEditDto.InstagramUrl;
            existCandidate.InstagramUrl = candidateEditDto.InstagramUrl;
            existCandidate.CityId = candidateEditDto.CityId;
            existCandidate.PositionId = candidateEditDto.PositionId;
 
            var existLanguages = _context.CandidateKnowingLanguages.Where(x => x.CandidateId == id).ToList();
            if (candidateEditDto.KnowingLanguageIds!=null)
            {
                foreach (var item in candidateEditDto.KnowingLanguageIds)
                {
                    var existLanguage = existLanguages.FirstOrDefault(x => x.Id == item);
                    if (existLanguage == null)
                    {
                        CandidateKnowingLanguage knowingLanguage = new CandidateKnowingLanguage
                        {
                            CandidateId = id,
                            LanguageId = (int)item
                        };
                        _context.CandidateKnowingLanguages.Add(knowingLanguage);
                    }
                    else
                    {
                        existLanguages.Remove(existLanguage);
                    }   
                }
            }
            _context.CandidateKnowingLanguages.RemoveRange(existLanguages);

            _context.SaveChanges();
            //return View(candidateEditDto);
            return RedirectToAction("index");
        }
        [Authorize(Roles ="Candidate")]
        public IActionResult CandidateResumeEdit(int id) 
        {
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            Candidate candidate = _context.Candidates.Include(x=>x.AppUser).Include(x=>x.City).Include(x=>x.CandidateCVs).Include(x=>x.CandidateSkills).Include(x=>x.CandidateImages).Include(x=>x.CandidateWorkItems).Include(x=>x.CandidateEducationItems).Include(x=>x.CandidateAwardItems).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            if (candidate.AppUser.UserName != user.UserName) return RedirectToAction("index");

            CandidateResumeEditDto candidateResume = new CandidateResumeEditDto();
            if (candidate.CandidateImages != null)
            {
                candidateResume.CandidateImages = candidate.CandidateImages;
            }
            candidateResume.CandidateSkills = candidate.CandidateSkills;
            candidateResume.CandidateEducationItems = candidate.CandidateEducationItems;
            candidateResume.CandidateAwardItems = candidate.CandidateAwardItems;
            candidateResume.CandidateWorkItems = candidate.CandidateWorkItems;
            candidateResume.CandidateCVs = candidate.CandidateCVs; 
            candidateResume.Id = id;
            candidateResume.Candidate = candidate;
            return View(candidateResume); 
        }
        [Authorize(Roles = "Candidate")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CandidateResumeEdit(int id,CandidateResumeEditDto resumeEditDto) 
        {
            Candidate candidate = _context.Candidates.Include(x => x.AppUser).Include(x=>x.City).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (candidate.AppUser.UserName != user.UserName)
            {
                return RedirectToAction("index");
            }
            Candidate existCandidate = _context.Candidates.Include(x=>x.CandidateSkills).Include(x=>x.CandidateCVs).Include(x=>x.CandidateImages).Include(x=>x.CandidateEducationItems).Include(x=>x.CandidateWorkItems).Include(x=>x.CandidateAwardItems).FirstOrDefault(x => x.Id == id);
            if (existCandidate == null) return RedirectToAction("index");
            //burdaki o demekdiki acsam bunu bir defe elave edilibse cv bir ededi qalibsa onu silmek olmaz
            //if (resumeEditDto.CandidateCVsId == null && resumeEditDto.CandidateCV == null)
            //{
            //    ModelState.AddModelError("", "CV daxil edilmesi mutleqdir");
            //    return View(resumeEditDto);
            //}
            if (resumeEditDto.CandidateSkills != null)
            {
                foreach (var item in resumeEditDto.CandidateSkills)
                {
                    if (item.Name == null)
                    {
                        ModelState.AddModelError("", "Skill Nmae-ler mutleq daxil edilmelidir");
                        return View(resumeEditDto);
                    }
                    if (item.Name.Length > 30)
                    {
                        ModelState.AddModelError("", "SkillName-lerin max uzunlugu 30-dur");
                        return View(resumeEditDto);
                    }
      
                }
            }
            if (resumeEditDto.CandidateEducationItems != null)
            {
                foreach (var item in resumeEditDto.CandidateEducationItems)
                {
                    if (item.Title == null)
                    {
                        ModelState.AddModelError("", "Title-larin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Years == null)
                    {
                        ModelState.AddModelError("", "Tehsil illerinin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.EducationPlace == null)
                    {
                        ModelState.AddModelError("", "Tehsil aldiginiz muessiselerin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Content == null)
                    {
                        ModelState.AddModelError("", "Umumi melumat daxil daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Title.Length > 50)
                    {
                        ModelState.AddModelError("", "Title-larin max uzunlugu 50-dir");
                        return View(resumeEditDto);
                    }
                    if (item.Years.Length>10)
                    {
                        ModelState.AddModelError("", "Years-larin max uzunlugu 10-dur");
                        return View(resumeEditDto);
                    }
                    if (item.EducationPlace.Length>50)
                    {
                        ModelState.AddModelError("", "EducationPlace-larin max uzunlugu 50-dur");
                        return View(resumeEditDto);
                    }
                    if (item.Content.Length > 200)
                    {
                        ModelState.AddModelError("", "Content-larin max uzunlugu 200-dur");
                        return View(resumeEditDto);
                    }
                   
                }
            }
            if (resumeEditDto.CandidateWorkItems != null)
            {
                foreach (var item in resumeEditDto.CandidateWorkItems)
                {
                    if (item.Title == null)
                    {
                        ModelState.AddModelError("", "Title-larin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.StartDate == null || item.EndDate == null)
                    {
                        ModelState.AddModelError("", "Tarixlerin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.WorkPlace == null)
                    {
                        ModelState.AddModelError("", "Is yerinin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Content == null)
                    {
                        ModelState.AddModelError("", "Umumi melumatin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Title.Length > 50)
                    {
                        ModelState.AddModelError("", "Title-larin max uzunlugu 50-dur");
                        return View(resumeEditDto);
                    }
                    if (item.WorkPlace.Length > 50)
                    {
                        ModelState.AddModelError("", "WorkPlace-larin max uzunlugu 50-dur");
                        return View(resumeEditDto);
                    }
                    if (item.Content.Length > 200)
                    {
                        ModelState.AddModelError("", "Content-larin max uzunlugu 200-dur");
                        return View(resumeEditDto);
                    }
                   
                }
            }
            if (resumeEditDto.CandidateAwardItems != null)
            {
                foreach (var item in resumeEditDto.CandidateAwardItems)
                {
                    if (item.Title == null)
                    {
                        ModelState.AddModelError("", "Title-nin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Years == null)
                    {
                        ModelState.AddModelError("", "Tarixlerin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Content == null)
                    {
                        ModelState.AddModelError("", "Umumi melumatin daxil edilmesi mutleqdir");
                        return View(resumeEditDto);
                    }
                    if (item.Title.Length > 50)
                    {
                        ModelState.AddModelError("", "Title-larin max uzunlugu 50-dur");
                        return View(resumeEditDto);
                    }
                    if (item.Years.Length > 50)
                    {
                        ModelState.AddModelError("", "Years-larin max uzunlugu 10-dur");
                        return View(resumeEditDto);
                    }
                    if (item.Content.Length > 200)
                    {
                        ModelState.AddModelError("", "Content-larin max uzunlugu 200-dur");
                        return View(resumeEditDto);
                    }

                }

            }
            if (resumeEditDto.CandidateSkills != null)
            {
                foreach (var item in resumeEditDto.CandidateSkills)
                {
                    if (item.Id != 0)
                    {
                        CandidateSkill candidateSkill = _context.CandidateSkills.FirstOrDefault(x => x.Id == item.Id);
                        candidateSkill.Name = item.Name;
                    }
                    else
                    {
                        CandidateSkill candidateSkill = new CandidateSkill
                        {
                            Name = item.Name,
                            CandidateId=existCandidate.Id
                        };
                        _context.CandidateSkills.Add(candidateSkill);
                    }
                }
            }
            if (resumeEditDto.CandidateEducationItems != null)
            {
                foreach (var item in resumeEditDto.CandidateEducationItems)
                {

                    if (item.Id != 0)
                    {
                        CandidateEducationItem educationItem = _context.CandidateEducationItems.Include(x=>x.Candidate).FirstOrDefault(x => x.Id == item.Id);
                        educationItem.Title = item.Title;
                        educationItem.Years = item.Years;
                        educationItem.Content = item.Content;
                        educationItem.EducationPlace = item.EducationPlace;
                    }
                    else
                    {
                        CandidateEducationItem educationItem = new CandidateEducationItem
                        {
                            Title = item.Title,
                            Years = item.Years,
                            Content = item.Content,
                            CandidateId = existCandidate.Id,
                            EducationPlace = item.EducationPlace,
                        };
                        _context.CandidateEducationItems.Add(educationItem);
                    }
  
                }
            }
            //if (existCandidate.CandidateEducationItems != null)
            //{
            //    foreach (var item in existCandidate.CandidateEducationItems)
            //    {
            //        if (item.Id != 0)
            //        {
            //            CandidateEducationItem educationItem = _context.CandidateEducationItems.FirstOrDefault(x => x.Id == item.Id);
            //            _context.CandidateEducationItems.Remove(educationItem);
            //        }

            //    }
            //}
            if (resumeEditDto.CandidateWorkItems != null)
            {
    
                foreach (var item in resumeEditDto.CandidateWorkItems)
                {
                    if (item.Id!=0)
                    {
                        CandidateWorkItem workItem = _context.CandidateWorkItems.Include(x => x.Candidate).FirstOrDefault(x => x.Id == item.Id);
                        workItem.Title = item.Title;
                        workItem.Content = item.Content;
                        workItem.StartDate = item.StartDate;
                        workItem.EndDate = item.EndDate;
                        workItem.WorkPlace = item.WorkPlace;
                    }
                    else
                    {
                        CandidateWorkItem workItem = new CandidateWorkItem
                        {
                            Title = item.Title,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            WorkPlace = item.WorkPlace,
                            Content = item.Content,
                            CandidateId = existCandidate.Id,
                        };
                        _context.CandidateWorkItems.Add(workItem);
                    }

                }
            }
            //if (existCandidate.CandidateWorkItems != null)
            //{
            //    foreach (var item in existCandidate.CandidateWorkItems)
            //    {
            //        if (item.Id != 0)
            //        {
            //            CandidateWorkItem workItem = _context.CandidateWorkItems.FirstOrDefault(x => x.Id == item.Id);
            //            _context.CandidateWorkItems.Remove(workItem);
            //        }
            //    }
            //}
            if (resumeEditDto.CandidateAwardItems != null)
            {
             
                foreach (var item in resumeEditDto.CandidateAwardItems)
                {
                    if (item.Id!=0)
                    {
                        CandidateAwardItem awardItem = _context.CandidateAwardItems.Include(x => x.Candidate).FirstOrDefault(x => x.Id == item.Id);
                        awardItem.Title = item.Title;
                        awardItem.Content = item.Content;
                        awardItem.Years = item.Years;
                    }
                    else
                    {
                        CandidateAwardItem awardItem = new CandidateAwardItem
                        {
                            Title = item.Title,
                            Years = item.Years,
                            Content = item.Content,
                            CandidateId = existCandidate.Id,
                        };
                        _context.CandidateAwardItems.Add(awardItem);
                    }
                }
            }
            //if (existCandidate.CandidateAwardItems != null)
            //{
            //    foreach (var item in existCandidate.CandidateAwardItems)
            //    {
            //        if (item.Id != 0)
            //        {
            //            CandidateAwardItem awardItem = _context.CandidateAwardItems.FirstOrDefault(x => x.Id == item.Id);
            //            _context.CandidateAwardItems.Remove(awardItem);
            //        }
            //    }
            //}
            if (resumeEditDto.CandidateSkillsId != null)
            {
                foreach (var item in existCandidate.CandidateSkills)
                {
                    if (!resumeEditDto.CandidateSkillsId.Contains(item.Id))
                    {
                        if (item.Id != 0)
                        {
                            _context.CandidateSkills.Remove(_context.CandidateSkills.FirstOrDefault(x => x.Id == item.Id));
                        }
                    }
                }
            }
            if (resumeEditDto.CandidateEducationItemsId != null)
            {
                foreach (var item in existCandidate.CandidateEducationItems)
                {
                    if (!resumeEditDto.CandidateEducationItemsId.Contains(item.Id))
                    {
                        if (item.Id != 0)
                        {
                            _context.CandidateEducationItems.Remove(_context.CandidateEducationItems.FirstOrDefault(x => x.Id == item.Id));
                        }
                    }
                }
            }
            if (resumeEditDto.CandidateWorkItemsId != null)
            {
                foreach (var item in existCandidate.CandidateWorkItems)
                {
                    if (!resumeEditDto.CandidateWorkItemsId.Contains(item.Id))
                    {
                        if (item.Id != 0)
                        {
                        _context.CandidateWorkItems.Remove(_context.CandidateWorkItems.FirstOrDefault(x => x.Id == item.Id));
                        }
                    }
                }
            }
            if (resumeEditDto.CandidateAwardItemsId != null)
            {
                foreach (var item in existCandidate.CandidateAwardItems)
                {
                    if (!resumeEditDto.CandidateAwardItemsId.Contains(item.Id))
                    {
                        if (item.Id != 0)
                        {
                            _context.CandidateAwardItems.Remove(_context.CandidateAwardItems.FirstOrDefault(x => x.Id == item.Id));
                        }
                    }
                }
            }
            //_context.SaveChanges();

            CandidateCV candidateCV = new CandidateCV();
            if (resumeEditDto.CandidateCV != null)
            {
                if (resumeEditDto.CandidateCV.ContentType != "application/pdf" && resumeEditDto.CandidateCV.ContentType != "application/msword" && resumeEditDto.CandidateCV.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    ModelState.AddModelError("CandidateCV", "Pdf,doc ve ya docx formatinda file daxil edilmelidir");
                    return View();
                }
                if (resumeEditDto.CandidateCV.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("CandidateCV", "File olcusu 5mb-dan cox olmaz!");
                    return View();
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + resumeEditDto.CandidateCV.FileName;
                var path = Path.Combine(rootPath, "CVs", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    resumeEditDto.CandidateCV.CopyTo(stream);
                }
                candidateCV.CVName = fileName;
                candidateCV.CandidateId = existCandidate.Id;
                
            }
            else
            {
                if (resumeEditDto.CandidateCVsId == null)
                {
                    if (existCandidate.CandidateCVs != null)
                    {
                        foreach (var item in existCandidate.CandidateCVs)
                        {
           
                                string rootPath = _env.WebRootPath;
                                var fileName = item.CVName;
                                var path = Path.Combine(rootPath, "CVs", fileName);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                _context.CandidateCVs.Remove(item);
                        }
                    }
                }
            }
            if (existCandidate.CandidateCVs != null)
            {
                if (resumeEditDto.CandidateCVsId != null)
                {
                    foreach (var item in existCandidate.CandidateCVs)
                    {
                        if (!resumeEditDto.CandidateCVsId.Contains(item.Id))
                        {
                            string rootPath = _env.WebRootPath;
                            var fileName = item.CVName;
                            var path = Path.Combine(rootPath, "CVs", fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            _context.CandidateCVs.Remove(item);
                        }
                    }
                }
            }

            if (candidateCV != null && candidateCV.CandidateId!=0 && candidateCV.CVName!=null)
            {
                _context.CandidateCVs.Add(candidateCV);
            }
            if (existCandidate.CandidateImages != null)
            {

                if (resumeEditDto.ImagesId != null)
                {
                    foreach (var item in existCandidate.CandidateImages)
                    {
                        if (!resumeEditDto.ImagesId.Contains(item.Id) && item.Id != 0)
                        {
                            if (item.IsPoster == false)
                            {
                                string rootPath = _env.WebRootPath;
                                var fileName = item.Image;
                                var path = Path.Combine(rootPath, "images/candidateImage", fileName);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                _context.CandidateImages.Remove(item);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in existCandidate.CandidateImages)
                    {
                        if (item.IsPoster == false)
                        {
                            string rootPath = _env.WebRootPath;
                            var fileName = item.Image;
                            var path = Path.Combine(rootPath, "images/candidateImage", fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            _context.CandidateImages.Remove(item);
                        }
                    }
                }
            }
            if (resumeEditDto.Images != null)
            {
                foreach (var item in resumeEditDto.Images)
                {
                    if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("Images", "Mime type yanlisdir!");
                        return View(resumeEditDto);
                    }

                    if (item.Length > (1024 * 1024) * 5)
                    {
                        ModelState.AddModelError("Images", "Faly olcusu 5MB-dan cox ola bilmez!");
                        return View(resumeEditDto);
                    }

                    string rootPath = _env.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + item.FileName;
                    var path = Path.Combine(rootPath, "images/candidateImage", fileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    CandidateImage candidateImage = new CandidateImage
                    {
                        Image = fileName,
                        IsPoster = false,
                        CandidateId = existCandidate.Id
                    };
                    _context.CandidateImages.Add(candidateImage);
                }
            }


            _context.SaveChanges();
            return RedirectToAction("index"); 
        }
        public IActionResult ApplyForJob(int jobId)
        {
            Apply apply = new Apply();
            Job job = _context.Jobs.FirstOrDefault(x => x.Id == jobId && x.IsDeleted==false && x.ApplyStatus==Enums.ApplyStatus.Accepted) ;
            if (job == null) return RedirectToAction("index");
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && x.UserStatus==Enums.UserStatus.Candidate);
            }
            else
            {
                return RedirectToAction("index");
            }
            if(user==null) return RedirectToAction("index");
            if (_context.Applies.Any(x => x.AppUserId == user.Id && x.JobId == jobId)) return RedirectToAction("index");// artiq var
            Candidate candidate = _context.Candidates.FirstOrDefault(x => x.AppUserId == user.Id && x.IsDeleted==false);
            if (candidate.PhoneNumber == null || candidate.FullName==null) return RedirectToAction("CandidateProfileEdit");  //melumatlar doldurulmalidir
            if(!_context.Applies.Any(x=>x.JobId==jobId && x.AppUserId == user.Id))
            {
                apply.AppUserId = user.Id;
                apply.JobId = jobId;
                apply.FullName = candidate.FullName;
                apply.ContactPhone = candidate.PhoneNumber;
                apply.RequestDate = DateTime.UtcNow.AddHours(4);
                apply.ApplyStatus = Enums.ApplyStatus.Pending;
                if (!ModelState.IsValid) return RedirectToAction("index");
                _context.Applies.Add(apply);
            }
            _context.SaveChanges();
            return RedirectToAction();
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult RemoveApply(int id)
        {
            Apply apply = _context.Applies.Include(x=>x.Job).Include(x=>x.AppUser).ThenInclude(x=>x.Candidate).FirstOrDefault(x => x.Id == id && x.Job.IsDeleted==false && x.Job.ApplyStatus==Enums.ApplyStatus.Accepted && x.AppUser.Candidate.IsDeleted==false);
            if (apply == null) return RedirectToAction();
            if (apply.ApplyStatus==Enums.ApplyStatus.Pending)
            {
                apply.ApplyStatus = Enums.ApplyStatus.UserReject;
            }
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult Following(int employerId)
        {
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user=_context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            }
            else { return RedirectToAction("index"); }
            Candidate candidate = _context.Candidates.Include(x=>x.AppUser).FirstOrDefault(x => x.AppUserId == user.Id && user.UserStatus==Enums.UserStatus.Candidate && x.IsDeleted==false);
            if (candidate == null) { return RedirectToAction("index"); }
            if (!_context.Followers.Include(x=>x.Candidate).Include(x=>x.Employer).Any(x => x.CandidateId==candidate.Id && x.EmployerId == employerId && x.Candidate.IsDeleted==false && x.Employer.IsDeleted==false))
            {
                Follower follower = new Follower
                {
                    CandidateId = candidate.Id,
                    EmployerId = employerId
                };
                _context.Followers.Add(follower);
            }

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult RemoveFollow(int id)
        {
            Follower follower = _context.Followers.Include(x=>x.Candidate).Include(x=>x.Employer).FirstOrDefault(x => x.Id == id && x.Employer.IsDeleted==false && x.Candidate.IsDeleted==false);
            if (follower == null) return RedirectToAction();
            _context.Followers.Remove(follower);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewContact(CandidateContact candidateContact)
        {
            if (candidateContact == null || candidateContact.CandidateId == 0 || !ModelState.IsValid) return RedirectToAction("detail");
            if (!_context.Candidates.Any(x => x.Id == candidateContact.CandidateId && x.IsDeleted==false))
            {
                return RedirectToAction("detail");
            }
            _context.CandidateContacts.Add(candidateContact);
            _context.SaveChanges();
            return RedirectToAction("detail");
        }
        [Authorize(Roles = "Candidate")]

        public IActionResult GetContacts(string search,int page = 1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.Search = search;
            AppUser user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null) return RedirectToAction("index");
            Candidate candidate = _context.Candidates.Include(x => x.AppUser).Include(x => x.CandidateImages).Include(x => x.City).FirstOrDefault(x => x.AppUser.UserName == user.UserName && x.IsDeleted==false);
            if (candidate == null) return RedirectToAction("index");
            List<CandidateContact> contacts;
            if (search!=null)
            {
                contacts = _context.CandidateContacts.Include(x => x.Candidate).ThenInclude(x => x.CandidateImages).Where(x => x.CandidateId == candidate.Id && x.Email.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 5).Take(5).ToList();
            }
            else
            {
                contacts = _context.CandidateContacts.Include(x => x.Candidate).ThenInclude(x => x.CandidateImages).Where(x => x.CandidateId == candidate.Id).Skip((page - 1) * 5).Take(5).ToList();
            }
            if (contacts == null) return RedirectToAction("index");
            ContactsAndCandidate contactsAndCandidate = new ContactsAndCandidate
            {
                Candidate = candidate,
                CandidateContacts = contacts,
            };
            ViewBag.TotalPageCount = Math.Ceiling(_context.CandidateContacts.Where(x => x.CandidateId == candidate.Id).Count() / 5m);
            return View(contactsAndCandidate);
        }
        public List<Apply> GetApplies(int id)
        {
            return _context.Applies.Include(x=>x.AppUser).ThenInclude(x=>x.Candidate).Where(x=>x.RequestDate.Year<=DateTime.UtcNow.Year && x.RequestDate.Year>=DateTime.UtcNow.Year-10 && x.AppUser.Candidate.Id==id).ToList();
        }
    }
}
