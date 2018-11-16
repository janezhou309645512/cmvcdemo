using HK.Pub.Externd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmvcdemo.Areas.WebSelf.Dal
{
    public class Login
    {

        static HK.Pub.Dal.Sql dbRmmsDataCollection = new HK.Pub.Dal.Sql(HK.ConfigFile.ConnectiongString_Get("RMMS"));



        public static string UserLogin(string UserName,string Pwd)
        {

            return dbRmmsDataCollection.QueryDataTable(@"select * from [dbo].[tuser] where name=@p1 and pwd=@p2", new { p1 = UserName, p2 = Pwd }).ToJson();

        }








    }
}