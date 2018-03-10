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
using LitJson;

public partial class user1 : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    other_sql othersql = new other_sql();
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
    public string get_jsonlist(string neirong, string type,string itemname)
    {
        string t1 = "";
        //*** 读取JSON字符串中的数据 *******************************            
        JsonData jd = JsonMapper.ToObject(neirong);
        JsonData jdItems = jd[type];
        int itemCnt = jdItems.Count;
        // 数组 items 中项的数量 
        foreach (JsonData item in jdItems)
        // 遍历数组 items             
        {
            if (t1 == "")
            {
                t1 = item[itemname].ToString();
            }
            else
            {
                t1 = t1+"|"+item[itemname].ToString();
            }
        }

        return t1;
    }
    public string get_neirong(string neirong, string type)
    {
        string[] aa = neirong.Split(',');
        for (int i = 0; i < aa.Length; i++)
        {
            Regex reg = new Regex("\".*?\"|\".*?", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(aa[i].ToString());


            if (matches[0].ToString().Replace("\"", "") == type)
            {
                try
                {
                    return matches[1].ToString().Replace("\"", "").Trim();
                }
                catch
                {
                    return matches[0].ToString().Replace("\"" + type + "\"", "").Replace(":", "").Trim();
                }
            }
        }

        return "";
    }
    public string get_json(string neirong, string type)
    {
     
        //*** 读取JSON字符串中的数据 *******************************            
        JsonData jd = JsonMapper.ToObject(neirong);
        String name = (String)jd[type];
        return name;
    }
    /// <summary>
    /// 返回JSon数据
    /// </summary>
    /// <param name="JSONData">要处理的JSON数据</param>
    /// <param name="Url">要提交的URL</param>
    /// <returns>返回的JSON处理字符串</returns>
    public string GetResponseData(string JSONData, string Url)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        request.Method = "POST";
        request.ContentLength = bytes.Length;
        request.ContentType = "text/xml";
        Stream reqstream = request.GetRequestStream();
        reqstream.Write(bytes, 0, bytes.Length);

        //声明一个HttpWebRequest请求
        request.Timeout = 90000;
        //设置连接超时时间
        request.Headers.Set("Pragma", "no-cache");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream streamReceive = response.GetResponseStream();
        Encoding encoding = Encoding.UTF8;

        StreamReader streamReader = new StreamReader(streamReceive, encoding);
        string strResult = streamReader.ReadToEnd();
        streamReceive.Dispose();
        streamReader.Dispose();

        return strResult;
    }
    //获取用户已领取的卡券
    public string user_search_card()
    {
        //获取access_token接口
        string neirong = getWebFile("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "");
        string access_token = get_json(neirong, "access_token");
        //获取access_token接口成功
        string openid = Request.QueryString["openid"].ToString();
        string card_id = Request.QueryString["card_id"].ToString();
        string fanhui = GetResponseData("{  \"openid\": \"" + openid + "\",  \"card_id\": \"" + card_id + "\"}        ", "https://api.weixin.qq.com/card/user/getcardlist?access_token=" + access_token);
        //return get_json(fanhui,"errmsg")+"|"+ get_jsonlist(fanhui, "card_list", "code");
        string huiyuanka = "";
        try
        {
            huiyuanka=get_jsonlist(fanhui, "card_list", "code");
        }
        catch { }
        if (huiyuanka != "")
        {
            string xinkahao = "";
            DataTable dt1 = othersql.GetTable("select * from V_CUSTOMER where SJ='" + my_b.k_cookie("user_name") + "'");
            if (dt1.Rows.Count > 0)
            {
                string DM = dt1.Rows[0]["DM"].ToString();
                DataTable dt2 = othersql.GetTable("select * from V_VIPSET where GKDM='" + DM + "'");
                if (dt2.Rows.Count > 0)
                {
                    xinkahao = dt2.Rows[0]["DM"].ToString();
                    //网站
                    my_c.genxin("update sl_user set huiyuanka='" + xinkahao + "' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                    //end
                    //修改卡号
                    string jihuo_fanhui = GetResponseData("{  \"init_bonus\": 100,  \"init_balance\": 200,  \"membership_number\": \"" + xinkahao + "\",  \"code\": \"" + huiyuanka + "\",  \"card_id\": \"" + card_id + "\",  \"init_custom_field_value1\": \"xxxxx\" }", "https://api.weixin.qq.com/card/membercard/activate?access_token=" + access_token);


                    //end
                }
            }

        }

        
       
        return get_json(fanhui, "errmsg") ;
    }
    //end
    //导入code接口
    public string card_dao_code()
    {
        //获取access_token接口
        string neirong = getWebFile("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "");
        string access_token = get_json(neirong, "access_token");
        //获取access_token接口成功
        string card_id = "pkrsVt3t7uBIa9dz-08m6VghtaGg";
        try
        {
            card_id = Request.QueryString["card_id"].ToString();
        }
        catch { }
        string fanhui = GetResponseData("{   \"card_id\": \""+ card_id + "\",   \"code\": [       \"11111\",       \"22222\",       \"33333\",       \"44444\",       \"55555\"   ]}", "http://api.weixin.qq.com/card/code/deposit?access_token=" + access_token);
        return fanhui;
    }
    //end
    //图文消息群发卡券
    public string code_qufa()
    {
        //获取access_token接口
        string neirong = getWebFile("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "");
        string access_token = get_json(neirong, "access_token");
        //获取access_token接口成功
        string card_id = "pkrsVt3t7uBIa9dz-08m6VghtaGg";
        try
        {
            card_id = Request.QueryString["card_id"].ToString();
        }
        catch { }
        string fanhui = GetResponseData("{   \"card_id\": \"" + card_id + "\"}", "https://api.weixin.qq.com/card/mpnews/gethtml?access_token=" + access_token);
        string errmsg= get_json(fanhui, "errmsg");
        File.WriteAllText(Server.MapPath("/a.txt"), fanhui);
        if (errmsg.Trim() == "ok")
        {
            Regex reg = new Regex("data-src=\".*?\"", RegexOptions.Singleline);

            Match matches = reg.Match(fanhui.Replace(@"\",""));
            string content = matches.ToString();
            content = content.Replace("data-src=", "");
            content = content.Replace("\"", "");
            return "<a href='"+ content + "'>"+ content + "</a>";
        }
        else
        {
            return "";
        }
       
    }
    //end
    //卡券接口激活
    public string code_jihuo()
    {
      
        //获取access_token接口
        string neirong = getWebFile("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "");
        string access_token = get_json(neirong, "access_token");
        //获取access_token接口成功
        string card_id = "pkrsVt3t7uBIa9dz-08m6VghtaGg";
        try
        {
            card_id = Request.QueryString["card_id"].ToString();
        }
        catch { }
        string fanhui = GetResponseData("{  \"init_bonus\": 100,  \"init_balance\": 200,  \"membership_number\": \"AAA00000001\",  \"code\": \"12312313\",  \"card_id\": \"" + card_id + "\",  \"init_custom_field_value1\": \"xxxxx\" }", "https://api.weixin.qq.com/card/membercard/activate?access_token=" + access_token);
        return fanhui;

    }
    //end
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //string type = "";
            //try
            //{
            //    type = Request.QueryString["type"].ToString();
            //}
            //catch { }
            //if (type == "user_search_card")
            //{
            //    Response.Write(user_search_card());
            //}
            //else if (type == "card_dao_code")
            //{
            //    Response.Write(card_dao_code());
            //}
            //else
            //{
            //    // Response.Write(code_qufa());
            //    //Response.Write(get_jsonlist(" {\"errcode\":0,\"errmsg\":\"ok\",\"card_list\": [      { \"code\": \"xxx1434079154\", \"card_id\": \"xxxxxxxxxx\"},      { \"code\": \"xxx1434079155\", \"card_id\": \"xxxxxxxxxx\"}      ]}            ", "card_list", "code"));
            //    Response.Write(code_jihuo());
            //}

            Response.Write(getWebFile("http://apis.map.qq.com/ws/location/v1/ip?ip=" + Server.HtmlEncode("61.135.17.68") + "&key="+Server.HtmlEncode("OCDBZ-LEUHI-T5TGX-5BIA7-OJSIF-LNBDP") +""));
        }




    }
}