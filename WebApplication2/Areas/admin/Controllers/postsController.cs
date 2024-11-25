using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Areas.admin.Controllers
{
    public class postsController : Controller
    {
        private ForumnManagerEntities6 db = new ForumnManagerEntities6();

        // GET: admin/posts
        public ActionResult Index()
        {
            var posts = db.posts.Include(p => p.category).Include(p => p.user);
            return View(posts.ToList());
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
        public ActionResult Create([Bind(Include = "post_id,title,created_at,content,user_id,category_id")] post post)
        {
            if (ModelState.IsValid)
            {

                post.created_at = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
        public ActionResult Edit([Bind(Include = "post_id,title,created_at,content,user_id,category_id")] post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", post.category_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "full_name", post.user_id);
            return View(post);
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
