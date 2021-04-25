using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System.IO;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class PRODUCTController : Controller
    {
        // GET: PRODUCT
        EFCommerceEntities e = new EFCommerceEntities();
        public ActionResult Index(int? CATEGORYID, string search, int? pageNo, int? pageOptions)
        {

            int _pageNo = pageNo ?? 0;
            int _pageOptions = pageOptions ?? 2;
            int pageKey = 1;
            var p = e.PRODUCT.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                p = p.Where(x => x.NAME.Contains(search));

            p = p.OrderBy(o => o.ID).Skip(_pageNo * _pageOptions).Take(_pageOptions);
            List<int> pagenationList = new List<int>();
            for (int i = 0; i < e.PRODUCT.Count(); i++)
                if (i % _pageOptions == 0)
                    pagenationList.Add(pageKey++);
            ViewBag.pagenationList = pagenationList;

            #region CATEGORY FILTER
            List<SelectListItem> LİST = new List<SelectListItem>();
            SelectListItem defaultKey = new SelectListItem { Text = "Seçiniz", Value = "0" };
            List<SelectListItem> categorys = (from x in e.CATEGORY.ToList()
                                              select new SelectListItem { Text = x.NAME, Value = x.ID.ToString() }).ToList();
            LİST.Add(defaultKey);
            LİST.AddRange(categorys);
            ViewBag.CATEGORYLİST = LİST;

            if (CATEGORYID != null && CATEGORYID != 0)
            {
                var filterp = e.PRODUCT.Where(o => o.CATEGORY == CATEGORYID);
                #region deneme
                //string name = e.PRODUCT.Where(o => o.CATEGORY == CATEGORYID)
                //    .Select(o => o.CATEGORY1.NAME).FirstOrDefault();
                //var id = e.PRODUCT.Where(o => o.CATEGORY == CATEGORYID)
                //     .Select(o => o.CATEGORY).FirstOrDefault();
                //LİST.Clear();
                //defaultKey.Text = name;
                //defaultKey.Value = id.ToString();
                //LİST.Add(defaultKey);
                //ViewBag.CATEGORYLİST = LİST;
                #endregion
                p = filterp;
            }
            #endregion
            return View(p.ToList());
        }

        [HttpGet]
        public ActionResult PRODUCT_ADD()
        {
            itemSelectedCategory();
            return View();
        }

        private void itemSelectedCategory()
        {
            List<SelectListItem> sLI = (from x in e.CATEGORY.ToList()
                                        select new SelectListItem
                                        {
                                            Text = x.NAME,
                                            Value = x.ID.ToString()
                                        }).ToList();
            ViewBag.ViewBagsLI = sLI;
        }
        [HttpPost]
        public ActionResult PRODUCT_ADD(PRODUCT p, int PRODUCT_STATUS)
        {
            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(p.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(p.BRAND)) { ModelState.AddModelError("BRAND", "Markayı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(p.STOCK.ToString())) { ModelState.AddModelError("STOCK", "Stok Sayısını Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(p.BUYPRICE.ToString())) { ModelState.AddModelError("BUYPRICE", "Alış Fiyatını Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(p.SALESPRICE.ToString())) { ModelState.AddModelError("SALESPRICE", "Satış Fiyatını Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(fileName)) { ModelState.AddModelError("IMAGE", "Resim Seçiniz"); isValid = true; }
            if (isValid)
            {
                itemSelectedCategory();
                return View();
            }
            else
            {
                if (Request.Files.Count > 0)
                {
                    string path = "~/Image/" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    p.IMAGE = fileName;
                }
                p.STATUS = PRODUCT_STATUS.Equals(1) ? true : false;
                e.PRODUCT.Add(p);
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult PRODUCT_DELETE(int ID)
        {
            var d = e.PRODUCT.Find(ID);
            e.PRODUCT.Remove(d);
            e.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult PRODUCT_UPDATE(int ID)
        {
            itemSelectedCategory();
            var g = e.PRODUCT.Find(ID);
            return View(g);
        }
        [HttpPost]
        public ActionResult PRODUCT_UPDATE(PRODUCT g, int PRODUCT_STATUS)
        {

            bool isValid = false;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            if (string.IsNullOrEmpty(g.NAME)) { ModelState.AddModelError("NAME", "Adınızı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.BRAND)) { ModelState.AddModelError("BRAND", "Markayı Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.STOCK.ToString())) { ModelState.AddModelError("STOCK", "Stok Sayısını Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.BUYPRICE.ToString())) { ModelState.AddModelError("BUYPRICE", "Alış Fiyatını Giriniz"); isValid = true; }
            if (string.IsNullOrEmpty(g.SALESPRICE.ToString())) { ModelState.AddModelError("SALESPRICE", "Satış Fiyatını Giriniz"); isValid = true; }
            if (isValid)
            {
                itemSelectedCategory();
                return View();
            }
            else
            {
                var p = e.PRODUCT.Find(g.ID);

                if (!fileName.Equals(""))
                {
                    string path = "~/Image/" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    p.IMAGE = fileName;
                }

                p.STATUS = PRODUCT_STATUS.Equals(1) ? true : false;
                p.NAME = g.NAME;
                p.BRAND = g.BRAND;
                p.BUYPRICE = g.BUYPRICE;
                p.SALESPRICE = g.SALESPRICE;
                p.STOCK = g.STOCK;
                p.CATEGORY = g.CATEGORY;
                p.STOCK = g.STOCK;
                e.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}