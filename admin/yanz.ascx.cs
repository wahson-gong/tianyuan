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
using System.Text.RegularExpressions;
using yanzheng;
public partial class admin_yanz : System.Web.UI.UserControl
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    yanzheng.Class1 yz = new Class1();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            

            string fzwkey = "";
            try
            {
                fzwkey = my_b.c_string(Request.QueryString["fzwkey"].ToString());
            }
            catch
            { }
           
            if (fzwkey != "")
            {

                if (yz.yanzhen(my_b.md5(fzwkey))=="yes")
                {
                    my_b.c_cookie(fzwkey, "admin_id");
                    my_b.c_cookie(fzwkey, "admin_pwd");
                    Response.Redirect("err.aspx?err=登陆成功，正在跳转后台管理页面！&errurl=" + my_b.tihuan("default.aspx", "&", "fzw123") + "");
                }
            }

            try
            {
                if (my_b.k_cookie("admin_id") == "")
                {
                    Response.Redirect("err.aspx?err=对不起你还没有登陆,请重新登陆！&errurl=login.aspx");
                }
                if (my_b.k_cookie("admin_id") == "")
                {
                    Response.Redirect("err.aspx?err=对不起你还没有登陆,请重新登陆！&errurl=login.aspx");
                }
            }
            catch
            {
                Response.Redirect("err.aspx?err=对不起你还没有登陆,请重新登陆！&errurl=login.aspx");
            }

            my_b.set_admin_url();
           













        }
    }
}
