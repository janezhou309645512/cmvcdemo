using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmvcdemo.Areas.WebSelf.Controllers
{
    public class LoginController : Controller
    {
        // GET: WebSelf/Login
        public ActionResult Index()
        {
            return View();


        }

        public ActionResult Login()
        {
            return View();


        }
        public string Login(string UserName,string Pwd)
        {


            return Dal.Login.UserLogin(UserName, Pwd);


        }




    }
}