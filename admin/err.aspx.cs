using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class fzw_admin_err : System.Web.UI.Page
{
    public string errurl = "";
    my_basic my_b = new my_basic();
    public string count_s = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {

                errurl = my_b.tihuan(Request.QueryString["errurl"].ToString(), "fzw123", "&");

            }
            catch
            {
                errurl = "default.aspx";
            }
            try
            {

                count_s = Request.QueryString["count_s"].ToString();

            }
            catch
            {
                count_s = "5";
            }
            Label1.Text = Request.QueryString["err"].ToString();
        }
    }
}
