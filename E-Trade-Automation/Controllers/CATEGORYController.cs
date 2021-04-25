using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using Rotativa.Options;
using Rotativa;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
     
    public class CATEGORYController : Controller
    {
        EFCommerceEntities e = new EFCommerceEntities();
        static List<CATEGORY> pdfList = new List<CATEGORY>();
        static bool isError = false;
        static bool swalIsDELETE, swalIsADD = false;
        static int pagenoo = 0;
        static int pageOptionss = 5;

        public ActionResult Index(int? pageNo, int? pageOptions)
        {
            if (Session["employeeID"] != null)
            {
                pagenoo = pageNo ?? pagenoo;
                int _pageNo = pagenoo;
                pageOptionss = pageNo ?? pageOptionss;
                int _pageOptions = pageOptionss;
                GetCategorys(_pageNo, _pageOptions);
                if (isError == true) ViewBag.ERROR = true;

                if (swalIsDELETE == true) ViewBag.swalFireDelete = true; swalIsDELETE = false;
                if (swalIsADD == true) ViewBag.swalFireAdd = true; swalIsADD = false;
                return View(pdfList);
            }
            else return Redirect("~/LOGIN/LOGIN");
        }

        public JsonResult GetCategorys(int? pageNo, int? pageOptions)
        {
            e.Configuration.ProxyCreationEnabled = false;
            pagenoo = pageNo ?? pagenoo;
            int _pageNo = pagenoo;
            pageOptionss = pageOptions ?? pageOptionss;
            int _pageOptions = pageOptionss;
            int pageKey = 1;
            var c = e.CATEGORY.AsQueryable();

            c = c.OrderBy(o => o.ID).Skip(_pageNo * _pageOptions).Take(_pageOptions);
            List<int> pagenationList = new List<int>();
            int countList = e.CATEGORY.Count();
            for (int i = 0; i < countList; i++)
                if (i % _pageOptions == 0) { pagenationList.Add(pageKey++); }
            ViewBag.pagenationList = pagenationList;
            pdfList = c.ToList();
            return Json(pdfList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategorysFilter(string name)
        {
            e.Configuration.ProxyCreationEnabled = false;
            var c = e.CATEGORY.Where(x => x.NAME.Contains(name)).ToList();
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        #region ADD
        public ActionResult CATEGORY_ADD()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CATEGORY_ADD(CATEGORY c, int CATEGORY_STATUS)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(c.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(fileName)) { ModelState.AddModelError("IMAGE", "Resim Seçiniz"); isValid = true; }
            if (isValid)
            {
                return View();
            }
            else
            {
                string path = "~/Image/" + fileName;
                Request.Files[0].SaveAs(Server.MapPath(path));
                c.IMAGE = fileName;
                c.STATUS = CATEGORY_STATUS.Equals(1) ? true : false;
                e.CATEGORY.Add(c);
                e.SaveChanges();
                swalIsADD = true;
                return RedirectToAction("Index");
            }
        }
        #endregion
        public ActionResult CATEGORY_EXPORT_PDF()
        {
            return View(pdfList);
        }
        public ActionResult CATEGORY_DELETE(int ID)
        {
            var c = e.CATEGORY.Find(ID);
            e.CATEGORY.Remove(c);
            try
            { e.SaveChanges(); isError = false; }
            catch (Exception) { isError = true; }
            swalIsDELETE = true;
            return RedirectToAction("Index");
        }

        #region UPDATE
        [HttpGet]
        public ActionResult CATEGORY_UPDATE(int ID)
        {
            var c = e.CATEGORY.Find(ID);
            return View(c);
        }
        [HttpPost]
        public ActionResult CATEGORY_UPDATE(CATEGORY g, int CATEGORY_STATUS)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(g.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (isValid)
                return View();
            else
            {
                var c = e.CATEGORY.Find(g.ID);
                if (!fileName.Equals(""))
                {
                    string path = "~/Image/" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    c.IMAGE = fileName;
                }
                c.NAME = g.NAME;
                c.STATUS = CATEGORY_STATUS.Equals(1) ? true : false;
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}