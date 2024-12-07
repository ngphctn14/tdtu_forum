using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class SignupController : Controller
    {
        private ForumnManagerEntities7 _db = new ForumnManagerEntities7();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public static string hashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(user user_)
        {
            if (ModelState.IsValid)
            {
                var checkEmail = _db.users.FirstOrDefault(s => s.email == user_.email);
                var checkUsername = _db.users.FirstOrDefault(s => s.username == user_.username);

                user_.password = hashPassword(user_.password);

                Console.WriteLine(checkEmail == null);
                Console.WriteLine(checkUsername == null);
                
                if (checkEmail != null)
                {
                    ModelState.AddModelError("email", "Email already exists");
                    return View(user_);
                }

                if (checkUsername != null)
                {
                    ModelState.AddModelError("username", "Username already exists");
                    return View(user_);
                }

                user_.created_at = DateTime.Now;

                try
                {
                    var maxId = _db.Database.SqlQuery<int>("SELECT ISNULL(MAX(user_id), 0) FROM [user]").First();
                    user_.user_id = maxId + 1;

                    user_.created_at = DateTime.Now;

                    var sql = "INSERT INTO [user] (user_id, username, email, full_name, password, created_at) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)";
                    _db.Database.ExecuteSqlCommand(sql, 
                        user_.user_id, 
                        user_.username, 
                        user_.email, 
                        user_.full_name, 
                        user_.password,
                        user_.created_at);
                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error occurred while registering. Please try again.");
                    return View(user_);
                }
            }
            return View(user_);
        }
    }
}
