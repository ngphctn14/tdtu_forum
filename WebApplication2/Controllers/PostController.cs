using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PostController : Controller
    {
        ForumnManagerEntities2 _db = new ForumnManagerEntities2();
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getPostByCategoryTop5(int category_id)
        {
            var v = (from post in _db.posts
                    join category in _db.categories on post.category_id equals category.category_id
                    where category.category_id == category_id
                    orderby post.created_at descending
                    select post).Take(5);
            return PartialView(v.ToList());
        }

        public ActionResult getPostByCategory(int category_id)
        {
            var v = from post in _db.posts
                    join category in _db.categories on post.category_id equals category.category_id
                    where category.category_id == category_id
                    select post;
            return PartialView(v.ToList());
        }
    }
}