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
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using yanzheng;
using System.Diagnostics;
using Microsoft.Win32;
using System.Globalization;
public partial class admin_main : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string conn_string = "";
    yanzheng.Class1 yz = new Class1();
    public string Downloads(string g1)
    {
        WebClient wc = new WebClient();
        string newilt = HttpContext.Current.Request.PhysicalApplicationPath + "upfile/data/" + g1.Substring(g1.LastIndexOf("/"));
        try
        {
            wc.DownloadFile(g1, newilt);
        }
        catch
        { }
        return my_b.get_ApplicationPath() + "/upfile/data/" + g1.Substring(g1.LastIndexOf("/"));
    }
    public DataTable GetTable(string g1)
    {
        try
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet dset = new DataSet();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = conn_string;
            con.Open();
            SqlCommand cmd;
            cmd = new SqlCommand(g1, con);
            adapter.SelectCommand = cmd;
            dset.Tables.Add("xuesheng");
            adapter.Fill(dset, "xuesheng");
            con.Close();
            return dset.Tables["xuesheng"];
        }
        catch
        {
            HttpContext.Current.Response.Write(g1);
            HttpContext.Current.Response.End();
            DataSet dset = new DataSet();
            return dset.Tables["xuesheng"];
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                conn_string = yz.getWebFile(Request.UserHostAddress.ToString(), Request.Url.ToString());
                
                DataTable dt = my_c.GetTable("select u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system where u4='在线更新'");
                string t1 = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (t1 == "")
                    {
                        t1 = dt.Rows[i]["u2"].ToString();
                    }
                    else
                    {
                        t1 = t1 + "," + dt.Rows[i]["u2"].ToString();
                    }
                }
                if (t1 != "")
                {
                    Repeater1.DataSource = GetTable("select * from up_table where id not in (" + t1 + ") and contents<>'' order by id desc");
                    Repeater1.DataBind();
                }

            }
            catch
            { }


            try
            {
                Literal1.Text = Request.ServerVariables["LOCAl_ADDR"];
            }
            catch
            { }
            try
            {
                Literal2.Text = Request.ServerVariables["SERVER_NAME"];
                
            }
            catch
            { }
            try
            {
                Literal3.Text = Request.ServerVariables["Server_Port"].ToString();
                
            }
            catch
            { }
            try
            {
                Literal4.Text = Request.PhysicalApplicationPath;
               
            }
            catch
            { }
            try
            {
                Literal5.Text = Environment.OSVersion.ToString();
               
            }
            catch
            { }
            try
            {
                Literal1.Text = Request.ServerVariables["LOCAl_ADDR"];
            }
            catch
            { }
            try
            {
                Literal6.Text = Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
                
            }
            catch
            { }
            try
            {
                Literal7.Text = (Server.ScriptTimeout / 1000).ToString();
                
            }
            catch
            { }
            try
            {
                Literal8.Text = ((Environment.TickCount / 0x3e8) / 60).ToString();
                
            }
            catch
            { }
            try
            {
                Literal9.Text = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS").ToString();
               
            }
            catch
            { }
            try
            {
                Literal10.Text = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER").ToString();
               
            }
            catch
            { }
            try
            {
                Literal11.Text = ((Double)Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2");

            }
            catch
            { }
            try
            {
                Literal12.Text = ((TimeSpan)Process.GetCurrentProcess().TotalProcessorTime).TotalSeconds.ToString("N0");
            }
            catch
            { }
            
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataTable dt = my_c.GetTable("select u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system where u4='在线更新'");
        string t1 = "";
        int i = 0;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            if (t1 == "")
            {
                t1 = dt.Rows[i]["u2"].ToString();
            }
            else
            {
                t1 = t1 + "," + dt.Rows[i]["u2"].ToString();
            }
        }
        if (t1 != "")
        {
            dt = GetTable("select * from up_table where id not in (" + t1 + ") order by id desc");
        }
        else
        {
            dt = GetTable("select * from up_table order by id desc");
        }

        string up_url = "http://www..com";
        try
        {
            string tt1 = my_b.getWebFile(up_url);
        }
        catch
        {
            up_url = "http://www.6e7e.com";
        }



        for (i = 0; i < dt.Rows.Count; i++)
        {
            string file_path = Downloads(up_url + dt.Rows[i]["file_path"].ToString());
            my_b.del_pic(dt.Rows[i]["up_path"].ToString());
            File.Move(HttpContext.Current.Request.PhysicalApplicationPath + file_path, HttpContext.Current.Request.PhysicalApplicationPath + dt.Rows[i]["up_path"].ToString());
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u2,u3,u4) values('" + dt.Rows[i]["id"].ToString() + "','" + Request.UserHostAddress.ToString() + "','在线更新')");
        }
        Response.Redirect("err.aspx?err=所有在线更新完成！&errurl=" + my_b.tihuan("main.aspx", "&", "fzw123") + "");

    }
}
