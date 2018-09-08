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



      {        return "这是我的一SDFSFSDFS ;)"; 
    
     }

        //返回视图

        public ActionResult GetView()
      {
             Employee emp = new Employee();
             emp.FirstName = "Sukesh";
             emp.LastName = "Marla";
             emp.Salary = 20000;
           // 在ViewData中存储Employee 对象

             ViewData["Employee"] = emp;
             return View("MyView");

        }







}
}