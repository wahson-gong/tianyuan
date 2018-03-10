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
/// <summary>
///my_conn 的摘要说明
/// </summary>
public class my_order
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    #region 获取地址表
    public DataTable get_add(string dianpuyonghuming, string diqu)
    {
        DataTable sl_dp_yunfei = new DataTable();
        sl_dp_yunfei = my_c.GetTable("select top 1  * from sl_dp_yunfei where yonghuming='" + dianpuyonghuming + "' and diqu like '" + diqu + "' order by id desc");
        if (sl_dp_yunfei.Rows.Count == 0)
        {
            sl_dp_yunfei = my_c.GetTable("select top 1  * from sl_dp_yunfei where yonghuming='" + dianpuyonghuming + "' and diqu like '" + diqu.Substring(0, diqu.LastIndexOf("-")) + "' order by id desc");

        }
        if (sl_dp_yunfei.Rows.Count == 0)
        {
            sl_dp_yunfei = my_c.GetTable("select top 1  * from sl_dp_yunfei where yonghuming='" + dianpuyonghuming + "' and diqu like '" + diqu.Split('-')[0].ToString() + "' order by id desc");
        }
        if (sl_dp_yunfei.Rows.Count == 0)
        {
            sl_dp_yunfei = my_c.GetTable("select top 1 * from sl_dp_yunfei where yonghuming='" + dianpuyonghuming + "' and (diqu is null or diqu='') order by id desc");
        }

        return sl_dp_yunfei;
    }
    #endregion
    #region 处理购买权限的问题
    public void set_goumaiquanxian(string id,int shuliang)
    {
        DataTable dt = my_c.GetTable("select * from sl_product where id=" + id + "");
        string goumairenqun = dt.Rows[0]["goumairenqun"].ToString();
        string xiangoushuliang = dt.Rows[0]["xiangoushuliang"].ToString();
        string yonghuming = my_b.k_cookie("user_name");
        string leixing = my_c.GetTable("select * from sl_user where yonghuming='" + yonghuming + "'").Rows[0]["leixing"].ToString();
        if (goumairenqun.IndexOf("金卡") > -1 || goumairenqun.IndexOf("白金") > -1)
        {
        
            #region 会员级别不满足
            if (leixing == "会员" || leixing == "体验")
            {
                HttpContext.Current.Response.Write("quanxian{fzw:next}您的会员级别是" + leixing + "，不能购买此商品");
                HttpContext.Current.Response.End();
            }
            #endregion
           
            #region 有金卡或白金订单
            DataTable sl_cart = my_c.GetTable("select * from sl_cart where  yonghuming='" + yonghuming + "' and (dingdanhao is not null or dingdanhao <>'') and laiyuanbianhao in (select id from sl_product where goumairenqun like '%金卡%' or goumairenqun like '%白金%')");
     
            if (sl_cart.Rows.Count > 0)
            {
                for (int i = 0; i < sl_cart.Rows.Count; i++)
                {
                    DataTable sl_fenqiguanli = my_c.GetTable("select * from sl_fenqiguanli where shangpindingdan='" + sl_cart.Rows[i]["dingdanhao"].ToString() + "' and zhuangtai<>'已还'");
                    if (sl_fenqiguanli.Rows.Count > 0)
                    {
                        HttpContext.Current.Response.Write("quanxian{fzw:next}您有未还帐单，不可以购买此商品，您可以把帐单还清后购买。");
                        HttpContext.Current.Response.End();
                    }
                }
            }
            #endregion
            #region 判断数量的问题
            sl_cart = my_c.GetTable("select * from sl_cart where  yonghuming='" + yonghuming + "' and (dingdanhao is  null or dingdanhao ='') and laiyuanbianhao in ("+id+")");
           
            if (sl_cart.Rows.Count > 0)
            {
                if ((int.Parse(sl_cart.Rows[0]["shuliang"].ToString()) + 1) >= int.Parse(xiangoushuliang))
                {
                    HttpContext.Current.Response.Write("quanxian{fzw:next}此商品最多可购买"+ xiangoushuliang + "个");
                    HttpContext.Current.Response.End();
                }
            }
            
            #endregion
        }

    }
    #endregion
    #region 设置额度变化 
    public void set_edu(string yonghuming, float bianhua)
    {
        DataTable sl_user = my_c.GetTable("select * from sl_user where yonghuming='" + yonghuming + "'");
        float shengyuedu = float.Parse(sl_user.Rows[0]["shengyuedu"].ToString()) + bianhua;
        my_c.genxin("update sl_user set shengyuedu=" + my_b.get_jiage(shengyuedu) + " where id=" + sl_user.Rows[0]["id"].ToString());
    }
    #endregion
    #region 设置分期
    public void setfenqi(DataTable sl_order)
    {
        double qishu = double.Parse(sl_order.Rows[0]["qishu"].ToString().Replace("个月", ""));
        double fenqijine = double.Parse(sl_order.Rows[0]["fenqijine"].ToString());
        set_edu(sl_order.Rows[0]["yonghuming"].ToString(), float.Parse("-"+ fenqijine.ToString()));
        DateTime dy = DateTime.Now;
        for (Double i = 1; i <= qishu; i++)
        {
            string leixing = "";
            double meiqijine = fenqijine / qishu;
            my_c.genxin("insert into sl_fenqiguanli(yonghuming,dingdanhao,jiekuanshijian,qishu,jine,huankuanshijian,zhuangtai,leixing,shangpindingdan) values('" + sl_order.Rows[0]["yonghuming"].ToString() + "','" + my_b.get_bianhao() + "','" + dy.ToString() + "','第" + i + "期'," + my_b.get_jiage(float.Parse(meiqijine.ToString())) + ",'" + dy.AddMonths(int.Parse(i.ToString())) + "','待还','" + leixing + "','" + sl_order.Rows[0]["dingdanhao"].ToString() + "') ");
        }
     

    }
    #endregion
    #region 支付成功后的订单状态的处理
    public void orderok_chuli(string dingdanhao,string tablename,float jine)
    {
        #region 如果tablename等于空
        if (tablename == "")
        {
            if (my_c.GetTable("select * from sl_order where zhuangtai='未付款' and dingdanhao='" + dingdanhao + "'").Rows.Count > 0)
            {
                tablename = "order";
            }

            if (my_c.GetTable("select * from sl_caiwu where zhuangtai='未付款' and dingdanhao='" + dingdanhao + "'").Rows.Count > 0)
            {
                tablename = "caiwu";
            }
        }
        #endregion
        string yonghuming = "";
        if (tablename == "caiwu")
        {
            DataTable sl_caiwu = my_c.GetTable("select * from sl_" + tablename + " where zhuangtai='未付款' and dingdanhao='" + dingdanhao + "'");
            if (sl_caiwu.Rows.Count > 0)
            {
                if (jine < float.Parse(sl_caiwu.Rows[0]["jine"].ToString()))
                {
                    HttpContext.Current.Response.Write("err");
                    HttpContext.Current.Response.End();
                }
                caiwu_pay(dingdanhao, tablename);
            }
              
        }
        else if (tablename == "hedan")
        {
            #region 处理合单
            DataTable sl_order = my_c.GetTable("select * from sl_" + tablename + " where zhuangtai='未付款' and dingdanhao='" + dingdanhao + "'");

            if (sl_order.Rows.Count > 0)
            {
                if (jine < float.Parse(sl_order.Rows[0]["jine"].ToString()))
                {
                    HttpContext.Current.Response.Write("err");
                    HttpContext.Current.Response.End();
                }
                #region 存在未付款的信息
                my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + sl_order.Rows[0]["yonghuming"].ToString() + "','" + dingdanhao + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','会员订单')");

                if (sl_order.Rows.Count > 0)
                {
                    string zhanghu = "";

                    //加财务记录 start
                    my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine,zhanghu) values('" + sl_order.Rows[0]["dingdanhao"].ToString() + "','" + sl_order.Rows[0]["zhifufangshi"].ToString() + "','" + sl_order.Rows[0]["yonghuming"].ToString() + "','已付款','充值','支付订单:" + sl_order.Rows[0]["dingdanhao"].ToString() + "，先充值。'," + sl_order.Rows[0]["jine"].ToString() + ",'" + zhanghu + "')");

                    my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine,zhanghu) values('" + sl_order.Rows[0]["dingdanhao"].ToString() + "','" + sl_order.Rows[0]["zhifufangshi"].ToString() + "','" + sl_order.Rows[0]["yonghuming"].ToString() + "','已付款','消费','支付订单:" + sl_order.Rows[0]["dingdanhao"].ToString() + "，使用充值金额。',-" + sl_order.Rows[0]["jine"].ToString() + ",'" + zhanghu + "')");
                    //加财务记录 end

                    my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' where dingdanhao='" + dingdanhao + "'");

                    //订单
                    my_c.genxin("update sl_order set zhuangtai='已付款' where hebingdingdan='" + dingdanhao + "'");
                    my_order my_o = new my_order();
                    //end
                }
                my_c.genxin("update sl_caiwu set zhuangtai='已付款' where miaoshu like '%" + my_b.c_string(dingdanhao) + "%'");
                my_c.genxin("update sl_jifen set zhuangtai='已处理' where shijian like '%" + my_b.c_string(dingdanhao) + "%'");
                HttpContext.Current.Response.Redirect("/err.aspx?err=已经支付！&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                #endregion
            }

            #endregion
        }
        else
        {
            #region 其它订单类型
            DataTable sl_order = my_c.GetTable("select * from sl_" + tablename + " where zhuangtai='未付款' and dingdanhao='" + dingdanhao + "'");
            if (sl_order.Rows.Count > 0)
            {
                if (jine < float.Parse(sl_order.Rows[0]["jine"].ToString()))
                {
                    HttpContext.Current.Response.Write("err");
                    HttpContext.Current.Response.End();
                }
                yonghuming = sl_order.Rows[0]["yonghuming"].ToString();
                my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' where dingdanhao='" + dingdanhao + "'");
            }
            my_c.genxin("update sl_caiwu set zhuangtai='已付款' where miaoshu like '%" + my_b.c_string(dingdanhao) + "%'");
            my_c.genxin("update sl_jifen set zhuangtai='已处理' where shijian like '%" + my_b.c_string(dingdanhao) + "%'");
            #endregion
        }

        my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + yonghuming + "','" + tablename + "订单号：" + dingdanhao + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','会员订单')");
    }
    #endregion
    //处理退货或取消订单时退还积分、余额等
    public void tuihuan(string tablename, string dingdanhao)
    {
        //处理如果重复的记录
        my_c.genxin("delete from sl_jifen where shijian like '%" + my_b.c_string(dingdanhao) + "%' and yonghuming='" + my_b.k_cookie("user_name") + "'");

        my_c.genxin("delete from sl_caiwu where miaoshu like '%" + my_b.c_string(dingdanhao) + "%' and yonghuming='" + my_b.k_cookie("user_name") + "'");

        my_c.genxin("update sl_user_yhq set zhuangtai='未使用' where dingdanhao='" + dingdanhao + "' and yonghuming='" + my_b.k_cookie("user_name") + "'");

        my_c.genxin("update " + tablename + " set jifen=0 ,yue=0 where dingdanhao='" + dingdanhao + "' and yonghuming='" + my_b.k_cookie("user_name") + "'");
        //end
    }
    //end
    //这里是对库存的处理
    public void set_xiaoliang(string dingdanhao)
    {
        DataTable dt = my_c.GetTable("select * from sl_cart where dingdanhao='" + dingdanhao + "'");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            my_c.genxin("insert into sl_xiaoshou (laiyuanbianhao,yonghuming,shuliang,jine,dingdanhao) values(" + dt.Rows[i]["laiyuanbianhao"].ToString() + ",'" + dt.Rows[i]["yonghuming"].ToString() + "'," + dt.Rows[i]["shuliang"].ToString() + "," + dt.Rows[i]["danjia"].ToString() + ",'" + dingdanhao + "')");

            int xiaoliang = int.Parse(my_c.GetTable("select xiaoliang  from sl_product where id=" + dt.Rows[i]["laiyuanbianhao"].ToString() + "").Rows[0]["xiaoliang"].ToString()) + int.Parse(dt.Rows[i]["shuliang"].ToString());

            int kucun = int.Parse(my_c.GetTable("select kucun from sl_product where id=" + dt.Rows[i]["laiyuanbianhao"].ToString() + "").Rows[0]["kucun"].ToString()) - int.Parse(dt.Rows[i]["shuliang"].ToString());
            // HttpContext.Current.Response.Write(kucun);


            my_c.genxin("update sl_product set xiaoliang=" + xiaoliang + ",kucun=" + kucun + " where id=" + dt.Rows[i]["laiyuanbianhao"].ToString() + "");
        }
    }
    //end
    //在订单状态改变后，设置订单积分
    public void set_order_jifen(string dingdanhao, string tablename)
    {
        DataTable dt = new DataTable();
        //这里是对库存的处理
        set_xiaoliang(dingdanhao);
        //end

        dt = my_c.GetTable("select * from sl_" + tablename.Replace("sl_", "") + " where dingdanhao='" + dingdanhao + "'");
        string songfen = "";
        try
        {
            songfen = dt.Rows[0]["songfen"].ToString();
        }
        catch { }
        if (dt.Rows[0]["songfen"].ToString() != "是")
        {
            string yonghuming = dt.Rows[0]["yonghuming"].ToString();

            float jine = float.Parse(dt.Rows[0]["jine"].ToString());

            if (dt.Rows[0]["zhuangtai"].ToString() == "订单完成")
            {
                //处理返利等
                string fenxiao = "";
                try
                {
                    fenxiao = ConfigurationSettings.AppSettings["fenxiao"].ToString();
                }
                catch { }
                if (fenxiao == "yes")
                {
                    fanli(dingdanhao, tablename, dt.Rows[0]["yonghuming"].ToString());
                }

                //end
                //处理积分
                string jifen = dt.Rows[0]["jine"].ToString();
                my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + yonghuming + "','消费送积分'," + jifen + ",'已处理','完成:" + dingdanhao + "，订单号：" + dingdanhao + "')");
                //end
                //HttpContext.Current.Response.End();
                my_c.genxin("update sl_" + tablename.Replace("sl_", "") + " set songfen='是' where dingdanhao='" + dingdanhao + "' ");
                //
                //   my_c.genxin("update sl_user set leixing='分销会员' where yonghuming='" + yonghuming + "' and leixing='普通会员'");

                //
            }
        }




    }
    //在订单状态改变后，设置订单积分 end
    //返积分倍数
    public string fanjifenbeishu(string yonghuming, string shangjia, string type)
    {
        //获取返积分倍数
        string fanjifen = "1";
        DataTable sl_user = my_c.GetTable("select * from sl_user where yonghuming='" + yonghuming + "'");
        if (sl_user.Rows.Count > 0)
        {
            string jieshaoren = sl_user.Rows[0]["jieshaoren"].ToString();
            //获取到会员的级别
            string jibie = "";

            string huiyuandengji = sl_user.Rows[0]["huiyuandengji"].ToString();
            string bendianhuiyuandengji = sl_user.Rows[0]["bendianhuiyuandengji"].ToString();
            int huiyuandengji_ = 1;
            try
            {
                huiyuandengji_ = int.Parse(huiyuandengji.ToLower().Replace("v", ""));
            }
            catch { }
            int bendianhuiyuandengji_ = 1;
            try
            {
                bendianhuiyuandengji_ = int.Parse(bendianhuiyuandengji.ToLower().Replace("v", ""));
            }
            catch { }
            if (bendianhuiyuandengji_ < huiyuandengji_)
            {
                jibie = huiyuandengji;
            }
            else
            {
                jibie = bendianhuiyuandengji;
            }
            //end




            if (type == "jifen")
            {
                if (jieshaoren == shangjia)
                {
                    try
                    {
                        fanjifen = my_c.GetTable("select * from sl_user_zheke where yonghuming='" + jieshaoren + "' and jibieming='" + jibie + "'").Rows[0]["fanjifen"].ToString();
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        fanjifen = my_c.GetTable("select * from sl_user_zheke where yonghuming='" + shangjia + "' and jibieming='" + jibie + "'").Rows[0]["fanjifen"].ToString();
                    }
                    catch { }
                }
            }
            else
            {
                if (jieshaoren == shangjia)
                {
                    try
                    {
                        fanjifen = my_c.GetTable("select * from sl_user_zheke where yonghuming='" + jieshaoren + "' and jibieming='" + jibie + "'").Rows[0]["zhekou"].ToString();
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        fanjifen = my_c.GetTable("select * from sl_user_zheke where yonghuming='" + shangjia + "' and jibieming='" + jibie + "'").Rows[0]["zhekou"].ToString();
                    }
                    catch { }
                }
            }
        }
        //end
        return fanjifen;
    }
    //end
    //设置返利  start
    public void fanli(string dingdanhao, string tablename, string yonghuming)
    {
        DataTable order_dt = my_c.GetTable("select * from " + tablename + " where dingdanhao='" + dingdanhao + "'");
        DataTable sl_fanli = my_c.GetTable("select * from sl_fanli");
        DataTable sl_user = my_c.GetTable("select * from sl_user where yonghuming='" + yonghuming + "'");
        for (int i = 0; i < sl_fanli.Rows.Count; i++)
        {
            if (sl_fanli.Rows[i]["fanlijibie"].ToString() == "一级")
            {
                //一级
                string jieshaoren = "";
                if (sl_user.Rows[0]["leixing"].ToString() == "分销会员" || sl_user.Rows[0]["leixing"].ToString() == "代理会员")
                {

                    jieshaoren = yonghuming;
                }
                else
                {
                    jibie_ = 1;
                    jieshaoren = get_jieshaoren(yonghuming, 1);
                }

                if (jieshaoren != "")
                {
                    if (my_c.GetTable("select * from sl_user where yonghuming='" + jieshaoren + "' and leixing<>'普通会员'").Rows.Count > 0)
                    {
                        fanwupin(sl_fanli.Rows[i]["fanlijibie"].ToString(), sl_fanli.Rows[i]["leixing"].ToString(), sl_fanli.Rows[i]["zhi"].ToString(), dingdanhao, tablename, jieshaoren);
                    }

                }
                //一级
            }
            if (sl_fanli.Rows[i]["fanlijibie"].ToString() == "二级")
            {

                //二级
                string jieshaoren = "";
                if (sl_user.Rows[0]["leixing"].ToString() == "分销会员")
                {
                    jibie_ = 1;
                    jieshaoren = get_jieshaoren(yonghuming, 1);
                }
                else
                {
                    jibie_ = 1;
                    jieshaoren = get_jieshaoren(yonghuming, 2);
                }

                if (jieshaoren != "")
                {
                    if (my_c.GetTable("select * from sl_user where yonghuming='" + jieshaoren + "' and leixing<>'普通会员'").Rows.Count > 0)
                    {
                        fanwupin(sl_fanli.Rows[i]["fanlijibie"].ToString(), sl_fanli.Rows[i]["leixing"].ToString(), sl_fanli.Rows[i]["zhi"].ToString(), dingdanhao, tablename, jieshaoren);
                    }

                }
                //二级
            }
            if (sl_fanli.Rows[i]["fanlijibie"].ToString() == "三级")
            {
                //三级
                string jieshaoren = "";
                if (sl_user.Rows[0]["leixing"].ToString() == "分销会员")
                {
                    jibie_ = 1;
                    jieshaoren = get_jieshaoren(yonghuming, 2);
                }
                else
                {
                    jibie_ = 1;
                    jieshaoren = get_jieshaoren(yonghuming, 3);
                }

                if (jieshaoren != "")
                {
                    if (my_c.GetTable("select * from sl_user where yonghuming='" + jieshaoren + "' and leixing<>'普通会员'").Rows.Count > 0)
                    {
                        fanwupin(sl_fanli.Rows[i]["fanlijibie"].ToString(), sl_fanli.Rows[i]["leixing"].ToString(), sl_fanli.Rows[i]["zhi"].ToString(), dingdanhao, tablename, jieshaoren);
                    }

                }
                //三级
            }
        }


    }
    //设置返利  end
    //按级别返用户级别 start
    int jibie_ = 1;
    string jieshaoren = "";
    public string get_jieshaoren(string yonghuming, int jibie)
    {

        DataTable dt = new DataTable();
        dt = my_c.GetTable("select * from sl_user where yonghuming='" + yonghuming + "'");
        if (dt.Rows.Count > 0)
        {
            if (jibie == jibie_)
            {

                jieshaoren = dt.Rows[0]["jieshaoren"].ToString();
            }
            else
            {
                if (dt.Rows[0]["jieshaoren"].ToString() != "")
                {
                    jibie_ = jibie_ + 1;
                    get_jieshaoren(dt.Rows[0]["jieshaoren"].ToString(), jibie);
                }

            }


        }
        return jieshaoren;


    }
    //按级别返用户级别 end
    //返回的物品 start
    public void fanwupin(string fanlijibie, string leixing, string zhi, string dingdanhao, string tablename, string jieshaoren)
    {
        DataTable order_dt = my_c.GetTable("select * from " + tablename + " where dingdanhao='" + dingdanhao + "'");
        if (leixing == "订单金额比例")
        {

            //订单金额比例 start
            float xianjin = float.Parse(order_dt.Rows[0]["chanpinjine"].ToString()) * float.Parse(zhi);
            //double d1 = Math.Round(double.Parse(xianjin.ToString()), 0);
            //xianjin = float.Parse(d1.ToString());
            if (xianjin > 0)
            {
                my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dingdanhao + "','','" + jieshaoren + "','已付款','返利','订单:" + dingdanhao + "，" + fanlijibie + "返利'," + xianjin + ")");
            }
            //订单金额比例 end
        }
        else
        {
            //产品佣金 start
            DataTable sl_cart = my_c.GetTable("select * from sl_cart where dingdanhao='" + dingdanhao + "'");
            for (int i = 0; i < sl_cart.Rows.Count; i++)
            {
                DataTable sl_product = my_c.GetTable("select *  from sl_product where id=" + sl_cart.Rows[i]["laiyuanbianhao"].ToString() + "");
                if (sl_product.Rows.Count > 0)
                {
                    float xianjin = float.Parse(sl_product.Rows[0]["fanlijine"].ToString());
                    if (xianjin > 0)
                    {
                        my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dingdanhao + "','','" + jieshaoren + "','已付款','返利','订单:" + dingdanhao + "，" + fanlijibie + "返利'," + xianjin + ")");
                    }
                }
            }
            //产品佣金 end
        }
    }
    //返回的物品 end
    //设置1级返利  start
    public void fanli1(string dingdanhao, string tablename)
    {
        DataTable order_dt = my_c.GetTable("select * from " + tablename + " where dingdanhao='" + dingdanhao + "'");
        DataTable sl_cart = my_c.GetTable("select * from sl_cart where dingdanhao='" + dingdanhao + "'");
        string yonghuming = sl_cart.Rows[0]["yonghuming"].ToString();
        float yue = 0;
        float jifen = 0;

        for (int i = 0; i < sl_cart.Rows.Count; i++)
        {
            DataTable sl_product = my_c.GetTable("select *  from sl_product where id=" + sl_cart.Rows[i]["laiyuanbianhao"].ToString() + "");
            if (sl_product.Rows.Count > 0)
            {
                try
                {
                    yue = yue + float.Parse(sl_product.Rows[0]["yue"].ToString()) * float.Parse(sl_cart.Rows[i]["shuliang"].ToString());
                }
                catch { }
                jifen = jifen + float.Parse(sl_product.Rows[0]["jifen"].ToString()) * float.Parse(sl_cart.Rows[i]["shuliang"].ToString());
                //优惠券 start
                if (sl_product.Rows[0]["youhuiquan"].ToString() != "")
                {
                    DataTable sl_youhuiquan = my_c.GetTable("select * from sl_youhuiquan where id in (" + sl_product.Rows[0]["youhuiquan"].ToString() + ")");
                    for (int j = 0; j < sl_youhuiquan.Rows.Count; j++)
                    {
                        my_c.genxin("insert into sl_user_yhq(youhuiquanbianhao,yonghuming,youxiaoqi,zhuangtai) values('" + sl_youhuiquan.Rows[j]["youhuiquanbianhao"].ToString() + "','" + yonghuming + "','" + sl_youhuiquan.Rows[j]["youxiaoqi"].ToString() + "','领取')");
                    }
                }

                //优惠券 end
            }
        }
        //处理现金 start
        float xianjin = yue;
        if (xianjin > 0)
        {
            my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dingdanhao + "','" + order_dt.Rows[0]["zhifufangshi"].ToString() + "','" + yonghuming + "','已付款','领取','完成:" + dingdanhao + "'," + xianjin + ")");
        }
        //处理现金 end

        //积分 start

        if (jifen > 0)
        {
            my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + yonghuming + "','领取'," + jifen + ",'未处理','完成:" + dingdanhao + "，订单号：" + dingdanhao + "')");
        }
        //积分 end


        //HttpContext.Current.Response.End();
        //end
    }
    //设置1级返利  end
    //设置10天自动发货
    public void autoorder(string yonghuming)
    {

        DataTable sl_order = my_c.GetTable("select * from sl_order  where id in (select id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='已发货' and datediff(day,[fahuoshijian],getdate())>10 )");
        for (int i = 0; i < sl_order.Rows.Count; i++)
        {
            set_order_jifen(sl_order.Rows[i]["dingdanhao"].ToString(), "sl_order");
        }
        my_c.genxin("update sl_order set zhuangtai='送单完成' where id in (select id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='已发货' and datediff(day,[fahuoshijian],getdate())>10 )");
    }
    //处理到店支付返利 start
    public void daodian_fanli(string dingdanhao, string tablename)
    {
        DataTable dt = my_c.GetTable("select * from sl_" + tablename + " where dingdanhao='" + dingdanhao + "' and zhuangtai='已付款'");
        if (dt.Rows.Count > 0)
        {
            float jine = float.Parse(dt.Rows[0]["jine"].ToString()) + float.Parse(dt.Rows[0]["yue"].ToString());
            //HttpContext.Current.Response.Write(jine.ToString());
            //HttpContext.Current.Response.End();
            DataTable sl_chongzhiyouli = new DataTable();
            sl_chongzhiyouli = my_c.GetTable("select top 1 * from sl_chongzhiyouli where jine<=" + jine + " and leixing='到店支付' order by jine desc");

            // sl_chongzhiyouli = my_c.GetTable("select top 1 * from sl_chongzhiyouli where jine<=" + dt.Rows[0]["chanpinjine"].ToString() + " and leixing='到店支付' order by jine desc");

            if (sl_chongzhiyouli.Rows.Count > 0)
            {
                //处理现金 start
                float xianjin = float.Parse(sl_chongzhiyouli.Rows[0]["xianjin"].ToString());

                if (xianjin > 0)
                {

                    my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dt.Rows[0]["dingdanhao"].ToString() + "','" + dt.Rows[0]["zhifufangshi"].ToString() + "','" + dt.Rows[0]["yonghuming"].ToString() + "','已付款','领取','到店支付，完成:" + sl_chongzhiyouli.Rows[0]["biaoti"].ToString() + "'," + xianjin + ")");
                }
                //处理现金 end

                //积分 start
                float jifen = jine * float.Parse(sl_chongzhiyouli.Rows[0]["jifen"].ToString());
                if (jifen > 0)
                {
                    my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + dt.Rows[0]["yonghuming"].ToString() + "','领取'," + jifen + ",'未处理','到店支付，完成:" + sl_chongzhiyouli.Rows[0]["biaoti"].ToString() + "，订单号：" + dt.Rows[0]["dingdanhao"].ToString() + "')");
                }
                //积分 end

                //优惠券 start
                if (sl_chongzhiyouli.Rows[0]["youhuiquan"].ToString() != "")
                {
                    DataTable sl_youhuiquan = my_c.GetTable("select * from sl_youhuiquan where id in (" + sl_chongzhiyouli.Rows[0]["youhuiquan"].ToString().Replace(",,", ",") + ")");
                    for (int i = 0; i < sl_youhuiquan.Rows.Count; i++)
                    {
                        my_c.genxin("insert into sl_user_yhq(youhuiquanbianhao,yonghuming,youxiaoqi,zhuangtai) values('" + sl_youhuiquan.Rows[0]["youhuiquanbianhao"].ToString() + "','" + dt.Rows[0]["yonghuming"].ToString() + "','" + sl_youhuiquan.Rows[0]["youxiaoqi"].ToString() + "','领取')");
                    }
                }

                //优惠券 end
            }


        }
        // my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' where dingdanhao='" + dingdanhao + "'");
        //end
    }
    //处理到店支付返利 end
    //到店支付处理积分等
    public void daodian_pay(string dingdanhao, string tablename)
    {
        DataTable dt = my_c.GetTable("select * from sl_" + tablename + " where dingdanhao='" + dingdanhao + "'");
        if (dt.Rows.Count > 0)
        {
            float zongjia = float.Parse(dt.Rows[0]["chanpinjine"].ToString());
            //处理优惠券部分 start
            if (dt.Rows[0]["youhuiquan"].ToString() != "")
            {
                DataTable sl_youhuiquan = my_c.GetTable("select top 1 * from sl_youhuiquan where laiyuanbianhao in (" + my_b.c_string(dt.Rows[0]["laiyuanbianhao"].ToString()) + ") and leixing='商家'");
                if (sl_youhuiquan.Rows.Count > 0)
                {
                    DataTable sl_user_yhq = my_c.GetTable("select top 1 * from sl_user_yhq where yonghuming='" + dt.Rows[0]["yonghuming"].ToString() + "' and  zhuangtai='未使用' and dingdanhao='" + dingdanhao + "' and youhuiquanbianhao='" + sl_youhuiquan.Rows[0]["youhuiquanbianhao"].ToString() + "'");
                    if (sl_user_yhq.Rows.Count > 0)
                    {
                        //计算优惠券
                        float mianzhi = float.Parse(sl_youhuiquan.Rows[0]["mianzhi"].ToString());
                        float suoxuxiaofeijine = float.Parse(sl_youhuiquan.Rows[0]["suoxuxiaofeijine"].ToString());
                        if (zongjia >= suoxuxiaofeijine)
                        {
                            zongjia = zongjia - mianzhi;

                        }
                        my_c.genxin("update sl_user_yhq set zhuangtai='已使用',dingdanhao='" + dingdanhao + "' where id=" + sl_user_yhq.Rows[0]["id"].ToString() + "");
                        //end
                    }
                }
            }

            //处理优惠券部分 end
            //处理积分部分
            float jifen = float.Parse(dt.Rows[0]["jifen"].ToString());
            if (jifen > 0)
            {

                try
                {
                    jifen = float.Parse(my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + dt.Rows[0]["yonghuming"].ToString() + "' and zhuangtai='已处理'").Rows[0]["count_id"].ToString());
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
                if (fenshu > 0)
                {
                    my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + dt.Rows[0]["yonghuming"].ToString() + "','积分抵现',-" + fenshu * jifenbili + ",'已处理','订单号：" + dingdanhao + "，抵扣" + fenshu + "元')");
                }

            }
            //处理积分部分 end

            //处理余额部分
            float yue = float.Parse(dt.Rows[0]["yue"].ToString());

            if (yue > 0)
            {

                try
                {
                    yue = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + dt.Rows[0]["yonghuming"].ToString() + "' and leixing <>'领取' and zhuangtai='已付款'").Rows[0]["count_id"].ToString());
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

                //pay
                if (yue1 > 0)
                {
                    my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dingdanhao + "','','" + dt.Rows[0]["yonghuming"].ToString() + "','已付款','消费','订单号：" + dingdanhao + "，抵扣" + yue1 + "元',-" + yue1 + ")");

                    //发送微信消息
                    DataTable dt1 = my_c.GetTable("select * from sl_caiwu where miaoshu like '%" + my_b.c_string(dingdanhao) + "%' and yonghuming='" + dt.Rows[0]["yonghuming"].ToString() + "'");
                    string wherestrig = "where id=" + dt1.Rows[0]["id"].ToString() + "";

                    my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=caiwu&tablename=sl_caiwu&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");
                    //end
                }

                //pay
            }

            //处理余额部分 end
        }
        my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' , songfen='是' where dingdanhao='" + dingdanhao + "'");
    }
    //到店支付处理积分等


    //新用户注册送 start
    public void newuser()
    {
        //处理现金 start
        float xianjin = float.Parse(my_c.GetTable("select u2 from sl_banner where id=60").Rows[0]["u2"].ToString());
        if (xianjin > 0)
        {
            my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('','','" + my_b.k_cookie("user_name") + "','已付款','领取','新用户注册送现金'," + xianjin + ")");
        }
        //处理现金 end

        //积分 start
        float jifen = float.Parse(my_c.GetTable("select u2 from sl_banner where id=61").Rows[0]["u2"].ToString());
        if (jifen > 0)
        {
            my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + my_b.k_cookie("user_name") + "','领取'," + jifen + ",'未处理','新用户注册送积分')");
        }
        //积分 end
    }
    //新用户注册送 end
    //处理在线充值 start
    public void caiwu_pay(string dingdanhao, string tablename)
    {
        DataTable dt = my_c.GetTable("select * from sl_" + tablename + " where zhuangtai='未付款' and dingdanhao='" + dingdanhao + "'");
        if (dt.Rows.Count > 0)
        {
            try
            {
                #region 充值送
                DataTable sl_chongzhiyouli = my_c.GetTable("select top 1 * from sl_chongzhiyouli where jine<=" + dt.Rows[0]["jine"].ToString() + " and leixing='在线充值' order by jine desc");
                if (sl_chongzhiyouli.Rows.Count > 0)
                {
                    //处理现金 start
                    float xianjin = float.Parse(sl_chongzhiyouli.Rows[0]["xianjin"].ToString());
                    if (xianjin > 0)
                    {
                        my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dt.Rows[0]["dingdanhao"].ToString() + "','" + dt.Rows[0]["zhifufangshi"].ToString() + "','" + my_b.k_cookie("user_name") + "','已付款','领取','在线充值，完成:" + sl_chongzhiyouli.Rows[0]["biaoti"].ToString() + "'," + xianjin + ")");
                    }
                    //处理现金 end

                    //积分 start
                    float jifen = float.Parse(sl_chongzhiyouli.Rows[0]["jifen"].ToString());
                    if (jifen > 0)
                    {
                        my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + dt.Rows[0]["yonghuming"].ToString() + "','领取'," + jifen + ",'未处理','在线充值，完成:" + sl_chongzhiyouli.Rows[0]["biaoti"].ToString() + "，订单号：" + dt.Rows[0]["dingdanhao"].ToString() + "')");
                    }
                    //积分 end

                    //优惠券 start
                    if (sl_chongzhiyouli.Rows[0]["youhuiquan"].ToString() != "")
                    {
                        DataTable sl_youhuiquan = my_c.GetTable("select * from sl_youhuiquan where id in (" + sl_chongzhiyouli.Rows[0]["youhuiquan"].ToString() + ")");
                        for (int i = 0; i < sl_youhuiquan.Rows.Count; i++)
                        {
                            my_c.genxin("insert into sl_user_yhq(youhuiquanbianhao,yonghuming,youxiaoqi,zhuangtai) values('" + sl_youhuiquan.Rows[0]["youhuiquanbianhao"].ToString() + "','" + dt.Rows[0]["yonghuming"].ToString() + "','" + sl_youhuiquan.Rows[0]["youxiaoqi"].ToString() + "','领取')");
                        }
                    }

                    //优惠券 end
                }
                #endregion
            }
            catch { }

            my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' where dingdanhao='" + dingdanhao + "'");
        }
    }
    //处理在线充值 end
}
