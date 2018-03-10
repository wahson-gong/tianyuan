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

public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    public string set_html(string u7, string u6, string id)
    {
        return my_h.set_Single(File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + u6), Encoding.UTF8), "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single", id);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            my_b.set_openid();
            my_b.fenxiaolink();
        
            DataTable xml_dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");
            string page_name = xml_dt.Rows[0]["u36"].ToString();
            try
            {
                if (Request.QueryString.ToString() != "")
                {
                    page_name = Server.HtmlDecode(Server.HtmlEncode(Request.QueryString.ToString()));
                }
            }
            catch
            { }
            if (page_name.IndexOf("&") > -1)
            {
                page_name = page_name.Substring(0, page_name.IndexOf("&"));
            }


            try
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single where u7 like '%" + page_name + "%'");
                Response.Write(set_html(dt.Rows[0]["u7"].ToString(), dt.Rows[0]["u6"].ToString(), dt.Rows[0]["id"].ToString()));
            }
            catch
            {
                page_name = xml_dt.Rows[0]["u36"].ToString();
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single where u7 like '%" + page_name + "%'");
                Response.Write(set_html(dt.Rows[0]["u7"].ToString(), dt.Rows[0]["u6"].ToString(), dt.Rows[0]["id"].ToString()));
            }


        }
    }
}
