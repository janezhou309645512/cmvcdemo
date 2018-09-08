using System.Web.Mvc;

namespace cmvcdemo.Areas.logis
{
    public class logisAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "logis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "logis_default",
                "logis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}