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
using System.Collections.Generic;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Newtonsoft.Json;
using LitJson;
public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true; //总是接受     
    }

    public string get_jsonlist(string neirong, string type, string itemname)
    {
        string t1 = "";
        //*** 读取JSON字符串中的数据 *******************************            
        JsonData jd = JsonMapper.ToObject(neirong);
        JsonData jdItems = jd[type];
        if (itemname == "")
        {
            return jdItems.ToString();
        }
        else
        {

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
                    t1 = t1 + "{fzw123}" + item[itemname].ToString();
                }
            }

            return t1;
        }

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
    public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
    {
        HttpWebRequest request = null;
        //HTTPSQ请求  
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        request = WebRequest.Create(url) as HttpWebRequest;
        request.ProtocolVersion = HttpVersion.Version10;
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.UserAgent = DefaultUserAgent;
        //如果需要POST数据     
        if (!(parameters == null || parameters.Count == 0))
        {
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in parameters.Keys)
            {
                if (i > 0)
                {
                    buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, parameters[key]);
                }
                i++;
            }
            byte[] data = charset.GetBytes(buffer.ToString());
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }
        return request.GetResponse() as HttpWebResponse;
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        string url = "http://poll.kuaidi100.com/poll/query.do";
        Encoding encoding = Encoding.GetEncoding("utf-8");

        //参数
        String str = "";
        try
        {
            str = Request.QueryString["nu"].ToString();
        }
        catch { }
        String com = "";
        try
        {
            com = Request.QueryString["com"].ToString();
        }
        catch { }
        if (com != "")
        {
            DataTable sl_Parameter = my_c.GetTable("select * from sl_Parameter where classid=169 and u1='" + com + "'");
            if (sl_Parameter.Rows.Count > 0)
            {
                com = sl_Parameter.Rows[0]["u2"].ToString();
            }
        }
        if (str != "")
        {
            //start
            try
            {
                if (com == "")
                {
                    com = my_b.getWebFile("http://www.kuaidi100.com/autonumber/auto?num=" + str + "&key=JyEpyFQz2312");
                    com = get_neirong(com, "comCode");
                }

                String param = "{\"com\":\"" + com + "\",\"num\":\"" + str + "\",\"from\":\"\",\"to\":\"\"}";
                //Response.Write(param);
                //Response.End();
                String customer = "A6B0BD6E2F1C7778635A2DAD7B6AB6BE";
                String key = "JyEpyFQz2312";
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] InBytes = Encoding.GetEncoding("UTF-8").GetBytes(param + key + customer);
                byte[] OutBytes = md5.ComputeHash(InBytes);
                string OutString = "";
                for (int i = 0; i < OutBytes.Length; i++)
                {
                    OutString += OutBytes[i].ToString("x2");
                }
                String sign = OutString.ToUpper();
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("param", param);
                parameters.Add("customer", customer);
                parameters.Add("sign", sign);
                HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
                //打印返回值  
                Stream stream = response.GetResponseStream();   //获取响应的字符串流  
                StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
                string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  

                string time1 = get_jsonlist(html, "data", "time");
                string ftime = get_jsonlist(html, "data", "ftime");
                string context = get_jsonlist(html, "data", "context");

                Regex reg = new Regex("{fzw123}", RegexOptions.Singleline);
                string[] time1_ = reg.Split(time1);
                string[] ftime_ = reg.Split(ftime);
                string[] context_ = reg.Split(context);
                string shuchu = "";
                for (int i = 0; i < time1_.Length; i++)
                {
                    shuchu = shuchu + "<tr><td width=\"40%\">" + time1_[i] + "</td><td>" + context_[i] + "</td></tr> ";

                }
                shuchu = "<table class=\"table-pad\">" + shuchu + "</table>";
                HttpContext.Current.Response.Write(shuchu);
            }
            catch { }
            //end
        }

        //end
    }

}