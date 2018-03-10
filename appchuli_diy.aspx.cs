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
public partial class appchuli_diy : System.Web.UI.Page
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
        my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi (yonghuming,miaoshu,ip,leixing) values('" + yonghuming + "','" + u3 + "','" + Request.UserHostAddress.ToString() + "','提交操作')");
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

        #region 自定义的提交操作
        if (type == "add")
        {

        }
        else if (type == "tuijian")
        {
            #region 签单退件 
            string laiyuanbianhao = my_b.c_string(Request["laiyuanbianhao"].ToString()); //签单ID
            string zhuangtai = my_b.c_string(Request["zhuangtai"].ToString()); //签单状态
            DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id in (" + laiyuanbianhao + ")");
            if (sl_qiandan.Rows[0]["zhuangtai"].ToString() == "待接单")
            {
               // my_c.genxin("update sl_zuodan set shanchuzhuangtai=-1 where qiandan in (" + laiyuanbianhao + ")");
            }
            my_c.genxin("update sl_qiandan set zhuangtai='退单中' where id=" + laiyuanbianhao + "");
            my_lc.set_liucheng("退件", laiyuanbianhao, "");

            #endregion
        }
        else if (type == "wancheng")
        {
            #region 签单完成 
            string laiyuanbianhao = my_b.c_string(Request["laiyuanbianhao"].ToString()); //签单ID
            string zhuangtai = my_b.c_string(Request["zhuangtai"].ToString()); //签单状态
            DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id in (" + laiyuanbianhao + ")");

            my_c.genxin("update sl_qiandan set zhuangtai='结算中',fenzu='完单' where id=" + laiyuanbianhao + "");

            my_lc.set_liucheng("完成", laiyuanbianhao, "");

            #endregion
        }
        else if (type == "tuidan")
        {
            #region 申请退单 
            string laiyuanbianhao = my_b.c_string(Request["laiyuanbianhao"].ToString()); //签单ID
            string zhuangtai = my_b.c_string(Request["zhuangtai"].ToString()); //签单状态
            DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id in (" + laiyuanbianhao + ")");

            my_c.genxin("update sl_qiandan set zhuangtai='退单中' where id=" + laiyuanbianhao + "");

            my_lc.set_liucheng("退单", laiyuanbianhao, "");

            #endregion
        }
        else if (type == "zuodan")
        {
            #region 做单流程审核
            string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
            DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
            string Model_id = model_table.Rows[0]["id"].ToString();
            #region API接口验证
            set_api(Model_id, type);
            #endregion
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
                #region 处理状态
                string zhuangtai = my_b.c_string(Request["zhuangtai"]);
                DataTable sl_zuodan = my_c.GetTable("select * from sl_zuodan where id=" + my_b.c_string(Request.QueryString["id"].ToString()) + "");
                string qiandan = sl_zuodan.Rows[0]["qiandan"].ToString();
                DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id=" + qiandan);
                if (zhuangtai == "确认放款")
                {
                    #region 申请放款，给财务发代办
                    //  DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id=" + qiandan);
                    //  DataTable sl_kehu = my_c.GetTable("select * from sl_kehu where id in (" + sl_qiandan.Rows[0]["kehu"].ToString() + ")");
                    //string  beizhu = sl_kehu.Rows[0]["xingming"].ToString() + "贷款的"+ sl_zuodan.Rows[0]["fangkuanjine	"].ToString() + "万已通过银行审核，申请放款。";
                    //  string wangzhi = "";//通知签单人的网址
                    //  string yonghuming = "";
                    //  my_lc.daibanshixiang(beizhu, "签单", DateTime.Now.ToString(), DateTime.Now.ToString(), "未处理", my_b.k_cookie("user_name"), DateTime.Now.ToString(), "系统", yonghuming, wangzhi); //通知签单人

                    my_lc.set_liucheng("做单", qiandan, "");
                    #endregion
                }
                //if (zhuangtai == "完成")
                //{
                //    #region 做单完成
                //    string sta = "";
                //    DataTable sl_zuodan1 = my_c.GetTable("select * from sl_zuodan where qiandan=" + qiandan + "");
                //    if (sl_zuodan1.Rows.Count > 1)
                //    {
                //        for (int i = 0; i < sl_zuodan1.Rows.Count; i++)
                //        {
                //            if (sl_zuodan1.Rows[i]["zhuangtai"].ToString() == "待接单" || sl_zuodan1.Rows[i]["zhuangtai"].ToString() == "做单中" || sl_zuodan1.Rows[i]["zhuangtai"].ToString() == "暂停" || sl_zuodan1.Rows[i]["zhuangtai"].ToString() == "渠道审核通过" || sl_zuodan1.Rows[i]["zhuangtai"].ToString() == "申请放款" || sl_zuodan1.Rows[i]["zhuangtai"].ToString() == "批准放款" || sl_zuodan1.Rows[i]["zhuangtai"].ToString() == "不批准放款")
                //            {
                //                sta = "yes";
                //            }

                //        }
                //    }
                //    //Response.Write(sta+"1");
                //    //Response.End();
                //    if (sta == "")
                //    {
                //        #region 为空代表此签单所有完成
                //        my_lc.set_liucheng("完成", qiandan, "");
                //        #endregion
                //    }
                //    else
                //    {
                //        #region 不为空给后台总监发消息
                //        DataTable sl_shenheliucheng = my_c.GetTable("select * from sl_shenheliucheng where classid in (select id from sl_shenheliucheng where biaoti='完成') and qiyong='是' and biaoti<>'特别授权' order by paixu asc,id asc");
                //        string yonghuming = my_lc.shenhe_yhq(sl_shenheliucheng, 0, sl_qiandan);
                //        DataTable sl_kehu = my_c.GetTable("select * from sl_kehu where id in (" + sl_qiandan.Rows[0]["kehu"].ToString() + ")");
                //        string beizhu = sl_kehu.Rows[0]["xingming"].ToString() + "贷款的" + sl_zuodan.Rows[0]["fangkuanjine"].ToString() + "万，银行已放款，任务完成。";
                //        string wangzhi = "";//通知签单人的网址
                //        my_lc.daibanshixiang(beizhu, "签单", DateTime.Now.ToString(), DateTime.Now.ToString(), "未处理", my_b.k_cookie("user_name"), DateTime.Now.ToString(), "系统", yonghuming, wangzhi); //通知签单人
                //        #endregion
                //    }

                //    #endregion
                //}
                if (zhuangtai == "申请退单")
                {
                    #region 为空代表此签单所有完成
                    string sta = "";
                    DataTable sl_zuodan1 = my_c.GetTable("select * from sl_zuodan where qiandan=" + qiandan + "");
                    if (sl_zuodan1.Rows.Count > 1)
                    {
                        for (int i = 0; i < sl_zuodan1.Rows.Count; i++)
                        {
                            if (sl_zuodan1.Rows[i]["zhuangtai"].ToString() != "退单中")
                            {
                                sta = "yes";
                            }

                        }
                    }
                    if (sta == "")
                    {
                        #region 为空代表此签单所有退单
                        my_lc.set_liucheng("退单", qiandan, "");
                        #endregion
                    }
                    else
                    {
                        #region 不为空给后台总监发消息
                        DataTable sl_shenheliucheng = my_c.GetTable("select * from sl_shenheliucheng where classid in (select id from sl_shenheliucheng where biaoti='完成') and qiyong='是' and biaoti<>'特别授权' order by paixu asc,id asc");
                        string yonghuming = my_lc.shenhe_yhq(sl_shenheliucheng, 0, sl_qiandan.Rows[0]["yonghuming"].ToString());
                        DataTable sl_kehu = my_c.GetTable("select * from sl_kehu where id in (" + sl_qiandan.Rows[0]["kehu"].ToString() + ")");
                        string beizhu = sl_kehu.Rows[0]["xingming"].ToString() + "贷款的" + sl_zuodan.Rows[0]["fangkuanjine	"].ToString() + "万，申请退单，任务失败。";
                        string wangzhi = "";//通知签单人的网址
                        my_lc.daibanshixiang(beizhu, "签单", DateTime.Now.ToString(), DateTime.Now.ToString(), "未处理", my_b.k_cookie("user_name"), DateTime.Now.ToString(), "系统", yonghuming, wangzhi); //通知签单人
                        #endregion
                    }

                    #endregion
                }
                #endregion
            }
            else
            {
                #region 跳转
                tiaozhuan("err", "", "");
                #endregion
            }

            //zhuangtai end
            #endregion
        }
        else if (type == "chongfenpei")
        {
            #region 签单重分配
            string laiyuanbianhao = my_b.c_string(Request["laiyuanbianhao"].ToString()); //签单ID
            string zuodan_yonghuming = my_b.c_string(Request["zuodan_yonghuming"].ToString());//做单人
            string zuodan_leixing = my_b.c_string(Request["zuodan_leixing"].ToString()); //做单类型
            string zuodan_zhuangtai = my_b.c_string(Request["zuodan_zhuangtai"].ToString()); //做单状态

            DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id in (" + laiyuanbianhao + ")");

            if (sl_qiandan.Rows[0]["zhuangtai"].ToString() == "待接单")
            {
                my_c.genxin("update sl_zuodan set shanchuzhuangtai=-1 where qiandan in (" + laiyuanbianhao + ")");
            }

            my_c.genxin("update sl_qiandan set zhuangtai='待接单' where id=" + laiyuanbianhao);


            string[] yonghuming_ = zuodan_yonghuming.Split(',');
            for (int j = 0; j < yonghuming_.Length; j++)
            {
                my_c.genxin("insert into sl_zuodan(qiandan,yonghuming,leixing,chuliren,zhuangtai) values(" + laiyuanbianhao + "," + yonghuming_[j] + ",'" + zuodan_leixing + "'," + sl_qiandan.Rows[0]["yonghuming"].ToString() + ",'" + zuodan_zhuangtai + "')");//插入做单


                my_c.genxin("insert into sl_rizhi(yonghuming,miaoshu,ip,yemiandizhi,leixing,biaoming,bianhao,jieshouren) values(" + my_b.k_cookie("user_name") + ",'重分配给','" + Request.UserHostAddress.ToString() + "','" + Request.Url.ToString() + "','签单','sl_qiandan'," + laiyuanbianhao + "," + yonghuming_[j] + ")"); //插入日志
            }


            //end
            #endregion
        }
        else if (type == "shenheliucheng")
        {
            #region 审核流程控制
            //前台传入参数 
            #region 前台传入参数
            string zhuangtai = my_b.c_string(Request["zhuangtai"].ToString()); //流程状态
            string id = my_b.c_string(Request["id"].ToString()); //流程ID
            string beizhu = my_b.c_string(Request["beizhu"].ToString()); //备注
            string wangzhi = "";//操作网址,选填
            try
            {
                wangzhi = my_b.c_string(Request["wangzhi"].ToString());
            }
            catch { }
            string zuodanid = ""; //做单ID,选填
            try
            {
                zuodanid = my_b.c_string(Request["zuodanid"].ToString());
            }
            catch { }

            #endregion
            DataTable sl_shenhe = my_c.GetTable("select * from sl_shenhe where id in (" + id + ")");
            string laiyuanbianhao = sl_shenhe.Rows[0]["laiyuanbianhao"].ToString();
            string leixing = sl_shenhe.Rows[0]["leixing"].ToString();
            string liuchengbianhao = sl_shenhe.Rows[0]["liuchengbianhao"].ToString();


            DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id in (" + laiyuanbianhao + ")");
            string yewuquerenfangshi = sl_qiandan.Rows[0]["yewuquerenfangshi"].ToString();
            DataTable sl_shenheliucheng = my_c.GetTable("select * from sl_shenheliucheng where id in (" + liuchengbianhao + ")");
            DataTable sl_zuodan = new DataTable();
            if (zuodanid != "")
            {
                sl_zuodan = my_c.GetTable("select * from sl_zuodan where id=" + zuodanid + "");
            }
            #region 开始判断
            if (zhuangtai == "未通过")
            {
                #region 不通过
                my_c.genxin("update sl_shenhe set zhuangtai='" + zhuangtai + "' , beizhu='" + beizhu + "',xianshi='否' where id=" + sl_shenhe.Rows[0]["id"].ToString() + "");
                string qiandanzhuangtai = sl_shenheliucheng.Rows[0]["butongguoqiandanzhuangtai"].ToString();//签单状态,选填

                string fenzu = sl_shenheliucheng.Rows[0]["butongguoqiandanfenzu"].ToString();//签单分组,选填

                string zuodanzhuangtai = sl_shenheliucheng.Rows[0]["butongguozuodanzhuangtai"].ToString();//做单状态,选填


                if (qiandanzhuangtai != "")
                {
                    my_c.genxin("update sl_qiandan set zhuangtai='" + qiandanzhuangtai + "' where id=" + laiyuanbianhao);
                }
                if (fenzu != "")
                {
                    my_c.genxin("update sl_qiandan set fenzu='" + fenzu + "' where id=" + laiyuanbianhao);
                }
                if (zuodanid != "" && zuodanzhuangtai != "")
                {
                    my_c.genxin("update sl_zuodan set zhuangtai='" + zuodanzhuangtai + "' where id=" + zuodanid);
                }




                #region 给上一级人发消息
                beizhu = my_lc.liucheng_daiban(sl_shenheliucheng.Rows[0]["butongguotishi"].ToString(), sl_qiandan, sl_zuodan, sl_shenheliucheng, sl_shenhe) + "，说明：" + beizhu;
                string yonghuming = ""; //收件人
                if (liuchengbianhao == "")
                {

                }
                else
                {
                    DataTable shenhe_id = my_lc.shenhe_id(laiyuanbianhao, id, sl_shenhe.Rows[0]["liuchengbianhao"].ToString(), "shang");
                    for (int i = 0; i < shenhe_id.Rows.Count; i++)
                    {
                        if (yonghuming == "")
                        {
                            yonghuming = shenhe_id.Rows[i]["yonghuming"].ToString();
                        }
                        else
                        {
                            yonghuming = yonghuming + "," + shenhe_id.Rows[i]["yonghuming"].ToString();
                        }
                    }
                }

                my_lc.daibanshixiang(beizhu, "签单", DateTime.Now.ToString(), DateTime.Now.ToString(), "未处理", my_b.k_cookie("user_name"), DateTime.Now.ToString(), "系统", yonghuming, wangzhi); //通知签单人
                #endregion
                my_lc.chongfenpei(id, laiyuanbianhao, beizhu, zhuangtai);//重新分配审核的功能
                if (liuchengbianhao == "12")
                {
                    my_lc.set_liucheng("退单不通过", laiyuanbianhao, "");
                }
                #endregion
            }
            else
            {
                #region 通过
                string shifou = ""; //是否有存在的记录
                #region 处理下一次事务 
                DataTable dt1 = new DataTable();
                #region 待审核签单的特别授权处理
                if (yewuquerenfangshi.IndexOf("特别授权") > -1)
                {
                    dt1 = my_c.GetTable("select  * from sl_shenheliucheng where classid in (select id from sl_shenheliucheng where biaoti='" + leixing + "' and classid=0) and qiyong='是' order by paixu asc,id desc");

                }
                else
                {
                    dt1 = my_c.GetTable("select  * from sl_shenheliucheng where classid in (select id from sl_shenheliucheng where biaoti='" + leixing + "' and classid=0) and qiyong='是' and biaoti<>'特别授权' order by paixu asc,id desc");

                }
                #endregion

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i]["id"].ToString() == sl_shenheliucheng.Rows[0]["id"].ToString())
                    {
                        shifou = "yes";
                    }

                    if (shifou == "yes")
                    {
                        DataTable dt2 = my_c.GetTable("select * from sl_shenhe where laiyuanbianhao in(" + laiyuanbianhao + ") and liuchengbianhao=" + dt1.Rows[i]["id"].ToString() + " and id<>" + id + "");
                        if (dt2.Rows.Count > 0)
                        {
                            my_c.genxin("delete sl_shenhe where id=" + dt2.Rows[0]["id"].ToString());
                            //  Response.Write("有记录了");
                            // shifou = "yes";
                            break;
                        }
                    }

                }
                #endregion
                #region 本流程只有一条记录
                my_c.genxin("update sl_shenhe set zhuangtai='" + zhuangtai + "' , beizhu='" + beizhu + "' where id=" + sl_shenhe.Rows[0]["id"].ToString() + "");
                //Response.Write("update sl_shenhe set zhuangtai='" + zhuangtai + "' , beizhu='" + beizhu + "' where id=" + sl_shenhe.Rows[0]["id"].ToString() + "");
                //Response.End();
                #region 签单和做单的状态
                string qiandanzhuangtai = sl_shenheliucheng.Rows[0]["tongguoqiandanzhuangtai"].ToString();//签单状态,选填

                string fenzu = sl_shenheliucheng.Rows[0]["tongguoqiandanfenzu"].ToString();//签单分组,选填

                string zuodanzhuangtai = sl_shenheliucheng.Rows[0]["tongguozuodanzhuangtai"].ToString();//做单状态,选填
                if (qiandanzhuangtai != "")
                {
                    my_c.genxin("update sl_qiandan set zhuangtai='" + qiandanzhuangtai + "' where id=" + laiyuanbianhao);
                }
                if (fenzu != "")
                {
                    my_c.genxin("update sl_qiandan set fenzu='" + fenzu + "' where id=" + laiyuanbianhao);
                }
                if (zuodanid != "" && zuodanzhuangtai != "")
                {
                    my_c.genxin("update sl_zuodan set zhuangtai='" + zuodanzhuangtai + "' where id=" + zuodanid);
                }
                #endregion
                my_lc.set_liucheng(leixing, laiyuanbianhao, "xia");
                #region 给上一级人发消息
                beizhu = my_lc.liucheng_daiban(sl_shenheliucheng.Rows[0]["tongguotishi"].ToString(), sl_qiandan, sl_zuodan, sl_shenheliucheng, sl_shenhe) + "，说明：" + beizhu;
                string yonghuming = ""; //收件人
                if (liuchengbianhao == "5"|| liuchengbianhao == "24")
                {
                    #region 派单
                    string zuodan_yonghuming = my_b.c_string(Request["zuodan_yonghuming"].ToString());//做单人

                    string zuodan_leixing = my_b.c_string(Request["zuodan_leixing"].ToString());//做单类型
                    string zuodan_zhuangtai = my_b.c_string(Request["zuodan_zhuangtai"].ToString());//做单状态

                    yonghuming = zuodan_yonghuming;
                    string[] yonghuming_ = zuodan_yonghuming.Split(',');
                    for (int j = 0; j < yonghuming_.Length; j++)
                    {
                        my_c.genxin("insert into sl_zuodan(qiandan,yonghuming,leixing,chuliren,zhuangtai) values(" + laiyuanbianhao + "," + yonghuming_[j] + ",'" + zuodan_leixing + "'," + sl_qiandan.Rows[0]["yonghuming"].ToString() + ",'" + zuodan_zhuangtai + "')");
                    }
                    #endregion
                }
                else
                {

                    DataTable shenhe_id = my_lc.shenhe_id(laiyuanbianhao, id, sl_shenhe.Rows[0]["liuchengbianhao"].ToString(), "shang");

                    for (int i = 0; i < shenhe_id.Rows.Count; i++)
                    {
                        if (yonghuming == "")
                        {
                            yonghuming = shenhe_id.Rows[i]["yonghuming"].ToString();
                        }
                        else
                        {
                            yonghuming = yonghuming + "," + shenhe_id.Rows[i]["yonghuming"].ToString();
                        }
                    }
                }
                //Response.Write(yonghuming);
                //Response.End();
                my_lc.daibanshixiang(beizhu, "签单", DateTime.Now.ToString(), DateTime.Now.ToString(), "未处理", my_b.k_cookie("user_name"), DateTime.Now.ToString(), "系统", yonghuming, wangzhi); //通知签单人
                #endregion
                #endregion
                //end
                #endregion


            }
            #endregion
            //end
            #endregion
        }
        else
        {

        }
        #endregion
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
            string callback = Request.QueryString["jsoncallback"];
            string type = Request.QueryString["type"];
            if (type == "getzuzhi")
            {

            }
            else if (type == "edit")
            {
                #region 修改
                string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
                string Model_id = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'").Rows[0]["id"].ToString();
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
                my_c.genxin(sql);
                #endregion
            }
            else if (type == "del")
            {
                #region 删除
                string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
                my_c.genxin("delete from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");
                #endregion
            }

            type = my_b.set_url_css(Request.QueryString["type"].ToString());
            set_user_sql(type);
        }
    }
}