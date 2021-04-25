using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System.IO;
using Newtonsoft.Json;
using System.Collections;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class SALESMOVEMENTController : Controller
    {
        // GET: SALESMOVEMENT
        EFCommerceEntities e = new EFCommerceEntities();
        public ActionResult Index()
        {
            var s = e.SALESMOVEMENT.AsQueryable();
            var sAll = e.SALESMOVEMENT.ToList();

            return View(s.ToList());
        }
        //static partialProduct partialP = new  
        //public PartialViewResult PartialProduct()
        //{

        //    var query = from x in e.SALESMOVEMENT
        //                select new partialProduct
        //                {
        //                    ProductName = x.PRODUCT.NAME,
        //                    Count = (int)x.COUNT
        //                };

        //    return PartialView(query.ToList());
        //}

        public JsonResult getSalesMovement()
        {

            var s = e.SALESMOVEMENT.AsQueryable();
            var sList = e.SALESMOVEMENT.ToList();
            return Json(sList, JsonRequestBehavior.AllowGet);
        }


        #region ADD
        public ActionResult SALESMOVEMENT_ADD()
        {
            dropdownFill();
            return View();
        }

        private void dropdownFill()
        {

            List<SelectListItem> EMPLOYEELİST = (from x in e.EMPLOYEE.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.NAME,
                                                     Value = x.ID.ToString()
                                                 }).ToList();
            ViewBag.EMPLOYEELİST = EMPLOYEELİST;
            List<SelectListItem> CARILİST = (from x in e.CARI.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.NAME,
                                                 Value = x.ID.ToString()
                                             }).ToList();
            ViewBag.CARILİST = CARILİST;
            List<SelectListItem> PRODUCTLİST = (from x in e.PRODUCT.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = x.NAME,
                                                    Value = x.ID.ToString(),
                                                }).ToList();
            ViewBag.PRODUCTLİST = PRODUCTLİST;
        }

        public JsonResult GetProductPrice(int ID)
        {
            e.Configuration.ProxyCreationEnabled = false;
            var c = e.PRODUCT.Where(x => x.ID == ID).Select(x => x.SALESPRICE);
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        static SALESMOVEMENT Sales = new SALESMOVEMENT();
        static List<SALESMOVEMENTLOWER> ListSalesLower = new List<SALESMOVEMENTLOWER>();
        public JsonResult appendGetProduct(int productID, int count, int employeeID, int cariID)
        {

            SALESMOVEMENTLOWER sl = new SALESMOVEMENTLOWER();

            var stock = e.PRODUCT.Where(x => x.ID == productID).Select(x => x.STOCK).First();
            if (stock - count >= 0)
            {
                Sales.CARIID = cariID;
                Sales.DATE = DateTime.Now;
                Sales.EMPLOYEEID = employeeID;
                Sales.GENERALPRICE = float.Parse(45.ToString());

                sl.Count = count;
                sl.ProductID = productID;
                string productName = e.PRODUCT.Where(x => x.ID == productID).Select(x => x.NAME).First();
                sl.Name = productName;
                var price = e.PRODUCT.Where(x => x.ID == sl.ProductID).Select(x => x.SALESPRICE).First();
                sl.Price = price;
                sl.GeneralPrice = sl.Count * sl.Price;

                ListSalesLower.Add(sl);
            }
            else
            {
                sl.Count = count;
                sl.Price = stock;
            }
            return Json(sl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SALESMOVEMENT_ADD(SALESMOVEMENT a)
        {
            Sales.SALESMOVEMENTLOWER = null;
            e.SALESMOVEMENT.Add(Sales);
            e.SaveChanges();

            var SalesKey = e.SALESMOVEMENT.OrderByDescending(x => x.ID).Select(o=>o.ID).First();
            foreach (var item in ListSalesLower)
            {
                item.SalesMovementID = SalesKey;
                e.SALESMOVEMENTLOWER.Add(item);
                e.SaveChanges();
            }
            ListSalesLower.Clear();
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult SALESMOVEMENTLOWER(int ID) {

            var sAll = e.SALESMOVEMENTLOWER.Where(x=>x.SalesMovementID == ID).ToList();

            return View(sAll.ToList());
        }


        public ActionResult SALESMOVEMENT_DELETE(int ID)
        {
            var c = e.SALESMOVEMENT.Find(ID);
            e.SALESMOVEMENT.Remove(c);
            e.SaveChanges();
            return RedirectToAction("Index");
        }

        #region UPDATE
        [HttpGet]
        public ActionResult SALESMOVEMENT_UPDATE(int ID)
        {
            var c = e.SALESMOVEMENT.Find(ID);
            dropdownFill();
            return View(c);
        }
        [HttpPost]
        public ActionResult SALESMOVEMENT_UPDATE(SALESMOVEMENT g)
        {
            //bool isValid = false;
            //if (string.IsNullOrEmpty(g.PRICE.ToString())) { ModelState.AddModelError("PRICE", "Fiyatı Giriniz"); isValid = true; }
            //if (string.IsNullOrEmpty(g.COUNT.ToString())) { ModelState.AddModelError("COUNT", "Satış Adeti Giriniz"); isValid = true; }
            //if (isValid) { dropdownFill(); return View(); }
            //else
            //{
            //    var c = e.SALESMOVEMENT.Find(g.ID);
            //    c.CARIID = g.CARIID;
            //    c.PRODUCTID = g.PRODUCTID;
            //    c.EMPLOYEEID = g.EMPLOYEEID;
            //    c.COUNT = g.COUNT;
            //    c.PRICE = g.PRICE;
            //    c.GENERALPRICE = g.GENERALPRICE;
            //    e.SaveChanges();
            return RedirectToAction("Index");
            //}
        }
        #endregion
    }
}