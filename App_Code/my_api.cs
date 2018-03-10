using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// my_api 的摘要说明
/// </summary>
public class my_api
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public class SampleCode
    {
        //public static string test()
        //{
        //    string _timestamp = DateTime.Now.Ticks.ToString();
        //    var param = new SortedDictionary<string, string>(new AsciiComparer());
        //    param.Add("z", "ZZZ");
        //    param.Add("a", "AAA");
        //    param.Add("Z", "zzz");
        //    param.Add("A", "aaa");
        //    param.Add("2", "贰");
        //    param.Add("1", "壹");
        //    param.Add("_appid", "club");
        //    param.Add("_timestamp", _timestamp.ToString());
        //    string _sign = GetSign(param);
        //    string urlParam = string.Join("&", param.Select(i => i.Key + "=" + i.Value));
        //    string url = "http://api.demo.com/dog/add?" + urlParam + "&_sign=" + _sign;
        //    return url;
        //}
        public static string test(DataTable Model_dt, string timestamp, string appid)
        {
            var param = new SortedDictionary<string, string>(new AsciiComparer());
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                string type = Model_dt.Rows[d1]["u6"].ToString();
                if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "头像" || type == "文本域" || type == "时间框" || type == "数字" || type == "密码框" || type == "货币" || type == "编号" || type == "下拉框" || type == "单选按钮组" || type == "多选按钮组")
                {
                    
                    param.Add(Model_dt.Rows[d1]["u1"].ToString(), HttpContext.Current.Request[Model_dt.Rows[d1]["u1"].ToString()].ToString());
                }
            }
            param.Add("timestamp", timestamp);
            param.Add("appid", appid);
            string _sign = my_api.SampleCode.GetSign(param);
            return _sign;
        }
        public static string test_search(string quer_string,string timestamp, string appid)
        {
            var param = new SortedDictionary<string, string>(new AsciiComparer());
            string[] quer_arr = quer_string.Split('&');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (quer_arr[i].ToString().IndexOf("m=") == -1 && quer_arr[i].ToString().IndexOf("ordertype=") == -1 && quer_arr[i].ToString().IndexOf("orderby=") == -1 && quer_arr[i].ToString().IndexOf("number=") == -1 && quer_arr[i].ToString().IndexOf("page=") == -1 && quer_arr[i].ToString().IndexOf("lan=") == -1 && quer_arr[i].ToString().IndexOf("type=") == -1 && quer_arr[i].ToString().IndexOf("lingshi2=") == -1 && quer_arr[i].ToString().IndexOf("lingshi1=") == -1 && quer_arr[i].ToString().IndexOf("lingshi3=") == -1 && quer_arr[i].ToString().IndexOf("lingshi4=") == -1 && quer_arr[i].ToString().IndexOf("jsoncallback=") == -1 && quer_arr[i].ToString().IndexOf("t=") == -1 && quer_arr[i].ToString().IndexOf("liemingcheng=") == -1 && quer_arr[i].ToString().IndexOf("timestamp=") == -1 && quer_arr[i].ToString().IndexOf("sign=") == -1)
                {
                    string[] sql_arr = quer_arr[i].ToString().Split('=');
                    param.Add(HttpUtility.UrlDecode(sql_arr[0]), HttpUtility.UrlDecode(sql_arr[1]));
                }
            }
            param.Add("timestamp", timestamp);
            param.Add("appid", appid);
            string _sign = my_api.SampleCode.GetSign(param);
            return _sign;
        }
        public static  string GetSign(SortedDictionary<string, string> paramList, string appKey = "test")
        {
            paramList.Remove("_sign");
            StringBuilder sb = new StringBuilder(appKey);
            foreach (var p in paramList)
                sb.Append(p.Key).Append(p.Value);
            sb.Append(appKey);
            return GetMD5(sb.ToString());
        }
        public static string GetMD5(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            var sb = new StringBuilder(32);
            var md5 = System.Security.Cryptography.MD5.Create();
            var output = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            for (int i = 0; i < output.Length; i++)
                sb.Append(output[i].ToString("X").PadLeft(2, '0'));
            return sb.ToString();
        }
    }
    /// <summary>
    /// 基于ASCII码排序规则的String比较器
    /// Author：HeDaHong
    /// </summary>
    public class AsciiComparer : System.Collections.Generic.IComparer<string>
    {
        public int Compare(string a, string b)
        {
            if (a == b)
                return 0;
            else if (string.IsNullOrEmpty(a))
                return -1;
            else if (string.IsNullOrEmpty(b))
                return 1;
            if (a.Length <= b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] < b[i])
                        return -1;
                    else if (a[i] > b[i])
                        return 1;
                    else
                        continue;
                }
                return a.Length == b.Length ? 0 : -1;
            }
            else
            {
                for (int i = 0; i < b.Length; i++)
                {
                    if (a[i] < b[i])
                        return -1;
                    else if (a[i] > b[i])
                        return 1;
                    else
                        continue;
                }
                return 1;
            }
        }
    }
}