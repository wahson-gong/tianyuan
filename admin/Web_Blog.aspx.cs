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

public partial class admin_Web_Blog : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string key = "";
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DropDownList1.DataSource = my_c.GetTable("select u4 from sl_system where id in (Select max(id) as id from sl_system group by u4)  order by id desc");
            DropDownList1.DataTextField = "u4";
            DropDownList1.DataValueField = "u4";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, "全部");
            DropDownList1.Items[0].Value = "";


            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }

            if (type == "del")
            {
                my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system where id in (" + Request.QueryString["id"].ToString() + ")");
                Response.Redirect("err.aspx?err=网站日志删除成功！&errurl=" + my_b.tihuan("Web_Blog.aspx", "&", "fzw123") + "");
            }
            else
            {
                string search_sql = "";
                string page = "1";
                string shijian1 = "";
                try
                {
                    shijian1 = Request.QueryString["shijian1"].ToString();
                    this.TextBox1.Text = shijian1;
                }
                catch
                { }
                string shijian2 = "";
                try
                {
                    shijian2 = Request.QueryString["shijian2"].ToString();
                    this.TextBox2.Text = shijian2;
                }
                catch
                { }
                try
                {
                    page = Request.QueryString["page"].ToString();
                }
                catch
                { }
                try
                {
                    search_sql = Request.QueryString["search_sql"].ToString();
                }
                catch
                { }
                try
                {
                    key = Request.QueryString["key"].ToString();
                    this.TextBox3.Text = key;
                }
                catch
                { }
                string u4 = "";
                try
                {
                    u4 = Request.QueryString["u4"].ToString();
                }
                catch
                { }
                if (u4 != "")
                {
                    search_sql = "u4='" + u4 + "'";
                    for (int i = 0; i < DropDownList1.Items.Count; i++)
                    {
                        if (DropDownList1.Items[i].Value == u4)
                        {
                            DropDownList1.Items[i].Selected = true;
                        }
                    }
                }
                if (shijian1 != "" && shijian2 != "")
                {
                    if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
                    {
                        if (search_sql == "")
                        {
                            search_sql = "dtime between '" + shijian1.ToString() + shang_time + "' and '" + shijian2.ToString() + xia_time + "'";
                        }
                        else
                        {
                            search_sql = search_sql + " and dtime between '" + shijian1.ToString() + shang_time + "' and '" + shijian2.ToString() + xia_time + "'";
                        }
                    }
                    else
                    {
                        if (search_sql == "")
                        {
                            search_sql = "dtime between #" + shijian1.ToString() + shang_time + "# and #" + shijian2.ToString() + xia_time + "#";
                        }
                        else
                        {
                            search_sql = search_sql + " and dtime between #" + shijian1.ToString() + shang_time + "# and #" + shijian2.ToString() + xia_time + "#";
                        }
                    }

                }
                if (key != "")
                {
                    if (search_sql == "")
                    {
                        search_sql = "(u1 like '%" + key + "%' or u2 like '%" + key + "%' or u3 like '%" + key + "%')";
                    }
                    else
                    {
                        search_sql = search_sql + " and (u1 like '%" + key + "%' or u2 like '%" + key + "%' or u3 like '%" + key + "%')";
                    }
                    this.TextBox1.Text = key;
                }
                if (search_sql == "")
                {
                    search_sql = "id>0";
                }

                my_b.page_list("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system", search_sql, "*", "order by id desc", 40, int.Parse(page), Repeater1, "Web_Blog.aspx?key=" + key + "&u4=" + u4 + "&shijian1="+shijian1+ "&shijian2="+shijian2+"&page=$page$", Literal1);
            }
            
        }
    }


    protected void Button1_Click1(object sender, EventArgs e)
    {
        Response.Redirect("Web_Blog.aspx?key=" + this.TextBox3.Text + "&u4=" + DropDownList1.SelectedValue + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
    }
}
