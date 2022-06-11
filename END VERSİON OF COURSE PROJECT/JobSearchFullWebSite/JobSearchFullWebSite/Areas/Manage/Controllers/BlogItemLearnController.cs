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

    public class BlogItemLearnController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogItemLearnController(AppDbContext context, IWebHostEnvironment env)
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
            ViewBag.TotalPage = Math.Ceiling(_context.BlogItemLearns.Where(x => x.BlogItemId == id).Count() / 6m);
            return View(_context.BlogItemLearns.Where(x=>x.BlogItemId==id).Skip((page-1)*6).Take(6).ToList());
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create(int learnId)
        {

            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == learnId);
            if (blogItem == null) return RedirectToAction("index", "blog");
            ViewBag.ItemId = learnId;
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int learnId, BlogItemLearn blogItemLearn)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == learnId);
            if (blogItem == null) return RedirectToAction("index", "blog");
            if (!ModelState.IsValid) return View(blogItemLearn);
            blogItemLearn.BlogItemId = blogItem.Id;
            ViewBag.ItemId = learnId;
            _context.BlogItemLearns.Add(blogItemLearn);
            _context.SaveChanges();
            return RedirectToAction("index", "blog");
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int blogItemId,int id)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == blogItemId);
            if (blogItem == null) return RedirectToAction("index", "blog");
            BlogItemLearn ıtemLearn = _context.BlogItemLearns.FirstOrDefault(x => x.Id == id && x.BlogItemId==blogItemId);
            if (ıtemLearn == null) return RedirectToAction("index", "blog");
            return View(ıtemLearn);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int blogItemId,int id,BlogItemLearn blogItemLearn)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == blogItemId);
            if (blogItem == null) return RedirectToAction("index", "blog");
            if (!ModelState.IsValid) return View(blogItemLearn);
            BlogItemLearn ıtemLearn = _context.BlogItemLearns.FirstOrDefault(x => x.Id == id);
            if (ıtemLearn == null) return RedirectToAction("index", "blog");
            ıtemLearn.Learn = blogItemLearn.Learn;
            _context.SaveChanges();
            return RedirectToAction("index", "blog");
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int blogItemId, int id)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == blogItemId);
            if (blogItem == null) return RedirectToAction("index", "blog");
            BlogItemLearn ıtemLearn = _context.BlogItemLearns.FirstOrDefault(x => x.Id == id);
            if(ıtemLearn == null) return RedirectToAction("index", "blog");
            _context.BlogItemLearns.Remove(ıtemLearn);
            _context.SaveChanges();
            return RedirectToAction("index", "blog");
        }
    }
}
