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

        ForumnManagerEntities2 _db = new ForumnManagerEntities2();

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryIndex(int category_id)
        {
            var category = _db.categories.FirstOrDefault(c => c.category_id == category_id);

            if (category == null) {
                return HttpNotFound();
            }

            return View(category);
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