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
using System.Text;
using System.Text.RegularExpressions;


public partial class fzw_admin_err : System.Web.UI.Page
{
    public string errurl = "";
    my_basic my_b = new my_basic();
    public string count_s = "";
    my_html my_h = new my_html();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }

            if (type == "cookie")
            {
                my_b.admin_o_cookie("user_id");
                my_b.admin_o_cookie("user_pwd");
                my_b.admin_o_cookie("user_no");
            }
            try
            {

                errurl = my_b.c_string(my_b.tihuan(Request.QueryString["errurl"].ToString(), "fzw123", "&"));

            }
            catch
            {
                errurl = "default.aspx";
            }
            try
            {

                count_s = my_b.t_string(Request.QueryString["count_s"].ToString());

            }
            catch
            {
                count_s = "3";
            }
           // Label1.Text = Request.QueryString["err"].ToString();
            if (count_s == "0")
            {
                Response.Redirect(errurl);
            }
            string err = "";
            try
            {
                err = my_b.t_string(Request.QueryString["err"].ToString());
            }
            catch { }
            if (err == "")
            {
                Response.Redirect(errurl);
            }
            string file_content = "";
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/err.html"))
            {
                file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/err.html", Encoding.UTF8);
            }
            else
            {
                file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/inc/err.html", Encoding.UTF8);
            }
            //替换

            file_content = file_content.Replace("{count_s}", count_s);
            file_content = file_content.Replace("{errurl}", errurl);
            file_content = file_content.Replace("{err}", my_b.t_string(Request.QueryString["err"].ToString()));
            //end
            Response.Write(my_h.Single_page(file_content));

        }
    }
}
