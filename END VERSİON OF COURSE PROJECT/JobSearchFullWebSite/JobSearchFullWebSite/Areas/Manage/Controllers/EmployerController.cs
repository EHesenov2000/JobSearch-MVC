using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.DTOs;
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

    public class EmployerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployerController(AppDbContext context, IWebHostEnvironment env)
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
                ViewBag.TotalPage = Math.Ceiling(_context.Employers.Where(x => x.IsDeleted == false && x.Name.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Employers.Where(x => x.IsDeleted == false).Count() / 6m);
            }

            List<Employer> employers;
            if (search != null)
            {
                employers = _context.Employers.Include(x => x.EmployerImages).Include(x => x.Category).Include(x => x.City).Include(x => x.Jobs).Include(x => x.Followers).Where(x => x.IsDeleted == false && x.Name.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList();
            }
            else
            {
                employers = _context.Employers.Include(x => x.EmployerImages).Include(x => x.Category).Include(x => x.City).Include(x => x.Jobs).Include(x => x.Followers).Where(x => x.IsDeleted == false).Skip((page - 1) * 6).Take(6).ToList();
            }
            return View(employers);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Detail(int id)
        {
            Employer employer = _context.Employers.Include(x=>x.EmployerImages).Include(x=>x.Jobs).Include(x=>x.Followers).Include(x=>x.City).Include(x=>x.Category).FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (employer == null) return RedirectToAction("index");
            return View(employer);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            Employer employer = _context.Employers.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if(employer==null ) return RedirectToAction("index");
            employer.IsDeleted = true;
            _context.SaveChanges(); 
            return RedirectToAction("index");
        }
    }
}
