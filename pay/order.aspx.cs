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
    biaodan bd = new biaodan();
    string type = "";
    DataTable dt = new DataTable();
    float zongjia_ = 0;
    string yonghuming = "";
    string user_ip = "";
    
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

    public string get_kj(string type, string id)
    {
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框" || type == "编辑器")
        {
            return "'" + my_b.c_string(Request[id].ToString()) + "'";
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
    public string get_kj_neirong(string type, string neirong)
    {
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框" || type == "编辑器")
        {
            return "'" + my_b.c_string(neirong) + "'";
        }
        else if (type == "数字")
        {
            return "" + my_b.c_string(neirong) + "";
        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(neirong) + "'";
        }
        else
        {
            return "'" + my_b.c_string(neirong) + "'";
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


            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }

            string table1 = "";
            try
            {
                table1 = Request.QueryString["table1"].ToString();
            }
            catch { }

            string table2 = "";
            try
            {
                table2 = Request.QueryString["table2"].ToString();
            }
            catch { }

            DataTable sl_order = my_c.GetTable("select distinct dianpu from sl_cart where yonghuming='" + my_b.k_cookie("user_name") + "' and (dingdanhao is null or dingdanhao='') and leixing='商品' and id in(" + Request["id"].ToString() + ")");



            if (type == "order")
            {
                #region 验证订单
                float jine = 0;
                string zhuangtai = my_b.c_string(Request["zhuangtai"].ToString());
                string suozaidiqu = my_b.c_string(Request["suozaidiqu"].ToString());
                //if (suozaidiqu !=" ")
                //{
                //    DataTable sl_yunfei = my_c.GetTable("select top 1 * from sl_yunfei where diqu='" + suozaidiqu.Split('-')[0].ToString() + "'");
                //    if (sl_yunfei.Rows.Count > 0)
                //    {
                //        if (jine_ < float.Parse(sl_yunfei.Rows[0]["mianyunfeijine"].ToString()))
                //        {
                //            jine_ = jine_ + float.Parse(sl_yunfei.Rows[0]["yunfei"].ToString());
                //        }

                //    }
                //}
                //Response.Write(jine_ +"||"+ jine + "||" + zhuangtai + "||" + suozaidiqu);
                //Response.End();

                if (zhuangtai != "未付款")
                {
                    Response.Redirect("/err.aspx?err=操作错误&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                if (my_b.k_cookie("user_name") != my_b.c_string(Request["yonghuming"].ToString()))
                {
                    Response.Redirect("/err.aspx?err=操作错误&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                #endregion 验证订单
                string hebingdingdan = my_b.get_bianhao() ;//后台生成合并订单号
                float zongjia = 0;
                for (int i = 0; i < sl_order.Rows.Count; i++)
                {
                    #region 提交订单
                    //处理订单
                    string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(table1);
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
                    //处理订单号
                    sql = sql + ",dingdanhao,jine,kuaidifei,dianpu,chanpinjine";
                    //处理订单号
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
                    #region 处理订单号
                    string dingdanhao = hebingdingdan;
                    #region 判断购物车中店铺用户是否多个

                    if (sl_order.Rows.Count > 1)
                    {
                        dingdanhao = my_b.get_bianhao();
                    }
                    #endregion
                    if (sl_order.Rows[i]["dianpu"].ToString() == "")
                    {
                        jine = float.Parse(my_c.GetTable("select sum(xiaoji) as count_id from sl_cart where yonghuming='" + my_b.k_cookie("user_name") + "' and (dingdanhao is null or dingdanhao='') and id in(" + Request["id"].ToString() + ") and leixing='商品' ").Rows[0]["count_id"].ToString());
                    }
                    else
                    {
                        jine = float.Parse(my_c.GetTable("select sum(xiaoji) as count_id from sl_cart where yonghuming='" + my_b.k_cookie("user_name") + "' and (dingdanhao is null or dingdanhao='') and id in(" + Request["id"].ToString() + ") and leixing='商品' and dianpu='" + sl_order.Rows[i]["dianpu"].ToString() + "'").Rows[0]["count_id"].ToString());
                    }

                    float chanpinjine = jine;
                    float yunfei_ = 0;
                    if (sl_order.Rows[i]["dianpu"].ToString() == "")
                    {
                        #region 店铺名称为空
                        float yunfei = float.Parse(ConfigurationSettings.AppSettings["yunfei"].ToString());
                        float mianyunfei = float.Parse(ConfigurationSettings.AppSettings["mianyunfei"].ToString());

                        if (jine > mianyunfei)
                        {

                        }
                        else
                        {
                            yunfei_ = yunfei;
                            jine = jine + yunfei;
                        }

                        #endregion 店铺名称为空
                    }
                    else
                    {
                        #region 店铺名称不为空
                        DataTable sl_dianpuyunfei = my_o.get_add(sl_order.Rows[i]["dianpuyonghuming"].ToString(), my_b.c_string(Request["suozaidiqu"].ToString()));
                        float yunfei = float.Parse(ConfigurationSettings.AppSettings["yunfei"].ToString());
                        float mianyunfei = float.Parse(ConfigurationSettings.AppSettings["mianyunfei"].ToString());
                        if (sl_dianpuyunfei.Rows.Count > 0)
                        {
                            yunfei = float.Parse(sl_dianpuyunfei.Rows[0]["yunfei"].ToString());
                            mianyunfei = float.Parse(sl_dianpuyunfei.Rows[0]["mianyunfei"].ToString());
                        }



                        if (jine > mianyunfei)
                        {

                        }
                        else
                        {
                            yunfei_ = yunfei;
                            jine = jine + yunfei;
                        }
                        #endregion 店铺名称不为空
                    }
                    sql = sql + ",'" + dingdanhao + "'," + jine + "," + yunfei_ + ",'" + sl_order.Rows[i]["dianpu"].ToString() + "'," + chanpinjine + "";
                    zongjia = jine + zongjia;
                    #endregion 处理订单号
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
                        Response.Write(sql);
                        Response.End();
                    }
                    //处理订单完

                    //处理优惠券
                    string youhuiquan = "";
                    try
                    {
                        youhuiquan = Request["youhuiquan"].ToString();
                    }
                    catch
                    { }
                    if (youhuiquan != "")
                    {
                        my_c.genxin("update sl_user_yhq set zhuangtai='已使用',dingdanhao='" + hebingdingdan + "' where youhuiquanbianhao='" + youhuiquan + "'");

                        //发送微信消息
                        //DataTable dt = my_c.GetTable("select * from sl_user_yhq where dingdanhao ='"+dingdanhao+"' and yonghuming='" + yonghuming + "'");
                        //string wherestrig = "where id=" + dt.Rows[0]["id"].ToString() + "";

                        //my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=youhuiquan&tablename=sl_user_yhq&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");
                        //end

                    }
                    //处理优惠券 end

                    //处理积分
                    float jifen = 0;
                    try
                    {
                        jifen = float.Parse(Request["jifen"].ToString());
                    }
                    catch
                    { }
                    if (jifen > 0)
                    {
                        my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + yonghuming + "','抵扣',-" + jifen + ",'未处理','" + hebingdingdan + "')");
                    }
                    //end
                    string shoujianrenxingming = "";
                    if (table2 == "add")
                    {
                        shoujianrenxingming = Request.Form["shoujianrenxingming"].ToString();
                        suozaidiqu = Request.Form["suozaidiqu"].ToString();
                        string jiedaodizhi = Request.Form["jiedaodizhi"].ToString();
                        string shoujihaoma = Request.Form["shoujihaoma"].ToString();


                        if (my_c.GetTable("select id from sl_add where shoujianrenxingming='" + shoujianrenxingming + "' and suozaidiqu='" + suozaidiqu + "' and jiedaodizhi='" + jiedaodizhi + "' and yonghuming='" + yonghuming + "'").Rows.Count == 0)
                        {
                            my_c.genxin("insert into sl_add (shoujianrenxingming,suozaidiqu,jiedaodizhi,shoujihaoma,yonghuming) values('" + shoujianrenxingming + "','" + suozaidiqu + "','" + jiedaodizhi + "','" + shoujihaoma + "','" + yonghuming + "')");
                        }

                    }
                    #region 处理购物车赋值
                    if (sl_order.Rows[i]["dianpu"].ToString() == "")
                    {
                        my_c.genxin("update sl_cart set dingdanhao='" + dingdanhao + "' where yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao='') and id in(" + Request["id"].ToString() + ") ");
                    }
                    else
                    {
                        my_c.genxin("update sl_cart set dingdanhao='" + dingdanhao + "' where yonghuming='" + yonghuming + "' and (dingdanhao is null or dingdanhao='') and id in(" + Request["id"].ToString() + ") and dianpu='" + sl_order.Rows[i]["dianpu"].ToString() + "'");
                    }



                    //这里是对库存的处理
                    my_o.set_xiaoliang(dingdanhao);
                    // HttpContext.Current.Response.End();
                    //end
                    #endregion
                    #endregion 提交完成
                }
                #region 提交合并订单
                my_c.genxin("insert into sl_hedan(yonghuming,dingdanhao,jine,zhifufangshi,zhuangtai) values('" + yonghuming + "','" + hebingdingdan + "'," + zongjia + ",'','未付款')");
                #endregion 提交合并订单


                float zongjia_ = 0;
                string tip_string = "下单成功，请尽快付款！";
                try
                {
                    tip_string = my_b.c_string(my_b.set_url_css(Request.QueryString["tip_string"].ToString()));
                }
                catch
                { }
                string tipurl = "/";
                try
                {
                    tipurl = my_b.c_string(my_b.set_url_css(Request.QueryString["tipurl"].ToString()));
                }
                catch
                { }






                string tipurl_type = "";
                try
                {
                    tipurl_type = my_b.c_string(my_b.set_url_css(Request.QueryString["tipurl_type"].ToString()));
                }
                catch
                { }
                if (tipurl_type == "alert")
                {
                    Response.Write("<script>alert('" + tip_string + "');window.location.href = '" + Request.UrlReferrer.ToString() + "'; window.event.returnValue = false;;</script>");
                }
                else if (tipurl_type == "ajax")
                {
                    Response.Write(hebingdingdan);
                }
                else
                {

                    Response.Redirect("/err.aspx?err=" + tip_string + "&errurl=" + my_b.tihuan(tipurl + "&dingdanhao=" + hebingdingdan + "", "&", "fzw123") + "");
                }
                //所有增加完成
            }

            //输出数据
            //my_html my_h = new my_html();
            //string file_content = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + my_b.set_moban(ConfigurationSettings.AppSettings["template"].ToString(), ConfigurationSettings.AppSettings["templatewap"].ToString(), "/order.html"), Encoding.UTF8);
            //替换

            //end
            //  Response.Write(my_h.Single_page(file_content));



        }
    }
}
