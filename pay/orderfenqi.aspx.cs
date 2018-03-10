using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Com.Alipay;
public partial class orderfenqi : System.Web.UI.Page
{
    my_order my_o = new my_order();
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    string type = "";
    DataTable dt = new DataTable();
 
    //计算
    public float jisuan(string type,float jifen,float yue)
    {
        string tablename = "";
        try
        {
            tablename = "sl_"+Request.QueryString["tablename"].ToString();
        }
        catch { }
        //处理如果重复的记录
        my_c.genxin("delete from sl_jifen where shijian like '%" + my_b.c_string(Request["dingdanhao"].ToString()) + "%' and yonghuming='" + yonghuming + "'");

        my_c.genxin("delete from sl_caiwu where miaoshu like '%" + my_b.c_string(Request["dingdanhao"].ToString()) + "%' and yonghuming='" + yonghuming + "'");

        my_c.genxin("update " + tablename + " set jifen=0 ,yue=0 where dingdanhao='" + Request["dingdanhao"].ToString() + "' and yonghuming='" + yonghuming + "'");
        //end
        DataTable sl_order = my_c.GetTable("select top 1 * from sl_"+Request["tablename"].ToString() +" where dingdanhao='" + my_b.c_string(Request["dingdanhao"].ToString()) + "'");
        float zongjia = float.Parse(sl_order.Rows[0]["jine"].ToString());
      
        //处理积分部分
        if (jifen > 0)
        {

            try
            {
                jifen = float.Parse(my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + yonghuming + "' and zhuangtai='已处理'").Rows[0]["count_id"].ToString());
            }
            catch {
                jifen = 0;
            }

            DataTable xml_dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");
            float jifenbili = float.Parse(xml_dt.Rows[0]["u23"].ToString());
            float fenshu = jifen / jifenbili;
       
            try
            {
                fenshu = float.Parse(fenshu.ToString().Substring(0, fenshu.ToString().IndexOf(".")));
            }
            catch
            {
                fenshu = float.Parse(fenshu.ToString("f0"));
            }
            if (zongjia > fenshu)
            {
                zongjia = zongjia - fenshu;
            }
            else
            {
                fenshu = zongjia;
                zongjia = 0;
                
            }

            if (type == "pay")
            {
                my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + yonghuming + "','积分抵现',-" + fenshu * jifenbili + ",'已处理','订单号：" + Request["dingdanhao"].ToString() + "，抵扣" + fenshu + "元')");

                my_c.genxin("update " + tablename + " set jifen=" + fenshu * jifenbili + " where dingdanhao='" + Request["dingdanhao"].ToString() + "'");
                //发送微信消息
                //DataTable dt= my_c.GetTable("select * from sl_jifen where shijian like '%" + my_b.c_string(Request["dingdanhao"].ToString()) + "%' and yonghuming='" + yonghuming + "'");
                //string wherestrig = "where id=" + dt.Rows[0]["id"].ToString() + "";
                //my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=jifen&tablename=sl_jifen&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");
                //end
            }
        }
        //处理积分部分 end

        //处理余额部分
        if (yue > 0)
        {
           
            try
            {
                yue = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + yonghuming + "' and leixing <>'领取' and zhuangtai='已付款'").Rows[0]["count_id"].ToString());
            }
            catch {
                yue = 0;
            }

            float yue1 = 0;
            if (yue > zongjia)
            {
                yue1 = zongjia;
                zongjia = 0;

            }
            else
            {
                zongjia = zongjia - yue;
                yue1 = yue;
            }
            if (type == "pay")
            {
                my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + Request["dingdanhao"].ToString() + "','','" + yonghuming + "','已付款','消费','订单号：" + Request["dingdanhao"].ToString() + "，抵扣" + yue1 + "元',-" + yue1 + ")");

                my_c.genxin("update " + tablename + " set yue=" + yue1 + " where dingdanhao='" + Request["dingdanhao"].ToString() + "'");

                //发送微信消息
                DataTable dt = my_c.GetTable("select * from sl_caiwu where miaoshu like '%" + my_b.c_string(Request["dingdanhao"].ToString()) + "%' and yonghuming='" + yonghuming + "'");
                string wherestrig = "where id=" + dt.Rows[0]["id"].ToString() + "";

                my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=caiwu&tablename=sl_caiwu&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");
                //end
            }
        }
       
        //处理余额部分 end

        return zongjia;
    }
    //计算 end
    string yonghuming = "";
    string user_ip = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {//用户状态
         //用户状态
            try
            {
                yonghuming = my_b.k_cookie("user_name");
            }
            catch { }
            try
            {
                user_ip = my_b.k_cookie("user_ip");
            }
            catch { }
            if (yonghuming == "" && user_ip == "")
            {

                my_b.user_sta("user_name");
            }
            if (yonghuming == "")
            {
                yonghuming = user_ip;
            }
            //end
            //end
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }

            string dingdanhao = "";
            try
            {
                dingdanhao = Request.QueryString["dingdanhao"].ToString();
            }
            catch { }

            string tablename = "";
            try
            {
                tablename = Request.QueryString["tablename"].ToString();
            }
            catch { }

            if (type == "cancel")
            {
                #region 取消订单
                DataTable dt = my_c.GetTable("select * from sl_" + tablename + " where id in (" + Request.QueryString["id"].ToString() + ") and zhuangtai='未付款' and yonghuming='" + yonghuming + "'");
                if (dt.Rows.Count > 0)
                {
                    DataTable sl_order = my_c.GetTable("select * from sl_order where hebingdingdan='" + dt.Rows[0]["hebingdingdan"].ToString() + "'");
                    for (int i = 0; i < sl_order.Rows.Count; i++)
                    {
                        my_c.genxin("update sl_" + tablename + " set zhuangtai='已取消' where id in (" + sl_order.Rows[i]["id"].ToString() + ")  and yonghuming='" + yonghuming + "' ");
                    }
                    my_o.tuihuan("sl_" + tablename, dt.Rows[0]["hebingdingdan"].ToString());
                }
                Response.Redirect("/err.aspx?err=修改订单状态成功，正在跳转订单管理页面！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                #endregion
            }
            if (type == "close")
            {
                #region 订单关闭
                my_c.genxin("update sl_" + tablename + " set zhuangtai='订单关闭'  where id in (" + Request.QueryString["id"].ToString() + ")  and zhuangtai='已完成' and yonghuming='" + yonghuming + "' ");
                Response.Redirect("/err.aspx?err=修改订单状态成功，正在跳转订单管理页面！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                #endregion
            }
            if (type == "shouhuo")
            {
                #region 确认收货
                DataTable sl_order = my_c.GetTable("select * from sl_" + tablename + "  where id in (" + Request.QueryString["id"].ToString() + ") and zhuangtai='已发货' and yonghuming='" + yonghuming + "' ");
                if (sl_order.Rows.Count > 0)
                {
                    my_c.genxin("update sl_" + tablename + " set zhuangtai='订单完成' where id in (" + Request.QueryString["id"].ToString() + ")  and yonghuming='" + yonghuming + "' ");

                    //  my_o.set_dianpu_jifen(sl_order.Rows[0]["dingdanhao"].ToString(), tablename);
                }

                Response.Redirect("/err.aspx?err=修改订单状态成功，正在跳转订单管理页面！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                #endregion
            }
            //处理订单金额查询 start
            if (type == "zongjia")
            {
                float jifen = 1;
                try
                {
                    jifen = float.Parse(Request.QueryString["jifen"].ToString());
                }
                catch { }
                float yue = 1;
                try
                {
                    yue = float.Parse(Request.QueryString["yue"].ToString());
                }
                catch { }
                Response.Write(jisuan("", jifen, yue).ToString("f2"));
                Response.End();
            }
            //处理订单金额查询 end


                string dingdanname = "";
            string dingdanjiage = "";
     
            if (dingdanhao != "")
            {
                DataTable dt = my_c.GetTable("select * from sl_"+tablename+" where dingdanhao='" + dingdanhao + "' and yonghuming='" + yonghuming + "'");
       
                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("/err.aspx?err=无此订单！&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                dingdanhao = dt.Rows[0]["dingdanhao"].ToString();
                dingdanname = dt.Rows[0]["dingdanhao"].ToString();
                dingdanjiage = dt.Rows[0]["jine"].ToString();
            }
            if (type == "pay")
            {
                string zhifufangshi = Request["zhifufangshi"].ToString();
                string yue = "0";
                try
                {
                    yue = Request["yue"].ToString();
                }
                catch { }
                string jifen = "0";
                try
                {
                    jifen = Request["jifen"].ToString();
                }
                catch { }
                if (yue == "")
                {
                    yue = "0";
                }
                if (jifen == "")
                {
                    jifen = "0";
                }
          
                //计算
                float jine = 0;
                try
                {
                    jine = jisuan("pay", float.Parse(jifen), float.Parse(yue));
                }
                catch { }
              
                //end

                DataTable dt = my_c.GetTable("select * from sl_" + tablename + " where dingdanhao='" + dingdanhao + "' and yonghuming='" + yonghuming + "'");
                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("/err.aspx?err=无此订单！&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                #region 处理分期
                string qishu = my_b.c_string(Request["qishu"].ToString());

                DataTable sl_cart = my_c.GetTable("select * from sl_cart where dingdanhao='" + Request.QueryString["dingdanhao"].ToString() + "' and yonghuming='" + my_b.k_cookie("user_name") + "'");
                float fenqijine = 0;
                for (int i = 0; i < sl_cart.Rows.Count; i++)
                {
                    float shuliang = float.Parse(sl_cart.Rows[i]["shuliang"].ToString());
                    float kefenqijine = float.Parse(my_c.GetTable("select kefenqijine from sl_product where id in(" + sl_cart.Rows[i]["laiyuanbianhao"].ToString() + ")").Rows[0]["kefenqijine"].ToString());
                    fenqijine = fenqijine + (shuliang * kefenqijine);

                }
                fenqijine = jine - fenqijine;
                #region 判断额度是否足够
                DataTable sl_user = my_c.GetTable("select * from sl_user where yonghuming='" + yonghuming + "'");
                float shengyuedu = float.Parse(sl_user.Rows[0]["shengyuedu"].ToString()) ;
                if (shengyuedu < fenqijine)
                {
                    Response.Redirect("/err.aspx?err=额度不够，不能分期购物！&errurl=" + my_b.tihuan("/Search.aspx?m=user_order", "&", "fzw123") + "");
                }
                #endregion
               
                my_c.genxin("update sl_" + tablename + " set zhifufangshi='" + zhifufangshi + "',yue=" + yue + ",jifen=" + jifen + ",qishu='" + qishu + "',fenqijine='" + fenqijine + "' where jine=" + jine + " and yonghuming='" + yonghuming + "' ");
                jine = jine - fenqijine;
                dingdanjiage = jine.ToString();
                #endregion
                if (jine == 0)
                {
                    //订单支付功能后
                    my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' where dingdanhao='" + my_b.c_string(Request["dingdanhao"].ToString()) + "'");
                    my_c.genxin("update sl_caiwu set zhuangtai='已付款' where miaoshu like '%" + my_b.c_string(Request["dingdanhao"].ToString()) + "%'");
                    my_c.genxin("update sl_jifen set zhuangtai='已处理' where shijian like '%" + my_b.c_string(Request["dingdanhao"].ToString()) + "%'");
                    //end
   
                    Response.Redirect("/err.aspx?err=订单支付成功！&errurl=" + my_b.tihuan("/single.aspx?m=orderok", "&", "fzw123") + "");
                }
             
                if (zhifufangshi == "支付宝")
                {
                    ////////////////////////////////////////////请求参数////////////////////////////////////////////

                    //支付类型
                    string payment_type = "1";
                    //必填，不能修改
                    //服务器异步通知页面路径
                    string notify_url = my_b.get_Domain() + "inc/alipay/notify_url.aspx?tablename=" + tablename + "";
                    //需http://格式的完整路径，不能加?id=123这类自定义参数

                    //页面跳转同步通知页面路径
                    string return_url = my_b.get_Domain() + "inc/alipay/return_url.aspx?tablename=" + tablename + "";
                    //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

                    //卖家支付宝帐户
                    string seller_email = my_b.get_pay("支付宝", "zhanghao");
                    //必填

                    //商户订单号
                    string out_trade_no = dingdanhao;
                    //商户网站订单系统中唯一订单号，必填

                    //订单名称
                    string subject = dingdanname;
                    //必填

                    //付款金额
                    string total_fee = dingdanjiage;
                    //必填

                    //订单描述

                    string body = "";
                    //商品展示地址
                    string show_url = "";
                    //需以http://开头的完整路径，例如：http://www.xxx.com/myorder.html

                    //防钓鱼时间戳
                    string anti_phishing_key = "";
                    //若要使用请调用类文件submit中的query_timestamp函数

                    //客户端的IP地址
                    string exter_invoke_ip = "";
                    //非局域网的外网IP地址，如：221.0.0.1


                    ////////////////////////////////////////////////////////////////////////////////////////////////

                    //把请求参数打包成数组
                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("partner", alipayConfig.Partner);
                    sParaTemp.Add("_input_charset", alipayConfig.Input_charset.ToLower());
                    sParaTemp.Add("service", "create_direct_pay_by_user");
                    sParaTemp.Add("payment_type", payment_type);
                    sParaTemp.Add("notify_url", notify_url);
                    sParaTemp.Add("return_url", return_url);
                    sParaTemp.Add("seller_email", seller_email);
                    sParaTemp.Add("out_trade_no", out_trade_no);
                    sParaTemp.Add("subject", subject);
                    sParaTemp.Add("total_fee", total_fee);
                    sParaTemp.Add("body", body);
                    sParaTemp.Add("show_url", show_url);
                    sParaTemp.Add("anti_phishing_key", anti_phishing_key);
                    sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

                    //建立请求
                    string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
                    Response.Write(sHtmlText);
                }
                else if (zhifufangshi == "京东支付")
                {
                    Response.Redirect("/inc/chinabank/Send.aspx?v_oid=" + dingdanhao + "&tablename=" + tablename + "&v_amount=" + dingdanjiage + "");
                }
                else if (zhifufangshi == "快钱")
                {
                    float kuaiqian = float.Parse(dingdanjiage) * 100;
                    Response.Redirect("/inc/kuaiqian/send.aspx?v_oid=" + dingdanhao + "&tablename=" + tablename + "&v_amount=" + kuaiqian + "");
                }
                else if (zhifufangshi == "微信支付")
                {
                    float weixinzhifu = float.Parse(dingdanjiage) * 100;
                    if (my_b.set_fangwen() == 0)
                    {
                        Response.Redirect("/inc/weixinpay/demo/native_dynamic_qrcode.php?dingdanhao=" + dingdanhao + "&bodystr=" + dingdanhao + "&tablename=" + tablename + "&jine=" + weixinzhifu + "");
                    }
                    else
                    {
                        Response.Redirect("/inc/weixinpay/demo/js_api_call.php?dingdanhao=" + dingdanhao + "&tablename=" + tablename + "&bodystr=网上购物（" + dingdanhao + "）&jine=" + weixinzhifu + "");
                    }
                  
                    
                }
                else if (zhifufangshi == "货到付款")
                {
                  
                    Response.Redirect("/");
                }
            }
       
          
           
            string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/orderok.html", Encoding.UTF8);
            //替换

            //end
            Response.Write(my_h.Single_page(file_content));


        }
    }
}
