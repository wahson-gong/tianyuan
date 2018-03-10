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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class Single : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    string type = "";
    DataTable dt = new DataTable();
    string yonghuming = "";
    string user_ip = "";
    my_order my_o = new my_order();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
            try
            {
                type = my_b.c_string(Request.QueryString["type"].ToString());
            }
            catch { }
            if (type == "del")
            {
                my_c.genxin("delete from sl_cart where id in (" + my_b.c_string(Request.QueryString["id"].ToString()) + ") and yonghuming='" + yonghuming + "'");
                Response.Redirect("/Single.aspx?m=cart");
            }
            else if (type == "all")
            {
                my_c.genxin("delete from sl_cart where yonghuming='" + yonghuming + "'");
                Response.Redirect("/Single.aspx?m=cart");
            }
            else if (type == "zongjia")
            {
                if (my_b.c_string(Request.QueryString["id"].ToString()) == "")
                {
                    Response.Write("0");
                    Response.End();
                }
                float zongjia = 0;
                DataTable sl_cart = my_c.GetTable("select * from sl_cart where yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao='') and leixing='商品' and id in (" + my_b.c_string(Request.QueryString["id"].ToString()) + ")");

                for (int i = 0; i < sl_cart.Rows.Count; i++)
                {
                    float xiaoji = float.Parse(sl_cart.Rows[i]["xiaoji"].ToString());
                    #region 获取在优惠券管理表中数据
                    DataTable sl_youhuiquan = my_c.GetTable("select * from sl_youhuiquan where laiyuanbianhao=" + sl_cart.Rows[i]["laiyuanbianhao"].ToString() + " and leixing='商品'");
                    int youhuiquan_Rows = 0;
                    try
                    {
                        youhuiquan_Rows = sl_youhuiquan.Rows.Count;
                    }
                    catch { }
                    if (youhuiquan_Rows > 0)
                    {
                        DataTable sl_user_yhq = my_c.GetTable("select * from sl_user_yhq where yonghuming='" + yonghuming + "' and zhuangtai='未使用' and youhuiquanbianhao='" + sl_youhuiquan.Rows[0]["youhuiquanbianhao"].ToString() + "'");//获取在会员优惠券管理表中数据
                        if (sl_user_yhq.Rows.Count > 0)
                        {
                            //计算优惠券
                            float mianzhi = float.Parse(sl_youhuiquan.Rows[0]["mianzhi"].ToString());
                            float suoxuxiaofeijine = float.Parse(sl_youhuiquan.Rows[0]["suoxuxiaofeijine"].ToString());
                            if (xiaoji >= suoxuxiaofeijine)
                            {
                                xiaoji = xiaoji - mianzhi;

                            }
                            //end
                        }
                    }
                    #endregion
                    //这里是循环的结束，并且做金额求和计算
                    zongjia = zongjia + xiaoji;
                }
                // Response.End();       
                #region     计算快递费 start  
                DataTable sl_order = my_c.GetTable("select distinct dianpu from sl_cart where yonghuming='" + my_b.k_cookie("user_name") + "' and id in (" + my_b.c_string(Request.QueryString["id"].ToString()) + ") and (dingdanhao is null or dingdanhao='') and leixing='商品'");
                string diqu = "";
                try
                {
                    diqu = Request.QueryString["diqu"].ToString();
                }
                catch { }
                int order_row = 0;
                try
                {
                    order_row = sl_order.Rows.Count;
                }
                catch { }
                if (order_row > 0)
                {
                    #region 使用商家运费 start
                    float yunfei_ = 0;
                    for (int i = 0; i < sl_order.Rows.Count; i++)
                    {
                        if (sl_order.Rows[i]["dianpu"].ToString() == "")
                        {
                            #region 店铺名称为空
                            float yunfei = float.Parse(ConfigurationSettings.AppSettings["yunfei"].ToString());
                            float mianyunfei = float.Parse(ConfigurationSettings.AppSettings["mianyunfei"].ToString());

                            float dianpu_cart_sum = float.Parse(my_c.GetTable("select sum(xiaoji) as count_id from sl_cart where yonghuming='" + my_b.k_cookie("user_name") + "' and (dingdanhao is null or dingdanhao='') and id in (" + my_b.c_string(Request.QueryString["id"].ToString()) + ") and leixing='商品'").Rows[0]["count_id"].ToString());

                            if (dianpu_cart_sum < mianyunfei)
                            {
                                yunfei_ = yunfei_ + yunfei;
                            }
                            #endregion 店铺名称为空
                        }
                        else
                        {
                            #region 店铺名称不为空
                            DataTable sl_dianpuyunfei = my_o.get_add(sl_order.Rows[i]["dianpuyonghuming"].ToString(), diqu);
                            float yunfei = float.Parse(ConfigurationSettings.AppSettings["yunfei"].ToString());
                            float mianyunfei = float.Parse(ConfigurationSettings.AppSettings["mianyunfei"].ToString());
                            if ( sl_dianpuyunfei.Rows.Count > 0)
                            {
                                yunfei = float.Parse( sl_dianpuyunfei.Rows[0]["yunfei"].ToString());
                                mianyunfei = float.Parse( sl_dianpuyunfei.Rows[0]["mianyunfei"].ToString());
                            }



                            float dianpu_cart_sum = float.Parse(my_c.GetTable("select sum(xiaoji) as count_id from sl_cart where yonghuming='" + my_b.k_cookie("user_name") + "' and id in (" + my_b.c_string(Request.QueryString["id"].ToString()) + ") and (dingdanhao is null or dingdanhao='') and leixing='商品' and dianpu=" + sl_order.Rows[i]["dianpu"].ToString() + "").Rows[0]["count_id"].ToString());

                            if (dianpu_cart_sum < mianyunfei)
                            {
                                yunfei_ = yunfei_ + yunfei;
                            }
                            #endregion 店铺名称不为空
                        }


                    }
                    //输出
                    if (yunfei_ == 0)
                    {
                        Response.Write(my_b.get_jiage(zongjia) + "|0");
                    }
                    else
                    {
                        zongjia = zongjia + yunfei_;
                        Response.Write(my_b.get_jiage(zongjia) + "|" + yunfei_);
                    }
                    #endregion 使用商家运费 end
                }
                else
                {
                    #region 使用系统运费 start
                    //使用系统运费 start
                    float yunfei = float.Parse(ConfigurationSettings.AppSettings["yunfei"].ToString());
                    if (zongjia < float.Parse(ConfigurationSettings.AppSettings["mianyunfei"].ToString()))
                    {
                        zongjia = zongjia + yunfei;
                        //计算快递费 end
                        Response.Write(my_b.get_jiage(zongjia) + "|" + yunfei);
                        //获取总价结束
                    }
                    else
                    {

                        Response.Write(my_b.get_jiage(zongjia) + "|0");

                    }
                    #endregion 使用系统运费 end
                }

                #endregion 计算快递费 end
                Response.End();

            }
            else if (type != "")
            {
                dt = my_c.GetTable("select * from sl_cart where  id=" + Request.QueryString["id"].ToString() + "");
               
                int shuliang1 = 0;
                if (type == "jia")
                {
                    shuliang1 = int.Parse(dt.Rows[0]["shuliang"].ToString()) + 1;

                }
                if (type == "jian")
                {
                    shuliang1 = int.Parse(dt.Rows[0]["shuliang"].ToString()) - 1;
                    if (shuliang1 < 1)
                    {
                        my_c.genxin("delete from sl_cart where id in (" + my_b.c_string(Request.QueryString["id"].ToString()) + ") and yonghuming='" + yonghuming + "'");
                        Response.Redirect("/Single.aspx?m=cart");
                    }

                }
                if (type == "shuliang")
                {
                    shuliang1 = int.Parse(Request.QueryString["shuliang"].ToString());
                    if (shuliang1 < 1)
                    {
                        shuliang1 = 1;
                    }
                }
                float xiaoji = float.Parse(shuliang1.ToString()) * float.Parse(dt.Rows[0]["danjia"].ToString());
                my_c.genxin("update sl_cart set shuliang=" + shuliang1 + ",xiaoji=" + xiaoji + " where id=" + my_b.c_string(Request.QueryString["id"].ToString()) + " and yonghuming='" + yonghuming + "'");
                Response.Redirect(Request.UrlReferrer.ToString());
            }

            string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/cart.html"), Encoding.UTF8);
            //替换
            float sum_jiege = 0;
            try
            {
                sum_jiege = float.Parse(my_c.GetTable("select sum(xiaoji) as s_count from sl_cart where yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao='')").Rows[0]["s_count"].ToString());
            }
            catch { }

            float meigedabao = float.Parse(ConfigurationSettings.AppSettings["yunfei"].ToString());
            float zongjia_ = 0;

            if (sum_jiege < float.Parse(ConfigurationSettings.AppSettings["mianyunfei"].ToString()))
            {

                zongjia_ = sum_jiege + meigedabao;
            }
            else
            {
                meigedabao = 0;
                zongjia_ = sum_jiege;
            }
            file_content = file_content.Replace("{yunfei}", my_b.get_jiage(meigedabao));
            file_content = file_content.Replace("{sum_jiege}", my_b.get_jiage(zongjia_));
            //end
            Response.Write(my_h.Single_page(file_content));


        }
    }
}
