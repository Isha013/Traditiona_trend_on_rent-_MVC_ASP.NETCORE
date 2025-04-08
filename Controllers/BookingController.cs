using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using Traditiona_trend_on_rent.Models;

public class BookingController : Controller
{
    private readonly string _connectionString;

    public BookingController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bookings;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    [HttpPost]
    public IActionResult SaveBooking(Booking booking)
    {
        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bookings;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
                string query = @"
                    INSERT INTO Booking 
                    (Name, Address, CholiName, CholiType, TopwearFabric, BottomwearFabric, 
                    DupattaType, RentalPrice, SetType, RentalTime, SetSize, ContactNumber) 
                    VALUES 
                    (@Name, @Address, @CholiName, @CholiType, @TopwearFabric, @BottomwearFabric, 
                    @DupattaType, @RentalPrice, @SetType, @RentalTime, @SetSize, @ContactNumber);";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", booking.Name);
                    cmd.Parameters.AddWithValue("@Address", booking.Address);
                    cmd.Parameters.AddWithValue("@CholiName", booking.CholiName);
                    cmd.Parameters.AddWithValue("@CholiType", booking.CholiType);
                    cmd.Parameters.AddWithValue("@TopwearFabric", booking.TopwearFabric);
                    cmd.Parameters.AddWithValue("@BottomwearFabric", booking.BottomwearFabric);
                    cmd.Parameters.AddWithValue("@DupattaType", booking.DupattaType);
                    cmd.Parameters.AddWithValue("@RentalPrice", booking.RentalPrice);
                    cmd.Parameters.AddWithValue("@SetType", booking.SetType);
                    cmd.Parameters.AddWithValue("@RentalTime", booking.RentalTime);
                    cmd.Parameters.AddWithValue("@SetSize", booking.SetSize);
                    cmd.Parameters.AddWithValue("@ContactNumber", booking.ContactNumber);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        TempData["SuccessMessage"] = "Booking saved successfully!";
                        return View("../Dashboard/SaveBooking", booking);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to save booking. Please try again.";
                        return View("../Dashboard/SaveBooking", booking);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Database Error: " + ex.Message;
            Console.WriteLine("Error: " + ex.ToString());  // Logs the full error details
            return View("../Dashboard/SaveBooking", booking);
        }
    }

    public IActionResult CustomerBooking()
    {

        List<Booking> bookings = new List<Booking>();

        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bookings;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
                string query = "SELECT * FROM Booking";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new Booking
                            {
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
                                ContactNumber = reader["ContactNumber"].ToString()
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Database Error: " + ex.Message;
            Console.WriteLine("Error: " + ex.ToString());
        }

        return View(bookings);
    }

    public IActionResult EditBooking(int id)
    {
        Booking booking = null;

        using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bookings;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"))
        {
            con.Open();
            string query = "SELECT * FROM Booking WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        booking = new Booking
                        {
                            id = (int)reader["Id"],
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
                            ContactNumber = reader["ContactNumber"].ToString()
                        };
                    }
                }
            }
        }

        return View(booking);
    }

    [HttpPost]
    public IActionResult EditBooking(Booking booking)
    {
        using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bookings;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
        {
            con.Open();
            string query = @"UPDATE Booking SET 
                        Name = @Name, Address = @Address, CholiName = @CholiName,
                        CholiType = @CholiType, TopwearFabric = @TopwearFabric,
                        BottomwearFabric = @BottomwearFabric, DupattaType = @DupattaType,
                        RentalPrice = @RentalPrice, SetType = @SetType, RentalTime = @RentalTime,
                        SetSize = @SetSize, ContactNumber = @ContactNumber
                        WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", booking.id);
                cmd.Parameters.AddWithValue("@Name", booking.Name);
                cmd.Parameters.AddWithValue("@Address", booking.Address);
                cmd.Parameters.AddWithValue("@CholiName", booking.CholiName);
                cmd.Parameters.AddWithValue("@CholiType", booking.CholiType);
                cmd.Parameters.AddWithValue("@TopwearFabric", booking.TopwearFabric);
                cmd.Parameters.AddWithValue("@BottomwearFabric", booking.BottomwearFabric);
                cmd.Parameters.AddWithValue("@DupattaType", booking.DupattaType);
                cmd.Parameters.AddWithValue("@RentalPrice", booking.RentalPrice);
                cmd.Parameters.AddWithValue("@SetType", booking.SetType);
                cmd.Parameters.AddWithValue("@RentalTime", booking.RentalTime);
                cmd.Parameters.AddWithValue("@SetSize", booking.SetSize);
                cmd.Parameters.AddWithValue("@ContactNumber", booking.ContactNumber);

                cmd.ExecuteNonQuery();
            }
        }

        TempData["SuccessMessage"] = "Booking updated successfully!";
        return RedirectToAction("CustomerBooking");
    }

    public IActionResult DeleteBooking(int id)
    {
        using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bookings;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
        {
            con.Open();
            string query = "DELETE FROM Booking WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        TempData["SuccessMessage"] = "Booking deleted successfully!";
        return RedirectToAction("CustomerBooking");
    }

}
