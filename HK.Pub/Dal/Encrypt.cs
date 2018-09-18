using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HK.Pub.Dal
{
    public class Encrypt
    {
        #region +Hash编码解码
        /// <summary>
        /// Hash编码解码
        /// </summary>
        public class Hash
        {
            /// <summary>
            /// 基于Md5的自定义加密字符串方法：输入一个字符串，返回一个由32个字符组成的十六进制的哈希散列（字符串）。
            /// </summary>
            /// <param name="str">要加密的字符串</param>
            /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
            public static string Md5(string str)
            {
                //将输入字符串转换成字节数组
                var buffer = Encoding.Default.GetBytes(str);
                //接着，创建Md5对象进行散列计算
                var data = System.Security.Cryptography.MD5.Create().ComputeHash(buffer);
                //创建一个新的Stringbuilder收集字节
                var sb = new StringBuilder();
                //遍历每个字节的散列数据 
                foreach (var t in data)
                {
                    //格式每一个十六进制字符串
                    sb.Append(t.ToString("X2"));
                }
                //返回十六进制字符串
                return sb.ToString();
            }

            /// <summary>
            /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
            /// </summary>
            /// <param name="str">要加密的字符串</param>
            /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
            public static string Sha1(string str)
            {
                var buffer = Encoding.UTF8.GetBytes(str);
                var data = System.Security.Cryptography.SHA1.Create().ComputeHash(buffer);
                var sb = new StringBuilder();
                foreach (var t in data)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        } 
        #endregion

        #region Rsa 加解密
        /// <summary>
        /// Rsa加密、解密 用法 new new Rsa("E219E449-25ED-4F71-9C34-780C2A6DD581").Decrypt(str)
        /// </summary>
        public class Rsa
        {
            #region Rsa加密、解密
            CspParameters param = new CspParameters() { KeyContainerName = "E219E449-25ED-4F71-9C34-780C2A6DD581" };

            public Rsa()
            {

            }
            public Rsa(string key)
            {
                param = new CspParameters();
            }
            /// <summary>
            /// Rsa加密
            /// </summary>
            /// <param name="express"></param>
            /// <returns></returns>
            public string Encryption(string express)
            {
                //param.KeyContainerName = key;//密匙容器的名称，保持加密解密一致才能解密成功
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
                {
                    byte[] plaindata = Encoding.Default.GetBytes(express);//将要加密的字符串转换为字节数组
                    byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                    return System.Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
                }
            }

            /// <summary>
            /// Rsa解密
            /// </summary>
            /// <param name="ciphertext"></param>
            /// <returns></returns>
            public string Decrypt(string ciphertext)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
                {
                    byte[] encryptdata = System.Convert.FromBase64String(ciphertext);
                    byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                    return Encoding.Default.GetString(decryptdata);
                }
            }
            #endregion
        }
        #endregion
    }
}
