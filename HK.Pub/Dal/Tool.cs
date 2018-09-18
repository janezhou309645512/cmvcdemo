using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace HK.Pub.Dal
{
    public class Tool
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static int TimeStamp
        {
            get
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return (int)ts.TotalSeconds;
            }
        }

        static public string Ping(string purpose)
        {
            StringBuilder sb = new StringBuilder(string.Format("Ping {0} 的结果:",purpose));
            Ping ping = new Ping();
            try
            {
                PingReply pingReply = ping.Send(purpose);
                if (pingReply.Status == IPStatus.Success)
                {
                    sb.Append(string.Format("相应时间【{0}】,  成功！", pingReply.RoundtripTime));
                }
                else
                {
                    sb.Append(string.Format("相应超时,  失败！"));
                }
            }
            catch (Exception ex)
            {

                sb.Append(ex.Message+ex.InnerException.Message+",  出错！");
            }
            
            return sb.ToString();
        }

        static public string ExcuteCmd(string input)
        {
             Process p = new Process();
             //设置要启动的应用程序
             p.StartInfo.FileName = "cmd.exe";
             //是否使用操作系统shell启动
             p.StartInfo.UseShellExecute = false;
             // 接受来自调用程序的输入信息
             p.StartInfo.RedirectStandardInput = true;
             //输出信息
             p.StartInfo.RedirectStandardOutput = true;
             // 输出错误
             p.StartInfo.RedirectStandardError = true;
             //不显示程序窗口
             p.StartInfo.CreateNoWindow = true;
             //启动程序
             p.Start();
 
             //向cmd窗口发送输入信息
             p.StandardInput.WriteLine(input+"&exit");
 
             p.StandardInput.AutoFlush=true;
 
              //获取输出信息
             string strOuput = p.StandardOutput.ReadToEnd();
             //等待程序执行完退出进程
             p.WaitForExit();
             p.Close();
             return strOuput;
 
        }
    }
}
