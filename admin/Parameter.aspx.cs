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

public partial class admin_Parameter : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string classid = "0";
    public string getstring(string g1)
    {
        if (g1 == "0")
        {
            return "顶级";
        }
        else
        {
            return my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + g1).Rows[0]["u1"].ToString();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            string search_sql = "";
            string page = "1";
            string key = "";
            try
            {
                page = Request.QueryString["page"].ToString();
            }
            catch
            { }
            try
            {
                key = Request.QueryString["key"].ToString();
                this.TextBox1.Text = key;
            }
            catch
            { }
            try
            {
                classid = Request.QueryString["classid"].ToString();
            }
            catch
            { }


            if (classid != "")
            {
                if (search_sql == "")
                {
                    search_sql = "classid=" + classid + "";
                }
                else
                {
                    search_sql = search_sql + " and " + "classid=" + classid + "";
                }
            }
            if (search_sql == "")
            {
                search_sql = "u1 <> ''";
            }
            #region 查询
            if (key != "")
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where u1 like '%" + key + "%'");
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("Parameter.aspx?classid=" + dt.Rows[0]["id"].ToString() + "");
                }

            }

            if (my_b.get_count("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter", "where " + search_sql + "") > 0)
            {
                my_b.page_list("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter", search_sql, "*", "order by id desc", 40, int.Parse(page), Repeater1, "Parameter.aspx?classid=" + classid + "&key=" + key + "&page=$page$", Literal1);
            }
            else
            {
                Response.Redirect("Parameter_add.aspx?type=edit&id=" + classid + "");
            }
            #endregion

        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Parameter.aspx?key=" + my_b.c_string(this.TextBox1.Text) + "");
    }
}
