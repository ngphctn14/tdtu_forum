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

        public ActionResult PostDetails(int id)
        {
            var post = _db.posts.Find(id);
            return View(post);
        }

        public ActionResult getPostByCategory(int category_id)
        {
            var v = from post in _db.posts
                    join category in _db.categories on post.category_id equals category.category_id
                    where category.category_id == category_id
                    select post;
            return PartialView(v.ToList());
        }

        public ActionResult getMostResponsesPost(int category_id)
        {
           
            var postsWithMostResponses = (from t in _db.posts
                                          where t.category_id == category_id
                                          select t)
                .OrderByDescending(p => p.comments.Count)
                .ToList();

            return PartialView(postsWithMostResponses);
        }
        public ActionResult getRecentPost(int category_id)
        {
            var recentPosts = (from t in _db.posts
                                  where t.category_id == category_id
                                  select t)
                .OrderByDescending(p => p.created_at)
                .ToList();

            return PartialView(recentPosts); 
        }
        public ActionResult getRecentlyAnswers(int category_id)
        {
            var postsWithRecentComments = (from t in _db.posts
                                  where t.category_id == category_id
                                  select t)
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
                .ToList();
            var posts = postsWithRecentComments.Select(pc => pc.Post).ToList();

            return PartialView(posts);
        }
        public ActionResult getNoAnswerPosts(int category_id)
        {
            var noAnswerPosts = (from t in _db.posts
                                  where t.category_id == category_id
                                  select t)
                .Where(p => !p.comments.Any())
                .OrderByDescending(p => p.created_at)
                .ToList();

            return PartialView(noAnswerPosts);
        }

        public ActionResult getPostsOfAUser(int user_id)
        {
            var v = from posts in _db.posts
                    where posts.user_id == user_id
                    orderby posts.created_at descending
                    select posts;
            return PartialView(v.ToList());
        }
    }
}