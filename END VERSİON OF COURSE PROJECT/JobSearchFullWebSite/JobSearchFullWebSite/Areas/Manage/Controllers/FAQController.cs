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

    public class FAQController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FAQController(AppDbContext context, IWebHostEnvironment env)
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
                ViewBag.TotalPage = Math.Ceiling(_context.FAQs.Where(x=>x.Title.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.FAQs.Count() / 6m);
            }

            if (search != null)
            {
                return View(_context.FAQs.Where(x=> x.Title.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList()); ;
            }
            return View(_context.FAQs.Skip((page - 1) * 6).Take(6).ToList()); ;
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FAQ faq)
        {
            if (!ModelState.IsValid) return View(faq);
            _context.FAQs.Add(faq);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            if(!_context.FAQs.Any(x=>x.Id==id)) return RedirectToAction("index");

            return View(_context.FAQs.FirstOrDefault(x=>x.Id==id));
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,FAQ faq)
        {
            FAQ existFaq = _context.FAQs.FirstOrDefault(x => x.Id == id);
            if(existFaq==null) return RedirectToAction("index");
            if (!ModelState.IsValid) return View(faq);

            existFaq.Title = faq.Title;
            existFaq.Question = faq.Question;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            FAQ existFaq = _context.FAQs.FirstOrDefault(x => x.Id == id);
            if (existFaq == null) return RedirectToAction("index");
            _context.FAQs.Remove(existFaq);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
