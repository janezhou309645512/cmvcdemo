using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HK.Pub.Data
{
    public class Error
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public Error()
        {

        }
        static public Error GetFromJson(string json)
        {
            try
            {
                return LitJson.JsonMapper.ToObject<Error>(json);
            }
            catch (Exception e)
            {
                return null;   
            }
        }
        public bool Ok
        {
            get { return errcode == 0 ? true : false; }
        }

        public string ToJson() { return Dal.Cast.ToJson(this); }
        public string ToXml() { return Dal.Cast.ToXml(this); }
    }
}
