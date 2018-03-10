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
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //获取access_token接口
            string neirong = getWebFile("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationSettings.AppSettings["appid"].ToString() + "&secret=" + ConfigurationSettings.AppSettings["AppSecret"].ToString() + "");
            string access_token = get_json(neirong, "access_token");
            //获取access_token接口成功

            string mobanneirong = File.ReadAllText(Server.MapPath("/inc/template/"+Request.QueryString["moban"].ToString() +".txt"), Encoding.Default);
            string tablename = Request.QueryString["tablename"].ToString();
            string openid = Request.QueryString["openid"].ToString();
            string yonghuming = Request.QueryString["yonghuming"].ToString();
            string wherestrig = Request.QueryString["wherestrig"].ToString();
            DataTable dt = my_c.GetTable("select top 1 * from " + tablename + " " + wherestrig + " order by id desc");
            //替换
            mobanneirong = mobanneirong.Replace("{fzw:database:openid/}", openid);
            mobanneirong = mobanneirong.Replace("{fzw:database:yonghuming/}", yonghuming);
            mobanneirong = my_b.set_neirong(dt, mobanneirong);

            string fanhui = GetResponseData(mobanneirong, "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + access_token);
            Response.Write(fanhui);

        }




    }
}