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

    public class SiteContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SiteContactController(AppDbContext context, IWebHostEnvironment env)
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
                ViewBag.TotalPage = Math.Ceiling(_context.SiteContacts.Where(x=>x.Name.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.SiteContacts.Count() / 6m);
            }

            if (search != null)
            {
                return View(_context.SiteContacts.Where(x=> x.Name.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList());
            }
            return View(_context.SiteContacts.Skip((page - 1) * 6).Take(6).ToList());
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SiteContact siteContact )
        {
            if (!ModelState.IsValid) return View(siteContact);
            _context.SiteContacts.Add(siteContact);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            SiteContact siteContact = _context.SiteContacts.FirstOrDefault(x => x.Id == id);
            if (siteContact == null) return RedirectToAction("index");

            return View(siteContact);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,SiteContact siteContact)
        {
            SiteContact existSiteContact = _context.SiteContacts.FirstOrDefault(x => x.Id == id);
            if (existSiteContact == null) return RedirectToAction("index");
            if (!ModelState.IsValid) return View(siteContact);
            existSiteContact.Name = siteContact.Name;
            existSiteContact.Email = siteContact.Email;
            existSiteContact.Subject = siteContact.Subject;
            existSiteContact.Comment=siteContact.Comment;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            SiteContact existSiteContact = _context.SiteContacts.FirstOrDefault(x => x.Id == id);
            if (existSiteContact == null) return RedirectToAction("index");
            _context.SiteContacts.Remove(existSiteContact);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
