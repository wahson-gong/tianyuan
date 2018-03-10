using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
/// <summary>
/// my_html 的摘要说明
/// </summary>
public class my_html
{
    //全局变量
    string ziduan_id = ""; //字段ID比如：p1
    int count_id = 0;//总记录数
    int list_size = 0;//列表页页码数
    string listid = "";//栏目ID
    //全局变量结束

    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    my_hanshu my_hs = new my_hanshu();
    //设置UBB标签
    public string get_ubb(string type, string content)
    {
        Regex reg = new Regex("{" + type + ".*?}", RegexOptions.Singleline);
        Match matches = reg.Match(content);
        string t1 = matches.ToString().Replace(type, "").Replace("{", "").Replace("}", "").Replace(":", "");
        return t1;
    }
    //
    static string get_ding_dir_str = "";
    //列出最顶级目录
    //列出最顶级目录
    public string Parameterweizhi(string classid)
    {


        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + classid + " and classid>0");
        //if (classid == "200")
        //{
        //    HttpContext.Current.Response.Write(dt1.Rows[0]["classid"].ToString());
        //    HttpContext.Current.Response.End();
        //}
        if (dt1.Rows.Count > 0)
        {
            if (get_ding_dir_str == "")
            {
                get_ding_dir_str = dt1.Rows[0]["u1"].ToString();
            }
            else
            {
                get_ding_dir_str = dt1.Rows[0]["u1"].ToString() + ">" + get_ding_dir_str;
            }
            if (dt1.Rows[0]["classid"].ToString() != "196")
            {
                Parameterweizhi(dt1.Rows[0]["classid"].ToString());

            }
        }

