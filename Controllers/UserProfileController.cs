using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Traditiona_trend_on_rent.Models;


public class UserProfileController : Controller
{
    public IActionResult Index()
    {
        var model = new UserProfile
        {
            Name = "Umang Itank",
            Role = "User",
            Email = "umangitank99@gmail.com",
            Mobile = "9173914174",
            PhotoPath = "/images/UserProfile.jpeg",
            Password = "123456"
        };

        return View(model); // ✅ Pass model to view
    }


    [HttpPost]
    public async Task<IActionResult> Update(UserProfile model, IFormFile? photoFile)
    {
        if (photoFile != null)
        {
            var fileName = Path.GetFileName(photoFile.FileName);
            var filePath = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photoFile.CopyToAsync(stream);
            }

            model.PhotoPath = "/images/" + fileName;
        }

        TempData["Success"] = "Profile updated successfully!";
        return View("Index", model);
    }

    public IActionResult Logout()
    {
        return RedirectToAction("Index", "Dashboard");
    }
}
