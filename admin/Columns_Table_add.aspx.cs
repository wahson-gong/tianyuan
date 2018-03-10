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
public partial class admin_Columns_Table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    DataTable dt = new DataTable();
    public string TextBox3_pic = "";
    int jj = 0;
    string classid = "0";
    public void dr1(string t1, int t2)
    {

        DataTable dt1 = my_c.GetTable("select id,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=" + t1 + " ");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "—";
                }
                DropDownList3.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                DropDownList3.Items[jj].Value = dt1.Rows[j]["id"].ToString();
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


            dr1("0", 0);
            DropDownList3.Items.Insert(0, "顶级目录");
            DropDownList3.Items[0].Value = "0";

            TextBox3_pic = "ficon-lanmu";

            int i = 0;

            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                this.TextBox2.Text = dt.Rows[0]["u3"].ToString();
                this.TextBox3.Text = dt.Rows[0]["u5"].ToString();
        
                for (i = 0; i < DropDownList2.Items.Count; i++)
                {
                    if (DropDownList2.Items[i].Value == dt.Rows[0]["u6"].ToString())
                    {
                        DropDownList2.Items[i].Selected = true;
                    }
                }

                TextBox3_pic = dt.Rows[0]["u5"].ToString();
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == dt.Rows[0]["u2"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }

                for (i = 0; i < DropDownList3.Items.Count; i++)
                {
                    if (DropDownList3.Items[i].Value == dt.Rows[0]["classid"].ToString())
                    {
                        DropDownList3.Items[i].Selected = true;
                    }
                }

                for (i = 0; i < RadioButtonList1.Items.Count; i++)
                {
                    if (RadioButtonList1.Items[i].Value == dt.Rows[0]["u4"].ToString())
                    {
                        RadioButtonList1.Items[i].Selected = true;
                    }
                }

                if (int.Parse(DropDownList3.SelectedItem.Value.ToString()) == 0)
                {
                    Panel1.Visible = true;
                    Panel2.Visible = false;
                }
                else
                {
                    Panel2.Visible = true;
                    Panel1.Visible = false;
                }
                classid = dt.Rows[0]["id"].ToString();
            }
            else if (type == "del")
            {
                if (Request.QueryString["id"].ToString() == "")
                {
                    Response.Redirect("err.aspx?err=请选择要删除的信息！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
                string id = Request.QueryString["id"].ToString();
                DataTable dt = new DataTable();
                dt = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=" + id + "");
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("err.aspx?err=对不起还有下级分类，如果想删除分类必须从最底级分类开始删除！&errurl=" + my_b.tihuan("Columns_Table.aspx", "&", "fzw123") + "");
                }
                else
                {

              //      string r_id = my_b.get_value("classid", "shop_lanmu", "where id=" + id + "");
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where id in (" + id + ")");

                    Response.Redirect("err.aspx?err=删除栏目成功！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }

            }
            else if (type == "shengcheng")
            {
                
                Response.Redirect("err.aspx?err=添加栏目成功！&errurl=default.aspx");
            }
            else
            {
                for (i = 0; i < DropDownList3.Items.Count; i++)
                {
                    if (DropDownList3.Items[i].Value == Request.QueryString["classid"].ToString())
                    {
                        DropDownList3.Items[i].Selected = true;
                    }
                }

                if (int.Parse(DropDownList3.SelectedItem.Value.ToString()) == 0)
                {
                    Panel1.Visible = true;
                    Panel2.Visible = false;
                }
                else
                {
                    Panel2.Visible = true;
                    Panel1.Visible = false;
                }
                if (Request.QueryString["classid"].ToString() != "")
                {
                    classid = Request.QueryString["classid"].ToString();
                }
            }
           
            
           
        }
    }



    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (int.Parse(DropDownList3.SelectedItem.Value.ToString()) == 0)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
        else
        {
            Panel2.Visible = true;
            Panel1.Visible = false;
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string classid = DropDownList3.SelectedItem.Value;
        string u2 = DropDownList1.SelectedItem.Value;
        string u3 = my_b.c_string(this.TextBox2.Text);
        string u4 = RadioButtonList1.SelectedItem.Value;
        string u5 = my_b.c_string(this.TextBox3.Text);
        string u6 = DropDownList2.SelectedValue;

        if (int.Parse(classid) == 0)
        {
            u3 = "";
        }
        else
        {
            u2 = "";
        }
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }
        if (type == "edit")
        {
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns set u1='" + u1 + "',classid=" + classid + ",u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',u5='" + u5 + "',u6='" + u6 + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改栏目成功！马上跳转到栏目管理页面！&errurl=" + my_b.tihuan("Columns_Table.aspx?classid=" + classid + "", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns (u1,classid,u2,u3,u4,u5,u6) values('" + u1 + "'," + classid + ",'" + u2 + "','" + u3 + "','" + u4 + "','" + u5 + "','" + u6 + "')");
            Response.Redirect("err.aspx?err=增加栏目成功！马上跳转到栏目管理页面！&errurl=" + my_b.tihuan("Columns_Table.aspx", "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
