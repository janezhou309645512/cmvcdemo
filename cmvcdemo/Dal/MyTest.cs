using HK.Pub.Externd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace cmvcdemo.Dal
{
    public class MyTest
    {
        static HK.Pub.Dal.Sql dbMachineDataCollection = new HK.Pub.Dal.Sql(HK.ConfigFile.ConnectiongString_Get("ShiMo"));


        public static string LshapeShow()
        {

            return dbMachineDataCollection.QueryDataTable(@"exec T_ScanRecord '','','2018-09-14','2018-09-14',1,10,10,100").ToJson();

        }












    }
}