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
public partial class caiwu_table : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    biaodan db = new biaodan();
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
            return "caiwu_table_add.aspx?laiyuanbianhao=" + laiyuanbianhao + "&Model_id=" + Request.QueryString["Model_id"].ToString();
        }
        else
        {
            return "caiwu_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString();
        }
    }
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
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DropDownList1.DataSource = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=270 order by u3 asc,id desc");
            DropDownList1.DataTextField = "u1";
            DropDownList1.DataValueField = "u1";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, "全部");
            DropDownList1.Items[0].Value = "";
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
       
            string leixing = "";
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
                leixing = Request.QueryString["leixing"].ToString();
                for (int i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == leixing)
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
            }
            catch
            {

            }
            string zhuangtai = "";
            try
            {
                zhuangtai = Request.QueryString["zhuangtai"].ToString();
                for (int i = 0; i < DropDownList2.Items.Count; i++)
                {
                    if (DropDownList2.Items[i].Value == zhuangtai)
                    {
                        DropDownList2.Items[i].Selected = true;
                    }
                }
            }
            catch
            {

            }
          
            if (zhuangtai != "" && zhuangtai != "全部")
            {
                search_sql = "zhuangtai like '%" + zhuangtai + "%'";
            }
            if (leixing != "" && leixing != "全部")
            {
                if (search_sql == "")
                {
                    search_sql = "leixing like '%" + leixing + "%'";
                }
                else
                {
                    search_sql = search_sql +" and leixing like '%" + leixing + "%'";
                }
            }
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
            if (search_sql == "")
            {
                search_sql = "  (" + search_sql1 + ")";
            }
            else
            {
                search_sql = search_sql + " and (" + search_sql1 + ")";
            }

            string edit_url = "";

            if (laiyuanbianhao != "")
            {
                edit_url = "&laiyuanbianhao=" + laiyuanbianhao + "";
                if (search_sql == "")
                {
                    search_sql = "laiyuanbianhao=" + laiyuanbianhao + "";
                }
                else
                {
                    search_sql = search_sql + " and laiyuanbianhao=" + laiyuanbianhao + "";
                }
            }
            search_sql = search_sql;

            DataTable dt1 = my_b.art_list(biaoming, search_sql, select_return, "order by id desc", 20, int.Parse(page), "?Model_id=" + Request.QueryString["Model_id"].ToString() + "&leixing=" + leixing + "&zhuangtai=" + zhuangtai + "&shijian1=" + shijian1 + "&shijian2=" + shijian2 + "&u1=" + u1 + "&page=$page$", Literal1);
            Literal2.Text = db.set_list(t1, dt2, dt1, i, edit_url, "caiwu_table_add.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "");

            #region 计算金额
            float count_jine = float.Parse(my_c.GetTable("select sum(jine) as count_jine from sl_caiwu where " + search_sql + "").Rows[0]["count_jine"].ToString());
            float touzi_jine = float.Parse(my_c.GetTable("select sum(jine) as count_jine from sl_caiwu where " + search_sql + " and leixing='投资'").Rows[0]["count_jine"].ToString());
            float count_jine_ = count_jine - touzi_jine;
            count_jine =  touzi_jine - count_jine_;
           
            #endregion
            Literal5.Text = "收到投资金额："+ my_b.set_jiage(touzi_jine.ToString()) + "元，支出金额："+ my_b.set_jiage(count_jine_.ToString()) + "元，剩余金额：" + my_b.set_jiage(count_jine.ToString()) + "元";
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("caiwu_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + Request.QueryString["laiyuanbianhao"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&leixing=" + DropDownList1.SelectedValue + "&zhuangtai=" + DropDownList2.SelectedValue + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
        }
        catch
        {
            Response.Redirect("caiwu_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&leixing=" + DropDownList1.SelectedValue + "&zhuangtai=" + DropDownList2.SelectedValue + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
        }
    }
}
