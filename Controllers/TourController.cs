using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.Services.Interfaces;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookingTourWebApp_MVC.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public TourController(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
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
                            UploadTime = DateTime.UtcNow
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
    }
}
