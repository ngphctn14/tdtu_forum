using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BCrypt.Net;
using WebApplication2.Models;

namespace WebApplication2.Areas.admin.Controllers
{
    public class usersController : Controller
    {
        private ForumnManagerEntities7 db = new ForumnManagerEntities7();

        // GET: admin/users
        public ActionResult Index()
        {
            return View(db.users.ToList());
        }

        // GET: admin/users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: admin/users/Create
        public ActionResult Create()
        {
            return View();
        }
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // POST: admin/users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,full_name,username,password,email,created_at,last_login")] user user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy giá trị user_id cao nhất hiện tại từ cơ sở dữ liệu
                    int newUserId = db.users.Any() ? db.users.Max(p => p.user_id) + 1 : 1;

                    // Gán giá trị user_id tự động
                    user.user_id = newUserId;

                    // Gán giá trị created_at là ngày hiện tại
                    user.created_at = DateTime.Now;
                    user.password = HashPassword(user.password);  // Băm mật khẩu

                    user.last_login = null;

                    db.users.Add(user);
                    db.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu
                    return RedirectToAction("Index");  // Điều hướng đến trang Index nếu thành công
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    // Tạo ngoại lệ mới để chứa thông tin chi tiết về lỗi
                    Exception raise = dbEx;

                    // Duyệt qua các lỗi validation của các thực thể
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // Tạo thông báo chi tiết về lỗi
                            string message = string.Format(
                                "Entity: {0}, Property: {1}, Error: {2}",
                                validationErrors.Entry.Entity.GetType().Name,  // Tên loại đối tượng
                                validationError.PropertyName,                    // Tên thuộc tính có lỗi
                                validationError.ErrorMessage                      // Thông báo lỗi
                            );

                            // Tạo một ngoại lệ mới và nest nó vào ngoại lệ hiện tại
                            raise = new InvalidOperationException(message, raise);
                        }
                    }

                    // Ném ngoại lệ mới có chứa thông tin chi tiết
                    throw raise;
                }
            }

            return View(user);  // Trả lại view với thông tin lỗi nếu ModelState không hợp lệ
        }



        // GET: admin/users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: admin/users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,full_name,username,password,email,created_at,last_login")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: admin/users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: admin/users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
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
