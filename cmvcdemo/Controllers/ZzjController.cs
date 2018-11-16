using cmvcdemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmvcdemo.Controllers
{
    public class ZzjController : Controller
    {
         string date = DateTime.Now.ToString("yyyy-MM-dd");
        //直接返回字符串
        public string GetString()

    {
            return "testone welcome you"; 
    
     }

        public ActionResult AllView()
        {

            return View();

        }


        public ActionResult MainView(string id)
        {

            //根据id在数据库查询地址url
            //传参数给ui
           ViewBag.Url = Dal.MyTest.KanBanUrl(Convert.ToInt32(id));
           return View();

        }
        public ActionResult SwitchingView(string id)
        {

            //根据id在数据库查询地址url
            //传参数给ui
            ViewBag.Url = id;
            return View();

        }
        public string PageSwitching(string id)
        {


            return Dal.MyTest.KanBan(Convert.ToInt32(id));


        }

        public ActionResult TestView()
        {
            ViewBag.Mal ="dfsfsdfsdfsdf";
            return View();

        }
        //返回视图

        public ActionResult MyView()
      {
           
            return View();

        }

        //返回一个报表的json值
       public string GetUsers()
        {
      
             return "";
        }


        public string LshapeShow(string id)
        {

           
            return Dal.MyTest.LshapeShow(date);


        }
        //直通率看板界面

        public ActionResult RirectRate()
        {
           

            
             return View();
        }

        

        public string FqcNg(string id)
        {






            return Dal.MyTest.FqcNg(date);


        }
        

        public string FqcRate(string id)
        {




         return Dal.MyTest.FqcRate(date);


        }
        //项目进度

        public ActionResult ProjectSchedule()
        {



            return View();
        }


        public string ProjectSch(string id)
        {




            return Dal.MyTest.ProjectSch(date);
        }


        //模切达成
        public ActionResult ModuleSchedule()
        {




            return View();
        }

        public string ModuleSch(string id)
        {




            return Dal.MyTest.ModuleSch(date);
        }






    }
}