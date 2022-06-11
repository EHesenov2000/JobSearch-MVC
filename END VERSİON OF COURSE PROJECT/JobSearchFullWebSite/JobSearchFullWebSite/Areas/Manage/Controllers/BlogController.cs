using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Areas.Manage.Controllers
{
    [Area("manage")]

    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
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
            if (search != null)
            {
                ViewBag.TotalPage = Math.Ceiling(_context.BlogItems.Where(x=>x.Title.ToUpper().Contains(search.ToUpper())).Count() / 6m);
                return View(_context.BlogItems.Include(x => x.BlogItemLearns).Include(x => x.BlogItemRequirements).Where(x=>x.Title.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList());

            }
            ViewBag.TotalPage = Math.Ceiling(_context.BlogItems.Count() / 6m);
            return View(_context.BlogItems.Include(x => x.BlogItemLearns).Include(x => x.BlogItemRequirements).Skip((page - 1) * 6).Take(6).ToList());
        }
        [Authorize(Roles = "Admin,SuperAdmin")]

        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogItem blogItem)
        {
            //if (!ModelState.IsValid) return View(blogItem);
            if (blogItem.ContainerImageFile == null || blogItem.FullImageFile == null)
            {
                if (blogItem.ContainerImageFile == null)
                {
                    ModelState.AddModelError("ContainerImageFile", "Container Image daxil edilmelidir");
                }
                if (blogItem.FullImageFile == null)
                {
                    ModelState.AddModelError("FullImageFile", "Full Image daxil edilmelidir");
                }
                    return View(blogItem);
            }


            if (blogItem.FullImageFile != null)
            {
                if (blogItem.FullImageFile.ContentType != "image/png" && blogItem.FullImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("FullImageFile", "Mime type yanlisdir!");
                    return View(blogItem);
                }

                if (blogItem.FullImageFile.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("FullImageFile", "Faly olcusu 5MB-dan cox ola bilmez!");
                    return View(blogItem);
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + blogItem.FullImageFile.FileName;
                var path = Path.Combine(rootPath, "images/blogImage", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    blogItem.FullImageFile.CopyTo(stream);
                }
                blogItem.FullImage = fileName;
            }
            if (blogItem.ContainerImageFile != null)
            {
                if (blogItem.ContainerImageFile.ContentType != "image/png" && blogItem.ContainerImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ContainerImageFile", "Mime type yanlisdir!");
                    return View(blogItem);
                }

                if (blogItem.ContainerImageFile.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("ContainerImageFile", "Faly olcusu 5MB-dan cox ola bilmez!");
                    return View(blogItem);
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + blogItem.ContainerImageFile.FileName;
                var path = Path.Combine(rootPath, "images/blogImage", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    blogItem.ContainerImageFile.CopyTo(stream);
                }
                blogItem.ContainerImage = fileName;
            }
            blogItem.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.BlogItems.Add(blogItem);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Admin,SuperAdmin")]

        public IActionResult Edit(int id)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == id);
            if (blogItem == null) return RedirectToAction("index");
            return View(blogItem);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,BlogItem blogItem)
        {
            BlogItem existBlogItem = _context.BlogItems.FirstOrDefault(x => x.Id == id);
            if (existBlogItem == null) return RedirectToAction("index");
            if (!ModelState.IsValid) return View(blogItem);
            if (blogItem.FullImageFile != null)
            {
                if (blogItem.FullImageFile.ContentType != "image/png" && blogItem.FullImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("FullImageFile", "Mime type yanlisdir!");
                    return View(blogItem);
                }

                if (blogItem.FullImageFile.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("FullImageFile", "Faly olcusu 5MB-dan cox ola bilmez!");
                    return View(blogItem);
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + blogItem.FullImageFile.FileName;
                var path = Path.Combine(rootPath, "images/blogImage", fileName);

                if (existBlogItem.FullImage != null)
                {
                    var existPath = Path.Combine(rootPath, "images/blogImage", existBlogItem.FullImage);
                    if (System.IO.File.Exists(existPath))
                    {
                        System.IO.File.Delete(existPath);
                    }
                    existBlogItem.FullImage = fileName;
                }
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    blogItem.FullImageFile.CopyTo(stream);
                }
            }
            if (blogItem.ContainerImageFile != null)
            {
                if (blogItem.ContainerImageFile.ContentType != "image/png" && blogItem.ContainerImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ContainerImageFile", "Mime type yanlisdir!");
                    return View(blogItem);
                }

                if (blogItem.ContainerImageFile.Length > (1024 * 1024) * 5)
                {
                    ModelState.AddModelError("ContainerImageFile", "Faly olcusu 5MB-dan cox ola bilmez!");
                    return View(blogItem);
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + blogItem.ContainerImageFile.FileName;
                var path = Path.Combine(rootPath, "images/blogImage", fileName);
   
                if (existBlogItem.ContainerImage != null)
                {
                    var existPath = Path.Combine(rootPath, "images/blogImage", existBlogItem.ContainerImage);
                    if (System.IO.File.Exists(existPath))
                    {
                        System.IO.File.Delete(existPath);
                    }
                    existBlogItem.ContainerImage = fileName;
                }
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    blogItem.ContainerImageFile.CopyTo(stream);
                }
            }
            existBlogItem.Title = blogItem.Title;
            existBlogItem.Subject = blogItem.Subject;
            existBlogItem.BackBlueTextHead = blogItem.BackBlueTextHead;
            existBlogItem.BackBlueTextFoot = blogItem.BackBlueTextFoot;
            existBlogItem.CreatedAt = DateTime.UtcNow.AddHours(4);
            existBlogItem.DescriptionTextEditor = blogItem.DescriptionTextEditor;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            BlogItem blogItem = _context.BlogItems.FirstOrDefault(x => x.Id == id);
            if (blogItem == null) return RedirectToAction("index");
            _context.BlogItems.Remove(blogItem);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
