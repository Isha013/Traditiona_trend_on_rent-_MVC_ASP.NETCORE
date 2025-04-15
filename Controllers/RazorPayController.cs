using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Traditiona_trend_on_rent.Controllers
{
    public class RazorPayController : Controller
    {
        private readonly string key = "rzp_test_x8tV5oSUixLmbV";  // Replace with live key later
        private readonly string secret = "fWH4faC9rEJ9StONJyc8ZXCc"; // Replace with live secret

        // Initial page to make a payment
        [Route("PayNow")]
        public IActionResult Index()
        {
            return View("~/Views/PayNow/Index.cshtml");
        }

        // Post method to create an order
        [HttpPost]
        public IActionResult CreateOrder(string name, string email, string phone, decimal amount)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(key, secret);

                var options = new Dictionary<string, object>
                {
                    { "amount", amount * 100 }, // Razorpay accepts amount in paise
                    { "currency", "INR" },
                    { "receipt", Guid.NewGuid().ToString() },
                    { "payment_capture", 1 }
                };

                // Create order via Razorpay API
                Order order = client.Order.Create(options);

                // Pass the Razorpay details to the view
                ViewBag.RazorpayKey = key;
                ViewBag.OrderId = order["id"].ToString();  // Order ID from Razorpay
                ViewBag.Amount = amount;  // Amount in INR
                ViewBag.CustomerName = name;
                ViewBag.CustomerEmail = email;
                ViewBag.CustomerPhone = phone;

                // Redirect to payment page
                return View("~/Views/PayNow/Payment.cshtml");
            }
            catch (Exception ex)
            {
                // Handle any errors during order creation
                ViewBag.ErrorMessage = "Order creation failed: " + ex.Message;
                return View("PaymentFailed");
            }
        }

        // Handle Payment Success
        public IActionResult PaymentSuccess(string paymentId, string orderId)
        {
            try
            {
                // Example customer details (in real scenario, you can fetch from DB or TempData)
                string name = "Demo User";
                string email = "demo@example.com";
                string phone = "1234567890";
                decimal amount = 100;

                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Payment;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Payment 
                                     (Name, Email, Phone, Amount, OrderId, PaymentId, PaymentStatus, CreatedAt)
                                     VALUES 
                                     (@Name, @Email, @Phone, @Amount, @OrderId, @PaymentId, @PaymentStatus, @CreatedAt)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@PaymentId", paymentId);
                    cmd.Parameters.AddWithValue("@PaymentStatus", "Success");
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return View("PaymentSuccess");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Payment success processing failed: " + ex.Message;
                return View("PaymentFailed");
            }
        }

        // Handle Payment Failure
        public IActionResult PaymentFailed()
        {
            return View("PaymentFailed");
        }
    }
}
