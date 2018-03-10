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
public partial class appcode : System.Web.UI.Page
{
    biaodan bd = new biaodan();
    my_order my_o = new my_order();
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
        //if (type.IndexOf("add")>-1)
        //{
        //    TimeSpan st = new TimeSpan();
        //    DateTime dy1 = DateTime.Now;
        //    DateTime dy2 = DateTime.Now;
        //    try
        //    {
        //        dy2 = DateTime.Parse(my_c.GetTable("select top 1 * from sl_system where u3='" + Request.UserHostAddress.ToString() + "' and u4='提交操作' order by dtime desc").Rows[0]["dtime"].ToString());

        //        st = dy1.Subtract(dy2);

        //        if (st.TotalMinutes < 1)
        //        {
        //            tiaozhuan("1分内只能操作一次", "/", "");
        //            //  Response.Redirect("/err.aspx?err=1分钟内只能一次。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
        //        }
        //    }
        //    catch
        //    { }
        //}

    }
    #endregion
    #region 记录操作日志
    public void set_system(string type)
    {
        string yonghuming = "匿名";
        try
        {
            yonghuming = my_b.k_cookie("user_name");
        }
        catch { }
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
        if ( t!= "")
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
        else if (tipurl_type == "jsonp")
        {
            string callback = Request["jsoncallback"];

            string result = callback + "({\"neirong\":\"" + tip_string + "\"})";

            Response.Clear();
            Response.Write(result);
            Response.End();
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
  
        DataTable sl_Model = my_c.GetTable("select * from sl_Model where u1='"+ ConfigurationSettings.AppSettings["Prefix"].ToString()+my_b.set_url_css(Request.QueryString["t"].ToString()) + "' and fangwen='是' and (fangwen_type='all' or fangwen_type like '%"+type+"%')");
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


        if (type == "dianzan")
        {
            #region 处理点赞
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();
            DataTable Model_dt = new DataTable();
            if (Request.QueryString.ToString() == "")
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
            }
            else
            {
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
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
            string laiyuanbianhao = Request["laiyuanbianhao"].ToString();
            my_c.genxin("update sl_koubei set dianzan=dianzan+1 where id=" + laiyuanbianhao + "");
            #endregion


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
        if (Request.UrlReferrer.ToString().ToLower().IndexOf(ConfigurationSettings.AppSettings["web_url"].ToString()) == -1)
        {
            Response.Redirect("/err.aspx?err=来源不对。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
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