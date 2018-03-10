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
using Newtonsoft.Json;
using LitJson;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
public partial class appsearch : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_hanshu my_hs = new my_hanshu();
    my_html my_h = new my_html();
    my_json my_js = new my_json();
    #region API接口验证
    public void set_api(string type)
    {
        if (type != "login")
        {
            string sign = "";
            try
            {
                sign = my_b.c_string(Request["sign"].ToString());
            }
            catch { }
            if (sign != "")
            {
                string timestamp = my_b.c_string(Request["timestamp"].ToString());
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");

                string appid = my_c.GetTable("select top 1 * from sl_xitongpeizhi").Rows[0]["appid"].ToString();
                string _sign = my_api.SampleCode.test_search(qu_str, timestamp,appid);
                HttpContext.Current.Response.Write(_sign);
                HttpContext.Current.Response.End();
                if (sign == _sign)
                {
                    //tiaozhuan("1", "", "");
                }
                else
                {
                    string callback = Request.QueryString["jsoncallback"];
                    string result = callback + "({\"date\":\"0\",\"msg\":\"签名不正确\"})";

                    Response.Clear();
                    Response.Write(result);
                    Response.End();
                }
            }

        }

    }
    #endregion
    #region 获取目录树
    private static string GetTasksString(int TaskId, DataTable table)
    {
        DataRow[] rows = table.Select("classid=" + TaskId.ToString());
        //HttpContext.Current.Response.Write(rows[0]["id"]);
        //HttpContext.Current.Response.End();
        if (rows.Length == 0) return string.Empty; ;
        StringBuilder str = new StringBuilder();
        int hangshu = 0;
        foreach (DataRow row in rows)
        {
            hangshu = hangshu + 1;
            str.Append("{");
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (i != 0) str.Append(",");
                str.Append("\"" + row.Table.Columns[i].ColumnName + "\"");
                str.Append(":\"");
                str.Append(row[i].ToString());
                str.Append("\"");
            }
            DataRow[] rowscount = table.Select("classid=" + row["id"]);
            if (rowscount.Length > 0)
            {
                str.Append(",\"children\":[");
                str.Append(GetTasksString((int)row["id"], table));
                if (hangshu == rows.Length)
                {
                    str.Append("]}");
                }
                else
                {
                    str.Append("]},");
                }

            }
            else
            {
                if (hangshu == rows.Length)
                {
                    str.Append("}");
                }
                else
                {
                    str.Append("},");
                }

            }

        }

        try
        {
            return str[str.Length] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }
        catch
        {
            return str.ToString();
        }

    }
    #endregion
    #region 获取目录树
    private static string Getcaidan(int TaskId, DataTable table)
    {
        DataRow[] rows = table.Select("classid=" + TaskId.ToString());
        //HttpContext.Current.Response.Write(rows[0]["id"]);
        //HttpContext.Current.Response.End();
        if (rows.Length == 0) return string.Empty; ;
        StringBuilder str = new StringBuilder();
        int hangshu = 0;
        foreach (DataRow row in rows)
        {
            DataRow[] rowscount = table.Select("classid=" + row["id"]);
            if (rowscount.Length > 0)
            {
                hangshu = hangshu + 1;
                str.Append("{");
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    if (i != 0) str.Append(",");
                    str.Append("\"" + row.Table.Columns[i].ColumnName + "\"");
                    str.Append(":\"");
                    str.Append(row[i].ToString());
                    str.Append("\"");
                }

                if (rowscount.Length > 0)
                {
                    str.Append(",\"children\":[");
                    str.Append(GetTasksString((int)row["id"], table));
                    if (hangshu == rows.Length)
                    {
                        str.Append("]}");
                    }
                    else
                    {
                        str.Append("]},");
                    }

                }
                else
                {
                    if (hangshu == rows.Length)
                    {
                        str.Append("}");
                    }
                    else
                    {
                        str.Append("},");
                    }

                }
            }
                

        }

        try
        {
            return str[str.Length] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }
        catch
        {
            return str.ToString();
        }

    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string Print = "";
            try
            {
                Print = Request.QueryString["Print"].ToString();
            }
            catch { }

            string callback = "";
            try
            {
                callback = Request.QueryString["jsoncallback"].ToString();
            }
            catch { }
            string type = Request.QueryString["type"];
            #region API接口验证
            set_api(type);
            #endregion
            if (type == "search")
            {
                #region 基础查询
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "";
                string[] table_name_ = my_b.set_url_css(Request.QueryString["t"].ToString()).Split(',');
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (table_name == "")
                    {
                        table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(),"");
                    }
                    else
                    {
                        table_name = table_name + "," + ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                    }
                }
        
                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");
                string quer_sql_diy = "";
                #region 根据表不同生成SQL
                quer_sql_diy += my_hs.get_diff_table_sql(table_name_[0].ToString());

                #endregion
                string quer_sql = my_h.get_quer_sql(qu_str, table_name);
                if (quer_sql_diy != "")
                {
                    if (quer_sql == "")
                    {
                        quer_sql = quer_sql + " where " + quer_sql_diy;
                    }
                    else {
                        quer_sql = quer_sql + " and " + quer_sql_diy;
                    }
                  
                }
                //Response.Write(quer_sql);
                //Response.End();
                #region 记录删除状态
                string[] cc = table_name.Split(',');
                string biaoming = "";
                for (int i = 0; i < cc.Length; i++)
                {
                    if (my_c.GetTable("select id from sl_field where u1='shanchuzhuangtai' and Model_id in (select id from sl_Model where u1='" + cc[i] + "')").Rows.Count > 0)
                    {
                        if (quer_sql == "")
                        {
                            quer_sql = "where ("+ cc[i] + ".shanchuzhuangtai>-1)";
                        }
                        else
                        {
                            quer_sql = quer_sql + " and (" + cc[i] + ".shanchuzhuangtai>-1)";
                        }
                    }
                  
                }
                 
                #endregion
                string sql = "";
                string ordertype = "id";
                string page = "1";
                string liemingcheng = "";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch {
                     liemingcheng = table_name.Split(',')[0].ToString()+".id";
                    #region 默认字段
                    string Model_id = "";
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                       string table_name1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable sl_Model = my_c.GetTable("select * from sl_Model where u1='" + table_name1 + "'");
                        if (Model_id == "")
                        {
                            Model_id = sl_Model.Rows[0]["id"].ToString();
                        }
                        else
                        {
                            Model_id = Model_id + "," + sl_Model.Rows[0]["id"].ToString();
                        }
                    }
                    DataTable Field = new DataTable();
                    Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (" + Model_id + ") and  U5='是'   order by u9,id");
                    for (int i1 = 0; i1 < Field.Rows.Count; i1++)
                    {
                        liemingcheng = liemingcheng + "," + Field.Rows[i1]["u1"].ToString();
                    }
                    #endregion
                }
                liemingcheng = liemingcheng.Replace("20%", " ");
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
                try
                {
                    page = my_b.c_string(Request.QueryString["page"].ToString());
                }
                catch { }
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }
                if (liemingcheng.IndexOf("count(") > -1 || liemingcheng.IndexOf("sum(") > -1 || liemingcheng.IndexOf("avg(") > -1)
                {
                    if (liemingcheng.IndexOf("sum(") > -1)
                    {
                        Regex reg1 = new Regex(" ", RegexOptions.Singleline);
                        string[] aa = reg1.Split(liemingcheng);
                        try
                        {
                            liemingcheng = "convert(decimal(38, 0)," + aa[0] + ") as " + aa[2] + "";
                        }
                        catch { }

                    }
                    ordertype = "";
                    orderby = "";
                    number = "";
                }

                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }
                string sql1 = "";

                if (number == "")
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    //Response.Write(table_name);
                    //Response.End();
                    sql1 = "select count("+ table_name.Split(',')[0].ToString() + ".id) as count_id from " + table_name + " " + quer_sql ;
                    if (page == "1")
                    {
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                    }
                    else
                    {
                        string quer_sql1 = quer_sql.Substring(0, 5);
                        if (quer_sql1 == "where")
                        {
                            quer_sql1 = quer_sql.Substring(5, quer_sql.Length - 5);
                            quer_sql1 = " and " + quer_sql1;
                        }
                        int d_page = int.Parse(number) * (int.Parse(page) - 1);
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " where " + table_name.Split(',')[0].ToString() + ".id not in (select top " + d_page + " " + table_name.Split(',')[0].ToString()+".id" + " from " + table_name + " " + quer_sql + order_str + ") " + quer_sql1 + order_str;
                    }

                }
                #region 关键词权限
                string Field_biaoming = "";
                for (int i = 0; i < cc.Length; i++)
                {
                    if (Field_biaoming == "")
                    {
                        Field_biaoming = "u1='"+ cc[i] + "'";
                    }
                    else
                    {
                        Field_biaoming = Field_biaoming+ " or u1='" + cc[i] + "'";
                    }

                    if (biaoming == "")
                    {
                        biaoming = "sl_guanjianciquanxian.biaoming='" + cc[i] + "'";
                    }
                    else
                    {
                        biaoming = biaoming + " or sl_guanjianciquanxian.biaoming='" + cc[i] + "'";
                    }
                }
                DataTable sl_Field = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");

                DataTable Field_dt = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where "+ Field_biaoming + ")");
                //Response.Write("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.End();
                #endregion

                if (Print == "yes")
                {
                    Response.Write(sql);
                    Response.End();
                }

                string result = my_js.OutJson(sql, callback,sql1, sl_Field, Field_dt,"");

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "list")
            {
                #region 获取目录树
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "";
                string[] table_name_ = my_b.set_url_css(Request.QueryString["t"].ToString()).Split(',');
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (table_name == "")
                    {
                        table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                    }
                    else
                    {
                        table_name = table_name + "," + ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                    }
                }
                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");
                string quer_sql = "";
               // quer_sql = my_h.get_quer_sql(qu_str, table_name);
                string sql = "";
                string ordertype = "id";
                string page = "1";
                string liemingcheng = "*";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch { }
                liemingcheng = liemingcheng.Replace("20%", " ");
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
                try
                {
                    page = my_b.c_string(Request.QueryString["page"].ToString());
                }
                catch { }
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }
                

                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }

                if (number == "")
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    if (page == "1")
                    {
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                    }
                    else
                    {
                        int d_page = int.Parse(number) * (int.Parse(page) - 1);
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " and id not in (select top " + d_page + " " + "id" + " from " + table_name + quer_sql + order_str + ") " + quer_sql + order_str;
                    }

                }
                //Response.Write(sql);
                //Response.End();
                DataTable dt1 = my_c.GetTable(sql);
                int classid = 0;
                try
                {
                    classid = int.Parse(Request.QueryString["classid"].ToString());
                }
                catch
                { }
                Response.Write(callback + "([" + GetTasksString(classid, dt1) + "])");
                Response.End();
                #endregion
            }
            else if (type == "caidan")
            {
                #region 获取菜单目录
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "";
                string[] table_name_ = my_b.set_url_css(Request.QueryString["t"].ToString()).Split(',');
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (table_name == "")
                    {
                        table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString();
                    }
                    else
                    {
                        table_name = table_name + "," + ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString();
                    }
                }
                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");
                string quer_sql = "";
                string sql = "";
                string ordertype = "id";
  
                string liemingcheng = "*";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch { }
                liemingcheng = liemingcheng.Replace("20%", " ");
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
        
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }


                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }

               
                DataTable sl_zhiwei = my_c.GetTable("select * from sl_zhiwei where id in (select suoshuzhiwei from sl_yuangong where id=" + my_b.k_cookie("user_name") + "	)");
                //Response.Write("select * from sl_zhiwei where id in (select suoshuzhiwei from sl_yuangong where yonghuming='" + my_b.k_cookie("user_name") + "'	)");
                //Response.End();
                if (sl_zhiwei.Rows.Count > 0)
                {
                    quer_sql = "where id in (" + sl_zhiwei.Rows[0]["caidanquanxian"].ToString() + ") or classid=0";
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                //Response.Write(sql);
                //Response.End();
                DataTable dt1 = my_c.GetTable(sql);
                Response.Write(callback + "([" + Getcaidan(0, dt1) + "])");
                Response.End();
                #endregion
            }
            else if (type == "shijian")
            {
                #region 北京时间
                DateTime bjshijian = DateTime.Now;
                string tian = "";
                try
                {
                    tian = Request.QueryString["tian"].ToString();
                }
                catch
                { }
                if (tian != "")
                {
                    bjshijian = bjshijian.AddDays(Double.Parse(tian));
                }
                string result = callback + "({\"date\":\"" + bjshijian.ToString() + "\"})";
                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "bianhao")
            {
                #region 订单号
                string result = callback + "({\"date\":\"" + my_b.get_bianhao() + "\"})";

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "ip")
            {
                #region IP
                string result = callback + "({\"date\":\"" + Request.UserHostAddress.ToString() + "\"})";

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "gonghao")
            {
                #region 员工工号
                string result = callback + "({\"date\":\"" + my_b.get_gonghao() + "\"})";

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "cookie")
            {
                #region 获取COOKIE
                string cookie_name = my_b.c_string(Request.QueryString["name"].ToString());
                string cookie_value = "";
                try
                {
                    cookie_value = my_b.k_cookie(cookie_name);
                    cookie_value = my_b.tihuan(cookie_value, "fzw123", "&");
                }
                catch
                {

                }
                string result = callback + "({\"date\":\"" + cookie_value + "\"})";

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }

        }

    }
}
