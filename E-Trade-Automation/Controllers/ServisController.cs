using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class ServisController : ApiController
    {
        EFCommerceEntities e = new EFCommerceEntities();
        static List<CATEGORY> pdfList = new List<CATEGORY>();
        public IEnumerable<CATEGORY> GetCategorysFilter(/*string name*/)
        {
            e.Configuration.ProxyCreationEnabled = false;
            //var c = e.CATEGORY.Where(x => x.NAME.Contains(name)).ToList();
            var c = e.CATEGORY.ToList();
            return c;
        }

        public IEnumerable<CATEGORY> GetCategorys(int? pageNo, int? pageOptions)
        {
            e.Configuration.ProxyCreationEnabled = false;
            int _pageNo = pageNo ?? 0;
            int _pageOptions = pageOptions ?? 5;
            int pageKey = 1;
            var c = e.CATEGORY.AsQueryable();

            c = c.OrderBy(o => o.ID).Skip(_pageNo * _pageOptions).Take(_pageOptions);
            List<int> pagenationList = new List<int>();
            for (int i = 0; i < e.CATEGORY.Count(); i++)
                if (i % _pageOptions == 0)
                    pagenationList.Add(pageKey++);
            //ViewBag.pagenationList = pagenationList;
            return c;
        }


    }
}
