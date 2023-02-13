using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using thfmsemi.Models;

namespace thfmsemi.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Form(string ReceiverEmail)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var MailPass = (from i in db.Logins
                                    where i.l_Email == ReceiverEmail
                                    select i.Password).First();
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("", " Admin THFM");
                        var receiverEmail = new MailAddress(ReceiverEmail, "Receiver");

                        var password = "";
                        //var sub = Subject;
                        var body = "Username : " + receiverEmail.Address + "\n" +
                                   "Password : " + MailPass;
                        var smtp = new SmtpClient
                        {
                            Host = "",
                            Port = 587,
                            //EnableSsl = true,
                            //DeliveryMethod = SmtpDeliveryMethod.Network,
                            //UseDefaultCredentials = tr,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };

                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = "Your Password (THFM)",
                            Body = body
                        }
                        )
                        {
                            smtp.Send(mess);
                        }
                        ViewBag.Pass = "ส่งรหัสผ่านไปที่อีเมล์ของคุณแล้ว กรุณาตรวจสอบค่ะ.";
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "ไม่พบอีเมล์ในระบบค่ะ.";
                }
                return View();
            }
        }
    }
}
