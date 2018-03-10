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
public partial class get_add : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string id = my_b.c_string(Request.QueryString["id"].ToString());
            DataTable dt = my_c.GetTable("select * from sl_add where yonghuming='" + my_b.k_cookie("user_name") + "' and id=" + id + "");
            Response.Write(dt.Rows[0]["shoujianrenxingming"].ToString() + "|");
            Response.Write(dt.Rows[0]["shoujihaoma"].ToString() + "|");
            Response.Write(dt.Rows[0]["suozaidiqu"].ToString() + "|");
            Response.Write(dt.Rows[0]["jiedaodizhi"].ToString());



        }
    }
}
