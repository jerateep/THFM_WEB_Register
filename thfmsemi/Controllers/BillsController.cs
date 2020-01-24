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
    public class BillsController : Controller
    {
        private Entities db = new Entities();

        // GET: Bills
        public ActionResult Index()
        {
            var bills = db.Bills.Include(b => b.BillType).Include(b => b.CompanyType).Include(b => b.Login).Include(b => b.Rate1).Include(b => b.Register);
            return View(bills.ToList());
        }

        // GET: Bills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bills/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BillType_Id = new SelectList(db.BillTypes, "BillType_Id", "BillType_Name");
                ViewBag.CompanyType_Id = new SelectList(db.CompanyTypes, "CompanyType_Id", "CompanyType_Name");
                ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname");
                ViewBag.Rate = new SelectList(db.Rates, "Rate1", "Rate1");
                ViewBag.Register_Id = new SelectList(db.Registers, "Register_Id", "Titlename");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "LoginFirstPage");
            }
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Bill_Id,CompanyType_Id,CompanyName,Contact_Firstname,Contact_Lastname,Tel,Fax,Tex,Rate,Price,Address_1,Township_1,Amphoe_1,Province_1,ZipCode_1,AddressSend,TownshipSend,AmphoeSend,ProvinceSend,ZipCodeSend,BillType_Id,Register_Id,Login_Id,chk")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                //(ราคา * จำนวนคน) - ((ราคา * จำนวนคน) / 1.07) * อัตราภาษี)
                double _Rate = Convert.ToDouble(bill.Rate.ToString());
                var SumMoney = (3000 * 1) - ((((3000 * 1) / 1.07) * _Rate) / 100);
                string sql = @"UPDATE Bills SET 
                                                CompanyType_Id = {0},
                                                CompanyName = {1},
                                                Contact_Firstname = {2},
                                                Contact_Lastname = {3},
                                                Tel = {4},
                                                Fax = {5},
                                                Tex = {6},
                                                Rate = {7},
                                                Price = {8},
                                                Address_1 = {9},
                                                Township_1 = {10},
                                                Amphoe_1 = {11},
                                                Province_1 = {12},
                                                ZipCode_1 = {13},
                                                AddressSend = {14},
                                                TownshipSend = {15},
                                                AmphoeSend = {16},
                                                ProvinceSend = {17},
                                                ZipCodeSend = {18},
                                                BillType_Id = {19},
                                                chk = 1
                             WHERE Login_Id ={20} AND chk = 0";
                List<Object> sqlParamsList = new List<object>();
                sqlParamsList.Add(bill.CompanyType_Id);//0
                sqlParamsList.Add(bill.CompanyName);//1
                sqlParamsList.Add(bill.Contact_Firstname);//2
                sqlParamsList.Add(bill.Contact_Lastname);//3
                sqlParamsList.Add(bill.Tel);//4
                sqlParamsList.Add(bill.Fax);//5
                sqlParamsList.Add(bill.Tex);//6
                sqlParamsList.Add(bill.Rate);//7 
                sqlParamsList.Add(SumMoney);//8
                sqlParamsList.Add(bill.Address_1);//9
                sqlParamsList.Add(bill.Township_1);//10
                sqlParamsList.Add(bill.Amphoe_1);//11
                sqlParamsList.Add(bill.Province_1);//12
                sqlParamsList.Add(bill.ZipCode_1);//13
                sqlParamsList.Add(bill.AddressSend);//14
                sqlParamsList.Add(bill.TownshipSend);//15
                sqlParamsList.Add(bill.AmphoeSend);//16
                sqlParamsList.Add(bill.ProvinceSend);//17
                sqlParamsList.Add(bill.ZipCodeSend);//18
                sqlParamsList.Add(bill.BillType_Id);//19
                sqlParamsList.Add(Convert.ToInt32(Session["UserID"]));//20
                db.Database.ExecuteSqlCommand(sql, sqlParamsList.ToArray());
                //db.Bills.Add(bill);
                //db.SaveChanges();
                int a = Convert.ToInt32(Session["UserID"]);
                //var ds = (from c in db.Registers
                //         where c.Login_Id == a && c.StatusPay_Id == 1
                //         select c).FirstOrDefault();
                //if(ds != null)
                //{
                //    ds.StatusPay_Id = 2;

                //}
                //db.SaveChanges();
                string sql2 = @"UPDATE Register SET StatusPay_Id = 2 WHERE Login_Id ={0} AND StatusPay_Id = 1";
                List<Object> sqlParamsList2 = new List<object>();
                sqlParamsList2.Add(a);//0
                db.Database.ExecuteSqlCommand(sql2, sqlParamsList2.ToArray());
                return RedirectToAction("Index", "Reports");
            }

            ViewBag.BillType_Id = new SelectList(db.BillTypes, "BillType_Id", "BillType_Name", bill.BillType_Id);
            ViewBag.CompanyType_Id = new SelectList(db.CompanyTypes, "CompanyType_Id", "CompanyType_Name", bill.CompanyType_Id);
            ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname", bill.Login_Id);
            ViewBag.Rate = new SelectList(db.Rates, "Rate1", "Rate1", bill.Rate);
            ViewBag.Register_Id = new SelectList(db.Registers, "Register_Id", "Titlename", bill.Register_Id);
            return View(bill);
        }

        // GET: Bills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillType_Id = new SelectList(db.BillTypes, "BillType_Id", "BillType_Name", bill.BillType_Id);
            ViewBag.CompanyType_Id = new SelectList(db.CompanyTypes, "CompanyType_Id", "CompanyType_Name", bill.CompanyType_Id);
            ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname", bill.Login_Id);
            ViewBag.Rate = new SelectList(db.Rates, "Rate1", "Rate1", bill.Rate);
            ViewBag.Register_Id = new SelectList(db.Registers, "Register_Id", "Titlename", bill.Register_Id);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Bill_Id,CompanyType_Id,CompanyName,Contact_Firstname,Contact_Lastname,Tel,Fax,Tex,Rate,Price,Address_1,Township_1,Amphoe_1,Province_1,ZipCode_1,AddressSend,TownshipSend,AmphoeSend,ProvinceSend,ZipCodeSend,BillType_Id,Register_Id,Login_Id")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillType_Id = new SelectList(db.BillTypes, "BillType_Id", "BillType_Name", bill.BillType_Id);
            ViewBag.CompanyType_Id = new SelectList(db.CompanyTypes, "CompanyType_Id", "CompanyType_Name", bill.CompanyType_Id);
            ViewBag.Login_Id = new SelectList(db.Logins, "Login_Id", "l_Firstname", bill.Login_Id);
            ViewBag.Rate = new SelectList(db.Rates, "Rate1", "Rate1", bill.Rate);
            ViewBag.Register_Id = new SelectList(db.Registers, "Register_Id", "Titlename", bill.Register_Id);
            return View(bill);
        }

        // GET: Bills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
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
