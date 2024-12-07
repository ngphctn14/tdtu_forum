using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Helpers;
using BCrypt.Net;
using WebApplication2.Models;
using System.Data.SqlClient;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {

        ForumnManagerEntities7 _db = new ForumnManagerEntities7();

        // GET: User
        public ActionResult UserInfo(int id)
        {
            var user = _db.users.Find(id);
            return View(user);
        }

        public ActionResult allUsers()
        {
            var v = from user in _db.users
                    select user;
            return View(v.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfilePicture(HttpPostedFileBase profilePicture)
        {
            int userId = Convert.ToInt32(Session["Id"]);
            if (ModelState.IsValid)
            {
                try
                {
                    if (profilePicture != null)
                    {
                        string fileName = FileHelper.SaveUserProfilePicture(userId, profilePicture);

                        var user = _db.users.Find(userId);
                        if (user != null)
                        {
                            var sql = "UPDATE [user] SET profile_picture = @p0 WHERE user_id = @p1";
                            _db.Database.ExecuteSqlCommand(sql, 
                                fileName,
                                user.user_id);
                            _db.SaveChanges();
                        }

                        return RedirectToAction("UserInfo", "User", new {id =  userId});
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("UserInfo", "User", new {id =  userId});
                }
            }
            return RedirectToAction("UserInfo", "User", new {id =  userId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUserDetails(string full_name, string email)
        {
            int userId = Convert.ToInt32(Session["Id"]);
            if (ModelState.IsValid)
            {
                var sql = "UPDATE [user] SET full_name = @p0, email = @p1 WHERE user_id = @p2";
                _db.Database.ExecuteSqlCommand(sql, 
                    full_name,
                    email,
                    userId);
            }
            return RedirectToAction("UserInfo", "User", new {id =  userId});
        }

        [HttpPost]
        public ActionResult ChangePassword(string newPassword)
        {
            int userId = Convert.ToInt32(Session["Id"]);

            string password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            if (ModelState.IsValid)
            {
                var sql = "UPDATE [user] SET password = @p0 WHERE user_id = @p1";
                _db.Database.ExecuteSqlCommand(sql, 
                    password,
                    userId);
            }
            return RedirectToAction("UserInfo", "User", new {id =  userId});
        }

        [HttpPost]
        public ActionResult VerifyCurrentPassword(string currentPassword)
        {
            if (Session["Username"] == null)
            {
                return Json(new { isValid = false });
            }

            var user = _db.users.FirstOrDefault(u => u.username == Session["Username"].ToString());

            if (user == null)
            {
                return Json(new { isValid = false });
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.password);

            return Json(new { isValid = isPasswordValid });
        }

        [HttpPost]
        public ActionResult ToggleFollow(int userId, string action)
        {
            try
            {
                int currentUserId = Convert.ToInt32(Session["Id"]);
                int targetId = Convert.ToInt32(userId);
                var currentUser = _db.users.Find(currentUserId);
                var targetUser = _db.users.Find(targetId);

                if (currentUser == null || targetUser == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                string sql;
                if (action.Equals("follow"))
                {
                    sql = "INSERT INTO user_followers (user_id, follower_id) VALUES (@userId, @followerId)";
                }
                else if (action.Equals("unfollow"))
                {
                    sql = "DELETE FROM user_followers WHERE user_id = @userId AND follower_id = @followerId";
                }
                else
                {
                    return Json(new { success = false, message = "Invalid action" });
                }

                using (var connection = _db.Database.Connection)
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.Parameters.Add(new SqlParameter("@userId", currentUserId));
                        command.Parameters.Add(new SqlParameter("@followerId", targetId));
                        command.ExecuteNonQuery();
                    }
                }

                int followersCount = _db.Database.SqlQuery<int>("SELECT COUNT(*) FROM user_followers WHERE user_id = @userId", new SqlParameter("@userId", targetId)).FirstOrDefault();

                return Json(new { success = true, isFollowing = (action.Equals("follow")), followersCount = followersCount });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in ToggleFollow: " + ex.Message);
                return Json(new { success = false, message = "An error occurred" });
            }
        }

    }

}