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
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
public partial class Execution : System.Web.UI.Page
{
    biaodan bd = new biaodan();
    my_order my_o = new my_order();
    my_hanshu my_hs = new my_hanshu();
    #region 判断登录
    public void set_login()
    {
        try
        {
            if (my_b.k_cookie("user_name") == "")
            {
                tiaozhuan("请登陆后操作", Request.UrlReferrer.ToString(), "");

            }
        }
        catch
        {
            tiaozhuan("请登陆后操作", Request.UrlReferrer.ToString(), "");
        }
    }
    #endregion
    #region 处理验证码
    public void set_yanzhengma()
    {
        #region 记住密码
        string jizhumima = "";
        try
        {
            jizhumima = Request.Form["jizhumima"].ToString();
        }
        catch { }
        if (jizhumima != "")
        {
            my_b.c_cookie_dtime(Request["yonghuming"].ToString(), "yonghuming", 60 * 60 * 24 * 30);
            my_b.c_cookie_dtime(Request["mima"].ToString(), "mima", 60 * 60 * 24 * 30);
        }
        else
        {
            my_b.admin_o_cookie("yonghuming");
            my_b.admin_o_cookie("mima");
        }
        #endregion
        string yzm = "";
        try
        {
            yzm = Request.Form["yzm"].ToString();
        }
        catch { }

        if (yzm != "")
        {
            string yzm_cookie = "";
            try
            {
                yzm_cookie = my_b.k_cookie("yzm");
            }
            catch { }
            if (yzm != yzm_cookie)
            {
                Response.Redirect("/err.aspx?err=验证码输入不正确&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
            }
        }

        string shoujiyzm = "";
        try
        {
            shoujiyzm = Request.Form["shoujiyzm"].ToString();
            if (shoujiyzm == "")
            {
                shoujiyzm = "err";
            }
        }
        catch { }

        if (shoujiyzm != "")
        {
            string shoujiyzm_cookie = "";
            try
            {
                shoujiyzm_cookie = my_b.k_cookie("shoujiyzm");
            }
            catch { }
            if (shoujiyzm != shoujiyzm_cookie)
            {
                Response.Redirect("/err.aspx?err=手机号验证码输入不正确&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
            }
        }

        string youxiangyzm = "";
        try
        {
            youxiangyzm = Request.Form["youxiangyzm"].ToString();
            if (youxiangyzm == "")
            {
                youxiangyzm = "err";
            }
        }
        catch { }

        if (youxiangyzm != "")
        {
            string youxiangyzm_cookie = "";
            try
            {
                youxiangyzm_cookie = my_b.k_cookie("youxiangyzm");
            }
            catch { }
            if (youxiangyzm != youxiangyzm_cookie)
            {
                Response.Redirect("/err.aspx?err=邮箱验证码输入不正确&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
            }
        }
    }
    #endregion
    #region 处理输入频率问题
    public void shurupinlv(string type)
    {
        if (type== "add")
        {
            string yonghuming = "";
            try
            {
                yonghuming = my_b.k_cookie("user_name");
            }
            catch { }
            if (yonghuming == "")
            {
                try
                {
                    yonghuming = my_b.k_cookie("yonghuming");
                }
                catch { }
                if (yonghuming == "")
                {
                    yonghuming = my_b.get_bianhao();
                    my_b.c_cookie(yonghuming, "yonghuming");
                }
            }
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                #region sql
                DataTable dt = new DataTable();
                dt = my_c.GetTable("select top 1 * from sl_system where u3='" + Request.UserHostAddress.ToString() + "' and u1='" + yonghuming + "' and u4='提交操作' and datediff(MINUTE, dtime ,getdate()) <=1 order by dtime desc");
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("/err.aspx?err=1分钟内只能一次。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                #endregion
            }
            else
            {
                #region asccess
                TimeSpan st = new TimeSpan();
                DateTime dy1 = DateTime.Now;
                DateTime dy2 = DateTime.Now;
                try
                {
                    dy2 = DateTime.Parse(my_c.GetTable("select top 1 * from sl_system where u3='" + Request.UserHostAddress.ToString() + "'  and u1='" + yonghuming + "' and u4='提交操作' order by dtime desc").Rows[0]["dtime"].ToString());

                    st = dy1.Subtract(dy2);

                    if (st.TotalMinutes < 1)
                    {
                        tiaozhuan("1分内只能操作一次", "/", "");
                        //  Response.Redirect("/err.aspx?err=1分钟内只能一次。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                    }
                }
                catch
                { }
                #endregion
            }


        }
    }
    #endregion
    #region 记录操作日志
    public void set_system(string type)
    {
        string yonghuming = "";
        try
        {
            yonghuming = my_b.k_cookie("user_name");
        }
        catch { }
        if (yonghuming == "")
        {
            try
            {
                yonghuming = my_b.k_cookie("yonghuming");
            }
            catch { }
            if (yonghuming == "")
            {
                yonghuming = my_b.get_bianhao();
                my_b.c_cookie(yonghuming, "yonghuming");
            }
        }
        string u2 = "";
        try
        {
            u2 = Request.UrlReferrer.ToString();
        }
        catch
        {
            u2 = Request.Url.ToString();
        }
        string t = "";
        try
        {
            t = Request.QueryString["t"].ToString();
        }
        catch { }

        string RequestForm = "";
        try
        {
            RequestForm = Request.Form.ToString();
        }
        catch { }

        string RequeszQueryString = "";
        try
        {
            RequeszQueryString = Request.QueryString.ToString();
        }
        catch { }
        if (t != "")
        {
            u2 = u2 + "【表名：" + t + "】";
        }
        if (RequestForm != "")
        {
            u2 = u2 + "【表单：" + RequestForm + "】";
        }
        if (RequeszQueryString != "")
        {
            u2 = u2 + "【参数：" + RequeszQueryString + "】";
        }

        u2 = u2 + "【类型：" + type + "】";
        my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + yonghuming + "','" + u2 + "','" + Request.UserHostAddress.ToString() + "','提交操作')");
    }
    #endregion
    #region 记录文章审核
    public void set_shenhe(string table_name, string Model_id)
    {
        string yonghuming = "匿名";
        try
        {
            yonghuming = my_b.k_cookie("user_name");
        }
        catch { }
        string article_new_id = my_c.GetTable("select top 1 id from " + table_name + " order by id desc").Rows[0]["id"].ToString();
        string article_log = table_name + "{fenge}" + article_new_id + "{fenge}" + Model_id;
        my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + yonghuming + "','" + article_log + "','" + Request.UserHostAddress.ToString() + "','文章审核')");
    }
    #endregion
    #region 设置跳转
    public void tiaozhuan(string tip_string, string tipurl, string tipurl_type)
    {
        if (tip_string == "")
        {
            tip_string = "增加信息成功！马上跳转到一级页面！";
            try
            {
                tip_string = my_b.c_string(my_b.set_url_css(Request.QueryString["tip_string"].ToString()));
            }
            catch
            { }
        }

        if (tipurl == "")
        {
            tipurl = "";
            try
            {
                tipurl = Request.UrlReferrer.ToString();
            }
            catch
            {
                tipurl = "/";
            }
            try
            {
                tipurl = Request.QueryString["tipurl"].ToString();
            }
            catch
            { }
        }
        if (tipurl == "")
        {
            tipurl = "/";
        }

        if (tipurl_type == "")
        {
            try
            {
                tipurl_type = my_b.c_string(my_b.set_url_css(Request.QueryString["tipurl_type"].ToString()));
            }
            catch
            { }
        }

        if (tipurl_type == "alert")
        {
            Response.Write("<script>alert('" + tip_string + "');window.location.href = '" + Request.UrlReferrer.ToString() + "'; window.event.returnValue = false;;</script>");
        }
        else if (tipurl_type == "ajax")
        {
            Response.Write(tip_string);
        }
        else
        {

            Response.Redirect("/err.aspx?err=" + tip_string + "&errurl=" + my_b.tihuan(tipurl, "&", "fzw123") + "");
        }
    }
    #endregion
    #region 发送微信消息
    public void set_weixin()
    {
        //处理预订发送微信消息
        //if (Request["yonghuming"].ToString() != "")
        //{
        //    string wherestrig = "where laiyuanbianhao='" + my_b.c_string(Request["laiyuanbianhao"]) + "' and yonghuming='" + Request["yonghuming"].ToString() + "'";

        //    my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=yuyue&tablename=" + table_name + "&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");
        //}
    }
    #endregion
    #region 可以处理的表
    public void viewbiao(string type)
    {

        DataTable sl_Model = my_c.GetTable("select * from sl_Model where u1='" + ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString()) + "' and fangwen='是' and (fangwen_type='all' or fangwen_type like '%" + type + "%')");
        if (sl_Model.Rows.Count == 0)
        {
            #region 跳转
            tiaozhuan("无此表权限", "", "");
            #endregion
        }
    }
    #endregion 可以处理的表
    #region 整体输入提交
    public void set_user_sql(string type)
    {
        #region 处理验证码
        set_yanzhengma();
        #endregion
        #region 处理输入频率问题
        shurupinlv(type);
        #endregion
        #region 记录操作日志
        set_system(type);
        #endregion
        viewbiao(type);

        if (type == "add")
        {
            set_laiyuan();
            #region 默认增加数据
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();
            DataTable Model_dt = new DataTable();
            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }

            string sql = "insert into " + table_name + " ";
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
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                sql = sql + ",Filepath,classid) values (";
            }
            else
            {
                sql = sql + ") values (";
            }
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + bd.page_get_kj(Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
            }
            else
            {
                sql = sql + ")";
            }
            //Response.Write(sql);
            //Response.End();
            try
            {
                my_c.genxin(sql);
            }
            catch
            {
                Response.Write("err");
                Response.End();
            }
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion
            //end
            #endregion

        }
        else if (type == "getid")
        {
            #region 增加数据后输入信息ID
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();
            DataTable Model_dt = new DataTable();
            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }

