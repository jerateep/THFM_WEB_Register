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
    public class RegistersController : Controller
    {
        private Entities db = new Entities();

        // GET: Registers
        public ActionResult Index()
        {
            var registers = db.Registers.Include(r => r.ClassroomDay1).Include(r => r.ClassroomDay2).Include(r => r.Login).Include(r => r.StatusPay);
            return View(registers.ToList());
        }

        // GET: Registers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // GET: Registers/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null)
            {
                int a = Convert.ToInt32(Session["UserID"]);
                var Name = db.Registers.Where(s => s.Login_Id == a).ToList();
                if (Name != null)
                {
                    ViewBag.Name = Name;
                    ViewBag.ClassroomDay1_Id = new SelectList(db.ClassroomDay1, "ClassroomDay1_Id", "ClassroomDay1_Name");
                    ViewBag.ClassroomDay2_Id = new SelectList(db.ClassroomDay2, "ClassroomDay2_Id", "ClassroomDay2_Name");
                    ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname");
                    ViewBag.StatusPay_Id = new SelectList(db.StatusPays, "StatusPay_Id", "StatusPay_Name");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "LoginFirstPage");
            }
        }

        // POST: Registers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Register_Id,Titlename,Firstname,Lastname,Position,Email,Tel,ClassroomDay1_Id,ClassroomDay2_Id,StatusPay_Id,DateRegister,DateChk,Login_Id")] Register register)
        {
            if (ModelState.IsValid)
            {
                db.Registers.Add(register);
                register.Login_Id = Convert.ToInt32(Session["UserID"]);
                register.StatusPay_Id = 1;
                db.SaveChanges();
                //Add Bill
                db.Bills.Add(new Bill()
                {
                    Login_Id = Convert.ToInt32(Session["UserID"]),
                    Register_Id = register.Register_Id,
                    Rate = 0,
                    Price = "3000",
                    chk = 0
                });
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.ClassroomDay1_Id = new SelectList(db.ClassroomDay1, "ClassroomDay1_Id", "ClassroomDay1_Name", register.ClassroomDay1_Id);
            ViewBag.ClassroomDay2_Id = new SelectList(db.ClassroomDay2, "ClassroomDay2_Id", "ClassroomDay2_Name", register.ClassroomDay2_Id);
            ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname", register.Login_Id);
            ViewBag.StatusPay_Id = new SelectList(db.StatusPays, "StatusPay_Id", "StatusPay_Name", register.StatusPay_Id);
            return View(register);
        }

        // GET: Registers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassroomDay1_Id = new SelectList(db.ClassroomDay1, "ClassroomDay1_Id", "ClassroomDay1_Name", register.ClassroomDay1_Id);
            ViewBag.ClassroomDay2_Id = new SelectList(db.ClassroomDay2, "ClassroomDay2_Id", "ClassroomDay2_Name", register.ClassroomDay2_Id);
            ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname", register.Login_Id);
            ViewBag.StatusPay_Id = new SelectList(db.StatusPays, "StatusPay_Id", "StatusPay_Name", register.StatusPay_Id);
            return View(register);
        }

        // POST: Registers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Register_Id,Titlename,Firstname,Lastname,Position,Email,Tel,ClassroomDay1_Id,ClassroomDay2_Id,StatusPay_Id,DateRegister,DateChk,Login_Id")] Register register)
        {
            if (ModelState.IsValid)
            {
                db.Entry(register).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassroomDay1_Id = new SelectList(db.ClassroomDay1, "ClassroomDay1_Id", "ClassroomDay1_Name", register.ClassroomDay1_Id);
            ViewBag.ClassroomDay2_Id = new SelectList(db.ClassroomDay2, "ClassroomDay2_Id", "ClassroomDay2_Name", register.ClassroomDay2_Id);
            ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname", register.Login_Id);
            ViewBag.StatusPay_Id = new SelectList(db.StatusPays, "StatusPay_Id", "StatusPay_Name", register.StatusPay_Id);
            return View(register);
        }

        // GET: Registers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // POST: Registers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Register register = db.Registers.Find(id);
            db.Registers.Remove(register);
            db.SaveChanges();
            return RedirectToAction("Create");
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
