using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System.IO;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class EMPLOYEEController : Controller
    {
        // GET: EMPLOYEE
        EFCommerceEntities e = new EFCommerceEntities();
        public ActionResult Index()
        {
            var em = e.EMPLOYEE.AsQueryable();

            var emAll = e.EMPLOYEE;

            em = emAll;
            return View(em.ToList());
        }

        #region ADD
        public ActionResult EMPLOYEE_ADD()
        {
            itemSelectedDEPARTMENT();
            return View();
        }

        private void itemSelectedDEPARTMENT() {
            ViewBag.DEPARTMENTLİST = (from x in e.DEPARTMENT.ToList()
                                                select new SelectListItem { Text = x.NAME, Value = x.ID.ToString() }).ToList();
        }

        [HttpPost]
        public ActionResult EMPLOYEE_ADD(EMPLOYEE c)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(c.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(c.LASTNAME)) { ModelState.AddModelError("LASTNAME", "Soyadınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(fileName)) { ModelState.AddModelError("IMAGE", "Resim Seçiniz"); isValid = true; }
            if (isValid) { itemSelectedDEPARTMENT(); 
                return View();
            }
            else
            {
                string path = "~/Image/" + fileName;
                Request.Files[0].SaveAs(Server.MapPath(path));
                c.IMAGE = fileName;
                e.EMPLOYEE.Add(c);
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion

        public ActionResult EMPLOYEE_DELETE(int ID)
        {
            var c = e.EMPLOYEE.Find(ID);
            e.EMPLOYEE.Remove(c);
            e.SaveChanges();
            return RedirectToAction("Index");
        }

        #region UPDATE
        [HttpGet]
        public ActionResult EMPLOYEE_UPDATE(int ID)
        {
            var c = e.EMPLOYEE.Find(ID);
            itemSelectedDEPARTMENT(); 
            return View(c);
        }
        [HttpPost]
        public ActionResult EMPLOYEE_UPDATE(EMPLOYEE g)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(g.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.LASTNAME)) { ModelState.AddModelError("LASTNAME", "Soyadınızı Giriniz"); isValid = true; }
            if (isValid){itemSelectedDEPARTMENT();return View();}
            else
            {
                var c = e.EMPLOYEE.Find(g.ID);
                if (!fileName.Equals(""))
                {
                    string path = "~/Image/" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    c.IMAGE = fileName;
                }
                c.NAME = g.NAME;
                c.LASTNAME = g.LASTNAME;
                c.DEPARTMENT = g.DEPARTMENT;
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}