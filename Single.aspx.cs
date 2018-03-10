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
using System.Text;
using System.Text.RegularExpressions;

public partial class Single : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_b.set_openid();
            my_b.fenxiaolink();
         

            string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + Request.QueryString["m"].ToString() + ".html"), Encoding.UTF8);
            Response.Write(my_h.Single_page(file_content));


        }
    }
}
