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
        ForumnManagerEntities2 _db = new ForumnManagerEntities2();

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
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult getNews()
        {
            var v = from t in _db.NEWS
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult getMostResponsesPost()
        {
           
            var postsWithMostResponses = _db.posts
                .OrderByDescending(p => p.comments.Count)
                .Take(5)
                .ToList();

            return PartialView(postsWithMostResponses);
        }
        public ActionResult getRecentPost()
        {
            var recentPosts = _db.posts
                .OrderByDescending(p => p.created_at)
                .Take(5)
                .ToList();

            return PartialView(recentPosts); 
        }
        public ActionResult getRecentlyAnswers()
        {
            var postsWithRecentComments = _db.posts
                .Where(p => p.comments.Any())
                .Select(p => new
                {
                    Post = p,
                    MostRecentCommentDate = p.comments
                        .OrderByDescending(c => c.commented_at)
                        .Select(c => c.commented_at)
                        .FirstOrDefault() 
                })
                .OrderByDescending(pc => pc.MostRecentCommentDate)
                .Take(5)
                .ToList();
            var posts = postsWithRecentComments.Select(pc => pc.Post).ToList();

            return PartialView(posts);
        }
        public ActionResult getNoAnswerPosts()
        {
            var noAnswerPosts = _db.posts
                .Where(p => !p.comments.Any())
                .OrderByDescending(p => p.created_at)
                .ToList();

            return PartialView(noAnswerPosts);
        }



    }
}