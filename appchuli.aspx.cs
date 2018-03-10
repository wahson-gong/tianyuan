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

public partial class Execution : System.Web.UI.Page
{
    my_hanshu my_hs = new my_hanshu();
    my_order my_o = new my_order();
    biaodan bd = new biaodan();
  
    #region 判断登录
    public void set_login()
    {
        try
        {
            if (my_b.c_string(Request["yonghuming"].ToString()) == "")
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
                tiaozhuan("验证码输入不正确", "", "");
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
                tiaozhuan("手机号验证码输入不正确", "", "");
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
                tiaozhuan("邮箱验证码输入不正确", "", "");
            }
        }
    }
    #endregion
    #region 处理输入频率问题
    public void shurupinlv()
    {
        TimeSpan st = new TimeSpan();
        DateTime dy1 = DateTime.Now;
        DateTime dy2 = DateTime.Now;
        try
        {
            dy2 = DateTime.Parse(my_c.GetTable("select top 1 * from sl_system where u3='" + Request.UserHostAddress.ToString() + "' and u4='提交操作' order by dtime desc").Rows[0]["dtime"].ToString());

            st = dy1.Subtract(dy2);

            if (st.TotalMilliseconds < 1)
            {
              //  tiaozhuan(st.TotalMilliseconds.ToString(), "/", "");
                //  Response.Redirect("/err.aspx?err=1分钟内只能一次。&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
            }
        }
        catch
        { }
    }
    #endregion
    #region 记录操作日志
    public void set_system(string type)
    {
        string yonghuming = my_b.get_yonghuming();

        string u3 = "";
        try
        {
            u3 = Request.UrlReferrer.ToString();
        }
        catch
        {
            u3 = Request.Url.ToString();
        }
       // yonghuming = yonghuming + "[" + type + "]";
        my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi (yonghuming,miaoshu,ip,leixing) values(" + yonghuming + ",'" + u3 + "','" + Request.UserHostAddress.ToString() + "','提交操作')");
    }
    #endregion
    #region 记录文章审核
    public void set_shenhe(string table_name, string Model_id)
    {
        string yonghuming = my_b.get_yonghuming();
        string article_new_id = my_c.GetTable("select top 1 id from " + table_name + " order by id desc").Rows[0]["id"].ToString();
        string article_log = table_name + "{fenge}" + article_new_id + "{fenge}" + Model_id;

        my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi (yonghuming,miaoshu,ip,leixing) values(" + yonghuming + ",'" + article_log + "','" + Request.UserHostAddress.ToString() + "','文章审核')");
    }
    #endregion
    #region 设置跳转
    public void tiaozhuan(string tip_string, string tipurl, string tipurl_type)
    {
        if (tip_string == "")
        {
            try
            {
                tip_string = Request.QueryString["tip_string"].ToString();
            }
            catch {
                tip_string = "ok";
            }
        }
       
        string callback = Request["jsoncallback"];

        string result = callback + "({\"neirong\":\"" + tip_string + "\"})";

        Response.Clear();
        Response.Write(result);
        Response.End();
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
    #region API接口验证
    public void set_api(string Model_id, string type)
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
                DataTable Model_dt = new DataTable();
                if (Request.QueryString.ToString() == "")
                {
                    Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + "  order by u9,id");
                }
                else
                {
                    Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")  order by u9,id");
                }

                string appid = my_c.GetTable("select top 1 * from sl_xitongpeizhi").Rows[0]["appid"].ToString();
                string _sign = my_api.SampleCode.test(Model_dt, timestamp, appid);
                //HttpContext.Current.Response.Write(_sign);
                //HttpContext.Current.Response.End();
                if (sign == _sign)
                {
                    //tiaozhuan("1", "", "");
                }
                else
                {
                    tiaozhuan("0", "", "");
                }
            }
           
        }
       
    }
    #endregion
    my_liucheng my_lc = new my_liucheng();
    #region 整体输入提交
    public void set_user_sql(string type)
    {
        #region 处理验证码
        set_yanzhengma();
        #endregion
        #region 处理输入频率问题
        shurupinlv();
        #endregion
        #region 记录操作日志
        set_system(type);
        #endregion

        string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
        DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
        string Model_id = model_table.Rows[0]["id"].ToString();
        #region API接口验证
        set_api(Model_id,type);
        #endregion
        if (type == "add")
        {
            #region 增加信息
          
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
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion
            #endregion
        }
        else if (type == "yuangong_add")
        {
            #region 增加信息

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
                    #region 跳转
                    tiaozhuan("err", "", "");
                    #endregion
                }
            }
            try
            {
                if (Request["yonghuming"].ToString() != "")
                {
                    if (my_c.GetTable("select * from " + table_name + " where yonghuming='" + Request["yonghuming"].ToString() + "'").Rows.Count > 0)
                    {
                        #region 跳转
                        tiaozhuan("err", "", "");
                        #endregion

                    }
                }
            }
            catch { }
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
            //头像开始
            DataTable sl_yuangong = my_c.GetTable("select * from sl_yuangong where id=" + id);
            if (sl_yuangong.Rows.Count > 0)
            {
                if (sl_yuangong.Rows[0]["yuangongxingming"].ToString() != "")
                {
                    string touxiang = my_b.CreateImage(sl_yuangong.Rows[0]["yuangongxingming"].ToString(), true, 12);
                    my_c.genxin("update sl_yuangong set yuangongtouxiang='" + touxiang + "' where id=" + id);
                }
               
            }
            //头像完成
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion
            #endregion
        }
        if (type == "add_getid")
        {
            #region 增加信息，返回此条ID号

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
            tiaozhuan(id, "", "");
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion
            #endregion
        }
        if (type == "add_qiandan")
        {
            #region 增加信息，返回此条ID号

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
          
            //end
            tiaozhuan(id, "", "");
            #endregion
        }
        else if (type == "cookie")
        {
            #region 生成COOKIE
            string cookie_name = Request.QueryString["cookie_name"].ToString();
            string cookie_value = Request.QueryString["cookie_value"].ToString();
            string tipurl = Request.QueryString["tipurl"].ToString();
            my_b.c_cookie(cookie_value, cookie_name);
            tiaozhuan("ok", "", "");
            #endregion
        }
        else if (type == "addadd")
        {
            #region 增加地址


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

            //设置默认
            string moren = Request["moren"].ToString();
            if (moren == "是")
            {
                my_c.genxin("update sl_add set moren='否' where yonghuming='" + my_b.c_string(Request["yonghuming"].ToString()) + "'");
            }

            //end

            try
            {
                my_c.genxin(sql);
            }
            catch
            {
                tiaozhuan("err", "", "");
            }
            #region 记录文章审核
            set_shenhe(table_name, Model_id);
            #endregion
            #endregion
        }
        else if (type == "editadd")
        {
            #region 修改地址

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
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

            sql = sql + " where yonghuming='" + my_b.c_string(Request["yonghuming"].ToString()) + "' and id=" + Request.QueryString["id"].ToString() + "";
            //设置默认
            string moren = Request["moren"].ToString();
            if (moren == "是")
            {
                my_c.genxin("update sl_add set moren='否' where yonghuming='" + my_b.c_string(Request["yonghuming"].ToString()) + "'");
            }

            //end
            my_c.genxin(sql);
            #endregion
        }
        else if (type == "useradd")
        {
            #region 会员注册



            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
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
           
            //end

            try
            {
                if (Request["youxiang"].ToString() != "")
                {
                    if (my_c.GetTable("select * from " + table_name + " where youxiang='" + Request["youxiang"].ToString() + "'").Rows.Count > 0)
                    {
                        #region 跳转
                        tiaozhuan("err", "", "");
                        #endregion

                    }
                }
            }
            catch { }

           

            //sql
            my_c.genxin(sql);
            //end
          //  my_b.c_cookie(Request["yonghuming"].ToString(), "user_name");

         //   my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + Request["yonghuming"].ToString() + "','此用户名（" + Request["yonghuming"].ToString() + "）注册并登陆!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','会员登陆')");
            #endregion
        }
        else if (type == "edit_qiandan")
        {
            #region 修改ID
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");

            if (Model_dt.Rows.Count > 0)
            {
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

                sql = sql + " where  id=" + Request.QueryString["id"].ToString() + "";
                //Response.Write(sql);
                //Response.End();
                my_c.genxin(sql);
                #region 流程
                my_lc.set_liucheng("签单", Request.QueryString["id"].ToString(),"");
                //  Response.End();
                #endregion
            }
            else
            {
                #region 跳转
                tiaozhuan("err", "", "");
                #endregion
            }

            #endregion
        }
        else if (type == "editid")
        {
            #region 修改ID
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
           
            if (Model_dt.Rows.Count>0)
            {
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

                sql = sql + " where  id=" + Request.QueryString["id"].ToString() + "";
                //Response.Write(sql);
                //Response.End();
                my_c.genxin(sql);
            }
            else
            {
                #region 跳转
                tiaozhuan("err", "", "");
                #endregion
            }

            #endregion
        }
        else if (type == "user_edit")
        {
            #region 会员资料修改

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
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

            sql = sql + " where yonghuming='" + my_b.c_string(Request["yonghuming"].ToString()) + "'";
            //Response.Write(sql);
            //Response.End();
            my_c.genxin(sql);
            #endregion
        }
        else if (type == "user_pwd")
        {
            #region 修改密码

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
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

            sql = sql + " where id in (" + my_b.k_cookie("user_name") + ") and mima='" + my_b.md5(my_b.set_url_css(Request["OldPassword"].ToString())) + "'";
            //Response.Write(sql);
            //Response.End();
            if (my_c.GetTable("select id from " + table_name + " where id in (" + my_b.k_cookie("user_name") + ") and mima='" + my_b.md5(my_b.set_url_css(Request["OldPassword"].ToString())) + "'").Rows.Count == 0)
            {
                tiaozhuan("err", "", "");

            }
            my_c.genxin(sql);
            #endregion
        }
        else if (type == "getpass")
        {
            #region 判断短信或邮箱验证码
            string shoujiyzm = "";
            try
            {
                shoujiyzm = Request["shoujiyzm"].ToString();
            }
            catch { }
            string youxiangyzm = "";
            try
            {
                youxiangyzm = Request["youxiangyzm"].ToString();
            }
            catch { }
            if (youxiangyzm == "" && shoujiyzm == "")
            {
                tiaozhuan("err", "", "");
            }
            #endregion
            #region 找回密码
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

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
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

            string tip_string = "找回密码成功！请重新登陆！";
            try
            {
                tip_string = my_b.set_url_css(Request.QueryString["tip_string"].ToString());
            }
            catch
            { }

            string tipurl = "/inc/out_login.aspx";
            try
            {
                tipurl = my_b.set_url_css(Request.QueryString["tipurl"].ToString());
            }
            catch
            { }
            tiaozhuan(tip_string, tipurl, "");
            #endregion
        }
        else if (type == "login")
        {
            #region 登录

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql1(Request.QueryString.ToString()) + ")");

            string user_name = "";
            string user_pwd = "";
            string sql = "select * from " + table_name + " where (yonghuming='" + my_b.c_string(Request["yonghuming"].ToString()) + "' or shoujihaoma='" + my_b.c_string(Request["yonghuming"].ToString()) + "') and mima='" + my_b.md5(my_b.c_string(Request["mima"].ToString())) + "'";
        

            DataTable dt = my_c.GetTable(sql);
            //Response.Write(sql);
            //Response.End();

            if (dt.Rows.Count > 0)
            {
                my_b.c_cookie(dt.Rows[0]["id"].ToString(), "user_name");
                my_b.c_cookie(my_b.c_string(Request["mima"].ToString()), "user_pwd");
                my_b.c_cookie("思乐科技", "gongshiming");
                my_b.c_cookie(my_c.GetTable("select top 1 * from sl_xitongpeizhi").Rows[0]["appid"].ToString(), "appid");
                my_c.genxin("update sl_yuangong set zuijindenglushijian='" + DateTime.Now.ToString() + "',zaixianzhuangtai='在线' where id=" + dt.Rows[0]["id"].ToString());
                //     my_b.c_cookie(my_b.get_bianhao(), "user_no");

                my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi (yonghuming,miaoshu,ip,leixing) values('" +my_b.k_cookie("user_name") + "','此用户名（" + my_b.k_cookie("user_name") + "）登陆成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','会员登陆')");
            }
            else
            {
                #region 跳转
                tiaozhuan("err", "", "");
                #endregion
            }
            #endregion
        }
        else if (type == "youxiang_yzm")
        {
            #region 发送邮件验证码
            string youxiang = my_b.set_url_css(Request["youxiang"].ToString());
            string leixingbiaoti = my_b.set_url_css(Request["leixingbiaoti"].ToString());

            //找回密码

            Random r = new Random();
            int Num1 = Convert.ToInt32(r.Next(999999)) + 100000;
            string shoujiyzm = Num1.ToString();
            DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");

            if (my_c.GetTable("select id from " + table_name + " where youxiang='" + youxiang + "'").Rows.Count == 0)
            {
                tiaozhuan("err", "", "");
            }

            string yx_biaoti = my_b.get_value("biaoti", ConfigurationSettings.AppSettings["Prefix"].ToString() + "fasong", "where leixing='邮件' and leixingbiaoti='" + leixingbiaoti + "'");
            string yx_neirong = my_b.get_value("youjianneirong", ConfigurationSettings.AppSettings["Prefix"].ToString() + "fasong", "where leixing='邮件' and leixingbiaoti='" + leixingbiaoti + "'");

            try
            {
                int sta = my_b.WebMailTo(youxiang, yx_biaoti.Replace("$username", youxiang), yx_neirong.Replace("$username", youxiang).Replace("{yzm}", shoujiyzm));

                my_b.c_cookie(shoujiyzm, "youxiangyzm");
                my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + youxiang + "','此会员（" + youxiang + "）登陆成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','" + leixingbiaoti + "')");
                if (sta == 0)
                {
                    tiaozhuan(sta.ToString(), "", "");
                }
                else
                {
                    tiaozhuan("ok", "", "");
                }
            }
            catch
            {
                tiaozhuan("err", "", "");
            }
            #endregion
        }
        else if (type == "shouji_yzm")
        {
            #region 发送短信，需要类型标题、手机号

            string leixingbiaoti = my_b.set_url_css(Request["leixingbiaoti"].ToString());
            DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");
            string gs_name = xml_dt.Rows[0]["u1"].ToString();
            string shouji = Request.QueryString["shouji"].ToString();
            if (my_c.GetTable("select id from " + table_name + " where shouji='" + shouji + "'").Rows.Count == 0)
            {
                tiaozhuan("err", "", "");
            }
            Random r = new Random();
            int Num1 = Convert.ToInt32(r.Next(900000)) + 100000;
            try
            {
                string shoujiyzm = Num1.ToString();
                my_b.c_cookie(shoujiyzm, "shoujiyzm");

                string fanhui = my_b.duanxing(shouji, my_b.get_value("duanxinneirong", ConfigurationSettings.AppSettings["Prefix"].ToString() + "fasong", "where leixing='短信' and leixingbiaoti='" + leixingbiaoti + "'").Replace("{yzm}", shoujiyzm), my_b.get_value("duanxinmobanid", ConfigurationSettings.AppSettings["Prefix"].ToString() + "fasong", "where leixing='短信' and leixingbiaoti='" + leixingbiaoti + "'"));
                tiaozhuan(shoujiyzm, "", "");
            }
            catch
            {
                tiaozhuan("err", "", "");
            }
            #endregion
        }
        else if (type == "del")
        {
            #region 删除，需要ID和用户名同时存在

            my_c.genxin("delete from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ") and yonghuming='" + my_b.c_string(Request["yonghuming"].ToString()) + "'");
            #endregion
        }
        else if (type == "alledit")
        {
            #region 修改全部,要修改的值，field是条件

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")");
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
        else if (type == "out_login")
        {
            #region 退出登录
            string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString()); //用户名
            DataTable sl_yuangong = my_c.GetTable("select * from sl_yuangong where id in (" + my_b.c_string(yonghuming) + ") or yonghuming='" + my_b.c_string(yonghuming) + "'");
            if (sl_yuangong.Rows.Count > 0)
            {
                my_c.genxin("update sl_yuangong set zaixianzhuangtai='离线' where id=" + sl_yuangong.Rows[0]["id"].ToString());

                try
                {
                    my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi (yonghuming,miaoshu,ip,leixing) values('" + yonghuming + "','此用户名（" + yonghuming + "）正在线上!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','会员离线')");

                    my_b.admin_o_cookie("user_name");
                    my_b.admin_o_cookie("user_pwd");
                    my_b.admin_o_cookie("user_no");
                }
                catch { }

                my_b.getWebFile("/calltel.aspx?type=zhuangtai&user=" + sl_yuangong.Rows[0]["zuoxihao"].ToString() + "&dnd=2&action=1");
                #region 跳转
                tiaozhuan("ok", "", "");
                #endregion

            }
            else
            {
                #region 跳转
                tiaozhuan("err", "", "");
                #endregion
            }
            #endregion
        }
        else
        {

        }
        //完
        #region 跳转
        tiaozhuan("", "", "");
        #endregion
    }
    #endregion
    #region 判断来源
    public void set_laiyuan()
    {
        if (Request.UrlReferrer.ToString().ToLower().IndexOf(ConfigurationSettings.AppSettings["web_url"].ToString()) == -1)
        {
            tiaozhuan("来源不对", "", "");
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
            //Response.Write(Request.QueryString.ToString());
            //Response.End();




            string type = my_b.set_url_css(Request.QueryString["type"].ToString());
            set_user_sql(type);
        }
    }
}