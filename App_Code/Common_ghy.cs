using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Common_ghy 的摘要说明
/// </summary>
public class Common_ghy
{

    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();

    System.Data.DataTable dt;
	public Common_ghy()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 自定义的返回json的函数,只返回一条和一个字段
    /// </summary>
    /// <param name="str1">需要返回的第一个值</param>
    /// <param name="callbackName">传过来的回调函数的名字</param>
    /// <param name="context"></param>
    public void my_jsonDataReturnOneString(string str1, string callbackName, HttpContext context)
    {
        string temp_json_str = "";
        //get_foot([{"str1":""}])    {"str1":""}
        temp_json_str = "{ \"str1\":" + "\"" + str1 + "\"" + "}";
        temp_json_str = callbackName + "([" + temp_json_str + "])";

        context.Response.ContentType = "application/json";
        
        context.Response.Write(temp_json_str);
        context.Response.End();
    }

    /// <summary>
    /// 得到当前服务器的域名
    /// </summary>
    /// <returns></returns>
    public string getUrl()
    {
        string Url_Name = HttpContext.Current.Request.Url.Host.ToString();
        Url_Name = "http://" + Url_Name;
        return Url_Name;
    }

    /// <summary>
    /// 将用户输入的信息转换成MD5字节码 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public  string getMd5Hash(string input)
    {
        MD5 md5Hasher = MD5.Create();
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }


    /// <summary>
    /// 将查询得到的数据以json的形式输出
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="sql"></param>
    /// <param name="context"></param>
    public void OutJson( string sql, string callback)
    {
       
        DataTable dt = new DataTable();
        string json_str = "";
     
        dt = my_c.GetTable(sql);
        json_str = DataTableToJson(json_str, dt);
        json_str = callback + "(" + json_str + ")";

        //当视频地址是据对地址时，不替换
        string a = json_str;
        string b = "'shipindizhi':'http:";
        if (a.IndexOf(b) > -1)
        {

        }
        else
        {
            json_str = json_str.Replace("/upfile/", getUrl() + "/upfile/");//将之前的相对路径，换成局对路径

        }

        HttpContext.Current.Response.ContentType = "application/json";
        HttpContext.Current.Response.Write(json_str);
        HttpContext.Current.Response.End();

    }
    /// <summary>
    /// 将datatable生成json
    /// </summary>
    /// <param name="jsonName">返回的string</param>
    /// <param name="dt">待转换的dt</param>
    /// <returns></returns>
    public static string DataTableToJson(string jsonName, DataTable dt)
    {
        StringBuilder Json = new StringBuilder();
        Json.Append("[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]");
        return Json.ToString();
    }

    public string re_string(string str,HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.Request.QueryString["\""+str+"\""]))
        {
            str = my_b.c_string(context.Request.QueryString["\"" + str + "\""]);
            return str;
        }
        else
            return "";

    }


    public string[] no_chongfu(string[] zj_id)
    {
        System.Collections.ArrayList al = new System.Collections.ArrayList();
        for (int j = 0; j < zj_id.Length; j++)
        {

            //判断是否已经存在
            if (al.Contains(zj_id[j]) == false)
            {
                al.Add(zj_id[j]);
            }

        }

        zj_id = new String[al.Count];
        zj_id = (string[])al.ToArray(typeof(string));
        return zj_id;
    }

    // <summary>
    /// 自动产生一个ID号
    /// </summary>
    /// <returns></returns>
    public  string createID()
    {
        return DateTime.Now.ToString("yyyyMMddhhmmss");
    }


    /// <summary>
    /// 返回表里面一个字段的值
    /// </summary>
    /// <param name="table">表命</param>
    /// <param name="key_re">返回的字段名</param>
    /// <param name="key_query"></param>
    /// <param name="value_query"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string get_table_param(string table, string key_re, string key_query, string value_query, HttpContext context)
    {
        string temp_sql = "";
        temp_sql = "select top 1 " + key_re + " from sl_" + table;
        
        if (!string.IsNullOrEmpty(key_query))
        {
            temp_sql = temp_sql + " where " + key_query + "='" + value_query+"'";
        }

        temp_sql += " order by id asc";
        //HttpContext.Current.Response.Write(temp_sql);
        //HttpContext.Current.Response.End();

        dt = my_c.GetTable(temp_sql);
        if (dt.Rows.Count > 0)
        {
            key_re = dt.Rows[0][key_re].ToString();
        }
        else
        {
            //my_jsonDataReturnOneString("err", "tip", context);
        }
        return key_re;

    }


