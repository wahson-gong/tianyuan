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
public partial class admin_collect : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string web_page_content = "";
    string home_url = "";
    no_html no_m = new no_html();
    string s_pic = "";
    public string set_15(string g1, string g2)
    {
        if (g2.IndexOf("保存远程图片") > -1)
        {
            g1 = my_b.Download_pic(g1);
        }

        return g1;
    }

    public string get_html(string g1, string g2, string g3, string g4)
    {

        string u11 = "";
        string u13 = Server.HtmlDecode(g3);
        home_url = g1.Replace("http://", "");
        home_url = home_url.Substring(0, home_url.IndexOf("/"));
        string page_url = "http://" + g1.Replace("http://", "").Substring(0, g1.Replace("http://", "").IndexOf("/"));

        Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
        string[] aa = reg.Split(g2);
        string t1 = "";
        for (int i = 0; i < aa.Length; i++)
        {
            Regex reg1 = new Regex("{fzw:Field}", RegexOptions.Singleline);
            string[] bb = reg1.Split(aa[i].ToString());
            DataTable dt = new DataTable();
            string qian_ = Server.HtmlDecode(bb[2].ToString());
            string hou_ = Server.HtmlDecode(bb[3].ToString());



            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + bb[0].ToString());
            if (dt.Rows[0]["u6"].ToString() == "文本框")
            {
                web_page_content = my_b.getWebFile(g1 + "|" + bianma);

                string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                t2 = t2.Substring(0, t2.IndexOf(hou_));

                Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                string[] cc = reg3.Split(u13);

                if (cc[0].ToString() != "")
                {
                    t2 = t2.Replace(cc[0].ToString(), cc[1].ToString());
                }
                if (cc[2].ToString() != "")
                {
                    t2 = t2.Replace(cc[2].ToString(), cc[3].ToString());
                }
                if (cc[4].ToString() != "")
                {
                    t2 = t2.Replace(cc[4].ToString(), cc[5].ToString());
                }
                if (g4 != "")
                {
                    DataTable html_dt = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + g4 + ")");
                    for (int x = 0; x < html_dt.Rows.Count; x++)
                    {
                        if (t2 != "")
                        {
                            t2 = no_m.set_html(html_dt.Rows[x]["u1"].ToString(), t2, page_url);
                        }
                    }
                }

                if (t1 == "")
                {
                    t1 = dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                }
                else
                {
                    t1 = t1 + "{fzw:che}" + dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                }



            }
            else if (dt.Rows[0]["u6"].ToString() == "缩略图")
            {
                web_page_content = my_b.getWebFile(g1 + "|" + bianma);

                string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                t2 = t2.Substring(0, t2.IndexOf(hou_));

                Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                string[] cc = reg3.Split(u13);

                if (cc[0].ToString() != "")
                {
                    t2 = t2.Replace(cc[0].ToString(), cc[1].ToString());
                }
                if (cc[2].ToString() != "")
                {
                    t2 = t2.Replace(cc[2].ToString(), cc[3].ToString());
                }
                if (cc[4].ToString() != "")
                {
                    t2 = t2.Replace(cc[4].ToString(), cc[5].ToString());
                }
                if (g4 != "")
                {
                    DataTable html_dt = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + g4 + ")");
                    for (int x = 0; x < html_dt.Rows.Count; x++)
                    {
                        if (t2 != "")
                        {
                            t2 = no_m.set_html(html_dt.Rows[x]["u1"].ToString(), t2, page_url);
                        }
                    }
                }

                if (t2.IndexOf("http://") == -1)
                {
                    if (t2.IndexOf("/") == 0)
                    {
                        t2 = "http://" + home_url + t2;
                    }
                    else
                    {
                        t2 = "http://" + g1.Substring(0, g1.LastIndexOf("/")) + "/" + t2;
                    }

                }

                t2 = my_b.Downloads(t2);
                s_pic = t2;
                if (t1 == "")
                {
                    t1 = dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                }
                else
                {
                    t1 = t1 + "{fzw:che}" + dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                }

            }
            else if (dt.Rows[0]["u6"].ToString() == "编辑器")
            {

                string list = "";
                string list1 = "";
                string list2 = "";
                string list3 = "";
                string list4 = "";
                string t5 = "";
                try
                {
                    list1 = Server.HtmlDecode(bb[4].ToString());
                }
                catch
                { }
                try
                {
                    list2 = Server.HtmlDecode(bb[5].ToString());
                }
                catch
                { }
                try
                {
                    list3 = Server.HtmlDecode(bb[6].ToString());
                }
                catch
                { }
                try
                {
                    list4 = Server.HtmlDecode(bb[7].ToString());
                }
                catch
                { }
                if (list1 != "")
                {

                    list = web_page_content.Substring(web_page_content.IndexOf(list1) + list1.Length);
                    list = list.Substring(0, list.IndexOf(list2));
                    //   Response.Write(list);
                    string list_reg = list3 + ".*?" + list4;
                    Regex reg2 = new Regex(list_reg, RegexOptions.Singleline);
                    MatchCollection matches = reg2.Matches(list);
                    list = "";
                    if (matches.Count > 0)
                    {
                        try
                        {
                            foreach (Match match in matches)
                            {
                                string t3 = "";
                                t3 = match.ToString().Replace(list3, "").Replace(list4, "");

                                if (u11.IndexOf(t3) < 0)
                                {

                                    if (t3.IndexOf("http://") > -1 || t3.IndexOf("/") == 0)
                                    {
                                        t3 = t3.Replace(home_url, "").Replace("http://", "");
                                        if (t3.IndexOf("/") == 0)
                                        {
                                            t3 = "http://" + home_url + t3;
                                        }
                                        else
                                        {
                                            t3 = "http://" + home_url + "/" + t3;
                                        }
                                    }
                                    else
                                    {
                                        if (t3.IndexOf("/") == 0)
                                        {
                                            t3 = "http://" + home_url + t3;
                                        }
                                        else
                                        {
                                            if (t3 != "#")
                                            {
                                                t3 = g1.Substring(0, g1.LastIndexOf("/")) + "/" + t3;
                                            }
                                            else
                                            {
                                                t3 = g1;
                                            }
                                        }
                                    }
                                    //Response.Write(t3);
                                    //Response.End();
                                    string t4 = "";
                                    if (t5 == "")
                                    {
                                        if (t3 != g1)
                                        {
                                            web_page_content = my_b.getWebFile(g1 + "|" + bianma);
                                            t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                            t4 = t4.Substring(0, t4.IndexOf(hou_));
                                            t5 = t5 + t4;
                                        }
                                    }
                                    web_page_content = my_b.getWebFile(t3 + "|" + bianma);
                                    t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                    t4 = t4.Substring(0, t4.IndexOf(hou_));
                                    t5 = t5 + t4;
                                }
                                u11 = u11 + t3;
                            }
                            t5 = my_b.set_http_url(t5, "http://" + home_url);

                            Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                            string[] cc = reg3.Split(u13);
                            if (cc[0].ToString() != "")
                            {
                                t5 = t5.Replace(cc[0].ToString(), cc[1].ToString());
                            }
                            if (cc[2].ToString() != "")
                            {
                                t5 = t5.Replace(cc[2].ToString(), cc[3].ToString());
                            }
                            if (cc[4].ToString() != "")
                            {
                                t5 = t5.Replace(cc[4].ToString(), cc[5].ToString());
                            }
                            if (g4 != "")
                            {
                                DataTable html_dt = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + g4 + ")");
                                for (int x = 0; x < html_dt.Rows.Count; x++)
                                {
                                    if (t5 != "")
                                    {
                                        t5 = no_m.set_html(html_dt.Rows[x]["u1"].ToString(), t5, page_url);
                                    }
                                }
                            }


                            if (t1 == "")
                            {
                                t1 = dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t5;
                            }
                            else
                            {
                                t1 = t1 + "{fzw:che}" + dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t5;
                            }
                        }
                        catch
                        {
                            web_page_content = my_b.getWebFile(g1 + "|" + bianma);
                            string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                            t2 = t2.Substring(0, t2.IndexOf(hou_));
                            t2 = my_b.set_http_url(t2, "http://" + home_url);
                            Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                            string[] cc = reg3.Split(u13);
                            if (cc[0].ToString() != "")
                            {
                                t2 = t2.Replace(cc[0].ToString(), cc[1].ToString());
                            }
                            if (cc[2].ToString() != "")
                            {
                                t2 = t2.Replace(cc[2].ToString(), cc[3].ToString());
                            }
                            if (cc[4].ToString() != "")
                            {
                                t2 = t2.Replace(cc[4].ToString(), cc[5].ToString());
                            }
                            if (g4 != "")
                            {
                                DataTable html_dt = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + g4 + ")");
                                for (int x = 0; x < html_dt.Rows.Count; x++)
                                {
                                    if (t2 != "")
                                    {
                                        t2 = no_m.set_html(html_dt.Rows[x]["u1"].ToString(), t2, page_url);
                                    }
                                }
                            }

                            if (t1 == "")
                            {
                                t1 = dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                            }
                            else
                            {
                                t1 = t1 + "{fzw:che}" + dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                            }
                        }

                    }
                    else
                    {
                        web_page_content = my_b.getWebFile(g1 + "|" + bianma);
                        string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                        t2 = t2.Substring(0, t2.IndexOf(hou_));
                        t2 = my_b.set_http_url(t2, "http://" + home_url);
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);
                        if (cc[0].ToString() != "")
                        {
                            t2 = t2.Replace(cc[0].ToString(), cc[1].ToString());
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = t2.Replace(cc[2].ToString(), cc[3].ToString());
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = t2.Replace(cc[4].ToString(), cc[5].ToString());
                        }
                        if (g4 != "")
                        {
                            DataTable html_dt = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + g4 + ")");
                            for (int x = 0; x < html_dt.Rows.Count; x++)
                            {
                                if (t2 != "")
                                {
                                    t2 = no_m.set_html(html_dt.Rows[x]["u1"].ToString(), t2, page_url);
                                }
                            }
                        }

                        if (t1 == "")
                        {
                            t1 = dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                        }
                        else
                        {
                            t1 = t1 + "{fzw:che}" + dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                        }
                    }

                }
                else
                {
                    web_page_content = my_b.getWebFile(g1 + "|" + bianma);
                    string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                    t2 = t2.Substring(0, t2.IndexOf(hou_));
                    t2 = my_b.set_http_url(t2, "http://" + home_url);
                    Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] cc = reg3.Split(u13);
                    if (cc[0].ToString() != "")
                    {
                        t2 = t2.Replace(cc[0].ToString(), cc[1].ToString());
                    }
                    if (cc[2].ToString() != "")
                    {
                        t2 = t2.Replace(cc[2].ToString(), cc[3].ToString());
                    }
                    if (cc[4].ToString() != "")
                    {
                        t2 = t2.Replace(cc[4].ToString(), cc[5].ToString());
                    }
                    if (g4 != "")
                    {
                        DataTable html_dt = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + g4 + ")");
                        for (int x = 0; x < html_dt.Rows.Count; x++)
                        {
                            if (t2 != "")
                            {
                                t2 = no_m.set_html(html_dt.Rows[x]["u1"].ToString(), t2, page_url);
                            }
                        }
                    }
                    //Response.Write(1);
                    //Response.End();
                    if (t1 == "")
                    {
                        t1 = dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                    }
                    else
                    {
                        t1 = t1 + "{fzw:che}" + dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                    }
                }


            }
            else
            {
                string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                t2 = t2.Substring(0, t2.IndexOf(hou_));
                t2 = my_b.set_http_url(t2, "http://" + home_url);
                Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                string[] cc = reg3.Split(u13);
                if (cc[0].ToString() != "")
                {
                    t2 = t2.Replace(cc[0].ToString(), cc[1].ToString());
                }
                if (cc[2].ToString() != "")
                {
                    t2 = t2.Replace(cc[2].ToString(), cc[3].ToString());
                }
                if (cc[4].ToString() != "")
                {
                    t2 = t2.Replace(cc[4].ToString(), cc[5].ToString());
                }
                if (g4 != "")
                {
                    dt = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + g4 + ")");
                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        if (t2 != "")
                        {
                            t2 = no_m.set_html(dt.Rows[x]["u1"].ToString(), t2, page_url);
                        }
                    }
                }

                if (t1 == "")
                {
                    t1 = dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                }
                else
                {
                    t1 = t1 + "{fzw:che}" + dt.Rows[0]["u1"].ToString() + "{fzw:Field}" + t2;
                }
            }

        }


        return t1;
    }
    public string set_Field(string g1, string g2)
    {
        for (int i = 0; i < Model_dt.Rows.Count; i++)
        {
            string type = Model_dt.Rows[i]["u6"].ToString();
            if (Model_dt.Rows[i]["u1"].ToString() == g1)
            {
                if (type == "文本框" || type == "缩略图" || type == "密码框" || type == "文本域")
                {
                    return "'" + my_b.NoHTML(my_b.c_string(g2)) + "'";
                }
                else if (type == "编辑器")
                {
                    return "'" + my_b.c_string(g2) + "'";
                }
                if (type == "文件框")
                {
                    return "'" + my_b.get_reg(my_b.NoHTML(my_b.c_string(g2)), ".rmvb|.rm") + "'";
                }
                else if (type == "数字")
                {
                    return my_b.c_string(g2);
                }
                else if (type == "多选按钮组")
                {
                    return "'" + my_b.c_string(g2) + "'";
                }
                else if (type == "单选按钮组")
                {
                    return "'" + my_b.c_string(g2) + "'";
                }
                else if (type == "下拉框")
                {
                    return "'" + my_b.c_string(g2) + "'";
                }
                else
                {
                    return "'" + my_b.c_string(g2) + "'";
                }
            }
        }
        return "";
    }
    static DataTable Model_dt = new DataTable();
    string bianma = "";
    string b_img = "";
    string t2 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            int page = 1;
            try
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }
            catch
            { }
            string list = "";
            DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect where id=" + Request.QueryString["id"].ToString());
            Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + dt.Rows[0]["u11"].ToString() + " order by u9,id");
            bianma = dt.Rows[0]["u2"].ToString();
            //批量指定分页URL代码开始
            if (dt.Rows[0]["u4"].ToString() == "批量指定分页URL代码")
            {
                if (page == 1)
                {
                    home_url = dt.Rows[0]["u3"].ToString().Replace("http://", "");
                    home_url = home_url.Substring(0, home_url.IndexOf("/"));
                    web_page_content = my_b.getWebFile(dt.Rows[0]["u3"].ToString() + "|" + bianma);
                    list = web_page_content.Substring(web_page_content.IndexOf(Server.HtmlDecode(dt.Rows[0]["u7"].ToString())) + Server.HtmlDecode(dt.Rows[0]["u7"].ToString()).Length);
                    list = list.Substring(0, list.IndexOf(Server.HtmlDecode(dt.Rows[0]["u8"].ToString())));

                    string list_reg = dt.Rows[0]["u9"].ToString() + ".*?" + dt.Rows[0]["u10"].ToString();
                    Regex reg = new Regex(list_reg, RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(list);
                    list = "";
                    Response.Write("列表页地址：<a href='" + dt.Rows[0]["u3"].ToString() + "' target=_blank>" + dt.Rows[0]["u3"].ToString() + "</a><br>");
                    Response.Flush();
                    foreach (Match match in matches)
                    {

                        //开始
                        string t1 = "";
                        t1 = match.ToString().Replace(dt.Rows[0]["u9"].ToString(), "").Replace(dt.Rows[0]["u10"].ToString(), "");
                        if (list.IndexOf(t1) < 0)
                        {
                            if (t1.IndexOf("http://") == -1)
                            {
                                if (t1.IndexOf("/") == 0)
                                {
                                    t1 = "http://" + home_url + t1;
                                }
                                else
                                {
                                    t1 = "http://" + home_url + "/" + t1;
                                }
                            }
                            list = list + "," + t1;

                            try
                            {
                                t2 = get_html(t1, dt.Rows[0]["u12"].ToString(), dt.Rows[0]["u13"].ToString(), dt.Rows[0]["u14"].ToString());
                                t2 = set_15(t2, dt.Rows[0]["u15"].ToString());

                                string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[0]["u11"].ToString() + "");
                                string sql = "insert into " + table_name + " ";
                                Regex reg1 = new Regex("{fzw:che}", RegexOptions.Singleline);
                                string[] aa = reg1.Split(t2);
                                string sql1 = "";
                                string sql2 = "";
                                string title = "";
                                string file_z = "";

                                for (int i = 0; i < aa.Length; i++)
                                {

                                    Regex reg2 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                                    string[] bb = reg2.Split(aa[i].ToString());
                                    if (title == "")
                                    {
                                        title = bb[1].ToString();
                                    }
                                    if (file_z == "")
                                    {
                                        file_z = bb[0].ToString();
                                    }
                                    if (sql1 == "")
                                    {
                                        sql1 = bb[0].ToString();
                                        sql2 = set_Field(bb[0].ToString(), bb[1].ToString());
                                    }
                                    else
                                    {
                                        sql1 = sql1 + "," + bb[0].ToString();
                                        sql2 = sql2 + "," + set_Field(bb[0].ToString(), bb[1].ToString());

                                    }
                                }

                                b_img = "";
                                if (dt.Rows[0]["u15"].ToString().IndexOf("为第一张图片创建缩略图") > -1)
                                {

                                    string b_tu = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field", "where u6 like '%缩略图%'  and Model_id=" + dt.Rows[0]["u11"].ToString() + "");
                                    if (b_tu != "")
                                    {
                                        sql1 = sql1 + "," + b_tu;
                                        try
                                        {
                                            b_img = my_b.set_images(my_b.get_images(t2, 1), dt.Rows[0]["u19"].ToString());
                                        }
                                        catch
                                        {
                                        }
                                        sql2 = sql2 + ",'" + b_img + "'";

                                    }
                                }
                                if (my_c.GetTable("select id from " + table_name + " where " + file_z + "='" + my_b.c_string(title) + "'").Rows.Count > 0)
                                {
                                    my_b.del_article_pic(t2);
                                    my_b.del_pic(b_img);
                                    my_b.del_pic(s_pic);
                                    Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：" + title + "</a>&nbsp;&nbsp;&nbsp;&nbsp;状态：采集重复<br>");
                                    Response.Flush();
                                }
                                else
                                {
                                    sql = sql + " (" + sql1 + ",classid,Filepath) values(" + sql2 + "," + dt.Rows[0]["u17"].ToString() + ",'" + my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + dt.Rows[0]["u17"].ToString() + "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "/')";
                                    try
                                    {
                                        my_c.genxin(sql);
                                        Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：" + title + "</a>&nbsp;&nbsp;&nbsp;&nbsp;状态：采集成功<br>");
                                        Response.Flush();
                                    }
                                    catch
                                    {
                                        Response.Write(sql);
                                        Response.End();
                                        my_b.del_article_pic(t2);
                                        my_b.del_pic(b_img);
                                        my_b.del_pic(s_pic);
                                        Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：未采集&nbsp;&nbsp;&nbsp;&nbsp;状态：<span style=color:red>SQL语句出错</span><br>");
                                        Response.Flush();
                                    }

                                }

                            }
                            catch
                            {
                                my_b.del_article_pic(t2);
                                my_b.del_pic(b_img);
                                my_b.del_pic(s_pic);
                                Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：未采集&nbsp;&nbsp;&nbsp;&nbsp;状态：<span style=color:red>采集失败</span><br>");
                                Response.Flush();
                            }



                        }

                        //完

                    }
                    Regex page_u6 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    page = int.Parse(page_u6.Split(dt.Rows[0]["u6"].ToString())[0].ToString());
                    my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect set u18=" + page + " where id=" + Request.QueryString["id"].ToString());
                    Response.Write("<script>window.location='err.aspx?err=此页采集成功，马上采集下一页！&errurl=" + my_b.tihuan("collect.aspx?page=" + page + "&id=" + Request.QueryString["id"].ToString() + "", "&", "fzw123") + "'</script>");


                }
                else
                {
                    Regex reg111 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] aa11 = reg111.Split(dt.Rows[0]["u6"].ToString());
                    int page_s = int.Parse(aa11[0].ToString());
                    int page_o = int.Parse(aa11[1].ToString());
                    int page_c = 0;
                    string page_t1 = "";
                    if (page_s > page_o)
                    {
                        page_c = page_s;
                        if (aa11[1].ToString().Length > page.ToString().Length)
                        {
                            for (int page_i = 0; page_i < aa11[1].ToString().Length - page.ToString().Length; page_i++)
                            {
                                page_t1 = page_t1 + "0";
                            }
                            page_t1 = page_t1 + page.ToString();
                        }
                        else
                        {
                            page_t1 = page.ToString();
                        }

                    }
                    else
                    {
                        if (aa11[0].ToString().Length > page.ToString().Length)
                        {
                            for (int page_i = 0; page_i < aa11[0].ToString().Length - page.ToString().Length; page_i++)
                            {
                                page_t1 = page_t1 + "0";
                            }
                            page_t1 = page_t1 + page.ToString();
                        }
                        else
                        {
                            page_t1 = page.ToString();
                        }
                        page_c = page_o;
                    }

                    if (page_c >= page)
                    {

                        home_url = dt.Rows[0]["u3"].ToString().Replace("http://", "");
                        home_url = home_url.Substring(0, home_url.IndexOf("/"));
                        web_page_content = my_b.getWebFile(dt.Rows[0]["u5"].ToString().Replace("{$ID}", page_t1) + "|" + bianma);
                        list = web_page_content.Substring(web_page_content.IndexOf(Server.HtmlDecode(dt.Rows[0]["u7"].ToString())) + Server.HtmlDecode(dt.Rows[0]["u7"].ToString()).Length);
                        list = list.Substring(0, list.IndexOf(Server.HtmlDecode(dt.Rows[0]["u8"].ToString())));

                        string list_reg = dt.Rows[0]["u9"].ToString() + ".*?" + dt.Rows[0]["u10"].ToString();
                        Regex reg = new Regex(list_reg, RegexOptions.Singleline);
                        MatchCollection matches = reg.Matches(list);
                        list = "";
                        Response.Write("列表页地址：<a href='" + dt.Rows[0]["u5"].ToString().Replace("{$ID}", page_t1) + "' target=_blank>" + dt.Rows[0]["u5"].ToString().Replace("{$ID}", page_t1) + "</a><br>");
                        Response.Flush();
                        foreach (Match match in matches)
                        {

                            //开始
                            string t1 = "";
                            t1 = match.ToString().Replace(dt.Rows[0]["u9"].ToString(), "").Replace(dt.Rows[0]["u10"].ToString(), "");
                            if (list.IndexOf(t1) < 0)
                            {
                                if (t1.IndexOf("http://") == -1)
                                {
                                    if (t1.IndexOf("/") == 0)
                                    {
                                        t1 = "http://" + home_url + t1;
                                    }
                                    else
                                    {
                                        t1 = "http://" + home_url + "/" + t1;
                                    }
                                }
                                list = list + "," + t1;

                                try
                                {
                                    t2 = get_html(t1, dt.Rows[0]["u12"].ToString(), dt.Rows[0]["u13"].ToString(), dt.Rows[0]["u14"].ToString());
                                    t2 = set_15(t2, dt.Rows[0]["u15"].ToString());
                                    string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[0]["u11"].ToString() + "");
                                    string sql = "insert into " + table_name + " ";
                                    Regex reg1 = new Regex("{fzw:che}", RegexOptions.Singleline);
                                    string[] aa = reg1.Split(t2);
                                    string sql1 = "";
                                    string sql2 = "";
                                    string title = "";
                                    string file_z = "";
                                    for (int i = 0; i < aa.Length; i++)
                                    {

                                        Regex reg2 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                                        string[] bb = reg2.Split(aa[i].ToString());
                                        if (title == "")
                                        {
                                            title = bb[1].ToString();
                                        }
                                        if (file_z == "")
                                        {
                                            file_z = bb[0].ToString();
                                        }
                                        if (sql1 == "")
                                        {
                                            sql1 = bb[0].ToString();
                                            sql2 = set_Field(bb[0].ToString(), bb[1].ToString());
                                        }
                                        else
                                        {
                                            sql1 = sql1 + "," + bb[0].ToString();
                                            sql2 = sql2 + "," + set_Field(bb[0].ToString(), bb[1].ToString());

                                        }
                                    }


                                    b_img = "";
                                    if (dt.Rows[0]["u15"].ToString().IndexOf("为第一张图片创建缩略图") > -1)
                                    {

                                        string b_tu = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field", "where u6 like '%缩略图%' and Model_id=" + dt.Rows[0]["u11"].ToString() + "");
                                        if (b_tu != "")
                                        {
                                            sql1 = sql1 + "," + b_tu;
                                            try
                                            {
                                                b_img = my_b.set_images(my_b.get_images(t2, 1), dt.Rows[0]["u19"].ToString());
                                            }
                                            catch
                                            {
                                            }
                                            sql2 = sql2 + ",'" + b_img + "'";

                                        }
                                    }
                                    if (my_c.GetTable("select id from " + table_name + " where " + file_z + "='" + my_b.c_string(title) + "'").Rows.Count > 0)
                                    {
                                        my_b.del_article_pic(t2);
                                        my_b.del_pic(b_img);
                                        my_b.del_pic(s_pic);
                                        Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：" + title + "&nbsp;&nbsp;&nbsp;&nbsp;状态：采集重复<br>");
                                        Response.Flush();
                                    }
                                    else
                                    {
                                        sql = sql + " (" + sql1 + ",classid,Filepath) values(" + sql2 + "," + dt.Rows[0]["u17"].ToString() + ",'" + my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + dt.Rows[0]["u17"].ToString() + "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "/')";

                                        try
                                        {
                                            my_c.genxin(sql);
                                            Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：" + title + "</a>&nbsp;&nbsp;&nbsp;&nbsp;状态：采集成功<br>");
                                            Response.Flush();
                                        }
                                        catch
                                        {
                                            Response.Write(sql);
                                            Response.End();
                                            my_b.del_article_pic(t2);
                                            my_b.del_pic(b_img);
                                            my_b.del_pic(s_pic);
                                            Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：未采集&nbsp;&nbsp;&nbsp;&nbsp;状态：<span style=color:red>SQL语句出错</span><br>");
                                            Response.Flush();
                                        }
                                    }

                                }
                                catch
                                {
                                    my_b.del_article_pic(t2);
                                    my_b.del_pic(b_img);
                                    my_b.del_pic(s_pic);
                                    Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：未采集&nbsp;&nbsp;&nbsp;&nbsp;状态：<span style=color:red>采集失败</span><br>");
                                    Response.Flush();
                                }

                                //这里是>1的处理区

                            }

                            //完

                        }
                        page = page + 1;
                        my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect set u18=" + page + " where id=" + Request.QueryString["id"].ToString());
                        Response.Write("<script>window.location='err.aspx?err=此页采集成功，马上采集下一页！&errurl=" + my_b.tihuan("collect.aspx?page=" + page + "&id=" + Request.QueryString["id"].ToString() + "", "&", "fzw123") + "'</script>");
                    }
                    else
                    {
                        Response.Write("所有完成");
                        Response.End();
                    }




                }
            }
            else
            {
                //批量指定分页URL代码结束
                home_url = dt.Rows[0]["u3"].ToString().Replace("http://", "");
                home_url = home_url.Substring(0, home_url.IndexOf("/"));
                web_page_content = my_b.getWebFile(dt.Rows[0]["u3"].ToString() + "|" + bianma);
                list = web_page_content.Substring(web_page_content.IndexOf(Server.HtmlDecode(dt.Rows[0]["u7"].ToString())) + Server.HtmlDecode(dt.Rows[0]["u7"].ToString()).Length);
                list = list.Substring(0, list.IndexOf(Server.HtmlDecode(dt.Rows[0]["u8"].ToString())));

                string list_reg = dt.Rows[0]["u9"].ToString() + ".*?" + dt.Rows[0]["u10"].ToString();
                Regex reg = new Regex(list_reg, RegexOptions.Singleline);
                MatchCollection matches = reg.Matches(list);
                list = "";
                Response.Write("列表页地址：<a href='" + dt.Rows[0]["u3"].ToString() + "' target=_blank>" + dt.Rows[0]["u3"].ToString() + "</a><br>");
                Response.Flush();
                foreach (Match match in matches)
                {

                    //开始
                    string t1 = "";
                    t1 = match.ToString().Replace(dt.Rows[0]["u9"].ToString(), "").Replace(dt.Rows[0]["u10"].ToString(), "");
                    if (list.IndexOf(t1) < 0)
                    {
                        if (t1.IndexOf("http://") == -1)
                        {
                            if (t1.IndexOf("/") == 0)
                            {
                                t1 = "http://" + home_url + t1;
                            }
                            else
                            {
                                t1 = "http://" + home_url + "/" + t1;
                            }
                        }
                        list = list + "," + t1;

                        try
                        {
                            t2 = get_html(t1, dt.Rows[0]["u12"].ToString(), dt.Rows[0]["u13"].ToString(), dt.Rows[0]["u14"].ToString());

                            t2 = set_15(t2, dt.Rows[0]["u15"].ToString());
                            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[0]["u11"].ToString() + "");
                            string sql = "insert into " + table_name + " ";
                            Regex reg1 = new Regex("{fzw:che}", RegexOptions.Singleline);
                            string[] aa = reg1.Split(t2);
                            string sql1 = "";
                            string sql2 = "";
                            string title = "";
                            string file_z = "";
                            for (int i = 0; i < aa.Length; i++)
                            {

                                Regex reg2 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                                string[] bb = reg2.Split(aa[i].ToString());
                                if (title == "")
                                {
                                    title = bb[1].ToString();
                                }
                                if (file_z == "")
                                {
                                    file_z = bb[0].ToString();
                                }
                                if (sql1 == "")
                                {
                                    sql1 = bb[0].ToString();
                                    sql2 = set_Field(bb[0].ToString(), bb[1].ToString());
                                }
                                else
                                {
                                    sql1 = sql1 + "," + bb[0].ToString();
                                    sql2 = sql2 + "," + set_Field(bb[0].ToString(), bb[1].ToString());

                                }
                            }

                            b_img = "";
                            if (dt.Rows[0]["u15"].ToString().IndexOf("为第一张图片创建缩略图") > -1)
                            {

                                string b_tu = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field", "where u6 like '%缩略图%' and Model_id=" + dt.Rows[0]["u11"].ToString() + "");
                                if (b_tu != "")
                                {
                                    sql1 = sql1 + "," + b_tu;
                                    try
                                    {
                                        b_img = my_b.set_images(my_b.get_images(t2, 1), dt.Rows[0]["u19"].ToString());
                                    }
                                    catch
                                    { }
                                    sql2 = sql2 + ",'" + b_img + "'";

                                }
                            }
                            if (my_c.GetTable("select id from " + table_name + " where " + file_z + "='" + my_b.c_string(title) + "'").Rows.Count > 0)
                            {
                                my_b.del_article_pic(t2);
                                my_b.del_pic(b_img);
                                my_b.del_pic(s_pic);

                                Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：" + title + "&nbsp;&nbsp;&nbsp;&nbsp;状态：采集重复<br>");
                                Response.Flush();
                            }
                            else
                            {
                                sql = sql + " (" + sql1 + ",classid,Filepath) values(" + sql2 + "," + dt.Rows[0]["u17"].ToString() + ",'" + my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + dt.Rows[0]["u17"].ToString() + "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "/')";

                                try
                                {
                                    my_c.genxin(sql);
                                    Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：" + title + "</a>&nbsp;&nbsp;&nbsp;&nbsp;状态：采集成功<br>");
                                    Response.Flush();
                                }
                                catch
                                {
                                    my_b.del_article_pic(t2);
                                    my_b.del_pic(b_img);
                                    my_b.del_pic(s_pic);
                                    Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：未采集&nbsp;&nbsp;&nbsp;&nbsp;状态：<span style=color:red>SQL语句出错</span><br>");
                                    Response.Flush();
                                }
                            }

                        }
                        catch
                        {
                            my_b.del_article_pic(t2);
                            my_b.del_pic(b_img);
                            my_b.del_pic(s_pic);
                            Response.Write("页面地址：<a href='" + t1 + "' target=_blank>" + t1 + "</a>&nbsp;&nbsp;&nbsp;&nbsp;标题：未采集&nbsp;&nbsp;&nbsp;&nbsp;状态：<span style=color:red>采集失败</span><br>");
                            Response.Flush();
                        }



                    }


                    //完

                }

                Response.Write("所有完成");
                Response.End();
            }









        }
    }







}
