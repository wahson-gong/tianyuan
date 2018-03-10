using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
public partial class admin_Repair_data : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public int access_i = 0;
    public DataTable GetSchemaTable()
    {
        using (OleDbConnection connection = new
                   OleDbConnection(my_c.Getconn()))
        {
            connection.Open();
            DataTable schemaTable = connection.GetOleDbSchemaTable(
                OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });
            return schemaTable;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                Repeater2.Visible = false;
                Repeater1.DataSource = my_c.GetTable("SELECT name,id,crdate,refdate FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0").DefaultView;
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.Visible = false;
                Repeater2.DataSource = GetSchemaTable();
                Repeater2.DataBind();
            }


        }
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        DataTable dt = my_c.GetTable("SELECT name,id,crdate,refdate FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataTable dt1 = my_c.GetTable("SELECT Name,xtype FROM SysColumns WHERE id=Object_Id('" + dt.Rows[i]["name"].ToString() + "')  and xtype=35");
            string t1 = "";
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (t1 == "")
                {
                    t1 = dt1.Rows[j]["Name"].ToString();
                }
                else
                {
                    t1 = t1 + "|" + dt1.Rows[j]["Name"].ToString();
                }
            }

            if (t1 != "")
            {

                DataTable dt2 = my_c.GetTable("select id," + t1.Replace("|", ",") + " from " + dt.Rows[i]["name"].ToString() + "");
                for (int h = 0; h < dt2.Rows.Count; h++)
                {
                    string sql = "update " + dt.Rows[i]["name"].ToString() + " set ";
                    string[] aa = t1.Split('|');
                    for (int l = 0; l < aa.Length; l++)
                    {
                        string s_sql = my_b.c_string(dt2.Rows[h][aa[l].ToString()].ToString().Replace(my_b.c_string(this.TextBox1.Text), ""));
                        Regex reg = new Regex(my_b.c_string(this.TextBox1.Text), RegexOptions.Singleline);
                        s_sql = reg.Replace(s_sql, "");
                        if (l == 0)
                        {
                            sql = sql + " " + aa[l].ToString() + "='" + s_sql + "'";
                        }
                        else
                        {
                            sql = sql + "," + aa[l].ToString() + "='" + s_sql + "'";
                        }


                    }

                    sql = sql + " where id=" + dt2.Rows[h]["id"].ToString();
                    my_c.genxin(sql);
                }
            }


            Response.Write(dt.Rows[i]["name"].ToString() + "表处理完成<br>");
            Response.Flush();

        }
        Response.Write("<script>window.location='err.aspx?err=所有表替换完成！&errurl=" + my_b.tihuan("Repair_data.aspx", "&", "fzw123") + "';</script>");
    }
}
