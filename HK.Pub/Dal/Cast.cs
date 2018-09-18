using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LitJson;

namespace HK.Pub.Dal
{
    public class Cast
    {
        #region Json对象互换
        /// <summary>
        /// 将Json字符串转换成对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="jsonObj">Json字符串</param>
        /// <returns></returns>
        static public T ToObjectFromJson<T>(string jsonObj)
        {
            return LitJson.JsonMapper.ToObject<T>(jsonObj);
        }

        
        /// <summary>
        /// 将对象转换成Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public string ToJson(object obj)
        {
            return System.Text.RegularExpressions.Regex.Unescape(LitJson.JsonMapper.ToJson(obj));
        } 
        #endregion

        #region Xml 与对象互转
        /// <summary>
        /// 根据Xml设置对象的属性值
        /// </summary>
        /// <param name="obj">待设置值的对象</param>
        /// <param name="xml">包含对象值的xml字符串</param>
        static public void SetObjectFromXml(object obj, string xml) { SetObjectFromXml(obj, new HK.Pub.Dal.Xml(xml)); }
        /// <summary>
        /// 根据Xml设置对象的属性值
        /// </summary>
        /// <param name="obj">待设置值的对象</param>
        /// <param name="xml">包含对象值的HK.Pub.Dal.Xml</param>
        static public void SetObjectFromXml(object obj, Xml xml)
        {
            foreach (var p in obj.GetType().GetProperties())
            {
                if (!string.IsNullOrEmpty(xml[p.Name]))
                {
                    if (xml.GetNode(p.Name).ChildNodes.Count > 1)
                    {
                        var child = p.PropertyType.Assembly.CreateInstance(p.PropertyType.FullName);
                        SetObjectFromXml(child, (new Xml(xml.GetNode(p.Name).OuterXml)));
                        p.SetValue(obj, child);

                    }
                    else p.SetValue(obj, Convert.ChangeType(xml[p.Name], p.PropertyType));
                }
            }
        }
        /// <summary>
        /// 将xml字符串转换成对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        static public T TobjectXml<T>(string xml) where T:new() { return GetFromXml<T>(new HK.Pub.Dal.Xml(xml)); }
        /// <summary>
        /// 将xml字符串转换成对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="xml">HK.Pub.Dal.Xml</param>
        /// <returns></returns>
        static public T GetFromXml<T>(Xml xml) where T : new()
        {
            T ret = new T();
            SetObjectFromXml(ret, xml);
            return ret;
        }
        /// <summary>
        /// 将对象转换成Xml字符串
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="isRoot">是否在外层增加root</param>
        /// <returns></returns>
        static public string ToXml(object obj, bool isRoot = true)
        {
            StringBuilder sb = new StringBuilder();
            if (isRoot) sb.Append("<xml>");
            foreach (var p in obj.GetType().GetProperties())
            {
                if (p.GetValue(obj) != null)
                {
                    if (p.PropertyType.ReflectedType == null) sb.Append(string.Format("<{0}>{1}</{0}>", p.Name, p.GetValue(obj).ToString()));
                    else sb.Append(string.Format("<{0}>{1}</{0}>", p.Name, ToXml(p.GetValue(obj), false)));
                }
            }
            if (isRoot) sb.Append("</xml>");
            return sb.ToString();
        }
        /// <summary>
        /// 将对象转换成微信用的Xml字符串
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="isRoot">是否在外层增加root</param>
        /// <returns></returns>
        static public string ToWxXml(object obj, bool isRoot = true)
        {
            StringBuilder sb = new StringBuilder();
            if (isRoot) sb.Append("<xml>");
            foreach (var p in obj.GetType().GetProperties())
            {
                if (p.GetValue(obj) != null)
                {
                    if (p.PropertyType.ReflectedType == null)
                    {
                        if(p.PropertyType.Name.ToLower()=="string")
                            sb.Append(string.Format("<{0}><![CDATA[{1}]]></{0}>", p.Name, p.GetValue(obj).ToString()));
                        else sb.Append(string.Format("<{0}>{1}</{0}>", p.Name, p.GetValue(obj).ToString()));
                    }
                    else sb.Append(string.Format("<{0}>{1}</{0}>", p.Name, ToWxXml(p.GetValue(obj), false)));
                }
            }
            if (isRoot) sb.Append("</xml>");
            return sb.ToString();
        }
        
        #endregion

