using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Traditiona_trend_on_rent.Controllers
{
    public class DashboardController : Controller
    {
        // ✅ Dashboard Page (After Login)
        public IActionResult Index()
        {
            // ✅ Ensure user is logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("~/Views/Home/Dashboard.cshtml"); // ✅ Load "Dashboard.cshtml"
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            return View();
        }

        public IActionResult Profile()
        {
            // ✅ Ensure user is logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }
    }
}
