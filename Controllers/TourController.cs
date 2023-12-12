using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.Services.Interfaces;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookingTourWebApp_MVC.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public TourController(ApplicationDbContext context,
            IPhotoService photoService,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _photoService = photoService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchTour(string Departure, string Destination)
        {
            IEnumerable<Tour> tours = await _context.Tours.Where(t => t.Departure == Departure
                                                && t.Destination == Destination)
                                                .OrderBy(t => t.DepartureDate)
                                                .ToListAsync();

            if (tours == null)
            {
                return RedirectToAction("ListTour", "Tour");
            }

            string toursJson = JsonConvert.SerializeObject(tours);
            HttpContext.Session.SetString("ToursData", toursJson); // Lưu chuỗi JSON vào Session

            return RedirectToAction("ListTour", "Tour");
        }

        [HttpGet]
        public async Task<IActionResult> ListTour()
        {
            string toursJson = HttpContext.Session.GetString("ToursData");

            // Ngăn cho người dùng truy cập bằng cách sử dụng url trực tiếp mà không thông qua form
            if (string.IsNullOrEmpty(toursJson))
            {
                return RedirectToAction("Index", "Tour");
            }

            IEnumerable<Tour> tours = JsonConvert.DeserializeObject<IEnumerable<Tour>>(toursJson);

            // Xóa dữ liệu từ Session sau khi đã sử dụng để không lưu trữ lâu dài
            //HttpContext.Session.Remove("ToursData");

            return View(tours);
        }

        [HttpPost]
        public async Task<IActionResult> BuyTour(int id)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var tour = await _context.Tours.FindAsync(id);

            if(tour == null)
            {
                TempData["ErrorMessage"] = "Đặt tour thất bại!";
                return RedirectToAction("Index", "Tour");
            }

            var userTour = new UserTour
            {
                AppUserId = appUser.Id,
                AppUser = appUser,
                TourId = tour.Id,
                Tour = tour,
            };

            _context.UserTours.Add(userTour);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đặt tour thành công!";
            return RedirectToAction("Index", "Tour");
        }

        [HttpGet]
        public async Task<IActionResult> TourManagement()
        {
            var tours = await _context.Tours.ToListAsync();
            return View(tours);
        }


        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            var createTourViewModel = new CreateTourViewModel();
            return View(createTourViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourViewModel tourVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _photoService.AddPhotoASync(tourVM.Image);

                    if (result != null && !string.IsNullOrEmpty(result.Url.ToString()))
                    {
                        var newTour = new Tour
                        {
                            Name = tourVM.Name,
                            Departure = tourVM.Departure,
                            Destination = tourVM.Destination,
                            DepartureDate = tourVM.DepartureDate,
                            ReturnDate = tourVM.ReturnDate,
                            Price = tourVM.Price,
                            Description = tourVM.Description,
                            Image = result.Url.ToString(),
                            UploadTime = DateTime.UtcNow,
                            Discount = tourVM.Discount,
                        };

                        _context.Tours.Add(newTour);
                        _context.SaveChanges();

                        TempData["SuccessMessage"] = "Tạo Tour du lịch thành công.";
                        return RedirectToAction("CreateTour");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Tạo Tour du lịch thất bại. Vui lòng thử lại sau.";
                    }
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải lên hình ảnh. Vui lòng thử lại sau.";
                }
            }

            // Nếu có lỗi, hoặc dịch vụ bên thứ ba trả về lỗi, hiển thị view với thông báo lỗi
            return View(tourVM);
        }

        [HttpGet]
        public async Task<IActionResult> DetailTour(int id)
        {
            Tour tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id);
            return View(tour);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTour(int id)
        {
            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id);
            if (tour == null)
            {
                return View("Error");
            }

            var tourVM = new UpdateTourViewModel
            {
                Name = tour.Name,
                Departure = tour.Departure,
                Destination = tour.Destination,
                DepartureDate = tour.DepartureDate,
                ReturnDate = tour.ReturnDate,
                Description = tour.Description,
                Price = tour.Price,
                URL = tour.Image,
                Discount = tour.Discount,
            };
            return View(tourVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(int id, UpdateTourViewModel tourVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Tạo Tour du lịch thất bại. Vui lòng thử lại sau.";
                return View("UpdateTour", tourVM);
            }

            var tour = await _context.Tours.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();

            if (tour != null)
            {
                try
                {
                    await _photoService.DeletePhotoASync(tour.Image);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Tạo Tour du lịch thất bại. Vui lòng thử lại sau.";
                    return View(tourVM);
                }
                var photoResult = await _photoService.AddPhotoASync(tourVM.Image);

                var updatedtour = new Tour
                {
                    Name = tourVM.Name,
                    Departure = tourVM.Departure,
                    Destination = tourVM.Destination,
                    DepartureDate = tourVM.DepartureDate,
                    ReturnDate = tourVM.ReturnDate,
                    Description = tourVM.Description,
                    Price = tourVM.Price,
                    Image = photoResult.Url.ToString(),
                    UploadTime = DateTime.UtcNow
                };

                _context.Tours.Update(updatedtour);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Tạo Tour du lịch thành công.";

                return RedirectToAction("UpdateTour");
            }
            else
            {
                return View(tourVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var tour = await _context.Tours.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();

            try
            {
                await _photoService.DeletePhotoASync(tour.Image);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Xóa Tour du lịch thất bại.";
                return View("TourManagement");
            }

            _context.Tours.Remove(tour);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Xóa Tour du lịch thành công.";
            return RedirectToAction("TourManagement");
        }
    }
}
