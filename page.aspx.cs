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
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
public partial class page : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //处理介绍人
            if (Request.QueryString.ToString() == "")
            {

                //介绍人等于空
                string yonghuming = "";
                string yonghuming_ = "";
                try
                {
                    yonghuming_ = my_b.k_cookie("user_name").ToString();
                }
                catch { }

                if (yonghuming_ != "")
                {
                    try
                    {
                        yonghuming = Request.QueryString["yonghuming"].ToString();
                    }
                    catch
                    {

                        Response.Redirect(Request.Url.ToString() + "?yonghuming=" + yonghuming_);
                    }
                }


                //空
            }
            if (Request.QueryString.ToString() != "")
            {

                string jieshaoren = "";
                try
                {
                    jieshaoren = my_b.k_cookie("jieshaoren").ToString();
                }
                catch { }
                if (jieshaoren == "")
                {
                    //介绍人等于空
                    string yonghuming = "";
                    string yonghuming_ = "";
                    try
                    {
                        yonghuming_ = my_b.k_cookie("user_name").ToString();
                    }
                    catch { }
                    try
                    {
                        yonghuming = Request.QueryString["yonghuming"].ToString();
                    }
                    catch
                    {

                    }

                    if (yonghuming != "")
                    {
                        my_b.c_cookie(yonghuming, "jieshaoren");
                    }
                    //Response.Write(my_b.k_cookie("jieshaoren"));
                    //Response.End();

                    //空

                }
            }


            //处理介绍人

            my_b.set_openid();
            string subscribe = "";
            try
            {
                subscribe = my_b.k_cookie("subscribe");
            }
            catch { }
            if (subscribe != "")
            {
                if (subscribe == "否")
                {
                    my_b.admin_o_cookie("openid");
                }
            }

            if (my_b.set_mode() == "伪静态")
            {
                string id = "";
                string classid = "";
                id = my_b.set_url_css(Request.QueryString["id"].ToString());
                Regex reg = new Regex("-", RegexOptions.Singleline);
                string[] bb = reg.Split(id);
                id = bb[0].ToString();
                classid = bb[1].ToString();
                my_b.canshu_ziduan(id, "数字");
                my_b.canshu_ziduan(classid, "数字");
                DataTable dt = my_c.GetTable("select u6,Model_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + "");
                string u6 = dt.Rows[0]["u6"].ToString();
                string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + u6), Encoding.UTF8);
                string table_name = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + dt.Rows[0]["Model_id"].ToString() + "").Rows[0]["u1"].ToString();
                Response.Write(my_h.set_w_content(file_content, table_name, id));
            }
            else
            {
                string id = "";
                string classid = "";
                try
                {
                    id = my_b.set_url_css(Request.QueryString["id"].ToString());
                }
                catch
                { }
                try
                {
                    classid = my_b.set_url_css(Request.QueryString["classid"].ToString());
                }
                catch
                { }
                my_b.canshu_ziduan(id, "数字");//参数字段验证
                my_b.canshu_ziduan(classid, "数字");//参数字段验证

                DataTable dt = my_c.GetTable("select u6,Model_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + "");
                string u6 = dt.Rows[0]["u6"].ToString();
                string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + u6), Encoding.UTF8);
                string table_name = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + dt.Rows[0]["Model_id"].ToString() + "").Rows[0]["u1"].ToString();



                Response.Write(my_h.set_w_content(file_content, table_name, id));
            }


        }
    }
}
