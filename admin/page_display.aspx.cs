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
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
public partial class admin_page_display : System.Web.UI.Page
{
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    my_html my_h = new my_html();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string Model_id = "";
            string id = "";
            string type = "page";

            try
            {
                Model_id = Request.QueryString["Model_id"].ToString();
            }
            catch
            { }
            try
            {
                id = Request.QueryString["id"].ToString();
            }
            catch
            { }
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            DataTable dt = my_c.GetTable("select * from " + Model_dt.Rows[0]["u1"].ToString() + " where  id=" + id);

            //处理非表单数据

            if (Model_dt.Rows[0]["u3"].ToString() != "文章模型")
            {
                Response.Redirect("show_auto_table.aspx?Model_id=" + Model_id + "&id=" + id);
            }
            DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + dt.Rows[0]["classid"].ToString());
            if (sort_dt.Rows[0]["u6"].ToString() == "")
            {
                Response.Redirect("show_auto_table.aspx?Model_id=" + Model_id + "&id=" + id);
            }
            else
            {
                Response.Redirect("/page.aspx?id=" + id + "&classid=" + dt.Rows[0]["classid"].ToString());
            }

            //end

            if (type == "page")
            {

                string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + sort_dt.Rows[0]["u6"].ToString(), Encoding.UTF8);
                if (my_b.set_mode() == "静态网站")
                {
                    DataTable dt1 = my_c.GetTable("select * from " + Model_dt.Rows[0]["u1"].ToString() + " where id=" + id + "");
                    Response.Redirect(dt1.Rows[0]["Filepath"].ToString() + dt1.Rows[0]["id"].ToString() + ".html");
                }

                Response.Write(my_h.set_w_content(file_content, Model_dt.Rows[0]["u1"].ToString(), id));
            }
            else if (type == "view")
            {
                string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + sort_dt.Rows[0]["u6"].ToString(), Encoding.UTF8);
                if (my_b.set_mode() == "静态网站")
                {
                    DataTable dt1 = my_c.GetTable("select * from " + Model_dt.Rows[0]["u1"].ToString() + " where id=" + id + "");
                    Response.Redirect(dt1.Rows[0]["Filepath"].ToString() + dt1.Rows[0]["id"].ToString() + ".html");
                }

                Response.Write(my_h.set_w_content(file_content, Model_dt.Rows[0]["u1"].ToString() + "_back", id));
            }

            if (type == "list")
            {

            }


        }
    }
}
