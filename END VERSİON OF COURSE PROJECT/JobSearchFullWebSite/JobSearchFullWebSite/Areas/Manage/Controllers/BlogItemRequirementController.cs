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

    public class BlogItemRequirementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogItemRequirementController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index(int id,int page=1)
        {
            if (page < 1)
            {
                page = 1;
            }
            ViewBag.BlogId = id;
            ViewBag.SelectedPage = page;
            ViewBag.TotalPage = Math.Ceiling(_context.BlogItemRequirements.Where(x => x.BlogItemId == id).Count() / 6m);
            return View(_context.BlogItemRequirements.Where(x=>x.BlogItemId==id).Skip((page-1)*6).Take(6).ToList());
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create(int id)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == id);
            if (blogItem == null)  return RedirectToAction("index", "blog");
            ViewBag.ItemId = blogItem.Id;
            
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id,BlogItemRequirement blogItemRequirement)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == id);
            if (blogItem == null) return RedirectToAction("index", "blog");
            if (!ModelState.IsValid) return View(blogItemRequirement);
            BlogItemRequirement newItem = new BlogItemRequirement
            {
                Requirement = blogItemRequirement.Requirement,
                BlogItemId = blogItem.Id,
                BlogItem=blogItem,
            };
            _context.BlogItemRequirements.Add(newItem);
            _context.SaveChanges();
            return RedirectToAction("index","blog");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int blogItemId,int id)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == blogItemId);
            if (blogItem == null) return RedirectToAction("index", blogItemId);
            BlogItemRequirement ıtemRequirement = _context.BlogItemRequirements.FirstOrDefault(x => x.Id == id && x.BlogItemId==blogItemId);
            if (ıtemRequirement == null) return RedirectToAction("index", blogItemId);
            return View(ıtemRequirement);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int blogItemId,int id,BlogItemRequirement blogItemRequirement)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == blogItemId);
            if (blogItem == null) return RedirectToAction("index", blogItemId);
            if (!ModelState.IsValid) return View(blogItemRequirement);
            BlogItemRequirement ıtemRequirement = _context.BlogItemRequirements.FirstOrDefault(x => x.Id == id);
            if (ıtemRequirement == null) return RedirectToAction("index", blogItemId);
            ıtemRequirement.Requirement = blogItemRequirement.Requirement;
            _context.SaveChanges();
            return RedirectToAction("index", "blog");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int blogItemId, int id)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == blogItemId);
            if (blogItem == null) return RedirectToAction("index", blogItemId);
            BlogItemRequirement ıtemRequirement = _context.BlogItemRequirements.FirstOrDefault(x => x.Id == id);
            if(ıtemRequirement==null) return RedirectToAction("index", blogItemId);
            _context.BlogItemRequirements.Remove(ıtemRequirement);
            _context.SaveChanges();
            return RedirectToAction("index", "blog");
        }
    }
}
