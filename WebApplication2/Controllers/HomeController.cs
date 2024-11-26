using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Controllers;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        ForumnManagerEntities6 _db = new ForumnManagerEntities6();

        public ActionResult Index()
        {
            return View();
        }
            
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Co gi vo sql them bang welcome_home
        public ActionResult getHomeWelcome()
        {
            var v = from t in _db.welcome_home
                    where t.hide == true
                    select t;
            return PartialView(v.ToList());
        }
        //public ActionResult getNews()
        //{
        //    var v = from t in _db.NEWS
        //            select t;
        //    return PartialView(v.ToList());
        //}
    }
}