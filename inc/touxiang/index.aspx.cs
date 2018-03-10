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
public partial class touxiang_index : System.Web.UI.Page
{
    public string avatar_sizes = "100*100";
    public string editname = "";
    public string imgurl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                avatar_sizes = Request.QueryString["avatar_sizes"].ToString();
            }
            catch { }
            try
            {
                editname = Request.QueryString["editname"].ToString();
            }
            catch { }
            try
            {
                imgurl = Request.QueryString["imgurl"].ToString();
            }
            catch { }

        }
    }
 
}
