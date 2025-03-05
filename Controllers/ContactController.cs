using Microsoft.AspNetCore.Mvc;
using Traditiona_trend_on_rent.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace Traditiona_trend_on_rent.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View("../Home/ContactUs"); // ✅ Ensure correct view path
        }

        [HttpPost]
        public IActionResult ContactUs(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // ✅ Get Connection String
                    string connectionString = _configuration.GetConnectionString("SecondConnection");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO ContactUs (Name, Email, Message, CreatedAt) VALUES (@Name, @Email, @Message, @CreatedAt)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Name", contact.Name);
                            cmd.Parameters.AddWithValue("@Email", contact.Email);
                            cmd.Parameters.AddWithValue("@Message", contact.Message);
                            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    ViewBag.Message = "Your message has been sent successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }

            return View("../Home/ContactUs", contact); // ✅ Ensure correct view after form submission
        }
    }
}
