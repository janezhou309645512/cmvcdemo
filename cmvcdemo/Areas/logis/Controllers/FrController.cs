using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FastReport.Utils;
using FastReport.Web;
using System.Web.UI.WebControls;

namespace cmvcdemo.Areas.logis.Controllers
{
    public class FrController : Controller
    {
        // GET: logis/Fr
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Fr()
        {
              WebReport webReport = new WebReport();
       
            string filename = @"F:\fxfile\test.frx";
            webReport.Report.Load(filename);
            webReport.Report.SetParameterValue("Test", "MY");

            ViewBag.WebReport = webReport;
          


            return View();
        }






    }
}