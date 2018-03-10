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
using System.Text.RegularExpressions;
public partial class set_cart : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_order my_o = new my_order();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string yonghuming = "";
            try
            {
                yonghuming = my_b.k_cookie("user_name");
            }
            catch
            {
                string niming = "";
                try
                {
                    niming = ConfigurationSettings.AppSettings["niming"].ToString();
                }
                catch { }
                if (niming == "yes")
                {
                    yonghuming = Request.UserHostAddress.ToString();
                    my_b.c_cookie(Request.UserHostAddress.ToString(), "user_ip");
                }
                else
                {
                    Response.Write("err");
                    Response.End();
                }


            }
            string type = "";
            try
            {
                type = my_b.c_string(Request.QueryString["type"].ToString());
            }
            catch { }

            //0代表加入到购物车不跳车，1代表加入购物车后跳转到提交订单页//

            string id = my_b.c_string(Request.QueryString["id"].ToString());
            string leixing = "";
            try
            {
                leixing = my_b.c_string(Request.QueryString["leixing"].ToString());
            }
            catch { }
            string dianpu = "0";
            try
            {
                dianpu = my_b.c_string(Request.QueryString["dianpu"].ToString());
            }
            catch { }

            string shuliang = "1";
            try
            {
                shuliang = my_b.c_string(Request.QueryString["shuliang"].ToString());
            }
            catch { }
            string beizhu = "";
            try
            {
                beizhu = my_b.c_string(Request.QueryString["beizhu"].ToString());
            }
            catch { }

          

            string xiaoji_ = "";

            if (type == "search")
            {
                //查询的

                if (leixing == "")
                {
                    xiaoji_ = my_c.GetTable("select sum(xiaoji) as xiaoji from sl_cart where  yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao ='')").Rows[0]["xiaoji"].ToString();
                }
                else
                {

                    xiaoji_ = my_c.GetTable("select sum(xiaoji) as xiaoji from sl_cart where  yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao ='') and leixing='" + leixing + "'").Rows[0]["xiaoji"].ToString();
                    //Response.Write("select sum(xiaoji) as xiaoji from sl_cart where  yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao ='') and leixing='" + leixing + "'");
                    //Response.End();
                }


            }
            else
            {
                //不是非查询的
                //处理价格及标题
                DataTable dt = my_c.GetTable("select * from sl_product where id=" + id + "");
                string biaoti = dt.Rows[0]["biaoti"].ToString();
                string danjia = dt.Rows[0]["jiage"].ToString();
                #region 处理规格
                string guige = "";
                try
                {
                    guige = my_b.c_string(Request.QueryString["guige"].ToString());
                }
                catch { }
                if (guige != "")
                {
                    DataTable sl_guigejiage = my_c.GetTable("select * from sl_guigejiage where laiyuanbianhao=" + dt.Rows[0]["guige"].ToString() + " and canshu='" + guige + "'");
                    if (sl_guigejiage.Rows.Count > 0)
                    {
                        danjia = sl_guigejiage.Rows[0]["jiage"].ToString();
                    }
                    biaoti = biaoti + "[" + guige + "]";
                }
                #endregion

                //处理价格及标题
                if (type == "1")
                {
                    #region 1代表加入购物车后跳转到提交订单页
                    float xiaoji = float.Parse(danjia) * float.Parse(shuliang);
                    string laiyuanbianhao = id;
                    my_c.genxin("delete from sl_cart where biaoti='" + biaoti + "' and laiyuanbianhao=" + id + " and yonghuming='" + yonghuming + "'  and (dingdanhao is null or dingdanhao='')");
                    string sql = "insert into sl_cart(biaoti,danjia,shuliang,laiyuanbianhao,yonghuming,xiaoji,beizhu,leixing,guige) values('" + biaoti + "'," + danjia + "," + shuliang + "," + laiyuanbianhao + ",'" + yonghuming + "'," + xiaoji.ToString() + ",'" + beizhu + "','" + leixing + "','" + guige + "')";


                    DataTable dt1 = my_c.GetTable("select * from sl_cart where biaoti='" + biaoti + "' and laiyuanbianhao=" + id + " and yonghuming='" + yonghuming + "'  and (dingdanhao is null or dingdanhao='')");

                    if (dt1.Rows.Count > 0)
                    {
                        if (type == "jian")
                        {
                            if (int.Parse(dt1.Rows[0]["shuliang"].ToString()) == 1)
                            {
                                my_c.genxin("delete from sl_cart where id=" + dt1.Rows[0]["id"].ToString() + "");
                            }
                            else
                            {
                                int shuliang1 = int.Parse(dt1.Rows[0]["shuliang"].ToString()) - 1;
                                xiaoji = float.Parse(shuliang1.ToString()) * float.Parse(dt1.Rows[0]["danjia"].ToString());
                                my_c.genxin("update sl_cart set shuliang=" + shuliang1 + ",xiaoji=" + xiaoji + " where biaoti='" + biaoti + "' and laiyuanbianhao=" + id + " and yonghuming='" + yonghuming + "'");
                            }
                        }
                        else
                        {

                            int shuliang1 = int.Parse(dt1.Rows[0]["shuliang"].ToString()) + int.Parse(shuliang);
                            xiaoji = float.Parse(shuliang1.ToString()) * float.Parse(dt1.Rows[0]["danjia"].ToString());

                            my_c.genxin("update sl_cart set shuliang=" + shuliang1 + ",xiaoji=" + xiaoji + " where biaoti='" + biaoti + "' and laiyuanbianhao=" + id + " and yonghuming='" + yonghuming + "'");

                        }

                    }
                    else
                    {
                        try
                        {
                            id = my_c.GetTable(sql + " select @@IDENTITY as id").Rows[0]["id"].ToString();
                        }
                        catch
                        {
                            Response.Write("err");
                            Response.End();
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 0代表加入到购物车不跳转
                    float xiaoji = float.Parse(danjia) * float.Parse(shuliang);
                    string laiyuanbianhao = id;
                    string sql = "insert into sl_cart(biaoti,danjia,shuliang,laiyuanbianhao,yonghuming,xiaoji,beizhu,leixing,dianpu,guige) values('" + biaoti + "'," + danjia + "," + shuliang + "," + laiyuanbianhao + ",'" + yonghuming + "'," + xiaoji.ToString() + ",'" + beizhu + "','" + leixing + "'," + dianpu + ",'"+ guige + "')";


                    DataTable dt1 = my_c.GetTable("select * from sl_cart where biaoti='" + biaoti + "' and laiyuanbianhao=" + id + " and yonghuming='" + yonghuming + "'  and (dingdanhao is null or dingdanhao='')");

                    //处理店铺
                    DataTable dt2 = my_c.GetTable("select * from sl_cart where  yonghuming='" + yonghuming + "'  and (dingdanhao is null or dingdanhao='')");

                    if (dt2.Rows.Count > 0)
                    {
                        if (dt2.Rows[0]["dianpu"].ToString() != dianpu)
                        {

                            my_c.genxin("delete from sl_cart where yonghuming='" + yonghuming + "'  and (dingdanhao is null or dingdanhao='')");
                            my_c.genxin(sql);
                        }//end
                        else
                        {

                            //如果店铺不等于 start
                            if (dt1.Rows.Count > 0)
                            {


                                if (type == "jian")
                                {

                                    if (int.Parse(dt1.Rows[0]["shuliang"].ToString()) == 1)
                                    {
                                        my_c.genxin("delete from sl_cart where id=" + dt1.Rows[0]["id"].ToString() + "");
                                    }
                                    else
                                    {

                                        int shuliang1 = int.Parse(dt1.Rows[0]["shuliang"].ToString()) - 1;
                                        xiaoji = float.Parse(shuliang1.ToString()) * float.Parse(dt1.Rows[0]["danjia"].ToString());
                                        my_c.genxin("update sl_cart set shuliang=" + shuliang1 + ",xiaoji=" + xiaoji + " where biaoti='" + biaoti + "' and laiyuanbianhao=" + id + " and yonghuming='" + yonghuming + "'");
                                    }
                                }
                                else
                                {

                                    int shuliang1 = int.Parse(dt1.Rows[0]["shuliang"].ToString()) + int.Parse(shuliang);
                                    xiaoji = float.Parse(shuliang1.ToString()) * float.Parse(dt1.Rows[0]["danjia"].ToString());

                                    my_c.genxin("update sl_cart set shuliang=" + shuliang1 + ",xiaoji=" + xiaoji + " where biaoti='" + biaoti + "' and laiyuanbianhao=" + id + " and yonghuming='" + yonghuming + "'");

                                }

                            }
                            else
                            {
                                if (type != "jian")
                                {

                                    my_c.genxin(sql);
                                }

                            }
                            //如果店铺不等于时 end
                        }
                    }
                    else
                    {
                        if (type != "jian")
                        {
                            my_c.genxin(sql);
                        }
                    }
                    #endregion

                }

            }

            #region 计算及输出
            xiaoji_ = "0";
            try
            {
                xiaoji_ = my_c.GetTable("select sum(xiaoji) as xiaoji from sl_cart where  yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao ='') and leixing='" + leixing + "'").Rows[0]["xiaoji"].ToString();
            }
            catch { }


            if (xiaoji_ == "")
            {
                Response.Write("0");
            }
            else
            {
                if (leixing == "商品")
                {
                    DataTable sl_cart = my_c.GetTable("select id from sl_cart where yonghuming='" + my_b.k_cookie("user_name") + "' and (dingdanhao is null or dingdanhao='') and leixing='商品' ");
                    Response.Write(sl_cart.Rows.Count.ToString());
                }
                else
                {
                    //显示金额
                    Response.Write(xiaoji_);
                }


            }


            if (type == "1" || type == "2")
            {
                Response.Write("{fzw:next}");
                Response.Write(type);
                Response.Write("{fzw:next}");
                Response.Write(id);
                //  Response.Write(my_c.GetTable("select top 1 * from sl_cart where  yonghuming='" + yonghuming + "'  and (dingdanhao is null or dingdanhao='') order by id desc").Rows[0]["id"].ToString());
            }
            #endregion


        }


    }
}
