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

    public class JobCategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public JobCategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index(string search,int page=1)
        {
            if (page < 1)
            {
                page = 1;
            }
            ViewBag.SelectedPage = page;
            ViewBag.Search = search;
            if (search!=null)
            {
                ViewBag.TotalPage = Math.Ceiling(_context.JobCategories.Where(x=>x.Name.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.JobCategories.Count() / 6m);
            }
            if (search != null)
            {
                return View(_context.JobCategories.Include(x => x.Jobs).Where(x=> x.Name.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList()); ;
            }

            return View(_context.JobCategories.Include(x => x.Jobs).Skip((page-1)*6).Take(6).ToList()); ;
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(JobCategory jobCategory)
        {
            if (!ModelState.IsValid) return View(jobCategory);
            _context.JobCategories.Add(jobCategory);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            JobCategory existJobCategory = _context.JobCategories.Include(x=>x.Jobs).FirstOrDefault(x => x.Id == id);
            if (existJobCategory == null) return View(existJobCategory);

            return View(existJobCategory);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, JobCategory jobCategory)
        {
            JobCategory existJobCategory = _context.JobCategories.Include(x => x.Jobs).FirstOrDefault(x => x.Id == id);
            if (existJobCategory == null) return View(existJobCategory);
            existJobCategory.Name = jobCategory.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    //[Authorize(Roles = "Admin,SuperAdmin")]
        //public IActionResult Delete(int id)
        //{
        //    JobCategory existJobCategory = _context.JobCategories.Include(x => x.Jobs).FirstOrDefault(x => x.Id == id);
        //    if (existJobCategory == null) return View(existJobCategory);
        //    _context.JobCategories.Remove(existJobCategory);
        //    _context.SaveChanges();
        //    return RedirectToAction("index");
        //}
    }
}
