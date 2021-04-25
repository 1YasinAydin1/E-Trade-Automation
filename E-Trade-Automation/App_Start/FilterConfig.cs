using System.Web;
using System.Web.Mvc;

namespace Asp.NET_E_Commerce_MVC5_ENTITY_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
