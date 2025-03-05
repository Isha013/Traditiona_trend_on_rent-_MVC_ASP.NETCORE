using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult OurStory()
    {
        return View();
    }

    public IActionResult OurTeam()
    {
        return View();
    }

    public IActionResult ContactUs()
    {
        return View();
    }
    public IActionResult Dashboard()
    {
        return View(); // ? Ensure this returns the view
    }
    public ActionResult Register()
    {
        return View();
    }

}
