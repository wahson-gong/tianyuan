using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using LitJson;
using System.Text.RegularExpressions;
using System.Text;
/// <summary>
///my_conn 的摘要说明
/// </summary>
public class my_json
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_hanshu my_hs = new my_hanshu();
    #region 链接转化成json
    public string url_json(string quer_string)
    {
        string t1 = "{";
        string[] quer_arr = quer_string.Split('&');
        for (int i = 0; i < quer_arr.Length; i++)
        {
            if (quer_arr[i].ToString().IndexOf("y=") == -1 && quer_arr[i].ToString().IndexOf("x=") == -1)
            {
                string[] sql_arr = quer_arr[i].ToString().Split('=');
                if (i == 0)
                {
                    t1 = t1 + "\"" + sql_arr[0] + "\":\"" + sql_arr[1] + "\"";
                }
                else
                {
                    t1 = t1+",\"" + sql_arr[0] + "\":\"" + sql_arr[1] + "\"";
                }
            }
        }
        t1 = t1 + "}";
        return t1;
    }
    #endregion
    //获取json内容,neirong  json字符串内容,type 其它一个值
    public string get_json(string neirong, string type)
    {
        string[] aa = neirong.Split(',');
        for (int i = 0; i < aa.Length; i++)
        {
            Regex reg = new Regex("\".*?\"|\".*?", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(aa[i].ToString());
      
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
    //获取json列表值,neirong  json字符串内容,type 其它一个值,itemname列表名
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
      
      
        string ContentStr1 = jdItems.ToJson().ToString();
        ContentStr1 = Regex.Unescape(ContentStr1);
    
        int itemCnt = jdItems.Count;
        // 数组 items 中项的数量 

        try
        {
            foreach (JsonData item in jdItems)
            // 遍历数组 items             
            {
                if (t1 == "")
                {
                    t1 = item[itemname].ToString();
                }
                else
                {
                    t1 = t1 + "|" + item[itemname].ToString();
                }
            }
        }
        catch
        {
            t1 = ContentStr1;
        }

        return t1;
    }
    /// <summary>
    /// 将查询得到的数据以json的形式输出
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="sql"></param>
    /// <param name="context"></param>
    public string OutJson(string sql, string callback,string sql1,DataTable sl_Field,DataTable Field_dt,string type)
    {
        DataTable dt = new DataTable();
        string json_str = "";
        biaotou="";
        dt = my_c.GetTable(sql);
        get_biaotou(dt);
        //HttpContext.Current.Response.Write(biaotou);
        //HttpContext.Current.Response.End();
        json_str = DataTableToJson(json_str, dt, sl_Field, Field_dt);
        //HttpContext.Current.Response.Write(json_str);
        //HttpContext.Current.Response.End();
        string shu = "";
        if (sql1== "")
        {
            shu = dt.Rows.Count.ToString();
        }
        else
        {
            shu = my_c.GetTable(sql1).Rows[0]["count_id"].ToString();
        }

        if (type == "order_list")
        {
            #region type=order_list
            StringBuilder zuodan = new StringBuilder();
            StringBuilder shenhe = new StringBuilder();
            StringBuilder daikuanliucheng = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region 做单记录
                DataTable sl_zuodan = my_c.GetTable("select * from sl_zuodan where qiandan in (" + dt.Rows[i]["id"].ToString() + ")  and shanchuzhuangtai>-1");
 
                #region 关键词权限
                string  biaoming = "sl_guanjianciquanxian.biaoming='sl_zuodan'";
                string Field_biaoming = "u1='sl_zuodan'";
                DataTable sl_Field_ = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                DataTable Field_dt_ = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //HttpContext.Current.Response.Write("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                //HttpContext.Current.Response.End();
                #endregion
              
                string zhi = yitiaoToJson("", sl_zuodan, sl_Field_, Field_dt_);
                if (zhi.Replace("[]", "") != "")
                {
                    if (zuodan.ToString() != "") zuodan.Append(",");
                    zuodan.Append(zhi);
                  
                }
                #endregion
                #region 审核记录
              DataTable  sl_shenhe = my_c.GetTable("select * from sl_shenhe where laiyuanbianhao in (" + dt.Rows[i]["id"].ToString() + ") and xianshi='是' and datediff(DAY, daoqishijian ,getdate()) <=1");

                #region 关键词权限
                 biaoming = "sl_guanjianciquanxian.biaoming='sl_shenhe'";
                 Field_biaoming = "u1='sl_shenhe'";
                 sl_Field_ = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                 Field_dt_ = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //HttpContext.Current.Response.Write("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                //HttpContext.Current.Response.End();
                #endregion

                 zhi = yitiaoToJson("", sl_shenhe, sl_Field_, Field_dt_);
                if (zhi.Replace("[]", "") != "")
                {
                    if (shenhe.ToString() != "") shenhe.Append(",");
                    shenhe.Append(zhi);

                }
                #endregion
                #region 贷款记录
                DataTable sl_daikuanliucheng = my_c.GetTable("select top 1 * from sl_daikuanliucheng where laiyuanbianhao in (" + dt.Rows[i]["id"].ToString() + ")   and shanchuzhuangtai>-1 order by id desc");

                #region 关键词权限
                biaoming = "sl_guanjianciquanxian.biaoming='sl_daikuanliucheng'";
                Field_biaoming = "u1='sl_daikuanliucheng'";
                sl_Field_ = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                Field_dt_ = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //HttpContext.Current.Response.Write("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                //HttpContext.Current.Response.End();
                #endregion

                zhi = yitiaoToJson("", sl_daikuanliucheng, sl_Field_, Field_dt_);
                if (zhi.Replace("[]", "") != "")
                {
                    if (daikuanliucheng.ToString() != "") daikuanliucheng.Append(",");
                    daikuanliucheng.Append(zhi);

                }
                #endregion
            }
            if (json_str == "")
            {
                json_str = "[]";
            }
            string zuodan_ = "";
            if (zuodan.ToString() == "")
            {
                zuodan.Append("[]");
                zuodan_ = zuodan.ToString();
            }
            else
            {
                zuodan_ = "["+ zuodan.ToString() + "]";
            }
            //if (shenhe.ToString() == "")
            //{
            //    shenhe.Append("[]");
            //    //HttpContext.Current.Response.Write(shenhe);
            //    //HttpContext.Current.Response.End();
            //}
            string shenhe_ = "";
            if (shenhe.ToString() == "")
            {
                shenhe.Append("[]");
                shenhe_ = shenhe.ToString();
            }
            else
            {
                shenhe_ = "[" + shenhe.ToString() + "]";
            }
            string daikuanliucheng_ = "";
            if (daikuanliucheng.ToString() == "")
            {
                daikuanliucheng.Append("[]");
                daikuanliucheng_ = daikuanliucheng.ToString();
            }
            else
            {
                daikuanliucheng_ = "[" + daikuanliucheng.ToString() + "]";
            }
            
            json_str = callback + "({\"list\":" + json_str + ",\"count\":[{\"shu\":\"" + shu + "\"}],\"biaotou\":[" + biaotou + "],\"zuodan\":" + zuodan_ + ",\"shenhe\":" + shenhe_ + ",\"daikuanliucheng\":" + daikuanliucheng_ + "})";
            return json_str;
            #endregion
        }
        else if (type == "order_list_show")
        {
            #region type=order_list_show
            StringBuilder zuodan = new StringBuilder();
            StringBuilder daikuanliucheng = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region 做单记录
                DataTable sl_zuodan = my_c.GetTable("select * from sl_zuodan where qiandan in (" + dt.Rows[i]["id"].ToString() + ") and shanchuzhuangtai>-1");

                #region 关键词权限
                string biaoming = "sl_guanjianciquanxian.biaoming='sl_zuodan'";
                string Field_biaoming = "u1='sl_zuodan'";
                DataTable sl_Field_ = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                DataTable Field_dt_ = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //HttpContext.Current.Response.Write("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                //HttpContext.Current.Response.End();
                #endregion

                string zhi = yitiaoToJson("", sl_zuodan, sl_Field_, Field_dt_);
                if (zhi.Replace("[]", "") != "")
                {
                    if (zuodan.ToString() != "") zuodan.Append(",");
                    zuodan.Append(zhi);

                }
                #endregion

                #region 签单流程
                DataTable sl_daikuanliucheng = my_c.GetTable("select * from sl_daikuanliucheng where laiyuanbianhao in (" + dt.Rows[i]["id"].ToString() + ") and shanchuzhuangtai>-1");

                #region 关键词权限
                biaoming = "sl_guanjianciquanxian.biaoming='sl_daikuanliucheng'";
                Field_biaoming = "u1='sl_daikuanliucheng'";
                sl_Field_ = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                Field_dt_ = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //HttpContext.Current.Response.Write("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                //HttpContext.Current.Response.End();
                #endregion

                zhi = yitiaoToJson("", sl_daikuanliucheng, sl_Field_, Field_dt_);
                if (zhi.Replace("[]", "") != "")
                {
                    if (daikuanliucheng.ToString() != "") daikuanliucheng.Append(",");
                    daikuanliucheng.Append(zhi);

                }
                #endregion
            }
            //HttpContext.Current.Response.Write(zuodan);
            //HttpContext.Current.Response.End();
            string zuodan_ = "";
            if (zuodan.ToString() == "")
            {
                zuodan.Append("[]");
                zuodan_ = zuodan.ToString();
            }
            else
            {
                zuodan_ = "[" + zuodan.ToString() + "]";
            }
            string daikuanliucheng_ = "";
            if (daikuanliucheng.ToString() == "")
            {
                daikuanliucheng.Append("[]");
                daikuanliucheng_ = daikuanliucheng.ToString();
            }
            else
            {
                daikuanliucheng_ = "[" + daikuanliucheng.ToString() + "]";
            }
            //if (daikuanliucheng.ToString() == "")
            //{
            //    daikuanliucheng.Append("[]");
            //}
            if (json_str == "")
            {
                json_str = "[]";
            }
            json_str = callback + "({\"list\":" + json_str + ",\"count\":[{\"shu\":\"" + shu + "\"}],\"biaotou\":[" + biaotou + "],\"zuodan\":" + zuodan_ + ",\"daikuanliucheng\":" + daikuanliucheng_ + "})";
            return json_str;
            #endregion
        }
        else if (type == "zuodanjiesuan")
        {
            #region type=zuodanjiesuan
            StringBuilder shenhe = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                #region 审核记录
                DataTable sl_shenhe = my_c.GetTable("select * from sl_shenhe where laiyuanbianhao in (" + dt.Rows[i]["id"].ToString() + ") and xianshi='是' and datediff(DAY, daoqishijian ,getdate()) <=1");

                #region 关键词权限
               string biaoming = "sl_guanjianciquanxian.biaoming='sl_shenhe'";
              string  Field_biaoming = "u1='sl_shenhe'";
               DataTable sl_Field_ = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                DataTable Field_dt_ = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //HttpContext.Current.Response.Write("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                //HttpContext.Current.Response.End();
                #endregion

               string zhi = yitiaoToJson("", sl_shenhe, sl_Field_, Field_dt_);
                if (zhi.Replace("[]", "") != "")
                {
                    if (shenhe.ToString() != "") shenhe.Append(",");
                    shenhe.Append(zhi);

                }
                #endregion

            }
            if (json_str == "")
            {
                json_str = "[]";
            }
          
            //if (shenhe.ToString() == "")
            //{
            //    shenhe.Append("[]");
            //    //HttpContext.Current.Response.Write(shenhe);
            //    //HttpContext.Current.Response.End();
            //}
            string shenhe_ = "";
            if (shenhe.ToString() == "")
            {
                shenhe.Append("[]");
                shenhe_ = shenhe.ToString();
            }
            else
            {
                shenhe_ = "[" + shenhe.ToString() + "]";
            }
            json_str = callback + "({\"list\":" + json_str + ",\"count\":[{\"shu\":\"" + shu + "\"}],\"biaotou\":[" + biaotou + "],\"shenhe\":" + shenhe_ + "})";
            return json_str;
            #endregion
        }
        else if (type == "qiandanjiesuan")
        {
            #region type=qiandanjiesuan
            StringBuilder shenhe = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                #region 审核记录
                DataTable sl_shenhe = my_c.GetTable("select * from sl_shenhe where laiyuanbianhao in (" + dt.Rows[i]["id"].ToString() + ") and xianshi='是' and datediff(DAY, daoqishijian ,getdate()) <=1");

                #region 关键词权限
                string biaoming = "sl_guanjianciquanxian.biaoming='sl_shenhe'";
                string Field_biaoming = "u1='sl_shenhe'";
                DataTable sl_Field_ = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                DataTable Field_dt_ = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //HttpContext.Current.Response.Write("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");
                //HttpContext.Current.Response.End();
                #endregion

                string zhi = yitiaoToJson("", sl_shenhe, sl_Field_, Field_dt_);
                if (zhi.Replace("[]", "") != "")
                {
                    if (shenhe.ToString() != "") shenhe.Append(",");
                    shenhe.Append(zhi);

                }
                #endregion

            }
            if (json_str == "")
            {
                json_str = "[]";
            }

            //if (shenhe.ToString() == "")
            //{
            //    shenhe.Append("[]");
            //    //HttpContext.Current.Response.Write(shenhe);
            //    //HttpContext.Current.Response.End();
            //}
            string shenhe_ = "";
            if (shenhe.ToString() == "")
            {
                shenhe.Append("[]");
                shenhe_ = shenhe.ToString();
            }
            else
            {
                shenhe_ = "[" + shenhe.ToString() + "]";
            }
            json_str = callback + "({\"list\":" + json_str + ",\"count\":[{\"shu\":\"" + shu + "\"}],\"biaotou\":[" + biaotou + "],\"shenhe\":" + shenhe_ + "})";
            return json_str;
            #endregion
        }
        else
        {
            #region 默认type
            if (json_str == "")
            {
                json_str = "[]";
            }
            json_str = callback + "({\"list\":" + json_str + ",\"count\":[{\"shu\":\"" + shu + "\"}],\"biaotou\":[" + biaotou + "]})";
            return json_str;
            #endregion
        }
      
        return json_str;
    }

    public string oneJson(string sql, string callback)
    {
        DataTable dt = new DataTable();
        string json_str = "";
        dt = my_c.GetTable(sql);
        json_str = morenToJson(json_str, dt);
        string shu = shu = dt.Rows.Count.ToString();
       
        #region 默认type
        if (json_str == "")
        {
            json_str = "[]";
        }
        json_str = callback + "({\"list\":" + json_str + ",\"count\":[{\"shu\":\"" + shu + "\"}]})";
        return json_str;
        #endregion
        return json_str;
    }
    //json替换
    public string jsontianhuan(string g1)
    {
        return g1.Replace("\"", "\\"+"\"");
    }
    /// <summary>
    /// 将datatable生成json
    /// </summary>
    /// <param name="jsonName">返回的string</param>
    /// <param name="dt">待转换的dt</param>
    /// <returns></returns>
    /// 
    string biaotou = "";
    #region 处理表头
    public void get_biaotou(DataTable dt)
    {
        biaotou = "{\"title\":\"编号\",\"key\":\"id\",\"width\":\"\",\"align\":\"center\"}";
        string number = "0";
        try
        {
            number = HttpContext.Current.Request.QueryString["number"].ToString();
        }
        catch { }

        if (dt.Rows.Count >= 0)
        {
            for (int i = 0; i < 1; i++)
            {
    
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                  //  HttpContext.Current.Response.Write(dt.Columns[j].ColumnName.ToString() + "<br>");
                    #region 设置表头 
                   
                    string table_name = "";
                    string Model_id = "";
                    string[] table_name_ = my_b.set_url_css(HttpContext.Current.Request.QueryString["t"].ToString()).Split(',');
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                        table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable sl_Model = my_c.GetTable("select * from sl_Model where u1='" + table_name + "'");
                        if (Model_id == "")
                        {
                            Model_id = sl_Model.Rows[0]["id"].ToString();
                        }
                        else
                        {
                            Model_id = Model_id+","+sl_Model.Rows[0]["id"].ToString();
                        }
                    }
               
                    DataTable sl_Field = new DataTable();
                    sl_Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (" + Model_id + ")  order by u9,id");
                  //  HttpContext.Current.Response.Write("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (" + Model_id + ")  order by u9,id");
                    //HttpContext.Current.Response.End();
                    DataRow[] rows = sl_Field.Select("u1='"+ dt.Columns[j].ColumnName.ToString() + "'");
                  //  HttpContext.Current.Response.Write("u1='" + dt.Columns[j].ColumnName.ToString() + "'|"+ rows.Length + "<br>");
                    if (rows.Length > 0)
                    {
                        if (biaotou == "")
                        {
                            if (rows[0]["shijian"].ToString() == "")
                            {
                                biaotou = "{\"title\":\"" + rows[0]["u2"].ToString() + "\",\"key\":\"" + rows[0]["u1"].ToString() + "\",\"width\":\"" + rows[0]["u11"].ToString() + "\",\"align\":\"" + rows[0]["align"].ToString() + "\"}";
                            }
                            else
                            {
                                biaotou = "{\"title\":\"" + rows[0]["u2"].ToString() + "\",\"key\":\"" + rows[0]["u1"].ToString() + "\",\"width\":\"" + rows[0]["u11"].ToString() + "\",\"align\":\"" + rows[0]["align"].ToString() + "\"," + rows[0]["shijian"].ToString() + "}";
                            }

                        }
                        else
                        {
                            if (rows[0]["shijian"].ToString() == "")
                            {
                                biaotou = biaotou + ",{\"title\":\"" + rows[0]["u2"].ToString() + "\",\"key\":\"" + rows[0]["u1"].ToString() + "\",\"width\":\"" + rows[0]["u11"].ToString() + "\",\"align\":\"" + rows[0]["align"].ToString() + "\"}";
                            }
                            else
                            {
           
                                biaotou = biaotou + ",{\"title\":\"" + rows[0]["u2"].ToString() + "\",\"key\":\"" + rows[0]["u1"].ToString() + "\",\"width\":\"" + rows[0]["u11"].ToString() + "\",\"align\":\"" + rows[0]["align"].ToString() + "\"," + rows[0]["shijian"].ToString() + "}";
                            }
                        }
                    }
                     
                    #endregion

                }
               
            }
        }
       
       
    }
    #endregion
    #region 获取字段函数
    public string get_url(string content)
    {
        return content;
    }
    public string get_Fields_hanshu(DataTable Field_dt, string zhi, string Field_u1)
    {
        string hanshu_str = "";
        StringBuilder str = new StringBuilder();
        str.Append("{");
        for (int j1 = 0; j1 < Field_dt.Rows.Count; j1++)
        {
            if (Field_dt.Rows[j1]["u1"].ToString() == Field_u1)
            {
                #region 有相同的字段
                string u6 = Field_dt.Rows[j1]["u6"].ToString();
                if (u6 == "时间框")
                {
                    zhi = my_b.set_time(zhi, "yyyy-MM-dd");
                    //HttpContext.Current.Response.Write(zhi);
                    //HttpContext.Current.Response.End();
                }
                else if (u6 == "子编辑器" || u6 == "编辑器")
                {
                    zhi = my_b.set_neirong_url(zhi);
                    zhi= HttpUtility.UrlEncode(zhi).Replace("+", "%20");
                }
                else
                {
                    if (zhi != "")
                    {
                        #region 其它字段类型

                        Regex reg = new Regex("{next}", RegexOptions.Singleline);

                        string[] u8 = reg.Split(Field_dt.Rows[j1]["u8"].ToString());
                        //HttpContext.Current.Response.Write(u8[0]);
                        //HttpContext.Current.Response.End();
                        for (int i = 0; i < u8.Length; i++)
                        {
                            Regex reg1 = new Regex("sql{.*?}", RegexOptions.Singleline);
                            MatchCollection matches1 = reg1.Matches(u8[i]);
                            if (matches1.Count > 0)
                            {
                                #region sql成立
                                string ziduan = "";
                                foreach (Match match in matches1)
                                {
                                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                                    string[] aa = match1.Split('|');
                                    string sql = "";
                                    try
                                    {
                                        sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi + ") " + aa[2].ToString();
                                        ziduan = aa[3].ToString();
                                    }
                                    catch
                                    {
                                        HttpContext.Current.Response.Write(Field_u1);
                                        HttpContext.Current.Response.End();
                                    }
                                    if (ziduan.IndexOf(",") > -1)
                                    {
                                        #region 针对逗号的处理
                                        if (ziduan.Split(',')[0].ToString() == "id")
                                        {
                                            sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi + ") " + aa[2].ToString();
                                        }
                                        else if (ziduan.Split(',')[0].ToString() == "yonghuming")
                                        {
                                            sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and yonghuming ='" + zhi + "' " + aa[2].ToString();
                                        }
                                        else
                                        {
                                            sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and " + ziduan.Split(',')[0].ToString() + " ='" + zhi + "' " + aa[2].ToString();

                                        }
                                        string sql1 = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();

                                        //try
                                        //{
                                        DataTable dt = my_c.GetTable(sql);
                                        int hangshu = dt.Rows.Count;
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (ziduan.Split(',').Length > 1)
                                            {
                                                if (str.ToString().Replace("{", "") != "") str.Append(",");
                                                str.Append("\"" + ziduan.Split(',')[1].ToString() + "\":" + "\"" + dt.Rows[0][ziduan.Split(',')[1].ToString()].ToString() + "\"");


                                            }
                                            else
                                            {
                                                if (str.ToString().Replace("{", "") != "") str.Append(",");
                                                str.Append("\"" + ziduan + "\":" + "\"" + dt.Rows[0][ziduan].ToString() + "\"");


                                            }

                                        }
                                        else
                                        {
                                            //if (str.ToString() != "") str.Append(",");
                                            //str.Append("\"" + ziduan + ":\"" + "\"" + dt.Rows[0][ziduan].ToString() + "\"");
                                            //if (hanshu_str == "")
                                            //{
                                            //    hanshu_str =  "";
                                            //}
                                            //else
                                            //{
                                            //    hanshu_str = hanshu_str + "{next}";
                                            //}

                                        }

                                        //}
                                        //catch
                                        //{

                                        //  dt = my_c.GetTable(sql1);

                                        //if (hanshu_str == "")
                                        //{
                                        //    hanshu_str =  zhi;
                                        //}
                                        //else
                                        //{
                                        //    hanshu_str = hanshu_str + "{next}" + zhi;
                                        //}
                                        // }
                                        #endregion
                                    }
                                    else
                                    {

                                        #region 针对加号的处理

                                        if (ziduan.Split('+')[0].ToString() == "id")
                                        {
                                            sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi + ") " + aa[2].ToString();
                                        }
                                        else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                                        {

                                            sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and yonghuming ='" + zhi + "' " + aa[2].ToString();

                                        }
                                        else
                                        {
                                            sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and " + ziduan.Split('+')[0].ToString() + " ='" + zhi + "' " + aa[2].ToString();

                                        }


                                        DataTable dt = my_c.GetTable(sql);
                                        int hangshu = dt.Rows.Count;
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (ziduan.Split('+').Length > 1)
                                            {
                                                if (str.ToString().Replace("{", "") != "") str.Append(",");
                                                str.Append("\"" + ziduan.Split('+')[1].ToString() + "\":" + "\"" + dt.Rows[0][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[0][ziduan.Split('+')[1].ToString()].ToString() + "\"");

                                            }
                                            else
                                            {
                                                if (str.ToString().Replace("{", "") != "") str.Append(",");
                                                str.Append("\"" + ziduan + "\":" + "\"" + dt.Rows[0][ziduan].ToString() + "\"");


                                            }

                                        }
                                        else
                                        {
                                            //if (hanshu_str == "")
                                            //{
                                            //    hanshu_str =  "";
                                            //}
                                            //else
                                            //{
                                            //    hanshu_str = hanshu_str + "{next}";
                                            //}

                                        }
                                        #endregion
                                    }


                                    ziduan = aa[3].ToString();

                                }
                                #endregion
                            }

                            Regex reg2 = new Regex("hanshu{.*?}", RegexOptions.Singleline);
                            MatchCollection matches2 = reg2.Matches(u8[i]);
                            if (matches2.Count > 0)
                            {

                                #region 函数（hanshu）成立
                                foreach (Match match in matches2)
                                {
                                    string match1 = match.ToString().Replace("hanshu{", "").Replace("}", "");
                                    string[] aa = match1.Split('|');
                                    if (zhi != "")
                                    {
                                        if (str.ToString().Replace("{", "") != "") str.Append(",");
                                        str.Append("\"" + aa[0].ToString() + "\":" + "\"" + my_hs.Fields_hanshu(aa[0].ToString(), zhi) + "\"");
                                        //if (hanshu_str == "")
                                        //{
                                        //    hanshu_str =  my_hs.Fields_hanshu(aa[0].ToString(), zhi);
                                        //}
                                        //else
                                        //{
                                        //    hanshu_str = hanshu_str + "{next}"+my_hs.Fields_hanshu(aa[0].ToString(), zhi);
                                        //}

                                    }

                                }
                                #endregion
                            }
                        }
                        #endregion
                    }

                }
                #endregion
            }
        }
        if (str.ToString().Replace("{","") == "")
        {
            hanshu_str ="\""+ jsontianhuan(zhi) + "\"";
        }
        else
        {
            if (str.ToString().Replace("{", "") != "") str.Append(",");
            str.Append("\"zhi\":" + "\"" + zhi + "\"");
            str.Append("}");
            //  hanshu_str = hanshu_str + "{next}" + zhi;
            hanshu_str = str.ToString();
        }
        //HttpContext.Current.Response.Write(str.ToString());
        //HttpContext.Current.Response.End();
        return hanshu_str;
    }
    #endregion
    #region 数据转json，根据记录需要换回中括号
    public string DataTableToJson(string jsonName, DataTable dt, DataTable sl_Field,DataTable Field_dt)
    {
        string number = "0";
        try
        {
            number = HttpContext.Current.Request.QueryString["number"].ToString();
        }
        catch { }

        StringBuilder Json = new StringBuilder();
        if (dt.Rows.Count >= 1)
        {
            Json.Append("[");
        }
        // Json.Append("[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    #region 处理字段是否显示
                    if (sl_Field.Rows.Count > 0)
                    {
                        #region sl_Field表有记录
                        for (int j1 = 0; j1 < sl_Field.Rows.Count; j1++)
                        {
                            if (sl_Field.Rows[j1]["u1"].ToString() == dt.Columns[j].ColumnName.ToString())
                            {

                                if (sl_Field.Rows[j1]["kejian"].ToString() == "false")
                                {
                                    #region 不可见
                                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + jsontianhuan("10001") + "\"");
                                    if (j < dt.Columns.Count - 1)
                                    {
                                        Json.Append(",");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 可见
                                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + get_Fields_hanshu(Field_dt, dt.Rows[i][j].ToString(), dt.Columns[j].ColumnName.ToString()) + "");
                                    if (j < dt.Columns.Count - 1)
                                    {
                                        Json.Append(",");
                                    }
                                    #endregion
                                }


                            }
                            else
                            {

                                #region 无记录
                                Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + get_Fields_hanshu(Field_dt, dt.Rows[i][j].ToString(), dt.Columns[j].ColumnName.ToString()) + "");
                                if (j < dt.Columns.Count - 1)
                                {
                                    Json.Append(",");
                                }
                                #endregion
                            }
                        }

                        #endregion
                    }
                    else
                    {
                      
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + get_Fields_hanshu(Field_dt, dt.Rows[i][j].ToString(), dt.Columns[j].ColumnName.ToString()) + "");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    #endregion

                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        if (dt.Rows.Count >= 1)
        {
            Json.Append("]");
        }
        //Json.Append("]");
        return Json.ToString();
    }
    #endregion
    #region 数据转json 无需返回中括号
    public string yitiaoToJson(string jsonName, DataTable dt, DataTable sl_Field, DataTable Field_dt)
    {
        string number = "0";
        try
        {
            number = HttpContext.Current.Request.QueryString["number"].ToString();
        }
        catch { }

        StringBuilder Json = new StringBuilder();
        //if (dt.Rows.Count > 1)
        //{
        //    Json.Append("[");
        //}
        // Json.Append("[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    #region 处理字段是否显示
                    if (sl_Field.Rows.Count > 0)
                    {
                        #region sl_Field表有记录
                        for (int j1 = 0; j1 < sl_Field.Rows.Count; j1++)
                        {
                            if (sl_Field.Rows[j1]["u1"].ToString() == dt.Columns[j].ColumnName.ToString())
                            {

                                if (sl_Field.Rows[j1]["kejian"].ToString() == "false")
                                {
                                    #region 不可见
                                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + jsontianhuan("10001") + "\"");
                                    if (j < dt.Columns.Count - 1)
                                    {
                                        Json.Append(",");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 可见
                                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + get_Fields_hanshu(Field_dt, dt.Rows[i][j].ToString(), dt.Columns[j].ColumnName.ToString()) + "");
                                    if (j < dt.Columns.Count - 1)
                                    {
                                        Json.Append(",");
                                    }
                                    #endregion
                                }
                            }
                            else
                            {

                                #region 无记录
                                Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + get_Fields_hanshu(Field_dt, dt.Rows[i][j].ToString(), dt.Columns[j].ColumnName.ToString()) + "");
                                if (j < dt.Columns.Count - 1)
                                {
                                    Json.Append(",");
                                }
                                #endregion
                            }
                        }

                        #endregion
                    }
                    else
                    {

                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + get_Fields_hanshu(Field_dt, dt.Rows[i][j].ToString(), dt.Columns[j].ColumnName.ToString()) + "");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    #endregion

                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        //if (dt.Rows.Count > 1)
        //{
        //    Json.Append("]");
        //}
        //Json.Append("]");
        return Json.ToString();
    }
    #endregion
    #region 数据转json 默认方式
    public string morenToJson(string jsonName, DataTable dt)
    {
        string number = "0";
        try
        {
            number = HttpContext.Current.Request.QueryString["number"].ToString();
        }
        catch { }

        StringBuilder Json = new StringBuilder();
        if (dt.Rows.Count > 1)
        {
            Json.Append("[");
        }
        // Json.Append("[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + jsontianhuan(dt.Rows[i][j].ToString()) + "\"");
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
        if (dt.Rows.Count > 1)
        {
            Json.Append("]");
        }
        //Json.Append("]");
        return Json.ToString();
    }
    #endregion
    #region 电子表格转json 无数据库关联
    public string XLSToJson(string jsonName, DataTable dt)
    {
        string number = "0";
        try
        {
            number = HttpContext.Current.Request.QueryString["number"].ToString();
        }
        catch { }

        StringBuilder Json = new StringBuilder();
        if (int.Parse(number) > 1)
        {
            Json.Append("[");
        }
        // Json.Append("[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + jsontianhuan(dt.Rows[i][j].ToString()) + "\"");
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
        if (int.Parse(number) > 1)
        {
            Json.Append("]");
        }
        //Json.Append("]");
        return Json.ToString();
    }
    #endregion
}
