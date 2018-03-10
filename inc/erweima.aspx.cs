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

public partial class Default2 : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    string type = "";
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //替换
            string id = "";
            try
            {
                id = Request.QueryString["id"].ToString();
            }
            catch { }

            my_b.c_cookie(id, "erweima");

            //end
            string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/erweima.html", Encoding.Default);

            Response.Write(my_h.Single_page(file_content));


        }
    }
}
