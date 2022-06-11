using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.Models;
using JobSearchFullWebSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        public HomeController(AppDbContext context, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(int jobsPage=1)
        {
        
            ViewBag.SelectedPage = jobsPage;
            ViewBag.TotalPageCount = Math.Ceiling(_context.Jobs.Where(x => x.IsFeatured && x.IsDeleted == false).ToList().Count()/9m);
            HomeViewModel HomeVm = new HomeViewModel
            {
                HowWorks = _context.HowWorks.ToList(),
                Jobs = _context.Jobs.Include(x => x.JobImages).Include(x => x.JobCategory).Include(x => x.City).Include(x => x.Employer).ThenInclude(x => x.Category).Where(x => x.IsFeatured && x.IsDeleted==false).Skip((jobsPage - 1) * 9).Take(9).ToList(),
                Cities = _context.Cities.Include(x => x.Jobs).OrderByDescending(x => x.Jobs.Where(x=>x.IsDeleted==false).Count()).ToList(),
                Candidates = _context.Candidates.Include(x => x.CandidateImages).Include(x => x.Position).Include(x => x.City).Where(x => x.IsFeatured).ToList(),
                BlogItems = _context.BlogItems.OrderBy(x => x.CreatedAt).Take(3).ToList(),
                JobCategories = _context.JobCategories.Include(x => x.Jobs).ToList(),
                Candidate = (_context.Candidates.Include(x => x.AppUser).FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name) != null ? _context.Candidates.Include(x => x.AppUser).FirstOrDefault(x => x.AppUser.UserName == User.Identity.Name) : null)
            };
            if (  User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
            { 
                await _signInManager.SignOutAsync();
            }
            return View(HomeVm);
        }
            
    }
}
