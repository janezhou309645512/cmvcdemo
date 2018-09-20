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

     //直接返回字符串
        public string GetString()

    {
            return "这是我的一SDFSFSDFS ;)"; 
    
     }

        public ActionResult TestView()
        {

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

            return Dal.MyTest.LshapeShow();


        }
        //直通率看板界面

        public ActionResult RirectRate()
        {
           

            
             return View();
        }

        

        public string FqcNg(string id)
        {






            return Dal.MyTest.FqcNg();


        }
        

        public string FqcRate(string id)
        {




         return Dal.MyTest.FqcRate();


        }









    }
}