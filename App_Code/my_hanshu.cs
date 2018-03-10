using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Web.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using System.Web.SessionState;
using Senparc.Weixin.Exceptions;
using LitJson;
/// <summary>
///my_conn 的摘要说明
/// </summary>
public class my_hanshu
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    jiami jm = new jiami();
    //处理退货或取消订单时退还积分、余额等
    #region 去除url中的参数
    public string qucanshu(string quer_string,string type)
    {
        StringBuilder return_string = new StringBuilder();
        if (quer_string != "")
        {
            string[] quer_arr = quer_string.Split('&');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                string[] sql_arr = quer_arr[i].ToString().Split('=');
                if (HttpUtility.UrlDecode(sql_arr[0]) != type)
                {
                    if (return_string.ToString() != "") return_string.Append("&");
                    return_string.Append(quer_arr[i].ToString());
                }
            }
        }
        return return_string.ToString();
    }
    #endregion
    #region 获取下级所有会员
    public string get_yonghuming(string yonghuming,string ziduantitle)
    {
        string pageid = "";
        try
        {
            pageid = my_b.c_string(HttpContext.Current.Request.QueryString["pageid"].ToString());
        }
        catch { }

        string yonghuming_str = "";
        DataTable sl_yuangong = my_c.GetTable("select * from sl_yuangong where id in (" + yonghuming + ")");
        if (sl_yuangong.Rows.Count > 0)
        {
          
            #region 员工数大于0
            for (int i = 0; i < sl_yuangong.Rows.Count; i++)
            {
                string suoshuzhiwei = sl_yuangong.Rows[i]["suoshuzhiwei"].ToString();
                string suoshubumen = sl_yuangong.Rows[i]["suoshubumen"].ToString();
                DataTable sl_zhiwei = my_c.GetTable("select * from sl_zhiwei where id=" + suoshuzhiwei);
                DataTable sl_jigou = my_c.GetTable("select * from 	sl_jigou where id=" + suoshubumen);
                //HttpContext.Current.Response.Write(sl_zhiwei.Rows[0]["shujuxingquanxian"].ToString());
                //HttpContext.Current.Response.End();
                if (sl_zhiwei.Rows.Count > 0)
                {
                    #region 有角色信息
                    Regex reg = new Regex("{next}", RegexOptions.Singleline);
                    string[] shujuxingquanxian = reg.Split(sl_zhiwei.Rows[0]["shujuxingquanxian"].ToString());
                    // HttpContext.Current.Response.Write(sl_zhiwei.Rows[0]["shujuxingquanxian"].ToString() + "<br>");
                    for (int j = 0; j < shujuxingquanxian.Length; j++)
                    {
                        #region 某一行
                        Regex reg1 = new Regex("{and}", RegexOptions.Singleline);
                        string[] yihang = reg1.Split(shujuxingquanxian[j].ToString());
                        // HttpContext.Current.Response.Write(shujuxingquanxian[i].ToString()+"<br>");
                        //   HttpContext.Current.Response.End();
                        if (yihang[1] == pageid)
                        {
                            #region 有相等的页面ID
                            string quanxian = yihang[0];
                            //HttpContext.Current.Response.Write(quanxian);
                            //HttpContext.Current.Response.End();
                            if (quanxian == "全公司")
                            {
                                yonghuming_str = "";
                            }
                            else if (quanxian == "本部门")
                            {
                                #region 本部门的处理
                                DataTable jigou = my_c.GetTable("select classid,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou ");

                                try
                                {
                                    suoshubumen = get_all_id(jigou, suoshubumen);
                                }
                                catch { }
                                //HttpContext.Current.Response.Write(suoshubumen);
                                //HttpContext.Current.Response.End();
                                if (suoshubumen != "")
                                {
                                    yonghuming_str = "" + ziduantitle + " in (select id from sl_yuangong where suoshubumen in (" + suoshubumen + "))";
                                }
                                else
                                {
                                    yonghuming_str = "" + ziduantitle + " in (" + yonghuming + ")";
                                }

                                #endregion
                            }
                            else
                            {
                                yonghuming_str = "" + ziduantitle + " in (" + yonghuming + ")";
                            }
                            //HttpContext.Current.Response.Write(yonghuming_str+"||"+ quanxian);
                            //HttpContext.Current.Response.End();
                            #endregion
                        }
                        #endregion
                    }

                    #endregion
                }
                if (sl_jigou.Rows.Count > 0)
                {
                    #region 有机构信息
                  
                    #endregion
                }

            }
            #endregion
        }
        else
        {
            #region 员工数=0
            yonghuming_str = "" + ziduantitle + " in (" + yonghuming + ")";
            #endregion
        }

        return yonghuming_str;
    }
    #endregion
    #region 列出所有下级的ID
    public string get_all_id(DataTable jigou, string id)
    {
        DataRow[] rows = jigou.Select("classid=" + id + "");
        //HttpContext.Current.Response.Write(rows.Length);
        //HttpContext.Current.Response.End();
        StringBuilder str = new StringBuilder();
        // int i = 0;
        if (rows.Length > 0)
        {
          
            for (int i = 0; i < rows.Length; i++)
            {
               
                if (i != 0) str.Append(",");
                str.Append(rows[i]["id"].ToString());
                DataRow[] rowscount = jigou.Select("classid=" + rows[i]["id"].ToString());
                if (rowscount.Length > 0)
                {
                    str.Append(",");
                    str.Append(get_all_id(jigou, rows[i]["id"].ToString()));
                }
            }
        }
        if (str.ToString() != "")
        {
            str.Append(",");
        }
       
        str.Append(id);
        return str.ToString();
    }
    #endregion
    #region 列出组织最顶级目录
    string get_ding_dir_str = "";

    public void Parameterweizhi(string classid)
    {
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou where id=" + classid + " and classid>=0");
        if (dt1.Rows.Count > 0)
        {
            if (get_ding_dir_str == "")
            {
                get_ding_dir_str = dt1.Rows[0]["jigoumingcheng"].ToString();
            }
            else
            {
                get_ding_dir_str = dt1.Rows[0]["jigoumingcheng"].ToString() + "," + get_ding_dir_str;
            }
            if (dt1.Rows[0]["classid"].ToString() != "0")
            {
                Parameterweizhi(dt1.Rows[0]["classid"].ToString());
            }
        }


    }
    #endregion
    #region 列出机构所有上级ID

    public string get_jigou(DataTable jigou, string id)
    {
        DataRow[] rows = jigou.Select("id=" + id + "");
        StringBuilder str = new StringBuilder();
        // int i = 0;
        if (rows.Length > 0)
        {

            for (int i = 0; i < rows.Length; i++)
            {
            
                if (i != 0) str.Append(",");
                str.Append(rows[i]["classid"].ToString());
                DataRow[] rowscount = jigou.Select("id=" + rows[i]["classid"].ToString());
                if (rowscount.Length > 0)
                {
                    str.Append(",");
                    str.Append(get_jigou(jigou, rows[i]["classid"].ToString()));
                }

            }
        }
        str.Append(",");
        str.Append(id);
        return str.ToString();
    }
    #endregion
    #region 字段函数
    public string Fields_hanshu(string type,string zhi)
    {
        string hanshu_str = "";
        if (zhi != "")
        {

            //HttpContext.Current.Response.Write(zhi);
            //HttpContext.Current.Response.End();
            #region 值不等于空
            if (type == "jigou")
            {
                #region 获取机构信息
                string classid = "";
                DataTable sl_yuangong = my_c.GetTable("select * from sl_yuangong where id in (" + zhi + ")");
                for (int i = 0; i < sl_yuangong.Rows.Count; i++)
                {
                    get_ding_dir_str = "";
                    classid = sl_yuangong.Rows[i]["suoshubumen"].ToString();
                    Parameterweizhi(classid);
                    if (hanshu_str == "")
                    {
                        hanshu_str = get_ding_dir_str;
                    }
                    else
                    {
                        hanshu_str = hanshu_str+"{next}"+get_ding_dir_str;
                    }
                    
                }
                #endregion
            }
            if (type == "shouji")
            {
                #region 处理手机号码
                zhi = jm.Decrypt(zhi);
                string pageid = "";
                try
                {
                    pageid = my_b.c_string(HttpContext.Current.Request.QueryString["pageid"].ToString());
                }
                catch { }
                if (pageid == "18")
                {
                    hanshu_str = my_b.ReturnPhoneNO(zhi);
                }
                else
                {
                    hanshu_str = zhi;
                }
             
                #endregion
            }
            #endregion
        }

        //HttpContext.Current.Response.Write(zhi);
        //HttpContext.Current.Response.End();
        return hanshu_str;
    }
    #endregion
    


     #region 根据表不同生成SQL
    /// <summary>
    ///根据表不同生成SQL
    /// </summary>
    /// <param name="table_name">当前查询的表名</param>
    /// <returns></returns>
    public string get_diff_table_sql(string table_name)
    {
        string quer_sql = string.Empty;
        string query_value = string.Empty;
        string query_value_temp = string.Empty;
        string key_str = string.Empty;

        try
        {
            query_value = HttpContext.Current.Request.QueryString["teshu"];
            query_value_temp = query_value.Replace("{fenge1}","|");
            string[] array_temp = query_value_temp.Split('|');
            for (int i = 0; i < array_temp.Length; i++)
            {
                string query_value_temp2 = string.Empty;
                query_value_temp2 = array_temp[i].Replace("{fenge2}","|");
                string[] array_temp2 = query_value_temp2.Split('|');
                for (int j = 0; j < array_temp2.Length;j++ )
                {
                    if (string.IsNullOrEmpty(key_str))
                    {
                        key_str = array_temp2[0] + " like '%" + array_temp2[1] + "%' ";
                    }
                    else
                    {
                        key_str += " or " + array_temp2[0] + " like '%" + array_temp2[1] + "%' ";
                    
                    }
                    
                }

            }
            

        }
        catch { }




       
        switch (table_name)
        { 
            //兼容之前的老接口
            case "hujiaotongji":
                if (!string.IsNullOrEmpty(query_value))
                 {
                     try {
                         string teshu_temp = HttpContext.Current.Request.QueryString["teshu"];
                         quer_sql = " yonghuming in (select id from sl_yuangong where yuangongxingming like '%" + teshu_temp + "%' or yonghuming like '%" + teshu_temp + "%' ) "; 

                     }
                     catch { }
                    
                 }
                
                break;
            case "hujiaofeiyong":
                if (!string.IsNullOrEmpty(query_value))
                {
                    try
                    {
                        string teshu_temp = HttpContext.Current.Request.QueryString["teshu"];
                        quer_sql = " yonghuming in (select id from sl_yuangong where yuangongxingming like '%" + query_value + "%' or yonghuming like '%" + query_value + "%' ) ";
                    }
                    catch { }
                   
                }
               break;
            case "hujiaotongjimingxi":
               if (!string.IsNullOrEmpty(query_value))
               {
                   try
                   {
                       string teshu_temp = HttpContext.Current.Request.QueryString["teshu"];
                       quer_sql = " yonghuming in (select id from sl_yuangong where yuangongxingming like '%" + query_value + "%' or yonghuming like '%" + query_value + "%' ) ";
                   }
                   catch { }
               }
               break;


               

        
        }

        if (string.IsNullOrEmpty(query_value))
        {
           // quer_sql = " 1=1 ";
        }
        return quer_sql;
    }

    #endregion
    #region 获取用户的姓名、及组名（可多级）
    public string get_yhm_info()
    {
        return "";
    }
    #endregion

}
