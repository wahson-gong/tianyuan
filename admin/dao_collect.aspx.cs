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
using System.Text;
public partial class admin_dao_collect : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect where id=" + Request.QueryString["id"].ToString());
            string sql = "insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect (u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,u12,u13,u14,u15,u16,u17,u18,u19) values('" + dt.Rows[0]["u1"].ToString() + "','" + dt.Rows[0]["u2"].ToString() + "','" + dt.Rows[0]["u3"].ToString() + "','" + dt.Rows[0]["u4"].ToString() + "','" + dt.Rows[0]["u5"].ToString() + "','" + dt.Rows[0]["u6"].ToString() + "','" + dt.Rows[0]["u7"].ToString() + "','" + dt.Rows[0]["u8"].ToString() + "','" + dt.Rows[0]["u9"].ToString() + "','" + dt.Rows[0]["u10"].ToString() + "','" + dt.Rows[0]["u11"].ToString() + "','" + dt.Rows[0]["u12"].ToString() + "','" + dt.Rows[0]["u13"].ToString() + "','" + dt.Rows[0]["u14"].ToString() + "','" + dt.Rows[0]["u15"].ToString() + "','" + dt.Rows[0]["u16"].ToString() + "','" + dt.Rows[0]["u17"].ToString() + "'," + dt.Rows[0]["u18"].ToString() + ",'" + dt.Rows[0]["u19"].ToString() + "')";
            Response.Clear();
            Response.Write(sql);
            Response.Buffer = false;
            Response.Charset = "utf-8"; 
            Response.ContentType = "application/octet-stream";
           // Response.AppendHeader("content-disposition", "attachment;filename=蓝色CMS_" + dt.Rows[0]["u1"].ToString() + "采集规则.txt;");
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("百分CMS_" + dt.Rows[0]["u1"].ToString() + "采集规则.txt", Encoding.UTF8));
            Response.Flush();

            Response.End();//End和Close的顺序是什么，测试时，两个位置排列交换后对执行没有任何影响
            Response.Close();


        }
    }
}
