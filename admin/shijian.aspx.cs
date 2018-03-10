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
public partial class admin_Ad_Table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

        }
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        string shang_time = this.TextBox1.Text+" 00:00:01";
        string xia_time = this.TextBox2.Text + " 23:59:59";
        DataTable sl_user = my_c.GetTable("select * from sl_user order by id desc");
        for(int i=0;i< sl_user.Rows.Count;i++)
        {
            DataTable dt1 = my_c.GetTable("select sum(shijian) as shijian from sl_time where dtime between '" + shang_time + "' and '" + xia_time + "'  and yonghuming='" + sl_user.Rows[i]["yonghuming"].ToString() + "'");
            string zaixianshijian = "0";
            if (dt1.Rows.Count>0)
            {
                zaixianshijian = dt1.Rows[0]["shijian"].ToString();

            }
            my_c.genxin("update sl_user set zaixianshijian=" + zaixianshijian + " where id=" + sl_user.Rows[i]["id"].ToString() + "");
        }

        Response.Redirect("err.aspx?err=会员排名更新成功！&errurl=" + my_b.tihuan("shijian.aspx", "&", "fzw123") + "");
    }

}
