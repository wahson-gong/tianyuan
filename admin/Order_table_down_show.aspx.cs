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
public partial class Order_table_down_show : System.Web.UI.Page
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
            page.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
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

            }
            catch
            { }
            try
            {
                shijian1 = Request.QueryString["shijian1"].ToString();

            }
            catch
            { }
            try
            {
                shijian2 = Request.QueryString["shijian2"].ToString();

            }
            catch
            { }

            //mddel
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            string search_sql = biaoming + ".id>0";
            if (Model_dt.Rows[0]["u8"].ToString() != "")
            {
                //  Literal3.Text = Model_dt.Rows[0]["u8"].ToString() + "&nbsp;&nbsp;";
            }
            //end
            string select_return = biaoming + ".id as 编号";
            dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9 ,id");
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {

                }
                else
                {

                }
                //Response.Write(dt2.Rows[i]["u6"].ToString());
                //Response.End();
                select_return = select_return + "," + biaoming + "." + dt2.Rows[i]["u1"].ToString() + " as " + dt2.Rows[i]["u2"].ToString() + "";

            }


            dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from sl_model where u1='" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "cart') order by u9 ,id");

            string biaoming_ = ConfigurationSettings.AppSettings["Prefix"].ToString() + "cart";
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {

                }
                else
                {

                }
                //Response.Write(dt2.Rows[i]["u6"].ToString());
                //Response.End();
                select_return = select_return + "," + biaoming_ + "." + dt2.Rows[i]["u1"].ToString() + " as " + dt2.Rows[i]["u2"].ToString() + "";

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
                        search_key = biaoming + "." + dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
                    }
                    else
                    {
                        search_key = search_key + " or " + biaoming + "." + dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
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
            string sql = "select " + select_return + " from " + biaoming + "," + biaoming_ + " where " + biaoming + ".dingdanhao=" + biaoming_ + ".dingdanhao  and " + search_sql + " order by " + biaoming + ".id desc";
            //Response.Write(sql);
            //Response.End();

            DataTable dt1 = my_c.GetTable(sql);
            GridView1.DataSource = dt1;
            GridView1.DataBind();
            // GridView1.SetRowCellValue(0, GridView1.Columns[0], "123");
            // ExportToExcel(Page, GridView1, "/order.xls");

            Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            biaoming = Model_dt.Rows[0]["u1"].ToString();

            ExportToExcel(Page, GridView1, biaoming + "_" + DateTime.Now.ToString().Replace("-", "").Replace(" ", "") + ".xls");
        }
    }





}
