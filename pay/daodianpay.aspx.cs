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

public partial class pay_oneorder : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    my_order my_o = new my_order();
    string type = "";
    DataTable dt = new DataTable();
    //计算
    float zong_yue = 0;
    float zong_jifen = 0;
    public float jisuan(string type, float jifen, float yue,float youhuiquan,float jine,string dingdanhao)
    {
        //处理如果重复的记录
        if (dingdanhao != "")
        {
            my_c.genxin("delete from sl_jifen where shijian like '%" + my_b.c_string(dingdanhao) + "%' and yonghuming='" + my_b.k_cookie("user_name") + "'");

            my_c.genxin("delete from sl_caiwu where miaoshu like '%" + my_b.c_string(dingdanhao) + "%' and yonghuming='" + my_b.k_cookie("user_name") + "'");

            my_c.genxin("update sl_user_yhq set zhuangtai='未使用' where dingdanhao= '" + my_b.c_string(dingdanhao) + "' and yonghuming='" + my_b.k_cookie("user_name") + "'");

            my_c.genxin("update sl_daodian set jifen=0 ,yue=0,youhuiquan='' where dingdanhao='" + dingdanhao + "' and yonghuming='" + my_b.k_cookie("user_name") + "'");
        }
        //end
   
        float zongjia = jine;
        //处理优惠券部分 start
        if (youhuiquan > 0)
        {
            DataTable sl_youhuiquan = my_c.GetTable("select top 1 * from sl_youhuiquan where laiyuanbianhao in (" + my_b.c_string(Request.QueryString["id"].ToString()) + ") and leixing='商家'");
            if (sl_youhuiquan.Rows.Count > 0)
            {
                DataTable sl_user_yhq = my_c.GetTable("select top 1 * from sl_user_yhq where yonghuming='" + my_b.k_cookie("user_name") + "' and  zhuangtai='未使用' and dingdanhao='"+dingdanhao+"' and youhuiquanbianhao='" + sl_youhuiquan.Rows[0]["youhuiquanbianhao"].ToString() + "'");
                if (sl_user_yhq.Rows.Count > 0)
                {
                    //计算优惠券
                    float mianzhi = float.Parse(sl_youhuiquan.Rows[0]["mianzhi"].ToString());
                    float suoxuxiaofeijine = float.Parse(sl_youhuiquan.Rows[0]["suoxuxiaofeijine"].ToString());
                    if (zongjia >= suoxuxiaofeijine)
                    {
                        zongjia = zongjia - mianzhi;

                    }
                    if (type == "pay")
                    {
                       // my_c.genxin("update sl_user_yhq set zhuangtai='已使用',dingdanhao='"+dingdanhao+"' where id="+ sl_user_yhq.Rows[0]["id"].ToString() + "");
                    }
                    //end
                }
            }
        }

        //处理优惠券部分 end
        //处理积分部分
        if (jifen > 0)
        {

            try
            {
                jifen = float.Parse(my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + my_b.k_cookie("user_name") + "' and zhuangtai='已处理'").Rows[0]["count_id"].ToString());
            }
            catch
            {
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
                //  my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + my_b.k_cookie("user_name") + "','积分抵现',-" + fenshu * jifenbili + ",'已处理','订单号：" + dingdanhao + "，抵扣" + fenshu + "元')");
                zong_jifen = fenshu * jifenbili;
               // my_c.genxin("update sl_daodian set jifen=" + fenshu * jifenbili + " where dingdanhao='" + dingdanhao + "'");
            }
        }
        //处理积分部分 end
        
        //处理余额部分
        if (yue > 0)
        {

            try
            {
                yue = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + my_b.k_cookie("user_name") + "' and leixing <>'领取' and zhuangtai='已付款'").Rows[0]["count_id"].ToString());
            }
            catch
            {
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
                // my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dingdanhao + "','','" + my_b.k_cookie("user_name") + "','已付款','消费','订单号：" + dingdanhao + "，抵扣" + yue1 + "元',-" + yue1 + ")");

                //发送微信消息
                //DataTable dt = my_c.GetTable("select * from sl_caiwu where miaoshu like '%" + my_b.c_string(dingdanhao) + "%' and yonghuming='" + my_b.k_cookie("user_name") + "'");
                //string wherestrig = "where id=" + dt.Rows[0]["id"].ToString() + "";

                //my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=caiwu&tablename=sl_caiwu&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");
                //end
                zong_yue = yue1;
              //  my_c.genxin("update sl_daodian set yue=" + yue1 + " where dingdanhao='" + dingdanhao + "'");
            }
        }

        //处理余额部分 end

        return float.Parse(my_b.get_jiage(zongjia).ToString());
    }
    //计算 end
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //用户状态
            my_b.user_sta("user_name");
            //end
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }

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
                

                float youhuiquan = 1;
                try
                {
                    youhuiquan = float.Parse(Request.QueryString["youhuiquan"].ToString());
                }
                catch { }
                float jine = 0;
                try
                {
                    jine = float.Parse(Request.QueryString["jine"].ToString());
                }
                catch { }
                Response.Write(jisuan("", jifen, yue, youhuiquan, jine,""));
                Response.End();
            }

            if (type == "pay")
            {
                string dingdanhao = my_b.get_bianhao();
                float jine = float.Parse(Request["jine"].ToString());
                string zhifufangshi = Request["zhifufangshi"];
                string laiyuanbianhao = Request["laiyuanbianhao"];
                string dianpumingcheng = Request["dianpumingcheng"];
          
                string yonghuming = my_b.k_cookie("user_name");
                string zhuangtai = "未付款";
                string miaoshu = "用户："+ yonghuming + "IP：" + Request.UserHostAddress.ToString() + "，到店付款"+ jine + "元";
                float jifen = 0;
                try
                {
                    jifen = float.Parse(Request["jifen"].ToString());
                }
                catch {
                    jifen = 0;
                }
                float jifen_ = 0;
                try
                {
                    jifen_ = float.Parse(my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + my_b.k_cookie("user_name") + "' and zhuangtai='已处理'").Rows[0]["count_id"].ToString());
                }
                catch { }
                if (jifen > jifen_)
                {
                    Response.Redirect("/err.aspx?err=操作不对！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }

         
            


                float yue = 0;
                try
                {
                    yue = float.Parse(Request["yue"].ToString());
                }
                catch {
                    yue = 0;
                }

                float yue_ = 0;
                try
                {
                    yue_ = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + my_b.k_cookie("user_name") + "' and leixing <>'领取' and zhuangtai='已付款'").Rows[0]["count_id"].ToString());
                }
                catch { }
                if (yue > yue_)
                {
                    Response.Redirect("/err.aspx?err=操作不对！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
              
            

                string youhuiquan1 = "";
                try
                {
                    youhuiquan1 = Request["youhuiquan"];
                }
                catch { }

                string youhuiquan = "";
                try
                {
                    youhuiquan = my_b.c_string(Request["youhuiquan"].ToString());
                }
                catch { }
     
                if (youhuiquan == "")
                {
                    youhuiquan = "0";
                }
                else
                {
                    youhuiquan = "1";
                }
                string chanpinjine = Request["jine"];
                //Response.Write(jifen.ToString() + "|" + yue.ToString() + "|" + youhuiquan + "|" + jine.ToString());
                //Response.End();

                jine = jisuan(type, jifen, yue, float.Parse(youhuiquan), jine,dingdanhao);
                //Response.Write(jine);
                //Response.End();
                if (jine == 0)
                {
                    zhuangtai = "已付款";
                }

                string tablename = "daodian";


                if (zhifufangshi == "现金支付")
                {
                    zhuangtai = "已付款";
                }

                my_c.genxin("insert into sl_daodian(dingdanhao,zhifufangshi,yonghuming,zhuangtai,miaoshu,jine,jifen,yue,youhuiquan,chanpinjine,laiyuanbianhao,dianpumingcheng) values('" + dingdanhao + "','" + zhifufangshi + "','" + yonghuming + "','" + zhuangtai + "','" + miaoshu + "'," + jine + "," + zong_jifen + "," + zong_yue + ",'" + youhuiquan1 + "'," + chanpinjine + "," + laiyuanbianhao + ",'" + dianpumingcheng + "')");

                if (zhifufangshi == "现金支付")
                {
                    zhuangtai = "已付款";
    
                    if (jine > 0)
                    {
                        my_o.daodian_pay(dingdanhao, tablename);
                        my_o.daodian_fanli(dingdanhao, tablename);
                    }
                }


                if (jine == 0)
                {
                    my_o.daodian_pay(dingdanhao, tablename);
                    my_o.daodian_fanli(dingdanhao, tablename);

                    Response.Redirect("/err.aspx?err=支付成功！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
                //处理付款 start
                

           
          
                string dingdanname = miaoshu;
                string dingdanjiage = jine.ToString();
                if (zhifufangshi == "微信支付")
                {
                    float weixinzhifu = float.Parse(dingdanjiage) * 100;
                    Response.Redirect("/inc/weixinpay/demo/js_api_call.php?dingdanhao=" + dingdanhao + "&tablename=" + tablename + "&bodystr=网上购物（" + dingdanhao + "）&jine=" + weixinzhifu + "");
                }
                else
                {
                    Response.Redirect("/err.aspx?err=请把现金交给收银员，并叫收银员把订单状态改为已付款！&errurl=" + my_b.tihuan("/single.aspx?m=user_index", "&", "fzw123") + "");
                }
                //处理付款 end

            }





        }
    }
}