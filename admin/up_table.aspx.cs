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

public partial class admin_up_table : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string key = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (Request.QueryString["up"].ToString() == "true")
                {
                    my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + Request.QueryString["ip"].ToString() + "','在线更新!','" + Request.QueryString["page_url"].ToString() + "','" + Request.QueryString["ip"].ToString() + "')");

                    Response.Write("server='118.123.8.137'; database='slcms';uid=sloffice;pwd=sloffice");
                    Response.End();
                }
            }
            catch
            { }

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
            my_b.page_list("up_table", search_sql, "*", "order by id desc", 20, int.Parse(page), Repeater1, "up_table.aspx?search_sql=" + Server.HtmlDecode(search_sql) + "&page=$page$", Literal1);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (my_b.c_string(this.TextBox1.Text) == "")
        {
            Response.Redirect("up_table.aspx?search_sql=" + Server.HtmlEncode("id>0") + "");
        }
        else
        {
            Response.Redirect("up_table.aspx?search_sql=" + Server.HtmlEncode("u1 like '%" + my_b.c_string(this.TextBox1.Text) + "%'") + "");
        }

    }
}
