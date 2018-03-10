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
public partial class admin_shop_Single : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string key = "";
    public string is_file(string g1,string g2)
    {
        if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath+g1))
        {
            return "<a href='" + g1 + "' target=_blank>已生成</a>";
        }
        else
        {
            return "<a href='shop_Single_add.aspx?type=sc&id="+g2+"'>未生成</a>";
        }
    }
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
            my_b.page_list("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single", search_sql, "*", "order by id desc", 40, int.Parse(page), Repeater1, "shop_Single.aspx?search_sql=" + Server.HtmlDecode(search_sql) + "&page=$page$", Literal1);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (my_b.c_string(this.TextBox1.Text) == "")
        {
            Response.Redirect("shop_Single.aspx?search_sql=" + Server.HtmlEncode("id>0") + "");
        }
        else
        {
            Response.Redirect("shop_Single.aspx?search_sql=" + Server.HtmlEncode("u1 like '%" + my_b.c_string(this.TextBox1.Text) + "%'") + "");
        }

    }
}
