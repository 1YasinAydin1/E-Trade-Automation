using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        EFCommerceEntities e = new EFCommerceEntities();
        public ActionResult Index()
        {
            var employeeCount = e.EMPLOYEE.Count();
            ViewBag.employeeCount = employeeCount;

            var cariCount = e.CARI.Count();
            ViewBag.cariCount = cariCount;

            var productCount = e.PRODUCT.Count();
            ViewBag.productCount = productCount;

            var categoryCount = e.CATEGORY.Count();
            ViewBag.categoryCount = categoryCount;

            var stockCount = e.PRODUCT.Sum(x => x.STOCK);
            ViewBag.stockCount = stockCount;

            var departmentCount = e.DEPARTMENT.Count();
            ViewBag.departmentCount = departmentCount;

            var bestStockName = e.PRODUCT.Where(x => x.STOCK == e.PRODUCT.Max(k => k.STOCK)).Select(o => o.NAME).First();
            var bestStockCount = e.PRODUCT.Max(k => k.STOCK);
            ViewBag.bestStock = bestStockName + " - " + bestStockCount;

            try
            {
                var bestProductID = e.SALESMOVEMENTLOWER.GroupBy(x => x.ProductID).OrderByDescending(z => z.Count()).Select(o => o.Key).FirstOrDefault();
                var bestSalesBrandCount = e.SALESMOVEMENTLOWER.Where(x => x.ProductID == bestProductID).Sum(x => x.Count);
                var bestBrandName = e.PRODUCT.Where(x => x.ID == bestProductID).Select(x => x.BRAND).First();
                ViewBag.bestBrand = bestBrandName + " - " + bestSalesBrandCount;
            }
            catch (Exception)
            {
                ViewBag.bestBrand = "-";
            }

            try {
                var bestProductID1 = e.SALESMOVEMENTLOWER.GroupBy(x => x.ProductID).OrderByDescending(z => z.Count()).Select(o => o.Key).FirstOrDefault();
                var bestProductName = e.PRODUCT.Where(x => x.ID == bestProductID1).Select(x => x.NAME).First();
                ViewBag.bestProduct = bestProductName;
            } catch (Exception){
                ViewBag.bestProduct = "-";
            }

            try
            {
                var bestEmployeeID = e.SALESMOVEMENT.GroupBy(x => x.EMPLOYEEID).OrderByDescending(z => z.Count()).Select(o => o.Key).FirstOrDefault();
            var bestEmployeeName = e.EMPLOYEE.Where(x => x.ID == bestEmployeeID).Select(x => x.NAME).First();
            ViewBag.bestEmployee = bestEmployeeName;
            }
            catch (Exception)
            {
                ViewBag.bestEmployee = "-";
            }
            try
            {
                var bestCariID = e.SALESMOVEMENT.GroupBy(x => x.CARIID).OrderByDescending(z => z.Count()).Select(o => o.Key).FirstOrDefault();
            var bestCariName = e.CARI.Where(x => x.ID == bestCariID).Select(x => x.NAME).First();
            ViewBag.bestCari = bestCariName;
            }
            catch (Exception)
            {
                ViewBag.bestCari = "-";
            }
            DateTime asf = DateTime.Today;
            double safePrice = 0;
            foreach (var item in e.SALESMOVEMENT)
            {
                DateTime itemDate = DateTime.Parse(item.DATE.ToString());
                if (itemDate.Day == DateTime.Today.Day)
                {
                    safePrice += (double)item.GENERALPRICE;
                }
            }
            ViewBag.safe = safePrice;
            ViewBag.progress = "50%";


            return View();
        }

        public ActionResult PartialDepartment() {

            var query = from x in e.EMPLOYEE
                        group x by x.DEPARTMENT1.NAME into g
                        select new partialDepartment
                        {
                            Department = g.Key,
                            ID = e.DEPARTMENT.Where(o => o.NAME == g.Key).Select(p => p.ID).FirstOrDefault(),
                            Count = g.Count()
                        };

            return Json(query, JsonRequestBehavior.AllowGet);
        }
   
    }
}