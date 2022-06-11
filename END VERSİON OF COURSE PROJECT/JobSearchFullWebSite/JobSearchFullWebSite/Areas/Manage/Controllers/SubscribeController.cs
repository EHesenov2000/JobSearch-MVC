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

    public class SubscribeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SubscribeController(AppDbContext context, IWebHostEnvironment env)
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
                ViewBag.TotalPage = Math.Ceiling(_context.Subscribes.Where(x=>x.Email.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Subscribes.Count() / 6m);
            }

            if (search != null)
            {
                return View(_context.Subscribes.Where(x => x.Email.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList());
            }
            return View(_context.Subscribes.Skip((page - 1) * 6).Take(6).ToList());
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subscribe subscribe)
        {
            if (!ModelState.IsValid) return View(subscribe);
            if (_context.Subscribes.Any(x => x.Email == subscribe.Email)){
                ModelState.AddModelError("Email", "Bu email artiq elave edilib");
                return View();
            }
            _context.Subscribes.Add(subscribe);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            Subscribe existSubscribe = _context.Subscribes.FirstOrDefault(x => x.Id == id);
            if (existSubscribe == null) return RedirectToAction("index");

            return View(existSubscribe);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Subscribe subscribe)
        {
            Subscribe existSubscribe = _context.Subscribes.FirstOrDefault(x => x.Id == id);
            if (existSubscribe == null) return RedirectToAction("index");
            if (_context.Subscribes.Any(x => x.Email == subscribe.Email))
            {
                ModelState.AddModelError("Email", "Bu email artiq elave edilib");
                return View();
            }
            existSubscribe.Email = subscribe.Email;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            Subscribe existSubscribe = _context.Subscribes.FirstOrDefault(x => x.Id == id);
            if (existSubscribe == null) return RedirectToAction("index");
            _context.Subscribes.Remove(existSubscribe);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

    }
}
