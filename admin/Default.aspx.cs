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
using System.IO;
using System.Net;
using yanzheng;
using System.Diagnostics;
using Microsoft.Win32;
using System.Globalization;
public partial class admin_Default : System.Web.UI.Page
{
   public  my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string model = "";
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    public string categories = "";
    public string  set_OSVersion(string t1)
    {
        string fanhui = "";
        if (t1.IndexOf("5.1") > -1)
        {
            fanhui= "Windows 2000";
        }
        else if (t1.IndexOf("5.1") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        else if (t1.IndexOf("5") > -1)
        {
            fanhui = "Windows 2000";
        }
        return fanhui;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (my_b.k_cookie("admin_id") == "")
                {
                    Response.Redirect("err.aspx?err=对不起你还没有登陆,请重新登陆！&errurl=login.aspx");
                }
                if (my_b.k_cookie("admin_pwd") == "")
                {
                    Response.Redirect("err.aspx?err=对不起你还没有登陆,请重新登陆！&errurl=login.aspx");
                }
            }
            catch
            {
                Response.Redirect("err.aspx?err=对不起你还没有登陆,请重新登陆！&errurl=login.aspx");
            }

            //
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }
            if(type=="loginout")
            {
                my_b.admin_o_cookie("admin_id");
                my_b.admin_o_cookie("admin_pwd");
                Response.Redirect("err.aspx?err=你已经安全退出网站，请关闭页面离开！&errurl=" + my_b.tihuan("login.aspx", "&", "fzw123") + "");
            }
            //统计 start
           
                DataTable dt = my_c.GetTable("select top 5 * from sl_Model where  (u3 like'文章模型' or u3 like'新闻模型') order by id asc");
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string model_name = "{ name: '" + dt.Rows[j]["u2"].ToString() + "',data:{fzw:data}}          ";
                string model_data = "";
           
                for (int i = 0; i <= 9; i++)
                {
                    int fushu = int.Parse("-" + i.ToString());
                    DateTime dy = DateTime.Now.AddDays(fushu);
                    string shijian = dy.ToString().Split(' ')[0];
                    if (model_data == "")
                    {
                        categories = "'"+ dy.ToString("M月dd号") + "'";
                        if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
                        {
                            model_data = my_b.get_count(dt.Rows[j]["u1"].ToString(), "where dtime between '" + shijian.ToString() + shang_time + "' and '" + shijian.ToString() + xia_time + "'").ToString();
                        }
                        else
                        {
                            model_data = my_b.get_count(dt.Rows[j]["u1"].ToString(), "where dtime between #" + shijian.ToString() + shang_time + "# and #" + shijian.ToString() + xia_time + "#").ToString();
                        }
                           
                    }
                    else
                    {
                        categories = categories+","+ "'" + dy.ToString("M月dd号") + "'";
                        if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
                        {
                            model_data = model_data + "," + my_b.get_count(dt.Rows[j]["u1"].ToString(), "where dtime between '" + shijian.ToString() + shang_time + "' and '" + shijian.ToString() + xia_time + "'").ToString();
                        }
                        else
                        {
                            model_data = model_data + "," + my_b.get_count(dt.Rows[j]["u1"].ToString(), "where dtime between #" + shijian.ToString() + shang_time + "# and #" + shijian.ToString() + xia_time + "#").ToString();
                        }
                    }
                   // Response.Write("where dtime between '" + shijian.ToString() + shang_time + "' and '" + shijian.ToString() + xia_time + "'<br>");

                }
                //Response.End();
                model_name = model_name.Replace("{fzw:data}", "["+ model_data + "]");
                if (model == "")
                {
                    model = model_name;
                }
                else
                {
                    model = model+","+model_name;
                }
            }


            //统计 end

            Repeater1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid>0 and u5<>'' and u6<>'' order by id asc");
            Repeater1.DataBind();

//            try
//            {
//                Literal13.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user ").Rows[0]["count_id"].ToString();
//            }
//            catch { }
//            try
//            {
//                Literal14.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "huoguoorder ").Rows[0]["count_id"].ToString();
//        }
//            catch { }
//            try
//            {
//                Literal15.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "order ").Rows[0]["count_id"].ToString();

//                Literal16.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "yuyue ").Rows[0]["count_id"].ToString();
//    }
//            catch { }
//            try
//            {
//                Literal17.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "product ").Rows[0]["count_id"].ToString();
//}
//            catch { }
//            try
//            {
//                Literal18.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "dianpu ").Rows[0]["count_id"].ToString();
//            }
//            catch { }
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

                string chaoshi = Environment.TickCount.ToString().Replace("-", "");
                Literal8.Text = ((int.Parse(chaoshi.ToString()) / 0x3e8) / 60).ToString();

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

                Literal11.Text = Server.MachineName.ToString();

            }
            catch
            { }
            try
            {
                if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
                {
                    Literal12.Text = "SQL SERVER";
                }
                else
                {
                    Literal12.Text = "ACCESS";
                }
                    
            }
            catch
            { }




        }
    }
   
}
