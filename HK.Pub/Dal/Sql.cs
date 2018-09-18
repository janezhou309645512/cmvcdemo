using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace HK.Pub.Dal
{
    public class Sql
    {

        public class PData
        {
            public string SInsertCols { get; set; }
            public string SInsertParameters { get; set; }
            public string SUpdateInfo { get; set; }
            public List<SqlParameter> Parameters { get; set; }
            string prexParaName { get; set; }
            public PData()
            {
            }

            
            public PData(object paras)
            {
                InitInstance(this, paras, null, "", "");
            }

            public PData AddParameter(string name,object value){
                Parameters.Add(new SqlParameter("@"+name,value==null?DBNull.Value:value));
                return this;
            }
            public PData(object paras,string notCols,string pre="")
            {
                InitInstance(this, paras, null, notCols, pre);
            }
            public PData(object paras,object otherParams,string notCols="",string pre="")
            {
                InitInstance(this, paras, otherParams, notCols, pre);
            }
            static  PData InitInstance(PData data, object obj,object otherParam, string notContainCols, string prexParName)
            {
                try
                {
                    if (data == null) data = new PData();
                    data.prexParaName = prexParName;
                    StringBuilder sbIcols = new StringBuilder();
                    StringBuilder sbIpars = new StringBuilder();
                    StringBuilder sbUps = new StringBuilder();
                    data.Parameters = new List<SqlParameter>();
                    foreach (var p in obj.GetType().GetProperties())
                    {
                        if (p.GetValue(obj) != null &&  !System.Text.RegularExpressions.Regex.IsMatch(notContainCols, "\\b" + p.Name + "\\b"))
                        {
                            if (p.PropertyType == typeof(DateTime) && ((DateTime)p.GetValue(obj)).ToString("yyyy-MM-dd") == "0001-01-01") continue;
                            sbIcols.Append(p.Name + ",");
                            sbIpars.Append("@" + data.prexParaName + p.Name + ",");
                            sbUps.Append(string.Format("{0}=@{1}{0},", p.Name, data.prexParaName));
                            data.Parameters.Add(new SqlParameter("@" + p.Name, p.GetValue(obj)));
                        }
                    }
                    if (sbIcols.Length > 0) sbIcols.Length--;
                    if (sbIpars.Length > 0) sbIpars.Length--;
                    if (sbUps.Length > 0) sbUps.Length--;
                    data.SInsertCols = sbIcols.ToString();
                    data.SInsertParameters = sbIpars.ToString();
                    data.SUpdateInfo = sbUps.ToString();
                    if (otherParam != null)
                    {
                        foreach (var o in otherParam.GetType().GetProperties())
                        {
                            if (o.GetValue(otherParam) != null)
                                data.Parameters.Add(new SqlParameter("@" + o.Name, o.GetValue(obj)));
                        }
                    }
                    return data;
                }
                catch (Exception e)
                {
                    new Log("HK.Pub").Error(e.Message+ e.StackTrace);
                    throw e;
                }
                
            }
        }
        public string DbConnectionString { get; set; }
        public Sql() { }
        public Sql(string connectionstr)
        {
            DbConnectionString = connectionstr;
        }

        #region 传统查询方法
        public int ExcuteSql(string sql, List<SqlParameter> pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd=new SqlCommand(sql,con))
                {
                    con.Open();
                    if (pars != null && pars.Count > 0) cmd.Parameters.AddRange(pars.ToArray());
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public object QuerySclar(string sql, List<SqlParameter> pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    if (pars != null && pars.Count > 0) cmd.Parameters.AddRange(pars.ToArray());
                    return cmd.ExecuteScalar();
                }
            }
        }
        public DataTable QueryDataTable(string sql, List< SqlParameter> pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    if (pars != null && pars.Count > 0) cmd.Parameters.AddRange(pars.ToArray());
                    DataTable dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    return dt;
                }
            }
        }
        public DataSet QueryDataSet(string sql, List<SqlParameter> pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    if (pars != null && pars.Count > 0) cmd.Parameters.AddRange(pars.ToArray());
                    DataSet ds = new DataSet();
                    new SqlDataAdapter(cmd).Fill(ds);
                    return ds;
                }
            }
        }
        #endregion

        #region 对象查询方法
        public int ExcuteSql(string sql, params object[] pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    AddSqlParameterFromObject(cmd.Parameters, pars);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public object QuerySclar(string sql, params object[] pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    AddSqlParameterFromObject(cmd.Parameters, pars);
                    return cmd.ExecuteScalar();
                }
            }
        }
        public DataTable QueryDataTable(string sql, params object[] pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    AddSqlParameterFromObject(cmd.Parameters, pars);
                    DataTable dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    return dt;
                }
            }
        }
        public DataSet QueryDataSet(string sql, params object[] pars)
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    AddSqlParameterFromObject(cmd.Parameters, pars);
                    DataSet ds = new DataSet();
                    new SqlDataAdapter(cmd).Fill(ds);
                    return ds;
                }
            }
        }
        #endregion

        #region 辅助
        static void AddSqlParameterFromObject(SqlParameterCollection collection,params object[] pars)
        {
            if (pars == null || pars.Length <= 0) return;
            foreach (var obj in pars)
            {
                foreach (var p in obj.GetType().GetProperties())
                {
                    if (p.GetValue(obj) != null) collection.Add(new SqlParameter("@" + p.Name, p.GetValue(obj)));
                }
            }
            
        }
        static public Dictionary<string,SqlParameter> GetSqlParametersFromObject(object obj)
        {
            Dictionary<string,SqlParameter> dic=new Dictionary<string,SqlParameter>();
            //var res = from p in obj.GetType().GetProperties()
            //          where p.GetValue(obj) != null
            //          select   new Data.KeyValue<string, SqlParameter> { Key = p.Name, Val = new SqlParameter("@" + p.Name, p.GetValue(obj)) };
            foreach (var p in obj.GetType().GetProperties())
                if (p.GetValue(obj) != null) dic.Add(p.Name, new SqlParameter("@" + p.Name, new SqlParameter("@" + p.Name, p.GetValue(obj))));
            return dic;
        }
        #endregion
    }
}
