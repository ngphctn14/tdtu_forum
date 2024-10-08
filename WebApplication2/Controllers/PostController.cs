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
        ForumnManagerEntities _db = new ForumnManagerEntities();
        // GET: Post
        public ActionResult Index()
        {

            return View();
        }
    }
}