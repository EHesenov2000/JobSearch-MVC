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

    public class LanguageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public LanguageController(AppDbContext context, IWebHostEnvironment env)
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
                ViewBag.TotalPage = Math.Ceiling(_context.Languages.Where(x=>x.Name.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Languages.Count() / 6m);
            }

            if (search != null)
            {
                return View(_context.Languages.Where(x=> x.Name.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList());
            }
            return View(_context.Languages.Skip((page-1)*6).Take(6).ToList());
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Language language)
        {
            if (!ModelState.IsValid) return View(language);
            _context.Languages.Add(language);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            Language existLanguage = _context.Languages.FirstOrDefault(x => x.Id == id);
            if (existLanguage == null) return RedirectToAction("index");

            return View(existLanguage);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Language language)
        {
            Language existLanguage = _context.Languages.FirstOrDefault(x => x.Id == id);
            if (existLanguage == null) return RedirectToAction("index");
            existLanguage.Name = language.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            Language existLanguage = _context.Languages.FirstOrDefault(x => x.Id == id);
            if (existLanguage == null) return RedirectToAction("index");
            _context.Languages.Remove(existLanguage);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
