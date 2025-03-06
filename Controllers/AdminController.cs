using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
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

        // ✅ Check if the user is an admin
        private bool IsAdminLoggedIn()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            return !string.IsNullOrEmpty(userEmail) && adminEmails.Contains(userEmail);
        }

        // ✅ Admin Dashboard (Displays total users & contact messages)
        public IActionResult Index()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            int totalRegisteredUsers = GetTotalRegisteredUsers();
            List<Contact> contacts = FetchContactMessages();

            ViewData["TotalRegisteredUsers"] = totalRegisteredUsers;
            return View("Index", contacts);
        }

        // ✅ Fetch and Display Registered Users
        public IActionResult RegisteredUsers()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            List<Users> users = FetchRegisteredUsers();
            return View("RegisteredUsers", users);
        }

        // ✅ Logout Function
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // ✅ Fetch Total Number of Registered Users
        private int GetTotalRegisteredUsers()
        {
            int count = 0;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    count = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                }
            }
            return count;
        }

        // ✅ Fetch Contact Messages from Database
        private List<Contact> FetchContactMessages()
        {
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
                                Name = reader["Name"]?.ToString() ?? "Unknown",
                                Email = reader["Email"]?.ToString() ?? "N/A",
                                Message = reader["Message"]?.ToString() ?? "No Message"
                            });
                        }
                    }
                }
            }
            return contacts;
        }

        // ✅ Fetch Registered Users from Database
        private List<Users> FetchRegisteredUsers()
        {
            List<Users> users = new List<Users>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // ✅ Fixed SQL query syntax error (added missing comma)
                string query = "SELECT UserName, Email, Phone, Password, CreatedAt FROM Users";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["UserName"]?.ToString() ?? "Unknown";
                            string email = reader["Email"]?.ToString() ?? "N/A";
                            string phone = reader["Phone"]?.ToString() ?? "0000000000";

                            // ✅ Mask password for security reasons
                            string password = "******";

                            DateTime createdAt = DateTime.MinValue;
                            if (reader["CreatedAt"] != DBNull.Value && DateTime.TryParse(reader["CreatedAt"].ToString(), out DateTime parsedDate))
                            {
                                createdAt = parsedDate;
                            }

                            users.Add(new Users
                            {
                                UserName = name,
                                Email = email,
                                Phone = phone,
                                Password = password,
                                CreatedAt = createdAt
                            });
                        }
                    }
                }
            }
            return users;
        }
    }
}
