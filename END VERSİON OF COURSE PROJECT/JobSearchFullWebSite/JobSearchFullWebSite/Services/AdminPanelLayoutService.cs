using JobSearchFullWebSite.DAL.AppDbContext;
using JobSearchFullWebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Services
{
    public class AdminPanelLayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public AdminPanelLayoutService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public List<SiteContact> GetContacts()
        {
            return _context.SiteContacts.ToList();
        }
        public List<Job> GetWaitingJobs()
        {
            return _context.Jobs.Where(x => x.IsDeleted == false && x.ApplyStatus==Enums.ApplyStatus.Pending).ToList();
        }
        public Admin GetAdmin(string userName)
        {
            return _context.Admins.Include(x=>x.AppUser).FirstOrDefault(x => x.AppUser.UserName== userName);
        }
    }
}
