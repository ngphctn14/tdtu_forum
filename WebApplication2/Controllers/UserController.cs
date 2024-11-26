using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {

        ForumnManagerEntities6 _db = new ForumnManagerEntities6();

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
                            user.profile_picture = fileName;
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
    }
}