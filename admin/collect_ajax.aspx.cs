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
public partial class admin_collect_ajax : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string web_page_content = "";
    string home_url = "";
    no_html no_m = new no_html();

    int jj1 = 0;
    public string conllect_dr = "";
    public void dr1(string t1, int t2,string t3)
    {
        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + " and Model_id=" + t3+ "");
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                string bb = "";
                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "—";
                }
                if (conllect_dr == "")
                {
                    conllect_dr = bb+ dt1.Rows[j]["u1"].ToString() + "{fzw:Field}" + dt1.Rows[j]["id"].ToString();
                }
                else
                {
                    conllect_dr = conllect_dr + "{fzw:che}" + bb + dt1.Rows[j]["u1"].ToString() + "{fzw:Field}" + dt1.Rows[j]["id"].ToString();
                }
                jj1 = jj1 + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1,t3);
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string type = Request.QueryString["type"].ToString();
            if (type == "3")
            {
                string list = "";
                //try
                //{
                my_b.set_xml("bianma", Request.QueryString["bianma"].ToString());
                    home_url = Request.QueryString["web_url"].ToString().Replace("http://", "");
                    home_url = home_url.Substring(0, home_url.IndexOf("/"));
                    web_page_content = my_b.getWebFile(Request.QueryString["web_url"].ToString());
                    list = web_page_content.Substring(web_page_content.IndexOf(Server.HtmlDecode(Request.QueryString["u7"].ToString())) + Server.HtmlDecode(Request.QueryString["u7"].ToString()).Length);
                    list = list.Substring(0, list.IndexOf(Server.HtmlDecode(Request.QueryString["u8"].ToString())));
                    string list_reg = Request.QueryString["u9"].ToString() + ".*?" + Request.QueryString["u10"].ToString();
                    Regex reg = new Regex(list_reg, RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(list);
                    list = "";
                    foreach (Match match in matches)
                    {

                        string t1 = "";
                        t1 = match.ToString().Replace(Request.QueryString["u9"].ToString(), "").Replace(Request.QueryString["u10"].ToString(), "");
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

                            if (list == "")
                            {
                                list = t1;
                            }
                            else
                            {
                                list = list + "," + t1;
                            }
                        }
                        
                    }
                //}
                //catch
                //{ }
                Response.Write(type);
                Response.Write("{fzw:collect_ajax}");
                if (list == "")
                {
                    Response.Write("err");
                }
                else
                {
                    Response.Write("ok");
                }
                Response.Write("{fzw:collect_ajax}");
                Response.Write(list);

            }
            else if (type == "4")
            {
                DataTable dt = new DataTable();
                int i = 0;
                dt = my_c.GetTable("select u2,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["u11"].ToString() + " order by u9 , id");
                string t1 = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                    {
                        if (i % 6 == 0)
                        {
                            t1 = t1 + "<br>";
                        }
                    }

                    t1 = t1 + "<input type=\"checkbox\" name=\"che_Fields\" value=\"" + dt.Rows[i]["u2"].ToString() + "\" /><span>" + dt.Rows[i]["u2"].ToString() + "</span><span style='display:none' id='div_Fields" + i.ToString() + "'>" + dt.Rows[i]["id"].ToString() + "</span>&nbsp;&nbsp;";
                }


                dr1("0", 0, Request.QueryString["u11"].ToString());
                string t2 = conllect_dr;
                Response.Write(type);
                Response.Write("{fzw:collect_ajax}");
                Response.Write(t1);
                Response.Write("{fzw:collect_ajax}");
                Response.Write(t2);
            }
            else if (type == "5")
            {
                Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
                string[] aa = reg.Split(Request.QueryString["u12"].ToString());
                string t1 = "";
                //Response.Write(HttpUtility.UrlDecode(Request.QueryString.ToString()));
                for (int i = 0; i < aa.Length; i++)
                {
                    DataTable dt = new DataTable();
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + aa[i].ToString().Split(',')[0].ToString());
                    if (dt.Rows[0]["u6"].ToString() == "编辑器")
                    {
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_type\">" + dt.Rows[0]["u6"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" cols=\"45\" rows=\"5\"></textarea></td></tr>            <tr>                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "右边代码 ：</td>                <td colspan=\"2\">                    <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" cols=\"45\" rows=\"5\"></textarea>                </td>            </tr>  <tr>                <td class=\"tRight\">内容分页类型：</td>                <td colspan=\"2\"><input type=\"radio\" name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" value=\"1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" onclick=\"feiye_fenye(" + aa[i].ToString().Split(',')[1].ToString() + ")\"  checked=\"checked\"/>不采集内容分页&nbsp;&nbsp;&nbsp;&nbsp; <input type=\"radio\" name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" value=\"1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" onclick=\"feiye_fenye(" + aa[i].ToString().Split(',')[1].ToString() + ")\"/>从内容中获取分页URL</td>            </tr> <tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_1\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "内容分页左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" cols=\"45\" rows=\"5\"></textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_2\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "内容分页右边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_2\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" cols=\"45\" rows=\"5\"></textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_3\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "目标链接左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_3\" cols=\"45\" rows=\"5\"></textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_4\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "目标链接右边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_4\" cols=\"45\" rows=\"5\"></textarea></td></tr>";
                    }
                    else
                    {
                        t1 = t1 + "<span style='display:none' id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_type\">" + dt.Rows[0]["u6"].ToString() + "</span> <tr>                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" cols=\"45\" rows=\"5\"></textarea></td></tr>            <tr>                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "右边代码 ：</td>                <td colspan=\"2\">                    <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" cols=\"45\" rows=\"5\"></textarea>                </td>            </tr>";
                    }
                }
                Response.Write(type);
                Response.Write("{fzw:collect_ajax}");
                Response.Write("<table class=\"cTable_2 table\">            <tr class=\"cTitle toolbarBg\">                <td width=\"25%\">                    <div>                        内容页设置</div>                </td>                <td colspan=\"2\">                </td>            </tr>" + t1 + "</table>");
            }
            else if (type == "6")
            {
                string u11 = "";
                home_url = Request.QueryString["u11"].ToString().Replace("http://", "");
                home_url = home_url.Substring(0, home_url.IndexOf("/"));
                web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                //Response.Write(HttpUtility.UrlDecode(Request.QueryString.ToString()));
                Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
                string[] aa = reg.Split(Server.HtmlDecode(Request.QueryString["u12"].ToString()));
          
                string t1 = "";
                for (int i = 0; i < aa.Length; i++)
                {
    
                    Regex reg1 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] bb = reg1.Split(aa[i].ToString());
                    DataTable dt = new DataTable();
                    string qian_ = Server.HtmlDecode(bb[2].ToString());
                    string hou_ = Server.HtmlDecode(bb[3].ToString());
                   
                    string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                    try
                    {
                        t2 = t2.Substring(0, t2.IndexOf(hou_));
                    }
                    catch
                    {
                        
                    }
                  
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + bb[0].ToString());
                    if (dt.Rows[0]["u6"].ToString() == "文本框")
                    {
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";
                    }
                    else if (dt.Rows[0]["u6"].ToString() == "文件框")
                    {
                        t2 = my_b.get_reg(t2, ".rmvb|.rm");
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";
                    }
                    else if (dt.Rows[0]["u6"].ToString() == "缩略图")
                    {
                        t2 = my_b.get_reg(t2, ".jpg|.gif");
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";
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

                            string list_reg = list3 + ".*?" + list4;
                            Regex reg2 = new Regex(list_reg, RegexOptions.Singleline);
                            MatchCollection matches = reg2.Matches(list);
                            list = "";
                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                {
                                    string t3 = "";
                                    t3 = match.ToString().Replace(list3, "").Replace(list4, "");
                                    if (u11.IndexOf(t3) < 0)
                                    {

                                        if (t3.IndexOf("http://") == -1)
                                        {
                                            if (t3.IndexOf("/") == 0)
                                            {
                                                t3 = "http://" + home_url + t3;
                                            }
                                            else
                                            {
                                                home_url = Request.QueryString["u11"].ToString();
                                                home_url = home_url.Substring(0, home_url.LastIndexOf("/"));
                                                t3 = "http://" + home_url + "/" + t3;
                                            }
                                        }
                                        try
                                        {
                                            web_page_content = my_b.getWebFile(t3);
                                        }
                                        catch
                                        { }
                                        string t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                        t4 = t4.Substring(0, t4.IndexOf(hou_));
                                        t5 = t5 + t4;
                                    }
                                    u11 = u11 + t3;
                                }
                                if (u11.IndexOf(Request.QueryString["u11"].ToString()) ==-1)
                                {
                                    web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                                    string t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                    t4 = t4.Substring(0, t4.IndexOf(hou_));
                                    t5 = t4+t5;
                                }
                                t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t5 + "</textarea></td></tr>";
                            }
                            else
                            {
                                web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                                t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                t2 = t2.Substring(0, t2.IndexOf(hou_));
                                t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t2 + "</textarea></td></tr>";
                            }

                        }
                        else
                        {
                            web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                            t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                            t2 = t2.Substring(0, t2.IndexOf(hou_));
                            t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t2 + "</textarea></td></tr>";
                        }

                      
                    }
                    else
                    {
                        t1 = t1 + " <tr>                <td class=\"tRight\"> <span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:300px; height:100px\">" + t2 + "</textarea></td></tr>";
                    }
                   
                }
                Response.Write(type);
                Response.Write("{fzw:collect_ajax}");
                Response.Write("<table class=\"cTable_2 table\">            <tr class=\"cTitle toolbarBg\">                <td width=\"25%\">                    <div>                        内容页采集预览</div>                </td>                <td colspan=\"2\">                </td>            </tr><tr>                <td class=\"tRight\">                    测试列表页URL ：</td>                <td colspan=\"2\">  " + Request.QueryString["u11"].ToString() + "</td>            </tr>" + t1 + "</table>");
            }
            else if (type == "8")
            {
                string u11 = "";
                string u13 = Server.HtmlDecode(Request.QueryString["u13"].ToString());
                home_url = Request.QueryString["u11"].ToString().Replace("http://", "");
                home_url = home_url.Substring(0, home_url.IndexOf("/"));
                web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());

                Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
                string[] aa = reg.Split(Request.QueryString["u12"].ToString());
                string t1 = "";
                for (int i = 0; i < aa.Length; i++)
                {
                    Regex reg1 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] bb = reg1.Split(aa[i].ToString());
                    DataTable dt = new DataTable();
                    string qian_ = Server.HtmlDecode(bb[2].ToString());
                    string hou_ = Server.HtmlDecode(bb[3].ToString());
                    string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                    t2 = t2.Substring(0, t2.IndexOf(hou_));

                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + bb[0].ToString());
                    if (dt.Rows[0]["u6"].ToString() == "文本框")
                    {
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);
                        if (cc[0].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                        }

                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";

                    }
                    else if (dt.Rows[0]["u6"].ToString() == "文件框")
                    {
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);
                        if (cc[0].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                        }
                        //读取文件地址
                        t2 = my_b.get_reg(t2,".rmvb|.rm");
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";
                    }
                    else if (dt.Rows[0]["u6"].ToString() == "缩略图")
                    {
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);
                        if (cc[0].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                        }
                        //读取文件地址
                        t2 = my_b.get_reg(t2, ".jpg|.gif");
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";
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
                                foreach (Match match in matches)
                                {
                                    string t3 = "";
                                    t3 = match.ToString().Replace(list3, "").Replace(list4, "");
                                    if (u11.IndexOf(t3) < 0)
                                    {

                                        if (t3.IndexOf("http://") == -1)
                                        {
                                            if (t3.IndexOf("/") == 0)
                                            {
                                                t3 = "http://" + home_url + t3;
                                            }
                                            else
                                            {
                                                home_url = Request.QueryString["u11"].ToString();
                                                home_url = home_url.Substring(0, home_url.LastIndexOf("/"));
                                                t3 = "http://" + home_url + "/" + t3;
                                            }
                                        }

                                        try
                                        {
                                            web_page_content = my_b.getWebFile(t3);
                                        }
                                        catch
                                        { }
                                        string t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                        t4 = t4.Substring(0, t4.IndexOf(hou_));
                                        t5 = t5 + t4;
                                    }
                                    u11 = u11 + t3;
                                }
                                if (u11.IndexOf(Request.QueryString["u11"].ToString()) == -1)
                                {
                                    web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                                    string t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                    t4 = t4.Substring(0, t4.IndexOf(hou_));
                                    t5 = t4 + t5;
                                }
                                Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                                string[] cc = reg3.Split(u13);
                                if (cc[0].ToString() != "")
                                {
                                    t5 = Regex.Replace(t5, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                                }
                                if (cc[2].ToString() != "")
                                {
                                    t5 = Regex.Replace(t5, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                                }
                                if (cc[4].ToString() != "")
                                {
                                    t5 = Regex.Replace(t5, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                                }
                                t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t5 + "</textarea></td></tr>";
                            }
                            else
                            {
                                web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                                t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                t2 = t2.Substring(0, t2.IndexOf(hou_));
                                Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                                string[] cc = reg3.Split(u13);
                                if (cc[0].ToString() != "")
                                {
                                    t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                                }
                                if (cc[2].ToString() != "")
                                {
                                    t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                                }
                                if (cc[4].ToString() != "")
                                {
                                    t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                                }
                                t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t2 + "</textarea></td></tr>";
                            }


                        }
                        else
                        {
                            web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                            t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                            t2 = t2.Substring(0, t2.IndexOf(hou_));
                            Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                            string[] cc = reg3.Split(u13);
                            if (cc[0].ToString() != "")
                            {
                                t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                            }
                            if (cc[2].ToString() != "")
                            {
                                t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                            }
                            if (cc[4].ToString() != "")
                            {
                                t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                            }
                            t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t2 + "</textarea></td></tr>";
                        }


                    }
                    else
                    {
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
                        t1 = t1 + " <tr>                <td class=\"tRight\"> <span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:300px; height:100px\">" + t2 + "</textarea></td></tr>";
                    }

                }
                Response.Write(type);
                Response.Write("{fzw:collect_ajax}");
                Response.Write("<table class=\"cTable_2 table\">            <tr class=\"cTitle toolbarBg\">                <td width=\"25%\">                    <div>                        内容页采集预览</div>                </td>                <td colspan=\"2\">                </td>            </tr><tr>                <td class=\"tRight\">                    测试列表页URL ：</td>                <td colspan=\"2\">  " + Request.QueryString["u11"].ToString() + "</td>            </tr>" + t1 + "</table>");
            }
            else if (type == "10")
            {





                string u11 = "";
                string u13 = Server.HtmlDecode(Request.QueryString["u13"].ToString());
                home_url = Request.QueryString["u11"].ToString().Replace("http://", "");
                home_url = home_url.Substring(0, home_url.IndexOf("/"));
                web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                string page_url = "http://" + Request.QueryString["u11"].ToString().Replace("http://", "").Substring(0, Request.QueryString["u11"].ToString().Replace("http://", "").IndexOf("/"));
                Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
                string[] aa = reg.Split(Request.QueryString["u12"].ToString());
                string t1 = "";
                for (int i = 0; i < aa.Length; i++)
                {
                    Regex reg1 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] bb = reg1.Split(aa[i].ToString());
                    DataTable dt = new DataTable();
                    string qian_ = Server.HtmlDecode(bb[2].ToString());
                    string hou_ = Server.HtmlDecode(bb[3].ToString());
                    string t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                    t2 = t2.Substring(0, t2.IndexOf(hou_));

                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + bb[0].ToString());
                    if (dt.Rows[0]["u6"].ToString() == "文本框")
                    {
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);

                        if (cc[0].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                        }
                        if (Request.QueryString["u14"].ToString() != "")
                        {
                            DataTable dt1 = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["u14"].ToString() + ")");
                            for (int x = 0; x < dt1.Rows.Count; x++)
                            {
                                if (t2 != "")
                                {
                                    t2 = no_m.set_html(dt1.Rows[x]["u1"].ToString(), t2, page_url);
                                }
                            }
                        }
                        
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";




                    }
                    else if (dt.Rows[0]["u6"].ToString() == "文本框")
                    {
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);

                        if (cc[0].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                        }
                        if (Request.QueryString["u14"].ToString() != "")
                        {
                            DataTable dt1 = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["u14"].ToString() + ")");
                            for (int x = 0; x < dt1.Rows.Count; x++)
                            {
                                if (t2 != "")
                                {
                                    t2 = no_m.set_html(dt1.Rows[x]["u1"].ToString(), t2, page_url);
                                }
                            }
                        }
                        t2 = my_b.get_reg(t2,".rmvb|.rm");
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";




                    }
                    else if (dt.Rows[0]["u6"].ToString() == "缩略图")
                    {
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);

                        if (cc[0].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                        }
                        if (Request.QueryString["u14"].ToString() != "")
                        {
                            DataTable dt1 = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["u14"].ToString() + ")");
                            for (int x = 0; x < dt1.Rows.Count; x++)
                            {
                                if (t2 != "")
                                {
                                    t2 = no_m.set_html(dt1.Rows[x]["u1"].ToString(), t2, page_url);
                                }
                            }
                        }
                        t2 = my_b.get_reg(t2,".jpg|.gif");
                        t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <input type=\"text\" name=\"div_Fields" + bb[1].ToString() + "_content\" id=\"div_Fields" + bb[1].ToString() + "_content\"  style=\"width:300px\" value=\"" + t2 + "\"/></td></tr>";




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
                                foreach (Match match in matches)
                                {
                                    string t3 = "";
                                    t3 = match.ToString().Replace(list3, "").Replace(list4, "");
                                    if (u11.IndexOf(t3) < 0)
                                    {

                                        if (t3.IndexOf("http://") == -1)
                                        {
                                            if (t3.IndexOf("/") == 0)
                                            {
                                                t3 = "http://" + home_url + t3;
                                            }
                                            else
                                            {
                                                home_url = Request.QueryString["u11"].ToString();
                                                home_url = home_url.Substring(0, home_url.LastIndexOf("/"));
                                                t3 = "http://" + home_url + "/" + t3;
                                            }
                                        }
                                        home_url = Request.QueryString["u11"].ToString();
                                        home_url = home_url.Substring(0, home_url.LastIndexOf("/"));
                                        try
                                        {
                                            web_page_content = my_b.getWebFile(t3);
                                        }
                                        catch
                                        { }
                                        string t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                        t4 = t4.Substring(0, t4.IndexOf(hou_));
                                        t5 = t5 + t4;
                                    }
                                    u11 = u11 + t3;
                                }
                                if (u11.IndexOf(Request.QueryString["u11"].ToString()) == -1)
                                {
                                    web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                                    string t4 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                    t4 = t4.Substring(0, t4.IndexOf(hou_));
                                    t5 = t4 + t5;
                                }
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
                                if (Request.QueryString["u14"].ToString() != "")
                                {
                                    DataTable dt1 = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["u14"].ToString() + ")");
                                    for (int x = 0; x < dt1.Rows.Count; x++)
                                    {
                                        if (t5 != "")
                                        {
                                            t5 = no_m.set_html(dt1.Rows[x]["u1"].ToString(), t5, page_url);
                                        }
                                    }
                                }
                                t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t5 + "</textarea></td></tr>";
                            }
                            else
                            {
                                web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                                t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                                t2 = t2.Substring(0, t2.IndexOf(hou_));
                                Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                                string[] cc = reg3.Split(u13);
                                if (cc[0].ToString() != "")
                                {
                                    t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                                }
                                if (cc[2].ToString() != "")
                                {
                                    t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                                }
                                if (cc[4].ToString() != "")
                                {
                                    t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                                }
                                if (Request.QueryString["u14"].ToString() != "")
                                {
                                    DataTable dt1 = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["u14"].ToString() + ")");
                                    for (int x = 0; x < dt1.Rows.Count; x++)
                                    {
                                        if (t2 != "")
                                        {
                                            t2 = no_m.set_html(dt1.Rows[x]["u1"].ToString(), t2, page_url);
                                        }
                                    }
                                }
                                t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t2 + "</textarea></td></tr>";
                            }


                        }
                        else
                        {
                            web_page_content = my_b.getWebFile(Request.QueryString["u11"].ToString());
                            t2 = web_page_content.Substring(web_page_content.IndexOf(qian_) + qian_.Length);
                            t2 = t2.Substring(0, t2.IndexOf(hou_));
                            Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                            string[] cc = reg3.Split(u13);
                            if (cc[0].ToString() != "")
                            {
                                t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                            }
                            if (cc[2].ToString() != "")
                            {
                                t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                            }
                            if (cc[4].ToString() != "")
                            {
                                t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                            }
                            if (Request.QueryString["u14"].ToString() != "")
                            {
                                DataTable dt1 = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["u14"].ToString() + ")");
                                for (int x = 0; x < dt1.Rows.Count; x++)
                                {
                                    if (t2 != "")
                                    {
                                        t2 = no_m.set_html(dt1.Rows[x]["u1"].ToString(), t2, page_url);
                                    }
                                }
                            }

                            t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:600px; height:400px\">" + t2 + "</textarea></td></tr>";
                        }


                    }
                    else
                    {
                        Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                        string[] cc = reg3.Split(u13);
                        if (cc[0].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[0].ToString(), cc[1].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[2].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[2].ToString(), cc[3].ToString(), RegexOptions.Singleline);
                        }
                        if (cc[4].ToString() != "")
                        {
                            t2 = Regex.Replace(t2, cc[4].ToString(), cc[5].ToString(), RegexOptions.Singleline);
                        }
                        if (Request.QueryString["u14"].ToString() != "")
                        {
                            DataTable dt1 = my_c.GetTable("select u2,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in(" + Request.QueryString["u14"].ToString() + ")");
                            for (int x = 0; x < dt1.Rows.Count; x++)
                            {
                                if (t2 != "")
                                {
                                    t2 = no_m.set_html(dt1.Rows[x]["u1"].ToString(), t2, page_url);
                                }
                            }
                        }


                        t1 = t1 + " <tr>                <td class=\"tRight\"> <span style='display:none' id=\"div_Fields" + bb[1].ToString() + "_u1\">" + dt.Rows[0]["u1"].ToString() + "</span>                   " + dt.Rows[0]["u2"].ToString() + "：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + bb[1].ToString() + "_qian\" id=\"div_Fields" + bb[1].ToString() + "_content\" cols=\"45\" rows=\"5\" style=\"width:300px; height:100px\">" + t2 + "</textarea></td></tr>";
                    }

                }
                Response.Write(type);
                Response.Write("{fzw:collect_ajax}");
                Response.Write("<table class=\"cTable_2 table\">            <tr class=\"cTitle toolbarBg\">                <td width=\"25%\">                    <div>                        内容页采集预览</div>                </td>                <td colspan=\"2\">                </td>            </tr><tr>                <td class=\"tRight\">                    测试列表页URL ：</td>                <td colspan=\"2\">  " + Request.QueryString["u11"].ToString() + "</td>            </tr>" + t1 + "</table>");
            }










        }
    }
}
