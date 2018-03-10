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

public partial class setsql : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable dt = my_c.GetTable("select top 20 * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "movie order by id desc");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t1 = dt.Rows[i]["u3"].ToString().Replace("<A href=\"http://www.eee4.cc/\" target=_blank><SPAN style=\"COLOR: black\">3E电影下载站</SPAN></A>", "<A href=\"http://www.qianqiandy.com/\" target=_blank><SPAN style=\"COLOR: black\">千千电影下载网</SPAN></A>").Replace("<A href=\"http://www.3edyy.com/\" target=_blank><SPAN style=\"COLOR: black\">电影下载</SPAN></A>", "<A href=\"http://www.qianqiandy.com/\" target=_blank><SPAN style=\"COLOR: black\">电影下载</SPAN></A>");

                my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "movie set u3='"+my_b.c_string(t1)+"' where id=" + dt.Rows[i]["id"].ToString());
                Response.Write(dt.Rows[i]["id"].ToString()+"行<br>");
                Response.Flush();
            }
            Response.End();
        }
    }
}
