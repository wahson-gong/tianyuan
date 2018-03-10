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

public partial class admin_Model : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
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
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }
            if (type == "quanxian")
            {
                my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model set fangwen='是',fangwen_type='all' where fangwen is null or fangwen=''");
            }
            Repeater1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model order by id");
            Repeater1.DataBind();

        }
    }
}
