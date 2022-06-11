﻿using JobSearchFullWebSite.DAL.AppDbContext;
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

    public class PositionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PositionController(AppDbContext context, IWebHostEnvironment env)
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
                ViewBag.TotalPage = Math.Ceiling(_context.Positions.Include(x => x.Candidates).Where(x=>x.PositionName.ToUpper().Contains(search.ToUpper())).Count() / 6m);
            }
            else
            {
                ViewBag.TotalPage = Math.Ceiling(_context.Positions.Include(x => x.Candidates).Count() / 6m);
            }

            if (search != null)
            {
                return View(_context.Positions.Include(x => x.Candidates).Where(x=> x.PositionName.ToUpper().Contains(search.ToUpper())).Skip((page - 1) * 6).Take(6).ToList());
            }
            return View(_context.Positions.Include(x => x.Candidates).Skip((page-1)*6).Take(6).ToList()) ;
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Position position)
        {
            if (!ModelState.IsValid) return View(position);
            _context.Positions.Add(position);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            Position existPosition = _context.Positions.FirstOrDefault(x => x.Id == id);
            if (existPosition == null) return RedirectToAction("index");

            return View(existPosition);
        }
    [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Position position)
        {
            Position existPosition = _context.Positions.FirstOrDefault(x => x.Id == id);
            if (existPosition == null) return RedirectToAction("index");
            existPosition.PositionName = position.PositionName;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    //[Authorize(Roles = "Admin,SuperAdmin")]
    //    public IActionResult Delete(int id)
    //    {
    //        Position existPosition = _context.Positions.FirstOrDefault(x => x.Id == id);
    //        if (existPosition == null) return RedirectToAction("index");
    //        _context.Positions.Remove(existPosition);
    //        _context.SaveChanges();
    //        return RedirectToAction("index");
    //    }
    }
}
