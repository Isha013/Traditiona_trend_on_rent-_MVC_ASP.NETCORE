using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Traditiona_trend_on_rent.Models;
//using razorpay
namespace Traditiona_trend_on_rent.Controllers
{
    [Route("RazorPay")]
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
        [Route("CreateOrder")]
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
        [Route("PaymentSuccess")]
        public IActionResult PaymentSuccess(string paymentId, string orderId)
        {
            try
            {
                string name = "Demo User"; // Placeholder values, replace with real values
                string email = "demo@example.com";
                string phone = "1234567890";
                decimal amount = 100; // Replace with actual amount if needed

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

                // Redirect to Payment Success view
                return View("~/Views/PayNow/PaymentSuccess.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Payment success processing failed: " + ex.Message;
                return View("~/Views/PayNow/PaymentFailed.cshtml");
            }
        }

        // Handle Payment Failure
        [Route("PaymentFailed")]
        public IActionResult PaymentFailed()
        {
            return View("~/Views/PayNow/PaymentFailed.cshtml");
        }

        // Show all payments
        [Route("ShowPayments")]
        public IActionResult ShowPayments()
        {
            List<Payment> payments = new List<Payment>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Payment;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Payment";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    payments.Add(new Payment
                    {
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        OrderId = reader["OrderId"].ToString(),
                        PaymentId = reader["PaymentId"].ToString(),
                        PaymentStatus = reader["PaymentStatus"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    });
                }
            }

            return View("~/Views/Admin/Payment.cshtml", payments);
        }
    }
}
