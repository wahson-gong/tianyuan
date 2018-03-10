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
public partial class show_hetong : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string t1 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string hetong = Request["url"];
            if (hetong != "")
            {
                string[] aa = Regex.Split(hetong, "\r\n");
                if (hetong.IndexOf("\r\n") == -1)
                {
                    aa = hetong.Split('|');
                }
                Repeater1.DataSource = aa;
                Repeater1.DataBind();

            }

        }
    }
}
