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
public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string get_shouji(string mobile)
    {
        string mobilecontent= my_b.getWebFile("http://www.ip138.com:8080/search.asp?action=mobile&mobile="+ mobile);
        Regex reg = new Regex("卡号归属地.*?</TR>", RegexOptions.Singleline);
        Match matches = reg.Match(mobilecontent);
        mobilecontent = my_b.NoHTML(matches.ToString()).Replace("卡号归属地","").Replace(" ","");
        return mobilecontent;
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Response.Write(get_shouji("18180229615"));
            Response.End();
            


        }
    }
}
