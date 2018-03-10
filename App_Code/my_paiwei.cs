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
/// my_paiwei 的摘要说明
/// </summary>
public class my_paiwei
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    #region 奖励制度
    public void jianglizhidu(string biaoti, string yonghuming, string dingdanhao)
    {
        DataTable sl_jianglizhidu = my_c.GetTable("select * from sl_jianglizhidu where biaoti='" + biaoti + "'");
        string zhuangtai = "";
        string leixing = "";
        string miaoshu = "";
        string jine = "";
        if (biaoti == "投资金额")
        {
          //  my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dingdanhao + "','','" + yonghuming + "','正常','消费','注册需要扣出金额',-" + sl_jianglizhidu.Rows[0]["jine"].ToString() + ")");//投资金额

            zhuangtai = "冻结";
            leixing = "投资";
            miaoshu = "注册投资。";
            jine = sl_jianglizhidu.Rows[0]["jine"].ToString();
        }
        else if (biaoti == "直推奖励")
        {
            DataTable sl_user = my_c.GetTable("select id from sl_user where jieshaoren='" + yonghuming + "'");
            zhuangtai = "冻结";
            if (sl_user.Rows.Count > 2)
            {
                zhuangtai = "正常";
            }

            leixing = "直推奖励";
            miaoshu = "介绍会员得到直推奖励。";
            jine = sl_jianglizhidu.Rows[0]["jine"].ToString();
        }
        else if (biaoti == "广告费")
        {
            zhuangtai = "冻结";
            leixing = "广告费";
            miaoshu = "介绍会员得到广告费。";
            jine = sl_jianglizhidu.Rows[0]["jine"].ToString();
        }
        else if (biaoti == "邮费补贴")
        {
            zhuangtai = "正常";
            leixing = "邮费补贴";
            miaoshu = "提位得到邮费补贴。";
            jine = sl_jianglizhidu.Rows[0]["jine"].ToString();
        }
        else if (biaoti == "出局奖励")
        {
            zhuangtai = "正常";
            leixing = "出局奖励";
            miaoshu = "正常出局奖励。";
            jine = sl_jianglizhidu.Rows[0]["jine"].ToString();
        }
        else
        {

        }
        try
        {
            my_c.genxin("insert into sl_caiwu(dingdanhao,zhifufangshi,yonghuming,zhuangtai,leixing,miaoshu,jine) values('" + dingdanhao + "','','" + yonghuming + "','" + zhuangtai + "','" + leixing + "','" + miaoshu + "'," + jine + ")");//投资金额
        }
        catch
        {
            HttpContext.Current.Response.Write("select * from sl_jianglizhidu where biaoti='" + biaoti + "'");
            HttpContext.Current.Response.End();
        }
       


    }
    #endregion
    #region 获取排位人
    public string get_dailiren()
    {
        string dailiren = "";
        DataTable dt = my_c.GetTable("select yonghuming from sl_user where lunshu in (SELECT MIN(lunshu)   FROM sl_user) order by id asc");

        DataTable dt1 = my_c.GetTable("select * from(select count(dailiren) as num, dailiren from sl_user where dailiren <> ''  group by dailiren) bb ");

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                DataRow[] rows = dt1.Select("dailiren='"+ dt.Rows[i]["yonghuming"] + "'");
                if (rows.Length > 0)
                {
                    if (int.Parse(rows[0]["num"].ToString()) < 2)
                    {
                        dailiren = rows[0]["dailiren"].ToString();
                        break;
                    }
                  
                }
                else
                {
                    dailiren = dt.Rows[i]["yonghuming"].ToString();
                    break;
                }
            }
        }
        //if (dailiren == "")
        //{
        //    dt = my_c.GetTable("select top 1 * from sl_user order by id asc");
        //    if (dt.Rows.Count > 0)
        //    {
        //        dailiren = dt.Rows[0]["yonghuming"].ToString();
        //    }
        //}
        

        return dailiren;
    }
    #endregion
    #region 列出组织最顶级目录
   

    public int get_dailiren(string yonghuming)
    {
        int get_dailiren_str = 0;
        DataTable sl_user = my_c.GetTable("select id,yonghuming from sl_user where dailiren='" + yonghuming + "'");//
   
        get_dailiren_str = get_dailiren_str + sl_user.Rows.Count;

        for (int i = 0; i < sl_user.Rows.Count; i++)
        {
            DataTable sl_user1 = my_c.GetTable("select id from sl_user where dailiren='" + sl_user.Rows[i]["yonghuming"] + "'");//
            if (sl_user1.Rows.Count > 0)
            {
                get_dailiren_str = get_dailiren_str + sl_user1.Rows.Count;
            }
         
        }
        return get_dailiren_str + 1;
    }
    #endregion
    #region 排位处理
    public void set_paiwei(string dailiren)
    {
        DataTable sl_user = my_c.GetTable("select id from sl_user where dailiren='" + dailiren + "'");//计算我的代理人是否满2个
        if (sl_user.Rows.Count == 2)
        {
            DataTable sl_user1 = my_c.GetTable("select dailiren from sl_user where yonghuming='" + dailiren + "'");//计算我的代理人的代理人
            if (sl_user1.Rows.Count > 0)
            {
                sl_user = my_c.GetTable("select * from sl_user where dailiren='" + sl_user1.Rows[0]["dailiren"].ToString() + "'");
                if (sl_user.Rows.Count == 2)
                {
                    sl_user1 = my_c.GetTable("select dailiren from sl_user where yonghuming='" + dailiren + "'");//获取到顶级代理人
                    //HttpContext.Current.Response.Write(sl_user1.Rows[0]["dailiren"].ToString());
                    //HttpContext.Current.Response.End();
                    int shuliang = get_dailiren(sl_user1.Rows[0]["dailiren"].ToString());
                    //HttpContext.Current.Response.Write(shuliang);
                    //HttpContext.Current.Response.End();
                    if (shuliang == 7)
                    {
                        #region 层级人数等于7
                        string dailiren_ = get_dailiren();

                        #region 费用结算
                        string dingdanhao = my_b.get_bianhao();
                        jianglizhidu("出局奖励", sl_user1.Rows[0]["dailiren"].ToString(), dingdanhao);//出局奖励

                        jianglizhidu("投资金额", sl_user1.Rows[0]["dailiren"].ToString(), dingdanhao);//投资金额
                        string jieshaoren = "";
                        try
                        {
                            jieshaoren = my_c.GetTable("select * from sl_user where yonghuming='" + sl_user1.Rows[0]["dailiren"].ToString() + "'").Rows[0]["jieshaoren"].ToString();
                        }
                        catch { }
                        if (jieshaoren != "")
                        {
                            jianglizhidu("直推奖励", jieshaoren, dingdanhao);//直推奖励
                            jianglizhidu("广告费", jieshaoren, dingdanhao);//广告费
                        }
                        jianglizhidu("邮费补贴", dailiren_, dingdanhao);//邮费补贴
                        #endregion
                        HttpContext.Current.Response.Write("update sl_user set dailiren='" + dailiren_ + "' , lunshu=lunshu+1 where yonghuming='" + sl_user1.Rows[0]["dailiren"].ToString() + "'<br>");
                       // HttpContext.Current.Response.End();
                        my_c.genxin("update sl_user set dailiren='" + dailiren_ + "' , lunshu=lunshu+1 where yonghuming='" + sl_user1.Rows[0]["dailiren"].ToString() + "'");
                        my_c.genxin("update sl_user set dailiren='' where dailiren='" + sl_user1.Rows[0]["dailiren"].ToString() + "'");
                        #endregion
                    }

                }
            }
           
        }
    }
    #endregion
}