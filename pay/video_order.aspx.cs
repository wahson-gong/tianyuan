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

public partial class video_order : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();

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
            if (yonghuming == "")
            {

                my_b.user_sta("user_name");
            }
            //end


            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }

         


            if (type == "add")
            {
                #region 提交订单
                string dingdanhao = my_b.c_string(Request["dingdanhao"].ToString());
                if (my_c.GetTable("select * from sl_kc_order where dingdanhao='" + dingdanhao + "'").Rows.Count > 0)
                {
                    Response.Redirect("/err.aspx?err=操作错误&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                string fangshi = my_b.c_string(Request["fangshi"].ToString());
                string jiebianhao = "";
                try
                {
                    jiebianhao = my_b.c_string(Request["jiebianhao"].ToString());
                }
                catch { }
                if (yonghuming != my_b.c_string(Request["yonghuming"].ToString()))
                {
                    Response.Redirect("/err.aspx?err=用户名操作错误&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
              
                string laiyuanbianhao = my_b.c_string(Request["laiyuanbianhao"].ToString());
                DataTable sl_kecheng = my_c.GetTable("select * from	sl_kecheng where id=" + laiyuanbianhao + "");
                float jine = 0;
                if (fangshi == "部分")
                {
                 
                    if (sl_kecheng.Rows.Count > 0)
                    {
                        DataTable sl_jie = my_c.GetTable("select * from	sl_jie where  id in ("+ jiebianhao + ")");
              
                        for (int i = 0; i < sl_jie.Rows.Count; i++)
                        {
                            #region 处理节
                             jine = jine + float.Parse(sl_jie.Rows[i]["jiage"].ToString());
                            my_c.genxin("insert into sl_kc_cart(biaoti,danjia,shuliang,xiaoji,laiyuanbianhao,zhangbianhao,jiebianhao,dingdanhao,yonghuming) values('" + sl_kecheng.Rows[0]["biaoti"].ToString()+ sl_jie.Rows[i]["biaoti"].ToString() + "'," + sl_jie.Rows[i]["jiage"].ToString() + ",1," + sl_jie.Rows[i]["jiage"].ToString() + "," + laiyuanbianhao + ",0,"+ sl_jie.Rows[i]["id"].ToString() + ",'" + dingdanhao + "','" + yonghuming + "')");
                            #endregion 处理节
                        }
                     
                    }
                    else
                    {
                        Response.Redirect("/err.aspx?err=无此数据&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                    }
                }
                else
                {
                  
                    if (sl_kecheng.Rows.Count > 0)
                    {
                        jine = float.Parse(sl_kecheng.Rows[0]["jiage"].ToString());
          
                        my_c.genxin("insert into sl_kc_cart(biaoti,danjia,shuliang,xiaoji,laiyuanbianhao,zhangbianhao,jiebianhao,dingdanhao,yonghuming) values('"+sl_kecheng.Rows[0]["biaoti"].ToString()+"',"+ sl_kecheng.Rows[0]["jiage"].ToString() + ",1," + sl_kecheng.Rows[0]["jiage"].ToString() + "," + laiyuanbianhao+",0,0,'"+dingdanhao+"','"+yonghuming+"')");
                        
                    }
                    else
                    {
                        Response.Redirect("/err.aspx?err=无此数据&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                    }
                   
                }
                if (jine != float.Parse(Request["jine"].ToString()))
                {
                    my_c.genxin("delete from sl_kc_cart where dingdanhao='" + dingdanhao + "'");
                    Response.Redirect("/err.aspx?err=金额不对&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                //处理订单
                string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(my_b.c_string(Request["t"].ToString()));
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
                if (youhuiquan!="")
                {
                    my_c.genxin("update sl_user_yhq set zhuangtai='已使用',dingdanhao='" + dingdanhao + "' where youhuiquanbianhao='" + youhuiquan + "'");

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
                    my_c.genxin("insert into sl_jifen(yonghuming,leixing,fenshu,zhuangtai,shijian) values('" + yonghuming + "','抵扣',-" + jifen + ",'未处理','" + dingdanhao + "')");
                }
                //end
               


             
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
                    Response.Write(dingdanhao);
                }
                else
                {

                    Response.Redirect("/err.aspx?err=" + tip_string + "&errurl=" + my_b.tihuan(tipurl + "&dingdanhao=" + dingdanhao + "", "&", "fzw123") + "");
                }
                //所有增加完成
                #endregion 提交订单


            }





        }
    }
}
