using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class CARIPROFILEController : Controller
    {

        EFCommerceEntities e = new EFCommerceEntities();
        static CARI cari;
        // GET: CARIPROFILE
        public ActionResult Index()
        {
            if (Session["CariID"] != null)
            {
                int ID = int.Parse(Session["CariID"].ToString());
                var c = e.CARI.Where(o => o.ID == ID).First();
                cari = c;
                ViewBag.CARIPROFILENAME = c.NAME;
                return View();
            }
            else return Redirect("~/LOGIN/LOGIN");
        }

        public ActionResult MESSAGE()
        {
            return View();
        }

        public JsonResult RECEIVERMESSAGE()
        {
            List <MESSAGE> m = e.MESSAGE.Where(o=>o.RECEIVERMAIL == cari.MAIL).ToList();
            return Json(m, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SENDERMESSAGE()
        {
            var m = e.MESSAGE.Where(o => o.SENDERMAIL == cari.MAIL).ToList();
            return Json(m, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MESSAGEDETAIL(int ID)
        {
            var m = e.MESSAGE.Where(o => o.ID == ID).FirstOrDefault();
            return Json(m, JsonRequestBehavior.AllowGet);
        }
        public void MESSAGESEND(MESSAGE s)
        {
            s.SENDERMAIL = cari.MAIL;
            s.DATE = DateTime.Now;
            e.MESSAGE.Add(s);
            e.SaveChanges();
        }
        public ActionResult MESSAGEewq()
        {
            return View();
        }
    }
}