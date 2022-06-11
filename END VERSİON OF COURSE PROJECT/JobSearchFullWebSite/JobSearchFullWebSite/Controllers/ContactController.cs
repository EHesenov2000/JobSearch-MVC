using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            
            _context = context;
        }
        public IActionResult Index()
        {
            Setting setting = _context.Settings.First();
            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddContactItem(SiteContact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(_context.Settings.First());
            }
            _context.SiteContacts.Add(contact);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult GetSubscribe(string email)
        {

            if (IsValidEmail(email) == false)
            {
                return Json(new { isSucceeded = false, message = "Düzgün e-mail daxil edin!" });
            }

            if (_context.Subscribes.Any(x => x.Email == email))
            {
                return Json(new { isSucceeded = false, message = "Email artıq 1 dəfə əlavə edilib!" });
            }
            Subscribe subscribe = new Subscribe
            {
                Email = email,
            };

            //if (!ModelState.IsValid) return RedirectToAction("index", "home");
            _context.Subscribes.Add(subscribe);
            _context.SaveChanges();
            return Json(new { isSucceeded = true,message="E-mail uğurla əlavə edildi" });
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
