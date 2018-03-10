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

public partial class callback : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
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


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string appid = "wxb0bd01360a283939";
            string secret = "4076fd59963c72517eff7e2ac237465b";

            string code = "";
            try
            {
                code = Request.QueryString["code"].ToString();
            }
            catch
            {

            }
            string neirong = getWebFile("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code");

          
            string openid = get_json(neirong, "openid");
            string access_token = get_json(neirong, "access_token");
         
            string jiekou = getWebFile("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "");
            string access_token1 = get_json(jiekou, "access_token");
            string userinfo = "";
            if (access_token1 == "")
            {
                userinfo = getWebFile("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN");
            }
            else
            {
                userinfo = getWebFile("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token1 + "&openid=" + openid + "&lang=zh_CN");
            }

          




            //   string userinfo = getWebFile("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN");
   
           
            string xingbie = get_json(userinfo, "sex");

            if (xingbie == "1")
            {
                xingbie = "男";
            }
            else
            {
                xingbie = "女";
            }
      
       
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
          //  my_b.c_cookie(subscribe, "subscribe");


            string yonghuming = openid;
            string nickname = get_json(userinfo, "nickname");
            string figureurl = get_json(userinfo, "headimgurl").Replace(@"\/", "/");
            string suozaidi = get_json(userinfo, "province")+ get_json(userinfo, "city");

            //跳转 start
           
            Response.Redirect("/inc/set_login.aspx?type=weixin&nickname=" + nickname + "&yonghuming=" + yonghuming + "&figureurl=" + figureurl + "&suozaidi=" + suozaidi + "&xingbie=" + xingbie + "");


        }

    }
}