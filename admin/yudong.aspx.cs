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

public partial class admin_yudong : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable dt = new DataTable();
    int jj = 0;
    int i = 0;
    public void dr1(string t1, int t2)
    {

        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + " and Model_id=" + Request.QueryString["Model_id"].ToString() + "");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "— ";
                }

                if (DropDownList1.Items.FindByValue(bb + dt1.Rows[j]["u1"].ToString()) == null)
                {
                    DropDownList1.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                    DropDownList1.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                    jj = jj + 1;
                    int tt1 = t2 + 1;
                    dr1(dt1.Rows[j]["id"].ToString(), tt1);
                }
            }
        }

    }
    public string type_str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"].ToString() == "")
            {
                Response.Redirect("err.aspx?err=请选择要处理的信息！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
            }

            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }
            if (type == "copy")
            {
                type_str = "复制";
            }
            else
            {
                type_str = "移动";
            }
            try
            {
                Literal1.Text = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + Request.QueryString["Model_id"].ToString() + " and id=" + Request.QueryString["classid"].ToString() + "").Rows[0]["u1"].ToString();
            }
            catch
            {
                Literal1.Text = "多个分类";
            }


            dr1("0", 0);
        }
    }
    public string get_kj(string type, string id,DataTable dt,int i)
    {
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框" || type == "编辑器")
        {
            return "'" + my_b.c_string(dt.Rows[i][id].ToString()) + "'";
        }
        else if (type == "数字")
        {
            return "" + my_b.c_string(dt.Rows[i][id].ToString()) + "";
        }
        else if (type == "单选按钮组")
        {
           
            if (id == "shenhe")
            {
                //处理会员
                DataTable sl_user = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where yonghuming='" + my_b.k_cookie("admin_id") + "'");
              
                if (sl_user.Rows[0]["huiyuanzu"].ToString().IndexOf("管理员") == -1)
                {
                    return "'未审核'";
                }
                else
                {
                    return "'" + dt.Rows[i][id].ToString() + "'";
                }
                //end
            }
            else
            {
                return "'" + dt.Rows[i][id].ToString() + "'";
            }
        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(my_b.c_string(dt.Rows[i][id].ToString())) + "'";
        }
        else if (type == "分类")
        {
            return "" + DropDownList1.SelectedValue + "";
        }
        else
        {
            return "'" + dt.Rows[i][id].ToString() + "'";
        }
    }
 

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string type = "";
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch { }
        if (type == "copy")
        {
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            string table_name = model_table.Rows[0]["u1"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " ");
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");
            ;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sql = "insert into " + table_name + " ";
                sql = sql + "(";
                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    if (d1 == 0)
                    {
                        sql = sql + Model_dt.Rows[d1]["u1"].ToString();
                    }
                    else
                    {
                        sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString();
                    }
                }

                if (model_table.Rows[0]["u3"].ToString() == "文章模型" || model_table.Rows[0]["u3"].ToString() == "新闻模型")
                {
                    sql = sql + ",Filepath) values (";
                }
                else
                {
                    sql = sql + ") values (";
                }

                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    if (d1 == 0)
                    {
                        sql = sql + get_kj(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), dt, i);
                    }
                    else
                    {
                        sql = sql + "," + get_kj(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), dt, i);
                    }
                }
                if (model_table.Rows[0]["u3"].ToString() == "文章模型" || model_table.Rows[0]["u3"].ToString() == "新闻模型")
                {
                    DateTime dy = DateTime.Now;
                    Random r = new Random();
                    int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                    string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + DropDownList1.SelectedValue + "") + my_b.chuli_lujing();
                    sql = sql + ",'" + filepath + "')";
                }
                else
                {
                    sql = sql + ")";
                }
                try
                {
                    my_c.genxin(sql);
                }
                catch
                {
                    Response.Write(sql);
                    Response.End();
                }


            }
            //跳转
            Response.Redirect("err.aspx?err=复制信息成功！&errurl=" + my_b.tihuan("Class_Table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + DropDownList1.SelectedValue + "", "&", "fzw123") + "");
            //end
        }
        else
        {
            //移动
            string table_name = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString()).Rows[0]["u1"].ToString();
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime dy = DateTime.Now;
                string Filepath = my_c.GetTable("select u7 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + DropDownList1.SelectedValue + "").Rows[0]["u7"].ToString() + my_b.chuli_lujing();
                my_c.genxin("update " + table_name + " set Filepath='" + Filepath + "',classid=" + DropDownList1.SelectedValue + " where id=" + dt.Rows[i]["id"].ToString() + "");
            }

            Response.Redirect("err.aspx?err=移动信息成功！&errurl=" + my_b.tihuan("Class_Table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + DropDownList1.SelectedValue + "", "&", "fzw123") + "");
            //end
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
