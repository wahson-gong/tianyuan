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
public partial class admin_auto_table : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    public DataTable sl_user = new DataTable();
    public string add__url()
    {
        string laiyuanbianhao = "";
        try
        {
            laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
        }
        catch
        { }
        if (laiyuanbianhao != "")
        {
            return "zz_table_add.aspx?laiyuanbianhao=" + laiyuanbianhao + "&Model_id=" + Request.QueryString["Model_id"].ToString();
        }
        else
        {
            return "zz_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString();
        }
    }
    biaodan db = new biaodan();
    public string set_m_url(string g1, DataTable dt1, int j, string u12, string g2)
    {
        if (u12 != "")
        {
            Regex reg = new Regex(@"{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(g1);
            foreach (Match match in matches)
            {
                string id_name = match.ToString().Replace("{", "").Replace("}", "").Trim();

                g1 = g1.Replace(match.ToString(), dt1.Rows[j][id_name].ToString().Replace("$我是分隔符$", "|"));
            }
            return g1;
        }
        else
        {
            return g2;
        }

    }
    public string shenhe_sta = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_b.set_admin_url();
            string laiyuanbianhao = "";
            try
            {
                laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
            }
            catch
            { }
            string t1 = "<table class=\"table table-border table - hover\" id=\"content\"><thead><tr><th width=\"8%\" class=\"hidden-xs\">编号</th>";

            string search_sql = "id>0";
            string page = "1";
            string u1 = "";
            try
            {
                page = Request.QueryString["page"].ToString();
            }
            catch
            { }
            try
            {
                u1 = Request.QueryString["u1"].ToString();
                this.TextBox3.Text = u1;
            }
            catch
            { }
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
            string shenhe = "";
            try
            {
                shenhe = Request.QueryString["shenhe"].ToString();
                if (shenhe != "")
                {
                    for (int i = 0; i < DropDownList1.Items.Count; i++)
                    {
                        if (DropDownList1.Items[i].Value == shenhe)
                        {
                            DropDownList1.Items[i].Selected = true;
                        }
                    }
                }

            }
            catch
            { }
            //mddel
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
        
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            if (Model_dt.Rows[0]["u8"].ToString() != "")
            {
                Literal3.Text = Model_dt.Rows[0]["u8"].ToString() + "&nbsp;&nbsp;";
            }
            //end

            string select_return = "id";
            dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and U5='是' order by u9 ,id");
            #region 处理表头
            t1 = t1 + db.list_biaotou(dt2);
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                select_return = select_return + "," + dt2.Rows[i]["u1"].ToString();
            }
            #endregion
            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and U10='是'");
            string search_sql1 = "";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (search_sql1 == "")
                {
                    search_sql1 = dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
                }
                else
                {
                    search_sql1 = search_sql1 + " or " + dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
                }
            }
            if (search_sql == "")
            {
                search_sql =  "  (" + search_sql1 + ")";
            }
            else
            {
                search_sql = search_sql + " and (" + search_sql1 + ")";
            }

            string edit_url = "";

            if (laiyuanbianhao != "")
            {
                edit_url = "&laiyuanbianhao="+laiyuanbianhao+"" ;
                if (search_sql == "")
                {
                    search_sql = "laiyuanbianhao=" + laiyuanbianhao + "";
                }
                else
                {
                    search_sql = search_sql + " and laiyuanbianhao=" + laiyuanbianhao + "";
                }
            }
            sl_user = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + my_b.k_cookie("admin_id") + "'");
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
            if (shenhe != "")
            {
                search_sql = search_sql + " and (shenhe = '" + shenhe + "') ";
            }
            if (sl_user.Rows[0]["u3"].ToString().IndexOf("管理员") == -1)
            {
               // search_sql = search_sql + " and (yonghuming = '" + my_b.k_cookie("admin_id") + "') ";
            }
       

            DataTable dt1 = my_b.art_list(biaoming, search_sql, select_return, "order by id desc", 20, int.Parse(page), "?Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + u1 + "&page=$page$", Literal1);
            //Response.Write(dt1.Rows.Count);
            //Response.End();
            Literal2.Text = db.set_list(t1, dt2, dt1, i, edit_url, "zz_table_add.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "");

        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("zz_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + Request.QueryString["laiyuanbianhao"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&shenhe=" + this.DropDownList1.SelectedValue.Replace("全部", "") + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
        }
        catch
        {
            Response.Redirect("zz_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&shenhe=" + this.DropDownList1.SelectedValue.Replace("全部", "") + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
        }
    }
}
