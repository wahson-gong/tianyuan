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

public partial class admin_admin_table : System.Web.UI.Page
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
                key = Request.QueryString["key"].ToString();
            }
            catch
            { }
            if (key != "")
            {
                search_sql = "u1 like '%" + my_b.c_string(key) + "%'";
            }
            my_b.page_list("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin", search_sql, "*", "order by id desc", 40, int.Parse(page), Repeater1, "admin_table.aspx?key=" + key + "&page=$page$", Literal1);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("admin_table.aspx?key=" + this.TextBox1.Text + "");

    }
}
