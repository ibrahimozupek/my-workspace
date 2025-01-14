using GeziRehberi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace GeziRehberi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Details(int id)
        {
            var city = await _context.Cities
                .Include(c => c.Places)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
            {
                return NotFound();
            }
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
     
            ViewData["CityId"] = city.Id; 
                ViewData["Comments"] = await _context.comments
                    .Where(c => c.CityId == city.Id)
                    .OrderByDescending(c => c.CreatedAt)
                     .Select(c => new Comment
                     {
                         Id = c.Id,
                         CityId = c.CityId,
                         UserName = c.UserName,
                         Content = c.Content,
                         CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(c.CreatedAt, timeZone)
                     })
                    .ToListAsync();
            

            return View(city);
        }

        public async Task<IActionResult> Cities()
        {
            var cities = await _context.Cities.ToListAsync();
            return View(cities); 
        }

        public async Task<IActionResult> Detail(int id)
        {
            var city = await _context.Cities
                .Include(c => c.comments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
            {
                return NotFound(); 
            }

            // Yorumlarý yükleme
            var comments = await _context.comments
                .Where(c => c.CityId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.comments = comments; 
            return View(city);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int cityId, string userName, string content)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(content))
            {
                return BadRequest("Geçersiz giriþ.");
            }

            var comment = new Comment
            {
                CityId = cityId,
                UserName = userName,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = cityId }); 
        }


    }
}
