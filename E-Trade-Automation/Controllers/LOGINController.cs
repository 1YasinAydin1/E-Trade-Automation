using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System.Web.Security;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class LOGINController : Controller
    {
        EFCommerceEntities e = new EFCommerceEntities();

        [HttpGet]
        public ActionResult LOGIN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LOGIN(string NAMEC, string PASSWORDC, string NAMEE, string PASSWORDE)
        {
            if (PASSWORDE.Length == 0)
            {
                var cari = e.CARILOGIN.Where(o => o.NAME == NAMEC && o.PASSWORD == PASSWORDC).FirstOrDefault();
                if (cari != null) {
                    Session["CariID"] = cari.ID;
                    return Redirect("~/CARIPROFILE/Index");
                }
                else
                    return View();
            }
            else
            {
                var employee = e.EMPLOYEELOGIN.Where(o => o.USERNAME == NAMEE && o.PASSWORD == PASSWORDE).FirstOrDefault();
                if (employee != null) {
                    Session["employeeID"] = employee.ID;
                    return Redirect("~/CATEGORY/Index");
                }
                else
                    return View();
            }

        }
    }
}