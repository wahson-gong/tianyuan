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
public partial class search : System.Web.UI.Page
{
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    my_html my_h = new my_html();
    public string set_str(string g1)
    {
        if (g1.IndexOf("&from=singlemessage") > -1)
        {
            g1 = g1.Replace("&from=singlemessage", "");

        }
        if (g1.IndexOf("&from=timeline") > -1)
        {
            g1 = g1.Replace("&from=timeline", "");

        }
        if (g1.IndexOf("&isappinstalled=0") > -1)
        {
            g1 = g1.Replace("&isappinstalled=0", "");

        }
        return g1;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString.ToString() == "")
            {
                Response.End();
            }
            my_b.set_openid();
            //Regex reg = new Regex("-", RegexOptions.Singleline);
            //string[] bb = reg.Split(Request.QueryString.ToString());
            //string key = "";
            //for (int i = 0; i < bb.Length; i++)
            //{ 

            //}

            //if (my_b.set_mode() == "伪静态")
            //{
            //    Regex reg = new Regex("-", RegexOptions.Singleline);
            //    string[] bb = reg.Split(Request.QueryString.ToString());
            //    if (bb.Length > 1)
            //    {
            //        string key = "";
            //        for (int i = 0; i < bb.Length; i++)
            //        {
            //            if (key == "")
            //            {
            //                int j = i + 1;
            //                key = bb[i].ToString() + "=" + bb[j].ToString();
            //                i = j;
            //            }
            //            else
            //            {
            //                int j = i + 1;
            //                key = key + "&" + bb[i].ToString() + "=" + bb[j].ToString();
            //                i = j;
            //            }

            //        }

            //        my_h.set_search(key);

            //    }
            //}
            //else
            //{
            //Response.Write(my_b.set_url_css(set_str(Request.QueryString.ToString())));
            //Response.End();
            my_h.set_search(my_b.set_url_css(set_str(Request.QueryString.ToString().Replace("&from=timeline", ""))));
            // }


        }
    }
}
