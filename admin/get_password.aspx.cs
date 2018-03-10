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

public partial class admin_get_password : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "edit";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }


            if (type == "edit")
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + my_b.k_cookie("admin_id") + "'");
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                this.TextBox1.Enabled = false;

            }

        }
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string u2 = my_b.md5(my_b.c_string(this.TextBox2.Text));
        string u3 = my_b.md5(my_b.c_string(this.TextBox3.Text));
        string sql_string = "";
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + u1 + "' and u2='" + u2 + "'").Rows.Count > 0)
        {
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin set u2='" + u3 + "' where u1='" + u1 + "' and u2='" + u2 + "'");
            Response.Redirect("err.aspx?err=修改管理员信息成功！&errurl=" + my_b.tihuan("get_password.aspx", "&", "fzw123") + "");
        }
        else
        {
            Response.Redirect("err.aspx?err=修改管理员信息失败，旧密码不对！&errurl=" + my_b.tihuan("get_password.aspx", "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
