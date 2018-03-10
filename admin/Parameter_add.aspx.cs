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
public partial class admin_Parameter_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    DataTable dt = new DataTable();
    int jj = 0;
    public void dr1(string t1, int t2)
    {
        if (t2 < 7)
        {
            DataTable dt1 = my_c.GetTable("select u1,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid =" + t1 + " order by id ");

            if (dt1.Rows.Count > 0)
            {


                for (int j = 0; j < dt1.Rows.Count; j++)
                {

                    string bb = "";

                    for (int j1 = 0; j1 < t2; j1++)
                    {
                        bb = bb + "—";
                    }
                    DropDownList1.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                    DropDownList1.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                    jj = jj + 1;
                    int tt1 = t2 + 1;
                    dr1(dt1.Rows[j]["id"].ToString(), tt1);
                }
            }
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            int i = 0;
            dr1("0", 0);
            DropDownList1.Items.Insert(0, "顶级");
            DropDownList1.Items[0].Value = "0";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == dt.Rows[0]["classid"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
                
                this.TextBox2.Text = dt.Rows[0]["u2"].ToString();
                this.TextBox3.Text = dt.Rows[0]["u3"].ToString();
                this.TextBox4.Text = dt.Rows[0]["u4"].ToString();
                if (dt.Rows[0]["u3"].ToString() == "")
                {
                    this.TextBox3.Text = "0";
                }
            }
            else if (type == "del")
            {
                if (Request.QueryString["id"].ToString() == "")
                {
                    Response.Redirect("err.aspx?err=请选择要删除的信息！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
                //my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in("+Request.QueryString["id"].ToString()+") ");
                DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["id"].ToString() + ")");
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id =" + dt1.Rows[i]["id"].ToString() + " ");
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=" + dt1.Rows[i]["id"].ToString() + "");
                }
                Response.Redirect("err.aspx?err=删除参数成功！马上跳转到参数页面！&errurl=" + my_b.tihuan("Parameter.aspx", "&", "fzw123") + "");

            }
            else
            {
                
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == Request.QueryString["classid"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
                this.TextBox3.Text = my_b.get_count("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter", "where classid=" + DropDownList1.SelectedItem.Value + "").ToString();
            }
        }
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string classid = DropDownList1.SelectedItem.Value;
        string u2 = my_b.c_string(this.TextBox2.Text);
        string u3 = my_b.c_string(this.TextBox3.Text);
        string u4 = my_b.c_string(this.TextBox4.Text);
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }
        if (type == "edit")
        {

            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter set  u1='" + u1 + "',u2='" + u2 + "',classid=" + classid + ",u3=" + u3 + ",u4='" + u4 + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改参数成功！马上跳转到参数页面！&errurl=" + my_b.tihuan("Parameter.aspx?classid=" + classid + "", "&", "fzw123") + "");
        }
        else
        {
            string[] aa = Regex.Split(u1, "\r\n");
            for (int j = 0; j < aa.Length; j++)
            {
                if (aa[j].Trim() != "")
                {
                    my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter(u1,u2,u3,u4,classid) values('" + aa[j] + "','" + u2 + "'," + u3 + ",'" + u4 + "'," + classid + ")");
                }
                 
            }
               
            Response.Redirect("err.aspx?err=增加参数成功！马上跳转到参数增加页！&errurl=" + my_b.tihuan("Parameter_add.aspx?classid=" + classid + "", "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
