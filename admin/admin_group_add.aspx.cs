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
public partial class admin_usergroup_addd : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    public int i1 = 1;
    public string set_check(string g1, int g2)
    {


        try
        {
            DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup where id=" + Request.QueryString["id"].ToString());



            string quanxian = my_c.GetTable("select quanxian from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup where zuming='" + dt.Rows[0]["zuming"].ToString() + "'").Rows[0]["quanxian"].ToString().Replace("&amp;", "&");
            string page_sta = "";
            Regex reg = new Regex("{fzw:dui}", RegexOptions.Singleline);
            string[] aa = reg.Split(quanxian);
            for (int i = 0; i < aa.Length; i++)
            {
                Regex reg1 = new Regex("{fzw:zu}", RegexOptions.Singleline);
                string[] bb = reg1.Split(aa[i].ToString());

                if (bb[0].ToString() == g1)
                {



                    Regex reg2 = new Regex(",", RegexOptions.Singleline);
                    string[] cc = reg2.Split(bb[1].ToString());

                    if (cc[g2] != "")
                    {
                        return " checked=\"checked\"";
                    }
                    else
                    {
                        return "";
                    }

                }


            }
        }
        catch
        { }


        return "";
    }
    public string set_dt(string classid)
    {
        DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid in (" + classid + ") and u4='显示' order by u2,id desc");
        string t1 = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            t1 = t1 + "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%; text-align:left\"><tr><td style=\"width:200px\" ><span id=\"qx_txt_" + i1 + "\" >" + dt.Rows[i]["u1"].ToString() + "</span>                         <span id=\"qx_value_" + i1 + "\" style=\"display:none\" >" + dt.Rows[i]["u3"].ToString() + "</span>&nbsp;</td><td><input type=\"checkbox\" name=\"qx_box_" + i1 + "\" id =\"qx_box\" value = \"查看\" " + set_check(dt.Rows[i]["u3"].ToString(), 0) + "  />查看&nbsp;<input type=\"checkbox\" name=\"qx_box_" + i1 + "\" id=\"Checkbox1\"  value=\"增加\" " + set_check(dt.Rows[i]["u3"].ToString(), 1) + " />增加&nbsp;<input type=\"checkbox\" name=\"qx_box_" + i1 + "\" id=\"Checkbox2\"  value=\"修改\"" + set_check(dt.Rows[i]["u3"].ToString(), 2) + " />修改&nbsp;<input type=\"checkbox\" name=\"qx_box_" + i1 + "\" id=\"Checkbox3\"  value=\"删除\" " + set_check(dt.Rows[i]["u3"].ToString(), 3) + " />删除&nbsp;                    </td></tr></table> ";
            i1++;
        }
        return t1;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
          
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }


            if (type == "edit")
            {
             
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["zuming"].ToString();
           
            }
            else if (type == "del")
            {
                if (Request.QueryString["id"].ToString() == "")
                {
                    my_b.tiaozhuan("请选择要删除的信息", Request.UrlReferrer.ToString());
                }
                my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup where id in (" + Request.QueryString["id"].ToString() + ")");
                my_b.tiaozhuan("会员组删除成功", "admin_group.aspx");
            }

            Repeater1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=0 and u4='显示' order by u2,id desc");
            Repeater1.DataBind();
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string zuming = my_b.c_string(this.TextBox1.Text);

        string quanxian = my_b.c_string(this.TextBox3.Text);
        string sql_string = "";
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            sql_string = "update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup set zuming='" + zuming + "',quanxian='" + quanxian + "'";


            sql_string = sql_string + " where id=" + Request.QueryString["id"].ToString();
            my_c.genxin(sql_string);
            Response.Redirect("err.aspx?err=修改会员组成功！&errurl=" + my_b.tihuan("admin_group.aspx", "&", "fzw123") + "");
        }
        else
        {

            if (my_b.get_count("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup", "where zuming='" + zuming + "'") > 0)
            {
                Response.Redirect("err.aspx?err=组名：" + zuming + "已存在，请重新设置！&errurl=" + my_b.tihuan("usergroup_add.aspx", "&", "fzw123") + "");
            }
            sql_string = "insert into  " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup (zuming,quanxian) values('" + zuming + "','" + quanxian + "')";
            my_c.genxin(sql_string);
            Response.Redirect("err.aspx?err=新建会员组帐号成功！&errurl=" + my_b.tihuan("admin_group.aspx", "&", "fzw123") + "");

        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
