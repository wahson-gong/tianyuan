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

public partial class out_login : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("micromessenger") > -1)
            {
                try
                {
                    my_c.GetTable("update sl_user set openid='' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                }
                catch { }
            }

            try
            {
                my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name").ToString() + "','此用户名（" + my_b.k_cookie("user_name").ToString() + "）会员退出成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','会员退出')");
                
                my_b.admin_o_cookie("user_name");
                my_b.admin_o_cookie("user_pwd");
                my_b.admin_o_cookie("user_no");

            }
            catch
            { }
            Response.Redirect("/err.aspx?err=会员退出成功，正在返回首页！&errurl=" + my_b.tihuan("/default.aspx", "&", "fzw123") + "");
        }
    }
}
