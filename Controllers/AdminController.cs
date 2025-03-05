using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Traditiona_trend_on_rent.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // ✅ Ensure Only Admin Can Access
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")) ||
                !(HttpContext.Session.GetString("Email") == "isha13@gmail.com" ||
                  HttpContext.Session.GetString("Email") == "umangi8@gmail.com" ||
                  HttpContext.Session.GetString("Email") == "jay14@gmail.com"))
            {
                return RedirectToAction("Login", "Account"); // Redirect to Login if Not Admin
            }

            return View(); // ✅ Load Admin Dashboard
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account"); // Redirect to Login Page
        }
    }
}
