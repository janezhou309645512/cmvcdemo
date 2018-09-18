using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HK.Pub.Dal
{
    public class Log
    {
        public enum Category
        {
            Log,Warn,Debug,Error
        }
        private string fileName = "log.txt";
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private string path = AppDomain.CurrentDomain.BaseDirectory;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        string fullName { get { return path + fileName; } }
        public Log()
        {

        }
        public Log(string fileName,string path="")
        {
            if (!string.IsNullOrEmpty(fileName)) FileName = fileName; 
            if (!string.IsNullOrEmpty(path)) Path = path;
        }
        public Log Write(string text,bool isLine=false)
        {
            try
            {
                FileStream fs = null;
                if (!File.Exists(fullName)) fs=File.Create(fullName);
                fs = new FileStream(fullName,FileMode.Append);
                StreamWriter writer = new StreamWriter(fs,  Encoding.Default);
                if (isLine) writer.WriteLine(text);else writer.Write(text);
                writer.Flush();
                writer.Close();
                writer.Dispose();
                fs.Dispose();
            }
            catch { }
            return this;
        }
        public Log WriteLine(string text) { Write(text, true); return this; }
        public Log Clear()
        {
            try
            {
                File.Delete(fullName); File.Create(fullName);
            }
            catch { }
            return this;
        }

        //public Log Log(string text)
        //{
        //    WriteLine(string.Format("{0}\t{1}\t{2}", Category.Log.ToString(), DateTime.Now.ToString(), text));
        //    return this;
        //}
        public Log Debug(string text)
        {
            WriteLine(string.Format("{0}\t{1}\t{2}", Category.Debug.ToString(), DateTime.Now.ToString(), text));
            return this;
        }
        public Log Warn(string text)
        {
            WriteLine(string.Format("{0}\t{1}\t{2}", Category.Warn.ToString(), DateTime.Now.ToString(), text));
            return this;
        }
        public Log Error(string text)
        {
            WriteLine(string.Format("{0}\t{1}\t{2}", Category.Error.ToString(), DateTime.Now.ToString(), text));
            return this;
        }
    }
}
