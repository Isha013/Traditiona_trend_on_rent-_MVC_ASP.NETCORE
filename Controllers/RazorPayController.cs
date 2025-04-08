using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Traditiona_trend_on_rent.Models;

namespace Traditiona_trend_on_rent.Controllers
{
    public class RazorPayController : Controller
    {
        private readonly string key = "YOUR_RAZORPAY_KEY";
        private readonly string secret = "YOUR_RAZORPAY_SECRET";
        private readonly string connectionString = "YOUR_DATABASE_CONNECTION_STRING";

        public IActionResult ProceedToPayment(decimal amount, string name, string email, string phone)
        {
            var client = new RazorpayClient(key, secret);
            var orderOptions = new Dictionary<string, object>
            {
                { "amount", (int)(amount * 100) }, // Amount in paise
                { "currency", "INR" },
                { "receipt", "receipt_" + Guid.NewGuid().ToString() },
                { "payment_capture", 1 }
            };
            var order = client.Order.Create(orderOptions);
            string orderId = order["id"].ToString();

            // Save payment details into the SQL database using ADO.NET
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Payment (Name, Email, Phone, Amount, OrderId, PaymentStatus, CreatedAt) " +
                               "VALUES (@Name, @Email, @Phone, @Amount, @OrderId, 'Pending', GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.ExecuteNonQuery();
                }
            }

            ViewBag.RazorpayKey = key;
            ViewBag.Amount = amount;
            ViewBag.OrderId = orderId;
            ViewBag.CustomerName = name;
            ViewBag.CustomerEmail = email;
            ViewBag.CustomerPhone = phone;

            return View("Payment");
        }

        public IActionResult PaymentSuccess(string paymentId, string orderId)
        {
            // Update payment status in the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Payment SET PaymentId = @PaymentId, PaymentStatus = 'Success' WHERE OrderId = @OrderId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaymentId", paymentId);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.ExecuteNonQuery();
                }
            }

            ViewBag.PaymentId = paymentId;
            return View("PaymentSuccess");
        }
    }
}
