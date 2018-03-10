using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Web.Mail;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class user2 : System.Web.UI.Page
{
    //采页面
    public string getWebFile(string url)
    {
        string bianma = "utf-8";
        if (bianma == "")
        {
            try
            {
                bianma = url.Split('|')[1].ToString();
            }
            catch
            { }
        }
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Split('|')[0].ToString());
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        request.Method = "GET";
        Stream receiveStream = response.GetResponseStream();
        StreamReader readStream = new StreamReader(receiveStream, Encoding.GetEncoding(bianma));
        string SourceCode = readStream.ReadToEnd();
        response.Close();
        readStream.Close();
        return SourceCode;
        return "";
    }
    public string get_json(string neirong, string type)
    {
        string[] aa = neirong.Split(',');
        for (int i = 0; i < aa.Length; i++)
        {
            Regex reg = new Regex("\".*?\"|\".*?", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(aa[i].ToString());


            // Response.End();
            if (matches[0].ToString().Replace("\"", "") == type)
            {
                if (type == "subscribe")
                {
                    return aa[i].ToString().Replace("\"", "").Replace(":", "").Replace("{", "").Replace(type, "");
                }
                else if (type == "sex")
                {
                    return aa[i].ToString().Replace("\"", "").Replace(":", "").Replace("{", "").Replace(type, "");
                }
                else
                {
                    try
                    {
                        return matches[1].ToString().Replace("\"", "");
                    }
                    catch
                    {
                        return matches[0].ToString().Replace("\"" + type + "\"", "").Replace(":", "");
                    }
                }

            }
        }

        return "";
    }
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {


            string code = "";
            try
            {
                code = Request.QueryString["code"].ToString();
            }
            catch
            {

            }
            string neirong = getWebFile("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "&code=" + code + "&grant_type=authorization_code");


            string openid = get_json(neirong, "openid");
            string access_token = get_json(neirong, "access_token");

            string jiekou = getWebFile("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "");
            string access_token1 = get_json(jiekou, "access_token");
            string userinfo = "";

            userinfo = getWebFile("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token1 + "&openid=" + openid + "&lang=zh_CN");
            if (get_json(userinfo, "subscribe") == "0")
            {
                userinfo = getWebFile("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN");
            }

            //Response.Write(userinfo);
            //Response.End();




            //   string userinfo = getWebFile("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN");
            my_b.c_cookie(openid, "openid");
            my_b.c_cookie(get_json(userinfo, "headimgurl").Replace(@"\/", "/"), "touxiang");
            string xingbie = get_json(userinfo, "sex");

            if (xingbie == "1")
            {
                xingbie = "男";
            }
            else
            {
                xingbie = "女";
            }
            my_b.c_cookie(xingbie, "xingbie");
            my_b.c_cookie(get_json(userinfo, "nickname"), "xingming");
            string subscribe = get_json(userinfo, "subscribe");

            if (subscribe == "1")
            {
                subscribe = "是";
            }
            else
            {
                subscribe = "否";
            }
            //Response.Write(subscribe);
            //Response.End();
            my_b.c_cookie(subscribe, "subscribe");

            //微信自动登录 start
            string login = "";
            try
            {
                login = ConfigurationSettings.AppSettings["login"].ToString();
            }
            catch { }

            if (login == "weixin")
            {
                DataTable dt = my_c.GetTable("select * from sl_user where openid='" + openid + "'");
                if (dt.Rows.Count == 0)
                {
                    my_c.genxin("insert into sl_user(yonghuming,mima,openid,touxiang,xingbie,xingming,guanzhu) values('" + openid + "','" + my_b.md5("123456") + "','" + openid + "','" + my_b.k_cookie("touxiang") + "','" + my_b.k_cookie("xingbie") + "','" + my_b.k_cookie("xingming") + "','" + my_b.k_cookie("subscribe") + "')");
                }

                my_b.c_cookie(openid, "user_name");
            }

            //微信自动登录 end
            string user_name = "";
            try
            {
                user_name = my_b.k_cookie("user_name");
            }
            catch
            { }

            if (user_name != "")
            {

                if (Request.QueryString["url"].ToString().IndexOf("/admin") > -1)
                {
                    DataTable dt = my_c.GetTable("select * from sl_admin where u1='" + user_name + "'");
                    if (dt.Rows.Count > 0)
                    {
                        my_c.genxin("update sl_admin set openid='" + openid + "',touxiang='" + my_b.k_cookie("touxiang") + "',guanzhu='" + subscribe + "' where u1='" + user_name + "'");
                    }
                }
                else
                {
                    DataTable dt = my_c.GetTable("select * from sl_user where yonghuming='" + user_name + "'");
                    if (dt.Rows.Count > 0)
                    {
                        my_c.genxin("update sl_user set openid='" + openid + "',touxiang='" + my_b.k_cookie("touxiang") + "',guanzhu='" + subscribe + "' where yonghuming='" + user_name + "'");
                    }
                }

            }
            else
            {
                my_b.c_cookie(openid, "openid");
            }

            Response.Redirect(my_b.tihuan(Request.QueryString["url"].ToString(), "fzw123", "&"));


        }

    }
}