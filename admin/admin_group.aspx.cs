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

public partial class admin_usergroup : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string key = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
          
            string search_sql = "id>0";
            string page = "1";
            try
            {
                page = Request.QueryString["page"].ToString();
            }
            catch
            { }
            try
            {
                search_sql = Request.QueryString["search_sql"].ToString();
            }
            catch
            { }
            try
            {
                key = Request.QueryString["key"].ToString();
            }
            catch
            { }
            my_b.page_list("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup", search_sql, "*", "order by id desc", 40, int.Parse(page), Repeater1, "usergroup.aspx?search_sql=" + Server.HtmlDecode(search_sql) + "&page=$page$", Literal1);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (my_b.c_string(this.TextBox1.Text) == "")
        {
            Response.Redirect("usergroup.aspx?search_sql=" + Server.HtmlEncode("id>0") + "");
        }
        else
        {
            Response.Redirect("usergroup.aspx?search_sql=" + Server.HtmlEncode("zuming like '%" + my_b.c_string(this.TextBox1.Text) + "%'") + "");
        }

    }
}
