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
using System.Text;
public partial class admin_auto_table : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();

    public void ExportToExcel(System.Web.UI.Page page, GridView excel, string fileName)
    {
        try
        {
            foreach (GridViewRow row in excel.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    excel.HeaderRow.Cells[i].BackColor = System.Drawing.Color.Yellow;

                }
            }
            excel.Font.Size = 10;
            excel.AlternatingRowStyle.BackColor = System.Drawing.Color.LightCyan;
            excel.RowStyle.Height = 25;

            page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            page.Response.Charset = "utf-8";
            page.Response.ContentType = "application/vnd.ms-excel";
            page.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=gb2312>");
            excel.Page.EnableViewState = false;
            excel.Visible = true;
            excel.HeaderStyle.Reset();
            excel.AlternatingRowStyle.Reset();

            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            excel.RenderControl(oHtmlTextWriter);
            page.Response.Write(oStringWriter.ToString());
            page.Response.End();

            excel.DataSource = null;
            excel.Visible = false;
        }
        catch (Exception e)
        {

        }
    }
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    int i = 0;
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            string shijian1 = "";
            string shijian2 = "";

            try
            {
                shijian1 = Request.QueryString["shijian1"].ToString();
                this.TextBox1.Text = shijian1;
            }
            catch { }
            try
            {
                shijian2 = Request.QueryString["shijian2"].ToString(); ;
                this.TextBox2.Text = shijian2;
            }
            catch { }


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
            //mddel
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            if (Model_dt.Rows[0]["u8"].ToString() != "")
            {
                Literal3.Text = Model_dt.Rows[0]["u8"].ToString() + "&nbsp;&nbsp;";
            }
            //end
            string select_return = "id as 编号";
            dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u13<>'是' order by u9 ,id");
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {

                }
                else
                {

                }

                select_return = select_return + "," + dt2.Rows[i]["u1"].ToString() + " as " + dt2.Rows[i]["u2"].ToString() + "";
            }
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
            if (search_key != "")
            {
                search_sql = search_sql + " and (" + search_key + ") ";
            }
            #region 自定义搜索条件
            try
            {
                search_sql = Request.QueryString["search_sql"].ToString();
            }
            catch { }
            #endregion

            //Response.Write("select " + select_return + " from " + biaoming + " where " + search_sql + "");
            //Response.End();

            DataTable dt1 = my_c.GetTable("select " + select_return + " from " + biaoming + " where " + search_sql + "");
            GridView1.DataSource = dt1;
            GridView1.DataBind();
            // ExportToExcel(Page, GridView1, "/order.xls");
        }
    }


    protected void Button1_Click1(object sender, EventArgs e)
    {
        Response.Redirect("user_table_down.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "&u1=" + this.TextBox3.Text + "");
    }



    protected void Button2_Click1(object sender, EventArgs e)
    {
        if (this.TextBox1.Text != "" && this.TextBox2.Text != "")
        {
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            Response.Redirect("/inc/daochuexcle.aspx?t=" + biaoming.Replace("sl_", "") + "&dtime=" + this.TextBox1.Text + "between" + this.TextBox2.Text + "&yuangongxingming=" + this.TextBox3.Text + "");
        }
        else
        {
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            Response.Redirect("/inc/daochuexcle.aspx?t=" + biaoming.Replace("sl_", "") + "&yuangongxingming=" + this.TextBox3.Text + "");
        }
       
    }
}
