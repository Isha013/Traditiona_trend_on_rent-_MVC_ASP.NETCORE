using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.IO;
using Traditiona_trend_on_rent.Models;


namespace Traditiona_trend_on_rent.Controllers
{
    public class ProfileController : Controller
    {
        private readonly string conStr = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=UserProfile;Trusted_Connection=True";

        public IActionResult UserProfile()
        {
            UserProfile profile = new UserProfile();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM UserProfile", con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    profile.Id = (int)reader["Id"];
                    profile.Name = reader["Name"].ToString();
                    profile.Email = reader["Email"].ToString();
                    profile.MobileNumber = reader["MobileNumber"].ToString();
                    profile.Password = reader["Password"].ToString();
                    profile.ProfileImagePath = reader["ProfileImagePath"].ToString();
                }
            }

            return View(profile);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            return RedirectToAction("UserProfile");
        }

        [HttpPost]
        public IActionResult EditProfile(UserProfile model, IFormFile profileImage)
        {
            string imagePath = model.ProfileImagePath;

            if (profileImage != null && profileImage.Length > 0)
            {
                // Ensure folder exists
                string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                // Save image
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
                string fullPath = Path.Combine(imagesFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    profileImage.CopyTo(stream);
                }

                imagePath = "/images/" + fileName;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE UserProfile SET Name=@Name, Email=@Email, MobileNumber=@MobileNumber, Password=@Password, ProfileImagePath=@ProfileImagePath WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@ProfileImagePath", imagePath);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("UserProfile");
        }
    }
}
