using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {

        private ForumnManagerEntities6 _db = new ForumnManagerEntities6();

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var _user = _db.users.Where(s => s.username.Equals(username) && s.password.Equals(password)).FirstOrDefault();
                if (_user != null)
                {
                    Session["Id"] = _user.user_id;
                    Session["Username"] = _user.username;
                   _user.last_login = DateTime.Now;
                    _db.SaveChanges();
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