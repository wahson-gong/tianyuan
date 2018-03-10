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

public partial class admin_Columns_Table : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable dt = new DataTable();
    public string classid = "0";

    public string getstring(string g1)
    {
        if (g1 == "0")
        {
            return "顶级栏目";
        }
        else
        {
            return my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where id=" + g1).Rows[0]["u1"].ToString();
        }
    }


    public string get_class(string g1)
    {
        return my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid =" + g1 + "").Rows[0]["count_id"].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            try
            {
                classid = Request.QueryString["classid"].ToString();
            }
            catch
            { }

            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=" + classid + " order by id asc");
            if (dt.Rows.Count > 0)
            {
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                Response.Redirect("Columns_Table_add.aspx?type=edit&id=" + classid);
            }

        }
    }
}
