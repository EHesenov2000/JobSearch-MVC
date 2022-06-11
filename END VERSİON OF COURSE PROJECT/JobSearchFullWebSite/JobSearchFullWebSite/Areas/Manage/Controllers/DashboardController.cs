using JobSearchFullWebSite.Areas.Manage.ViewModels;
using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Areas.Manage.Controllers
{
    [Area("manage")]

    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DashboardController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel
            {
                Jobs = _context.Jobs.Where(x => x.IsDeleted == false).ToList(),
                Candidates = _context.Candidates.Where(x => x.IsDeleted == false).ToList(),
                Employers = _context.Employers.Where(x => x.IsDeleted == false).ToList()
            };
            return View(dashboardViewModel);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public List<Job> GetJobs()
        {
            return _context.Jobs.Where(x => x.IsDeleted == false && x.CreatedAt.Year<=DateTime.UtcNow.Year && x.CreatedAt.Year>=DateTime.UtcNow.Year-10).ToList();
        }
    }
}
