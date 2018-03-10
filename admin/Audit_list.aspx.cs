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

public partial class admin_Audit_list : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string key = "";
    public string get_article_title(string g1)
    {
        try
        {
            string Model_id = get_fenge(g1, 2);
            string id = get_fenge(g1, 1);
            string table_name = get_fenge(g1, 0);

            string title_name = my_c.GetTable("select top 1 * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " order by u9,id").Rows[0]["u1"].ToString();

            string t1 = my_c.GetTable("select " + title_name + " from " + table_name + " where id=" + id).Rows[0][title_name].ToString();
            t1 = "<a href='page_display.aspx?Model_id=" + Model_id + "&id=" + id + "' target='_blank'>" + t1 + "</a>";
            return t1;
        }
        catch
        {
            return "";
        }
    }
    public string get_fenge(string g1, int g2)
    {
        Regex reg = new Regex("{fenge}", RegexOptions.Singleline);
        string[] bb = reg.Split(g1);
        return bb[g2];
    }
    public string get_classid(string g1)
    {
        try
        {
            string Model_id = get_fenge(g1, 2);
            string id = get_fenge(g1, 1);
            string table_name = get_fenge(g1, 0);
            return my_c.GetTable("select classid from " + table_name + " where id=" + id).Rows[0]["classid"].ToString();
        }
        catch
        {
            return "";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "Audit")
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system where id in (" + Request.QueryString["id"].ToString() + ")");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string id = get_fenge(dt.Rows[i]["u2"].ToString(), 1);
                    string table_name = get_fenge(dt.Rows[i]["u2"].ToString(), 0);
                    try
                    {
                        my_c.genxin("update " + table_name + " set Audit='通过' where id=" + id);
                    }
                    catch
                    { }
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system where id in (" + dt.Rows[i]["id"].ToString() + ")");
                }
                Response.Redirect("err.aspx?err=文章审核成功！&errurl=" + my_b.tihuan("Audit_list.aspx", "&", "fzw123") + "");
            }
            else if (type == "del")
            {
                my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system where id in (" + Request.QueryString["id"].ToString() + ")");
                Response.Redirect("err.aspx?err=文章审核删除成功！&errurl=" + my_b.tihuan("Audit_list.aspx", "&", "fzw123") + "");
            }
            else
            {
                string search_sql = "";
                string page = "1";
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
                }
                catch
                { }
                if (search_sql == "")
                {
                    search_sql = " u4='文章审核'";
                }
                else
                {
                    search_sql = search_sql + " and u4='文章审核'";
                }
                my_b.page_list("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system", search_sql, "*", "order by id desc", 40, int.Parse(page), Repeater1, "Audit_list.aspx?search_sql=" + Server.HtmlDecode(search_sql) + "&page=$page$", Literal1);
            }

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (my_b.c_string(this.TextBox1.Text) == "")
        {
            Response.Redirect("Audit_list.aspx?search_sql=" + Server.HtmlEncode("id>0") + "");
        }
        else
        {
            Response.Redirect("Audit_list.aspx?search_sql=" + Server.HtmlEncode("u1 like '%" + my_b.c_string(this.TextBox1.Text) + "%'") + "");
        }

    }
}