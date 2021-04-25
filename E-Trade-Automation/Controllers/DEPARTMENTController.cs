using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System.IO;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class DEPARTMENTController : Controller
    {
        // GET: Department
        EFCommerceEntities e = new EFCommerceEntities();
        public ActionResult Index()
        {
            var d = e.DEPARTMENT.ToList();
            return View(d);
        }
        [HttpGet]
        public ActionResult DEPARTMENT_ADD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DEPARTMENT_ADD(DEPARTMENT d)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(d.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(fileName)) { ModelState.AddModelError("IMAGE", "Resim Seçiniz"); isValid = true; }
            if (isValid)
                return View();
            else
            {
                if (Request.Files.Count > 0)
                {
                    string path = "~/Image/" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    d.IMAGE = fileName;
                }
                e.DEPARTMENT.Add(d);
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult DEPARTMENT_DELETE(int ID)
        {
            var d = e.DEPARTMENT.Find(ID);
            e.DEPARTMENT.Remove(d);
            e.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DEPARTMENT_UPDATE(int ID)
        {
            var d = e.DEPARTMENT.Find(ID);
            return View(d);
        }
        [HttpPost]
        public ActionResult DEPARTMENT_UPDATE(DEPARTMENT g)
        {
            var d = e.DEPARTMENT.Find(g.ID);
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(g.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (isValid)
                return View(); 
            else
            {
                if (!fileName.Equals(""))
                {
                    string path = "~/Image/" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    d.IMAGE = fileName;
                }
            }
            d.NAME = g.NAME;
            e.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DEPARTMENT_EMPLOYEE_DETAIL(int ID)
        {
            var employees = e.EMPLOYEE.Where(x => x.DEPARTMENT == ID).ToList();
            ViewBag.DNAME = e.DEPARTMENT.Where(o => o.ID == ID).Select(n => n.NAME).FirstOrDefault();
            return View(employees);
        }

        public ActionResult EMPLOYEE_SALESMOVEMENT_DETAIL(int ID)
        {
            var savemovements = e.SALESMOVEMENT.Where(x => x.EMPLOYEEID == ID).ToList();
            ViewBag.ENAME = e.EMPLOYEE.Where(o => o.ID == ID).Select(n => n.NAME).FirstOrDefault().ToUpper();
            return View(savemovements);
        }
    }
}