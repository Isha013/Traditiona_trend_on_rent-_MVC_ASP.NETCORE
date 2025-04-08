using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.IO;
using Traditiona_trend_on_rent.Models;

namespace Traditiona_trend_on_rent.Controllers
{
    public class CollectionController : Controller
    {
        public IActionResult PartyWear()
        {
            return View("PartyWear");
        }

        public IActionResult Wedding()
        {
            return View("Wedding");
        }

        public IActionResult Navratri()
        {
            return View("Navratri");
        }

    }
}
    
