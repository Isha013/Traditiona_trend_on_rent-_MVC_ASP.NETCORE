using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using Traditiona_trend_on_rent.Models;
using Traditiona_trend_on_rent.Services;

public class BookingController : Controller
{
    private readonly string _connectionString;
    private readonly EmailService _emailService;

    public BookingController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("FourthConnection");
        _emailService = new EmailService(configuration);
    }

    // GET: AddBooking
    [HttpGet]
    public IActionResult AddBooking()
    {
        return View();
    }
    public IActionResult RazorPay()
    {
        return View(); // This will return the view RazorPay.cshtml from Views/Dashboard
    }


    // POST: AddBooking
    [HttpPost]
    public IActionResult AddBooking(Booking model)
    {
        if (ModelState.IsValid)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = @"INSERT INTO Booking 
                                     (Name, Address, CholiName, CholiType, TopwearFabric, BottomwearFabric, 
                                      DupattaType, RentalPrice, SetType, RentalTime, SetSize, ContactNumber, Email) 
                                     VALUES 
                                     (@Name, @Address, @CholiName, @CholiType, @TopwearFabric, @BottomwearFabric, 
                                      @DupattaType, @RentalPrice, @SetType, @RentalTime, @SetSize, @ContactNumber, @Email)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@CholiName", model.CholiName);
                    cmd.Parameters.AddWithValue("@CholiType", model.CholiType);
                    cmd.Parameters.AddWithValue("@TopwearFabric", model.TopwearFabric);
                    cmd.Parameters.AddWithValue("@BottomwearFabric", model.BottomwearFabric);
                    cmd.Parameters.AddWithValue("@DupattaType", model.DupattaType);
                    cmd.Parameters.AddWithValue("@RentalPrice", model.RentalPrice);
                    cmd.Parameters.AddWithValue("@SetType", model.SetType);
                    cmd.Parameters.AddWithValue("@RentalTime", model.RentalTime);
                    cmd.Parameters.AddWithValue("@SetSize", model.SetSize);
                    cmd.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                    cmd.Parameters.AddWithValue("@Email", model.Email);

                    cmd.ExecuteNonQuery();
                }
            }

            // Send email
            string subject = "Booking Confirmation";
            string body = $"Hi {model.Name},\n\nThank you for your booking.\nYour booking has been received successfully!\nhttps://localhost:7021/PayNow";
            _emailService.SendEmail(model.Email, subject, body);

            TempData["Success"] = "Booking added and email sent!";
            return RedirectToAction("Booking", "Dashboard");

        }

        return View(model);
    }

    // GET: Booking List
    public IActionResult CustomerBooking()
    {
        List<Booking> bookings = new List<Booking>();

        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();
            string query = "SELECT * FROM Booking";

            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    bookings.Add(new Booking
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Address = reader["Address"].ToString(),
                        CholiName = reader["CholiName"].ToString(),
                        CholiType = reader["CholiType"].ToString(),
                        TopwearFabric = reader["TopwearFabric"].ToString(),
                        BottomwearFabric = reader["BottomwearFabric"].ToString(),
                        DupattaType = reader["DupattaType"].ToString(),
                        RentalPrice = Convert.ToDecimal(reader["RentalPrice"]),
                        SetType = reader["SetType"].ToString(),
                        RentalTime = reader["RentalTime"].ToString(),
                        SetSize = reader["SetSize"].ToString(),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
        }

        return View(bookings);
    }

    // Accept Booking
    public IActionResult AcceptBooking(int id)
    {
        string email = GetEmailById(id);
        if (!string.IsNullOrEmpty(email))
        {
            _emailService.SendEmail(email, "Order Accepted", "Your order has been accepted!");
        }

        TempData["Success"] = "Accepted and email sent.";
        return RedirectToAction("CustomerBooking");
    }

    // Deny Booking
    public IActionResult DenyBooking(int id)
    {
        string email = GetEmailById(id);
        if (!string.IsNullOrEmpty(email))
        {
            _emailService.SendEmail(email, "Order Denied", "Your order has been denied.");
        }

        TempData["Error"] = "Denied and email sent.";
        return RedirectToAction("CustomerBooking");
    }

    private string GetEmailById(int id)
    {
        string email = "";

        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();
            string query = "SELECT Email FROM Booking WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        email = reader["Email"].ToString();
                    }
                }
            }
        }

        return email;
    }
}


