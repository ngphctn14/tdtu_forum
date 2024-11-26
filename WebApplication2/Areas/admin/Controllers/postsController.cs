using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Areas.admin.Controllers
{
    public class postsController : Controller
    {
        private ForumnManagerEntities7 db = new ForumnManagerEntities7();

        // GET: admin/posts
        public ActionResult Index(int? id = null)
        {
            getCategory(id);  // Gọi phương thức getCategory để hiển thị danh mục trong dropdown

            var posts = db.posts.Include(p => p.category).Include(p => p.user);
            return View();
        }
        public void getCategory(int? selectedId = null)
        {
            // Lấy danh sách các danh mục không bị ẩn và sắp xếp theo tên
            ViewBag.Category = new SelectList(db.categories.OrderBy(x => x.name), "category_id", "name", selectedId);
        }

        public ActionResult getPost(int? id)
        {
            if(id == null)
            {
                var v = db.posts.OrderBy(x => x.post_id).ToList();
                return PartialView(v);
            }
            var m = db.posts.Where(x => x.category_id == id).OrderBy(x => x.post_id).ToList();
            return PartialView(m);
        }

        // GET: admin/posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: admin/posts/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name");
            ViewBag.user_id = new SelectList(db.users, "user_id", "full_name");
            return View();
        }

        // POST: admin/posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "title,created_at,content,user_id,category_id")] post post)
        {
            if (ModelState.IsValid)
            {
                // Lấy giá trị post_id cao nhất hiện tại từ cơ sở dữ liệu
                int newPostId = db.posts.Any() ? db.posts.Max(p => p.post_id) + 1 : 1;

                // Gán giá trị post_id tự động
                post.post_id = newPostId;

                // Gán giá trị created_at là ngày hiện tại
                post.created_at = DateTime.Now;

                // Thêm bài đăng vào cơ sở dữ liệu
                db.posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu không hợp lệ, trả lại dữ liệu cho view
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", post.category_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "full_name", post.user_id);
            return View(post);
        }


        // GET: admin/posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", post.category_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "full_name", post.user_id);
            return View(post);
        }

        // POST: admin/posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "post_id,title,created_at,content,user_id,category_id")] post post)
        {
            try
            {
                post temp = getById(post.post_id);
                if (ModelState.IsValid)
                {
                    //db.Entry(post).State = System.Data.Entity.EntityState.Modified;
                    //db.SaveChanges();
                    //return RedirectToAction("Index");

                    temp.title = post.title;
                    temp.content = post.content;
                    temp.category_id = post.category_id;
                    temp.user_id = post.user_id;
                    db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", post.category_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "full_name", post.user_id);
            return View(post);
        }
        public post getById(int id){
            return db.posts.Where(x => x.post_id == id).FirstOrDefault();
        }

        // GET: admin/posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: admin/posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            post post = db.posts.Find(id);
            db.posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
