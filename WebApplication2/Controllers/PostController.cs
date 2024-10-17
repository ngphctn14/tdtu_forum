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
        ForumnManagerEntities6 _db = new ForumnManagerEntities6();
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