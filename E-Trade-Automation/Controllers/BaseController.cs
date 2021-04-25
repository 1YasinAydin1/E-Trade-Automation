using Asp.NET_E_Commerce_MVC5_ENTITY_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Controllers
{
    public class BaseController : Controller
    {
       public EFCommerceEntities e = new EFCommerceEntities();
        // GET: Base
        public BaseController()
        {
            e.Configuration.ProxyCreationEnabled = false;
        }
    }
}