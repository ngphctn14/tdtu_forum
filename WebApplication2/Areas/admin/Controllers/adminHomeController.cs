using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Areas.admin.Controllers
{
    public class adminHomeController : Controller
    {
        // GET: admin/adminHome
        public ActionResult Index()
        {
            return View();
        }
    }
}