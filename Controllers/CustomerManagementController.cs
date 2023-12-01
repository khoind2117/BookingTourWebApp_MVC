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
            return View("CustomerManagement",result);
        }

        public async Task<IActionResult> SortASC()
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var userInfo = await _context.Users.ToListAsync();
            var result = userInfo.Select(us => new CustomerInfo
            {
                Id = us.Id,
                userName = us.UserName,
                fullName = us.FullName,
                gmail = us.Email,
                phoneNumber = us.PhoneNumber,
            });
            return PartialView("CustomerSortASC", result.OrderBy(u => u.fullName).ToList());
        }
        public async Task<IActionResult> SortDESC()
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var userInfo = await _context.Users.ToListAsync();
            var result = userInfo.Select(us => new CustomerInfo
            {
                Id = us.Id,
                userName = us.UserName,
                fullName = us.FullName,
                gmail = us.Email,
                phoneNumber = us.PhoneNumber,
            });
            return PartialView("CustomerSortDESC", result.OrderByDescending(u => u.fullName).ToList());
        }
        public async Task<IActionResult> RefreshUserList()
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var userInfo = await _context.Users.ToListAsync();
            var result = userInfo.Select(us => new CustomerInfo
            {
                Id = us.Id,
                userName = us.UserName,
                fullName = us.FullName,
                gmail = us.Email,
                phoneNumber = us.PhoneNumber,
            }).ToList();
            return PartialView("CustomerRefresh", result);
        }
        public async Task<IActionResult> HandleSearchUser(string keyword,string typekw)
        {

            var dt = _context.Users.AsQueryable();

                if (typekw == "fullname")
                {
                    dt = dt.Where(u => u.UserName.Contains(keyword));

                }
                if (typekw == "phonenumber")
                {
                    dt = dt.Where(u => u.PhoneNumber.Contains(keyword));
                }
                if (typekw == "gmail")
                {
                    dt = dt.Where(u => u.Email.Contains(keyword));
                }
                var data = await dt.Select(u => new CustomerInfo()
                {
                    Id = u.Id,
                    userName = u.UserName,
                    fullName = u.FullName,
                    gmail = u.Email,
                    phoneNumber = u.PhoneNumber,
                }).ToListAsync();
                return PartialView("HandleSearchUser", data);
                



        }

    }
}
