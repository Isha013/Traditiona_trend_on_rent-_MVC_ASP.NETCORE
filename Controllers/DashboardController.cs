using Microsoft.AspNetCore.Mvc;
using Traditiona_trend_on_rent.Models;

public class DashboardController : Controller
{
    // Dummy data
    private static List<HistoryItem> historyItems = new List<HistoryItem>
    {
        new HistoryItem { Id = 2, Image = "~/Images/choli 1.png", Name = "Royal Blue Gown", Address = "Surat", CholiName = "Classic Choli", SetType = "Partial Set", Time = "12 Hours", SetSize = "L", Contact = "91739014175" },
        new HistoryItem { Id = 3, Image = "~/Images/design 5.png", Name = "Elegant Red Gown", Address = "Rajkot", CholiName = "Wedding Choli", SetType = "Full Set", Time = "48 Hours", SetSize = "XL", Contact = "91739014176" },
        new HistoryItem { Id = 4, Image = "~/Images/design 4.png", Name = "Navratri Special", Address = "Rajkot", CholiName = "Navratri Choli", SetType = "Full Set", Time = "12 Hours", SetSize = "XL", Contact = "91739014176" },
        new HistoryItem { Id = 5, Image = "~/Images/red.png", Name = "Red Designer Lehenga", Address = "Ahmedabad", CholiName = "Festive Choli", SetType = "Full Set", Time = "24 Hours", SetSize = "M", Contact = "91739014177" }
    };

    public IActionResult History()
    {
        return View(historyItems);
    }

    public IActionResult Edit(int id)
    {
        var item = historyItems.FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    public IActionResult Edit(HistoryItem updatedItem)
    {
        var index = historyItems.FindIndex(x => x.Id == updatedItem.Id);
        if (index != -1)
        {
            historyItems[index] = updatedItem;
        }
        return RedirectToAction("History");
    }

    public IActionResult Delete(int id)
    {
        var item = historyItems.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            historyItems.Remove(item);
        }
        return RedirectToAction("History");
    }

    public IActionResult Categories() => View();
    public IActionResult Booking() => View();
    public IActionResult SaveBooking() => View();
    public IActionResult Wishlist() => View();
    public IActionResult UserProfile() => View();
    public IActionResult RazorPay() => View();

    public IActionResult Profile()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            return RedirectToAction("Login", "Account");

        ViewBag.UserName = HttpContext.Session.GetString("UserName");
        return View();
    }
}