            string sql = "insert into " + table_name + " ";
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
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                sql = sql + ",Filepath,classid) values (";
            }
            else
            {
                sql = sql + ") values (";
            }
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + bd.page_get_kj(Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
            }
            else
            {
                sql = sql + ")";
            }
            //Response.Write(sql);
            //Response.End();
            string id = "";
            try
            {
                id = my_c.GetTable(sql + " select @@IDENTITY as id").Rows[0]["id"].ToString();
            }
            catch
            {
                Response.Write("err");
                Response.End();
            }
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion
            Response.Write(id);
            Response.End();
            //end
            #endregion

        }
        else if (type == "pingfen")
        {
            #region 体院评分
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();


            DataTable Model_dt = new DataTable();
            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }

            string sql = "insert into " + table_name + " ";
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
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                sql = sql + ",Filepath,classid) values (";
            }
            else
            {
                sql = sql + ") values (";
            }
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + bd.page_get_kj(Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString() + "/";
                sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
            }
            else
            {
                sql = sql + ")";
            }

            try
            {
                my_c.genxin(sql);
            }
            catch
            {
                Response.Write("err");
                Response.End();
            }
            try
            {
                string article_new_id = my_c.GetTable("select top 1 id from " + table_name + " order by id desc").Rows[0]["id"].ToString();
                string article_log = table_name + "{fenge}" + article_new_id + "{fenge}" + Model_id;
                my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name") + "','" + article_log + "','" + Request.UserHostAddress.ToString() + "','文章审核')");
            }
            catch
            { }
            //算分
            string laiyuanbianhao = Request["laiyuanbianhao"].ToString();
            float pingfen = float.Parse(my_c.GetTable("select avg(pingfen) count_id from sl_pinglun where laiyuanbianhao=" + laiyuanbianhao + " and leixing='评分'").Rows[0]["count_id"].ToString());
            pingfen = pingfen / 5 * 100;

            my_c.genxin("update sl_video set pingfen=" + pingfen + " where id=" + laiyuanbianhao + "");
            //end


            #endregion
        }
        else if (type == "id_add")
        {


            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();
            DataTable Model_dt = new DataTable();
            Response.Write(Request.Form.ToString());

            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }
            string laiyuanbianhao = "";
            try
            {
                laiyuanbianhao = Request["laiyuanbianhao"].ToString();
            }
            catch { }
            string[] aa = laiyuanbianhao.Split(',');

            for (int i = 0; i < aa.Length; i++)
            {
                #region 批量提交
                string sql = "insert into " + table_name + " ";
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
                if (model_table.Rows[0]["u3"].ToString() == "文章模型")
                {
                    sql = sql + ",Filepath,classid) values (";
                }
                else
                {
                    sql = sql + ") values (";
                }
                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    if (Model_dt.Rows[d1]["u1"].ToString() == "laiyuanbianhao")
                    {
                        if (d1 == 0)
                        {
                            sql = sql + aa[i];
                        }
                        else
                        {
                            sql = sql + "," + aa[i];
                        }
                    }
                    else
                    {
                        if (d1 == 0)
                        {
                            sql = sql + bd.page_get_kj(Model_dt, d1);
                        }
                        else
                        {
                            sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                        }
                    }

                }
                if (model_table.Rows[0]["u3"].ToString() == "文章模型")
                {
                    DateTime dy = DateTime.Now;
                    Random r = new Random();
                    int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                    string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + my_b.chuli_lujing();
                    sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
                }
                else
                {
                    sql = sql + ")";
                }
                //Response.Write(sql);
                //Response.End();
                try
                {
                    my_c.genxin(sql);
                }
                catch
                {
                    Response.Write("err");
                    Response.End();
                }
                #endregion 批量提交
            }

            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion



        }
        else if (type == "guahao")
        {
            if (Request.UrlReferrer.ToString().ToLower().IndexOf(ConfigurationSettings.AppSettings["web_url"].ToString()) == -1)
            {
                Response.Redirect("/err.aspx?err=来源不对。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
            }
            TimeSpan st = new TimeSpan();
            DateTime dy1 = DateTime.Now;
            DateTime dy2 = DateTime.Now;
            DataTable sl_system = my_c.GetTable("select top 1 * from sl_system where u3='" + Request.UserHostAddress.ToString() + "' and u4='提交操作' order by dtime desc");
            if (sl_system.Rows.Count > 0)
            {
                try
                {
                    dy2 = DateTime.Parse(sl_system.Rows[0]["dtime"].ToString());
                }
                catch
                {
                    dy2 = DateTime.Now.AddMinutes(1);
                }

            }
            else
            {
                dy2 = dy1.AddDays(-1);
            }


            st = dy1.Subtract(dy2);

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('','','" + Request.UserHostAddress.ToString() + "','提交操作')");

            //if (st.TotalMinutes < 1)
            //{
            //    Response.Redirect("/err.aspx?err=1分钟内只能一次。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
            //}
            //else
            //{
            //    my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('','','" + Request.UserHostAddress.ToString() + "','提交操作')");
            //}
            //Response.Write(dy1.ToString() + "||" + dy2.ToString() + "||" + st.TotalMinutes);
            //Response.End();

            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();


            DataTable Model_dt = new DataTable();
            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }

            string sql = "insert into " + table_name + " ";
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
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                sql = sql + ",Filepath,classid) values (";
            }
            else
            {
                sql = sql + ") values (";
            }
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + bd.page_get_kj(Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
            }
            else
            {
                sql = sql + ")";
            }

            try
            {
                my_c.genxin(sql);
            }
            catch
            {
                Response.Write("err");
                Response.End();
            }
            string zhifufangshi = my_b.c_string(Request["zhifufangshi"].ToString());
            string dingdanhao = my_b.c_string(Request["dingdanhao"].ToString());
            if (zhifufangshi == "在线支付")
            {
                Response.Redirect("/pay/userorder.aspx?type=pay&tablename=register&dingdanhao=" + dingdanhao + "");
            }
            else
            {
                my_c.genxin("update sl_register set dingdanhao='' where dingdanhao='" + dingdanhao + "'");
            }
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion



        }
        else if (type == "cookie")
        {
            string cookie_name = Request.QueryString["cookie_name"].ToString();
            string cookie_value = Request.QueryString["cookie_value"].ToString();
            string tipurl = Request.QueryString["tipurl"].ToString();
            my_b.c_cookie(cookie_value, cookie_name);
            Response.Redirect(tipurl);
        }
        else if (type == "addadd")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();


            DataTable Model_dt = new DataTable();
            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }

            string sql = "insert into " + table_name + " ";
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
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                sql = sql + ",Filepath,classid) values (";
            }
            else
            {
                sql = sql + ") values (";
            }
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + bd.page_get_kj(Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
            }
            else
            {
                sql = sql + ")";
            }

            //设置默认
            string moren = Request["moren"].ToString();
            if (moren == "是")
            {
                my_c.genxin("update sl_add set moren='否' where yonghuming='" + my_b.k_cookie("user_name") + "'");
            }

            //end

            try
            {
                my_c.genxin(sql);
            }
            catch
            {
                Response.Write("err");
                Response.End();
            }
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion


        }
        else if (type == "useradd")
        {

            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();


            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            string sql = "insert into " + table_name + " ";
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
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                sql = sql + ",Filepath,classid) values (";
            }
            else
            {
                sql = sql + ") values (";
            }
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + bd.page_get_kj(Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request["classid"].ToString()) + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request["classid"].ToString()) + ")";
            }
            else
            {
                sql = sql + ")";
            }
            //Response.Write(sql);
            //Response.End();

            string tipurl_type = "";
            try
            {
                tipurl_type = my_b.c_string(my_b.set_url_css(Request.QueryString["tipurl_type"].ToString()));
            }
            catch
            { }


            //处理手机号
            string shouji = "";
            try
            {
                shouji = Request["shouji"].ToString();
            }
            catch { }
            if (shouji != "")
            {
                if (my_c.GetTable("select * from " + table_name + " where shouji='" + Request["shouji"].ToString() + "'").Rows.Count > 0)
                {
                    string openid = "";
                    try
                    {
                        openid = my_b.k_cookie("openid");
                    }
                    catch
                    { }

                    if (openid != "")
                    {
                        my_c.genxin("update sl_user set openid='" + openid + "' where yonghuming='" + shouji + "'");
                        my_b.c_cookie(shouji, "user_name");
                        tipurl_type = "";
                        try
                        {
                            tipurl_type = my_b.c_string(my_b.set_url_css(Request.QueryString["tipurl_type"].ToString()));
                        }
                        catch
                        { }
                        #region 跳转
                        tiaozhuan("", "", "");
                        #endregion
                    }
                    else
                    {
                        #region 跳转
                        tiaozhuan("此手机已经有人注册了", "", "");
                        #endregion

                    }



                }
            }
            //end

            try
            {
                if (Request["youxiang"].ToString() != "")
                {
                    if (my_c.GetTable("select * from " + table_name + " where youxiang='" + Request["youxiang"].ToString() + "'").Rows.Count > 0)
                    {
                        #region 跳转
                        tiaozhuan("此邮箱已经有人注册了", "", "");
                        #endregion

                    }
                }
            }
            catch { }

            try
            {
                if (Request["yonghuming"].ToString() != "")
                {
                    if (my_c.GetTable("select * from " + table_name + " where yonghuming='" + Request["yonghuming"].ToString() + "'").Rows.Count > 0)
                    {
                        #region 跳转
                        tiaozhuan("此用户名已经有人注册了", "", "");
                        #endregion

                    }
                }
            }
            catch { }




            //sql
            my_c.genxin(sql);
            //end
            string leixing = my_b.c_string(Request["leixing"].ToString());
            if (leixing != "老师")
            {
                my_b.c_cookie(Request["yonghuming"].ToString(), "user_name");
            }



            //微信
            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("micromessenger") > -1)
            {

                //my_c.genxin("update sl_user set openid='" + my_b.k_cookie("openid") + "',touxiang='" + my_b.k_cookie("touxiang") + "',xingbie='" + my_b.k_cookie("xingbie") + "',xingming='" + my_b.k_cookie("xingming") + "',guanzhu='" + my_b.k_cookie("subscribe") + "' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                string openid = "";
                try
                {
                    openid = my_b.k_cookie("openid");
                }
                catch
                { }
                if (openid != "")
                {
                    my_c.genxin("update sl_user set openid='" + my_b.k_cookie("openid") + "',touxiang='" + my_b.k_cookie("touxiang") + "',xingbie='" + my_b.k_cookie("xingbie") + "',guanzhu='" + my_b.k_cookie("subscribe") + "' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                }




            }

            //end


            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + Request["yonghuming"].ToString() + "','此用户名（" + Request["yonghuming"].ToString() + "）注册并登陆!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','会员登陆')");


        }
        else if (type == "youxiang_yzm")
        {
            #region 发送邮件
            string youxiang = my_b.set_url_css(Request["youxiang"].ToString());
            string leixingbiaoti = my_b.set_url_css(Request["leixingbiaoti"].ToString());

            //找回密码

            Random r = new Random();
            int Num1 = Convert.ToInt32(r.Next(999999)) + 100000;
            string shoujiyzm = Num1.ToString();
            DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");

            string yx_biaoti = my_b.get_value("biaoti", "sl_fasong", "where leixing='邮件' and leixingbiaoti='" + leixingbiaoti + "'");
            string yx_neirong = my_b.get_value("youjianneirong", "sl_fasong", "where leixing='邮件' and leixingbiaoti='" + leixingbiaoti + "'");

            yx_neirong = my_b.fasong_neirong_url(yx_neirong);
            int sta = my_b.WebMailTo(youxiang, yx_biaoti.Replace("$username", youxiang), yx_neirong.Replace("$username", youxiang).Replace("{yzm}", shoujiyzm));

            my_b.c_cookie(shoujiyzm, "youxiangyzm");
            my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + youxiang + "','此会员（" + youxiang + "）登陆成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','" + leixingbiaoti + "')");
            if (sta == 0)
            {
                Response.Write(sta);
                Response.End();
            }
            else
            {
                Response.Write("ok");
                Response.End();
            }
            #endregion
        }
        else if (type == "shouji_yzm")
        {
            #region 发短信判断验证码
            try
            {
                if (ConfigurationSettings.AppSettings["shouji_yzm"].ToString() == "yes")
                {
                    string yzm = Request["yzm"].ToString();
                    string yzm_cookie = "";
                    try
                    {
                        yzm_cookie = my_b.k_cookie("yzm");
                    }
                    catch { }
                    if (yzm != yzm_cookie)
                    {
                        Response.Redirect("/err.aspx?err=验证码输入不正确&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                    }
                }
            }
            catch { }

            #endregion
            string leixingbiaoti = my_b.set_url_css(Request["leixingbiaoti"].ToString());
            DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");
            string gs_name = xml_dt.Rows[0]["u1"].ToString();
            string shouji = Request.QueryString["shouji"].ToString();
            Random r = new Random();
            int Num1 = Convert.ToInt32(r.Next(900000)) + 100000;

            string shoujiyzm = Num1.ToString();
            my_b.c_cookie(shoujiyzm, "shoujiyzm");
            string neirong = my_b.get_value("duanxinneirong", "sl_fasong", "where leixing='短信' and leixingbiaoti='" + leixingbiaoti + "'");
            if (neirong == "")
            {
                Response.Write("未找到短信模板");
            }
            neirong = my_b.fasong_neirong_url(neirong);
             
            //写入日志 手机号 验证码 start --ghy
            string _sql = string.Empty;
            _sql = " insert into sl_rizhi(yonghuming,miaoshu,ip,leixing,dtime) values('" + shouji + "','" + shoujiyzm + "','" + Request.ServerVariables["LOCAl_ADDR"] + "','发送验证码','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')"; 
            my_c.genxin(_sql);
            
            //写入日志 手机号 验证码 end --ghy


            string fanhui = my_b.duanxing(shouji, shoujiyzm, my_b.get_value("duanxinmobanid", "sl_fasong", "where leixing='短信' and leixingbiaoti='" + leixingbiaoti + "'"));
           

            Response.Write(fanhui);
            Response.End();
        }
        else if (type == "edit")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            try
            {
                int hangshu = Model_dt.Rows.Count;
            }
            catch
            {
                Response.Write("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
                Response.End();
            }
            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }

            try
            {

                sql = sql + " where yonghuming='" + my_b.k_cookie("user_name") + "' and id in (" + my_b.c_string(Request["id"].ToString()) + ")";
            }
            catch
            {
                sql = sql + " where id in (" + my_b.c_string(Request["id"].ToString()) + ")";
            }
            //Response.Write(sql);
            //Response.End();
            my_c.genxin(sql);

        }
        else if (type == "edit_inid")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());

            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");

            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }


            sql = sql + " where  id in (" + Request.QueryString["id"].ToString() + ")";
            //Response.Write(sql);
            //Response.End();
            my_c.genxin(sql);
            //Response.Write("ok");
            //Response.End();
        }
        else if (type == "edit_noyonghuming")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());

            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");

            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }
            string zhanghao = "";
            try
            {
                zhanghao = my_b.k_cookie("admin_id");
            }
            catch { }

            try
            {
                zhanghao = my_b.k_cookie("user_name");
            }
            catch { }

            if (zhanghao == "")
            {
                #region 跳转
                Response.Write("err");
                Response.End();
                #endregion
            }

            sql = sql + " where  id in (" + Request.QueryString["id"].ToString() + ")";
            //Response.Write(sql);
            //Response.End();
            my_c.genxin(sql);
            Response.Write("ok");
            Response.End();
        }
        else if (type == "user_edit")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }

            sql = sql + " where yonghuming='" + my_b.k_cookie("user_name") + "'";
            //Response.Write(sql);
            //Response.End();
            my_c.genxin(sql);
            string tip_string = "修改信息成功！马上跳转到一级页面！";
            try
            {
                tip_string = my_b.set_url_css(Request.QueryString["tip_string"].ToString());
            }
            catch
            { }

            string tipurl = Server.HtmlDecode(Request.UrlReferrer.ToString());
            try
            {
                tipurl = my_b.set_url_css(Request.QueryString["tipurl"].ToString());
            }
            catch
            { }
            //Response.Write(Request.QueryString["tipurl"].ToString());
            //Response.End();
            Response.Redirect("/err.aspx?err=" + tip_string + "&errurl=" + my_b.tihuan(tipurl, "&", "{and}") + "");
        }
        else if (type == "editadd")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }

            sql = sql + " where yonghuming='" + my_b.k_cookie("user_name") + "' and id=" + Request.QueryString["id"].ToString() + "";
            //设置默认
            string moren = Request["moren"].ToString();
            if (moren == "是")
            {
                my_c.genxin("update sl_add set moren='否' where yonghuming='" + my_b.k_cookie("user_name") + "'");
            }

            //end
            my_c.genxin(sql);
            string tip_string = "修改信息成功！马上跳转到一级页面！";
            try
            {
                tip_string = my_b.set_url_css(Request.QueryString["tip_string"].ToString());
            }
            catch
            { }

            string tipurl = Server.HtmlDecode(Request.UrlReferrer.ToString());
            try
            {
                tipurl = my_b.set_url_css(Request.QueryString["tipurl"].ToString());
            }
            catch
            { }
            //Response.Write(Request.QueryString["tipurl"].ToString());
            //Response.End();
            Response.Redirect("/err.aspx?err=" + tip_string + "&errurl=" + my_b.tihuan(tipurl, "&", "{and}") + "");
        }
        else if (type == "diy_edit")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());

            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and u1='" + my_b.set_url_css(Request.QueryString["field"].ToString()) + "'");
            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }
            try
            {

                sql = sql + " where yonghuming='" + my_b.k_cookie("user_name") + "' and id=" + Request.QueryString["id"].ToString() + "";
            }
            catch
            {
                sql = sql + " where yonghuming='" + my_b.k_cookie("user_name") + "'";
            }
            //Response.Write(sql);
            //Response.End();
            my_c.genxin(sql);

        }
        else if (type == "alledit")
        {
            #region 修改全部
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());

            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }
            string field = my_b.c_string(Request["field"].ToString());
            sql = sql + " where yonghuming='" + my_b.k_cookie("user_name") + "' and " + field + "='" + my_b.c_string(Request[field].ToString()) + "'";
            my_c.genxin(sql);
            #endregion 修改全部
        }
        else if (type == "user_pwd")
        {
            #region 修改密码
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }

            sql = sql + " where yonghuming='" + my_b.k_cookie("user_name") + "' and mima='" + my_b.md5(my_b.set_url_css(Request["OldPassword"].ToString())) + "'";
            //Response.Write(sql);
            //Response.End();
            if (my_c.GetTable("select id from " + table_name + " where yonghuming='" + my_b.k_cookie("user_name") + "' and mima='" + my_b.md5(my_b.set_url_css(Request["OldPassword"].ToString())) + "'").Rows.Count == 0)
            {

                tiaozhuan("旧密码错误，请确认。", Request.UrlReferrer.ToString(), "");

            }
            my_c.genxin(sql);
            #region  清除COOKIE
            my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name").ToString() + "','此用户名（" + my_b.k_cookie("user_name").ToString() + "）会员退出成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','会员退出')");

            my_b.admin_o_cookie("user_name");
            my_b.admin_o_cookie("user_pwd");
            #endregion
            tiaozhuan("修改密码成功！请重新登陆！", "", "");
            #endregion 修改密码
        }
        else if (type == "getpass")
        {

            string yonghuming = Request["yonghuming"].ToString();
            string youxiang = "";
            try
            {
                youxiang = my_b.set_url_css(Request["youxiang"].ToString());
            }
            catch { }
            string shouji = "";
            try
            {
                shouji = Request["shouji"].ToString();
            }
            catch { }
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();


            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            string sql = "update " + table_name + " set mima='" + my_b.md5(Request["mima"].ToString()) + "'";
            string sql1 = "select * from " + table_name + "";
            sql = sql + " where  ";
            sql1 = sql1 + " where  ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (Model_dt.Rows[d1]["u1"].ToString() != "mima")
                {
                    if (d1 == 0)
                    {
                        sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                        sql1 = sql1 + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                    }
                    else
                    {
                        sql = sql + " and " + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                        sql1 = sql1 + " and " + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                    }
                }

            }

            //Response.Write(my_c.GetTable(sql1).Rows.Count);
            //Response.End();
            if (my_c.GetTable(sql1).Rows.Count > 0)
            {
                my_c.genxin(sql);
            }
            else
            {
                tiaozhuan("err", "", "");
            }

        }
        else if (type == "login")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql1(Request.Form.ToString()) + ")");

            string user_name = "";
            string user_pwd = "";
            string sql = "select * from " + table_name + " where ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    // user_name = cookie_bd.page_get_kj(Model_dt, d1);
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
                else
                {
                    //   user_pwd = cookie_bd.page_get_kj(Model_dt, d1);
                    sql = sql + " and " + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.page_get_kj(Model_dt, d1) + "";
                }
            }

            DataTable dt = my_c.GetTable(sql);


            if (dt.Rows.Count > 0)
            {

                my_b.c_cookie(dt.Rows[0]["yonghuming"].ToString(), "user_name");
                //微信
                string qqid = "";
                try
                {
                    qqid = my_b.k_cookie("qqid");
                }
                catch { }
                string weiboid = "";
                try
                {
                    weiboid = my_b.k_cookie("weiboid");
                }
                catch { }
                string openid = "";
                try
                {
                    openid = my_b.k_cookie("openid");
                }
                catch { }
                if (qqid != "")
                {
                    my_c.genxin("update sl_user set qqid='" + my_b.k_cookie("qqid") + "',touxiang='" + my_b.k_cookie("touxiang") + "',xingbie='" + my_b.k_cookie("xingbie") + "',xingming='" + my_b.k_cookie("xingming") + "' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                }
                if (weiboid != "")
                {
                    my_c.genxin("update sl_user set weiboid='" + my_b.k_cookie("weiboid") + "',touxiang='" + my_b.k_cookie("touxiang") + "',xingbie='" + my_b.k_cookie("xingbie") + "',xingming='" + my_b.k_cookie("xingming") + "' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                }
                if (openid != "")
                {
                    my_c.genxin("update sl_user set openid='" + my_b.k_cookie("openid") + "',touxiang='" + my_b.k_cookie("touxiang") + "',xingbie='" + my_b.k_cookie("xingbie") + "',xingming='" + my_b.k_cookie("xingming") + "' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                }


                if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("micromessenger") > -1)
                {
                    openid = "";
                    try
                    {
                        openid = my_b.k_cookie("openid");
                    }
                    catch { }
                    if (openid != "")
                    {
                        my_c.genxin("update sl_user set openid='" + my_b.k_cookie("openid") + "',touxiang='" + my_b.k_cookie("touxiang") + "',xingbie='" + my_b.k_cookie("xingbie") + "',xingming='" + my_b.k_cookie("xingming") + "' where yonghuming='" + my_b.k_cookie("user_name") + "'");
                    }




                }

                //end
                my_b.c_cookie(user_pwd, "user_pwd");
                my_b.c_cookie(my_b.get_bianhao(), "user_no");
                my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + user_name + "','此用户名（" + user_name + "）登陆成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','会员登陆')");



            }
            else
            {
                #region 跳转
                tiaozhuan("会员名或者密码错误，请确认后在此登陆", Request.UrlReferrer.ToString(), "");
                #endregion


            }

        }
        else if (type == "del")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());


            my_c.genxin("delete from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ") and yonghuming='" + my_b.k_cookie("user_name") + "'");

        }
        else if (type == "del_noyonghuming")
        {
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());

            string zhanghao = "";
            try
            {
                zhanghao = my_b.k_cookie("admin_id");
            }
            catch { }

            try
            {
                zhanghao = my_b.k_cookie("user_name");
            }
            catch { }

            if (zhanghao == "")
            {
                #region 跳转
                Response.Write("err");
                Response.End();
                #endregion
            }
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string id = Request.QueryString["id"].ToString();
            if (model_table.Rows[0]["u3"].ToString() == "分类模型")
            {
                #region 表模型是分类模型
                DataTable dt = my_c.GetTable("select * from " + table_name + "");
                string[] id_ = id.Split(',');
                for (int i = 0; i < id_.Length; i++)
                {
                    string str = my_hs.get_all_id(dt, id_[i]);
                    if (str != "")
                    {
                        my_c.genxin("delete from " + table_name + " where id in (" + str + ") ");
                    }
                }
                  
                #endregion
            }

            my_c.genxin("delete from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ") ");

        }
        else if (type == "sms")
        {
            #region 处理站内信
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();


            DataTable Model_dt = new DataTable();
            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }
            string yonghuming = Request["yonghuming"].ToString();
            string[] aa = Regex.Split(yonghuming, ",");
            for (int i = 0; i < aa.Length; i++)
            {
                string sql = "insert into " + table_name + " ";
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
                if (model_table.Rows[0]["u3"].ToString() == "文章模型")
                {
                    sql = sql + ",Filepath,classid) values (";
                }
                else
                {
                    sql = sql + ") values (";
                }
                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    if (Model_dt.Rows[d1]["u1"].ToString() == "yonghuming")
                    {
                        if (d1 == 0)
                        {
                            sql = sql + "'" + aa[i].ToString() + "'";
                        }
                        else
                        {
                            sql = sql + ",'" + aa[i].ToString() + "'";
                        }
                    }
                    else
                    {
                        if (d1 == 0)
                        {
                            sql = sql + bd.page_get_kj(Model_dt, d1);
                        }
                        else
                        {
                            sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                        }
                    }

                }
                if (model_table.Rows[0]["u3"].ToString() == "文章模型")
                {
                    DateTime dy = DateTime.Now;
                    Random r = new Random();
                    int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                    string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + my_b.chuli_lujing();
                    sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
                }
                else
                {
                    sql = sql + ")";
                }

                //Response.Write(sql);
                //Response.End();
                my_c.genxin(sql);
            }

            #endregion 处理站内信
        }
        else if (type == "tixian")
        {
            #region 判断登录
            set_login();
            #endregion


            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();


            DataTable Model_dt = new DataTable();
            if (Request.Form.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.Form.ToString()) + ")");
            }

            string sql = "insert into " + table_name + " ";
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
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                sql = sql + ",Filepath,classid) values (";
            }
            else
            {
                sql = sql + ") values (";
            }
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + bd.page_get_kj(Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + bd.page_get_kj(Model_dt, d1);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + my_b.set_url_css(Request.QueryString["classid"].ToString()) + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "'," + my_b.set_url_css(Request.QueryString["classid"].ToString()) + ")";
            }
            else
            {
                sql = sql + ")";
            }

            float yue = 0;
            try
            {
                //  yue = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + my_b.k_cookie("user_name") + "' and zhuangtai='已付款'  and datediff(mm,getdate(),[dtime])<=-1").Rows[0]["count_id"].ToString());
                yue = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + my_b.k_cookie("user_name") + "' and zhuangtai='已付款' ").Rows[0]["count_id"].ToString());
            }
            catch { }


            float jine = float.Parse(Request["jine"].ToString());


            if (yue < jine)
            {
                #region 跳转
                tiaozhuan("余额不足", "", "");
                #endregion

            }
            try
            {
                my_c.genxin(sql);
                my_c.genxin("insert into sl_caiwu(yonghuming,leixing,jine,zhuangtai,miaoshu,dingdanhao) values('" + my_b.k_cookie("user_name") + "','提现',-" + jine + ",'已付款','会员提现，处理金额','" + my_b.c_string(Request["dingdanhao"].ToString()) + "')");
            }
            catch
            {
                Response.Write("err");
                Response.End();
            }
            try
            {
                string article_new_id = my_c.GetTable("select top 1 id from " + table_name + " order by id desc").Rows[0]["id"].ToString();
                string article_log = table_name + "{fenge}" + article_new_id + "{fenge}" + Model_id;
                my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name") + "','" + article_log + "','" + Request.UserHostAddress.ToString() + "','文章审核')");
            }
            catch
            { }


        }
        else
        {

        }
        //完
        #region 设置跳转
        tiaozhuan("", "", "");
        #endregion
    }
    #endregion
    #region 判断来源
    public void set_laiyuan()
    {
        if (Request.UrlReferrer.ToString().ToLower().IndexOf("localhost") == -1)
        {
            if (Request.UrlReferrer.ToString().ToLower().IndexOf(ConfigurationSettings.AppSettings["web_url"].ToString()) == -1)
            {
                Response.Redirect("/err.aspx?err=来源不对。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
            }
        }
          
    }
    #endregion
    #region 拆分Request.QueryString的值，组成where部分
    public string user_sql(string quer_string)
    {
        string return_string = "";
        if (quer_string != "")
        {

            string[] quer_arr = quer_string.Split('&');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (quer_arr[i].ToString().IndexOf("y=") == -1 && quer_arr[i].ToString().IndexOf("x=") == -1)
                {
                    string[] sql_arr = quer_arr[i].ToString().Split('=');
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {

                        if (return_string == "")
                        {
                            return_string = "u1='" + sql_arr[0] + "'";
                        }
                        else
                        {
                            return_string = return_string + " or u1='" + sql_arr[0] + "'";
                        }
                    }
                }
            }
        }

        return return_string;
    }

    public string user_sql1(string quer_string)
    {
        string return_string = "";
        if (quer_string != "")
        {

            string[] quer_arr = quer_string.Split('&');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (quer_arr[i].ToString().IndexOf("y=") == -1 && quer_arr[i].ToString().IndexOf("x=") == -1)
                {
                    string[] sql_arr = quer_arr[i].ToString().Split('=');
                    if (return_string == "")
                    {
                        return_string = "u1='" + sql_arr[0] + "'";
                    }
                    else
                    {
                        return_string = return_string + " or u1='" + sql_arr[0] + "'";
                    }
                }
            }
        }

        return return_string;
    }
    #endregion
    #region 根据类型获取input值，在返回sql需要的
    public string get_kj(string type, string id)
    {
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框")
        {
            return "'" + my_b.c_string(Request[id].ToString()) + "'";
        }
        else if (type == "编辑器" || type == "子编辑器")
        {
            string classid = "0";
            try
            {
                classid = Request["classid"].ToString();
            }
            catch { }
            DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + "");
            string pic_width = "800*0";
            if (sort_dt.Rows.Count > 0)
            {
                pic_width = sort_dt.Rows[0]["u10"].ToString();
            }

            return "'" + my_b.c_string(my_b.set_pic_size(Request[id].ToString(), pic_width)) + "'";
        }
        else if (type == "数字")
        {
            return "" + my_b.c_string(Request[id].ToString()) + "";
        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(my_b.c_string(Request[id].ToString())) + "'";
        }
        else
        {
            return "'" + Request[id].ToString() + "'";
        }
    }

    public string cookie_get_kj(string type, string id)
    {
        if (type == "密码框")
        {
            return "" + my_b.md5(my_b.c_string(Request[id].ToString())) + "";
        }
        else
        {
            return "" + my_b.c_string(Request[id].ToString()) + "";
        }
    }
    #endregion
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Response.Write(Request.Form.ToString());
            //Response.End();




            string type = my_b.set_url_css(Request.QueryString["type"].ToString());
            set_user_sql(type);
        }
    }
}