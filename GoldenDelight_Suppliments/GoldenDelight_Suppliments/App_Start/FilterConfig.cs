using System.Web;
using System.Web.Mvc;

namespace GoldenDelight_Suppliments
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
