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
    public class welcome_homeController : Controller
    {
        private ForumnManagerEntities7 db = new ForumnManagerEntities7();

        // GET: admin/welcome_home
        public ActionResult Index()
        {
            return View(db.welcome_home.ToList());
        }

        // GET: admin/welcome_home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            welcome_home welcome_home = db.welcome_home.Find(id);
            if (welcome_home == null)
            {
                return HttpNotFound();
            }
            return View(welcome_home);
        }

        // GET: admin/welcome_home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/welcome_home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "welcome_id,welcome_title,link")] welcome_home welcome_home)
        {
            if (ModelState.IsValid)
            {
                db.welcome_home.Add(welcome_home);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(welcome_home);
        }

        // GET: admin/welcome_home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            welcome_home welcome_home = db.welcome_home.Find(id);
            if (welcome_home == null)
            {
                return HttpNotFound();
            }
            return View(welcome_home);
        }

        // POST: admin/welcome_home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "welcome_id,welcome_title,link")] welcome_home welcome_home)
        {
            if (ModelState.IsValid)
            {
                db.Entry(welcome_home).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(welcome_home);
        }

        // GET: admin/welcome_home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            welcome_home welcome_home = db.welcome_home.Find(id);
            if (welcome_home == null)
            {
                return HttpNotFound();
            }
            return View(welcome_home);
        }

        // POST: admin/welcome_home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            welcome_home welcome_home = db.welcome_home.Find(id);
            db.welcome_home.Remove(welcome_home);
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
