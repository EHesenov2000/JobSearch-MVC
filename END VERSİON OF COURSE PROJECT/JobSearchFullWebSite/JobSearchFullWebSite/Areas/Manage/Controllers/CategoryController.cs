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

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
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
            if (search!=null)
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Categories.Where(x=> x.Name.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Categories.Count() / 6m);
            }
            if (search != null)
            {
                return View(_context.Categories.Include(x => x.Employers).Where(x=>x.Name.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList());
            }
            return View(_context.Categories.Include(x=>x.Employers).Skip((page-1)*6).Take(6).ToList());
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            _context.Categories.Add(category);
            _context.SaveChanges();
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existCategory == null) return RedirectToAction("index");

            return View(existCategory);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Category category)
        {
            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existCategory == null) return RedirectToAction("index");
            if (category == null) return RedirectToAction("index");
            existCategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        //public IActionResult Delete(int id)
        //{
        //    Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
        //    if (existCategory == null) return RedirectToAction("index");
        //    //existCategory.IsDeleted = true;
        //    _context.SaveChanges();
        //    return RedirectToAction("index");
        //}
    }
}
