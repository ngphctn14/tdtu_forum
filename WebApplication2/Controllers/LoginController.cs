using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {

        private ForumnManagerEntities7 _db = new ForumnManagerEntities7();

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public bool unhashPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _db.users.Where(s => s.username.Equals(username)).FirstOrDefault();
                if (user != null && unhashPassword(password, user.password))
                {
                    Session["Id"] = user.user_id;
                    Session["Username"] = user.username;

                    var sql = "UPDATE [user] SET last_login = @p0 WHERE user_id = @p1";
                    _db.Database.ExecuteSqlCommand(sql, 
                        DateTime.Now,
                        user.user_id);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult loginPartialView()
        {
            return PartialView();
        }
    }
}