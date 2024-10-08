using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CategoryController : Controller
    {

        ForumnManagerEntities _db = new ForumnManagerEntities();

        // GET: Category
        public ActionResult Index()
        {
            return View();
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