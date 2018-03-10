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
public partial class admin_x_article : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string editname = "";
    public string listid = "";
    //获取相关选择中的表名
    public string get_tablename(string sql)
    {
        string sql_str = "";
        Regex reg = new Regex("from.*?order|from.*", RegexOptions.Singleline);
        Match sql_ma = reg.Match(sql);
        sql_str = sql_ma.ToString().Replace("order", "");
        sql_str = sql_str.ToString().Replace("from", "");
        sql_str = sql_str.Trim();
        return sql_str;
    }
    //end
    //获取相关选择中的字段名
    public string get_sql(int dijige, string sql)
    {

        string sql_str = "";
        Regex reg = new Regex("select.*?from", RegexOptions.Singleline);
        Match sql_ma = reg.Match(sql);
        sql_str = sql_ma.ToString().Replace("select", "");
        sql_str = sql_str.ToString().Replace("from", "");
        sql_str = sql_str.Trim();
        string[] aa = sql_str.Split(',');
        return aa[dijige];
    }
    public string set_che(string g1)
    {

        string[] cc = Request.QueryString["listid"].ToString().Split(',');
        for (int x = 0; x < cc.Length; x++)
        {

            if (g1 == cc[x].ToString())
            {
                return "checked";
            }
        }

        return "";
    }
    public string get_sql(int dijige)
    {
        string sql_str = "";
        Regex reg = new Regex("select.*?from", RegexOptions.Singleline);
        Match sql_ma = reg.Match(HttpUtility.UrlDecode(Request.QueryString["sql"].ToString()));
        sql_str = sql_ma.ToString().Replace("select", "");
        sql_str = sql_str.ToString().Replace("from", "");
        sql_str = sql_str.Trim();
        string[] aa = sql_str.Split(',');
        return aa[dijige];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string key = "";
            try
            {
                key = Request.QueryString["key"].ToString();
            }
            catch { }
            string txt_value = HttpUtility.UrlDecode(Request.QueryString["sql"].ToString());
            string table_name = get_tablename(txt_value);
            DataTable dt = new DataTable();
            if (key != "")
            {
                this.TextBox1.Text = key;
                string ziduan = get_sql(1);
                if (table_name.IndexOf("where") > -1)
                {

                    dt = my_c.GetTable("select * from " + table_name + " and " + ziduan + " like '%" + key + "%'");
                }
                else
                {
                    dt = my_c.GetTable("select * from " + table_name + " where " + ziduan + " like '%" + key + "%'");
                }
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.DataSource = my_c.GetTable(HttpUtility.UrlDecode(Request.QueryString["sql"].ToString()));
                Repeater1.DataBind();
            }




            editname = Request.QueryString["editname"].ToString();
            listid = Request.QueryString["listid"].ToString();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("x_article.aspx?editname=" + Request.QueryString["editname"].ToString() + "&listid=" + Request.QueryString["listid"].ToString() + "&sql=" + Request.QueryString["sql"].ToString() + "&key=" + my_b.c_string(this.TextBox1.Text) + "");

    }
}
