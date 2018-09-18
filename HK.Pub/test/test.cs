using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HK.Pub.test
{
    public class test
    {
        public string ToUserName { get; set; }
        public string MsgType { get; set; }
        public long CreateTime { get; set; }
        public scancodeInfo ScanCodeInfo { get; set; }

        public class scancodeInfo
        {
            public string ScanType { get; set; }
            public string ScanResult { get; set; }
        }
    }
}
