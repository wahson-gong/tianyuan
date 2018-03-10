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
using System.Text.RegularExpressions;
public partial class admin_login : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (my_b.get_count(ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin", "") == 0)
                {
                    my_b.set_xml("install", "0");
                    Response.Redirect("err.aspx?err=程序未安装，请安装在进后台操作！&errurl=" + my_b.tihuan("../install/default.aspx", "&", "fzw123") + "");
                   
                }
            }
            catch
            {
                my_b.set_xml("install", "0");
                Response.Redirect("err.aspx?err=程序未安装，请安装在进后台操作！&errurl=" + my_b.tihuan("../install/default.aspx", "&", "fzw123") + "");
                
            }

            


        }
    }




 
}
