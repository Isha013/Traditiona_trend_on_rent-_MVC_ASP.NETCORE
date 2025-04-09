using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Traditiona_trend_on_rent.Controllers
{
    public class DashboardController : Controller
    {
        // ✅ Dashboard Page (After Login)
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("Dashboard"); // Assuming your Dashboard view is in Views/Dashboard
        }

        public IActionResult History()
        {
            return View("History");
        }

        public IActionResult Categories()
        {
            return View("Categories"); // Simplified path
        }

        public IActionResult Booking()
        {
            return View("Booking"); // Simplified path
        }
        public IActionResult SaveBooking()
        {
            return View("SaveBooking"); // Simplified path
        }
        public IActionResult Edit()
        {
            return View(); // Full path if not in the default folder
        }
        public IActionResult Wishlist()
        {
            return View(); // Typo here
        }
       

        public IActionResult UserProfile()
        {
            return View(); // Typo here
        }

        public IActionResult RazorPay()
        {
            return View("RazorPay");
        }
   




        public IActionResult Profile()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }
    }
}
