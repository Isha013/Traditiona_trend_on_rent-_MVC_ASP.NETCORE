using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Collections.Generic;
using Traditiona_trend_on_rent.Models;
using Microsoft.Extensions.Configuration;

namespace Traditiona_trend_on_rent.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly List<string> adminEmails = new List<string> { "isha13@gmail.com", "umangi8@gmail.com", "jay14@gmail.com" };

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ✅ Function to Check Admin Session
        private bool CheckAdminSession()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            return !string.IsNullOrEmpty(userEmail) && adminEmails.Contains(userEmail);
        }

        public IActionResult Index()
        {
            // ✅ Ensure Only Admin Can Access
            if (!CheckAdminSession())
            {
                return RedirectToAction("Login", "Account");
            }

            // ✅ Fetch Total Registered Users Count
            int totalRegisteredUsers = GetRegisteredUserCount();

            // ✅ Fetch Customer Reviews from Database
            List<Contact> contacts = new List<Contact>();
            string connectionString = _configuration.GetConnectionString("SecondConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Name, Email, Message FROM ContactUs";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(new Contact
                            {
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Message = reader["Message"].ToString()
                            });
                        }
                    }
                }
            }

            // ✅ Pass total registered users count to the view
            ViewData["TotalRegisteredUsers"] = totalRegisteredUsers;

            return View("Index", contacts);
        }

        // ✅ Fetch Registered Users List
        public IActionResult RegisteredUsers()
        {
            if (!CheckAdminSession())
            {
                return RedirectToAction("Login", "Account");
            }

            List<User> users = new List<User>();
            string connectionString = _configuration.GetConnectionString("SecondConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Name, Email, Phone, CreatedAt FROM Users";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : (DateTime?)null
                            });
                        }
                    }
                }
            }

            return View("RegisteredUsers", users);
        }

        // ✅ Function to Get Registered Users Count
        private int GetRegisteredUserCount()
        {
            int count = 0;
            string connectionString = _configuration.GetConnectionString("SecondConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
