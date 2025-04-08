using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

using System.IO;
using Traditiona_trend_on_rent.Models;

public class AddCollectionController : Controller
{
    private readonly string _connectionString;

    public AddCollectionController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Collections;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    // **FETCH COLLECTIONS**
    public IActionResult ManageCollection()
    {
        List<Collection> collections = new List<Collection>();

        try
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Collections";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    collections.Add(new Collection
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        CholiName = reader["CholiName"].ToString(),
                        CholiType = reader["CholiType"].ToString(),
                        TopwearFabric = reader["TopwearFabric"].ToString(),
                        BottomwearFabric = reader["BottomwearFabric"].ToString(),
                        DupattaType = reader["DupattaType"].ToString(),
                        RentalPrice = Convert.ToDecimal(reader["RentalPrice"]),
                        SetType = reader["SetType"].ToString(),
                        RentalDuration = reader["RentalDuration"].ToString(),
                        SetSize = reader["SetSize"].ToString(),
                        CholiImage = reader["CholiImage"].ToString()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // Log exception (implement a logging service if needed)
            Console.WriteLine(ex.Message);
        }

        return View(collections);
    }

    // **ADD COLLECTION**
    [HttpPost]
    public IActionResult Collection(Collection model, IFormFile CholiImageFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            string imagePath = null;
            if (CholiImageFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(CholiImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    CholiImageFile.CopyTo(fileStream);
                }

                imagePath = "/images/" + uniqueFileName;
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Collections 
                                (CholiName, CholiType, TopwearFabric, BottomwearFabric, DupattaType, 
                                RentalPrice, SetType, RentalDuration, SetSize, CholiImage) 
                                VALUES (@CholiName, @CholiType, @TopwearFabric, @BottomwearFabric, 
                                @DupattaType, @RentalPrice, @SetType, @RentalDuration, @SetSize, @CholiImage)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CholiName", model.CholiName);
                cmd.Parameters.AddWithValue("@CholiType", model.CholiType);
                cmd.Parameters.AddWithValue("@TopwearFabric", model.TopwearFabric);
                cmd.Parameters.AddWithValue("@BottomwearFabric", model.BottomwearFabric);
                cmd.Parameters.AddWithValue("@DupattaType", model.DupattaType);
                cmd.Parameters.AddWithValue("@RentalPrice", model.RentalPrice);
                cmd.Parameters.AddWithValue("@SetType", model.SetType);
                cmd.Parameters.AddWithValue("@RentalDuration", model.RentalDuration);
                cmd.Parameters.AddWithValue("@SetSize", model.SetSize);
                cmd.Parameters.AddWithValue("@CholiImage", (object)imagePath ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return RedirectToAction("ManageCollection");
    }

    // **EDIT COLLECTION**
    public IActionResult Edit(int id)
    {
        Collection collection = null;

        try
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Collections WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    collection = new Collection
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        CholiName = reader["CholiName"].ToString(),
                        CholiType = reader["CholiType"].ToString(),
                        TopwearFabric = reader["TopwearFabric"].ToString(),
                        BottomwearFabric = reader["BottomwearFabric"].ToString(),
                        DupattaType = reader["DupattaType"].ToString(),
                        RentalPrice = Convert.ToDecimal(reader["RentalPrice"]),
                        SetType = reader["SetType"].ToString(),
                        RentalDuration = reader["RentalDuration"].ToString(),
                        SetSize = reader["SetSize"].ToString(),
                        CholiImage = reader["CholiImage"].ToString()
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        if (collection == null)
        {
            return NotFound();
        }

        return View(collection);
    }

    [HttpPost]
    public IActionResult Edit(Collection model, IFormFile CholiImageFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            string imagePath = model.CholiImage;

            if (CholiImageFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + CholiImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    CholiImageFile.CopyTo(fileStream);
                }

                imagePath = "/images/" + uniqueFileName;
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Collections 
                                SET CholiName=@CholiName, CholiType=@CholiType, 
                                TopwearFabric=@TopwearFabric, BottomwearFabric=@BottomwearFabric, 
                                DupattaType=@DupattaType, RentalPrice=@RentalPrice, SetType=@SetType, 
                                RentalDuration=@RentalDuration, SetSize=@SetSize, CholiImage=@CholiImage 
                                WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@CholiName", model.CholiName);
                cmd.Parameters.AddWithValue("@CholiType", model.CholiType);
                cmd.Parameters.AddWithValue("@TopwearFabric", model.TopwearFabric);
                cmd.Parameters.AddWithValue("@BottomwearFabric", model.BottomwearFabric);
                cmd.Parameters.AddWithValue("@DupattaType", model.DupattaType);
                cmd.Parameters.AddWithValue("@RentalPrice", model.RentalPrice);
                cmd.Parameters.AddWithValue("@SetType", model.SetType);
                cmd.Parameters.AddWithValue("@RentalDuration", model.RentalDuration);
                cmd.Parameters.AddWithValue("@SetSize", model.SetSize);
                cmd.Parameters.AddWithValue("@CholiImage", (object)imagePath ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return RedirectToAction("ManageCollection");
    }
}
