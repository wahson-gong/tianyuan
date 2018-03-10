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

public partial class admin_Fields : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string biaoming = "";
    public string get_url(string u1, string u2, string type)
    {
        string t1 = u1 + "?Model_id=" + u2;
        if (type == "no")
        {
            return t1;
        }
        else
        {
            return my_b.tihuan(t1, "&", "fzw123");
        }
        return "";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            Repeater2.DataSource = Model_dt;
            Repeater2.DataBind();

            Repeater1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id="+Request.QueryString["Model_id"].ToString()+" order by u9,id");
            Repeater1.DataBind();

        }
    }
}
