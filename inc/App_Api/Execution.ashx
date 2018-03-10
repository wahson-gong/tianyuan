<%@ WebHandler Language="C#" Class="order" %>

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public class order : IHttpHandler {


    /***************引用start*******************/
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    Common_ghy my_com = new Common_ghy();
    /***************引用end*******************/

    System.Data.DataTable dt;
    System.Data.DataTable Model_dt;
    System.Data.DataTable model_table;

    public string json_str = string.Empty;
    StringBuilder Sql = new StringBuilder();//SQL字符串
    HttpContext context;


    #region 接收参数
    public string callback = "";
    //public string sql = "";
    public string id = "";
    public string ids = "";//多个id 中间以,隔开
    public string top = "";
    public string sql_str = "";
    public string table = "";
    public string re_param = "";
    public string query_str = "";
    public string Model_id = "";
    public string table_name = "";
    public string sql = "";
    public string where = "";//接收where条件
    #endregion

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Charset = "utf-8";

        #region 处理接收参数
        if (!string.IsNullOrEmpty(context.Request.Params["callback"]))
        {
            callback = my_b.c_string(context.Request.Params["callback"]);
        }

        if (!string.IsNullOrEmpty(context.Request.Params["id"]))
        {
            id = my_b.c_string(context.Request.Params["id"]);
        }
        if (!string.IsNullOrEmpty(context.Request.Params["ids"]))
        {
            ids = my_b.c_string(context.Request.Params["ids"]);
        }
        if (!string.IsNullOrEmpty(context.Request.Params["top"]))
        {
            top = my_b.c_string(context.Request.Params["top"]);
        }

        if (!string.IsNullOrEmpty(context.Request.Params["sql_str"]))
        {
            sql_str = my_b.c_string(context.Request.Params["sql_str"]);
        }

        if (!string.IsNullOrEmpty(context.Request.Params["table"]))
        {
            table = my_b.c_string(context.Request.Params["table"]);
        }
        if (!string.IsNullOrEmpty(context.Request.Params["re_param"]))
        {
            re_param = my_b.c_string(context.Request.Params["re_param"]);
        }

        if (!string.IsNullOrEmpty(context.Request.Params["query_str"]))
        {
            query_str = my_b.c_string(context.Request.Params["query_str"]);
        }

        if (!string.IsNullOrEmpty(context.Request.Params["where"]))
        {
            where = my_b.c_string(context.Request.Params["where"]);
        }
        //query_str  where
        #endregion

        #region 处理callback

        string temp_callback = "";
        temp_callback = callback;
        callback = my_com.get_callback(callback, "query");
        callback = my_com.get_callback(callback, "add");
        callback = my_com.get_callback(callback, "edit");
        callback = my_com.get_callback(callback, "isadded");
        #endregion


        switch (callback)
        {

            //单标查询
            case "query":
                if (!string.IsNullOrEmpty(table))
                {
                    Sql.Append(" select  ");
                    if (!string.IsNullOrEmpty(top))
                    {
                        Sql.Append(" top  " + top);
                    }
                    if (!string.IsNullOrEmpty(re_param))
                    {
                        Sql.Append(" " + re_param.Replace("and", ",") + " ");
                    }
                    else
                    {
                        Sql.Append(" * ");
                    }
                    Sql.Append(" from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table + " ");
                    Sql.Append(" where 1=1 " );

                    Model_id = my_com.get_table_param("Model", "id", "u1", System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table, context);

                    Model_dt = my_c.GetTable("select * from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
                    //查询请求是传递过来的参数
                    for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                    {
                        //  context.Response.Write(context.Request.Params[Model_dt.Rows[d1]["u1"].ToString()] + "   ");
                        Sql.Append( get_kj2(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString()));

                    }



                    //含有 !=， >， <等符号的查询 
                    if (!string.IsNullOrEmpty(sql_str))
                    {
                        Sql.Append(user_sql(sql_str));
                    }
                    //模糊查询参数
                    if (!string.IsNullOrEmpty(query_str))
                    {
                        Sql.Append(get_mohu_sql(table, query_str));

                    }

                    if (!string.IsNullOrEmpty(ids))
                    {
                        Sql.Append("  and id in (" + ids + ")");
                    }
                    if (!string.IsNullOrEmpty(id))
                    {
                        Sql.Append("  and id ="+id);
                    }
                    //添加where条件
                    Sql.Append(re_whereSql( where));


                    Sql.Append(" order by id desc");

                }

                //context.Response.Write(Sql.ToString());
                //context.Response.End();

                //写入日志
                try
                {
                    my_c.genxin("insert into " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name") + "','查询内容===》成功,sql=" + Sql.ToString() + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','查询内容')");

                }
                catch {
                    my_c.genxin("insert into " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('未登录','查询内容===》成功,sql=" + HttpContext.Current.Server.HtmlEncode(Sql.ToString()) + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','查询内容')");

                }

                my_com.OutJson( Sql.ToString(), temp_callback);
                break;

            //判断某一条数据是否存在
            case "isadded":
                if (!isPassTable(table))
                {
                    my_com.my_jsonDataReturnOneString("access defined", callback, context);
                }
                //my_com.my_jsonDataReturnOneString(isPassTable(table).ToString(), callback, context);
                if (!string.IsNullOrEmpty(table))
                {
                    Sql.Append(" select  id ");
                    Sql.Append(" from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table + " ");
                    Sql.Append(" where 1=1 ");
                    if (!string.IsNullOrEmpty(sql_str))
                    {

                        Sql.Append(user_sql(sql_str));

                    }
                    if (!string.IsNullOrEmpty(query_str))
                    {
                        Sql.Append(get_mohu_sql(table, query_str));

                    }
                    if (!string.IsNullOrEmpty(ids))
                    {
                        Sql.Append("  and id in (" + ids + ")");
                    }
                    if (!string.IsNullOrEmpty(id))
                    {
                        Sql.Append("  and id =" + id);
                    }
                    //添加where条件
                    Sql.Append(re_whereSql(where));

                }
                dt = my_c.GetTable(Sql.ToString());


                //context.Response.Write(Sql.ToString());
                //context.Response.End();


                if (dt.Rows.Count > 0)
                {
                    my_com.my_jsonDataReturnOneString("true", temp_callback, context);
                    my_c.genxin("insert into " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name") + "','查询内容是否存在===》成功,sql=" + Sql.ToString() + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','查询内容是否存在')");
                }
                else
                {
                    my_com.my_jsonDataReturnOneString("false", callback, context);
                    my_c.genxin("insert into " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name") + "','查询内容是否存在===》失败','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','查询内容是否存在')");
                }
                break;


            case  "add":

                //try
                //{
                //    if (my_b.k_cookie("user_name") == "")
                //    {
                //        Response.Redirect("/err.aspx?err=请登陆后操作。&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                //    }
                //}
                //catch {
                //    Response.Redirect("/err.aspx?err=请登陆后操作。&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                //}

                //if (Request.UrlReferrer.ToString().ToLower().IndexOf(ConfigurationSettings.AppSettings["web_url"].ToString()) == -1)
                //{
                //    Response.Redirect("/err.aspx?err=来源不对。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                //}

                if (!isPassTable1(table))
                {
                    my_com.my_jsonDataReturnOneString(isPassTable1(table).ToString(), callback, context);
                }


                table_name = System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table;
                model_table = my_c.GetTable("select * from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
                Model_id = my_com.get_table_param("Model", "id", "u1", System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table, context);




                Model_dt = my_c.GetTable("select * from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");



                sql = "insert into " + table_name + " ";
                sql = sql + "(";
                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    if (d1 == 0)
                    {
                        sql = sql + Model_dt.Rows[d1]["u1"].ToString();
                    }
                    else
                    {
                        sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString();
                    }
                }

                sql = sql + ") values (";

                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    //  context.Response.Write(context.Request.Params[Model_dt.Rows[d1]["u1"].ToString()] + "   ");
                    if (d1 == 0)
                    {
                        sql = sql + get_kj(Model_dt.Rows[d1]["u6"].ToString(),Model_dt.Rows[d1]["u1"].ToString());
                    }
                    else
                    {
                        sql = sql + "," + get_kj(Model_dt.Rows[d1]["u6"].ToString(),Model_dt.Rows[d1]["u1"].ToString());
                    }
                }

                sql = sql + ")";

                //context.Response.Write(sql);
                try
                {
                    //context.Response.Write(sql);
                    my_c.genxin(sql);
                    my_com.my_jsonDataReturnOneString("success", temp_callback, context);
                }
                catch
                {
                    //HttpContext.Current.Response.Write("err");
                    //HttpContext.Current.Response.End();
                }
                try
                {
                    string article_new_id = my_c.GetTable("select top 1 id from " + table_name + " order by id desc").Rows[0]["id"].ToString();
                    string article_log = table_name + "{fenge}" + article_new_id + "{fenge}" + Model_id;
                    my_c.genxin("insert into " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name") + "','" + article_log + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','添加内容')");
                }
                catch
                { }




                break;

            case "edit":
                //如果条件全部为空即无 id ids where 等参数  则弹回
                if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(ids) && string.IsNullOrEmpty(where))
                {
                    context.Response.Write("条件不能为空");

                }


                if (!isPassTable1(table))
                {
                    my_com.my_jsonDataReturnOneString(isPassTable1(table).ToString(), callback, context);
                }

                table_name = System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table;
                model_table = my_c.GetTable("select * from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
                Model_id = my_com.get_table_param("Model", "id", "u1", System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table, context);




                Model_dt = my_c.GetTable("select * from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
                sql = "update " + table_name + " set ";
                int temp_u = 0;
                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    if (d1 == 0)
                    {
                        sql = sql + get_kj3(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString());
                        if (!string.IsNullOrEmpty(get_kj3(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString())))
                        {
                            temp_u++;
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(get_kj3(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString())))
                        {
                            if (temp_u == 0)
                            {
                                sql = sql + " " + get_kj3(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString());
                            }
                            else
                            {
                                sql = sql + "," + get_kj3(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString());

                            }
                            temp_u++;
                        }


                    }
                }

                sql = sql +" where 1=1 ";

                if (!string.IsNullOrEmpty(id))
                {
                    sql = sql + " and id=" + id;
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    sql = sql + " and id in (" + ids+")";

                }
                //添加where条件
                sql += re_whereSql(where);


                //context.Response.Write(sql);
                //context.Response.End();
                try
                {

                    my_c.genxin(sql);
                    my_com.my_jsonDataReturnOneString("success", temp_callback, context);
                }
                catch
                {

                }
                try
                {
                    string article_new_id = my_c.GetTable("select top 1 id from " + table_name + " order by id desc").Rows[0]["id"].ToString();
                    string article_log = table_name + "{fenge}" + article_new_id + "{fenge}" + Model_id;
                    my_c.genxin("insert into " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name") + "','" + article_log + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','更新内容')");
                }
                catch
                { }




                break;
        }




    }

    public string user_sql(string sql_str)
    {
        string[] sql_arr=new string[100];
        string re_sql = "";
        if (!string.IsNullOrEmpty(sql_str))
        {

            //sql_str = " 1=1 and " + sql_str;
            string[] sql_array = Regex.Split(sql_str, "and", RegexOptions.IgnoreCase);
            for (int i = 0; i < sql_array.Length; i++)
            {

                string fuhao = "";
                if (sql_array[i].ToString().IndexOf("!=") > 0)
                {
                    fuhao = "!=";
                    sql_array[i] = sql_array[i].Replace(fuhao, "$");
                    sql_arr = sql_array[i].ToString().Split('$');
                }
                else if (sql_array[i].ToString().IndexOf("<") > 0)
                {
                    fuhao = "<";
                    sql_array[i] = sql_array[i].Replace(fuhao, "$");
                    sql_arr = sql_array[i].ToString().Split('$');
                }
                else if (sql_array[i].ToString().IndexOf(">") > 0)
                {
                    fuhao = ">";
                    sql_array[i] = sql_array[i].Replace(fuhao, "$");
                    sql_arr = sql_array[i].ToString().Split('$');
                }
                else if (sql_array[i].ToString().IndexOf("=") > 0)
                {

                    fuhao = "=";
                    sql_array[i] = sql_array[i].Replace(fuhao, "$");
                    sql_arr = sql_array[i].ToString().Split('$');
                }


                //HttpUtility.UrlDecode(sql_arr[1]) != ""
                if (sql_arr[1] != "")
                {
                    if (sql_arr[0].ToString().IndexOf("shuzi") > 0)
                    {

                        re_sql = re_sql + "  and " + sql_arr[0].Replace("shuzi","") + fuhao + sql_arr[1];


                    }
                    else
                    {

                        re_sql = re_sql + " and " + sql_arr[0] + fuhao + " '" + sql_arr[1] + "'";


                    }


                }



            }

        }
        return re_sql;
    }

    //凭借模糊查询SQL
    public string get_mohu_sql(string table,string query_str)
    {
        string re_sql = "";
        string Model_id = "";
        Model_id = my_com.get_table_param("Model", "id", "u1", System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table, context);

        DataTable Model_dt = new DataTable();
        Model_dt = my_c.GetTable("select * from " + System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id);
        for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
        {
            if (d1 == 0)
            {
                re_sql = Model_dt.Rows[d1]["u1"].ToString() + " like '%" + query_str + "%'";
            }
            else
            {
                re_sql = re_sql + " or " + Model_dt.Rows[d1]["u1"].ToString() + " like '%" + query_str + "%'";
            }
        }

        return " and (" + re_sql+")";
    }

    /// <summary>
    /// 判断对应的表是否是运行查询的表
    /// </summary>
    /// <param name="table_name"></param>
    /// <returns></returns>
    public bool isPassTable(string table_name)
    {
        string temp_table = "";
        temp_table = my_com.get_table_param("Model", "u1", "u1", System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name, context);
        //HttpContext.Current.Response.Write(temp_table);
        //HttpContext.Current.Response.End();
        if (temp_table != "u1")
        {
            //已存在
            if (table_name.IndexOf("order") > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        else
        {
            return false;
        }




    }

    public bool isPassTable1(string table_name)
    {
        string temp_table = "";
        temp_table = my_com.get_table_param("Model", "u1", "u1", System.Configuration.ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name, context);
        if (temp_table != "u1")
        {
            //已存在
            if (table_name.IndexOf("user") > 0)
            {
                return false;
            }
            else if (table_name.IndexOf("order") > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        return false;
    }

    /// <summary>
    /// 用来add添加方法的返回增加的参数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string get_kj(string type, string id)
    {
        string temp_id = "";
        string temp_re = "";
        try
        {
            temp_id = id;
            //HttpContext.Current.Response.Write(id+"   ");
            temp_re = HttpContext.Current.Request.Params[id].ToString();
        }
        catch { }

        if (id == "yonghuming")
        {
            try
            {
                return "'" + my_b.k_cookie("user_name") + "'";
            }
            catch
            {
                return "'没有用户名'";
            }
            //HttpContext.Current.Response.Write(id + "   ");

        }

        if (id == "xianshi")
        {
            if (string.IsNullOrEmpty(temp_re))
            {
                return "'不显示'";
            }


        }

        if (id == "dtime")
        {
            if (string.IsNullOrEmpty(temp_re))
            {
                return "'"+DateTime.Now.ToString()+"'";
            }


        }

        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框" || type == "编辑器")
        {


            return "'" + my_b.c_string(temp_re) + "'";
        }
        else if (type == "数字")
        {
            return "" + my_b.c_string(temp_re) + "";
        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(my_b.c_string(temp_re)) + "'";
        }
        else
        {

            return "'" + temp_re + "'";
        }
        //return "";
    }

    /// <summary>
    /// 用来query查询方法的返回查询参数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string get_kj2(string type, string id)
    {
        string temp_id = "";
        string temp_re = "";
        string temp_re_sql = "";
        try
        {
            temp_id = id;
            //HttpContext.Current.Response.Write(id+"   ");
            temp_re = HttpContext.Current.Request.Params[id].ToString();
        }
        catch { }

        if (id == "yonghuming")
        {
            try
            {
                return " and " + id + "='" + my_b.k_cookie("user_name") + "' ";
            }
            catch
            {

                return " and " + id + "='没有用户名' ";
            }
            // HttpContext.Current.Response.Write(temp_re_sql + "   ");

        }

        if(id=="xianshi")
        {
            // HttpContext.Current.Response.Write(temp_re + " 1  ");
            if (string.IsNullOrEmpty(temp_re))
            {
                return " and " + id + "!='不显示' ";
            }

        }


        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框" || type == "编辑器")
        {


            temp_re_sql = "'" + my_b.c_string(temp_re) + "'";
        }
        else if (type == "数字")
        {
            temp_re_sql = "" + my_b.c_string(temp_re) + "";
        }
        else if (type == "密码框")
        {
            temp_re_sql = "'" + my_b.md5(my_b.c_string(temp_re)) + "'";
        }
        else
        {

            temp_re_sql = "'" + temp_re + "'";
        }

        //HttpContext.Current.Response.Write(temp_re_sql+" ==>  " +id);

        if (temp_re_sql != "''" && temp_re_sql != "")
        {
            return " and "+id + "=" + temp_re_sql;
        }
        else
        {
            return "";
        }

        //return "";
    }

    /// <summary>
    /// 用来edit修改方法的返回需要更新的参数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string get_kj3(string type, string id)
    {
        string temp_id = "";
        string temp_re = "";
        string temp_re_sql = "";
        try
        {
            temp_id = id;
            //HttpContext.Current.Response.Write(id+"   ");
            temp_re = HttpContext.Current.Request.Params[id].ToString();
        }
        catch { }

        if (id == "yonghuming")
        {
            try
            {
                temp_re_sql = "'" + my_b.k_cookie("user_name") + "'";
            }
            catch
            {
                temp_re_sql = "'没有用户名'";
            }
            //HttpContext.Current.Response.Write(id + "   ");

        }

        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框" || type == "编辑器")
        {


            temp_re_sql = "'" + my_b.c_string(temp_re) + "'";
        }
        else if (type == "数字")
        {
            temp_re_sql = "" + my_b.c_string(temp_re) + "";
        }
        else if (type == "密码框")
        {
            temp_re_sql = "'" + my_b.md5(my_b.c_string(temp_re)) + "'";
        }
        else
        {

            temp_re_sql = "'" + temp_re + "'";
        }

        // HttpContext.Current.Response.Write(temp_re_sql+"   ");

        if (temp_re_sql != "''" && temp_re_sql != "" )
        {
            return  id + "=" + temp_re_sql;
        }
        else
        {
            return "";
        }

        //return "";
    }

    public string re_whereSql(string where)
    {
        string sql = "";
        if (!string.IsNullOrEmpty(where))
        {
            string[] temp_arr = where.Split('$');
            string[] temp_where = new string[2];
            for (int j = 0; j < temp_arr.Length; j++)
            {
                temp_where = temp_arr[j].Split('=');
                sql += " and  " + temp_where[0] + "=" + temp_where[1] + " ";

            }

        }
        return sql.Replace("''", "'");
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}