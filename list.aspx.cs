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
public partial class list : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    int jj = 0;
    string list_id = "";
    public void dr1(string t1, int t2, string t3)
    {
        DataTable dt1 = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id in (" + t1 + ") and Model_id=" + t3 + "");
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (list_id == "")
                {
                    list_id = dt1.Rows[j]["id"].ToString();
                }
                else
                {
                    list_id = list_id + "," + dt1.Rows[j]["id"].ToString();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1, t3);
            }
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_b.set_openid();
            if (my_b.set_mode() == "伪静态")
            {
                int page = 0;
                string id = my_b.c_string(Request.QueryString["id"].ToString());

                Regex reg = new Regex("-", RegexOptions.Singleline);
                string[] bb = reg.Split(id);
                if (bb.Length > 1)
                {
                    string Model_id = bb[1].ToString();
                    id = bb[0].ToString();
                    my_b.canshu_ziduan(id, "数字");//参数字段验证
                    if (id.IndexOf(",") == -1)
                    {
                        if (my_b.get_count("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where Sort_id=" + id + "") > 0)
                        {
                            dr1(id, 0, Model_id);
                            id = list_id;
                        }
                    }


                    try
                    {
                        page = int.Parse(bb[2].ToString());
                    }
                    catch
                    { }
                    my_h.set_wei_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id), id, page, Model_id);
                }
            }
            else
            {
                string lan = "";
                try
                {
                    lan = Server.HtmlDecode(my_b.t_string(Server.HtmlEncode(Request.QueryString["lan"].ToString())));
                }
                catch
                { }
                int page = 0;
                try
                {
                    page = int.Parse(Server.HtmlDecode(my_b.c_string(Server.HtmlEncode(Request.QueryString["page"].ToString()))));
                }
                catch
                { }
                string id = Server.HtmlDecode(my_b.c_string(Server.HtmlEncode(Request.QueryString["id"].ToString())));
                string Model_id = Server.HtmlDecode(my_b.c_string(Server.HtmlEncode(Request.QueryString["Model_id"].ToString())));
                my_b.canshu_ziduan(id, "数字");//参数字段验证
                my_b.canshu_ziduan(Model_id, "数字");//参数字段验证
                if (my_b.get_count("" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where Sort_id=" + id + "") > 0)
                {
                    dr1(id, 0, Model_id);
                    id = list_id;
                }
                if (lan == "en")
                {
                    my_h.set_e_w_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id), id, page);
                }
                else
                {

                    my_h.set_w_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id), id, page);
                }
            }
        }



    }
}
