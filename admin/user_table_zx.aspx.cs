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
public partial class user_table_zx : System.Web.UI.Page
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
            return "user_table_add.aspx?laiyuanbianhao=" + laiyuanbianhao + "&Model_id=" + Request.QueryString["Model_id"].ToString();
        }
        else
        {
            return "user_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString();
        }
    }
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    public string idlist = "";
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_b.set_admin_url();
            DropDownList1.DataSource = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=252 order by u3 asc,id desc");
            DropDownList1.DataTextField = "u1";
            DropDownList1.DataValueField = "u1";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, "全部");
            DropDownList1.Items[0].Value = "";
            string laiyuanbianhao = "";
            try
            {
                laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
            }
            catch
            { }
            string t1 = "<table class=\"table table-border table-hover\" id=\"content\"><thead><tr><th style='width:8%'>编号</th>";

            string search_sql = "id>0";
            string page = "1";
            string u1 = "";
            string leixing = "";
            string jihuo = "";
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

            try
            {
                jihuo = Request.QueryString["jihuo"].ToString();
                for (int i = 0; i < DropDownList2.Items.Count; i++)
                {
                    if (DropDownList2.Items[i].Value == jihuo)
                    {
                        DropDownList2.Items[i].Selected = true;
                    }
                }
            }
            catch
            {

            }

            if (leixing != "" && leixing != "全部")
            {
                search_sql = "leixing like '%" + leixing + "%'";
            }

            if (jihuo != "" && jihuo != "全部")
            {
                search_sql = "jihuo = '" + jihuo + "'";
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
            //Response.Write(search_sql);
            //Response.End();
 
            DataTable dt1 = my_b.art_list(biaoming, search_sql, select_return, "order by id desc", 20, int.Parse(page), "?Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + u1 + "&leixing=" + leixing + "&jihuo=" + jihuo + "&shijian1=" + shijian1 + "&shijian2=" + shijian2 + "&page=$page$", Literal1);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (idlist == "")
                {
                    idlist = dt1.Rows[i]["id"].ToString();
                }
                else
                {
                    idlist = idlist+","+dt1.Rows[i]["id"].ToString();
                }

            }

        }
    }



    protected void Button1_Click1(object sender, EventArgs e)
    {

        try
        {
            Response.Redirect("user_table_zx.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + Request.QueryString["laiyuanbianhao"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&leixing=" + DropDownList1.SelectedValue + "&jihuo=" + DropDownList2.SelectedValue + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
        }
        catch
        {
            Response.Redirect("user_table_zx.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&jihuo=" + DropDownList2.SelectedValue + "&leixing=" + DropDownList1.SelectedValue + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
        }
    }
}
