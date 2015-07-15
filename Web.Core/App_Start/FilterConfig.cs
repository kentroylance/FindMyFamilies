using System.Web;
using System.Web.Mvc;
using FindMyFamiles.Web.Extensions;
using FindMyFamilies.Web.Models;

namespace FindMyFamilies.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
	
}
