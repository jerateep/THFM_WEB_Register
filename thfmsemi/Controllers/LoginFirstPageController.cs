using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thfmsemi.Models;
namespace thfmsemi.Controllers
{
    public class LoginFirstPageController : Controller
    {
        // GET: LoginFirstPage
        public ActionResult Index()
        {
            using (Entities db = new Entities())
            {
                return View(db.Logins.ToList());
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login user)
        {
            using (Entities db = new Entities())
            {
                var usr = db.Logins.Where(u => u.l_Email == user.l_Email && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {
                    Session["UserID"] = usr.Login_Id.ToString();
                    Session["FirstName"] = usr.l_Firstname.ToString();
                    Session["LastName"] = usr.l_Lastname.ToString();
                    Session["Email"] = usr.l_Email.ToString();
                    return RedirectToAction("Create", "Registers");
                }
                else
                {
                    ModelState.AddModelError("", "อีเมล์ หรือ รหัสผ่าน ผิดพลาด.");
                }
            }
            return View();
        }
    }
}