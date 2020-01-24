using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using thfmsemi.Models;

namespace thfmsemi.Controllers
{
    public class LoginsController : Controller
    {
        private Entities db = new Entities();

        // GET: Logins
        public ActionResult Index()
        {
            return View(db.Logins.ToList());
        }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Login_Id,l_Firstname,l_Lastname,l_Email,Password,ConfirmPassword")] Login login)
        {
            var mail = db.Logins.Where(u => u.l_Email == login.l_Email).FirstOrDefault();
            if (mail == null)
            {
                if (ModelState.IsValid)
                {
                    db.Logins.Add(login);
                    db.SaveChanges();
                    Session["Email"] = login.l_Email.ToString();
                    Session["UserID"] = login.Login_Id.ToString();
                    Session["FirstName"] = login.l_Firstname.ToString();
                    Session["LastName"] = login.l_Lastname.ToString();
                    //var Reg = new RegistersController();
                    //Reg._UserId = Session["UserID"].ToString();
                    return RedirectToAction("Create", "Registers");
                }
            }
            else
            {
                ModelState.AddModelError("", "ไม่สามารถใช้อีเมล์นี้ได้ มีอีเมล์นี้อยู่ในระบบแล้ว");
            }
            return View(login);
        }

        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Login_Id,l_Firstname,l_Lastname,l_Email,Password,ConfirmPassword")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(login);
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Login login = db.Logins.Find(id);
            db.Logins.Remove(login);
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
