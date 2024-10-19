using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {

        ForumnManagerEntities6 _db = new ForumnManagerEntities6();

        // GET: User
        public ActionResult Index(int id)
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
    }
}