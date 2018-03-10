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

public partial class tiaozhuan : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string url = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/"));
            //Response.Write(Request.QueryString.ToString());
            //Response.End();
            string xinlianjie = Request.QueryString.ToString().Replace("%7c", "&").Replace("%3d", "=");
            string[] quer_arr = xinlianjie.Split('&');
            string canshu = "";
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (quer_arr[i].ToString().IndexOf("pagename=") == -1)
                {
                    if (canshu == "")
                    {
                        canshu = quer_arr[i].ToString();
                    }
                    else
                    {
                        canshu = canshu+"&"+quer_arr[i].ToString();
                    }
                }
                else
                {
                    url = url.Replace("/inc","") + "/" + quer_arr[i].ToString().Replace("pagename=", "");
                }
            }
            url = url + "?" + canshu;
            //Response.Write(url);
            //Response.End();
            Response.Redirect(url);
        }
    }
}