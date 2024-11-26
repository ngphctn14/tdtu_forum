using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CategoryController : Controller
    {

        private ForumnManagerEntities7 _db = new ForumnManagerEntities7();

        // GET: Category
        public ActionResult Index(int id)
        {
            var postsInCategory = _db.posts.Where(p => p.category_id == id).ToList();

            var category = _db.categories.Find(id);

            ViewBag.CategoryName = category?.name;
            ViewBag.Id = category?.category_id;

            return View(postsInCategory);
        }

        public ActionResult getPostsForEachCategory()
        {
            var v = from t in _db.categories
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getDetails()
        {
            var v = from t in _db.categories
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getDetailsSelect()
        {
            var v = from t in _db.categories
                    select t;
            return PartialView(v.ToList());
        }
    }
}