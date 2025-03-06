using Microsoft.AspNetCore.Mvc;
using Traditiona_trend_on_rent.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

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
            return View("../Home/ContactUs");
        }

        [HttpPost]
        public IActionResult ContactUs(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string connectionString = _configuration.GetConnectionString("SecondConnection");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO ContactUs (Name, Email, Message, CreatedAt) VALUES (@Name, @Email, @Message, @CreatedAt)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Name", contact.Name ?? "Unknown");  // ✅ Null-safe
                            cmd.Parameters.AddWithValue("@Email", contact.Email ?? "No Email");  // ✅ Null-safe
                            cmd.Parameters.AddWithValue("@Message", contact.Message ?? "No Message");  // ✅ Null-safe
                            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    ViewBag.Message = "Your message has been sent successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }

            return View("../Home/ContactUs", contact);
        }

        public IActionResult ContactList()
        {
            List<Contact> contacts = new List<Contact>();
            string connectionString = _configuration.GetConnectionString("SecondConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name, Email, Message, CreatedAt FROM ContactUs";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // ✅ Null-safe variables
                            string name = reader["Name"] != DBNull.Value ? reader["Name"].ToString()! : "Unknown";
                            string email = reader["Email"] != DBNull.Value ? reader["Email"].ToString()! : "N/A";
                            string message = reader["Message"] != DBNull.Value ? reader["Message"].ToString()! : "No Message";

                            DateTime createdAt = DateTime.UtcNow; // ✅ Default Value
                            if (reader["CreatedAt"] != DBNull.Value)
                            {
                                DateTime.TryParse(reader["CreatedAt"].ToString(), out createdAt);
                            }

                            contacts.Add(new Contact
                            {
                                Name = name,
                                Email = email,
                                Message = message,
                                CreatedAt = createdAt // ✅ Safe Null Handling
                            });
                        }
                    }
                }
            }

            return View("ContactList", contacts);
        }
    }
}
