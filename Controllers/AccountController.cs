using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Traditiona_trend_on_rent.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

public class AccountController : Controller
{
    private readonly string _connectionString;

    public AccountController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    // ✅ Register GET
    public IActionResult Register()
    {
        return View();
    }
 


    // ✅ Register POST
    [HttpPost]
    public IActionResult Register(Users user, string CPassword)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        if (user.Password != CPassword)
        {
            ViewBag.ConfirmPasswordError = "Passwords do not match.";
            return View(user);
        }

        string hashedPassword = HashPassword(user.Password);

        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();

            string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            using (SqlCommand checkCmd = new SqlCommand(checkEmailQuery, con))
            {
                checkCmd.Parameters.AddWithValue("@Email", user.Email);
                int emailExists = (int)checkCmd.ExecuteScalar();
                if (emailExists > 0)
                {
                    ViewBag.Message = "Email already exists. Please use another email.";
                    return View(user);
                }
            }

            try
            {
                string query = "INSERT INTO Users (UserName, Email, Phone, Password) VALUES (@UserName, @Email, @Phone, @Password)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return View(user);
            }
        }

        ViewBag.Message = "Registration successful!";
        return RedirectToAction("Login");
    }

    // ✅ Login GET
    public IActionResult Login()
    {
        return View();
    }

    // ✅ Login POST
    [HttpPost]
    public IActionResult Login(string Email, string Password)
    {
        List<string> adminEmails = new List<string> { "isha13@gmail.com", "umangi8@gmail.com", "jay14@gmail.com" };

        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();
            string query = "SELECT * FROM Users WHERE Email = @Email";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", Email);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHashedPassword = reader["Password"]?.ToString() ?? string.Empty;
                        string userName = reader["UserName"]?.ToString() ?? string.Empty;

                        if (VerifyPassword(Password, storedHashedPassword))
                        {
                            HttpContext.Session.SetString("UserName", userName);
                            HttpContext.Session.SetString("Email", Email);

                            // ✅ Check if User is Admin
                            if (adminEmails.Contains(Email))
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                        }
                    }
                }
            }
        }

        ViewBag.ErrorMessage = "Invalid Email or Password";
        return View();
    }

    // ✅ Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Register", "Account");
    }

    // ✅ Hash Password
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    // ✅ Verify Password
    private bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return HashPassword(enteredPassword) == storedHash;
    }
}
