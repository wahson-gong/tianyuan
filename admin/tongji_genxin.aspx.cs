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

public partial class admin_Ad_Table : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    public my_conn my_c = new my_conn();
    public string key = "";
    public string model = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable dt = my_c.GetTable("select  * from sl_Parameter where classid=251 order by u1 desc");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (model == "")
                {
                    model = "{ date: \"" + dt.Rows[i]["u1"].ToString() + "\", value: " + my_b.get_count("sl_wenzhang", "where dtime like'%" + dt.Rows[i]["u1"].ToString() + "%'") + " }";
                }
                else
                {
                    model = model + ",{ date: \"" + dt.Rows[i]["u1"].ToString() + "\", value: " + my_b.get_count("sl_wenzhang", "where dtime like'%" + dt.Rows[i]["u1"].ToString() + "%'") + " }";
                }
            }
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
          
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {


    }
}
