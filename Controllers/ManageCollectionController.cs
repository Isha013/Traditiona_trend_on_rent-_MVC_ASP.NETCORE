using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Traditiona_trend_on_rent.Models; // Make sure this matches your namespace

namespace Traditiona_trend_on_rent.Controllers
{
    public class ManageCollectionController : Controller
    {
        private readonly string connectionString;

        public ManageCollectionController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ThirdConnection");
        }
      

        [HttpGet]
      

        public IActionResult Index()
        {
            List<ManageCollection> collections = new List<ManageCollection>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Collections";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    collections.Add(new ManageCollection
                    {
                        Id = (int)reader["Id"],
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
            return View(collections);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create(ManageCollection collection)
        {
            string imageName = null;

            if (collection.ImageFile != null)
            {
                // Set file name and path
                imageName = Guid.NewGuid().ToString() + Path.GetExtension(collection.ImageFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

                // Save the file
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await collection.ImageFile.CopyToAsync(stream);
                }
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Collections 
            (CholiName, CholiType, TopwearFabric, BottomwearFabric, DupattaType, RentalPrice, SetType, RentalDuration, SetSize, CholiImage)
            VALUES (@CholiName, @CholiType, @TopwearFabric, @BottomwearFabric, @DupattaType, @RentalPrice, @SetType, @RentalDuration, @SetSize, @CholiImage)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CholiName", collection.CholiName);
                cmd.Parameters.AddWithValue("@CholiType", collection.CholiType);
                cmd.Parameters.AddWithValue("@TopwearFabric", collection.TopwearFabric);
                cmd.Parameters.AddWithValue("@BottomwearFabric", collection.BottomwearFabric);
                cmd.Parameters.AddWithValue("@DupattaType", collection.DupattaType);
                cmd.Parameters.AddWithValue("@RentalPrice", collection.RentalPrice);
                cmd.Parameters.AddWithValue("@SetType", collection.SetType);
                cmd.Parameters.AddWithValue("@RentalDuration", collection.RentalDuration);
                cmd.Parameters.AddWithValue("@SetSize", collection.SetSize);
                cmd.Parameters.AddWithValue("@CholiImage", "/images/" + imageName); // Save the path

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ManageCollection collection = new ManageCollection();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Collections WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    collection.Id = (int)reader["Id"];
                    collection.CholiName = reader["CholiName"].ToString();
                    collection.CholiType = reader["CholiType"].ToString();
                    collection.TopwearFabric = reader["TopwearFabric"].ToString();
                    collection.BottomwearFabric = reader["BottomwearFabric"].ToString();
                    collection.DupattaType = reader["DupattaType"].ToString();
                    collection.RentalPrice = Convert.ToDecimal(reader["RentalPrice"]);
                    collection.SetType = reader["SetType"].ToString();
                    collection.RentalDuration = reader["RentalDuration"].ToString();
                    collection.SetSize = reader["SetSize"].ToString();
                    collection.CholiImage = reader["CholiImage"].ToString(); // ✅                }
                }
                return View(collection);
            }

        }
        [HttpPost]
        public IActionResult Edit(ManageCollection collection)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Collections SET 
                    CholiName=@CholiName, CholiType=@CholiType, TopwearFabric=@TopwearFabric, BottomwearFabric=@BottomwearFabric, 
                    DupattaType=@DupattaType, RentalPrice=@RentalPrice, SetType=@SetType, RentalDuration=@RentalDuration, 
                    SetSize=@SetSize, CholiImage=@CholiImage WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", collection.Id);
                cmd.Parameters.AddWithValue("@CholiName", collection.CholiName);
                cmd.Parameters.AddWithValue("@CholiType", collection.CholiType);
                cmd.Parameters.AddWithValue("@TopwearFabric", collection.TopwearFabric);
                cmd.Parameters.AddWithValue("@BottomwearFabric", collection.BottomwearFabric);
                cmd.Parameters.AddWithValue("@DupattaType", collection.DupattaType);
                cmd.Parameters.AddWithValue("@RentalPrice", collection.RentalPrice);
                cmd.Parameters.AddWithValue("@SetType", collection.SetType);
                cmd.Parameters.AddWithValue("@RentalDuration", collection.RentalDuration);
                cmd.Parameters.AddWithValue("@SetSize", collection.SetSize);
                cmd.Parameters.AddWithValue("@CholiImage", collection.CholiImage);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Collections WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
