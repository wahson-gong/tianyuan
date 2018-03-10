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
public partial class admin_Model_move : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable dt = new DataTable();
    int jj = 0;
    int i = 0;
    public void dr1(string t1, int t2)
    {

        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=" + t1 + " ");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "— ";
                }

                DropDownList1.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                DropDownList1.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1);
            }
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            TextBox1.Text = Request.QueryString["name"].ToString();
            Literal1.Text = Request.QueryString["url"].ToString().Replace("fzw123", "&");
            dr1("0", 0);
            try
            {
                my_b.c_cookie(Request.UrlReferrer.ToString(), "shangji");
            }
            catch { }
        }
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string classid = DropDownList1.SelectedItem.Value;
        string u3 = Request.QueryString["url"].ToString().Replace("fzw123", "&");
        string u4 = "显示";
        my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns (u1,classid,u3,u4) values('" + u1 + "'," + classid + ",'" + u3 + "','" + u4 + "')");
        Response.Redirect("err.aspx?err=增加栏目成功！马上生成栏目！&errurl=" + my_b.tihuan(my_b.k_cookie("shangji"), "&", "fzw123") + "");
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
