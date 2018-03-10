using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
/// <summary>
///my_conn 的摘要说明
/// </summary>
public class my_conn
{
    public string Getconn(string sql_conn="")
    {
        //修改链接的数据库 --ghy
        if (!string.IsNullOrEmpty(sql_conn))
        {
            return ConfigurationSettings.AppSettings[sql_conn].ToString();
        }
        else
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                return ConfigurationSettings.AppSettings["sql_conn"].ToString();
            }
            else
            {
                return "provider=microsoft.jet.oledb.4.0;data source=" + HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationSettings.AppSettings["access_conn"].ToString();
            }
        }

        

    }
    //查询
    public DataTable GetTable(string g1, string sql_conn="")
    {
       　//兼容原来的接口 --ghy
        if (!string.IsNullOrEmpty(sql_conn))
        {
            //更新后的程序 --ghy
            if (g1 != "")
            {
                //try
                //{
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet dset = new DataSet();
                SqlConnection con = new SqlConnection();

                con.ConnectionString = Getconn(sql_conn);



                SqlCommand cmd;
                cmd = new SqlCommand(g1, con);

                adapter.SelectCommand = cmd;
                dset.Tables.Add("xuesheng");
                adapter.Fill(dset, "xuesheng");


                adapter.Dispose();
                dset.Dispose();
                return dset.Tables["xuesheng"];
            }
            else
            {
                DataSet dset = new DataSet();
                return dset.Tables["xuesheng"];
            }


        }
        else
        {
            if (g1 != "")
            {
                if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
                {
                    //try
                    //{
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet dset = new DataSet();
                    SqlConnection con = new SqlConnection();

                    con.ConnectionString = Getconn();



                    SqlCommand cmd;
                    cmd = new SqlCommand(g1, con);

                    adapter.SelectCommand = cmd;
                    dset.Tables.Add("xuesheng");
                    adapter.Fill(dset, "xuesheng");


                    adapter.Dispose();
                    dset.Dispose();
                    return dset.Tables["xuesheng"];
                    //}
                    //catch
                    //{
                    //    HttpContext.Current.Response.Write(g1);
                    //    HttpContext.Current.Response.End();
                    //    DataSet dset = new DataSet();
                    //    return dset.Tables["xuesheng"];
                    //}
                }
                else
                {
                    //try
                    //{
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    DataSet dset = new DataSet();
                    OleDbConnection con = new OleDbConnection();

                    con.ConnectionString = Getconn();
                    // con.Open();
                    OleDbCommand cmd;

                    cmd = new OleDbCommand(g1, con);
                    adapter.SelectCommand = cmd;
                    dset.Tables.Add("xuesheng");
                    adapter.Fill(dset, "xuesheng");
                    adapter.Dispose();
                    dset.Dispose();
                    return dset.Tables["xuesheng"];
                    //}
                    //catch
                    //{
                    //    HttpContext.Current.Response.Write(g1);
                    //    HttpContext.Current.Response.End();
                    //    DataSet dset = new DataSet();
                    //    return dset.Tables["xuesheng"];
                    //}
                }
            }
            else
            {
                DataSet dset = new DataSet();
                return dset.Tables["xuesheng"];
            }
        
        }


       
    }

    //更新
    public void genxin(string g1, string sql_conn = "")
    {
        //兼容原来的接口 --ghy
        if (!string.IsNullOrEmpty(sql_conn))
        {
            //更新后的程序 --ghy
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Getconn(sql_conn);
            con.Open();
            SqlCommand cmd;
            cmd = new SqlCommand(g1, con);
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();

        }
        else
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                //try
                //{
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Getconn();
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand(g1, con);
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();

                //}
                //catch
                //{
                //    HttpContext.Current.Response.Write(g1);
                //    HttpContext.Current.Response.End();
                //}
            }
            else
            {
                //try
                //{
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = Getconn();
                con.Open();
                OleDbCommand cmd;
                cmd = new OleDbCommand(g1, con);
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                //}
                //catch
                //{
                //    HttpContext.Current.Response.Write(g1);
                //    HttpContext.Current.Response.End();
                //}
            }
        
        }
    }


    //操作xml
    public DataTable read_xml(string file_path,string jidian)
    {
        DataSet objDataSet = new DataSet();
        objDataSet.ReadXml(HttpContext.Current.Request.PhysicalApplicationPath+file_path);
        return objDataSet.Tables[jidian];
    }

}