        return get_ding_dir_str.ToString();
    }
    //
    //设置标签
    public string set_other(string g1, string g2)
    {

        string t1 = g2;
        string fzw_yanse = "";
        string fzw_jiacu = "";
        fzw_yanse = get_ubb("color", t1);
        fzw_jiacu = get_ubb("b", t1);
        if (fzw_yanse != "")
        {
            Regex reg = new Regex("{.*?}", RegexOptions.Singleline);
            t1 = reg.Replace(t1, "");
        }
        if (g1.IndexOf("size") > -1)
        {
            string t2 = g1.Replace("size=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            t1 = my_b.jiequ("yes", t1, int.Parse(t2));

        }
        if (g1.IndexOf("string") > -1)
        {
            string t2 = g1.Replace("string=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            if (t2 == "￥")
            {
                try
                {
                    t1 = my_b.get_jiage(float.Parse(t1));
                }
                catch
                {
                    t1 = "0";
                }

            }
            if (t2 == "parlist")
            {
                get_ding_dir_str = "";
                t1 = Parameterweizhi(t1);


            }
            if (t2 == "Parameter")
            {
                t1 = my_b.get_ding_dir(t1);


            }
            if (t2 == "age")
            {
                t1 = my_b.c_time(t1);
            }
            if (t2 == "TimeSpan")
            {

                DateTime dy2 = DateTime.Parse(t1);
                TimeSpan span = DateTime.Now - dy2;
                return span.TotalDays.ToString();
            }
            if (t2 == "shijian")
            {
                try
                {
                    t1 = my_b.DateStringFromNow(DateTime.Parse(t1));
                }
                catch
                {
                    t1 = t1;
                }

            }
            if (t2 == "shouji")
            {
                Regex re = new Regex("(\\d{3})(\\d{4})(\\d{4})", RegexOptions.None);
                t1 = re.Replace(t1, "$1****$3");

            }
            if (t2 == "pinyin")
            {
                try
                {
                    t1 = my_b.pinyin(t1, 100);
                }
                catch
                {
                    t1 = t1;
                }

            }
            if (t2 == "zutu")
            {
                string[] aa = Regex.Split(t1, "{next}");
                t1 = "";
                for (int j = 0; j < aa.Length; j++)
                {
                    string[] b = Regex.Split(aa[j], "{title}");
                    t1 = t1 + "<li><img src=\"" + b[0].ToString() + "\" /></li>";
                }
            }

        }
        else if (g1.IndexOf("htmlcode") > -1)
        {
            string t2 = g1.Replace("htmlcode=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            if (t2 == "yes")
            {
                t1 = HttpContext.Current.Server.UrlEncode(t1);
            }
            else
            {
                t1 = HttpContext.Current.Server.UrlDecode(t1);
            }
        }
        else if (g1.IndexOf("nohtml") > -1)
        {
            string t2 = g1.Replace("nohtml=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            if (t2 == "yes")
            {
                t1 = my_b.NoHTML(t1);
            }
        }
        else if (g1.IndexOf("datetime") > -1)
        {
            string t2 = g1.Replace("datetime=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            DateTime b = new DateTime();
            try
            {
                b = DateTime.Parse(t1);
            }
            catch {
                //HttpContext.Current.Response.Write(t1);
                //HttpContext.Current.Response.End();
            }
            string b_Year = b.Year.ToString();
            string b_Month = b.Month.ToString();
            if (int.Parse(b_Month) < 10)
            {
                b_Month = "0" + b_Month;
            }
            string b_Day = b.Day.ToString();
            if (int.Parse(b_Day) < 10)
            {
                b_Day = "0" + b_Day;
            }
            string b_Hour = b.Hour.ToString();
            if (int.Parse(b_Hour) < 10)
            {
                b_Hour = "0" + b_Hour;
            }
            string b_Minute = b.Minute.ToString();
            if (int.Parse(b_Minute) < 10)
            {
                b_Minute = "0" + b_Minute;
            }
            string b_Second = b.Second.ToString();
            if (int.Parse(b_Second) < 10)
            {
                b_Second = "0" + b_Second;
            }
            t1 = t2.Replace("yyyy", b_Year).Replace("MM", b_Month).Replace("dd", b_Day).Replace("hh", b_Hour).Replace("mm", b_Minute).Replace("ss", b_Second);
        }
        else if (g1.IndexOf("display") > -1)
        {
            string t2 = g1.Replace("display=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            if (t2 == "img")
            {
                t1 = my_b.get_article_pic(t1);
            }

        }
        else if (g1.IndexOf("Replace") > -1)
        {
            string t2 = g1.Replace("Replace=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            t2 = t2.Replace(" ", "");

            if (t2.Split('|')[0].ToString() == "")
            {
                t1 = t1.Replace("\r\n", t2.Split('|')[1].ToString());
            }
            else if (t2.Split('|')[0].ToString() == "zu")
            {
                t1 = t1.Replace("{zu}", t2.Split('|')[1].ToString());
            }
            else
            {
                t1 = t1.Replace(t2.Split('|')[0].ToString(), t2.Split('|')[1].ToString());
            }

        }
        else if (g1.IndexOf("lable") > -1)
        {
            string t2 = g1.Replace("lable=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");

            Regex reg1 = new Regex(" |,", RegexOptions.Singleline);

            string[] aa = reg1.Split(t1);
            string t3 = "";
            for (int x = 0; x < aa.Length; x++)
            {
                string lableurl = t2.Replace("&dangqian", aa[x]);
                t3 = t3 + "<a href='" + lableurl + "'>" + aa[x] + "</a>";
            }
            t1 = t3;

        }
        else if (g1.IndexOf("split") > -1)
        {
          

            string t2 = g1.Replace("split=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");

            Regex reg1 = new Regex("fenge", RegexOptions.Singleline);
            if (reg1.Split(t2)[0].ToString() == "no")
            {

                string[] cc = t1.Split('|');
                t1 = cc[int.Parse(reg1.Split(t2)[1].ToString())];

            }
            else if (reg1.Split(t2)[0].ToString().IndexOf("$") > -1)
            {


                Regex reg = new Regex(reg1.Split(t2)[0].ToString().Replace("$", ""), RegexOptions.Singleline);
                string[] cc = reg.Split(t1.Replace("$", ""));

                t1 = cc[int.Parse(reg1.Split(t2)[1].ToString())];

            }
            else
            {
                
                Regex reg = new Regex(reg1.Split(t2)[0].ToString().Trim(), RegexOptions.Singleline);
                string[] cc = reg.Split(t1);
         
                t1 = cc[int.Parse(reg1.Split(t2)[1].ToString())];
              
            }
        }
        else if (g1.IndexOf("sql") > -1)
        {
            string t2 = g1.Replace("sql=", "");
            t2 = t2.Replace("\"", "");
            t2 = t2.Replace("/}", "");
            string sql = "";
            if (t2.Split('|')[2].ToString() == "count")
            {
                sql = "select count(id) as count_id from " + t2.Split('|')[0].ToString() + " " + t2.Split('|')[1].ToString().Replace("&nbsp;", " ").Replace("&dangqian", t1) + " ";
            }
            else if (t2.Split('|')[2].ToString() == "sum")
            {
                sql = "select sum(" + t2.Split('|')[3].ToString() + ") as count_id from " + t2.Split('|')[0].ToString() + " " + t2.Split('|')[1].ToString().Replace("&nbsp;", " ").Replace("&dangqian", t1) + " ";
            }
            else if (t2.Split('|')[2].ToString() == "avg")
            {
                sql = "select avg(" + t2.Split('|')[3].ToString() + ") as count_id from " + t2.Split('|')[0].ToString() + " " + t2.Split('|')[1].ToString().Replace("&nbsp;", " ").Replace("&dangqian", t1) + " ";
            }
            else
            {
                sql = "select top 1 " + t2.Split('|')[2].ToString() + " from " + t2.Split('|')[0].ToString() + " " + t2.Split('|')[1].ToString().Replace("&nbsp;", " ").Replace("&dangqian", t1) + " order by id desc";
            }
            DataTable dt = new DataTable();
            dt = my_c.GetTable(sql);


            if (dt.Rows.Count > 0)
            {
                if (t2.Split('|')[2].ToString() == "count")
                {
                    if (dt.Rows[0]["count_id"].ToString() == "")
                    {
                        t1 = "0";
                    }
                    else
                    {
                        t1 = dt.Rows[0]["count_id"].ToString();
                    }
                }
                else if (t2.Split('|')[2].ToString() == "sum")
                {
                    if (dt.Rows[0]["count_id"].ToString() == "")
                    {
                        t1 = "0";
                    }
                    else
                    {
                        t1 = dt.Rows[0]["count_id"].ToString();
                    }
                }
                else if (t2.Split('|')[2].ToString() == "avg")
                {
                    if (dt.Rows[0]["count_id"].ToString() == "")
                    {
                        t1 = "0";
                    }
                    else
                    {
                        t1 = dt.Rows[0]["count_id"].ToString();
                    }
                }
                else
                {

                    t1 = dt.Rows[0][t2.Split('|')[2].ToString()].ToString();
                }
            }
            else
            {

                t1 = "";
            }

            // t1 = t1.Replace(t2.Split('|')[0].ToString(), t2.Split('|')[1].ToString());

            //HttpContext.Current.Response.Write(sql);
            //HttpContext.Current.Response.End();
        }

        if (t1 != "")
        {


            string css_str = "";
            if (fzw_yanse != "")
            {
                css_str = "color:" + fzw_yanse + "";
            }
            if (fzw_jiacu == "yes")
            {
                if (css_str == "")
                {
                    css_str = "font-weight:700";
                }
                else
                {
                    css_str = css_str + "; font-weight:700";
                }
            }

            if (css_str != "")
            {
                t1 = "<font style=\"" + css_str + "\">" + t1 + "</font>";
            }
        }


        return t1;
    }
    //设置标签结束

    //获取Lable
    //g1 内容  g2标签名
    public string get_lable(string g1, string g2)
    {
        g1 = Regex.Match(g1, "" + g2 + "=" + @""".*?""|" + g2 + "=" + @".*?[\S]", RegexOptions.Singleline).ToString();
        g1 = g1.Replace(g2 + "=", "");
        g1 = g1.Replace("\"", "");
        return g1;
    }
    //获取Lable结束

    //设置URL开始
    public string set_url(string file_content)
    {
        Regex reg4 = new Regex(@"{fzw:aspx.*?}", RegexOptions.Singleline);
        MatchCollection matches4 = reg4.Matches(file_content);
        foreach (Match match4 in matches4)
        {

            string aspx = match4.ToString().Replace("{fzw:aspx", "");
            aspx = aspx.Replace("src=", "");
            aspx = aspx.Replace("/}", "");
            aspx = aspx.Replace("}", "");
            aspx = aspx.Replace("\"", "");
            aspx = my_b.c_string(aspx);
            string aspx_conetnt = "";
            string web_url = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();

            if (web_url == "localhost" || web_url == "127.0.0.1" || web_url.IndexOf("192.") > -1)
            {

                web_url = "http://" + web_url + ":" + HttpContext.Current.Request.ServerVariables["Server_Port"].ToString();
            }
            else
            {
                web_url = "http://" + web_url;
            }

            web_url = web_url + aspx;
            //HttpContext.Current.Response.Write(web_url);
            //HttpContext.Current.Response.End();
            try
            {
                aspx_conetnt = my_b.getWebFile(web_url);
            }
            catch
            {
                HttpContext.Current.Response.Write(web_url);
                HttpContext.Current.Response.End();
            }

            file_content = file_content.Replace(match4.ToString(), aspx_conetnt);
        }

        file_content = set_otherkey(file_content);
        Regex reg = new Regex(@"href="".*?""|src="".*?""|background-image:url[\s\S]*?\)|background="".*?""|background=.*?|background:url[\s\S]*?\)", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            string url_string = match.ToString();
            url_string = url_string.Replace("href=", "").Replace("src=", "").Replace("\"", "").Replace("background-image:url(", "").Replace(")", "").Replace("background=", "").Replace("background:url(", "");
            if (url_string.IndexOf("/") == 0 || url_string.IndexOf("#") == 0)
            {
                try
                {
                    if (HttpContext.Current.Request.ApplicationPath.ToString() != "/")
                    {
                        if (url_string.IndexOf(HttpContext.Current.Request.ApplicationPath.ToString()) == -1)
                        {
                            file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, HttpContext.Current.Request.ApplicationPath.ToString() + url_string));
                        }
                    }

                }
                catch { }
            }
            else
            {
                if (url_string.IndexOf("{other}") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), match.ToString().Replace("{other}", ""));
                }
                else if (url_string.IndexOf("http://") == -1 && url_string.IndexOf("ftp://") == -1 && url_string.IndexOf("https://") == -1 && url_string.IndexOf("javascript:") == -1 && url_string.IndexOf("tel:") == -1 && url_string.IndexOf("mailto:") == -1 && url_string.IndexOf("mqqwpa:") == -1 && url_string.IndexOf("tencent://") == -1 && url_string.IndexOf("sms:") == -1)
                {
                    if (url_string.IndexOf("$") != 0)
                    {

                        try
                        {
                            if (HttpContext.Current.Request.ApplicationPath.ToString() == "/")
                            {
                                file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + url_string)));
                            }
                            else
                            {
                                file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, HttpContext.Current.Request.ApplicationPath.ToString() + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + url_string)));
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        //  HttpContext.Current.Response.Write(url_string + "|" + url_string.Substring(1) + "<br>");
                        file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, url_string.Substring(1)));
                    }
                }
            }

            //end

        }

        // HttpContext.Current.Response.End();
        //设置if
        Regex reg8 = new Regex(@"{if.*?{/if}", RegexOptions.Singleline);
        MatchCollection matches8 = reg8.Matches(file_content);
        foreach (Match match8 in matches8)
        {
            string if_string = match8.ToString();
            string value1 = get_lable(if_string, "value1");
            string value2 = get_lable(if_string, "value2");
            string bi = get_lable(if_string, "bi");
            string neirong1 = "";
            string neirong2 = "";
            Regex reg9 = new Regex(@"{if.*?{else}|{if.*?{/if}", RegexOptions.Singleline);
            Match match9 = reg9.Match(if_string);
            Regex reg10 = new Regex(@"{if.*?}", RegexOptions.Singleline);
            neirong1 = reg10.Replace(match9.ToString(), "");
            neirong1 = neirong1.Replace("{else}", "");
            Regex reg11 = new Regex(@"{else}.*?{/if}", RegexOptions.Singleline);
            Match match911 = reg11.Match(if_string);
            neirong2 = match911.ToString().Replace("{else}", "");
            neirong2 = neirong2.Replace("{/if}", "");

            if (bi == "==")
            {
                if (value1 == value2)
                {
                    file_content = file_content.Replace(match8.ToString(), neirong1);
                }
                else
                {
                    file_content = file_content.Replace(match8.ToString(), neirong2);
                }
            }
            else if (bi == "IndexOf")
            {
                if (value2.IndexOf(value1) > -1)
                {
                    file_content = file_content.Replace(match8.ToString(), neirong1);
                }
                else
                {
                    file_content = file_content.Replace(match8.ToString(), neirong2);
                }
            }
            else if (bi == "!=")
            {
                if (value1 != value2)
                {
                    file_content = file_content.Replace(match8.ToString(), neirong1);
                }
                else
                {
                    file_content = file_content.Replace(match8.ToString(), neirong2);
                }
            }
            else if (bi == ">")
            {
                if (value1 == "")
                {
                    value1 = "0";
                }
                if (value2 == "")
                {
                    value2 = "0";
                }

                if (float.Parse(value1) > float.Parse(value2))
                {
                    file_content = file_content.Replace(match8.ToString(), neirong1);
                }
                else
                {
                    file_content = file_content.Replace(match8.ToString(), neirong2);
                }
            }
            else if (bi == ">=")
            {
                if (value1 == "")
                {
                    value1 = "0";
                }
                if (value2 == "")
                {
                    value2 = "0";
                }

                if (float.Parse(value1) >= float.Parse(value2))
                {
                    file_content = file_content.Replace(match8.ToString(), neirong1);
                }
                else
                {
                    file_content = file_content.Replace(match8.ToString(), neirong2);
                }
            }
            else if (bi == "<")
            {
                if (value1 == "")
                {
                    value1 = "0";
                }
                if (value2 == "")
                {
                    value2 = "0";
                }

                if (float.Parse(value1) < float.Parse(value2))
                {
                    file_content = file_content.Replace(match8.ToString(), neirong1);
                }
                else
                {
                    file_content = file_content.Replace(match8.ToString(), neirong2);
                }
            }
            else if (bi == "<=")
            {
                if (value1 == "")
                {
                    value1 = "0";
                }
                if (value2 == "")
                {
                    value2 = "0";
                }

                if (float.Parse(value1) <= float.Parse(value2))
                {
                    file_content = file_content.Replace(match8.ToString(), neirong1);
                }
                else
                {
                    file_content = file_content.Replace(match8.ToString(), neirong2);
                }
            }

        }
        //ajax处理开始
        Regex ajax_reg = new Regex(@"{ajax:.*?/}", RegexOptions.Singleline);
        MatchCollection ajax_matches = ajax_reg.Matches(file_content);
        foreach (Match ajax_match in ajax_matches)
        {

        }
        file_content = set_ajax(file_content);
        //ajax处理完成
        return file_content;
    }
    public string set_other_url(string file_content)
    {
        file_content = set_otherkey(file_content);
        Regex reg = new Regex(@"href="".*?""|src="".*?""|background-image:url[\s\S]*?\)|background="".*?""|background=.*?|background:url[\s\S]*?\)", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            string url_string = match.ToString();
            url_string = url_string.Replace("href=", "").Replace("src=", "").Replace("\"", "").Replace("background-image:url(", "").Replace(")", "").Replace("background=", "").Replace("background:url(", "");
            if (url_string.IndexOf("/") == 0)
            {
                try
                {
                    if (HttpContext.Current.Request.ApplicationPath.ToString() != "/")
                    {
                        if (url_string.IndexOf(HttpContext.Current.Request.ApplicationPath.ToString()) == -1)
                        {
                            file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, HttpContext.Current.Request.ApplicationPath.ToString() + url_string));
                        }
                    }

                }
                catch { }
            }
            else
            {
                if (url_string.IndexOf("http://") == -1 && url_string.IndexOf("ftp://") == -1 && url_string.IndexOf("javascript:") == -1 && url_string.IndexOf("mailto:") == -1 && url_string.IndexOf("https:") == -1 && url_string.IndexOf("tel:") == -1)
                {
                    if (url_string.IndexOf("$") != 0)
                    {

                        try
                        {
                            if (HttpContext.Current.Request.ApplicationPath.ToString() == "/")
                            {
                                file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, "/" + url_string));
                            }
                            else
                            {
                                file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, HttpContext.Current.Request.ApplicationPath.ToString() + "/" + url_string));
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        //  HttpContext.Current.Response.Write(url_string + "|" + url_string.Substring(1) + "<br>");
                        file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, url_string.Substring(1)));
                    }
                }
            }

            //end

        }
        // HttpContext.Current.Response.End();
        return file_content;
    }
    //设置URL结束

    //设置其它默认标签
    public string set_otherkey(string file_content)
    {
        //listurl
        Regex reg5 = new Regex(@"{listurl.*?}", RegexOptions.Singleline);
        MatchCollection matches5 = reg5.Matches(file_content);

        foreach (Match match5 in matches5)
        {

            string classid = get_lable(match5.ToString(), "classid").Trim();
            string m = get_lable(match5.ToString(), "m").Trim();

            string listurl = "";
            if (my_b.set_mode() == "伪静态")
            {
                listurl = "/list_" + classid + "_" + m + ConfigurationSettings.AppSettings["Suffix"].ToString();
            }
            else if (my_b.set_mode() == "静态网站")
            {
                try
                {
                    listurl = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + " and Model_id=" + m + "").Rows[0]["u7"].ToString();
                }
                catch
                {

                }

            }
            else
            {
                listurl = "/list.aspx?id=" + classid + "&Model_id=" + m + "";
            }
            file_content = file_content.Replace(match5.ToString(), listurl);
        }


        //pageurl
        Regex reg6 = new Regex(@"{pageurl.*?}", RegexOptions.Singleline);
        MatchCollection matches6 = reg6.Matches(file_content);

        foreach (Match match6 in matches6)
        {

            string classid = get_lable(match6.ToString(), "classid");
            string page_match = match6.ToString().Replace("classid", "");
            string id = get_lable(page_match, "id");

            string pageurl = "";

            if (my_b.set_mode() == "伪静态")
            {
                pageurl = "/page_" + classid + "_" + id + ConfigurationSettings.AppSettings["Suffix"].ToString();
            }
            else if (my_b.set_mode() == "静态网站")
            {

                try
                {
                    string table_name = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id in (select Model_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + ")").Rows[0]["u1"].ToString();

                    DataTable dt = my_c.GetTable("select * from " + table_name + " where id=" + id);
                    pageurl = dt.Rows[0]["Filepath"].ToString() + dt.Rows[0]["id"].ToString() + ConfigurationSettings.AppSettings["Suffix"].ToString();
                }
                catch
                { }


            }
            else
            {
                pageurl = "/page.aspx?id=" + id + "&classid=" + classid + "";
            }
            file_content = file_content.Replace(match6.ToString(), pageurl);
        }


        return file_content;
    }

    //设置其它默认标签END

    //设置AJAX
    public string set_ajax(string file_content)
    {

        file_content = set_sitekey(file_content);

        Regex ajax_reg = new Regex(@"{ajax:.*?/}", RegexOptions.Singleline);
        MatchCollection ajax_matches = ajax_reg.Matches(file_content);

        if (ajax_matches.Count > 0)
        {
            if (file_content.IndexOf("/inc/page_ajax.js") == -1)
            {
                try
                {
                    file_content = file_content.Insert(file_content.IndexOf("</head>"), "<script type=\"text/javascript\" src=\"/inc/page_ajax.js\"></script>");
                }
                catch
                {
                    file_content = file_content.Insert(file_content.IndexOf("</HEAD>"), "<script type=\"text/javascript\" src=\"/inc/page_ajax.js\"></script>");
                }

            }
        }
        foreach (Match ajax_match in ajax_matches)
        {

            file_content = file_content.Replace(ajax_match.ToString(), "set_ajax('" + HttpUtility.UrlEncode(ajax_match.ToString()) + "');");
        }

        return file_content;
    }
    //设置AJAX结束

    //设置默认标签
    public string set_sitekey(string file_content)
    {

        //引用文件
        Regex reg1 = new Regex(@"{fzw:inside.*?}", RegexOptions.Singleline);
        MatchCollection matches1 = reg1.Matches(file_content);
        foreach (Match match1 in matches1)
        {
            string inside = match1.ToString().Replace("{fzw:inside", "");
            inside = inside.Replace("src=", "");
            inside = inside.Replace("/}", "");
            inside = inside.Replace("}", "");
            inside = inside.Replace("\"", "");
            inside = my_b.c_string(inside);
            string inside_conetnt = "";
            if (inside.IndexOf("/") > -1)
            {
                inside_conetnt = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + inside, Encoding.UTF8);
            }
            else
            {
                inside_conetnt = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + inside), Encoding.UTF8);
            }
            file_content = file_content.Replace(match1.ToString(), inside_conetnt);
        }


        DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");
        // Regex reg = new Regex(@"{fzw:key:[\w\d|:]*?/}|{fzw:config:[\w\d|:]*?/}", RegexOptions.Singleline);
        Regex reg = new Regex(@"{fzw:key:.*?/}|{fzw:config:.*?/}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        //HttpContext.Current.Response.Write(file_content);
        //HttpContext.Current.Response.End();
        foreach (Match match in matches)
        {

            if (match.ToString().IndexOf("config") > -1)
            {
                string config_str = match.ToString().Replace("{fzw:config:", "").Replace("/}", "").Replace("}", "").Trim();

                file_content = file_content.Replace(match.ToString(), xml_dt.Rows[0][config_str].ToString());
            }
            else
            {
                if (match.ToString().IndexOf("sitename") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), xml_dt.Rows[0]["u1"].ToString());
                }
                else if (match.ToString().IndexOf("fenlei") > -1)
                {
                    fenlei = "";
                    try
                    {
                        get_fenlei(HttpContext.Current.Request.QueryString["fenlei"].ToString(), 0);
                    }
                    catch
                    {
                        get_fenlei("194", 0);
                    }
                    file_content = file_content.Replace(match.ToString(), fenlei);
                }
                else if (match.ToString().IndexOf("sitekey") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), xml_dt.Rows[0]["u3"].ToString());
                }
                else if (match.ToString().IndexOf("sitedesc") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), xml_dt.Rows[0]["u4"].ToString());
                }
                else if (match.ToString().IndexOf("subtitle") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), xml_dt.Rows[0]["u2"].ToString());
                }
                else if (match.ToString().IndexOf("siteweb") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), my_b.get_Domain());
                }
                else if (match.ToString().IndexOf("jifen") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), xml_dt.Rows[0]["u23"].ToString());
                }
                else if (match.ToString().IndexOf("siteurl") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), my_b.tihuan(HttpContext.Current.Request.Url.ToString(), "&", "fzw123"));

                }
                else if (match.ToString().IndexOf("filename") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), System.IO.Path.GetFileName(HttpContext.Current.Request.Path).ToString().ToLower());

                }
                else if (match.ToString().IndexOf("httpurl") > -1)
                {
                    try
                    {
                        file_content = file_content.Replace(match.ToString(), my_b.tihuan(HttpContext.Current.Request.ServerVariables["http_referer"].ToString(), "&", "fzw123"));
                    }
                    catch
                    {
                        file_content = file_content.Replace(match.ToString(), my_b.tihuan(HttpContext.Current.Request.Url.ToString(), "&", "fzw123"));
                    }
                }
                else if (match.ToString().IndexOf("time") > -1)
                {

                    #region 获取key time
                    DateTime dy = DateTime.Now;
                    string shijian = dy.ToString();
                    if (match.ToString().ToLower().IndexOf("hours") > -1)
                    {
                        string time_str = match.ToString().ToLower();
                        Regex time_reg = new Regex(@"hours=.*?}", RegexOptions.Singleline);
                        Match ma = time_reg.Match(time_str);

                        time_str = ma.ToString().Replace("hours=", "");
                        time_str = time_str.Replace("\"", "");
                        time_str = time_str.Replace("/", "");
                        time_str = time_str.Replace("}", "");
                        time_str = time_str.Trim();

                        dy = dy.AddHours(double.Parse(time_str));
                        shijian = dy.ToString();
                        file_content = file_content.Replace(match.ToString(), shijian);
                    }
                    else if (match.ToString().ToLower().IndexOf("minutes") > -1)
                    {

                        string time_str = match.ToString().ToLower();
                        Regex time_reg = new Regex(@"minutes=.*?}", RegexOptions.Singleline);
                        Match ma = time_reg.Match(time_str);

                        time_str = ma.ToString().Replace("minutes=", "");
                        time_str = time_str.Replace("\"", "");
                        time_str = time_str.Replace("/", "");
                        time_str = time_str.Replace("}", "");
                        time_str = time_str.Trim();

                        dy = dy.AddMinutes(double.Parse(time_str));
                        shijian = dy.ToString();
                        file_content = file_content.Replace(match.ToString(), shijian);
                    }
                    else if (match.ToString().ToLower().IndexOf("seconds") > -1)
                    {
                        string time_str = match.ToString().ToLower();
                        Regex time_reg = new Regex(@"Seconds=.*?}", RegexOptions.Singleline);
                        Match ma = time_reg.Match(time_str);

                        time_str = ma.ToString().Replace("seconds=", "");
                        time_str = time_str.Replace("\"", "");
                        time_str = time_str.Replace("/", "");
                        time_str = time_str.Replace("}", "");
                        time_str = time_str.Trim();

                        dy = dy.AddSeconds(double.Parse(time_str));
                        shijian = dy.ToString();
                        file_content = file_content.Replace(match.ToString(), shijian);
                    }
                    else if (match.ToString().ToLower().IndexOf("day") > -1)
                    {
                        string time_str = match.ToString().ToLower();
                        Regex time_reg = new Regex(@"day=.*?}", RegexOptions.Singleline);
                        Match ma = time_reg.Match(time_str);

                        time_str = ma.ToString().Replace("day=", "");
                        time_str = time_str.Replace("\"", "");
                        time_str = time_str.Replace("/", "");
                        time_str = time_str.Replace("}", "");
                        time_str = time_str.Trim();

                        dy = dy.AddDays(double.Parse(time_str));
                        shijian = my_b.set_time(dy.ToString(), "yyyy-MM-dd");
                        file_content = file_content.Replace(match.ToString(), shijian);
                    }
                    else if (match.ToString().ToLower().IndexOf("month") > -1)
                    {
                        string time_str = match.ToString().ToLower();
                        Regex time_reg = new Regex(@"month=.*?}", RegexOptions.Singleline);
                        Match ma = time_reg.Match(time_str);

                        time_str = ma.ToString().Replace("month=", "");
                        time_str = time_str.Replace("\"", "");
                        time_str = time_str.Replace("/", "");
                        time_str = time_str.Replace("}", "");
                        time_str = time_str.Trim();

                        dy = dy.AddMonths(int.Parse(time_str));
                        shijian = my_b.set_time(dy.ToString(), "yyyy-MM-dd");

                        file_content = file_content.Replace(match.ToString(), shijian);
                    }

                    else if (match.ToString().ToLower().IndexOf("year") > -1)
                    {
                        string time_str = match.ToString().ToLower();
                        Regex time_reg = new Regex(@"year=.*?}", RegexOptions.Singleline);
                        Match ma = time_reg.Match(time_str);

                        time_str = ma.ToString().Replace("year=", "");
                        time_str = time_str.Replace("\"", "");
                        time_str = time_str.Replace("/", "");
                        time_str = time_str.Replace("}", "");
                        time_str = time_str.Trim();

                        dy = dy.AddYears(int.Parse(time_str));
                        shijian = my_b.set_time(dy.ToString(), "yyyy-MM-dd");
                        file_content = file_content.Replace(match.ToString(), shijian);
                    }
                    else
                    {
                        file_content = file_content.Replace(match.ToString(), shijian);
                    }
                    #endregion

                }
                else if (match.ToString().IndexOf("mianyunfei") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), ConfigurationSettings.AppSettings["mianyunfei"].ToString());
                }
                else if (match.ToString().IndexOf("yunfei") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), ConfigurationSettings.AppSettings["yunfei"].ToString());
                }
                else if (match.ToString().IndexOf("bianhao") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), my_b.get_bianhao());
                }
                else if (match.ToString().IndexOf("ip") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), HttpContext.Current.Request.UserHostAddress.ToString());
                }
                else if (match.ToString().IndexOf("huiyuanka") > -1)
                {
                    file_content = file_content.Replace(match.ToString(), my_b.get_huiyuanka());
                }
            }
            //HttpContext.Current.Response.Write(file_content.ToString());
            //HttpContext.Current.Response.End();

        }



        //调用广告
        Regex reg2 = new Regex(@"{fzw:ad:.*?}", RegexOptions.Singleline);
        MatchCollection matches2 = reg2.Matches(file_content);
        foreach (Match match2 in matches2)
        {
            string ad_string = match2.ToString().Replace("{fzw:ad:", "");
            ad_string = ad_string.Replace("/}", "");
            ad_string = ad_string.Replace("}", "");
            ad_string = ad_string.Trim();
            string u6 = set_other_url(my_b.get_value("u6", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad", "where u1='" + ad_string + "'"));

            file_content = file_content.Replace(match2.ToString(), u6);
        }
        //获取url参数
        Regex reg3 = new Regex(@"{fzw:url:.*?}", RegexOptions.Singleline);
        MatchCollection matches3 = reg3.Matches(file_content);
        foreach (Match match3 in matches3)
        {

            string url_string = match3.ToString().Replace("{fzw:url:", "");
            url_string = url_string.Replace("}", "");
            url_string = url_string.Replace("/}", "");
            url_string = url_string.Replace("/", "");
            url_string = url_string.Trim();
            string guizhe = "";
            try
            {
                guizhe = url_string.Substring(url_string.IndexOf(" "));
                url_string = url_string.Substring(0, url_string.IndexOf(" "));


            }
            catch
            {
            }
            url_string = my_b.tihuan(url_string, "fzw123", "&");

            //HttpContext.Current.Response.Write(guizhe);
            //HttpContext.Current.Response.End();
            try
            {
                string urlstr = HttpContext.Current.Request.QueryString[url_string].ToString();
                // urlstr = HttpContext.Current.Server.UrlEncode(urlstr);
                if (guizhe != "")
                {
                    urlstr = set_other(guizhe, urlstr);
                }

                file_content = file_content.Replace(match3.ToString(), urlstr);
            }
            catch
            {
                file_content = file_content.Replace(match3.ToString(), "");
            }

        }
        //获取enurl参数

        Regex reg8 = new Regex(@"{fzw:curl:.*?}", RegexOptions.Singleline);
        MatchCollection matches8 = reg8.Matches(file_content);
        foreach (Match match8 in matches8)
        {

            string url_string = match8.ToString().Replace("{fzw:curl:", "");
            url_string = url_string.Replace("}", "");
            url_string = url_string.Replace("/}", "");
            url_string = url_string.Replace("/", "");
            url_string = url_string.Trim();
            //HttpContext.Current.Response.Write(url_string);
            //HttpContext.Current.Response.End();
            try
            {
                string urlstr = HttpContext.Current.Request.QueryString[url_string].ToString().Replace(",", "");
                urlstr = HttpContext.Current.Server.UrlEncode(urlstr);

                file_content = file_content.Replace(match8.ToString(), urlstr);
            }
            catch
            {
                file_content = file_content.Replace(match8.ToString(), "");
            }

        }
        //文字编码
        Regex reg10 = new Regex(@"{fzw:string:.*?}", RegexOptions.Singleline);
        MatchCollection matches10 = reg10.Matches(file_content);
        foreach (Match match10 in matches10)
        {

            string url_string = match10.ToString().Replace("{fzw:string:", "");
            url_string = url_string.Replace("}", "");
            url_string = url_string.Replace("/}", "");
            url_string = url_string.Replace("/", "");
            url_string = url_string.Trim();
            //HttpContext.Current.Response.Write(url_string);
            //HttpContext.Current.Response.End();
            try
            {
                string stringstr = HttpContext.Current.Server.UrlEncode(url_string);


                file_content = file_content.Replace(match10.ToString(), stringstr);
            }
            catch
            {
                file_content = file_content.Replace(match10.ToString(), "");
            }

        }
        //生成cookie标签处理
        Regex reg11 = new Regex(@"{fzw:setcookie.*?}", RegexOptions.Singleline);
        MatchCollection matches11 = reg11.Matches(file_content);
        foreach (Match match11 in matches11)
        {
           

            string cookie_name = get_lable(match11.ToString(), "name");
            cookie_name = cookie_name.Trim();
            string cookie_value = get_lable(match11.ToString(), "value");
            cookie_value = cookie_value.Trim();
            if (cookie_value == "ip")
            {
                cookie_value = HttpContext.Current.Request.UserHostAddress.ToString();
            }
            my_b.c_cookie(cookie_value, cookie_name);
            //HttpContext.Current.Response.Write(my_b.k_cookie(cookie_name));
            //HttpContext.Current.Response.End();
            file_content = file_content.Replace(match11.ToString(), "");
        }
        //生成单页面url
        Regex reg12 = new Regex(@"{fzw:single.*?}", RegexOptions.Singleline);
        MatchCollection matches12= reg12.Matches(file_content);
 
        foreach (Match match12 in matches12)
        {
         
            string url = get_lable(match12.ToString(), "url");
            url = url.Trim();
            if (my_b.set_mode() == "伪静态")
            {
                url = "/" + url;
            }
            else if (my_b.set_mode() == "静态网站")
            {
                url = "/" + url;

            }
            else
            {
                url = "/?"+url;
            }
            file_content = file_content.Replace(match12.ToString(), url);
        }
        //cookie标签处理
        Regex reg6 = new Regex(@"{fzw:cookie.*?}", RegexOptions.Singleline);
        MatchCollection matches6 = reg6.Matches(file_content);
        foreach (Match match6 in matches6)
        {

            string cookie_name = get_lable(match6.ToString(), "name");
            cookie_name = cookie_name.Trim();
            string cookie_value = "";
            try
            {
                cookie_value = my_b.k_cookie(cookie_name);
                cookie_value = my_b.tihuan(cookie_value, "fzw123", "&");
            }
            catch
            {

            }

            file_content = file_content.Replace(match6.ToString(), cookie_value);
        }
        




        Regex reg5 = new Regex(@"{fzw:listurl.*?}", RegexOptions.Singleline);
        MatchCollection matches5 = reg5.Matches(file_content);
        foreach (Match match5 in matches5)
        {

            string classid = get_lable(match5.ToString(), "classid");
            string m = get_lable(match5.ToString(), "m");

            string listurl = "";
            if (my_b.set_mode() == "伪静态")
            {
                listurl = "/list_" + classid + "_" + m + ConfigurationSettings.AppSettings["Suffix"].ToString();
            }
            else if (my_b.set_mode() == "静态网站")
            {
                try
                {
                    listurl = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + " and Model_id=" + m + "").Rows[0]["u7"].ToString();
                }
                catch
                {

                }

            }
            else
            {
                listurl = "/list.aspx?id=" + classid + "&Model_id=" + m + "";
            }
            file_content = file_content.Replace(match5.ToString(), listurl);
        }

        //系统内置控件
        Regex reg7 = new Regex(@"{fzw:server.*?}", RegexOptions.Singleline);
        MatchCollection matches7 = reg7.Matches(file_content);
        foreach (Match match7 in matches7)
        {
            string server_type = get_lable(match7.ToString(), "type");
            string server_id = get_lable(match7.ToString(), "id");
            string server_user = get_lable(match7.ToString(), "user");

            string server_value = "";
            if (server_type == "Editor")
            {
                #region 编辑器
                string server_width = get_lable(match7.ToString(), "width");
                string server_height = get_lable(match7.ToString(), "height");

                server_value = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/inc/editor.txt", Encoding.UTF8);
                server_value = server_value.Replace("{id}", server_id).Replace("{width}", server_width).Replace("{height}", server_height);
                my_b.Set_FreeTextBox(server_user);
                #endregion
            }
            else if (server_type == "upload")
            {
                #region 上传控件
                string extensions = get_lable(match7.ToString(), "extensions");
                string kuangao = get_lable(match7.ToString(), "kuangao");
                string towidth = "0";
                string toheight = "0";
                string crop = "false";
                if (kuangao != "")
                {
                    crop = "true";
                    towidth = kuangao.Substring(0, kuangao.IndexOf("*"));
                    toheight = kuangao.Substring(kuangao.IndexOf("*") + 1);
                    my_b.c_cookie(kuangao, "kuangao");
                }
                DataTable dt = new DataTable();
                dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");

                string mimeTypes = "";
                string anniu_str = "";
                if (extensions == "video")
                {
                    extensions = dt.Rows[0]["u7"].ToString();
                    mimeTypes = "video/*";
                    anniu_str = "点击选择视频";

                }
                else if (extensions == "mp3")
                {
                    extensions = dt.Rows[0]["u8"].ToString();
                    mimeTypes = "mp3/*";
                    anniu_str = "点击选择音频";
                }
                else if (extensions == "soft")
                {
                    extensions = dt.Rows[0]["u9"].ToString();
                    mimeTypes = "soft";
                    anniu_str = "点击选择文件";
                }
                else if (extensions == "other")
                {
                    extensions = dt.Rows[0]["u10"].ToString();
                    mimeTypes = "other/*";
                    anniu_str = "点击选择文件";
                }
                else
                {
                    extensions = dt.Rows[0]["u6"].ToString();
                    mimeTypes = "image/*";
                    anniu_str = "点击选择图片";
                }

                extensions = extensions.Replace("|", ",").Replace(".", "");
                server_value = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/inc/webuploader/temp.txt", Encoding.UTF8);
                server_value = server_value.Replace("<%=anniu_str%>", anniu_str).Replace("<%=typefile%>", extensions).Replace("<%=newTypeFile%>", mimeTypes).Replace("<%=id%>", server_id).Replace("<%=toheight%>", toheight).Replace("<%=towidth%>", towidth).Replace("<%=crop%>", crop);
                #endregion
            }

            file_content = file_content.Replace(match7.ToString(), server_value);

        }



        return file_content;
    }
    //设置默认标签结束
    #region 设置分页时的效果 start
    public string set_pagelist_all(string list1, int fenye_count, int page_list_size, int d_page, string file_content, string save_path)
    {
        string url_str = HttpContext.Current.Request.Url.ToString().ToLower();

        string web_mode = my_b.set_mode();
        string lan = get_lable(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}|{fzw:search:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), "lan");
        if (lan == "")
        {
            lan = "pagelist";
        }
        list1 = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/inc/temp/" + lan + ".txt", Encoding.UTF8);
        list1 = list1.Replace("{pagelist:pagecount}", fenye_count.ToString());
        if (fenye_count > page_list_size)
        {
            #region 当总页数大于页码数
            int stapage = 1;
            int overpage = page_list_size;
            if (d_page > page_list_size / 2)
            {
                stapage = d_page - (page_list_size / 2) - 1;
                if (d_page + page_list_size / 2 < fenye_count)
                {
                    overpage = d_page + page_list_size / 2;
                }
                else
                {
                    overpage = fenye_count;
                    stapage = fenye_count - (page_list_size - 1);
                }
                if (stapage < 1)
                {
                    stapage = 1;
                }

            }


            for (int j = stapage; j <= overpage; j++)
            {
                Regex reg1 = new Regex("{pagelist:linklist}.*?{/pagelist:linklist}", RegexOptions.Singleline);
                Match matches1 = reg1.Match(list1);
                string linklist = matches1.ToString();
                linklist = linklist.Replace("{pagelist:linklist}", "").Replace("{/pagelist:linklist}", "");
                if (j == d_page)
                {
                    Regex reg2 = new Regex("{pagelist:linkcurrent}.*?{/pagelist:linkcurrent}", RegexOptions.Singleline);
                    Match matches2 = reg2.Match(list1);
                    string linklist2 = matches2.ToString();
                    linklist2 = linklist2.Replace("{pagelist:linkcurrent}", "").Replace("{/pagelist:linkcurrent}", "");

                    linklist2 = linklist2.Replace("{pagelist:current_link}", "").Replace("{pagelist:current_title}", j.ToString());
                    if (matches2.Length > 0)
                    {
                        list1 = list1.Replace(matches2.ToString(), linklist2 + matches1.ToString());
                    }

                    // list1 = set_pagelist(file_content, ziduan_id, "current", "", j.ToString(), list1);
                    //  list1 = list1 + " <strong>" + j + "</strong>";

                }
                else
                {
                    #region 非当前页
                    Regex reg2 = new Regex("{pagelist:linkcurrent}.*?{/pagelist:linkcurrent}", RegexOptions.Singleline);
                    Match matches2 = reg2.Match(list1);

                    string linklist2 = matches2.ToString();
                    if (web_mode == "动态网站" || url_str.IndexOf("search.aspx") > -1 || url_str.IndexOf("list.aspx") > -1)
                    {
                        linklist = linklist.Replace("{pagelist:linklist_link}", save_path + "&page=" + j).Replace("{pagelist:linklist_title}", j.ToString());
                    }
                    else
                    {
                        linklist = linklist.Replace("{pagelist:linklist_link}", save_path + j + ConfigurationSettings.AppSettings["Suffix"].ToString()).Replace("{pagelist:linklist_title}", j.ToString());
                    }
                    if (matches1.Length > 0)
                    {
                        if (matches2.Length > 0)
                        {
                            list1 = list1.Replace(matches2.ToString(), "");
                            list1 = list1.Replace(matches1.ToString(), linklist + matches2.ToString() + matches1.ToString());
                        }
                        else
                        {
                            list1 = list1.Replace(matches1.ToString(), linklist + matches1.ToString());
                        }

                    }
                    #endregion
                }
                //循环结束



            }
            Regex reg3 = new Regex("{pagelist:linklist}.*{/pagelist:linklist}.*?{/pagelist:linklist}", RegexOptions.Singleline);
            list1 = reg3.Replace(list1, "");
            //HttpContext.Current.Response.Write(list1);
            //HttpContext.Current.Response.End();
            #endregion

        }
        else
        {
            #region 当总页数小于页码数
            for (int j = 1; j <= fenye_count; j++)
            {
                Regex reg1 = new Regex("{pagelist:linklist}.*?{/pagelist:linklist}", RegexOptions.Singleline);
                Match matches1 = reg1.Match(list1);
                string linklist = matches1.ToString();
                linklist = linklist.Replace("{pagelist:linklist}", "").Replace("{/pagelist:linklist}", "");
                if (j == d_page)
                {
                    Regex reg2 = new Regex("{pagelist:linkcurrent}.*?{/pagelist:linkcurrent}", RegexOptions.Singleline);
                    Match matches2 = reg2.Match(list1);
                    string linklist2 = matches2.ToString();
                    linklist2 = linklist2.Replace("{pagelist:linkcurrent}", "").Replace("{/pagelist:linkcurrent}", "");
                    linklist2 = linklist2.Replace("{pagelist:current_link}", "").Replace("{pagelist:current_title}", j.ToString());
                    if (matches1.Length > 0)
                    {
                        list1 = list1.Replace(matches1.ToString(), linklist2 + matches1.ToString());
                    }


                    // list1 = set_pagelist(file_content, ziduan_id, "current", "", j.ToString(), list1);
                    //  list1 = list1 + " <strong>" + j + "</strong>";
                }
                else
                {

                    if (j == 1)
                    {
                        linklist = linklist.Replace("{pagelist:linklist_link}", save_path).Replace("{pagelist:linklist_title}", j.ToString());
                        if (matches1.Length > 0)
                        {
                            list1 = list1.Replace(matches1.ToString(), linklist + matches1.ToString());
                        }


                        // list1 = set_pagelist(file_content, ziduan_id, "linklist", save_path, j.ToString(), list1);
                        //  list1 = list1 + "<A  href=\"" + save_path + "\">" + j + "</A>";
                    }
                    else
                    {

                        if (web_mode == "动态网站" || url_str.IndexOf("search.aspx") > -1 || url_str.IndexOf("list.aspx") > -1)
                        {
                            linklist = linklist.Replace("{pagelist:linklist_link}", save_path + "&page=" + j).Replace("{pagelist:linklist_title}", j.ToString());
                        }
                        else
                        {
                            linklist = linklist.Replace("{pagelist:linklist_link}", save_path + j + ConfigurationSettings.AppSettings["Suffix"].ToString()).Replace("{pagelist:linklist_title}", j.ToString());
                        }
                        if (matches1.Length > 0)
                        {
                            list1 = list1.Replace(matches1.ToString(), linklist + matches1.ToString());
                        }


                        //list1 = set_pagelist(file_content, ziduan_id, "linklist", save_path + j + ConfigurationSettings.AppSettings["Suffix"].ToString(), j.ToString(), list1);
                        // list1 = list1 + "<A  href=\"" + save_path + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                    }

                }
            }
            #endregion
        }
        if ((d_page - 1) == 0 || (d_page - 1) == 1)
        {
            list1 = set_pagelist(file_content, ziduan_id, "pgup", save_path, "", list1);
            list1 = set_pagelist(file_content, ziduan_id, "home", save_path, "", list1);
            // list1 = "<A  href=\"" + save_path + "\">上一页</A>" + list1;
            //list1 = "<A  href=\"" + save_path + "\">首页</A>" + list1;
        }
        else
        {
            int shangyiye = d_page - 1;
            if (web_mode == "动态网站" || url_str.IndexOf("search.aspx") > -1 || url_str.IndexOf("list.aspx") > -1)
            {
                list1 = set_pagelist(file_content, ziduan_id, "pgup", save_path + "&page=" + shangyiye.ToString(), "", list1);
            }
            else
            {
                list1 = set_pagelist(file_content, ziduan_id, "pgup", save_path + shangyiye.ToString() + ConfigurationSettings.AppSettings["Suffix"].ToString(), "", list1);
            }
            list1 = set_pagelist(file_content, ziduan_id, "home", save_path, "", list1);
            //  list1 = "<A  href=\"" + save_path + shangyiye.ToString() + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
            // list1 = "<A  href=\"" + save_path + "\">首页</A>" + list1;
        }

        if ((d_page) < fenye_count)
        {
            int xiayeye = d_page + 1;
            if (web_mode == "动态网站" || url_str.IndexOf("search.aspx") > -1 || url_str.IndexOf("list.aspx") > -1)
            {
                list1 = set_pagelist(file_content, ziduan_id, "pgdn", save_path + "&page=" + xiayeye.ToString(), "", list1);
                list1 = set_pagelist(file_content, ziduan_id, "last", save_path + "&page=" + fenye_count.ToString(), "", list1);
            }
            else
            {
                if (web_mode == "动态网站" || url_str.IndexOf("search.aspx") > -1 || url_str.IndexOf("list.aspx") > -1)
                {
                    list1 = set_pagelist(file_content, ziduan_id, "pgdn", save_path + "&page=" + xiayeye.ToString(), "", list1);
                    list1 = set_pagelist(file_content, ziduan_id, "last", save_path + "&page=" + fenye_count.ToString(), "", list1);
                }
                else
                {
                    list1 = set_pagelist(file_content, ziduan_id, "pgdn", save_path + xiayeye.ToString() + ConfigurationSettings.AppSettings["Suffix"].ToString(), "", list1);
                    list1 = set_pagelist(file_content, ziduan_id, "last", save_path + fenye_count.ToString() + ConfigurationSettings.AppSettings["Suffix"].ToString(), "", list1);
                }

            }

            //list1 = list1 + "<A  href=\"" + save_path + xiayeye.ToString() + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
            //list1 = list1 + "<A  href=\"" + save_path + fenye_count.ToString() + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
        }
        else
        {
            if (fenye_count == 1)
            {
                list1 = set_pagelist(file_content, ziduan_id, "pgdn", save_path, "", list1);
                list1 = set_pagelist(file_content, ziduan_id, "last", save_path, "", list1);
                //   list1 = list1 + "<A  href=\"" + save_path + "\">下一页</A>";
                // list1 = list1 + "<A  href=\"" + save_path + "\">尾页</A>";
            }
            else
            {
                if (web_mode == "动态网站" || url_str.IndexOf("search.aspx") > -1 || url_str.IndexOf("list.aspx") > -1)
                {
                    list1 = set_pagelist(file_content, ziduan_id, "pgdn", save_path + "&page=" + fenye_count, "", list1);
                    list1 = set_pagelist(file_content, ziduan_id, "last", save_path + "&page=" + fenye_count, "", list1);
                }
                else
                {
                    list1 = set_pagelist(file_content, ziduan_id, "pgdn", save_path + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString(), "", list1);
                    list1 = set_pagelist(file_content, ziduan_id, "last", save_path + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString(), "", list1);
                }

                // list1 = list1 + "<A  href=\"" + save_path + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                // list1 = list1 + "<A  href=\"" + save_path + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
            }
        }
        list1 = set_pagelist(file_content, ziduan_id, "size", "", list_size.ToString(), list1);
        list1 = set_pagelist(file_content, ziduan_id, "count", "", count_id.ToString(), list1);
        //HttpContext.Current.Response.Write(list1);
        //HttpContext.Current.Response.End();
        return list1;
    }
    public string set_pagelist(string file_content, string ziduan_id, string type, string a_link, string a_title, string list_str)
    {

        if (type == "current")
        {
            list_str = list_str.Replace("{pagelist:current_link}", a_link).Replace("{pagelist:current_title}", a_title);
        }
        if (type == "size")
        {
            list_str = list_str.Replace("{pagelist:size}", a_title);
        }
        if (type == "count")
        {
            list_str = list_str.Replace("{pagelist:count}", a_title);
        }
        if (type == "home")
        {
            list_str = list_str.Replace("{pagelist:home_link}", a_link);
        }
        if (type == "pgup")
        {
            list_str = list_str.Replace("{pagelist:pgup_link}", a_link);
        }
        if (type == "pgdn")
        {
            list_str = list_str.Replace("{pagelist:pgdn_link}", a_link);
        }
        //if (type == "linklist")
        //{
        //    Regex reg1 = new Regex("{pagelist:linklist}.*?{/pagelist:linklist}", RegexOptions.Singleline);
        //    Match matches1 = reg1.Match(list_str);
        //    string linklist = matches1.ToString();
        //    HttpContext.Current.Response.Write(linklist);
        //    HttpContext.Current.Response.End();
        //}
        if (type == "last")
        {
            list_str = list_str.Replace("{pagelist:last_link}", a_link);
            Regex reg1 = new Regex("{pagelist:linklist}.*?{/pagelist:linklist}", RegexOptions.Singleline);
            Match matches1 = reg1.Match(list_str);
            if (matches1.Length > 0)
            {
                list_str = list_str.Replace(matches1.ToString(), "");
            }


            Regex reg2 = new Regex("{pagelist:linkcurrent}.*?{/pagelist:linkcurrent}", RegexOptions.Singleline);
            Match matches2 = reg2.Match(list_str);
            if (matches2.Length > 0)
            {
                list_str = list_str.Replace(matches2.ToString(), "");
            }



        }

        return list_str;
    }
    #endregion
    //静态分页第二步
    public void page_list(string sql, string list_content, string file_content, string list_c)
    {
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + listid + ")");
        if (dt1.Rows.Count > 1)
        {
            dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + dt1.Rows[0]["sort_id"].ToString() + ")");
        }

        string save_path = dt1.Rows[0]["u7"].ToString();
        if (save_path.IndexOf("/") != 0)
        {
            save_path = "/" + save_path;
        }

        int sta = 1;
        string t2 = "";
        int d_page = 1;
        DataTable dt = my_c.GetTable(sql);
        //HttpContext.Current.Response.Write(sql);
        //HttpContext.Current.Response.End();
        count_id = dt.Rows.Count;
        int fenye_count = count_id / list_size;
        float fenye_count1 = (float)count_id / (float)list_size;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t1 = list_content;
                Regex reg1 = new Regex("{fzw:" + ziduan_id + @":[\s\S]*?}", RegexOptions.Singleline);
                MatchCollection matches1 = reg1.Matches(list_content);
                foreach (Match match1 in matches1)
                {

                    string t3 = "";

                    string[] aa = Regex.Split(match1.ToString(), " ");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string ziduan = "";

                        if (j == 0)
                        {
                            ziduan = aa[j].ToString().Replace("{fzw:" + ziduan_id + ":", "");
                            try
                            {
                                ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                            }
                            catch
                            {

                                ziduan = ziduan.Replace("/}", "");
                            }

                            t3 = my_b.set_htmldecode(dt.Rows[i][ziduan.Trim()].ToString());
                        }
                        else
                        {
                            if (aa[j].ToString().Replace("/}", "").Trim() != "")
                            {
                                t3 = set_other(aa[j].ToString(), t3);
                            }
                        }


                    }
                    t1 = t1.Replace(match1.ToString(), t3);

                }
                t1 = set_pageurl(t1, dt.Rows[i]["id"].ToString(), dt.Rows[i]["Filepath"].ToString(), dt.Rows[i]["classid"].ToString());
                try
                {
                    t1 = set_xuliehao(t1, i + 1);
                }
                catch
                { }
                t2 = t2 + t1;



                if (sta % list_size == 0 || i + 1 == dt.Rows.Count)
                {

                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + save_path);

                    string html_content = file_content.Replace(list_c, t2);
                    string list1 = "";
                    int page_list_size = 8;
                    try
                    {
                        page_list_size = int.Parse(get_lable(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), "size"));
                    }
                    catch
                    { }

                    //设置pagelist
                    list1 = set_pagelist_all(list1, fenye_count, page_list_size, d_page, file_content, save_path);
                    //pagelist end
                    // list1 = "<strong> " + list_size + " 条/页 共 " + count_id.ToString() + " 条记录</strong>" + list1;
                    try
                    {
                        html_content = html_content.Replace(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), list1);
                    }
                    catch
                    {
                        //HttpContext.Current.Response.Write("{fzw:list:page id=\"" + ziduan_id + "\".*?/}");
                        //HttpContext.Current.Response.End();
                    }

                    Regex reg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(file_content);
                    foreach (Match match in matches)
                    {
                        string[] aa = Regex.Split(match.ToString(), " ");
                        string t3 = "";
                        for (int j = 0; j < aa.Length; j++)
                        {
                            string ziduan = "";

                            if (j == 0)
                            {
                                ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                try
                                {
                                    ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                }
                                catch
                                {

                                    ziduan = ziduan.Replace("/}", "");
                                }

                                t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                            }
                            else
                            {
                                if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                {
                                    t3 = set_other(aa[j].ToString(), t3);
                                }
                            }
                        }
                        html_content = html_content.Replace(match.ToString(), t3);
                    }

                    html_content = get_fzw_html(html_content);
                    html_content = set_url(html_content);
                    if (d_page == 1)
                    {
                        File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + save_path + "/index.html", html_content, Encoding.UTF8);
                    }
                    else
                    {
                        File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + save_path + "/" + d_page + ".html", html_content, Encoding.UTF8);
                    }

                    t2 = "";
                    d_page = d_page + 1;

                }


                sta = sta + 1;
            }
        }
        else
        {
            string html_content = "";
            //没有循环时的内容
            Regex reg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(file_content);
            foreach (Match match in matches)
            {
                string[] aa = Regex.Split(match.ToString(), " ");
                string t3 = "";
                for (int j = 0; j < aa.Length; j++)
                {
                    string ziduan = "";

                    if (j == 0)
                    {
                        ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                        try
                        {
                            ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                        }
                        catch
                        {

                            ziduan = ziduan.Replace("/}", "");
                        }

                        t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                    }
                    else
                    {
                        if (aa[j].ToString().Replace("/}", "").Trim() != "")
                        {
                            t3 = set_other(aa[j].ToString(), t3);
                        }
                    }
                }
                file_content = file_content.Replace(match.ToString(), t3);
            }
            Regex reg1 = new Regex(@"{fzw:list.*?}.*?{/fzw:list.*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(file_content);
            foreach (Match match in matches1)
            {
                file_content = file_content.Replace(match.ToString(), "");
            }
            Regex reg2 = new Regex("{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline);
            MatchCollection matches2 = reg2.Matches(file_content);
            foreach (Match match in matches2)
            {
                file_content = file_content.Replace(match.ToString(), "");
            }
            file_content = get_fzw_html(file_content);
            file_content = set_url(file_content);
            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + save_path);
            if (d_page == 1)
            {
                File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + save_path + "/index.html", file_content, Encoding.UTF8);
            }
            else
            {
                File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + save_path + "/" + d_page + ".html", file_content, Encoding.UTF8);
            }

            t2 = "";
            d_page = d_page + 1;
        }
        //没有循环时结束



    }
    ////静态分页第二步结束

    //静态分页第三步获取SQL
    public string get_list_sql(string g1, string n_id, string table_name)
    {
        listid = n_id;
        Regex reg = new Regex(@"{fzw:list[\s\S ]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {


            string t1 = match.ToString().Replace("  ", " ");
            string ziduan = "";
            string orderby = "";
            string classid = "";
            string top_string = "";
            string wherestring = "";
            string type_Fields = "";
            string type = "";
            string order_by = "";
            string sqlvalue = "";
            string[] aa = Regex.Split(t1, " ");
            for (int j = 0; j < aa.Length; j++)
            {
                if (aa[j].ToString().IndexOf("number") > -1)
                {
                    top_string = aa[j].ToString().Replace("number=", "");
                    top_string = top_string.Replace("\"", "").Replace("}", "");
                    if (top_string == "" || top_string == "0")
                    {
                        list_size = 15;
                    }
                    else
                    {
                        list_size = int.Parse(top_string);
                    }
                }
                else if (aa[j].ToString().IndexOf("orderby=") > -1)
                {
                    order_by = aa[j].ToString().Replace("orderby=", "");
                    order_by = order_by.Replace("\"", "").Replace("}", "");

                }
                else if (aa[j].ToString().IndexOf("order") > -1)
                {

                    orderby = aa[j].ToString().Replace("order=", "");
                    orderby = orderby.Replace("\"", "").Replace("}", "");


                }
                else if (aa[j].ToString().IndexOf("type_Fields=") > -1)
                {
                    type_Fields = aa[j].ToString().Replace("type_Fields=", "");
                    type_Fields = type_Fields.Replace("\"", "").Replace("}", "");
                }
                else if (aa[j].ToString().IndexOf("type=") > -1)
                {
                    type = aa[j].ToString().Replace("type=", "");
                    type = type.Replace("\"", "").Replace("}", "");

                }
                else if (aa[j].ToString().IndexOf("sqlvalue=") > -1)
                {

                    sqlvalue = aa[j].ToString().Replace("sqlvalue=", "");
                    sqlvalue = sqlvalue.Replace("\"", "").Replace("}", "");
                    sqlvalue = sqlvalue.Replace("&nbsp;", " ");

                }
            }
            string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
            id = id.Substring(0, id.IndexOf("\""));
            ziduan_id = id;
            Regex reg1 = new Regex("{fzw:" + id + @"[\s\S]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(g1);
            string match_string = "";
            foreach (Match match1 in matches1)
            {


                string ziduan1 = "";

                if (ziduan == "")
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }

                    ziduan = ziduan1;
                }
                else
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    //  HttpContext.Current.Response.Write(match_string + "_____" + match_string.IndexOf(match1.ToString()) + "__________" + match1.ToString() + "<br><br><br>");
                    if (match_string.IndexOf(match1.ToString()) == -1)
                    {
                        if (ziduan1 != "id")
                        {
                            string ziduan_str = "";
                            for (int i = 0; i < ziduan.Split(',').Length; i++)
                            {
                                if (ziduan.Split(',')[i].ToString() == ziduan1)
                                {
                                    ziduan_str = "yes";
                                }
                            }
                            if (ziduan_str == "")
                            {
                                ziduan = ziduan + "," + ziduan1;
                            }

                        }
                        else
                        {
                            ziduan = ziduan + "," + ziduan1;
                        }
                    }

                }
                match_string = match1.ToString() + match_string;


            }
            //HttpContext.Current.Response.Write(ziduan);
            // HttpContext.Current.Response.End();
            if (my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + table_name + "'") == "文章模型" || my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + table_name + "'") == "新闻模型")
            {
                //设置pageurl
                if (ziduan.IndexOf("Filepath,") == -1)
                {
                    if (ziduan.IndexOf(",Filepath") == -1)
                    {
                        ziduan = ziduan + ",Filepath";
                    }
                }

                ziduan = ziduan.Replace("classid", "lanmuhao");
                if (ziduan.IndexOf("id,") == -1)
                {
                    if (ziduan.IndexOf(",id") == -1)
                    {
                        ziduan = ziduan + ",id";
                    }
                }
                ziduan = ziduan.Replace("lanmuhao", "classid");
                if (ziduan.IndexOf("classid,") == -1)
                {
                    if (ziduan.IndexOf(",classid") == -1)
                    {
                        ziduan = ziduan + ",classid";
                    }
                }
                //end


            }

            if (type_Fields != "")
            {
                wherestring = "where classid in (" + n_id + ") and " + type_Fields + " like '%" + type + "%'";
            }
            else
            {
                wherestring = "where classid in (" + n_id + ")";
            }
            if (orderby != "id")
            {
                orderby = "order by " + orderby + " " + order_by + " , id desc";
            }
            else
            {
                orderby = "order by id desc";
            }
            if (sqlvalue != "")
            {
                if (wherestring == "")
                {
                    wherestring = " where " + sqlvalue;
                }
                else
                {
                    wherestring = wherestring + " and " + sqlvalue;
                }
            }
            string sql = "select table_ziduan from table_name table_where table_order ";

            sql = sql.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where", wherestring).Replace("table_order", orderby);

            return sql;

        }

        return "";
    }
    //静态分页第三步获取SQL结束

    //静态分页第一步
    public void set_list(string table_name, string n_id)
    {
        string file_content = my_b.get_value("u5", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id in (" + n_id + ")");
        file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + file_content), Encoding.UTF8);
        file_content = set_sitekey(file_content);

        Regex reg = new Regex(@"{fzw:list.*?}.*?{/fzw:list.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            string sql = get_list_sql(match.ToString(), n_id, table_name);

            string list_content = "";
            list_content = Regex.Replace(match.ToString(), @"{fzw:list.*?}", "", RegexOptions.Singleline).ToString();
            list_content = Regex.Replace(list_content.ToString(), @"{/fzw:list}", "", RegexOptions.Singleline).ToString();
            page_list(sql, list_content, file_content, match.ToString());
        }

    }
    //静态分页第一步结束

    //主目录
    public void set_zhu_list(string table_name, string n_id, string classid)
    {
        string file_content = my_b.get_value("u5", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id in (" + classid + ")");

        file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + file_content), Encoding.UTF8);
        file_content = set_sitekey(file_content);
        //ajax处理开始
        Regex ajax_reg = new Regex(@"{fzw:ajax:.*?/}", RegexOptions.Singleline);
        MatchCollection ajax_matches = ajax_reg.Matches(file_content);
        foreach (Match ajax_match in ajax_matches)
        {

        }
        file_content = set_ajax(file_content);
        //ajax处理完成
        Regex reg = new Regex(@"{fzw:list.*?}.*?{/fzw:list.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            string sql = get_list_sql(match.ToString(), n_id, table_name);

            string list_content = "";
            list_content = Regex.Replace(match.ToString(), @"{fzw:list.*?}", "", RegexOptions.Singleline).ToString();
            list_content = Regex.Replace(list_content.ToString(), @"{/fzw:list}", "", RegexOptions.Singleline).ToString();
            page_list(sql, list_content, file_content, match.ToString());
        }

    }








    //动态分页第二步
    public void page_w_list(string sql, string sql2, string list_content, string file_content, string list_c, int page)
    {
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + HttpContext.Current.Request.QueryString["id"].ToString() + ")");

        if (dt1.Rows.Count > 1)
        {
            dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + dt1.Rows[0]["sort_id"].ToString() + ")");
        }

        string save_path = "/list.aspx?id=" + HttpContext.Current.Request.QueryString["id"].ToString() + "&Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "";
        try
        {
            save_path = "/list.aspx?id=" + HttpContext.Current.Request.QueryString["id"].ToString() + "&Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&number=" + HttpContext.Current.Request.QueryString["number"].ToString() + "";
        }
        catch { }
        int sta = 1;
        string t2 = "";
        int d_page = page;
        if (page == 0)
        {
            d_page = 1;
        }
        int top2 = list_size * (page - 1);
        sql = sql.Replace("top1", list_size.ToString());
        sql = sql.Replace("top2", top2.ToString());
        // HttpContext.Current.Response.Write(sql);

        DataTable dt = my_c.GetTable(sql);
        //HttpContext.Current.Response.Write(sql);
        //HttpContext.Current.Response.End();
        count_id = int.Parse(my_c.GetTable(sql2).Rows[0]["count_id"].ToString());
        int fenye_count = count_id / list_size;
        float fenye_count1 = (float)count_id / (float)list_size;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }

        //如果没有记录时
        if (dt.Rows.Count > 0)
        {
            #region 循环开始
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t1 = list_content;
                Regex reg1 = new Regex("{fzw:" + ziduan_id + @":[\s\S]*?}", RegexOptions.Singleline);
                MatchCollection matches1 = reg1.Matches(list_content);
                foreach (Match match1 in matches1)
                {

                    string t3 = "";

                    string[] aa = Regex.Split(match1.ToString(), " ");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string ziduan = "";

                        if (j == 0)
                        {
                            ziduan = aa[j].ToString().Replace("{fzw:" + ziduan_id + ":", "");
                            try
                            {
                                ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                            }
                            catch
                            {

                                ziduan = ziduan.Replace("/}", "");
                            }

                            t3 = my_b.set_htmldecode(dt.Rows[i][ziduan.Trim()].ToString());
                        }
                        else
                        {
                            if (aa[j].ToString().Replace("/}", "").Trim() != "")
                            {
                                t3 = set_other(aa[j].ToString(), t3);
                            }
                        }


                    }
                    t1 = t1.Replace(match1.ToString(), t3);

                }
                try
                {
                    t1 = set_pageurl(t1, dt.Rows[i]["id"].ToString(), dt.Rows[i]["Filepath"].ToString(), dt.Rows[i]["classid"].ToString());
                }
                catch { }
                try
                {
                    t1 = set_xuliehao(t1, i + 1);
                }
                catch
                { }
                t2 = t2 + t1;

                if (sta % list_size == 0 || i + 1 == dt.Rows.Count)
                {
                    #region 最后一条
                    string html_content = file_content.Replace(list_c, t2);
                    string list1 = "";
                    int page_list_size = 8;
                    try
                    {
                        page_list_size = int.Parse(get_lable(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), "size"));
                    }
                    catch
                    { }

                    //设置pagelist
                    list1 = set_pagelist_all(list1, fenye_count, page_list_size, d_page, file_content, save_path);
                    //pagelist end

                    // list1 = "<strong> " + list_size + " 条/页 共 " + count_id.ToString() + " 条记录</strong>" + list1;
                    try
                    {
                        html_content = html_content.Replace(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), list1);
                    }
                    catch
                    {

                    }

                    Regex reg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(file_content);
                    foreach (Match match in matches)
                    {
                        string[] aa = Regex.Split(match.ToString(), " ");
                        string t3 = "";
                        for (int j = 0; j < aa.Length; j++)
                        {
                            string ziduan = "";

                            if (j == 0)
                            {
                                ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                try
                                {
                                    ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                }
                                catch
                                {

                                    ziduan = ziduan.Replace("/}", "");
                                }

                                t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                            }
                            else
                            {
                                if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                {
                                    t3 = set_other(aa[j].ToString(), t3);
                                }
                            }
                        }
                        html_content = html_content.Replace(match.ToString(), t3);
                    }

                    html_content = get_fzw_html(html_content);
                    html_content = set_url(html_content);

                    HttpContext.Current.Response.Write(html_content);
                    HttpContext.Current.Response.End();
                    #endregion
                }


                sta = sta + 1;
            }
            #endregion 循环结束
        }
        else
        {
            #region 没有循环时的内容
            Regex reg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(file_content);
            foreach (Match match in matches)
            {
                string[] aa = Regex.Split(match.ToString(), " ");
                string t3 = "";
                for (int j = 0; j < aa.Length; j++)
                {
                    string ziduan = "";

                    if (j == 0)
                    {
                        ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                        try
                        {
                            ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                        }
                        catch
                        {

                            ziduan = ziduan.Replace("/}", "");
                        }

                        t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                    }
                    else
                    {
                        if (aa[j].ToString().Replace("/}", "").Trim() != "")
                        {
                            t3 = set_other(aa[j].ToString(), t3);
                        }
                    }
                }
                file_content = file_content.Replace(match.ToString(), t3);
            }
            Regex reg1 = new Regex(@"{fzw:list.*?}.*?{/fzw:list.*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(file_content);
            foreach (Match match in matches1)
            {
                file_content = file_content.Replace(match.ToString(), "");
            }
            Regex reg2 = new Regex("{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline);
            MatchCollection matches2 = reg2.Matches(file_content);
            foreach (Match match in matches2)
            {
                file_content = file_content.Replace(match.ToString(), "");
            }
            file_content = get_fzw_html(file_content);
            file_content = set_url(file_content);

            HttpContext.Current.Response.Write(file_content);
            #endregion
        }


    }
    ////动态分页第二步结束

    //动态分页第三步获取SQL
    string d_sql1 = "";
    string d_sql2 = "";
    public void get_w_list_sql(string g1, string n_id, string table_name, int page)
    {

        listid = n_id;
        Regex reg = new Regex(@"{fzw:list[\s\S ]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {


            string t1 = match.ToString().Replace("  ", " ");
            string ziduan = "";
            string orderby = "";
            string classid = "";
            string top_string = "";
            string wherestring = "";
            string type_Fields = "";
            string type = "";
            string order_by = "";
            string sqlvalue = "";
            string[] aa = Regex.Split(t1, " ");
            for (int j = 0; j < aa.Length; j++)
            {
                if (aa[j].ToString().IndexOf("number") > -1)
                {
                    top_string = aa[j].ToString().Replace("number=", "");
                    top_string = top_string.Replace("\"", "").Replace("}", "");
                    if (top_string == "" || top_string == "0")
                    {
                        list_size = 15;
                    }
                    else
                    {
                        list_size = int.Parse(top_string);
                    }
                    try
                    {
                        list_size = int.Parse(HttpContext.Current.Request.QueryString["number"].ToString());
                    }
                    catch { }
                }
                else if (aa[j].ToString().IndexOf("orderby=") > -1)
                {

                    order_by = aa[j].ToString().Replace("orderby=", "");
                    order_by = order_by.Replace("\"", "").Replace("}", "");

                }
                else if (aa[j].ToString().IndexOf("order") > -1)
                {

                    orderby = aa[j].ToString().Replace("order=", "");
                    orderby = orderby.Replace("\"", "").Replace("}", "");
                    orderby = orderby;


                }
                else if (aa[j].ToString().IndexOf("type_Fields=") > -1)
                {
                    type_Fields = aa[j].ToString().Replace("type_Fields=", "");
                    type_Fields = type_Fields.Replace("\"", "").Replace("}", "");
                }
                else if (aa[j].ToString().IndexOf("type=") > -1)
                {
                    type = aa[j].ToString().Replace("type=", "");
                    type = type.Replace("\"", "").Replace("}", "");

                }
                else if (aa[j].ToString().IndexOf("sqlvalue=") > -1)
                {

                    sqlvalue = aa[j].ToString().Replace("sqlvalue=", "");
                    sqlvalue = sqlvalue.Replace("\"", "").Replace("}", "");
                    sqlvalue = sqlvalue.Replace("&nbsp;", " ");

                }
            }

            string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
            id = id.Substring(0, id.IndexOf("\""));
            ziduan_id = id;
            Regex reg1 = new Regex("{fzw:" + id + @"[\s\S]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(g1);
            foreach (Match match1 in matches1)
            {

                string ziduan1 = "";

                if (ziduan == "")
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    ziduan = ziduan1;
                }
                else
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }

                    string ziduan_str = "";
                    for (int i = 0; i < ziduan.Split(',').Length; i++)
                    {
                        if (ziduan.Split(',')[i].ToString() == ziduan1)
                        {
                            ziduan_str = "yes";
                        }
                    }
                    if (ziduan_str == "")
                    {
                        ziduan = ziduan + "," + ziduan1;
                    }
                }



            }
            string wherestring1 = "";
            if (type_Fields != "")
            {
                wherestring = "where classid in (" + n_id + ") and " + type_Fields + " like '%" + type + "%'";
                wherestring1 = " classid in (" + n_id + ") and " + type_Fields + " like '%" + type + "%'";
            }
            else
            {
                wherestring = "where classid in (" + n_id + ")";
                wherestring1 = " classid in (" + n_id + ")";
            }
            if (sqlvalue != "")
            {
                wherestring = wherestring + " and " + sqlvalue;
                wherestring1 = wherestring1 + " and " + sqlvalue;
            }

            if (orderby != "dtime")
            {
                orderby = "order by " + orderby + " " + order_by + " , dtime desc";
            }
            else
            {
                orderby = "order by dtime desc";
            }
            //HttpContext.Current.Response.Write(orderby);
            //HttpContext.Current.Response.End();
            //设置pageurl
            if (ziduan.IndexOf("Filepath,") == -1)
            {
                if (ziduan.IndexOf(",Filepath") == -1)
                {
                    ziduan = ziduan + ",Filepath";
                }
            }

            ziduan = ziduan.Replace("classid", "lanmuhao");
            if (ziduan.IndexOf("id,") == -1)
            {
                if (ziduan.IndexOf(",id") == -1)
                {
                    ziduan = ziduan + ",id";
                }
            }
            ziduan = ziduan.Replace("lanmuhao", "classid");
            if (ziduan.IndexOf("classid,") == -1)
            {
                if (ziduan.IndexOf(",classid") == -1)
                {
                    ziduan = ziduan + ",classid";
                }
            }
            //end
            if (page == 0)
            {
                d_sql1 = "SELECT TOP top1 table_ziduan FROM table_name WHERE  table_where1 table_order";
                d_sql1 = d_sql1.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where1", wherestring1).Replace("table_order", orderby);
            }
            else
            {
                d_sql1 = "SELECT TOP top1 table_ziduan FROM table_name WHERE id not in (SELECT TOP top2 id FROM table_name where table_where1 table_order)  and  table_where1 table_order";
                d_sql1 = d_sql1.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where1", wherestring1).Replace("table_order", orderby);
            }

            d_sql2 = "select count(id) as count_id from table_name table_where";
            d_sql2 = d_sql2.Replace("table_name", table_name).Replace("table_where", wherestring);


        }


    }
    //动态分页第三步获取SQL结束

    //动态分页第一步
    public void set_w_list(string table_name, string n_id, int page)
    {

        string file_content = "";
        string m = "";
        try
        {
            m = HttpContext.Current.Request.QueryString["m"].ToString();
        }
        catch { }
        if (m == "")
        {
            file_content = my_b.get_value("u5", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id in (" + HttpContext.Current.Request.QueryString["id"].ToString() + ")");
        }
        else
        {
            file_content = m;
        }

        file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + file_content), Encoding.UTF8);
        file_content = set_sitekey(file_content);

        Regex reg = new Regex(@"{fzw:list.*?}.*?{/fzw:list.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        if (matches.Count > 0)
        {
            foreach (Match match in matches)
            {
                get_w_list_sql(match.ToString(), n_id, table_name, page);
                string list_content = "";
                list_content = Regex.Replace(match.ToString(), @"{fzw:list.*?}", "", RegexOptions.Singleline).ToString();
                list_content = Regex.Replace(list_content.ToString(), @"{/fzw:list}", "", RegexOptions.Singleline).ToString();

                page_w_list(d_sql1, d_sql2, list_content, file_content, match.ToString(), page);
            }
        }
        else
        {
            DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + HttpContext.Current.Request.QueryString["id"].ToString() + ")");
            //在没有list的list时
            Regex listreg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
            MatchCollection matches1 = listreg.Matches(file_content);
            foreach (Match match in matches1)
            {
                string[] aa = Regex.Split(match.ToString(), " ");
                string t3 = "";
                for (int j = 0; j < aa.Length; j++)
                {
                    string ziduan = "";

                    if (j == 0)
                    {
                        ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                        try
                        {
                            ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                        }
                        catch
                        {

                            ziduan = ziduan.Replace("/}", "");
                        }

                        t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                    }
                    else
                    {
                        if (aa[j].ToString().Replace("/}", "").Trim() != "")
                        {
                            t3 = set_other(aa[j].ToString(), t3);
                        }
                    }
                }
                file_content = file_content.Replace(match.ToString(), t3);
            }

            file_content = get_fzw_html(file_content);
            file_content = set_url(file_content);

            HttpContext.Current.Response.Write(file_content);
        }



    }
    //动态分页第一步结束
    //动态分页---------英文
    public void page_e_w_list(string sql, string sql2, string list_content, string file_content, string list_c, int page)
    {
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + listid + ")");

        if (dt1.Rows.Count > 1)
        {
            dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + dt1.Rows[0]["sort_id"].ToString() + ")");
        }

        string save_path = "/list.aspx?id=" + HttpContext.Current.Request.QueryString["id"].ToString() + "&Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "";

        int sta = 1;
        string t2 = "";
        int d_page = page;
        if (page == 0)
        {
            d_page = 1;
        }
        int top2 = list_size * (page - 1);
        sql = sql.Replace("top1", list_size.ToString());
        sql = sql.Replace("top2", top2.ToString());
        // HttpContext.Current.Response.Write(sql);

        DataTable dt = my_c.GetTable(sql);
        file_content = file_content.Replace("{fzw:pagecount/}", dt.Rows.Count.ToString());
        //HttpContext.Current.Response.Write(sql);
        //HttpContext.Current.Response.End();
        count_id = int.Parse(my_c.GetTable(sql2).Rows[0]["count_id"].ToString());
        int fenye_count = count_id / list_size;
        float fenye_count1 = (float)count_id / (float)list_size;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string t1 = list_content;
            Regex reg1 = new Regex("{fzw:" + ziduan_id + @":[\s\S]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(list_content);
            foreach (Match match1 in matches1)
            {

                string t3 = "";

                string[] aa = Regex.Split(match1.ToString(), " ");
                for (int j = 0; j < aa.Length; j++)
                {
                    string ziduan = "";

                    if (j == 0)
                    {
                        ziduan = aa[j].ToString().Replace("{fzw:" + ziduan_id + ":", "");
                        try
                        {
                            ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                        }
                        catch
                        {

                            ziduan = ziduan.Replace("/}", "");
                        }

                        t3 = my_b.set_htmldecode(dt.Rows[i][ziduan.Trim()].ToString());
                    }
                    else
                    {
                        if (aa[j].ToString().Replace("/}", "").Trim() != "")
                        {
                            t3 = set_other(aa[j].ToString(), t3);
                        }
                    }


                }
                t1 = t1.Replace(match1.ToString(), t3);

            }
            t1 = set_pageurl(t1, dt.Rows[i]["id"].ToString(), dt.Rows[i]["Filepath"].ToString(), dt.Rows[i]["classid"].ToString());
            try
            {
                t1 = set_xuliehao(t1, i + 1);
            }
            catch
            { }
            t2 = t2 + t1;

            if (sta % list_size == 0 || i + 1 == dt.Rows.Count)
            {

                string html_content = file_content.Replace(list_c, t2);
                string list1 = "";
                int page_list_size = 8;
                try
                {
                    page_list_size = int.Parse(get_lable(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), "size"));
                }
                catch
                { }

                if (fenye_count > page_list_size)
                {

                    int stapage = 1;
                    int overpage = page_list_size;
                    if (d_page > page_list_size / 2)
                    {
                        stapage = d_page - (page_list_size / 2) - 1;
                        if (d_page + page_list_size / 2 < fenye_count)
                        {
                            overpage = d_page + page_list_size / 2;
                        }
                        else
                        {
                            overpage = fenye_count;
                            stapage = fenye_count - (page_list_size - 1);
                        }

                    }


                    for (int j = stapage; j <= overpage; j++)
                    {
                        if (j == d_page)
                        {
                            list1 = list1 + " <strong>" + j + "</strong>";
                        }
                        else
                        {
                            if (j == 1)
                            {
                                list1 = list1 + "<A  href=\"" + save_path + "&lan=en\">" + j + "</A>";
                            }
                            else
                            {
                                list1 = list1 + "<A  href=\"" + save_path + "&page=" + j + "&lan=en\">" + j + "</A>";
                            }
                        }

                    }
                }
                else
                {
                    for (int j = 1; j <= fenye_count; j++)
                    {
                        if (j == d_page)
                        {
                            list1 = list1 + " <strong>" + j + "</strong>";
                        }
                        else
                        {
                            if (j == 1)
                            {
                                list1 = list1 + "<A  href=\"" + save_path + "&lan=en\">" + j + "</A>";
                            }
                            else
                            {
                                list1 = list1 + "<A  href=\"" + save_path + "&page=" + j + "&lan=en\">" + j + "</A>";
                            }

                        }
                    }

                }
                if ((d_page - 1) == 0 || (d_page - 1) == 1)
                {
                    list1 = "<A  href=\"" + save_path + "&lan=en\">Previous</A>" + list1;
                    list1 = "<A  href=\"" + save_path + "&lan=en\">Home</A>" + list1;
                }
                else
                {
                    int shangyiye = d_page - 1;
                    list1 = "<A  href=\"" + save_path + "&page=" + shangyiye.ToString() + "&lan=en\">Previous</A>" + list1;
                    list1 = "<A  href=\"" + save_path + "&lan=en\">Home</A>" + list1;
                }

                if ((d_page) < fenye_count)
                {
                    int xiayeye = d_page + 1;
                    list1 = list1 + "<A  href=\"" + save_path + "&page=" + xiayeye.ToString() + "&lan=en\">Next</A>";
                    list1 = list1 + "<A  href=\"" + save_path + "&page=" + fenye_count.ToString() + "&lan=en\">Last</A>";
                }
                else
                {
                    if (fenye_count == 1)
                    {
                        list1 = list1 + "<A  href=\"" + save_path + "&lan=en\">Next</A>";
                        list1 = list1 + "<A  href=\"" + save_path + "&lan=en\">Last</A>";
                    }
                    else
                    {
                        list1 = list1 + "<A  href=\"" + save_path + "&page=" + fenye_count + "&lan=en\">Next</A>";
                        list1 = list1 + "<A  href=\"" + save_path + "&page" + fenye_count + "&lan=en\">Last</A>";
                    }
                }

                list1 = "<strong> " + list_size + " Record / page Total " + count_id.ToString() + " Records</strong>" + list1;
                try
                {
                    html_content = html_content.Replace(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), list1);
                }
                catch
                {

                }

                Regex reg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
                MatchCollection matches = reg.Matches(file_content);
                foreach (Match match in matches)
                {
                    string[] aa = Regex.Split(match.ToString(), " ");
                    string t3 = "";
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string ziduan = "";

                        if (j == 0)
                        {
                            ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                            try
                            {
                                ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                            }
                            catch
                            {

                                ziduan = ziduan.Replace("/}", "");
                            }

                            t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                        }
                        else
                        {
                            if (aa[j].ToString().Replace("/}", "").Trim() != "")
                            {
                                t3 = set_other(aa[j].ToString(), t3);
                            }
                        }
                    }
                    html_content = html_content.Replace(match.ToString(), t3);
                }

                html_content = get_fzw_html(html_content);
                html_content = set_url(html_content);

                HttpContext.Current.Response.Write(html_content);

            }


            sta = sta + 1;
        }


    }
    ////动态分页第二步结束

    //动态分页第三步获取SQL

    public void get_e_w_list_sql(string g1, string n_id, string table_name, int page)
    {
        listid = n_id;
        Regex reg = new Regex(@"{fzw:list[\s\S ]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {


            string t1 = match.ToString().Replace("  ", " ");
            string ziduan = "";
            string orderby = "";
            string classid = "";
            string top_string = "";
            string wherestring = "";
            string type_Fields = "";
            string type = "";
            string order_by = "";
            string[] aa = Regex.Split(t1, " ");
            for (int j = 0; j < aa.Length; j++)
            {
                if (aa[j].ToString().IndexOf("number") > -1)
                {
                    top_string = aa[j].ToString().Replace("number=", "");
                    top_string = top_string.Replace("\"", "").Replace("}", "");
                    if (top_string == "" || top_string == "0")
                    {
                        list_size = 15;
                    }
                    else
                    {
                        list_size = int.Parse(top_string);
                    }
                }
                else if (aa[j].ToString().IndexOf("orderby=") > -1)
                {
                    order_by = aa[j].ToString().Replace("orderby=", "");
                    order_by = order_by.Replace("\"", "").Replace("}", "");

                }
                else if (aa[j].ToString().IndexOf("order") > -1)
                {

                    orderby = aa[j].ToString().Replace("order=", "");
                    orderby = orderby.Replace("\"", "").Replace("}", "");
                 //   orderby = "order by " + orderby + "  ";
                    // orderby = "order by id ";

                }
                else if (aa[j].ToString().IndexOf("type_Fields=") > -1)
                {
                    type_Fields = aa[j].ToString().Replace("type_Fields=", "");
                    type_Fields = type_Fields.Replace("\"", "").Replace("}", "");
                }
                else if (aa[j].ToString().IndexOf("type=") > -1)
                {
                    type = aa[j].ToString().Replace("type=", "");
                    type = type.Replace("\"", "").Replace("}", "");

                }
            }
            string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
            id = id.Substring(0, id.IndexOf("\""));
            ziduan_id = id;
            Regex reg1 = new Regex("{fzw:" + id + @"[\s\S]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(g1);
            foreach (Match match1 in matches1)
            {

                string ziduan1 = "";

                if (ziduan == "")
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    ziduan = ziduan1;
                }
                else
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    ziduan = ziduan + "," + ziduan1;
                }



            }
            string wherestring1 = "";
            if (type_Fields != "")
            {
                wherestring = "where classid in (" + n_id + ") and " + type_Fields + " like '%" + type + "%'";
                wherestring1 = " classid in (" + n_id + ") and " + type_Fields + " like '%" + type + "%'";
            }
            else
            {
                wherestring = "where classid in (" + n_id + ")";
                wherestring1 = " classid in (" + n_id + ")";
            }
            orderby = orderby + order_by;
            //设置pageurl
            if (ziduan.IndexOf("Filepath,") == -1)
            {
                if (ziduan.IndexOf(",Filepath") == -1)
                {
                    ziduan = ziduan + ",Filepath";
                }
            }

            ziduan = ziduan.Replace("classid", "lanmuhao");
            if (ziduan.IndexOf("id,") == -1)
            {
                if (ziduan.IndexOf(",id") == -1)
                {
                    ziduan = ziduan + ",id";
                }
            }
            ziduan = ziduan.Replace("lanmuhao", "classid");
            if (ziduan.IndexOf("classid,") == -1)
            {
                if (ziduan.IndexOf(",classid") == -1)
                {
                    ziduan = ziduan + ",classid";
                }
            }
            //end
            if (page == 0)
            {
                d_sql1 = "SELECT TOP top1 table_ziduan FROM table_name WHERE  table_where1 table_order";
                d_sql1 = d_sql1.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where1", wherestring1).Replace("table_order", orderby);
            }
            else
            {
                d_sql1 = "SELECT TOP top1 table_ziduan FROM table_name WHERE id not in (SELECT TOP top2 id FROM table_name where table_where1 table_order)  and  table_where1 table_order";
                d_sql1 = d_sql1.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where1", wherestring1).Replace("table_order", orderby);
            }
            //HttpContext.Current.Response.Write(d_sql1);
            //HttpContext.Current.Response.End();
            d_sql2 = "select count(id) as count_id from table_name table_where";
            d_sql2 = d_sql2.Replace("table_name", table_name).Replace("table_where", wherestring);


        }


    }
    //动态分页第三步获取SQL结束

    //动态分页第一步
    public void set_e_w_list(string table_name, string n_id, int page)
    {
        string file_content = my_b.get_value("u5", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id in (" + n_id + ")");
        file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + file_content), Encoding.UTF8);
        file_content = set_sitekey(file_content);

        Regex reg = new Regex(@"{fzw:list.*?}.*?{/fzw:list.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            get_e_w_list_sql(match.ToString(), n_id, table_name, page);
            string list_content = "";
            list_content = Regex.Replace(match.ToString(), @"{fzw:list.*?}", "", RegexOptions.Singleline).ToString();
            list_content = Regex.Replace(list_content.ToString(), @"{/fzw:list}", "", RegexOptions.Singleline).ToString();
            page_e_w_list(d_sql1, d_sql2, list_content, file_content, match.ToString(), page);
        }

    }
    //动态分页---------英文完

    //伪静态开始------------------------------------------------------
    //伪静态分页第二步
    public void page_wei_list(string sql, string sql2, string list_content, string file_content, string list_c, int page, string Model_id)
    {
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + listid + ")");
        if (dt1.Rows.Count > 1)
        {
            dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + dt1.Rows[0]["sort_id"].ToString() + ")");
        }

        string save_path = "/list" + ConfigurationSettings.AppSettings["line"].ToString() + "" + listid + "" + ConfigurationSettings.AppSettings["line"].ToString() + "" + Model_id + "";

        int sta = 1;
        string t2 = "";
        int d_page = page;
        if (page == 0)
        {
            d_page = 1;
        }
        int top2 = list_size * (page - 1);
        sql = sql.Replace("top1", list_size.ToString());
        sql = sql.Replace("top2", top2.ToString());
        // HttpContext.Current.Response.Write(sql);

        DataTable dt = my_c.GetTable(sql);
        file_content = file_content.Replace("{fzw:pagecount/}", dt.Rows.Count.ToString());
        //HttpContext.Current.Response.Write(dt.Rows.Count);
        //HttpContext.Current.Response.End();
        count_id = int.Parse(my_c.GetTable(sql2).Rows[0]["count_id"].ToString());
        int fenye_count = count_id / list_size;
        float fenye_count1 = (float)count_id / (float)list_size;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string t1 = list_content;
            Regex reg1 = new Regex("{fzw:" + ziduan_id + @":[\s\S]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(list_content);
            foreach (Match match1 in matches1)
            {

                string t3 = "";

                string[] aa = Regex.Split(match1.ToString(), " ");
                for (int j = 0; j < aa.Length; j++)
                {
                    string ziduan = "";

                    if (j == 0)
                    {
                        ziduan = aa[j].ToString().Replace("{fzw:" + ziduan_id + ":", "");
                        try
                        {
                            ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                        }
                        catch
                        {

                            ziduan = ziduan.Replace("/}", "");
                        }

                        t3 = my_b.set_htmldecode(dt.Rows[i][ziduan.Trim()].ToString());
                    }
                    else
                    {
                        if (aa[j].ToString().Replace("/}", "").Trim() != "")
                        {
                            t3 = set_other(aa[j].ToString(), t3);
                        }
                    }


                }
                t1 = t1.Replace(match1.ToString(), t3);

            }
            t1 = set_pageurl(t1, dt.Rows[i]["id"].ToString(), dt.Rows[i]["Filepath"].ToString(), dt.Rows[i]["classid"].ToString());
            try
            {
                t1 = set_xuliehao(t1, i + 1);
            }
            catch
            { }
            t2 = t2 + t1;

            if (sta % list_size == 0 || i + 1 == dt.Rows.Count)
            {

                string html_content = file_content.Replace(list_c, t2);
                string list1 = "";
                int page_list_size = 8;
                try
                {
                    page_list_size = int.Parse(get_lable(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), "size"));
                }
                catch
                { }

                //设置pagelist
                list1 = set_pagelist_all(list1, fenye_count, page_list_size, d_page, file_content, save_path);
                //pagelist end
                // list1 = "<strong> " + list_size + " 条/页 共 " + count_id.ToString() + " 条记录</strong>" + list1;
                try
                {
                    html_content = html_content.Replace(Regex.Match(file_content, "{fzw:list:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), list1);
                }
                catch
                {
                    HttpContext.Current.Response.Write("{fzw:list:page id=\"" + ziduan_id + "\".*?/}");
                    HttpContext.Current.Response.End();
                }

                Regex reg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
                MatchCollection matches = reg.Matches(file_content);
                foreach (Match match in matches)
                {
                    string[] aa = Regex.Split(match.ToString(), " ");
                    string t3 = "";
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string ziduan = "";

                        if (j == 0)
                        {
                            ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                            try
                            {
                                ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                            }
                            catch
                            {

                                ziduan = ziduan.Replace("/}", "");
                            }

                            t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                        }
                        else
                        {
                            if (aa[j].ToString().Replace("/}", "").Trim() != "")
                            {
                                t3 = set_other(aa[j].ToString(), t3);
                            }
                        }
                    }
                    html_content = html_content.Replace(match.ToString(), t3);
                }

                html_content = get_fzw_html(html_content);
                html_content = set_url(html_content);
                HttpContext.Current.Response.Write(html_content);

            }


            sta = sta + 1;
        }


    }
    ////伪静态分页第二步结束

    //伪静态分页第三步获取SQL
    public void get_wei_list_sql(string g1, string n_id, string table_name, int page)
    {
        listid = n_id;
        Regex reg = new Regex(@"{fzw:list[\s\S ]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {


            string t1 = match.ToString().Replace("  ", " ");
            string ziduan = "";
            string orderby = "";
            string classid = "";
            string top_string = "";
            string wherestring = "";
            string type_Fields = "";
            string type = "";
            string order_by = "";
            string[] aa = Regex.Split(t1, " ");
            for (int j = 0; j < aa.Length; j++)
            {
                if (aa[j].ToString().IndexOf("number") > -1)
                {
                    top_string = aa[j].ToString().Replace("number=", "");
                    top_string = top_string.Replace("\"", "").Replace("}", "");
                    if (top_string == "" || top_string == "0")
                    {
                        list_size = 15;
                    }
                    else
                    {
                        list_size = int.Parse(top_string);
                    }
                }
                else if (aa[j].ToString().IndexOf("orderby=") > -1)
                {
                    order_by = aa[j].ToString().Replace("orderby=", "");
                    order_by = order_by.Replace("\"", "").Replace("}", "");

                }
                else if (aa[j].ToString().IndexOf("order") > -1)
                {

                    orderby = aa[j].ToString().Replace("order=", "");
                    orderby = orderby.Replace("\"", "").Replace("}", "");
                    //orderby = "order by " + orderby + "  desc";
                    orderby = "order by id ";

                }
                else if (aa[j].ToString().IndexOf("type_Fields=") > -1)
                {
                    type_Fields = aa[j].ToString().Replace("type_Fields=", "");
                    type_Fields = type_Fields.Replace("\"", "").Replace("}", "");
                }
                else if (aa[j].ToString().IndexOf("type=") > -1)
                {
                    type = aa[j].ToString().Replace("type=", "");
                    type = type.Replace("\"", "").Replace("}", "");

                }
            }
            string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
            id = id.Substring(0, id.IndexOf("\""));
            ziduan_id = id;
            Regex reg1 = new Regex("{fzw:" + id + @"[\s\S]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(g1);
            foreach (Match match1 in matches1)
            {

                string ziduan1 = "";

                if (ziduan == "")
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    ziduan = ziduan1;
                }
                else
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    ziduan = ziduan + "," + ziduan1;
                }



            }
            string wherestring1 = "";
            if (type_Fields != "")
            {
                wherestring = "where classid in (" + n_id + ") and " + type_Fields + " like '%" + type + "%'";
                wherestring1 = " classid in (" + n_id + ") and " + type_Fields + " like '%" + type + "%'";
            }
            else
            {
                wherestring = "where classid in (" + n_id + ")";
                wherestring1 = " classid in (" + n_id + ")";
            }
            orderby = orderby + order_by;
            //设置pageurl
            if (ziduan.IndexOf("Filepath,") == -1)
            {
                if (ziduan.IndexOf(",Filepath") == -1)
                {
                    ziduan = ziduan + ",Filepath";
                }
            }

            ziduan = ziduan.Replace("classid", "lanmuhao");
            if (ziduan.IndexOf("id,") == -1)
            {
                if (ziduan.IndexOf(",id") == -1)
                {
                    ziduan = ziduan + ",id";
                }
            }
            ziduan = ziduan.Replace("lanmuhao", "classid");
            if (ziduan.IndexOf("classid,") == -1)
            {
                if (ziduan.IndexOf(",classid") == -1)
                {
                    ziduan = ziduan + ",classid";
                }
            }
            //end
            if (page == 0)
            {
                d_sql1 = "SELECT TOP top1 table_ziduan FROM table_name WHERE  table_where1 table_order";
                d_sql1 = d_sql1.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where1", wherestring1).Replace("table_order", orderby);
            }
            else
            {
                d_sql1 = "SELECT TOP top1 table_ziduan FROM table_name WHERE id not in (SELECT TOP top2 id FROM table_name where table_where1 table_order)  and  table_where1 table_order";
                d_sql1 = d_sql1.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where1", wherestring1).Replace("table_order", orderby);
            }

            d_sql2 = "select count(id) as count_id from table_name table_where";
            d_sql2 = d_sql2.Replace("table_name", table_name).Replace("table_where", wherestring);

        }


    }
    //动态分页第三步获取SQL结束

    //伪静分页第一步
    public void set_wei_list(string table_name, string n_id, int page, string Model_id)
    {
        string file_content = my_b.get_value("u5", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id in (" + n_id + ")");
        file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + file_content), Encoding.UTF8);
        file_content = set_sitekey(file_content);

        Regex reg = new Regex(@"{fzw:list.*?}.*?{/fzw:list.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            get_wei_list_sql(match.ToString(), n_id, table_name, page);
            string list_content = "";
            list_content = Regex.Replace(match.ToString(), @"{fzw:list.*?}", "", RegexOptions.Singleline).ToString();
            list_content = Regex.Replace(list_content.ToString(), @"{/fzw:list}", "", RegexOptions.Singleline).ToString();
            page_wei_list(d_sql1, d_sql2, list_content, file_content, match.ToString(), page, Model_id);
        }

    }
    //伪静态分页第一步结束

    //静态内容
    public void set_content(string file_content, string table_name, string n_id, string file_path)
    {

        DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + n_id + ")");
        if (Regex.IsMatch(file_content, "{fzw:database:page.*?/}"))
        {
            #region 详细页生成静态文件，出现分页
            string Content_list = Regex.Match(file_content, "{fzw:database:page.*?/}").ToString();
            string type = "test";
            string number = "5000";
            string size = "8";
            string Fields = "Content";
            string Titlte = "u1";
            try
            {
                type = get_lable(Content_list, "type");
            }
            catch
            { }
            try
            {
                number = get_lable(Content_list, "number");
            }
            catch
            { }
            try
            {
                size = get_lable(Content_list, "size");
            }
            catch
            { }
            try
            {
                Fields = get_lable(Content_list, "Fields");
            }
            catch
            { }
            try
            {
                Titlte = get_lable(Content_list, "Titlte");
            }
            catch
            { }

            string Fields_string = dt.Rows[0][Fields].ToString();
            //处理按图片分页
            if (type == "img")
            {


                Regex content_reg = new Regex("<img.*?>", RegexOptions.Singleline);
                MatchCollection content_matches = content_reg.Matches(Fields_string);

                if (content_matches.Count >= 1)
                {

                    int Content_i = 1;
                    foreach (Match content_match in content_matches)
                    {

                        string content_text = "";
                        string list1 = "";
                        int page_list_size = int.Parse(size);
                        int fenye_count = content_matches.Count;
                        int d_page = Content_i;
                        if (fenye_count > page_list_size)
                        {

                            int stapage = 1;
                            int overpage = page_list_size;
                            if (d_page > page_list_size / 2)
                            {
                                stapage = d_page - (page_list_size / 2) - 1;
                                if (d_page + page_list_size / 2 < fenye_count)
                                {
                                    overpage = d_page + page_list_size / 2;
                                }
                                else
                                {
                                    overpage = fenye_count;
                                    stapage = fenye_count - (page_list_size - 1);
                                }

                            }


                            for (int j = stapage; j <= overpage; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int j = 1; j <= fenye_count; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }

                                }
                            }

                        }
                        if ((d_page - 1) == 0 || (d_page - 1) == 1)
                        {
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }
                        else
                        {
                            int shangyiye = d_page - 1;
                            list1 = "<A  href=\"" + n_id + "_" + shangyiye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }

                        if ((d_page) < fenye_count)
                        {
                            int xiayeye = d_page + 1;
                            list1 = list1 + "<A  href=\"" + n_id + "_" + xiayeye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                            list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                        }
                        else
                        {
                            if (fenye_count == 1)
                            {
                                list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                            else
                            {
                                list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                        }

                        content_text = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), list1);


                        Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                        MatchCollection matches = reg.Matches(content_text);
                        foreach (Match match in matches)
                        {
                            string[] aa = Regex.Split(match.ToString(), " ");
                            string t3 = "";
                            for (int j = 0; j < aa.Length; j++)
                            {
                                string ziduan = "";

                                if (j == 0)
                                {
                                    ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                    try
                                    {
                                        ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                    }
                                    catch
                                    {

                                        ziduan = ziduan.Replace("/}", "");
                                    }

                                    if (ziduan.Trim() == Fields)
                                    {
                                        try
                                        {
                                            string img_name = content_text.Substring(content_text.IndexOf("<title>") + "<title>".Length);
                                            img_name = img_name.Substring(0, img_name.IndexOf("</title>"));
                                            t3 = my_b.get_article_pic(content_match.ToString());
                                            t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                        }
                                        catch
                                        {
                                            string img_name = content_text.Substring(content_text.IndexOf("<TITLE>") + "<TITLE>".Length);
                                            img_name = img_name.Substring(0, img_name.IndexOf("</TITLE>"));
                                            t3 = my_b.get_article_pic(content_match.ToString());
                                            t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                        }
                                    }
                                    else if (ziduan.ToLower().Trim() == Titlte)
                                    {
                                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString()) + "（" + d_page.ToString() + "）";
                                        //aa
                                    }
                                    else
                                    {
                                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                                    }
                                }
                                else
                                {
                                    if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                    {
                                        if (ziduan.Trim() == Fields)
                                        {
                                            try
                                            {
                                                string img_name = content_text.Substring(content_text.IndexOf("<title>") + "<title>".Length);
                                                img_name = img_name.Substring(0, img_name.IndexOf("</title>"));
                                                t3 = my_b.get_article_pic(content_match.ToString());
                                                t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                            }
                                            catch
                                            {
                                                string img_name = content_text.Substring(content_text.IndexOf("<TITLE>") + "<TITLE>".Length);
                                                img_name = img_name.Substring(0, img_name.IndexOf("</TITLE>"));
                                                t3 = my_b.get_article_pic(content_match.ToString());
                                                t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                            }
                                        }
                                        else if (ziduan.ToLower().Trim() == Titlte)
                                        {
                                            t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString()) + "（" + d_page.ToString() + "）";
                                        }
                                        else
                                        {
                                            t3 = set_other(aa[j].ToString(), t3);
                                        }

                                    }
                                }
                            }
                            content_text = content_text.Replace(match.ToString(), t3);
                        }



                        content_text = get_fzw_html(content_text);
                        content_text = set_url(content_text);
                        content_text = set_sitekey(content_text);
                        if (Content_i == 1)
                        {
                            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + file_path + n_id + ".html", content_text, Encoding.UTF8);
                        }
                        else
                        {
                            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + file_path + n_id + "_" + Content_i + ".html", content_text, Encoding.UTF8);
                        }
                        Content_i = Content_i + 1;
                    }
                }
                else
                {

                    file_content = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), "");
                    Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(file_content);
                    foreach (Match match in matches)
                    {
                        string[] aa = Regex.Split(match.ToString(), " ");
                        string t3 = "";
                        for (int j = 0; j < aa.Length; j++)
                        {
                            string ziduan = "";

                            if (j == 0)
                            {
                                ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                try
                                {
                                    ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                }
                                catch
                                {

                                    ziduan = ziduan.Replace("/}", "");
                                }

                                t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                            }
                            else
                            {
                                if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                {
                                    t3 = set_other(aa[j].ToString(), t3);
                                }
                            }
                        }
                        file_content = file_content.Replace(match.ToString(), t3);
                    }



                    file_content = get_fzw_html(file_content);
                    file_content = set_sitekey(file_content);
                    File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + file_path + n_id + ".html", file_content, Encoding.UTF8);
                }
            }
            else
            {
                //按文字分页
                if (type == "<hr/>")
                {
                    type = @"<hr style=""page-break-after:always;"" class=""ke-pagebreak"" />";
                }
                Regex content_reg = new Regex(type, RegexOptions.Singleline);
                //MatchCollection content_matches = content_reg.Matches(Fields_string);
                string[] bb = content_reg.Split(Fields_string);
                //HttpContext.Current.Response.Write(bb[0]);
                //HttpContext.Current.Response.End();
                if (bb.Length > 1)
                {

                    int Content_i = 1;
                    for (int i = 0; i < bb.Length; i++)
                    {

                        string content_text = "";
                        string list1 = "";
                        int page_list_size = int.Parse(size);
                        int fenye_count = bb.Length;
                        int d_page = Content_i;
                        if (fenye_count > page_list_size)
                        {

                            int stapage = 1;
                            int overpage = fenye_count;



                            for (int j = stapage; j <= overpage; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + file_path + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + file_path + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int j = 1; j <= fenye_count; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + file_path + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + file_path + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }

                                }
                            }

                        }
                        if ((d_page - 1) == 0 || (d_page - 1) == 1)
                        {
                            list1 = "<A  href=\"" + file_path + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + file_path + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }
                        else
                        {
                            int shangyiye = d_page - 1;
                            list1 = "<A  href=\"" + file_path + n_id + "_" + shangyiye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + file_path + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }

                        if ((d_page) < fenye_count)
                        {
                            int xiayeye = d_page + 1;
                            list1 = list1 + "<A  href=\"" + file_path + n_id + "_" + xiayeye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                            list1 = list1 + "<A  href=\"" + file_path + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                        }
                        else
                        {
                            if (fenye_count == 1)
                            {
                                list1 = list1 + "<A  href=\"" + file_path + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + file_path + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                            else
                            {
                                list1 = list1 + "<A  href=\"" + file_path + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + file_path + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                        }
                        //加入css
                        list1 = "<div class=\"page\">" + list1 + "</div>";
                        //end
                        content_text = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), bb[i].ToString() + list1);
                        content_text = content_text.Replace(@"<hr style=""page-break-after:always;"" class=""ke-pagebreak"" />", "");

                        Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                        MatchCollection matches = reg.Matches(content_text);
                        foreach (Match match in matches)
                        {
                            string[] aa = Regex.Split(match.ToString(), " ");
                            string t3 = "";
                            for (int j = 0; j < aa.Length; j++)
                            {
                                string ziduan = "";

                                if (j == 0)
                                {
                                    ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                    try
                                    {
                                        ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                    }
                                    catch
                                    {

                                        ziduan = ziduan.Replace("/}", "");
                                    }
                                    if (ziduan.Trim() == Fields)
                                    {
                                        t3 = bb[i].ToString();
                                    }
                                    else
                                    {
                                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                                    }
                                }
                                else
                                {
                                    if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                    {
                                        if (ziduan.Trim() == Fields)
                                        {
                                            t3 = set_other(aa[j].ToString(), bb[i].ToString()) + type;
                                        }
                                        else
                                        {
                                            t3 = set_other(aa[j].ToString(), t3);
                                        }

                                    }
                                }
                            }
                            content_text = content_text.Replace(match.ToString(), t3);
                        }



                        content_text = get_fzw_html(content_text);
                        content_text = set_url(content_text);
                        content_text = set_sitekey(content_text);
                        //HttpContext.Current.Response.Write(content_text);
                        //HttpContext.Current.Response.End();
                        if (Content_i == 1)
                        {
                            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + file_path + n_id + ".html", content_text, Encoding.UTF8);
                        }
                        else
                        {
                            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + file_path + n_id + "_" + Content_i + ".html", content_text, Encoding.UTF8);
                        }
                        Content_i = Content_i + 1;
                    }
                }
                else
                {
                    file_content = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), Fields_string);


                    Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(file_content);
                    foreach (Match match in matches)
                    {
                        string[] aa = Regex.Split(match.ToString(), " ");
                        string t3 = "";
                        for (int j = 0; j < aa.Length; j++)
                        {
                            string ziduan = "";

                            if (j == 0)
                            {
                                ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                try
                                {
                                    ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                }
                                catch
                                {

                                    ziduan = ziduan.Replace("/}", "");
                                }

                                t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                            }
                            else
                            {
                                if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                {
                                    t3 = set_other(aa[j].ToString(), t3);
                                }
                            }
                        }
                        file_content = file_content.Replace(match.ToString(), t3);
                    }



                    file_content = get_fzw_html(file_content);
                    file_content = set_url(file_content);
                    file_content = set_sitekey(file_content);
                    File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + file_path + n_id + ".html", file_content, Encoding.UTF8);
                }



            }
            #endregion

        }
        else
        {
            #region 无分页
            Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(file_content);
            foreach (Match match in matches)
            {
                string[] aa = Regex.Split(match.ToString(), " ");
                string t3 = "";
                for (int j = 0; j < aa.Length; j++)
                {
                    string ziduan = "";

                    if (j == 0)
                    {
                        ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                        try
                        {
                            ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                        }
                        catch
                        {

                            ziduan = ziduan.Replace("/}", "");
                        }

                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                    }
                    else
                    {
                        if (aa[j].ToString().Replace("/}", "").Trim() != "")
                        {
                            t3 = set_other(aa[j].ToString(), t3);
                        }
                    }
                }
                file_content = file_content.Replace(match.ToString(), t3);
            }



            file_content = get_fzw_html(file_content);
            file_content = set_url(file_content);
            file_content = set_sitekey(file_content);
            file_content = get_fzw_html(file_content);

            try
            {
                File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + file_path + n_id + ".html", file_content, Encoding.UTF8);
            }
            catch
            { }
            #endregion
        }

    }
    //静态内容结束

    //动态内容
    public string set_w_content(string file_content, string table_name, string n_id)
    {
        file_content = set_sitekey(file_content);


        DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + n_id + ")");

        if (Regex.IsMatch(file_content, "{fzw:database:page.*?/}"))
        {

            string Content_list = Regex.Match(file_content, "{fzw:database:page.*?/}").ToString();
            string type = "test";
            string number = "5000";
            string size = "8";
            string Fields = "Content";
            string Titlte = "u1";
            try
            {
                type = get_lable(Content_list, "type");
            }
            catch
            { }
            try
            {
                number = get_lable(Content_list, "number");
            }
            catch
            { }
            try
            {
                size = get_lable(Content_list, "size");
            }
            catch
            { }
            try
            {
                Fields = get_lable(Content_list, "Fields");
            }
            catch
            { }
            try
            {
                Titlte = get_lable(Content_list, "Titlte");
            }
            catch
            { }
            string Fields_string = dt.Rows[0][Fields].ToString();

            if (type == "img")
            {

                Regex content_reg = new Regex("<img.*?>", RegexOptions.Singleline);
                MatchCollection content_matches = content_reg.Matches(Fields_string);
                if (content_matches.Count > 1)
                {

                    int Content_i = 1;
                    foreach (Match content_match in content_matches)
                    {

                        string content_text = "";
                        string list1 = "";
                        int page_list_size = int.Parse(size);
                        int fenye_count = content_matches.Count;
                        int d_page = Content_i;
                        if (fenye_count > page_list_size)
                        {

                            int stapage = 1;
                            int overpage = page_list_size;
                            if (d_page > page_list_size / 2)
                            {
                                stapage = d_page - (page_list_size / 2) - 1;
                                if (d_page + page_list_size / 2 < fenye_count)
                                {
                                    overpage = d_page + page_list_size / 2;
                                }
                                else
                                {
                                    overpage = fenye_count;
                                    stapage = fenye_count - (page_list_size - 1);
                                }

                            }


                            for (int j = stapage; j <= overpage; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int j = 1; j <= fenye_count; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }

                                }
                            }

                        }
                        if ((d_page - 1) == 0 || (d_page - 1) == 1)
                        {
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }
                        else
                        {
                            int shangyiye = d_page - 1;
                            list1 = "<A  href=\"" + n_id + "_" + shangyiye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }

                        if ((d_page) < fenye_count)
                        {
                            int xiayeye = d_page + 1;
                            list1 = list1 + "<A  href=\"" + n_id + "_" + xiayeye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                            list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                        }
                        else
                        {
                            if (fenye_count == 1)
                            {
                                list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                            else
                            {
                                list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                        }

                        content_text = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), list1);


                        Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                        MatchCollection matches = reg.Matches(content_text);
                        foreach (Match match in matches)
                        {
                            string[] aa = Regex.Split(match.ToString(), " ");
                            string t3 = "";
                            for (int j = 0; j < aa.Length; j++)
                            {
                                string ziduan = "";

                                if (j == 0)
                                {
                                    ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                    try
                                    {
                                        ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                    }
                                    catch
                                    {

                                        ziduan = ziduan.Replace("/}", "");
                                    }

                                    if (ziduan.Trim() == Fields)
                                    {
                                        try
                                        {
                                            string img_name = content_text.Substring(content_text.IndexOf("<title>") + "<title>".Length);
                                            img_name = img_name.Substring(0, img_name.IndexOf("</title>"));
                                            t3 = my_b.get_article_pic(content_match.ToString());
                                            t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                        }
                                        catch
                                        {
                                            string img_name = content_text.Substring(content_text.IndexOf("<TITLE>") + "<TITLE>".Length);
                                            img_name = img_name.Substring(0, img_name.IndexOf("</TITLE>"));
                                            t3 = my_b.get_article_pic(content_match.ToString());
                                            t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                        }
                                    }
                                    else if (ziduan.ToLower().Trim() == Titlte)
                                    {
                                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString()) + "（" + d_page.ToString() + "）";
                                    }
                                    else
                                    {
                                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                                    }
                                }
                                else
                                {
                                    if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                    {
                                        if (ziduan.Trim() == Fields)
                                        {
                                            try
                                            {
                                                string img_name = content_text.Substring(content_text.IndexOf("<title>") + "<title>".Length);
                                                img_name = img_name.Substring(0, img_name.IndexOf("</title>"));
                                                t3 = my_b.get_article_pic(content_match.ToString());
                                                t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                            }
                                            catch
                                            {
                                                string img_name = content_text.Substring(content_text.IndexOf("<TITLE>") + "<TITLE>".Length);
                                                img_name = img_name.Substring(0, img_name.IndexOf("</TITLE>"));
                                                t3 = my_b.get_article_pic(content_match.ToString());
                                                t3 = "<img src=" + t3 + " alt=" + img_name + "_" + d_page.ToString() + "><br>" + img_name + "（" + d_page.ToString() + "）";
                                            }
                                        }
                                        else if (ziduan.ToLower().Trim() == Titlte)
                                        {
                                            t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString()) + "（" + d_page.ToString() + "）";
                                        }
                                        else
                                        {
                                            t3 = set_other(aa[j].ToString(), t3);
                                        }

                                    }
                                }
                            }
                            content_text = content_text.Replace(match.ToString(), t3);
                        }



                        content_text = get_fzw_html(content_text);
                        content_text = set_url(content_text);

                        return content_text;
                        Content_i = Content_i + 1;
                    }
                }
                else
                {

                    file_content = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), "");
                    Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(file_content);
                    foreach (Match match in matches)
                    {
                        string[] aa = Regex.Split(match.ToString(), " ");
                        string t3 = "";
                        for (int j = 0; j < aa.Length; j++)
                        {
                            string ziduan = "";

                            if (j == 0)
                            {
                                ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                try
                                {
                                    ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                }
                                catch
                                {

                                    ziduan = ziduan.Replace("/}", "");
                                }

                                t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                            }
                            else
                            {
                                if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                {
                                    t3 = set_other(aa[j].ToString(), t3);
                                }
                            }
                        }
                        file_content = file_content.Replace(match.ToString(), t3);
                    }



                    file_content = get_fzw_html(file_content);
                    file_content = set_url(file_content);
                    return file_content;
                }
            }
            else
            {
                //动态文字分页
                if (type == "<hr/>")
                {
                    type = @"<hr style=""page-break-after:always;"" class=""ke-pagebreak"" />";
                }
                Regex content_reg = new Regex(type, RegexOptions.Singleline);
                //MatchCollection content_matches = content_reg.Matches(Fields_string);
                string[] bb = content_reg.Split(Fields_string);
                //HttpContext.Current.Response.Write(bb[0]);
                //HttpContext.Current.Response.End();
                if (bb.Length > 1)
                {


                    int Content_i = 1;
                    for (int i = 0; i < bb.Length; i++)
                    {

                        string content_text = "";
                        string list1 = "";
                        int page_list_size = int.Parse(size);
                        int fenye_count = bb.Length;
                        int d_page = Content_i;
                        if (fenye_count > page_list_size)
                        {

                            int stapage = 1;
                            int overpage = fenye_count;



                            for (int j = stapage; j <= overpage; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int j = 1; j <= fenye_count; j++)
                            {
                                if (j == d_page)
                                {
                                    list1 = list1 + " <strong>" + j + "</strong>";
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }
                                    else
                                    {
                                        list1 = list1 + "<A  href=\"" + n_id + "_" + j + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">" + j + "</A>";
                                    }

                                }
                            }

                        }
                        if ((d_page - 1) == 0 || (d_page - 1) == 1)
                        {
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }
                        else
                        {
                            int shangyiye = d_page - 1;
                            list1 = "<A  href=\"" + n_id + "_" + shangyiye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">上一页</A>" + list1;
                            list1 = "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">首页</A>" + list1;
                        }

                        if ((d_page) < fenye_count)
                        {
                            int xiayeye = d_page + 1;
                            list1 = list1 + "<A  href=\"" + n_id + "_" + xiayeye + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                            list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                        }
                        else
                        {
                            if (fenye_count == 1)
                            {
                                list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + n_id + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                            else
                            {
                                list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">下一页</A>";
                                list1 = list1 + "<A  href=\"" + n_id + "_" + fenye_count + ConfigurationSettings.AppSettings["Suffix"].ToString() + "\">尾页</A>";
                            }
                        }

                        //加入css
                        list1 = "<div class=\"page\">" + list1 + "</div>";
                        //end
                        content_text = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), bb[i].ToString() + list1);



                        Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                        MatchCollection matches = reg.Matches(content_text);
                        foreach (Match match in matches)
                        {
                            string[] aa = Regex.Split(match.ToString(), " ");
                            string t3 = "";
                            for (int j = 0; j < aa.Length; j++)
                            {
                                string ziduan = "";

                                if (j == 0)
                                {
                                    ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                    try
                                    {
                                        ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                    }
                                    catch
                                    {

                                        ziduan = ziduan.Replace("/}", "");
                                    }
                                    if (ziduan.Trim() == Fields)
                                    {
                                        t3 = bb[i].ToString();
                                    }
                                    else
                                    {
                                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                                    }
                                }
                                else
                                {
                                    if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                    {
                                        if (ziduan.Trim() == Fields)
                                        {
                                            t3 = set_other(aa[j].ToString(), bb[i].ToString()) + type;
                                        }
                                        else
                                        {
                                            t3 = set_other(aa[j].ToString(), t3);
                                        }

                                    }
                                }
                            }
                            content_text = content_text.Replace(match.ToString(), t3);
                        }

                        content_text = set_sitekey(content_text);


                        content_text = get_fzw_html(content_text);
                        content_text = set_url(content_text);
                        return content_text;
                        Content_i = Content_i + 1;
                    }
                }
                else
                {

                    file_content = file_content.Replace(Regex.Match(file_content, "{fzw:database:page.*?/}", RegexOptions.Singleline).ToString(), "");
                    Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(file_content);
                    foreach (Match match in matches)
                    {
                        string[] aa = Regex.Split(match.ToString(), " ");
                        string t3 = "";
                        for (int j = 0; j < aa.Length; j++)
                        {
                            string ziduan = "";

                            if (j == 0)
                            {
                                ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                try
                                {
                                    ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                }
                                catch
                                {

                                    ziduan = ziduan.Replace("/}", "");
                                }

                                t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                            }
                            else
                            {
                                if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                {
                                    t3 = set_other(aa[j].ToString(), t3);
                                }
                            }
                        }
                        file_content = file_content.Replace(match.ToString(), t3);
                    }



                    file_content = get_fzw_html(file_content);
                    file_content = set_url(file_content);
                    return file_content;
                }



            }



        }
        else
        {

            //所有没有
            Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(file_content);
            foreach (Match match in matches)
            {
                string[] aa = Regex.Split(match.ToString(), " ");
                string t3 = "";
                for (int j = 0; j < aa.Length; j++)
                {
                    string ziduan = "";

                    if (j == 0)
                    {
                        ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                        try
                        {
                            ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                        }
                        catch
                        {

                            ziduan = ziduan.Replace("/}", "");
                        }

                        t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                    }
                    else
                    {
                        if (aa[j].ToString().Replace("/}", "").Trim() != "")
                        {
                            t3 = set_other(aa[j].ToString(), t3);
                        }
                    }
                }
                file_content = file_content.Replace(match.ToString(), t3);
            }



            file_content = get_fzw_html(file_content);

            file_content = set_url(file_content);

            return file_content;
        }
        return "";

    }
    //动态内容结束

    //静态单页面
    public string set_Single(string file_content, string table_name, string n_id)
    {
        file_content = set_sitekey(file_content);


        DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + n_id + ")");

        Regex reg = new Regex(@"{fzw:database:.*?/}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            string[] aa = Regex.Split(match.ToString(), " ");
            string t3 = "";
            for (int j = 0; j < aa.Length; j++)
            {
                string ziduan = "";

                if (j == 0)
                {
                    ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                    try
                    {
                        ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                    }
                    catch
                    {

                        ziduan = ziduan.Replace("/}", "");
                    }

                    t3 = my_b.set_htmldecode(dt.Rows[0][ziduan.Trim()].ToString());
                }
                else
                {
                    if (aa[j].ToString().Replace("/}", "").Trim() != "")
                    {
                        t3 = set_other(aa[j].ToString(), t3);
                    }
                }
            }
            file_content = file_content.Replace(match.ToString(), t3);
        }

        file_content = get_fzw_html(file_content);

        return set_url(file_content);
    }
    //静态单页面结束

    //动态单页面
    public string Single_page(string file_content)
    {
        file_content = set_sitekey(file_content);



        file_content = get_fzw_html(file_content);

        return set_url(file_content);
    }
    //动态单页面结束

    //成对标签处理
    public string get_dan_html(string file_content)
    {


        Regex reg = new Regex(@"{fzw:sql[\s\S]*?{/fzw[\s\S]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            string sql = get_fzw_sql(match.ToString());

            if (sql == "")
            {
                break;
            }
            string list_content = Regex.Replace(match.ToString(), @"{fzw:[\s\S]*?id=""[\d\w]*""}", "");
            list_content = list_content.Replace(Regex.Match(list_content, @"{/fzw:.*?}").ToString(), "");

            string sql_html_str = set_sql_html(sql, list_content);
            file_content = file_content.Replace(match.ToString(), sql_html_str);


        }

        //HttpContext.Current.Response.End();
        return file_content;
    }
    public string get_fzw_html(string file_content)
    {
        file_content = set_sitekey(file_content);

        Regex reg = new Regex(@"{fzw:for:.*?{/fzw:[\s\S]*?}[\s\S]*?{/fzw:for:[\s\S]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);

        foreach (Match match in matches)
        {
            //HttpContext.Current.Response.Write(match);
            //HttpContext.Current.Response.End();

            string sql = get_fzw_for_sql(match.ToString());
            //HttpContext.Current.Response.Write(sql);
            //HttpContext.Current.Response.End();
            string list_content = "";
            Regex reg1 = new Regex(@"{fzw:for:[\s\S ]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(match.ToString());
            foreach (Match match1 in matches1)
            {
                list_content = match.ToString().Replace(match1.ToString(), "");
                break;
            }
            Regex reg2 = new Regex(@"{/fzw:for:[\s\S]*?}[\s\S]*?{/fzw:", RegexOptions.Singleline);
            list_content = Regex.Replace(list_content, @"{/fzw:for:[\s\S]*?}[\s\S]*?{/fzw:[\s\S]*?}", reg2.Match(list_content).ToString().Substring(0, reg2.Match(list_content).ToString().Length - 6));

            if (Regex.IsMatch(list_content.ToString(), @"{fzw:for:.*id=""[\w\d]*""}.*?{/fzw:[\s\S]*?}", RegexOptions.Singleline))
            {

                string sql_html_str = set_sql_html(sql, list_content).Replace("{fzw:for:", "{fzw:");

                file_content = file_content.Replace(match.ToString(), sql_html_str);
                //HttpContext.Current.Response.Write(file_content);
                //HttpContext.Current.Response.End();
            }
        }

        file_content = get_dan_html(file_content);
        return file_content;
    }
    //for结构输出
    public string get_fzw_for_sql(string g1)
    {

        Regex reg = new Regex(@"{fzw:for:[\s\S ]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {

            if (match.ToString().IndexOf("sql=") > -1)
            {
                string gg1 = g1.ToString().Replace("{fzw:for:sql=\"", "");
                gg1 = gg1.Substring(0, gg1.IndexOf("\""));
                string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
                id = id.Substring(0, id.IndexOf("\""));
                ziduan_id = id;

                //自动增加系统默认字段
                string jia_table_name = gg1;
                try
                {
                    jia_table_name = jia_table_name.Substring(jia_table_name.IndexOf("from") + 4).Trim();
                    jia_table_name = jia_table_name.Substring(0, jia_table_name.IndexOf(" ")).Trim();
                }
                catch
                { }

                if (gg1.IndexOf("*") == -1)
                {

                    if (my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + jia_table_name + "'") == "文章模型")
                    {
                        if (gg1.Substring(0, gg1.IndexOf("from")).IndexOf("Filepath") == -1)
                        {
                            gg1 = gg1.Insert(gg1.IndexOf("from"), ",Filepath ");
                        }
                        if (gg1.Substring(0, gg1.IndexOf("from")).IndexOf("id") == -1)
                        {
                            gg1 = gg1.Insert(gg1.IndexOf("from"), ",id ");
                        }
                        if (gg1.Substring(0, gg1.IndexOf("from")).IndexOf("classid") == -1)
                        {
                            gg1 = gg1.Insert(gg1.IndexOf("from"), ",classid ");
                        }
                    }
                }
                //自动增加系统默认字段（完）

                return gg1;
            }
            else
            {

                string t1 = match.ToString().Replace("  ", " ");
                string biaoming = "";
                string ziduan = "";
                string orderby = "";
                string classid = "";
                string top_string = "";
                string wherestring = "";
                string type_Fields = "";
                string type = "";
                string[] aa = Regex.Split(t1, " ");
                for (int j = 0; j < aa.Length; j++)
                {
                    if (j == 0)
                    {
                        biaoming = aa[j].ToString().Replace("{fzw:for:", "").Trim();
                    }
                    else
                    {
                        if (aa[j].ToString().IndexOf("number=") > -1)
                        {
                            top_string = aa[j].ToString().Replace("number=", "");
                            top_string = top_string.Replace("\"", "").Replace("}", "");
                            if (top_string == "" || top_string == "0")
                            {
                                top_string = "";
                            }
                            else
                            {
                                top_string = "top " + top_string;
                            }
                        }
                        else if (aa[j].ToString().IndexOf("listid=") > -1)
                        {
                            classid = aa[j].ToString().Replace("listid=", "");
                            classid = classid.Replace("\"", "").Replace("}", "");
                            DataTable dt = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id in (" + classid + ")");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i2 = 0; i2 < dt.Rows.Count; i2++)
                                {
                                    if (classid == "")
                                    {
                                        classid = dt.Rows[i2]["id"].ToString();
                                    }
                                    else
                                    {
                                        classid = classid + "," + dt.Rows[i2]["id"].ToString();
                                    }
                                }
                            }

                        }
                        else if (aa[j].ToString().IndexOf("type_Fields=") > -1)
                        {
                            type_Fields = aa[j].ToString().Replace("type_Fields=", "");
                            type_Fields = type_Fields.Replace("\"", "").Replace("}", "");
                        }
                        else if (aa[j].ToString().IndexOf("type=") > -1)
                        {
                            type = aa[j].ToString().Replace("type=", "");
                            type = type.Replace("\"", "").Replace("}", "");

                        }
                        else if (aa[j].ToString().IndexOf("order=") > -1)
                        {
                            orderby = aa[j].ToString().Replace("order=", "");
                            orderby = orderby.Replace("\"", "").Replace("}", "");
                            orderby = "order by " + orderby + " desc";
                        }


                    }
                }
                string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
                id = id.Substring(0, id.IndexOf("\""));
                ziduan_id = id;


                Regex reg1 = new Regex("{fzw:for:" + id + @"[\s\S]*?}", RegexOptions.Singleline);
                MatchCollection matches1 = reg1.Matches(g1);
                foreach (Match match1 in matches1)
                {

                    string ziduan1 = "";

                    if (ziduan == "")
                    {
                        ziduan1 = match1.ToString().Replace("{fzw:for:" + id + ":", "");
                        try
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                        }
                        catch
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                        }
                        ziduan = ziduan1;
                    }
                    else
                    {
                        ziduan1 = match1.ToString().Replace("{fzw:for:" + id + ":", "");
                        try
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                        }
                        catch
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                        }
                        ziduan = ziduan + "," + ziduan1;
                    }



                }
                if (classid != "")
                {
                    if (type_Fields != "")
                    {
                        wherestring = "where classid in (" + classid + ") and " + type_Fields + " like '%" + type + "%'";
                    }
                    else
                    {
                        wherestring = "where classid in (" + classid + ")";
                    }
                }
                string sql = "select table_top table_ziduan from table_name table_where table_order ";

                if (ziduan.IndexOf("*") == -1)
                {

                    if (my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + biaoming + "'") == "文章模型")
                    {
                        //设置pageurl
                        if (ziduan.IndexOf("Filepath,") == -1)
                        {
                            if (ziduan.IndexOf(",Filepath") == -1)
                            {
                                ziduan = ziduan + ",Filepath";
                            }
                        }

                        ziduan = ziduan.Replace("classid", "lanmuhao");
                        if (ziduan.IndexOf("id,") == -1)
                        {
                            if (ziduan.IndexOf(",id") == -1)
                            {
                                ziduan = ziduan + ",id";
                            }
                        }
                        ziduan = ziduan.Replace("lanmuhao", "classid");
                        if (ziduan.IndexOf("classid,") == -1)
                        {
                            if (ziduan.IndexOf(",classid") == -1)
                            {
                                ziduan = ziduan + ",classid";
                            }
                        }
                        //end
                    }
                }


                sql = sql.Replace("table_top", top_string).Replace("table_ziduan", ziduan).Replace("table_name", biaoming).Replace("table_where", wherestring).Replace("table_order", orderby);
                return sql;

            }

        }


        return "";
    }

    //单条输出
    public string get_fzw_sql(string g1)
    {

        Regex reg = new Regex(@"{fzw:[\s\S ]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {

            if (match.ToString().IndexOf("sql=") > -1)
            {
                string gg1 = g1.ToString().Replace("{fzw:sql=\"", "");
                gg1 = gg1.Substring(0, gg1.IndexOf("\""));
                string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
                id = id.Substring(0, id.IndexOf("\""));
                ziduan_id = id;

                //自动增加系统默认字段
                string jia_table_name = gg1;
                try
                {
                    jia_table_name = jia_table_name.Substring(jia_table_name.IndexOf("from") + 4).Trim();
                    jia_table_name = jia_table_name.Substring(0, jia_table_name.IndexOf(" ")).Trim();
                }
                catch
                { }

                if (gg1.IndexOf("*") == -1)
                {
                    //HttpContext.Current.Response.Write(gg1);
                    //HttpContext.Current.Response.End();
                    if (my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + jia_table_name + "'") == "文章模型" && gg1.IndexOf("count(") == -1)
                    {
                        if (gg1.Substring(0, gg1.IndexOf("from")).IndexOf("Filepath") == -1)
                        {
                            gg1 = gg1.Insert(gg1.IndexOf("from"), ",Filepath ");
                        }
                        if (gg1.Substring(0, gg1.IndexOf("from")).IndexOf("id") == -1)
                        {
                            gg1 = gg1.Insert(gg1.IndexOf("from"), ",id ");
                        }
                        if (gg1.Substring(0, gg1.IndexOf("from")).IndexOf("classid") == -1)
                        {
                            gg1 = gg1.Insert(gg1.IndexOf("from"), ",classid ");
                        }
                    }
                }
                //自动增加系统默认字段（完）

                return gg1;
            }
            else
            {

                string t1 = match.ToString().Replace("  ", " ");
                string biaoming = "";
                string ziduan = "";
                string orderby = "";
                string classid = "";
                string top_string = "";
                string wherestring = "";
                string type_Fields = "";
                string type = "";
                string[] aa = Regex.Split(t1, " ");
                for (int j = 0; j < aa.Length; j++)
                {
                    if (j == 0)
                    {
                        biaoming = aa[j].ToString().Replace("{fzw:", "").Trim();
                    }
                    else
                    {
                        if (aa[j].ToString().IndexOf("number=") > -1)
                        {
                            top_string = aa[j].ToString().Replace("number=", "");
                            top_string = top_string.Replace("\"", "").Replace("}", "");
                            if (top_string == "" || top_string == "0")
                            {
                                top_string = "";
                            }
                            else
                            {
                                top_string = "top " + top_string;
                            }
                        }
                        else if (aa[j].ToString().IndexOf("listid=") > -1)
                        {
                            classid = aa[j].ToString().Replace("listid=", "");
                            classid = classid.Replace("\"", "").Replace("}", "");
                            DataTable dt = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id in (" + classid + ")");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i2 = 0; i2 < dt.Rows.Count; i2++)
                                {
                                    if (classid == "")
                                    {
                                        classid = dt.Rows[i2]["id"].ToString();
                                    }
                                    else
                                    {
                                        classid = classid + "," + dt.Rows[i2]["id"].ToString();
                                    }
                                }
                            }

                        }
                        else if (aa[j].ToString().IndexOf("type_Fields=") > -1)
                        {
                            type_Fields = aa[j].ToString().Replace("type_Fields=", "");
                            type_Fields = type_Fields.Replace("\"", "").Replace("}", "");
                        }
                        else if (aa[j].ToString().IndexOf("type=") > -1)
                        {
                            type = aa[j].ToString().Replace("type=", "");
                            type = type.Replace("\"", "").Replace("}", "");

                        }
                        else if (aa[j].ToString().IndexOf("order=") > -1)
                        {
                            orderby = aa[j].ToString().Replace("order=", "");
                            orderby = orderby.Replace("\"", "").Replace("}", "");
                            orderby = "order by " + orderby + " desc";
                        }


                    }
                }
                string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
                id = id.Substring(0, id.IndexOf("\""));
                ziduan_id = id;


                Regex reg1 = new Regex("{fzw:" + id + @"[\s\S]*?}", RegexOptions.Singleline);
                MatchCollection matches1 = reg1.Matches(g1);
                foreach (Match match1 in matches1)
                {

                    string ziduan1 = "";

                    if (ziduan == "")
                    {
                        ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                        try
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                        }
                        catch
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                        }
                        ziduan = ziduan1;
                    }
                    else
                    {
                        ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                        try
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                        }
                        catch
                        {
                            ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                        }
                        ziduan = ziduan + "," + ziduan1;
                    }



                }
                if (classid != "")
                {
                    if (type_Fields != "")
                    {
                        wherestring = "where classid in (" + classid + ") and " + type_Fields + " like '%" + type + "%'";
                    }
                    else
                    {
                        wherestring = "where classid in (" + classid + ")";
                    }
                }
                string sql = "select table_top table_ziduan from table_name table_where table_order ";

                if (ziduan.IndexOf("*") == -1)
                {

                    if (my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + biaoming + "'") == "文章模型" && ziduan.IndexOf("count(") == -1)
                    {

                        //设置pageurl
                        if (ziduan.IndexOf("Filepath,") == -1)
                        {
                            if (ziduan.IndexOf(",Filepath") == -1)
                            {
                                ziduan = ziduan + ",Filepath";
                            }
                        }

                        ziduan = ziduan.Replace("classid", "lanmuhao");
                        if (ziduan.IndexOf("id,") == -1)
                        {
                            if (ziduan.IndexOf(",id") == -1)
                            {
                                ziduan = ziduan + ",id";
                            }
                        }
                        ziduan = ziduan.Replace("lanmuhao", "classid");
                        if (ziduan.IndexOf("classid,") == -1)
                        {
                            if (ziduan.IndexOf(",classid") == -1)
                            {
                                ziduan = ziduan + ",classid";
                            }
                        }
                        //end
                    }
                }


                sql = sql.Replace("table_top", top_string).Replace("table_ziduan", ziduan).Replace("table_name", biaoming).Replace("table_where", wherestring).Replace("table_order", orderby);
                return sql;

            }

        }


        return "";
    }

    //end
    public string set_sql_html(string g1, string g2)
    {

        DataTable dt = new DataTable();
        try
        {
            dt = my_c.GetTable(g1);
        }
        catch { }
        string t2 = "";
        try
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string t1 = g2;
                Regex reg1 = new Regex("{fzw:" + ziduan_id + @":[\s\S]*?}", RegexOptions.Singleline);
                MatchCollection matches1 = reg1.Matches(g2);
                foreach (Match match1 in matches1)
                {

                    string t3 = "";

                    string[] aa = Regex.Split(match1.ToString(), " ");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string ziduan = "";

                        if (j == 0)
                        {
                            ziduan = aa[j].ToString().Replace("{fzw:" + ziduan_id + ":", "");
                            try
                            {
                                ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                            }
                            catch
                            {
                                ziduan = ziduan.Replace("/}", "");
                            }

                            t3 = my_b.set_htmldecode(dt.Rows[i][ziduan.Trim()].ToString());
                        }
                        else
                        {
                            if (aa[j].ToString().Replace("/}", "").Trim() != "")
                            {
                                t3 = set_other(aa[j].ToString(), t3);
                            }
                        }


                    }

                    t1 = t1.Replace(match1.ToString(), t3);

                }
                Regex reg = new Regex(@"{fzw:pageurl.*?}", RegexOptions.Singleline);

                if (reg.IsMatch(t1))
                {

                    t1 = set_pageurl(t1, dt.Rows[i]["id"].ToString(), dt.Rows[i]["Filepath"].ToString(), dt.Rows[i]["classid"].ToString());
                }
                try
                {
                    t1 = set_xuliehao(t1, i);
                }
                catch
                { }
                t2 = t2 + t1;

            }
        }
        catch { }

        return t2;

    }
    //成对标签处理结束

    public string set_xuliehao(string file_content, int i)
    {
        Regex reg = new Regex(@"{fzw:xuliehao.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        i = i + 1;
        foreach (Match match in matches)
        {
            file_content = file_content.Replace(match.ToString(), i.ToString());
        }
        return file_content;
    }

    //设置文章url
    public string set_pageurl(string file_content, string id, string Filepath, string classid)
    {

        Regex reg = new Regex(@"{fzw:pageurl.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);

        foreach (Match match in matches)
        {

            string listurl = "";
            if (my_b.set_mode() == "伪静态")
            {

                listurl = "/page_" + id + "_" + classid + "" + ConfigurationSettings.AppSettings["Suffix"].ToString();
            }
            else if (my_b.set_mode() == "静态网站")
            {
                listurl = Filepath + id + ConfigurationSettings.AppSettings["Suffix"].ToString();
            }
            else
            {
                listurl = "/page.aspx?id=" + id + "&classid=" + classid + "";
            }
            file_content = file_content.Replace(match.ToString(), listurl);

        }
        return file_content;
    }
    //


    //搜索页列表第二步
    public void set_search_list(string sql, string list_content, string file_content, string list_c, int page, string key)
    {
        //HttpContext.Current.Response.Write(sql);
        //HttpContext.Current.Response.End();
        //sql = "select u1,dtime,Filepath,classid,id from sl_article where u1 like '%后%' order by id";
        key = HttpUtility.UrlDecode(key);
        int sta = 1;
        string t2 = "";
        int d_page = page;
        DataTable dt = my_c.GetTable(sql);

        file_content = file_content.Replace("{fzw:pagecount/}", dt.Rows.Count.ToString());
        int fenye_count = count_id / list_size;
        float fenye_count1 = (float)count_id / (float)list_size;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }

        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t1 = list_content;
                Regex reg1 = new Regex("{fzw:" + ziduan_id + @":[\s\S]*?}", RegexOptions.Singleline);
                MatchCollection matches1 = reg1.Matches(list_content);
                foreach (Match match1 in matches1)
                {

                    string t3 = "";

                    string[] aa = Regex.Split(match1.ToString(), " ");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string ziduan = "";

                        if (j == 0)
                        {
                            ziduan = aa[j].ToString().Replace("{fzw:" + ziduan_id + ":", "");
                            try
                            {
                                ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                            }
                            catch
                            {

                                ziduan = ziduan.Replace("/}", "");
                            }

                            t3 = my_b.set_htmldecode(dt.Rows[i][ziduan.Trim()].ToString());
                        }
                        else
                        {
                            if (aa[j].ToString().Replace("/}", "").Trim() != "")
                            {
                                t3 = set_other(aa[j].ToString(), t3);
                            }
                        }


                    }
                    t1 = t1.Replace(match1.ToString(), t3);

                }
                try
                {
                    t1 = set_pageurl(t1, dt.Rows[i]["id"].ToString(), dt.Rows[i]["Filepath"].ToString(), dt.Rows[i]["classid"].ToString());
                }
                catch
                { }
                try
                {
                    t1 = set_xuliehao(t1, i + 1);
                }
                catch
                { }
                t2 = t2 + t1;

                if (sta % list_size == 0 || i + 1 == dt.Rows.Count)
                {

                    string html_content = file_content.Replace(list_c, t2);
                    string list1 = "";
                    int page_list_size = 8;
                    try
                    {
                        page_list_size = int.Parse(get_lable(Regex.Match(file_content, "{fzw:search:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), "size"));
                    }
                    catch
                    { }
                    //------搜索列表
                    //设置searchlist
                    if (my_b.set_mode() == "伪静态")
                    {
                        list1 = set_pagelist_all(list1, fenye_count, page_list_size, d_page, file_content, "/search" + ConfigurationSettings.AppSettings["line"].ToString() + "" + HttpContext.Current.Request.QueryString.ToString() + "" + ConfigurationSettings.AppSettings["line"].ToString() + "");
                    }
                    else
                    {
                        list1 = set_pagelist_all(list1, fenye_count, page_list_size, d_page, file_content, "/search.aspx?" + HttpContext.Current.Server.UrlDecode(set_search_url(HttpContext.Current.Request.QueryString.ToString())) + "");
                    }
                        
                    //pagelist end
                    
                    //------搜索列表
                    if (my_b.set_fangwen() == 1)
                    {

                    }
                    else
                    {
                        try
                        {
                            html_content = html_content.Replace(Regex.Match(file_content, "{fzw:search:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), list1);
                        }
                        catch
                        {

                        }
                    }
                   
                    try
                    {
                        html_content = html_content.Replace(Regex.Match(file_content, "{fzw:search:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), list1);
                    }
                    catch { }

                    Regex reg = new Regex(@"{fzw:database:[\w\d]*?/}", RegexOptions.Singleline);
                    MatchCollection matches = reg.Matches(file_content);
                    foreach (Match match in matches)
                    {
                        string[] aa = Regex.Split(match.ToString(), " ");
                        string t3 = "";
                        for (int j = 0; j < aa.Length; j++)
                        {
                            string ziduan = "";

                            if (j == 0)
                            {
                                ziduan = aa[j].ToString().Replace("{fzw:database:", "");
                                try
                                {
                                    ziduan = ziduan.Substring(0, ziduan.IndexOf(" "));
                                }
                                catch
                                {

                                    ziduan = ziduan.Replace("/}", "");
                                }

                                //t3 = dt1.Rows[0][ziduan.Trim()].ToString();
                            }
                            else
                            {
                                if (aa[j].ToString().Replace("/}", "").Trim() != "")
                                {
                                    t3 = set_other(aa[j].ToString(), t3);
                                }
                            }
                        }

                        html_content = html_content.Replace(match.ToString(), t3);
                    }

                    html_content = get_fzw_html(html_content);

                    html_content = set_url(html_content);


                    HttpContext.Current.Response.Write(html_content);
                    HttpContext.Current.Response.End();

                }


                sta = sta + 1;
            }
        }
        else
        {
            //HttpContext.Current.Response.Write(dt.Rows.Count.ToString());
            //HttpContext.Current.Response.End();
            file_content = Regex.Replace(file_content, @"{fzw:search[\s\S]*?{/fzw:search}", "您搜索的信息不存在", RegexOptions.Singleline).ToString();
            try
            {
                file_content = file_content.Replace(Regex.Match(file_content, "{fzw:search:page id=\"" + ziduan_id + "\".*?/}", RegexOptions.Singleline).ToString(), "");
                file_content = Regex.Replace(file_content, @"{fzw:database:.*?}", "", RegexOptions.Singleline).ToString();
            }
            catch { }
            file_content = get_fzw_html(file_content);

            file_content = set_url(file_content);

            HttpContext.Current.Response.Write(file_content);
        }

    }
    //搜索页列表第二步结束
    public string set_search_url(string g1)
    {
        if (g1.IndexOf("&page") > -1)
        {
            return g1.Substring(0, g1.LastIndexOf("&"));
        }
        return g1;
    }
    //搜索页列表获取SQL
    public string get_search_sql(string g1, string key, string table_name, int page)
    {

        Regex reg = new Regex(@"{fzw:search[\s\S ]*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {
            string t1 = match.ToString().Replace("  ", " ");
            string ziduan = "";
            string orderby = "";
            string classid = "";
            string top_string = "";
            string wherestring = "";
            string order_by = "";
            string sqlvalue = "";
            string[] aa = Regex.Split(t1, " ");
            for (int j = 0; j < aa.Length; j++)
            {
                if (aa[j].ToString().IndexOf("number") > -1)
                {
                    top_string = aa[j].ToString().Replace("number=", "");
                    top_string = top_string.Replace("\"", "").Replace("}", "");
                    if (top_string == "" || top_string == "0")
                    {
                        list_size = 15;
                    }
                    else
                    {
                        list_size = int.Parse(top_string);
                    }
                    try
                    {
                        list_size = int.Parse(HttpContext.Current.Request.QueryString["number"].ToString());
                    }
                    catch { }
                }
                else if (aa[j].ToString().IndexOf("orderby=") > -1)
                {
                    order_by = aa[j].ToString().Replace("orderby=", "");
                    order_by = order_by.Replace("\"", "").Replace("}", "");


                }
                else if (aa[j].ToString().IndexOf("order") > -1)
                {


                    orderby = aa[j].ToString().Replace("order=", "");
                    orderby = orderby.Replace("\"", "").Replace("}", "");



                }
                else if (aa[j].ToString().IndexOf("sqlvalue=") > -1)
                {

                    sqlvalue = aa[j].ToString().Replace("sqlvalue=", "");
                    sqlvalue = sqlvalue.Replace("\"", "").Replace("}", "");
                    sqlvalue = sqlvalue.Replace("&nbsp;", " ");

                }
            }

            string id = g1.Substring(g1.IndexOf(" id=\"") + 5);
            id = id.Substring(0, id.IndexOf("\""));
            ziduan_id = id;

            Regex reg1 = new Regex("{fzw:" + id + @"[\s\S]*?}", RegexOptions.Singleline);
            MatchCollection matches1 = reg1.Matches(g1);
            foreach (Match match1 in matches1)
            {

                string ziduan1 = "";

                if (ziduan == "")
                {

                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    if (ziduan.IndexOf(ziduan1) == -1)
                    {
                        ziduan = ziduan1;
                    }
                }
                else
                {
                    ziduan1 = match1.ToString().Replace("{fzw:" + id + ":", "");
                    try
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf(" "));
                    }
                    catch
                    {
                        ziduan1 = ziduan1.Substring(0, ziduan1.IndexOf("/}"));
                    }
                    string ziduan_str = "";
                    for (int i = 0; i < ziduan.Split(',').Length; i++)
                    {
                        if (ziduan.Split(',')[i].ToString() == ziduan1)
                        {

                            ziduan_str = "yes";
                        }
                    }
                    if (ziduan_str == "")
                    {
                        ziduan = ziduan + "," + ziduan1;
                    }

                }



            }

            if (orderby == "" || order_by == "")
            {
                orderby = "order by id desc";
            }
            else
            {
                orderby = "order  by " + orderby + " " + order_by + " ,id desc";
            }
            //HttpContext.Current.Response.Write(orderby);
            //HttpContext.Current.Response.End();
            wherestring = get_quer_sql(key, table_name);
            if (sqlvalue != "")
            {
                if (wherestring == "")
                {
                    wherestring = wherestring + " where " + sqlvalue;
                }
                else
                {
                    wherestring = wherestring + " and " + sqlvalue;
                }
            }
            string sql = "select table_ziduan from table_name table_where table_order ";

            if (page > 1)
            {
                int n_page = page - 1;
                sql = "select top " + page * list_size + " table_ziduan from table_name table_where and id not in (select top " + n_page * list_size + " id from table_name table_where table_order) table_order ";
            }
            if (my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + table_name + "'") == "文章模型"|| my_b.get_value("u3", ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where u1='" + table_name + "'") == "产品模型")
            {
                //设置pageurl
                if (ziduan.IndexOf("Filepath,") == -1)
                {
                    if (ziduan.IndexOf(",Filepath") == -1)
                    {
                        ziduan = ziduan + ",Filepath";
                    }
                }

                ziduan = ziduan.Replace("classid", "lanmuhao");
                if (ziduan.IndexOf("id,") == -1)
                {
                    if (ziduan.IndexOf(",id") == -1)
                    {
                        ziduan = ziduan + ",id";
                    }
                }
                ziduan = ziduan.Replace("lanmuhao", "classid");
                if (ziduan.IndexOf("classid,") == -1)
                {
                    if (ziduan.IndexOf(",classid") == -1)
                    {
                        ziduan = ziduan + ",classid";
                    }
                }
                //end


            }
            if (wherestring == "")
            {
                wherestring = "where id>0 ";
            }
            sql = sql.Replace("table_ziduan", ziduan).Replace("table_name", table_name).Replace("table_where", wherestring).Replace("table_order", orderby);
            count_id = my_b.get_count(table_name, wherestring);

            return sql;

        }

        return "";
    }
    //搜索页列表获取SQL结束

    public void dr1(string t1, int t2)
    {
        DataTable dt1 = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid in (" + t1 + ") ");

        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (sort_id == "")
                {
                    sort_id = dt1.Rows[j]["id"].ToString();
                }
                else
                {
                    sort_id = sort_id + "," + dt1.Rows[j]["id"].ToString();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1);
            }
        }

    }
    //从url中配置sql
    public string user_sql(string quer_string)
    {
        string return_string = "";
        if (quer_string != "")
        {

            string[] quer_arr = quer_string.Split('&');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (quer_arr[i].ToString().IndexOf("m=") == -1 && quer_arr[i].ToString().IndexOf("ordertype=") == -1 && quer_arr[i].ToString().IndexOf("orderby=") == -1 && quer_arr[i].ToString().IndexOf("number=") == -1 && quer_arr[i].ToString().IndexOf("page=") == -1 && quer_arr[i].ToString().IndexOf("lan=") == -1 && quer_arr[i].ToString().IndexOf("type=") == -1 && quer_arr[i].ToString().IndexOf("lingshi2=") == -1 && quer_arr[i].ToString().IndexOf("lingshi1=") == -1 && quer_arr[i].ToString().IndexOf("lingshi3=") == -1 && quer_arr[i].ToString().IndexOf("lingshi4=") == -1 && quer_arr[i].ToString().IndexOf("jsoncallback=") == -1 && quer_arr[i].ToString().IndexOf("t=") == -1 && quer_arr[i].ToString().IndexOf("liemingcheng=") == -1 && quer_arr[i].ToString().IndexOf("timestamp=") == -1 && quer_arr[i].ToString().IndexOf("sign=") == -1)
                {
                    string[] sql_arr = quer_arr[i].ToString().Split('=');
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {
                        #region 返回查询字段的拼接SQL语句
                        if (sql_arr[0].IndexOf(".") > -1)
                        {
                            if (return_string == "")
                            {
                                return_string = "sl_Field.u1='" + sql_arr[0].Split('.')[1] + "'";
                            }
                            else
                            {
                                return_string = return_string + " or sl_Field.u1='" + sql_arr[0].Split('.')[1] + "'";
                            }
                        }
                        else
                        {
                            if (return_string == "")
                            {
                                return_string = "sl_Field.u1='" + sql_arr[0] + "'";
                            }
                            else
                            {
                                return_string = return_string + " or sl_Field.u1='" + sql_arr[0] + "'";
                            }
                        }
                        #endregion
                    }
                }
            }
        }

        return return_string;
    }
    public string get_quer_sql(string quer_string, string table_name)
    {
        string all_quer_string = user_sql(quer_string);
      
        #region 获取字段表
        DataTable sl_Field = new DataTable();
        if (all_quer_string != "")
        {
            string[] table_name_ = table_name.Split(',');
            string model_u1 = "";
            for (int i = 0; i < table_name_.Length; i++)
            {
                if (model_u1 == "")
                {
                    model_u1 = "u1='" + table_name_[i] + "'";
                }
                else
                {
                    model_u1 = model_u1+" or u1='" + table_name_[i] + "'";
                }
            }
            //HttpContext.Current.Response.Write("select sl_Field.u1 as u1,sl_Model.u1 as biaoming from sl_Field,sl_Model where sl_FieldModel_id in (select id from sl_Model where " + model_u1 + ") and (" + all_quer_string + ")");
            //HttpContext.Current.Response.End();
            sl_Field = my_c.GetTable("select sl_Field.u1 as u1,sl_Model.u1 as biaoming,sl_Field.u6 as u6 from sl_Field,sl_Model where sl_Field.Model_id=sl_Model.id and sl_Field.Model_id in (select id from sl_Model where " + model_u1 + ") and (" + all_quer_string + ")");
        }
    
        #endregion

        string return_string = "";
        string[] quer_arr = quer_string.Split('&');
        for (int i = 0; i < quer_arr.Length; i++)
        {
            if (quer_arr[i].ToString().IndexOf("m=") == -1 && quer_arr[i].ToString().IndexOf("ordertype=") == -1 && quer_arr[i].ToString().IndexOf("orderby=") == -1 && quer_arr[i].ToString().IndexOf("number=") == -1 && quer_arr[i].ToString().IndexOf("page=") == -1 && quer_arr[i].ToString().IndexOf("lan=") == -1 && quer_arr[i].ToString().IndexOf("type=") == -1 && quer_arr[i].ToString().IndexOf("lingshi2=") == -1 && quer_arr[i].ToString().IndexOf("lingshi1=") == -1 && quer_arr[i].ToString().IndexOf("lingshi3=") == -1 && quer_arr[i].ToString().IndexOf("lingshi4=") == -1 && quer_arr[i].ToString().IndexOf("jsoncallback=") == -1 && quer_arr[i].ToString().IndexOf("t=") == -1 && quer_arr[i].ToString().IndexOf("liemingcheng=") == -1 && quer_arr[i].ToString().IndexOf("timestamp=") == -1 && quer_arr[i].ToString().IndexOf("sign=") == -1 && quer_arr[i].ToString().IndexOf("pageid=") == -1 && quer_arr[i].ToString().IndexOf("ziduan=") == -1)
            {

                string[] sql_arr = quer_arr[i].ToString().Split('=');

                if (sql_arr[1].ToLower()== "between")
                {
                    #region 对时间的处理
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {

                        Regex reg = new Regex(@"^(\d{1,4})(-|\/)(\d{1,2})(-)(\d{1,2})(between)(\d{1,4})(-|\/)(\d{1,2})(-)(\d{1,2})|(between)(\d{1,4})(-|\/)(\d{1,2})(-)(\d{1,2})|(\d{1,4})(-|\/)(\d{1,2})(-)(\d{1,2})(between)", RegexOptions.Singleline);
                        MatchCollection matches = reg.Matches(sql_arr[1].ToLower());

                        if (matches.Count > 0)
                        {

                            string shijian = matches[0].ToString();

                            Regex reg1 = new Regex("between", RegexOptions.Singleline);
                            string[] cc = reg1.Split(shijian);

                            string shijian_qian = cc[0].ToString();
                            if (shijian_qian == "")
                            {
                                shijian_qian = "1990-01-01";
                            }
                            string shijian_hou = cc[1].ToString();
                            if (shijian_hou == "")
                            {
                                shijian_hou = "2020-01-01";
                            }
                            string shang_time = " 00:00:01";
                            string xia_time = " 23:59:59";
                            shijian_qian = shijian_qian + shang_time;
                            shijian_hou = shijian_hou + xia_time;
                            if (return_string == "")
                            {
                                return_string = "where " + sql_arr[0] + "  BETWEEN '" + shijian_qian + "' and '" + shijian_hou + "'";
                            }
                            else
                            {
                                return_string = return_string + " and " + sql_arr[0] + " BETWEEN '" + shijian_qian + "' and '" + shijian_hou + "'";
                            }

                        }
                        else
                        {
                            Regex reg1 = new Regex(@"(\d{1,10})(between)(\d{1,10})", RegexOptions.Singleline);
                            MatchCollection matches1 = reg1.Matches(sql_arr[1].ToLower());
                            if (matches1.Count > 0)
                            {
                                Regex reg2 = new Regex("between", RegexOptions.Singleline);
                                string[] dd = reg2.Split(matches1[0].ToString());

                                if (return_string == "")
                                {
                                    return_string = "where " + sql_arr[0] + "  BETWEEN " + dd[0] + " and " + dd[1] + "";
                                }
                                else
                                {
                                    return_string = return_string + " and " + sql_arr[0] + " BETWEEN " + dd[0] + " and " + dd[1] + "";
                                }
                            }
                            else
                            {
                                if (return_string == "")
                                {
                                    return_string = "where " + sql_arr[0] + "  " + HttpUtility.UrlDecode(sql_arr[1]) + "";
                                }
                                else
                                {
                                    return_string = return_string + " and " + sql_arr[0] + " " + HttpUtility.UrlDecode(sql_arr[1]);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (sql_arr[0].ToLower()== "classid")
                {
                    #region 栏目的处理
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {

                        sort_id = "";
                        string tablename = ConfigurationSettings.AppSettings["Prefix"].ToString()+ "sort";
                        try
                        {
                            string[] table_name_ = my_b.set_url_css(HttpContext.Current.Request.QueryString["t"].ToString()).Split(',');
                            for (int i1 = 0; i1 < 1; i1++)
                            {
                                tablename = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                            }
                        }
                        catch { }
                        if (my_c.GetTable("select id from sl_Model where u1='" + tablename + "' and u3 like '分类模型'").Rows.Count == 0)
                        {
                            tablename = ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort";
                        }

                        string ziduan = "Sort_id";
                        try
                        {
                            ziduan = HttpContext.Current.Request.QueryString["ziduan"].ToString();
                        }
                        catch { }

                        get_sort(HttpUtility.UrlDecode(sql_arr[1]), 0, tablename, ziduan);

                        if (sort_id == "")
                        {
                            sort_id = HttpUtility.UrlDecode(sql_arr[1]);
                        }
                        else
                        {
                            sort_id = sort_id+","+HttpUtility.UrlDecode(sql_arr[1]);
                        }
                        if (return_string == "")
                        {
                            return_string = "where " + sql_arr[0] + " in(" + sort_id + ")";
                        }
                        else
                        {
                            return_string = return_string + " and " + sql_arr[0] + " in(" + sort_id + ")";
                        }
                    }
                    #endregion
                }
                else if (sql_arr[0].ToLower()== "id")
                {
                    #region id查询,可以大于，小于之内，用法可以是id>=1,翻译过来就是id>1
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {
                        if (HttpUtility.UrlDecode(sql_arr[1]).IndexOf(">") > -1 || HttpUtility.UrlDecode(sql_arr[1]).IndexOf("<") > -1)
                        {
                            if (return_string == "")
                            {
                                return_string = "where " + sql_arr[0] + " " + HttpUtility.UrlDecode(sql_arr[1]) + "";
                            }
                            else
                            {
                                return_string = return_string + " and " + sql_arr[0] + " " + HttpUtility.UrlDecode(sql_arr[1]) + "";
                            }
                        }
                        else
                        {
                            if (return_string == "")
                            {
                                return_string = "where " + sql_arr[0] + " in(" + HttpUtility.UrlDecode(sql_arr[1]) + ")";
                            }
                            else
                            {
                                return_string = return_string + " and " + sql_arr[0] + " in(" + HttpUtility.UrlDecode(sql_arr[1]) + ")";
                            }
                        }


                    }
                    #endregion
                }
                else if (HttpUtility.UrlDecode(sql_arr[1].ToLower()).IndexOf("or") > -1)
                {
                    #region 或者条件
                    Regex reg3 = new Regex("or", RegexOptions.Singleline);
                    string[] cc = reg3.Split(HttpUtility.UrlDecode(sql_arr[1].ToLower()));
                    string Field_string = "";
                    for (i = 0; i < cc.Length; i++)
                    {
                        if (Field_string == "")
                        {
                            Field_string = sql_arr[0].ToLower() + " = '" + cc[i] + "'";
                        }
                        else
                        {
                            Field_string = Field_string + " or " + sql_arr[0].ToLower() + " = '" + cc[i] + "'";
                        }
                    }
                    Field_string = "(" + Field_string + ")";

                    if (return_string == "")
                    {
                        return_string = "where " + Field_string;
                    }
                    else
                    {
                        return_string = return_string + " and " + Field_string;
                    }
                    #endregion
                }
                else if (sql_arr[0].ToLower()== "sqlvalue")
                {
                    #region 内置查询
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {
                        string[] sqlvalue = my_b.set_url_css(HttpUtility.UrlDecode(sql_arr[1])).Split('/');
                        if (sqlvalue.Length > 1)
                        {
                            #region OR的条件
                            string sqlvalue_ = "";
                            string sql_str = "";
                            for (int i1 = 0; i1 < sqlvalue.Length; i1++)
                            {
                                //HttpContext.Current.Response.Write(sqlvalue[i1]);
                                //HttpContext.Current.Response.End();
                                sqlvalue_ = sqlvalue[i1].Replace("|", "=");
                                sqlvalue_ = sqlvalue_.Replace("{like}", " like ");
                                sqlvalue_ = sqlvalue_.Replace("{bfb}", "%");
                                if (sql_str == "")
                                {
                                    sql_str =  sqlvalue_;
                                }
                                else
                                {
                                    sql_str = sql_str + " or " + sqlvalue_ + "";
                                }

                            }
                            if (return_string == "")
                            {
                                return_string = "where (" + sql_str + ")";
                            }
                            else
                            {
                                return_string = return_string + " and (" + sql_str + ")";
                            }
                            #endregion

                        }
                        else
                        {
                            #region 其它条件
                            sqlvalue = my_b.set_url_css(HttpUtility.UrlDecode(sql_arr[1])).Split(',');
                            string sqlvalue_ = "";
                            for (int i1 = 0; i1 < sqlvalue.Length; i1++)
                            {
                                sqlvalue_ = sqlvalue[i1].Replace("|", "=");
                                sqlvalue_ = sqlvalue_.Replace("{like}", " like ");
                                sqlvalue_ = sqlvalue_.Replace("{bfb}", "%");
                                if (return_string == "")
                                {
                                    return_string = "where " + sqlvalue_ + "";
                                }
                                else
                                {
                                    return_string = return_string + " and " + sqlvalue_ + "";
                                }

                            }
                            #endregion
                        }


                    }
                    //HttpContext.Current.Response.Write(return_string);
                    //HttpContext.Current.Response.End();
                    #endregion
                }
                else if (sql_arr[0].ToLower()== "yonghuming")
                {
                 
                    #region 处理用户名情况
                    string pageid = "";
                    try
                    {
                        pageid = my_b.c_string(HttpContext.Current.Request.QueryString["pageid"].ToString());
                    }
                    catch { }
                    if (pageid != "")
                    {
                        #region pageid存在
                        string yonghuming = my_b.c_string(HttpUtility.UrlDecode(sql_arr[1]));
                        yonghuming = my_hs.get_yonghuming(yonghuming, sql_arr[0].ToLower());
                        if (yonghuming != "")
                        {
                            if (return_string == "")
                            {
                                return_string = "where (" + yonghuming + ")";
                            }
                            else
                            {
                                return_string = return_string + " and (" + yonghuming + ")";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 其它情况
                        if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                        {
                            #region 参数值不为空
                 
                            for (int j = 0; j < sl_Field.Rows.Count; j++)
                            {
                                string ziduan = "";
                                string biaoming = table_name;
                                if (sql_arr[0].IndexOf(".") > -1)
                                {
                                    ziduan = sql_arr[0].Split('.')[1];
                                    biaoming= sql_arr[0].Split('.')[0];
                                }
                                else
                                {
                                    ziduan = sql_arr[0];
                                }

                                if (sl_Field.Rows[j]["u1"].ToString().ToLower() == ziduan.ToLower()&& biaoming== sl_Field.Rows[j]["biaoming"].ToString().ToLower())
                                {
                                  //  HttpContext.Current.Response.Write(j+"<br>");

                                    #region 判断字段是否相等
                                    string type = sl_Field.Rows[j]["u6"].ToString();
                                    if (type == "数字" || type == "货币" || type == "联动" || type == "分类")
                                    {
                                        #region int float类型
                                        if (return_string == "")
                                        {
                                            return_string = "where " + sql_arr[0] + " = " + HttpUtility.UrlDecode(sql_arr[1]) + "";
                                        }
                                        else
                                        {
                                            return_string = return_string + " and " + sql_arr[0] + " = " + HttpUtility.UrlDecode(sql_arr[1]) + "";
                                        }
                                        #endregion
                                    }
                                    else
                                    {

                                        #region 非int float类型
                                        Regex reg = new Regex("<.*?>", RegexOptions.Singleline);

                                        if (reg.IsMatch(HttpUtility.UrlDecode(sql_arr[1])))
                                        {
                                            if (return_string == "")
                                            {
                                                return_string = "where " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]).Replace("<", "").Replace(">", "") + "'";
                                            }
                                            else
                                            {
                                                return_string = return_string + " and " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]).Replace("<", "").Replace(">", "") + "'";
                                            }
                                        }
                                        else
                                        {
                                            if (return_string == "")
                                            {
                                                return_string = "where " + sql_arr[0] + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                                            }
                                            else
                                            {
                                                return_string = return_string + " and " + sql_arr[0] + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                                            }
                                        }

                                        #endregion
                                    }
                                    #endregion
                                }

                            }

                        
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (sql_arr[0].ToLower()== "chuliren")
                {
                    #region 处理用户名情况
                    string pageid = "";
                    try
                    {
                        pageid = my_b.c_string(HttpContext.Current.Request.QueryString["pageid"].ToString());
                    }
                    catch { }
                    if (pageid != "")
                    {
                        string chuliren = my_b.c_string(HttpContext.Current.Request.QueryString["chuliren"].ToString());
                        chuliren = my_hs.get_yonghuming(chuliren, "chuliren");
                        if (chuliren != "")
                        {
                            if (return_string == "")
                            {
                                return_string = "where (" + chuliren + ")";
                            }
                            else
                            {
                                return_string = return_string + " and (" + chuliren + ")";
                            }
                        }

                    }
                    else
                    {
                        #region 其它情况

                        if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                        {
                            #region 参数值不为空
                            for (int j = 0; j < sl_Field.Rows.Count; j++)
                            {
                                string type = sl_Field.Rows[j]["u6"].ToString();
                                if (type == "数字" || type == "货币" || type == "联动" || type == "分类")
                                {
                                    #region int float类型
                                    if (return_string == "")
                                    {
                                        return_string = "where " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]) + "'";
                                    }
                                    else
                                    {
                                        return_string = return_string + " and " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]) + "'";
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 非int float类型
                                    Regex reg = new Regex("<.*?>", RegexOptions.Singleline);

                                    if (reg.IsMatch(HttpUtility.UrlDecode(sql_arr[1])))
                                    {
                                        if (return_string == "")
                                        {
                                            return_string = "where " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]).Replace("<", "").Replace(">", "") + "'";
                                        }
                                        else
                                        {
                                            return_string = return_string + " and " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]).Replace("<", "").Replace(">", "") + "'";
                                        }
                                    }
                                    else
                                    {
                                        if (return_string == "")
                                        {
                                            return_string = "where " + sql_arr[0] + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                                        }
                                        else
                                        {
                                            return_string = return_string + " and " + sql_arr[0] + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                                        }
                                    }

                                    #endregion
                                }
                            }


                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (sql_arr[0].ToLower()== "fenlei")
                {

                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {

                        sort_id = "";
                        dr1(HttpUtility.UrlDecode(sql_arr[1]), 0);

                        if (sort_id == "")
                        {
                            sort_id = HttpUtility.UrlDecode(sql_arr[1]);
                        }
                        if (return_string == "")
                        {
                            return_string = "where " + sql_arr[0] + " in(" + sort_id + ")";
                        }
                        else
                        {
                            return_string = return_string + " and " + sql_arr[0] + " in(" + sort_id + ")";
                        }
                    }
                }
                else if (sql_arr[0].ToLower()== "all")
                {
                    #region 全表查询
                    sl_Field = my_c.GetTable("select * from sl_Field where Model_id in (select id from sl_Model where u1='" + table_name.Split(',')[0] + "') and (u6 like '文本框' or u6  like  '文本域' or u6  like  '编辑器' or u6  like  '子编辑器' or u6  like  '下拉框' or u6  like  '单选按钮组' or u6  like  '多选按钮组')");
                    string Field_string = "";
                    for (i = 0; i < sl_Field.Rows.Count; i++)
                    {
                        if (Field_string == "")
                        {
                            Field_string = table_name.Split(',')[0] + "." + sl_Field.Rows[i]["u1"].ToString() + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                        }
                        else
                        {
                            Field_string = Field_string + " or " + table_name.Split(',')[0] + "." + sl_Field.Rows[i]["u1"].ToString() + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                        }
                    }
                    Field_string = "(" + Field_string + ")";

                    if (return_string == "")
                    {
                        return_string = "where " + Field_string;
                    }
                    else
                    {
                        return_string = return_string + " and " + Field_string;
                    }
                    #endregion
                }
                else
                {
                    #region 其它情况
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {
                        #region 参数值不为空

                        for (int j = 0; j < sl_Field.Rows.Count; j++)
                        {
                            string ziduan = "";
                            string biaoming = table_name;
                            if (sql_arr[0].IndexOf(".") > -1)
                            {
                                ziduan = sql_arr[0].Split('.')[1];
                                biaoming = sql_arr[0].Split('.')[0];
                            }
                            else
                            {
                                ziduan = sql_arr[0];
                            }

                            if (sl_Field.Rows[j]["u1"].ToString().ToLower() == ziduan.ToLower() && biaoming == sl_Field.Rows[j]["biaoming"].ToString().ToLower())
                            {
                                //  HttpContext.Current.Response.Write(j+"<br>");

                                #region 判断字段是否相等
                                string type = sl_Field.Rows[j]["u6"].ToString();
                                if (type == "数字" || type == "货币" || type == "联动" || type == "分类")
                                {
                                    #region int float类型
                                    if (return_string == "")
                                    {
                                        return_string = "where " + sql_arr[0] + " = " + HttpUtility.UrlDecode(sql_arr[1]) + "";
                                    }
                                    else
                                    {
                                        return_string = return_string + " and " + sql_arr[0] + " = " + HttpUtility.UrlDecode(sql_arr[1]) + "";
                                    }
                                    #endregion
                                }
                                else
                                {

                                    #region 非int float类型
                                    Regex reg = new Regex("<.*?>", RegexOptions.Singleline);

                                    if (reg.IsMatch(HttpUtility.UrlDecode(sql_arr[1])))
                                    {
                                        if (return_string == "")
                                        {
                                            return_string = "where " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]).Replace("<", "").Replace(">", "") + "'";
                                        }
                                        else
                                        {
                                            return_string = return_string + " and " + sql_arr[0] + " = '" + HttpUtility.UrlDecode(sql_arr[1]).Replace("<", "").Replace(">", "") + "'";
                                        }
                                    }
                                    else
                                    {
                                        if (return_string == "")
                                        {
                                            return_string = "where " + sql_arr[0] + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                                        }
                                        else
                                        {
                                            return_string = return_string + " and " + sql_arr[0] + " like '%" + HttpUtility.UrlDecode(sql_arr[1]) + "%'";
                                        }
                                    }

                                    #endregion
                                }
                                #endregion
                            }

                        }


                        #endregion
                    }
                    #endregion
                }
            }
        }
        //HttpContext.Current.Response.Write(return_string);
        //HttpContext.Current.Response.End();
        return return_string;
    }
    //从url中配置sql结束
    //获取属性所有下级
    int jj = 0;
    string fenlei = "";
    public void get_fenlei(string t1, int t2)
    {
        DataTable dt1 = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid in (" + t1 + ") ");
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (fenlei == "")
                {
                    fenlei = dt1.Rows[j]["id"].ToString();
                }
                else
                {
                    fenlei = fenlei + "," + dt1.Rows[j]["id"].ToString();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                get_fenlei(dt1.Rows[j]["id"].ToString(), tt1);
            }
        }

    }
    string sort_id = "";
    public void get_sort(string t1, int t2,string tablename,string ziduan)
    {
        DataTable dt1 =new DataTable();
        try {
             dt1 = my_c.GetTable("select id from sl_sort where " + ziduan + " in (" + t1 + ") ");
        }
        catch {
             dt1 = my_c.GetTable("select id from sl_sort where Sort_id in (" + t1 + ") ");
        }
      

        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (sort_id == "")
                {
                    sort_id = dt1.Rows[j]["id"].ToString();
                }
                else
                {
                    sort_id = sort_id + "," + dt1.Rows[j]["id"].ToString();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                get_sort(dt1.Rows[j]["id"].ToString(), tt1,tablename,ziduan);
            }
        }

    }

    //end
    //获取url中所有参数
    public string get_quer(string quer_string, string quer_type)
    {
        string return_string = "";
        string[] quer_arr = quer_string.Split('&');
        for (int i = 0; i < quer_arr.Length; i++)
        {
            if (quer_arr[i].ToString().IndexOf(quer_type + "=") > -1)
            {
                return_string = quer_arr[i].ToString().Replace(quer_type + "=", "");
            }
        }
        return return_string;
    }
    //获取url中所有参数结束

    //搜索页列表第一步
    string urlkey = "";
    public void set_search(string key)
    {

        urlkey = key;
        string table_name = "";
        int page = 1;
        try
        {
            page = int.Parse(get_quer(key, "page"));
        }
        catch
        { }
        string file_content = get_quer(key, "m");

        file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/" + file_content + ".html"), Encoding.UTF8);
        //这个地方调用了设置url的连接
        file_content = set_sitekey(file_content);


        Regex reg = new Regex(@"{fzw:search[\s\S]*?{/fzw:search}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            table_name = get_lable(match.ToString(), "table");
            string sql = get_search_sql(match.ToString(), key, table_name, page);
            //HttpContext.Current.Response.Write(sql);
            //HttpContext.Current.Response.End();
            string list_content = "";
            list_content = Regex.Replace(match.ToString(), @"{fzw:search[\s\S]*?}", "", RegexOptions.Singleline).ToString();
            list_content = Regex.Replace(list_content.ToString(), @"{/fzw:search}", "", RegexOptions.Singleline).ToString();

            set_search_list(sql, list_content, file_content, match.ToString(), page, key);


        }

    }
    //搜索页列表第一步结束






}
