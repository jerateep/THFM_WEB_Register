using PagedList;
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
    public class ReportsController : Controller
    {
        private Entities db = new Entities();

        // GET: Reports
        public ActionResult Index(string sortOrder, string currentFilter, string _searchString, int? page)
        {
            var searchString = _searchString;
            if(searchString == null)
            {
                if(Session["Email"] == null || searchString == "")
                {
                    searchString = "?";
                    Session.Clear();
                    //return RedirectToAction("Index", "Reports");
                }
                else
                {
                    searchString = Session["Email"].ToString();
                    Session.Clear();
                }
                
            }
            ViewBag.RegisterIdParm = String.IsNullOrEmpty(sortOrder) ? "RegId_asc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            // search
            var Report = from s in db.Reports select s;
            if (!String.IsNullOrEmpty(searchString) || searchString != "" || searchString != null)
            {
                Report = Report.Where(s =>
                s.Firstname == searchString
                ||
                s.Lastname == searchString
                ||
                s.l_Email == searchString
                ||
                s.Email == searchString
                ||
                s.Tel == searchString
                );

            }
            // search end
            switch (sortOrder)
            {
                case "RegId_asc":
                    Report = Report.OrderBy(s => s.Register_Id);
                    break;
            }
            //sorting end
            //int pageSize = 15;
            //int pageNumber = (page ?? 1);      
            return View(Report.ToList());
        }

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Register_Id,Titlename,Firstname,Lastname,Position,Email,Phone,ClassroomDay1_Name,ClassroomDay2_Name,DateRegister,CompanyType_Name,CompanyName,Contact_Firstname,Contact_Lastname,Tel,Fax,Tex,Rate,Price,Address_1,Township_1,Amphoe_1,Province_1,ZipCode_1,AddressSend,TownshipSend,AmphoeSend,ProvinceSend,ZipCodeSend,BillType_Name,l_Firstname,l_Lastname,l_Email,StatusPay_Name")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Register_Id,Titlename,Firstname,Lastname,Position,Email,Phone,ClassroomDay1_Name,ClassroomDay2_Name,DateRegister,CompanyType_Name,CompanyName,Contact_Firstname,Contact_Lastname,Tel,Fax,Tex,Rate,Price,Address_1,Township_1,Amphoe_1,Province_1,ZipCode_1,AddressSend,TownshipSend,AmphoeSend,ProvinceSend,ZipCodeSend,BillType_Name,l_Firstname,l_Lastname,l_Email,StatusPay_Name")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
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