/// <summary>
    /// 修改里面一个字段的值
/// </summary>
/// <param name="table">表命</param>
/// <param name="key_eidt">要修改的字段</param>
/// <param name="value_edit">需要修改的字段的值</param>
/// <param name="key_query">查询字段</param>
/// <param name="value_query">查询字段的值</param>
/// <param name="context"></param>
/// <returns></returns>
    public string edit_table_param(string table, string key_eidt,string value_edit, string key_query, string value_query, HttpContext context)
    {
        if (key_eidt == "mima")
        {
            return "no_access"; ;
        }
        string temp_sql = "";

        temp_sql = "update sl_" + table + " set " + key_eidt + "='" + value_edit + "' where " + key_query + "='" + value_query + "'";

        my_c.genxin(temp_sql);
        
        return "edit_success";

    }



    /// <summary>
    /// 自定义的返回json的函数,返回一条和多个字段
    /// </summary>
    /// <param name="str1">需要返回的第一个值</param>
    /// <param name="callbackName">传过来的回调函数的名字</param>
    /// <param name="context"></param>
    public void my_jsonDataReturnOneString(string[] str, string callbackName,HttpContext context)
    {
        string temp_json_str = "";
        //get_foot([{"str1":""}])    {"str1":""}
        for (int i = 0; i < str.Length; i++)
        {
            if (i == 0)
            {
                temp_json_str += "\"str" + i + "\":" + "\"" + str[i] + "\"" + "";
            }
            else
            {
                temp_json_str += ", \"str" + i + "\":" + "\"" + str[i] + "\"" + "";
            }

        }

        temp_json_str = callbackName + "([{" + temp_json_str + "}])";

        context.Response.ContentType = "application/json";

        context.Response.Write(temp_json_str);
        context.Response.End();
    }


    //去除HTML
    public string NoHTML(string Htmlstring)
    {

        //删除脚本   
        Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        //删除HTML   
        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "mdash;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "amp;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "rdquo;", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "ldquo;", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "mdash;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "hellip;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&.*?;", "", RegexOptions.IgnoreCase);
        Htmlstring = System.Web.HttpContext.Current.Server.HtmlEncode(Htmlstring.Trim());
        Htmlstring.Replace("<", "");
        Htmlstring.Replace(">", "");
        Htmlstring.Replace("\r\n", "");
        Htmlstring = Htmlstring.Replace("&amp;", "");

        return Htmlstring;
    }

    /// <summary>
    /// 自定义的返回json的函数,返回一条和多个字段
    /// </summary>
    /// <param name="str1">需要返回的第一个值</param>
    /// <param name="callbackName">传过来的回调函数的名字</param>
    /// <param name="context"></param>
    public void my_jsonDataReturnAllString(string[] str, string callbackName,HttpContext context)
    {
        string temp_json_str = "";
        //get_foot([{"str1":""}])    {"str1":""}
        for (int i = 0; i < str.Length ; i++)
        {
            if (str[i]==null || str[i]=="")
            {
                break;
            }
            if (i == 0)
            {
                temp_json_str += "\"str" + i + "\":" + "\"" + str[i] + "\"" + "";
            }
            else
            {
                temp_json_str += ", \"str" + i + "\":" + "\"" + str[i] + "\"" + "";
            }

        }

        temp_json_str = callbackName + "([{" + temp_json_str + "}])";

        context.Response.ContentType = "application/json";

        context.Response.Write(temp_json_str);
        context.Response.End();
    }

    /// <summary>
    /// 返回查询结果的一行没有html的json
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public string[] re_table_detail_array(string sql)
    {
        string[] table = new string[100];
        dt = my_c.GetTable(sql.ToString());

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string temp_str = "";
                    temp_str = dt.Rows[i][j].ToString();
                    table[j] = NoHTML(temp_str);

                }

            }
        }

        return table;
    }



   
    /// <summary>
    /// 处理一个页面返回多个同样回调函数名json的文题
    /// </summary>
    /// <param name="all_value">callback 的全部值</param>
    /// <param name="sub_value"> 需要的callback的值</param>
    /// <returns></returns>
    public string get_callback(string all_value,string sub_value)
    {
        if (all_value.IndexOf(sub_value) > -1)
        {
            return sub_value;
        }
        return all_value;
    }

}