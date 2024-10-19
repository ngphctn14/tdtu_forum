using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{


    public class CommentController : Controller
    {

        ForumnManagerEntities6 _db = new ForumnManagerEntities6();

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult commentsOfAPost(int post_id)
        {
            var v = from comments in _db.comments
                    where comments.post_id == post_id
                    select comments;
            return PartialView(v.ToList());
        }
    }
}