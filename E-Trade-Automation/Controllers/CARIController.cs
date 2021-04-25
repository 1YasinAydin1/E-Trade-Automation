using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System.IO;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class CARIController : Controller
    {
        // GET: CARI
        EFCommerceEntities e = new EFCommerceEntities();
        public ActionResult Index()
        {
            if (Session["employeeID"] != null)
            {
                var c = e.CARI.AsQueryable();

                var cList = e.CARI;

                c = cList;
                return View(c.ToList());
            }
            else return Redirect("~/LOGIN/LOGIN");
        }

        #region ADD
        public ActionResult CARI_ADD()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CARI_ADD(CARI c)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(c.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(c.LASTNAME)) { ModelState.AddModelError("LASTNAME", "Soyadınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(c.CITY)) { ModelState.AddModelError("CITY", "Şehirinizi Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(c.MAIL)) { ModelState.AddModelError("MAIL", "Mail adresinizi Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(fileName)) { ModelState.AddModelError("IMAGE", "Resim Seçiniz"); isValid = true; }
            if (isValid)
                return View();
            else
            {
                string path = "~/Image/" + fileName;
                Request.Files[0].SaveAs(Server.MapPath(path));
                c.IMAGE = fileName;
                e.CARI.Add(c);
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion

        public ActionResult CARI_DELETE(int ID)
        {
            var c = e.CARI.Find(ID);
            e.CARI.Remove(c);
            e.SaveChanges();
            return RedirectToAction("Index");
        }

        #region UPDATE
        [HttpGet]
        public ActionResult CARI_UPDATE(int ID)
        {
            var c = e.CARI.Find(ID);
            return View(c);
        }
        [HttpPost]
        public ActionResult CARI_UPDATE(CARI g)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(g.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.LASTNAME)) { ModelState.AddModelError("LASTNAME", "Soyadınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.CITY)) { ModelState.AddModelError("CITY", "Şehirinizi Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.MAIL)) { ModelState.AddModelError("MAIL", "Mail adresinizi Giriniz"); isValid = true; }
            if (isValid)
                return View();
            else
            {
                var c = e.CARI.Find(g.ID);
                if (!fileName.Equals(""))
                {
                    string path = "~/Image/" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    c.IMAGE = fileName;
                }
                c.NAME = g.NAME;
                c.LASTNAME = g.LASTNAME;
                c.CITY = g.CITY;
                c.MAIL = g.MAIL;
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}