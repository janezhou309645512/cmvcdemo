using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Text.RegularExpressions;

namespace HK.Pub.Dal
{
    public class Http
    {
        static public HttpRequest Request = System.Web.HttpContext.Current.Request;
        static public HttpResponse Response = System.Web.HttpContext.Current.Response;
        static Log log = new Log();
        static public string Param(string key){return Request[key];}

        static public void Redirect(string url,bool end)
        {
            Response.Redirect(url,end);
        }
        static public void ResponeWrite(string text)
        {
            Response.Write(text);
        }
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        static public string UrlEncode(string context)
        {
            return System.Web.HttpUtility.UrlEncode(context);
        }
        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);
                //request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                log.Error("Exception message: "+ e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                log.Error("HttpService "+ e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    log.Error("HttpService StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    log.Error("HttpService StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw e;
            }
            catch (Exception e)
            {
                log.Error("HttpService  "+ e.ToString());
                throw e;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        /// <summary>
        /// 处理http Post请求，提交Json数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="json">提交的数据</param>
        /// <returns></returns>
        public static string PostJson(string url, string json)
        {
            return Post(url, "application/json", json, 20);
        }
        /// <summary>
        /// 处理http Post请求，提交Xml数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="xml">提交的数据</param>
        /// <returns>请求成功后 返回数据，失败抛出异常</returns>
        public static string PostXml(string url, string xml)
        {
            return Post(url, "text/xml", xml, 20);
        }
        /// <summary>
        /// 处理http post 请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="method">提交数据的类型</param>
        /// <param name="context">提交的数据</param>
        /// <param name="timeout">超时的时间</param>
        /// <returns>请求成功后 返回数据，失败抛出异常</returns>
        public static string Post(string url, string method, string context,  /*bool isUseCert, */int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = method;// "text/xml"; application/json
                byte[] data = System.Text.Encoding.UTF8.GetBytes(context);
                request.ContentLength = data.Length;

                //是否使用证书
                //if (isUseCert)
                //{
                //    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                //    X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD);
                //    request.ClientCertificates.Add(cert);
                //    Log.Debug("WxPayApi", "PostXml used cert");
                //}

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                //Log.Error("Exception message: {0}", e.Message);
                //HK.IO.Log.Ins.Write("HttpService:Thread - caught ThreadAbortException - resetting.");
                log.Error(string.Format("Exception message: {0}", e.Message));
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                log.Error("HttpService  " + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    log.Error("HttpService  StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    log.Error("HttpService  StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw e;
            }
            catch (Exception e)
            {
                log.Error("HttpService  "+e.Message);
                throw  e;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        static public string GetCurrentRequestHtml()
        {
            Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
            byte[] requestByte = new byte[requestStream.Length];
            requestStream.Read(requestByte, 0, (int)requestStream.Length);
            string requestStr = Encoding.UTF8.GetString(requestByte);
            return requestStr;
        }

        static public string BrowerUserAgent()
        {
            return Request.UserAgent.ToLower();
        }
        static public bool BrowerCategoryByString(string str)
        {
            return BrowerUserAgent().Contains(str.Trim().ToLower());
        }
        #region HttpCookies操作
        public class Cookie
        {
            static public void Add(string name, string value, int expre)
            {
                System.Web.HttpCookie myCookie = new System.Web.HttpCookie(name);
                myCookie.Expires = DateTime.Now.AddDays(expre);
                myCookie.Value = value;
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            static public void Add(string name, string value)
            {
                Add(name, value, 7);
            }

            static public string Get(string name)
            {
                return System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(name) ? System.Web.HttpContext.Current.Request.Cookies[name].Value : "";
            }
            static public void SetExpires(string name, int seconds)
            {
                if (System.Web.HttpContext.Current.Response.Cookies.AllKeys.Contains(name))
                    System.Web.HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddSeconds(seconds);
            }
            static public void Delete(string name)
            {
                System.Web.HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddYears(-1);
            }
        } 
#endregion

        public enum ClientType{Mobile,PC}
        static public ClientType ClientTypeIs()
        {
            try
            {
                string userAgent = Request.UserAgent;
                System.Text.RegularExpressions.Regex b = new System.Text.RegularExpressions.Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
                System.Text.RegularExpressions.Regex v = new System.Text.RegularExpressions.Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
                if (!(b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4))))
                {
                    return ClientType.PC;
                    //PC访问   
                }
                else
                {
                    return ClientType.Mobile;
                    //手机访问   
                }
            }
            catch (Exception e)
            {

                return ClientType.Mobile;
            }
            
        }
    }


}
