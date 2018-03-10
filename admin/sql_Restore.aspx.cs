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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
public partial class admin_sql_Restore : System.Web.UI.Page
{
    public int i = 0;
    public string getfile(string g1)
    {
        return Directory.GetCreationTime(g1).ToString();
    }
    public string get_size(string g1)
    {
        FileInfo f = new FileInfo(g1);
        return long.Parse(f.Length.ToString()).ToString() + " KB";
    }
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string type = "";
    public void getsql()
    {
        if (type == "del")
        {
            string[] aa = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\sql\");
            File.Delete(aa[int.Parse(Request.QueryString["id"].ToString())].ToString());
            Response.Redirect("err.aspx?err=删除备份文件成功！&errurl=" + my_b.tihuan("sql_Restore.aspx", "&", "fzw123") + "");
        }
        else if (type == "huifu")
        {
            string[] aa = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\sql\");
            my_c.genxin("use master restore database " + Regex.Match(ConfigurationSettings.AppSettings["sql_conn"].ToString(), "database='.*?'").ToString().Replace("database=", "").Replace("'", "") + " from disk='" + aa[int.Parse(Request.QueryString["id"].ToString())].ToString() + "'");
            Response.Redirect("err.aspx?err=数据库已经成功恢复到" + getfile(aa[int.Parse(Request.QueryString["id"].ToString())].ToString()) + "！&errurl=" + my_b.tihuan("sql_Restore.aspx", "&", "fzw123") + "");
        }
        else
        {
            string[] aa = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\sql\");
            Repeater1.DataSource = aa;
            Repeater1.DataBind();
        }
    }
    public void getacces()
    {
        if (type == "del")
        {
            string[] aa = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\access\");
            File.Delete(aa[int.Parse(Request.QueryString["id"].ToString())].ToString());
            Response.Redirect("err.aspx?err=删除备份文件成功！&errurl=" + my_b.tihuan("sql_Restore.aspx", "&", "fzw123") + "");
        }
        else if (type == "huifu")
        {
            string[] aa = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\access\");
            string access = File.ReadAllText(aa[int.Parse(Request.QueryString["id"].ToString())].ToString(), System.Text.Encoding.UTF8).Replace("$Prefix$", ConfigurationSettings.AppSettings["Prefix"].ToString());
            string[] bb = Regex.Split(access, "sqlnextsql");
            for (int j = 0; j < bb.Length; j++)
            {
                try
                {
                    my_c.genxin(bb[j].ToString());
                    Response.Write("已执行" + j.ToString() + "行，还有行" + aa.Length.ToString() + "未执行。");
                    Response.Flush();
                }
                catch
                {

                }
            }

            Response.Redirect("err.aspx?err=数据库已经成功恢复到" + getfile(aa[int.Parse(Request.QueryString["id"].ToString())].ToString()) + "！&errurl=" + my_b.tihuan("sql_Restore.aspx", "&", "fzw123") + "");

        }
        else
        {
            string[] aa = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\access\");
            Repeater1.DataSource = aa;
            Repeater1.DataBind();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            {

            }
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                getsql();
            }
            else
            {
                getacces();
            }

        }
    }
}
