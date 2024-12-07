using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CommentController : Controller
    {

        private ForumnManagerEntities7 _db = new ForumnManagerEntities7();

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult commentsOfAPost(int post_id)
        {
            var v = from comments in _db.comments
                    where comments.post_id == post_id
                    select comments;
            return PartialView(v.ToList());
        }

        [HttpPost]
        public ActionResult LikeComment(int comment_id)
        {
            int currentUserId = Convert.ToInt32(Session["Id"]);

            var comment = _db.comments.Find(comment_id);
            var user = _db.users.Find(currentUserId);

            var existingLike = comment.users.Contains(user);

            bool isLiked;

            if (!existingLike)
            {
                comment.users.Add(user);

                isLiked = true;
            }
            else
            {
                comment.users.Remove(user);

                isLiked = false;
            }

            _db.SaveChanges();

            int likeCount = comment.users.Count;

            return Json(new
            {
                success = true,
                isLiked = isLiked,
                likeCount = likeCount
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult PostComment(string comment_content, int post_id)
        {
            if (ModelState.IsValid)
            {

                comment comment = new comment();
                comment.content = comment_content;
                comment.user_id = (int)Session["Id"];
                comment.post_id = post_id;
                comment.commented_at = DateTime.Now;
                try
                {
                    var maxId = _db.Database.SqlQuery<int>("SELECT ISNULL(MAX(comment_id), 0) FROM [comment]").First();
                    comment.comment_id = maxId + 1;

                    var sql = "INSERT INTO [comment] (comment_id, content, post_id, user_id, commented_at) VALUES (@p0, @p1, @p2, @p3, @p4)";
                    _db.Database.ExecuteSqlCommand(sql,
                        comment.comment_id,
                        comment.content,
                        comment.post_id,
                        comment.user_id,
                        comment.commented_at);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error occurred while posting a comment. Please try again.");
                    return View(comment);
                }
            }
            return RedirectToAction("PostDetails", "Post", new { id = post_id });
        }

    }
}