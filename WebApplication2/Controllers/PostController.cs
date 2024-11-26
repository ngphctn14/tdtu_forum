using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PostController : Controller
    {
        ForumnManagerEntities7 _db = new ForumnManagerEntities7();
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

        public ActionResult get15RecentPosts()
        {
            var v = (from posts in _db.posts
                    orderby posts.created_at descending
                    select posts).Take(15);
            return PartialView(v.ToList());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreatePost(string title, string content, int category_id, IEnumerable<HttpPostedFileBase> attachments)
        {
            if (ModelState.IsValid)
            {
                post post = new post();
                post.user_id = (int)Session["Id"];
                post.title = title;
                post.content = content;
                post.category_id = category_id;
                post.created_at = DateTime.Now;

                try
                {
                    var maxId = _db.Database.SqlQuery<int>("SELECT ISNULL(MAX(post_id), 0) FROM [post]").First();
                    post.post_id = maxId + 1;

                    var sql = "INSERT INTO [post] (post_id, title, content, created_at, user_id, category_id) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)";
                    _db.Database.ExecuteSqlCommand(sql, 
                        post.post_id,
                        post.title,
                        post.content,
                        post.created_at,
                        post.user_id,
                        post.category_id);

                    if (attachments != null)
                    {
                        var savedFiles = FileHelper.SavePostFiles(post.post_id, attachments);
                        
                        foreach (var fileName in savedFiles)
                        {
                            var maxAttachId = _db.Database.SqlQuery<int>("SELECT ISNULL(MAX(attachment_id), 0) FROM [attachment]").First();
                            var attachSql = @"INSERT INTO [attachment] (attachment_id, post_id, file_name) 
                                            VALUES (@p0, @p1, @p2)";
                            
                            _db.Database.ExecuteSqlCommand(attachSql,
                                maxAttachId + 1,
                                post.post_id,
                                fileName);
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error occurred while creating a post. Please try again.");
                    return View(post);
                }
            }
            return View();
        }
        
        public FileResult DownloadAttachment(int id)
        {
            var attachment = _db.attachments.Find(id);
            if (attachment == null)
                throw new HttpException(404, "File not found");

            string filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "Uploads", 
                "Posts", 
                attachment.post_id.ToString(), 
                attachment.file_name);

            return File(filePath, "application/octet-stream", attachment.file_name);
        }

        [HttpPost]
        public ActionResult LikePost(int post_id)
        {
            int currentUserId = Convert.ToInt32(Session["Id"]);

            var post = _db.posts.Find(post_id);
            var user = _db.users.Find(currentUserId);

            var existingLike = post.users.Contains(user);

            bool isLiked;

            if (!existingLike)
            {
                post.users.Add(user);

                isLiked = true;
            }
            else
            {
                post.users.Remove(user);
                isLiked = false;
            }

            _db.SaveChanges();

            int likeCount = post.users.Count;

            return Json(new
            {
                success = true,
                isLiked = isLiked,
                likeCount = likeCount
            });
        }
    }


}