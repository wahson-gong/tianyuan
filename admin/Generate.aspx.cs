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

public partial class admin_Generate : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int jj = 0;
    int jj1 = 0;
    public void dr1(string t1, int t2, string t3)
    {
        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + " and Model_id=" + t3 + " order by id");
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                string bb = "";
                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "—";
                }
                ListBox1.Items.Insert(jj1, bb + dt1.Rows[j]["u1"].ToString());
                ListBox1.Items[jj1].Value = dt1.Rows[j]["id"].ToString();
                jj1 = jj1 + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1, t3);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Panel1.Visible = false;

            DropDownList1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u3 like '文章模型' order by id");
            DropDownList1.DataTextField = "u2";
            DropDownList1.DataValueField = "id";
            DropDownList1.DataBind();

            dr1("0", 0, DropDownList1.SelectedItem.Value);


            ListBox1.Items.Insert(0, "所有栏目");
            ListBox1.Items[0].Value = "0";
            ListBox1.Items[0].Selected = true;

        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();
        dr1("0", 0, DropDownList1.SelectedItem.Value);
        ListBox1.Items.Insert(0, "所有栏目");
        ListBox1.Items[0].Value = "0";
        ListBox1.Items[0].Selected = true;

    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonList1.SelectedValue == "内容页")
        {
            Panel1.Visible = true;
        }
        else
        {
            Panel1.Visible = false;
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (RadioButtonList1.SelectedValue == "内容页")
        {
            Response.Write("<script>window.location='err.aspx?count_s=2&err=正在跳转生成静态处理页！&errurl=" + my_b.tihuan("Generates.aspx?Model_id=" + DropDownList1.SelectedItem.Value + "&classid=" + ListBox1.SelectedItem.Value + "&type=" + RadioButtonList1.SelectedItem.Value + "&order_type=" + RadioButtonList2.SelectedItem.Value + "&order_by=" + RadioButtonList3.SelectedItem.Value + "&Generate_count=" + my_b.c_string(this.TextBox1.Text) + "", "&", "fzw123") + "'</script>");
        }
        else
        {
            Response.Write("<script>window.location='err.aspx?count_s=2&err=正在跳转生成静态处理页！&errurl=" + my_b.tihuan("Generates.aspx?Model_id=" + DropDownList1.SelectedItem.Value + "&classid=" + ListBox1.SelectedItem.Value + "&type=" + RadioButtonList1.SelectedItem.Value + "", "&", "fzw123") + "'</script>");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
