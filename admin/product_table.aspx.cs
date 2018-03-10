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

public partial class admin_articles : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    string parameter_list = "";
    int jj = 0;
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    public DataTable sl_user = new DataTable();
    biaodan db = new biaodan();
    public void Parameter_class(string classid, int t2)
    {
        DataTable dt1 = my_c.GetTable("select id,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where sort_id=" + classid + " and Model_id=" + Request.QueryString["Model_id"].ToString() + "  order by paixu asc,id asc");
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                string bb = "";
                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "—";
                }
                if (parameter_list == "")
                {
                    parameter_list = "{k:" + dt1.Rows[j]["id"].ToString() + ",v:'" + bb + dt1.Rows[j]["u1"].ToString() + "'}";
                }
                else
                {
                    parameter_list = parameter_list + "|" + "{k:" + dt1.Rows[j]["id"].ToString() + ",v:'" + bb + dt1.Rows[j]["u1"].ToString() + "'}";
                }

                jj = jj + 1;
                int tt1 = t2 + 1;
                Parameter_class(dt1.Rows[j]["id"].ToString(), tt1);
            }
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_b.set_admin_url();
         
            Parameter_class("0", 0);
            this.fenlei_list.Text = parameter_list;

            string t1 = "<table class=\"table table-border table - hover\" id=\"content\"><thead><tr><th width=\"8%\" class=\"hidden-xs\">编号</th>";
            string classid = "";
            try
            {
                classid = Request.QueryString["classid"].ToString();
                this.fenlei.Text = classid;
            }
            catch
            {
                Response.Redirect("Class_Table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString());
            }
            //设置当前位置
            try
            {
                Literal4.Text = my_b.set_weizhi(classid, "Class_Table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=$classid$", "sl_sort");
            }
            catch
            {
                Literal4.Text = "内容搜索";
            }
            //end
            string search_sql = "id>0";
            string page = "1";
            string u1 = "";
            try
            {
                page = Request.QueryString["page"].ToString();
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


            try
            {
                u1 = Request.QueryString["u1"].ToString();
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
            string select_return = "id,classid";
            dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and U5='是' order by u9 ,id");
            #region 文章、新闻模型标题控制
            DataRow[] rows = dt2.Select("u6='标题'");
            if (rows.Length == 0)
            {
                HttpContext.Current.Response.Redirect("err.aspx?err=文章、新闻模型必须存在一个及最多字段类型为《标题》的字段！&errurl=" + my_b.tihuan("Fields.aspx?Model_id="+ Request.QueryString["Model_id"].ToString(), "&", "fzw123") + "");
            }
            #endregion
            #region 处理表头
            t1 = t1 + db.list_biaotou(dt2);
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                select_return = select_return + "," + dt2.Rows[i]["u1"].ToString();
            }
            #endregion
            //Response.Write(select_return);
            //Response.End();
            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and U10='是'  order by u9,id");
            string search_key = "";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (u1 != "")
                {
                    if (search_key == "")
                    {
                        search_key = dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
                    }
                    else
                    {
                        search_key = search_key + " or " + dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
                    }
                    this.TextBox3.Text = u1;
                }
            }
            if (search_key != "")
            {
                search_sql = search_sql + " and (" + search_key + ") ";
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
            if (shenhe != "")
            {
                search_sql = search_sql + " and (shenhe = '" + shenhe + "') ";
            }
            //处理会员
            sl_user = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + my_b.k_cookie("admin_id") + "'");
            if (sl_user.Rows[0]["u3"].ToString().IndexOf("管理员") == -1)
            {
                search_sql = search_sql + " and (yonghuming = '" + my_b.k_cookie("admin_id") + "') ";
            }
            //end
            if (Request.QueryString["classid"].ToString() != "")
            {
                search_sql = search_sql + " and classid in (" + Request.QueryString["classid"].ToString() + ")";
            }
       
            DataTable dt1 = my_b.art_list(biaoming, search_sql, select_return, "order by id desc", 40, int.Parse(page), "?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + classid + "&u1=" + u1 + "&page=$page$", Literal1);
           
            Literal2.Text = db.set_list(t1, dt2, dt1, i, "", "product_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + Request.QueryString["classid"].ToString() + "");

        }
    }



    protected void Button1_Click1(object sender, EventArgs e)
    {
       
        string classid = this.fenlei.Text;
        Response.Redirect("product_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + classid + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&shenhe=" + this.DropDownList1.SelectedValue.Replace("全部", "") + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
    }
}
