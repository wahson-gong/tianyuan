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
using System.Text;
using System.Text.RegularExpressions;

public partial class daoruexcle : System.Web.UI.Page
{

    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_json my_js = new my_json();
    biaodan bd = new biaodan();
    jiami jm = new jiami();
    daoru dr = new daoru();

    int zongshu, zhongfu, wuxiao = 0, chenggong = 0, yifugai = 0; //统计字段
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string ziduan= "";
            try
            {
                ziduan = Request.QueryString["ziduan"].ToString();
            }
            catch { }
            string type = Request.QueryString["type"].ToString();
            string Pathurl = Request.QueryString["Pathurl"].ToString();
            string callback = Request.QueryString["jsoncallback"];
            if (type == "tablelist")
            {
                #region 获取库中所有表名
                DataTable tablelist = dr.Exceltable(Server.MapPath(Pathurl));
                Response.Write(my_js.XLSToJson("tablelist", tablelist));
                #endregion
            }
            else if (type == "tableinfo")
            {
                #region 获取表数据
                int number = 0;
                try
                {
                    number = int.Parse(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = Request.QueryString["table_name"].ToString();
                DataTable tableinfo = dr.ExcelToDS(Server.MapPath(Pathurl), table_name, number);
                Response.Write(my_js.XLSToJson("tablelist", tableinfo));
                #endregion
            }
            else if (type == "tableColumns")
            {
                #region 获取表字段
                int number = 1;
                string table_name = Request.QueryString["table_name"].ToString();
                DataTable tableinfo = dr.ExcelToDS(Server.MapPath(Pathurl), table_name, number);
                string Columns_str = "";
                for (int d1 = 0; d1 < tableinfo.Columns.Count; d1++)
                {
                    if (Columns_str == "")
                    {
                        Columns_str = tableinfo.Columns[d1].ToString();
                    }
                    else
                    {
                        Columns_str = Columns_str+","+tableinfo.Columns[d1].ToString();
                    }
                }
                string result = "({\"date\":\"" + Columns_str + "\"})";
                Response.Write(result);
                #endregion
            }
            else if (type == "tablerowcount")
            {
                #region 获取表行数
                int number = 0;
                string table_name = Request.QueryString["table_name"].ToString();
                DataTable tableinfo = dr.ExcelToDS(Server.MapPath(Pathurl), table_name, number);

                string result = "({\"date\":\"" + tableinfo.Rows.Count + "\"})";
                Response.Write(result);
                #endregion
            }
            else if (type == "tableall")
            {
                #region 获取表所有
                string data = "";
                #region 获取库中所有表名
                DataTable tablelist = dr.Exceltable(Server.MapPath(Pathurl));
                //Response.Write(my_js.XLSToJson("tablelist", tablelist));
                data = "\"tableinfo\":"+my_js.XLSToJson("tablelist", tablelist);
                #endregion
                #region 获取表数据
                int number = 0;
                try
                {
                    number = int.Parse(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = tablelist.Rows[0]["table_name"].ToString();
                DataTable tableinfo = dr.ExcelToDS(Server.MapPath(Pathurl), table_name, number);
                //Response.Write(my_js.XLSToJson("tablelist", tableinfo));
                //Response.End();

                data = data +",\"tabledata\":"+ my_js.XLSToJson("tablelist", tableinfo);
                #endregion
                #region 获取表字段
                string Columns_str = "";
                for (int d1 = 0; d1 < tableinfo.Columns.Count; d1++)
                {
                    if (Columns_str == "")
                    {
                        Columns_str = "{\"title\":\"" + tableinfo.Columns[d1].ToString() + "\",\"key\":\""+ tableinfo.Columns[d1].ToString() + "\"}";
                    }
                    else
                    {
                        Columns_str = Columns_str + "," + "{\"title\":\"" + tableinfo.Columns[d1].ToString() + "\",\"key\":\"" + tableinfo.Columns[d1].ToString() + "\"}";
                    }
                }
                string result = ",\"tableColumns\":[" + Columns_str + "]";
                //Response.Write(result);
                data = data + result;
                #endregion
                #region 获取表行数
                tableinfo = dr.ExcelToDS(Server.MapPath(Pathurl), table_name, 0);
                result = ",\"tablerowcount\":\"" + tableinfo.Rows.Count + "\"";
                //Response.Write(result);
                data = data + result;
                #endregion
                Response.Write(callback + "({" + data + "})");
                #endregion
            }
            else if (type == "daoru")
            {
                string bianhao = my_b.get_bianhao();
                string Modelu1 = Request.QueryString["Modelu1"].ToString();
                string suoshuqudao = "";

                string chuliren = "0";
                try
                {
                    chuliren = my_c.GetTable("select id from sl_yuangong where id=" + my_b.k_cookie("user_name") + "").Rows[0]["id"].ToString();
                }
                catch { }
                string leixing = "未分配客户";
                string youxian = Request.QueryString["youxian"].ToString();
        
                if (Modelu1 == "sl_kehu")
                {
                    suoshuqudao = Request.QueryString["suoshuqudao"].ToString();
                }
                #region 开始导入
                DataTable Model_dt = new DataTable();
                string u1 = dr.get_data_Field(ziduan);
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + Modelu1 + "') and ("+ u1 + ")");
                //Response.Write("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + Modelu1 + "') and (" + u1 + ")");
                //Response.End();
                #region 获取excle表数据
                int number = 0;
                //try
                //{
                //    number = int.Parse(Request.QueryString["number"].ToString());
                //}
                //catch { }
                string table_name = Request.QueryString["table_name"].ToString();
                DataTable dt = dr.ExcelToDS(Server.MapPath(Pathurl), table_name, number);

                 dt = dr.GetDistinctTable(dt);
                //Response.Write(dt.Rows.Count);
                //Response.End();
                #endregion
                string u3 = my_c.GetTable("select u3 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + Modelu1 + "'").Rows[0]["u3"].ToString();
                #region 循环
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (u3 == "文章模型")
                    {

                    }
                    else
                    {
                        //新建sql
                        string sql = "insert into " + Modelu1 + " ";
                        if (Modelu1 == "sl_kehu")
                        {
                            sql = sql + "(qudaolaiyuan,chuliren,leixing,bianhao,";
                        }
                        else
                        {
                            sql = sql + "(";
                        }
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
                        if (Modelu1 == "sl_kehu")
                        {
                            sql = sql + ") values ('" + suoshuqudao + "','" + chuliren + "','" + leixing + "','"+ bianhao + "',";
                        }
                        else
                        {
                            sql = sql + ") values (";
                        }
                        int haoma = 0;
                        for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                        {
                            if (Modelu1 == "sl_kehu")
                            {
                                #region 客户表
                                if (Model_dt.Rows[d1]["u1"].ToString() == "dianhua")
                                {
                                    string zhi = dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(),ziduan)].ToString();
                                    string shouji = my_b.get_shouji(zhi);
                                    if (shouji == "")
                                    {
                                        haoma++;
                                    }
                                    if (d1 == 0)
                                    {
                                        sql = sql + bd.value_get_kj(Model_dt, d1, shouji);
                                    }
                                    else
                                    {
                                        sql = sql + "," + bd.value_get_kj(Model_dt, d1, shouji);
                                    }
                                }
                                else
                                {
                                    if (d1 == 0)
                                    {
                                        sql = sql + bd.value_get_kj(Model_dt, d1, dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(),ziduan)].ToString());
                                    }
                                    else
                                    {
                                        sql = sql + "," + bd.value_get_kj(Model_dt, d1, dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(),ziduan)].ToString());
                                    }
                                }
                               
                               
                                #endregion
                            }
                            else
                            {
                                #region 其它表
                                if (d1 == 0)
                                {
                                    sql = sql + bd.value_get_kj(Model_dt, d1, dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(),ziduan)].ToString());
                                }
                                else
                                {
                                    sql = sql + "," + bd.value_get_kj(Model_dt, d1, dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(),ziduan)].ToString());
                                }
                                #endregion
                            }

                           
                        }
                        sql = sql + ")";
                        //end


                        zongshu++;
                        //Response.Write(sql);
                        //Response.End();
                        if (haoma > 0)
                        {
                            wuxiao++;
                        }
                        else
                        {
                            //判断sql
                            string panduansql = "select id from " + Modelu1 + " where ";
                            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                            {
                                if (Model_dt.Rows[d1]["u1"].ToString() == "dianhua")
                                {
                                    string zhi = dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(), ziduan)].ToString();
                                    string shouji = my_b.get_shouji(zhi);
                                    if (d1 == 0)
                                    {
                                        panduansql = panduansql +" " + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.value_get_kj(Model_dt, d1, shouji);
                                    }
                                    else
                                    {
                                        panduansql = panduansql + " and " + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.value_get_kj(Model_dt, d1, shouji);
                                    }
                                }
                                else
                                {
                                    if (d1 == 0)
                                    {
                                        panduansql = panduansql + " " + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.value_get_kj(Model_dt, d1, dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(), ziduan)].ToString()) + "";
                                    }
                                    else
                                    {
                                        panduansql = panduansql + " and " + Model_dt.Rows[d1]["u1"].ToString() + "=" + bd.value_get_kj(Model_dt, d1, dt.Rows[i][dr.get_text2(Model_dt.Rows[d1]["u1"].ToString(), ziduan)].ToString()) + "";
                                    }
                                }
                                
                            }
                            //end
                            //Response.Write(sql + "<br>");
                            //Response.Flush();
                            //Response.End();
                            // my_c.genxin(sql);
                            //int dt_count = 0;
                            #region 判断类型不等于已分配客户
                            if (Modelu1 == "sl_kehu")
                            {
                                panduansql = panduansql + " and leixing<>'已分配客户' and 	shanchuzhuangtai>-1";
                            }
                            #endregion
                            DataTable dtcount = new DataTable();
                            int dt_count = 0;
                            try
                            {
                                dtcount = my_c.GetTable(panduansql);
                                dt_count = dtcount.Rows.Count;
                            }
                            catch { }
                            
                            if (dt_count == 0)
                            {
                                #region 数据库中无数据
                                string sta = "1";
                                try
                                {
                                    my_c.genxin(sql);
                                    sta = "0";
                                }
                                catch
                                {
                                    sta = "1";
                                }
                                if (sta == "1")
                                {
                                    wuxiao++;
                                    //Response.Write(sql + "行<span style='color:red'>失败</span><br>");
                                    //Response.Flush();
                                }
                                else
                                {
                                    chenggong++;
                                    //Response.Write(sql + "行成功<br>");
                                    //Response.Flush();
                                }
                                #endregion
                            }
                            else
                            {
                                #region 数据库中有数据 

                                if (youxian == "普通")
                                {
                                    DataTable fanhui = my_c.GetTable(sql + " select @@IDENTITY as id");
                                    my_c.genxin("update " + Modelu1 + " set shanchuzhuangtai=-2 where id=" + fanhui.Rows[0]["id"].ToString());
                                    zhongfu++;
                                }
                                else
                                {
                                    #region 优先级为高级
                                    my_c.genxin("update " + Modelu1 + " set shanchuzhuangtai=-1 where id=" + dtcount.Rows[0]["id"].ToString());
                                    string sta = "1";
                                    try
                                    {
                                        my_c.genxin(sql);
                                        sta = "0";
                                    }
                                    catch
                                    {
                                        sta = "1";
                                    }
                                    if (sta == "1")
                                    {
                                        wuxiao++;
                                        //Response.Write(sql + "行<span style='color:red'>失败</span><br>");
                                        //Response.Flush();
                                    }
                                    else
                                    {
                                        chenggong++;
                                        //Response.Write(sql + "行成功<br>");
                                        //Response.Flush();
                                    }
                                    #endregion
                                    yifugai++;
                                }
                                #endregion
                                //Response.Write(sql + "行<span style='color:green'>重复</span><br>");
                                //Response.Flush();
                            }
                        }
                        

                        //end
                    }
                }
                #endregion
              //  Response.End();
                #region 处理多个号码的情况
                if (Modelu1 == "sl_kehu")
                {
                    DataTable sl_kehu = my_c.GetTable("select * from sl_kehu where bianhao='" + bianhao + "' and dianhua like '%,%'");
                    for (int i = 0; i < sl_kehu.Rows.Count; i++)
                    {
                        
                        string[] dianhua = jm.Decrypt(sl_kehu.Rows[i]["dianhua"].ToString()).Split(',');
                        for (int j = 0; j < dianhua.Length; j++)
                        {
                            if (j >1)
                            {
                                #region 有多条的情况
                                string sql = "insert into " + Modelu1 + " ";
                                if (Modelu1 == "sl_kehu")
                                {
                                    sql = sql + "(qudaolaiyuan,chuliren,leixing,bianhao,";
                                }
                                else
                                {
                                    sql = sql + "(";
                                }
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
                                if (Modelu1 == "sl_kehu")
                                {
                                    sql = sql + ") values ('" + suoshuqudao + "','" + chuliren + "','" + leixing + "','" + bianhao + "',";
                                }
                                else
                                {
                                    sql = sql + ") values (";
                                }

                                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                                {
                                    if (Model_dt.Rows[d1]["u1"].ToString() == "dianhua")
                                    {
                                        if (d1 == 0)
                                        {
                                            sql = sql + bd.value_get_kj(Model_dt, d1, dianhua[j]);
                                        }
                                        else
                                        {
                                            sql = sql + "," + bd.value_get_kj(Model_dt, d1, dianhua[j]);
                                        }
                                    }
                                    else
                                    {
                                        if (d1 == 0)
                                        {
                                            sql = sql + bd.value_get_kj(Model_dt, d1, sl_kehu.Rows[i][Model_dt.Rows[d1]["u1"].ToString()].ToString());
                                        }
                                        else
                                        {
                                            sql = sql + "," + bd.value_get_kj(Model_dt, d1, sl_kehu.Rows[i][Model_dt.Rows[d1]["u1"].ToString()].ToString());
                                        }
                                    }

                                }
                                sql = sql + ")";
                                my_c.genxin(sql);
                                //Response.Write("----------------"+sql + "<br>");
                                //Response.Flush();
                                zongshu++;
                                chenggong++;

                                //插入end
                                #endregion
                            }
                          
                         
                          
                        }
                    }
                }
                   
                #endregion
                string info = "ok";
                if (chenggong == 0)
                {
                    info = "fail";
                }
                Response.Write(callback + "({\"info\":\""+ info + "\",\"zongshu\":\"" + zongshu + "\",\"zhongfu\":\"" + zhongfu + "\",\"wuxiao\":\"" + wuxiao + "\",\"chenggong\":\"" + chenggong + "\",\"yifugai\":\"" + yifugai + "\",\"bianhao\":\"" + bianhao + "\"})");
                Response.End();
                Response.Write("<script>window.location='err.aspx?count_s=2&err=导入数据成功！&errurl=dao_xls.aspx'</script>");
                #endregion
            }



            //GridView1.DataSource = Exceltable(Server.MapPath("/aa.xlsx"));
            //GridView1.DataBind();


        }
    }
}