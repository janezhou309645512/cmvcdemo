using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HK.Pub.Externd
{
    static public class ListExternd
    {
        static public string ToJson<T>(this List<T> lst) where T:new()
        {
            if (lst == null || lst.Count <= 0) return "";
            return LitJson.JsonMapper.ToJson(lst);
        }


        //static public string ToJson(this object obj)
        //{
        //    if (obj == null) return "";
        //    return LitJson.JsonMapper.ToJson(obj);
        //}
    }
}
