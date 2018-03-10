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
public partial class admin_admin_table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    public string touxiang_show = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            DropDownList1.DataSource = my_c.GetTable("select zuming from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup order by id asc");
            DropDownList1.DataTextField = "zuming";
            DropDownList1.DataValueField = "zuming";
            DropDownList1.DataBind();
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }


            if (type == "edit")
            {

                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                this.TextBox1.Enabled = false;
                for (int i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == dt.Rows[0]["u3"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
                this.xingming.Text = dt.Rows[0]["xingming"].ToString();
                this.xingbie.Text = dt.Rows[0]["xingbie"].ToString();
                this.shouji.Text = dt.Rows[0]["shouji"].ToString();
                this.touxiang.Text = dt.Rows[0]["touxiang"].ToString();
                touxiang_show = dt.Rows[0]["touxiang"].ToString();
                this.youxiang.Text = dt.Rows[0]["youxiang"].ToString();
                this.openid.Text = dt.Rows[0]["openid"].ToString();

            }
            else if (type == "del")
            {
                #region 删除管理员
                if (Request.QueryString["id"].ToString() == "")
                {
                    my_b.tiaozhuan("请选择要删除的信息！", Request.UrlReferrer.ToString());
                }
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where id in (" + Request.QueryString["id"].ToString() + ")");
                if (dt.Rows[0]["u1"].ToString() == my_b.k_cookie("admin_id"))
                {
                    my_b.tiaozhuan("不能删除当前使用帐号！", Request.UrlReferrer.ToString());
                }
                else
                {
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where id in (" + Request.QueryString["id"].ToString() + ")");
                    my_b.tiaozhuan("理员帐号删除成功！", "admin_table.aspx");
                }
                #endregion


            }
            else
            {

            }

        }
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string u2 = my_b.md5(my_b.c_string(this.TextBox2.Text));
        string u3 = DropDownList1.SelectedValue;
        #region 判断新增超级管理员
        if (u1 != my_b.k_cookie("admin_id"))
        {
            if (u3 == "超级管理员")
            {
                if (my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u3 like '" + u3 + "'").Rows.Count > 0)
                {
                    Response.Redirect("err.aspx?err=只能增加一个超级管理员！&errurl=" + my_b.tihuan("admin_table.aspx", "&", "fzw123") + "");
                }
            }
        }

        #endregion
        string xingming = my_b.c_string(this.xingming.Text);
        string xingbie = my_b.c_string(this.xingbie.Text);
        string shouji = my_b.c_string(this.shouji.Text);
        string touxiang = my_b.c_string(this.touxiang.Text);
        string youxiang = my_b.c_string(this.youxiang.Text);
        string openid = my_b.c_string(this.openid.Text);

        string sql_string = "";
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            sql_string = "update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin set u1='" + u1 + "',u3='" + u3 + "',xingming='" + xingming + "',xingbie='" + xingbie + "',shouji='" + shouji + "',touxiang='" + touxiang + "',youxiang='" + youxiang + "',openid='" + openid + "'";

            if (this.TextBox2.Text != "")
            {
                sql_string = sql_string + ",u2='" + u2 + "'";
            }
            sql_string = sql_string + " where id=" + Request.QueryString["id"].ToString();
            my_c.genxin(sql_string);
            Response.Redirect("err.aspx?err=修改管理员信息成功！&errurl=" + my_b.tihuan("admin_table.aspx", "&", "fzw123") + "");
        }
        else
        {

            if (this.TextBox2.Text == "")
            {
                Response.Redirect("err.aspx?err=在新建帐号时密码不可以为空！&errurl=" + my_b.tihuan("admin_table_add.aspx", "&", "fzw123") + "");
            }
            else
            {
                if (my_b.get_count("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin", "where u1='" + u1 + "'") > 0)
                {
                    Response.Redirect("err.aspx?err=帐号：" + u1 + "已存在，请重新申请！&errurl=" + my_b.tihuan("admin_table_add.aspx", "&", "fzw123") + "");
                }
                sql_string = "insert into  " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin (u1,u2,u3,xingming,xingbie,shouji,touxiang,youxiang,openid) values('" + u1 + "','" + u2 + "','" + u3 + "','" + xingming + "','" + xingbie + "','" + shouji + "','" + touxiang + "','" + youxiang + "','" + openid + "')";
                my_c.genxin(sql_string);
                Response.Redirect("err.aspx?err=新建管理员帐号成功，请记住你的密码！&errurl=" + my_b.tihuan("admin_table.aspx", "&", "fzw123") + "");
            }

        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