        #region DataTable转换成对象
        /// <summary>
        /// 将DataTable 转换成对象列表
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="dt">包含对象的数据表</param>
        /// <returns></returns>
        static public List<T> ToObjectsFromDataTable<T>(System.Data.DataTable dt) where T : new()
        {
            List<T> lst = new List<T>();
            if (dt == null || dt.Rows.Count <= 0) return lst;
            foreach (System.Data.DataRow dr in dt.Rows)
                lst.Add(ToObjectFromDataRow<T>(dr));
            return lst;
        }
        /// <summary>
        /// 将DataTable的第一行转换成对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="dt">包含对象的表</param>
        /// <returns></returns>
        static public T ToObjectFromDataTable<T>(System.Data.DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0) return default(T);
            return ToObjectFromDataRow<T>(dt.Rows[0]);
        }
        /// <summary>
        /// 将DataRow转换成对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="dr">包含对象的数据行</param>
        /// <returns></returns>
        static public T ToObjectFromDataRow<T>(System.Data.DataRow dr) where T : new()
        {
            if (dr == null) return default(T);
            T ret = new T();
            foreach (var p in ret.GetType().GetProperties())
            {
                if (p.SetMethod != null && dr.Table.Columns.Contains(p.Name) && dr[p.Name] != null && dr[p.Name] != DBNull.Value)
                {
                    switch (p.PropertyType.Name)
                    {
                        case "DataTime":
                            dr[p.Name] = (dr[p.Name] == null ? null : ((DateTime)dr[p.Name]).ToString("yyyy-MM-dd hh:mm:ss"));
                            break;
                        default:
                            break;
                    }
                    if (p.PropertyType.BaseType.Name == "Enum")
                    {
                        Type e = p.PropertyType; 
                        object o = System.Enum.ToObject(e, int.Parse(dr[p.Name].ToString()));

                        p.SetValue(ret, Convert.ChangeType(o, p.PropertyType));
                    }
                    else p.SetValue(ret, Convert.ChangeType(dr[p.Name], p.PropertyType));
                }
            }
            return ret;
        } 
        #endregion
        #region
        static public IEnumerable<Data.KeyValue<string, object>> GetObjectProps(object obj)
        {
            if (obj == null) return null;
            return obj.GetType().GetProperties().Select(p => new Data.KeyValue<string, object> { Key = p.Name, Val = p.GetValue(obj), Tp = p.PropertyType });
        }
        /// <summary>
        /// 根据原对象复制一个对象，非引用复制
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ret"></param>
        static public object CopyObject(object source, object ret)
        {
            System.Reflection.PropertyInfo[] retProperties = ret.GetType().GetProperties();
            System.Reflection.PropertyInfo[] souProperties = source.GetType().GetProperties();
            for (int i = 0; i < souProperties.Length; i++)
            {
                System.Reflection.PropertyInfo p = souProperties[i];
                if (p.GetValue(source) != null && p.SetMethod != null) 
                if (p.PropertyType.ReflectedType == null)
                    retProperties[i].SetValue(ret, souProperties[i].GetValue(source));
                else
                {
                    var child = retProperties[i].GetType().Assembly.CreateInstance(retProperties[i].PropertyType.FullName);
                    CopyObject(p.GetValue(source), child);
                    retProperties[i].SetValue(ret, child);
                }
            }
            return ret;
        }
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        static public T CloneObject<T>(T source) where T : new()
        {
            T ret = new T();
            CopyObject(source, ret);
            return ret;
        }
        static public bool EqualVal(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null || obj2 == null) return false;
            
            bool ret = true;
            System.Reflection.PropertyInfo[] retProperties = obj1.GetType().GetProperties();
            System.Reflection.PropertyInfo[] souProperties = obj2.GetType().GetProperties();
            for (int i = 0; i < souProperties.Length && ret; i++)
            {
                System.Reflection.PropertyInfo p = souProperties[i];
                if (p.GetValue(obj2) == null) continue;
                if (p.PropertyType.ReflectedType == null)
                    ret = (Convert.ChangeType(p.GetValue(obj2),p.PropertyType).Equals(Convert.ChangeType(retProperties[i].GetValue(obj1),retProperties[i].PropertyType)));
                else
                {
                    ret = EqualVal(p.GetValue(obj2), retProperties[i].GetValue(obj1));
                }
            }
            return ret;
        } 
        #endregion

    }
}
