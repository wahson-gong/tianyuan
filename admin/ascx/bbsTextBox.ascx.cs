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
public partial class ascx_FreeTextBox : System.Web.UI.UserControl
{
    public my_basic my_b = new my_basic();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           
        }
    }


    public string Text
    {
        set { this.TextBox1.Text = value; }
        get { return TextBox1.Text; }
    }
    public string upfile
    {
        set {
           
            if (value!="")
            {

                DateTime dy = DateTime.Now;
                string upfile_path = value;
                my_b.c_cookie("/upfile/user/" + upfile_path+"/", "upfile");
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "/upfile/user/" + upfile_path + "/");
            }
            else
            {
                my_b.c_cookie("/upfile/Editor/", "upfile");
            }
        }
        get { return my_b.k_cookie("upfile").ToString(); }
       
    }
}
