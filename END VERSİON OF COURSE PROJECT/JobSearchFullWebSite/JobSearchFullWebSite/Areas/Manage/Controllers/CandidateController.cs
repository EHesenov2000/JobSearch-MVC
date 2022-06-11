using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.DTOs;
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

    public class CandidateController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CandidateController(AppDbContext context, IWebHostEnvironment env)
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
                ViewBag.TotalPage = Math.Ceiling(_context.Candidates.Where(x=>x.FullName.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Candidates.Count() / 6m);
            }
            List<Candidate> candidates;
            if (search!=null)
            {
                candidates = _context.Candidates.Include(x => x.CandidateImages).Include(x=>x.AppUser).Include(x => x.Position).Include(x => x.City).Include(x => x.KnowingLanguages).Include(x => x.Followers).Include(x => x.CandidateCVs).Where(x => x.IsDeleted == false && x.FullName.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList();
            }
            else
            {
                candidates = _context.Candidates.Include(x => x.CandidateImages).Include(x => x.AppUser).Include(x => x.Position).Include(x => x.City).Include(x => x.KnowingLanguages).Include(x => x.Followers).Include(x => x.CandidateCVs).Where(x => x.IsDeleted == false).Skip((page - 1) * 6).Take(6).ToList();
            }
            return View(candidates);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Detail(int id)
        {
            Candidate candidate = _context.Candidates.Include(x => x.CandidateImages).Include(x => x.Position).Include(x => x.City).Include(x => x.KnowingLanguages).Include(x => x.Followers).Include(x => x.CandidateCVs).FirstOrDefault(x => x.Id == id && x.IsDeleted==false);
            if (candidate == null) return RedirectToAction("index");
            return View(candidate);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            Candidate candidate = _context.Candidates.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (candidate == null) return RedirectToAction("index");
            candidate.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
