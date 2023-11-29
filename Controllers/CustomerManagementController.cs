using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BookingTourWebApp_MVC.Controllers
{
    public class CustomerManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerManagementController(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if(_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var userInfo= await _context.Users.ToListAsync();
            var result = userInfo.Select(us => new CustomerInfo
            {
                Id = us.Id,
                userName = us.UserName,
                fullName = us.FullName,
                gmail = us.Email,
                phoneNumber = us.PhoneNumber,
            });
            return View(result);
        }
        //public async Task<IActionResult> GetList()
        //{
        //    var data = await _context.Users.FromSqlInterpolated($"select * from dbo.AspNetUsers").Select(u => new CustomerInfo
        //    {
        //        Id = u.Id,
        //        userName = u.UserName,
        //        fullName = u.FullName,
        //        gmail = u.Email,
        //        phoneNumber = u.PhoneNumber,
        //    }).ToListAsync();
        //    return View(data);
        //}
        //public async Task<IActionResult> GetPlane()
        //{
        //    var data = await _context.Planes.FromSqlInterpolated($"select * from dbo.Plane").ToListAsync();
        //    return View(data);
        //}
        //public IActionResult SearchUser()
        //{
        //    return View("SearchUser");
        //}
        
        public async Task<IActionResult> HandleSearchUser(string keyword,string typekw)
        {

            
			if (typekw == "fullname")
            {
                var data = await _context.Users.Where(u => u.UserName.Contains(keyword)).Select(u => new CustomerInfo
				{
					Id = u.Id,
					userName = u.UserName,
					fullName = u.FullName,
					gmail = u.Email,
					phoneNumber = u.PhoneNumber,
				}).ToListAsync();
				if (data == null)
				{
					return NotFound();
				}
				return PartialView("HandleSearchUser", data);
			}
			else if(typekw == "phonenumber")
            {
                var data = await _context.Users.Where(u => u.PhoneNumber.Contains(keyword)).Select(u => new CustomerInfo
				{
					Id = u.Id,
					userName = u.UserName,
					fullName = u.FullName,
					gmail = u.Email,
					phoneNumber = u.PhoneNumber,
				}).ToListAsync();
				if (data == null)
				{
					return NotFound();
				}
				return PartialView("HandleSearchUser", data);
			}
            else
            {
				var data = await _context.Users.Where(u => u.Email.Contains(keyword)).Select(u => new CustomerInfo
				{
					Id = u.Id,
					userName = u.UserName,
					fullName = u.FullName,
					gmail = u.Email,
					phoneNumber = u.PhoneNumber,
				}).ToListAsync();
                if(data == null)
                {
                    return NotFound();
                }
				return PartialView("HandleSearchUser", data);

			}
            
        }

    }
}
