using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Areas.Manage.Controllers
{
    [Area("manage")]

    public class TermController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TermController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index(string search,int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }
            ViewBag.SelectedPage = page;
            ViewBag.Search = search;
            if (search!=null)
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Terms.Where(x=>x.SiteTerm.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Terms.Count() / 6m);
            }

            if (search != null)
            {
                return View(_context.Terms.Where(x => x.SiteTerm.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList());
            }
            return View(_context.Terms.Skip((page - 1) * 6).Take(6).ToList());
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Term term)
        {
            if (!ModelState.IsValid) return View(term);
            _context.Terms.Add(term);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            Term existTerm = _context.Terms.FirstOrDefault(x => x.Id == id);
            if (existTerm == null) return RedirectToAction("index");

            return View(existTerm);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Term term)
        {
            Term existTerm = _context.Terms.FirstOrDefault(x => x.Id == id);
            if (existTerm == null) return RedirectToAction("index");
            existTerm.SiteTerm = term.SiteTerm;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            Term existTerm = _context.Terms.FirstOrDefault(x => x.Id == id);
            if (existTerm == null) return RedirectToAction("index");
            _context.Terms.Remove(existTerm);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
