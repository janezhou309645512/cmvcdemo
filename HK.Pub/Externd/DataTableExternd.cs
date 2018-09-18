using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HK.Pub.Externd
{
    static public class DataTableExternd
    {
        #region +DataTable的扩展
        /// <summary>
        /// DataTable转动态列表对象列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public List<dynamic> ToDynamicObjects(this DataTable dt)
        {

            if (dt == null || dt.Rows.Count <= 0) return null;
            List<dynamic> ret = new List<dynamic>();
            foreach (DataRow dr in dt.Rows)
            {
                ret.Add(dr.ToDynamicObject());
            }
            return ret;
        }
        /// <summary>
        /// DataTable转动态列表对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public dynamic ToDynamicObject(this DataTable dt)
        {

            if (dt == null || dt.Rows.Count <= 0) return null;
            return dt.Rows[0].ToDynamicObject();
            
        }

        /// <summary>
        /// 转换为Model对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public List<T> ToModels<T>(this DataTable dt) where T : new()
        {
            List<T> ret = new List<T>();
            if (dt == null || dt.Rows.Count <= 0) return ret;
            foreach (DataRow dr in dt.Rows)
            {
                ret.Add(dr.ToModel<T>());
            }
            return ret;
        }
        /// <summary>
        /// 转换为Model对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public T ToModel<T>(this DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0) return default(T);
            List<T> ret = new List<T>();
            return dt.Rows[0].ToModel<T>();
        }

        static public string ToJson(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0) return "";
            //if (dt.Rows.Count == 1) return dt.Rows[0].ToJson();
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(dr.ToJson() + ",");

            }
            if (sb.Length > 2) sb.Length = sb.Length - 1;
            sb.Append("]");
            return sb.ToString();
        }
        #endregion

        #region +DataRow的扩展
        /// <summary>
        /// DataRow转动态对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        static public dynamic ToDynamicObject(this DataRow dr)
        {
            if (dr == null) return null;
            dynamic obj = new System.Dynamic.ExpandoObject();
            var dict = (IDictionary<string, object>)obj;
            foreach (DataColumn col in dr.Table.Columns)
            {
                object val = dr[col.ColumnName];
                dict[col.ColumnName] = val;
                switch (col.DataType.Name)
                {
                    case "Int32":
                        dict[col.ColumnName] = 0;
                        break;
                    case "DateTime":
                        if(val!=null)
                        dict[col.ColumnName] = ((DateTime)val).ToString("yyyy-MM-dd hh:mm:ss");
                        break;
                    default:

                        break;
                }
            }
            return obj;
        }

        /// <summary>
        /// DataRow 转换为Model对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        static public T ToModel<T>(this DataRow dr) where T : new()
        {
            if (dr == null) return default(T);
            return Dal.Cast.ToObjectFromDataRow<T>(dr);
        }
        /// <summary>
        /// DataRow 转换为Json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        static public string ToJson(this DataRow dr) 
        {
            if (dr == null) return null;
            StringBuilder sb = new StringBuilder("{");
            foreach (DataColumn col in dr.Table.Columns)
            {
                string obj ="\""+ col.ColumnName+"\":";
                if (dr[col.ColumnName] == null) obj += "null";
                else if (dr[col.ColumnName] == DBNull.Value) obj += "null";
                else
                    switch (col.DataType.Name)
                    {
                        case "Int32":
                            obj += dr[col.ColumnName].ToString();
                            break;
                        //case "DateTime":
                        //    var val = dr[col.ColumnName];
                        //    if (val != null)
                        //        obj+= ((DateTime)val).ToString("yyyy-MM-dd hh:mm:ss");
                        //    break;
                        case "Boolean": obj += dr[col.ColumnName].ToString().Replace("T", "t").Replace("F", "f"); break;
                        default:
                            obj += "\"" + dr[col.ColumnName].ToString() + "\""; ;
                            break;
                    }
                sb.Append(obj + ",");
            }
            if (sb.Length > 2) sb.Length = sb.Length - 1;
            sb.Append("}");
            return sb.ToString();
        } 
        #endregion
    }
}
