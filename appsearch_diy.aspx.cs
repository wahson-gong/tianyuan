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
using Newtonsoft.Json;
using LitJson;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
public partial class appsearch_diy : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_hanshu my_hs = new my_hanshu();
    my_html my_h = new my_html();
    my_json my_js = new my_json();
    jiami jm = new jiami();
    #region 获取此栏目下所有ID
    string list_id = "";
    public void set_list_id(string tablename, string t1)
    {
        DataTable dt1 = my_c.GetTable("select id from " + tablename + " where classid in (" + t1 + ") ");

        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (list_id == "")
                {
                    list_id = dt1.Rows[j]["id"].ToString();
                }
                else
                {
                    list_id = list_id + "," + dt1.Rows[j]["id"].ToString();
                }

                set_list_id(tablename, dt1.Rows[j]["id"].ToString());
            }
        }

    }
    #endregion
    #region 列出组织最顶级目录
    string get_ding_dir_str = "";
    public string Parameterweizhi(string classid)
    {
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou where id=" + classid + " and classid>=0");
        if (dt1.Rows.Count > 0)
        {
            if (get_ding_dir_str == "")
            {
                get_ding_dir_str = "{\"name\":\"" + dt1.Rows[0]["jigoumingcheng"].ToString() + "\",\"id\":\"" + dt1.Rows[0]["id"].ToString() + "\"}";
            }
            else
            {
                get_ding_dir_str = "{\"name\":\"" + dt1.Rows[0]["jigoumingcheng"].ToString() + "\",\"id\":\"" + dt1.Rows[0]["id"].ToString() + "\"}," + get_ding_dir_str;
            }
            if (dt1.Rows[0]["classid"].ToString() != "0")
            {
                Parameterweizhi(dt1.Rows[0]["classid"].ToString());
            }
        }

        return get_ding_dir_str.ToString();
    }
    #endregion
    #region API接口验证
    public void set_api(string type)
    {
        if (type != "login")
        {
            string sign = "";
            try
            {
                sign = my_b.c_string(Request["sign"].ToString());
            }
            catch { }
            if (sign != "")
            {
                string timestamp = my_b.c_string(Request["timestamp"].ToString());
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");

                string appid = my_c.GetTable("select top 1 * from sl_xitongpeizhi").Rows[0]["appid"].ToString();
                string _sign = my_api.SampleCode.test_search(qu_str, timestamp, appid);
                HttpContext.Current.Response.Write(_sign);
                HttpContext.Current.Response.End();
                if (sign == _sign)
                {
                    //tiaozhuan("1", "", "");
                }
                else
                {
                    string callback = Request.QueryString["jsoncallback"];
                    string result = callback + "({\"date\":\"0\",\"msg\":\"签名不正确\"})";

                    Response.Clear();
                    Response.Write(result);
                    Response.End();
                }
            }

        }

    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string Print = "";
            try
            {
                Print = Request.QueryString["Print"].ToString();
            }
            catch { }
            string callback = Request.QueryString["jsoncallback"];
            string type = Request.QueryString["type"];
            #region API接口验证
            set_api(type);
            #endregion
            #region 自定义的查询
            if (type == "search")
            {
                #region 基础查询
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "";
                string[] table_name_ = my_b.set_url_css(Request.QueryString["t"].ToString()).Split(',');
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (table_name == "")
                    {
                        table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                    }
                    else
                    {
                        table_name = table_name + "," + ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                    }
                }

                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");

                string quer_sql = my_h.get_quer_sql(qu_str, table_name);
                #region 记录删除状态
                string[] cc = table_name.Split(',');
                string biaoming = "";
                for (int i = 0; i < cc.Length; i++)
                {
                    if (my_c.GetTable("select id from sl_field where u1='shanchuzhuangtai' and Model_id in (select id from sl_Model where u1='" + cc[i] + "')").Rows.Count > 0)
                    {
                        if (quer_sql == "")
                        {
                            quer_sql = "where (" + cc[i] + ".shanchuzhuangtai>-1)";
                        }
                        else
                        {
                            quer_sql = quer_sql + " and (" + cc[i] + ".shanchuzhuangtai>-1)";
                        }
                    }

                }

                #endregion
                string sql = "";
                string ordertype = "id";
                string page = "1";
                string liemingcheng = "";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch
                {
                    liemingcheng = table_name.Split(',')[0].ToString() + ".id";
                    #region 默认字段
                    string Model_id = "";
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                        string table_name1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable sl_Model = my_c.GetTable("select * from sl_Model where u1='" + table_name1 + "'");
                        if (Model_id == "")
                        {
                            Model_id = sl_Model.Rows[0]["id"].ToString();
                        }
                        else
                        {
                            Model_id = Model_id + "," + sl_Model.Rows[0]["id"].ToString();
                        }
                    }
                    DataTable Field = new DataTable();
                    Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (" + Model_id + ") and  U5='是'   order by u9,id");
                    for (int i1 = 0; i1 < Field.Rows.Count; i1++)
                    {
                        liemingcheng = liemingcheng + "," + Field.Rows[i1]["u1"].ToString();
                    }
                    #endregion
                }
                liemingcheng = liemingcheng.Replace("20%", " ");
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
                try
                {
                    page = my_b.c_string(Request.QueryString["page"].ToString());
                }
                catch { }
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }
                if (liemingcheng.IndexOf("count(") > -1 || liemingcheng.IndexOf("sum(") > -1 || liemingcheng.IndexOf("avg(") > -1)
                {
                    if (liemingcheng.IndexOf("sum(") > -1)
                    {
                        Regex reg1 = new Regex(" ", RegexOptions.Singleline);
                        string[] aa = reg1.Split(liemingcheng);
                        try
                        {
                            liemingcheng = "convert(decimal(38, 0)," + aa[0] + ") as " + aa[2] + "";
                        }
                        catch { }

                    }
                    ordertype = "";
                    orderby = "";
                    number = "";
                }

                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }
                string sql1 = "";

                if (number == "")
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    //Response.Write(table_name);
                    //Response.End();
                    sql1 = "select count(" + table_name.Split(',')[0].ToString() + ".id) as count_id from " + table_name + " " + quer_sql;
                    if (page == "1")
                    {
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                    }
                    else
                    {
                        string quer_sql1 = quer_sql.Substring(0, 5);
                        if (quer_sql1 == "where")
                        {
                            quer_sql1 = quer_sql.Substring(5, quer_sql.Length - 5);
                            quer_sql1 = " and " + quer_sql1;
                        }
                        int d_page = int.Parse(number) * (int.Parse(page) - 1);
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " where " + table_name.Split(',')[0].ToString() + ".id not in (select top " + d_page + " " + table_name.Split(',')[0].ToString() + ".id" + " from " + table_name + " " + quer_sql + order_str + ") " + quer_sql1 + order_str;
                    }

                }
                #region 关键词权限
                string Field_biaoming = "";
                for (int i = 0; i < cc.Length; i++)
                {
                    if (Field_biaoming == "")
                    {
                        Field_biaoming = "u1='" + cc[i] + "'";
                    }
                    else
                    {
                        Field_biaoming = Field_biaoming + " or u1='" + cc[i] + "'";
                    }

                    if (biaoming == "")
                    {
                        biaoming = "sl_guanjianciquanxian.biaoming='" + cc[i] + "'";
                    }
                    else
                    {
                        biaoming = biaoming + " or sl_guanjianciquanxian.biaoming='" + cc[i] + "'";
                    }
                }
                DataTable sl_Field = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");

                DataTable Field_dt = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.Write("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.End();
                #endregion

                if (Print == "yes")
                {
                    Response.Write(sql);
                    Response.End();
                }

                string result = my_js.oneJson(sql, callback);

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "lianxuhujiao")
            {
                string table_name = "sl_kehu";
                string[] table_name_ = table_name.Split(',');
                #region 连续呼叫
                string liemingcheng = "";

                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch
                {
                    liemingcheng = table_name.Split(',')[0].ToString() + ".id";
                    #region 默认字段
                    string Model_id = "";
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                        string table_name1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable sl_Model = my_c.GetTable("select * from sl_Model where u1='" + table_name1 + "'");
                        if (Model_id == "")
                        {
                            Model_id = sl_Model.Rows[0]["id"].ToString();
                        }
                        else
                        {
                            Model_id = Model_id + "," + sl_Model.Rows[0]["id"].ToString();
                        }
                    }
                    DataTable Field = new DataTable();
                    Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (" + Model_id + ") and  U5='是'   order by u9,id");
                    for (int i1 = 0; i1 < Field.Rows.Count; i1++)
                    {
                        liemingcheng = liemingcheng + "," + Field.Rows[i1]["u1"].ToString();
                    }
                    #endregion
                }
                liemingcheng = liemingcheng.Replace("20%", " ");
                string id = my_b.c_string(Request["id"].ToString());
                string yonghuming = my_b.c_string(Request["yonghuming"].ToString());
                string leixing = my_b.c_string(Request["leixing"].ToString());
                string sql = "";
                string sql1 = "";
                if (id == "")
                {
                    //sql = "select " + liemingcheng + " from sl_kehu where yonghuming in (" + yonghuming + ") and leixing='" + leixing + "' and datediff(second, xiacihujiaoshijian ,getdate()) >=1 or (shifoulianxi<>'是' and yonghuming in (" + yonghuming + ") and leixing='" + leixing + "' )  ";
                    sql = "select " + liemingcheng + " from sl_kehu where yonghuming in (" + yonghuming + ") and leixing='" + leixing + "' and datediff(second, xiacihujiaoshijian ,getdate()) >=1 and shanchuzhuangtai>=0 ";


                }
                else
                {
                    //sql = "select " + liemingcheng + " from sl_kehu where id in (" + id + ") and leixing='" + leixing + "' and datediff(second, xiacihujiaoshijian ,getdate()) >=1  or (shifoulianxi<>'是' and yonghuming in (" + yonghuming + ") and leixing='" + leixing + "' )   ";
                    sql = "select " + liemingcheng + " from sl_kehu where id in (" + id + ") and leixing='" + leixing + "' and datediff(second, xiacihujiaoshijian ,getdate()) >=1  and shanchuzhuangtai>=0 ";
                }
                #region 关键词权限
                string Field_biaoming = "";
                string biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (Field_biaoming == "")
                    {
                        Field_biaoming = "u1='" + table_name_[i] + "'";
                    }
                    else
                    {
                        Field_biaoming = Field_biaoming + " or u1='" + table_name_[i] + "'";
                    }

                    if (biaoming == "")
                    {
                        biaoming = "sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                    else
                    {
                        biaoming = biaoming + " or sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                }
                DataTable sl_Field = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");


                DataTable Field_dt = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.Write("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.End();
                #endregion
                //Response.Write(sql);
                //Response.End();
                string result = my_js.OutJson(sql, callback, "", sl_Field, Field_dt, "");
                //result.Replace("\r", "");
                result= result.Replace("\\", "");
                Response.Clear();
                Response.Write(result);
                Response.End();


                #endregion
            }
            else if (type == "renshu")
            {
                #region 处理人数
                DataTable dt1 = my_c.GetTable("select jigoumingcheng,id,classid from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou");
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    list_id = "";
                    set_list_id(ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou", dt1.Rows[i]["id"].ToString());
                    //Response.Write(list_id);
                    //Response.End();
                    string suoshubumen = list_id;
                    if (suoshubumen == "")
                    {
                        suoshubumen = dt1.Rows[i]["id"].ToString();
                    }
                    DataTable sl_yuangong = my_c.GetTable("select count(id) as count_id from sl_yuangong where suoshubumen in (" + suoshubumen + ")");
                    my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou set renshu=" + sl_yuangong.Rows[0]["count_id"].ToString() + "");
                }

                Response.End();
                #endregion
            }
            else if (type == "currentzuzhilist")
            {
                #region 当前组织架构
                string id = Request.QueryString["id"];
                string data = Parameterweizhi(id);
                Response.Write(callback + "([" + data + "])");
                Response.End();
                #endregion
            }
            else if (type == "getkuhu")
            {
                #region 当前账户的客户
                string yhm = Request.QueryString["yhm"];
                string data = "";
                DataTable sl_yuangong = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "yuangong where yonghuming=" + yhm);
                if (sl_yuangong.Rows.Count > 0)
                {

                    DataTable sl_jigou = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou where id=" + sl_yuangong.Rows[0]["suoshubumen"].ToString());
                    if (sl_jigou.Rows.Count > 0)
                    {
                        if (yhm == sl_jigou.Rows[0]["fuzeren"].ToString())
                        {
                            #region 如果是负责人

                            #endregion
                        }
                        else
                        {
                            #region 不是负责人
                            DataTable sl_kehu = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "kehu where fenpeiduixiang='" + yhm + "'");
                            if (sl_kehu.Rows.Count > 0)
                            {
                                for (int i = 0; i < sl_kehu.Rows.Count; i++)
                                {
                                    if (data == "")
                                    {
                                        data = "{\"xingming\":\"" + sl_kehu.Rows[i]["xingming"].ToString() + "\",\"dianhua\":\"" + sl_kehu.Rows[i]["dianhua"].ToString() + "\",\"zhuangtai\":\"" + sl_kehu.Rows[i]["zhuangtai"] + "\",\"qudaolaiyuan\":\"" + sl_kehu.Rows[i]["qudaolaiyuan"] + "\",\"fenpeishijian\":\"" + sl_kehu.Rows[i]["fenpeishijian"] + "\",\"zhuangtai\":\"" + sl_kehu.Rows[i]["zhuangtai"] + "\",";
                                    }
                                }
                            }
                            else
                            {
                                data = "no_kehu";
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        data = "no_jigou";
                    }
                }
                else
                {
                    data = "no_user";
                }
                Response.Write(callback + "([" + data + "])");
                Response.End();
                #endregion
            }
            else if (type == "getmodel")
            {
                #region 获取模型表
                string data = "";
                DataTable sl_model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where xianshi='是'");
                if (sl_model.Rows.Count > 0)
                {
                    for (int i = 0; i < sl_model.Rows.Count; i++)
                    {
                        string ziduan = "{\"u1\":\"\",\"u2\":\"选择字段\",\"id\":\"0\"}";
                        DataTable sl_field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where daoru='是' and Model_id=" + sl_model.Rows[i]["id"].ToString());
                        if (sl_field.Rows.Count > 0)
                        {
                            for (int j = 0; j < sl_field.Rows.Count; j++)
                            {
                                if (ziduan == "")
                                {
                                    ziduan = "{\"u1\":\"" + sl_field.Rows[j]["u1"].ToString() + "\",\"u2\":\"" + sl_field.Rows[j]["u2"].ToString() + "\",\"id\":\"" + sl_field.Rows[j]["id"].ToString() + "\"}";
                                }
                                else
                                {
                                    ziduan = ziduan + "," + "{\"u1\":\"" + sl_field.Rows[j]["u1"].ToString() + "\",\"u2\":\"" + sl_field.Rows[j]["u2"].ToString() + "\",\"id\":\"" + sl_field.Rows[j]["id"].ToString() + "\"}";
                                }
                            }
                        }
                        if (data == "")
                        {
                            data = "{\"u1\":\"" + sl_model.Rows[i]["u1"].ToString() + "\",\"u2\":\"" + sl_model.Rows[i]["u2"].ToString() + "\",\"id\":\"" + sl_model.Rows[i]["id"].ToString() + "\",\"ziduan\":[" + ziduan + "]}";
                        }
                        else
                        {
                            data = data + "," + "{\"u1\":\"" + sl_model.Rows[i]["u1"].ToString() + "\",\"u2\":\"" + sl_model.Rows[i]["u2"].ToString() + "\",\"id\":\"" + sl_model.Rows[i]["id"].ToString() + "\",\"ziduan\":[" + ziduan + "]}";
                        }
                    }
                }
                Response.Write(callback + "([" + data + "])");
                Response.End();
                #endregion
            }
            else if (type == "getfenzu")
            {
                #region 获取客户分组
                string yhm = Request.QueryString["yhm"];
                string data = "";
                DataTable fenzuall = my_c.GetTable("select *,(select count(id) from sl_kehu as kh where kh.fenzu=cs.biaoti and kh.yonghuming=" + yhm + ") as shu from sl_canshu as cs where cs.classid=106");
                if (fenzuall.Rows.Count > 0)
                {
                    for (int i = 0; i < fenzuall.Rows.Count; i++)
                    {
                        if (data == "")
                        {
                            data = "{\"zuming\":\"" + fenzuall.Rows[i]["biaoti"].ToString() + "\",\"renshu\":\"" + fenzuall.Rows[i]["shu"].ToString() + "\"}";
                        }
                        else
                        {
                            data = data + ",{\"zuming\":\"" + fenzuall.Rows[i]["biaoti"].ToString() + "\",\"renshu\":\"" + fenzuall.Rows[i]["shu"].ToString() + "\"}";
                        }
                    }
                }
                DataTable fenzuyonghu = my_c.GetTable("select *,(select count(id) from sl_kehu as kh where kh.fenzu=khfz.zuming and kh.yonghuming=" + yhm + ") as shu from sl_kehufenzu as khfz where khfz.yonghuming=" + yhm);
                if (fenzuyonghu.Rows.Count > 0)
                {
                    for (int i = 0; i < fenzuyonghu.Rows.Count; i++)
                    {
                        if (data == "")
                        {
                            data = "{\"zuming\":\"" + fenzuyonghu.Rows[i]["zuming"].ToString() + "\",\"renshu\":\"" + fenzuyonghu.Rows[i]["shu"].ToString() + "\"}";
                        }
                        else
                        {
                            data = data + ",{\"zuming\":\"" + fenzuyonghu.Rows[i]["zuming"].ToString() + "\",\"renshu\":\"" + fenzuyonghu.Rows[i]["shu"].ToString() + "\"}";
                        }
                    }
                }
                Response.Write(callback + "([" + data + "])");
                Response.End();
                #endregion
            }
            else if (type == "getqudao")
            {
                #region 获取渠道类型
                string data = "";
                string suoshuleibie = Request.QueryString["suoshuleibie"];
                DataTable qudaoone = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "qudaoleixing where suoshuleibie='" + suoshuleibie + "'");
                if (qudaoone.Rows.Count > 0)
                {
                    for (int i = 0; i < qudaoone.Rows.Count; i++)
                    {
                        DataTable qudaoer = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "qudao where ziyuanleixing='" + qudaoone.Rows[i]["id"].ToString() + "'");
                        string child = "";
                        if (qudaoer.Rows.Count > 0)
                        {
                            for (int j = 0; j < qudaoer.Rows.Count; j++)
                            {
                                if (child == "")
                                {
                                    child = "{\"value\":\"" + qudaoer.Rows[j]["id"].ToString() + "\",\"label\":\"" + qudaoer.Rows[j]["ziyuanmingcheng"].ToString() + "\"}";
                                }
                                else
                                {
                                    child = child + ",{\"value\":\"" + qudaoer.Rows[j]["id"].ToString() + "\",\"label\":\"" + qudaoer.Rows[j]["ziyuanmingcheng"].ToString() + "\"}";
                                }
                            }
                        }
                        if (data == "")
                        {
                            if (child == "")
                            {
                                data = "{\"value\":\"" + qudaoone.Rows[i]["id"].ToString() + "\",\"label\":\"" + qudaoone.Rows[i]["qudaoleixing"].ToString() + "\"}";
                            }
                            else
                            {
                                data = "{\"value\":\"" + qudaoone.Rows[i]["id"].ToString() + "\",\"label\":\"" + qudaoone.Rows[i]["qudaoleixing"].ToString() + "\",\"children\":[" + child + "]}";
                            }
                        }
                        else
                        {
                            if (child == "")
                            {
                                data = data + ",{\"value\":\"" + qudaoone.Rows[i]["id"].ToString() + "\",\"label\":\"" + qudaoone.Rows[i]["qudaoleixing"].ToString() + "\"}";
                            }
                            else
                            {
                                data = data + ",{\"value\":\"" + qudaoone.Rows[i]["id"].ToString() + "\",\"label\":\"" + qudaoone.Rows[i]["qudaoleixing"].ToString() + "\",\"children\":[" + child + "]}";
                            }
                        }
                    }
                }
                Response.Write(callback + "([" + data + "])");
                Response.End();
                #endregion
            }
            else if (type == "jiemi")
            {
                #region 解密信息
                string date = Request.QueryString["date"];

                date = jm.Decrypt(date);
                string result = callback + "({\"date\":\"" + date + "\"})";
                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "getcaidanquanxian")
            {
                #region 获取菜单权限
                string id = Request.QueryString["id"];
                DataTable zhiwei = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "zhiwei where id=" + id);
                string shujuxingquanxian = zhiwei.Rows[0]["shujuxingquanxian"].ToString();
                string anniuquanxian = zhiwei.Rows[0]["anniuquanxian"].ToString();
                if (zhiwei.Rows.Count > 0)
                {
                    DataTable caidanone = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "caidanguanli where classid=0 order by paixu desc");
                    string data = "";
                    if (caidanone.Rows.Count > 0)
                    {
                        for (int i = 0; i < caidanone.Rows.Count; i++)
                        {
                            DataTable caidaner = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "caidanguanli where classid=" + caidanone.Rows[i]["id"].ToString() + " order by paixu desc");
                            string erjicaidan = "";
                            if (caidaner.Rows.Count > 0)
                            {
                                for (int j = 0; j < caidaner.Rows.Count; j++)
                                {
                                    DataTable anniutable = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "caidanguanli where classid=" + caidaner.Rows[j]["id"].ToString() + " order by paixu desc");
                                    string buttonlist = "";
                                    if (anniutable.Rows.Count > 0)
                                    {
                                        for (int m = 0; m < anniutable.Rows.Count; m++)
                                        {
                                            if (buttonlist == "")
                                            {
                                                buttonlist = "{\"button\":\"" + anniutable.Rows[m]["biaoti"].ToString() + "\",\"id\":\"" + anniutable.Rows[m]["id"].ToString() + "\"}";
                                            }
                                            else
                                            {
                                                buttonlist = buttonlist + ",{\"button\":\"" + anniutable.Rows[m]["biaoti"].ToString() + "\",\"id\":\"" + anniutable.Rows[m]["id"].ToString() + "\"}";
                                            }
                                        }
                                    }
                                    bool xianshi = false;
                                    if (zhiwei.Rows[0]["caidanquanxian"].ToString() != "")
                                    {
                                        DataTable caidanxianshi = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "caidanguanli where classid=" + caidanone.Rows[i]["id"].ToString() + " and id in (" + zhiwei.Rows[0]["caidanquanxian"].ToString() + ") order by paixu desc");
                                        if (caidanxianshi.Rows.Count > 0)
                                        {
                                            for (int k = 0; k < caidanxianshi.Rows.Count; k++)
                                            {
                                                if (caidanxianshi.Rows[k]["id"].ToString() == caidaner.Rows[j]["id"].ToString())
                                                {
                                                    xianshi = true;
                                                }
                                            }
                                        }
                                    }
                                    string quanxian = "仅本人";
                                    if (shujuxingquanxian != "")
                                    {
                                        string[] quanxianlist = Regex.Split(shujuxingquanxian, "{next}", RegexOptions.IgnoreCase);
                                        for (int a = 0; a < quanxianlist.Length; a++)
                                        {
                                            if (quanxianlist[a] != "")
                                            {
                                                string[] quanxianeach = Regex.Split(quanxianlist[a], "{and}", RegexOptions.IgnoreCase);
                                                if (quanxianeach[1] == caidaner.Rows[j]["id"].ToString())
                                                {
                                                    quanxian = quanxianeach[0];
                                                }
                                            }
                                        }
                                    }
                                    string anniu = "";
                                    if (anniuquanxian != "")
                                    {
                                        string[] anniulist = Regex.Split(anniuquanxian, "{next}", RegexOptions.IgnoreCase);
                                        for (int a = 0; a < anniulist.Length; a++)
                                        {
                                            if (anniulist[a] != "")
                                            {
                                                string[] anniueach = Regex.Split(anniulist[a], "{and}", RegexOptions.IgnoreCase);
                                                if (anniueach[1] == caidaner.Rows[j]["id"].ToString())
                                                {
                                                    anniu = anniueach[0];
                                                }
                                            }
                                        }
                                    }
                                    if (erjicaidan == "")
                                    {
                                        erjicaidan = "{\"biaoti\":\"" + caidaner.Rows[j]["biaoti"].ToString() + "\",\"id\":\"" + caidaner.Rows[j]["id"].ToString() + "\",\"xianshi\":\"" + xianshi + "\",\"quanxian\":\"" + quanxian + "\",\"button\":[" + anniu + "],\"buttonlist\":[" + buttonlist + "]}";
                                    }
                                    else
                                    {
                                        erjicaidan = erjicaidan + ",{\"biaoti\":\"" + caidaner.Rows[j]["biaoti"].ToString() + "\",\"id\":\"" + caidaner.Rows[j]["id"].ToString() + "\",\"xianshi\":\"" + xianshi + "\",\"quanxian\":\"" + quanxian + "\",\"button\":[" + anniu + "],\"buttonlist\":[" + buttonlist + "]}";
                                    }
                                }
                            }
                            if (data == "")
                            {
                                data = "{\"biaoti\":\"" + caidanone.Rows[i]["biaoti"].ToString() + "\",\"id\":\"" + caidanone.Rows[i]["id"] + "\",\"children\":[" + erjicaidan + "]}";
                            }
                            else
                            {
                                data = data + ",{\"biaoti\":\"" + caidanone.Rows[i]["biaoti"].ToString() + "\",\"id\":\"" + caidanone.Rows[i]["id"] + "\",\"children\":[" + erjicaidan + "]}";
                            }
                        }
                    }
                    Response.Write(callback + "([" + data + "])");
                    Response.End();
                }
                #endregion
            }
            else if (type == "getziduanquanxian")
            {
                #region 获取字段权限
                string id = Request.QueryString["id"];
                DataTable zhiwei = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "zhiwei where id=" + id);
                DataTable model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where xianshi='是'");
                string data = "";
                if (model.Rows.Count > 0)
                {
                    for (int i = 0; i < model.Rows.Count; i++)
                    {
                        DataTable field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + model.Rows[i]["id"].ToString());
                        string ziduan = "";
                        if (field.Rows.Count > 0)
                        {
                            for (int j = 0; j < field.Rows.Count; j++)
                            {
                                DataTable ziduanquanxian = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guanjianciquanxian where laiyuanbianhao=" + zhiwei.Rows[0]["id"].ToString() + " and ziduanid=" + field.Rows[j]["id"].ToString());
                                string xiugai = "add";
                                string xiugaiid = "";
                                string edit = "true";
                                string show = "true";
                                if (ziduanquanxian.Rows.Count > 0)
                                {
                                    xiugai = "editid";
                                    xiugaiid = ziduanquanxian.Rows[0]["id"].ToString();
                                    if (ziduanquanxian.Rows[0]["kejian"].ToString() == "false")
                                    {
                                        show = "false";
                                    }
                                    if (ziduanquanxian.Rows[0]["bianji"].ToString() == "false")
                                    {
                                        edit = "false";
                                    }
                                }
                                if (ziduan == "")
                                {
                                    ziduan = "{\"id\":\"" + field.Rows[j]["id"].ToString() + "\",\"ziduanming\":\"" + field.Rows[j]["u2"].ToString() + "\",\"type\":\"" + xiugai + "\",\"typeid\":\"" + xiugaiid + "\",\"edit\":\"" + edit + "\",\"show\":\"" + show + "\",\"origin_edit\":\"" + edit + "\",\"origin_show\":\"" + show + "\"}";
                                }
                                else
                                {
                                    ziduan = ziduan + ",{\"id\":\"" + field.Rows[j]["id"].ToString() + "\",\"ziduanming\":\"" + field.Rows[j]["u2"].ToString() + "\",\"type\":\"" + xiugai + "\",\"typeid\":\"" + xiugaiid + "\",\"edit\":\"" + edit + "\",\"show\":\"" + show + "\",\"origin_edit\":\"" + edit + "\",\"origin_show\":\"" + show + "\"}";
                                }
                            }
                        }
                        if (data == "")
                        {
                            data = "{\"id\":\"" + model.Rows[i]["id"].ToString() + "\",\"biaoming\":\"" + model.Rows[i]["u2"].ToString() + "\",\"children\":[" + ziduan + "]}";
                        }
                        else
                        {
                            data = data + ",{\"id\":\"" + model.Rows[i]["id"].ToString() + "\",\"biaoming\":\"" + model.Rows[i]["u2"].ToString() + "\",\"children\":[" + ziduan + "]}";
                        }
                    }
                }
                Response.Write(callback + "([" + data + "])");
                Response.End();
                #endregion
            }
            else if (type == "anniuquanxian")
            {
                #region 按钮权限 
                StringBuilder str = new StringBuilder();
                string pageid = "";
                try
                {
                    pageid = my_b.c_string(HttpContext.Current.Request.QueryString["pageid"].ToString());
                }
                catch { }
                string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());
                DataTable sl_yuangong = my_c.GetTable("select * from sl_yuangong where id in (" + yonghuming + ")");

                for (int i = 0; i < sl_yuangong.Rows.Count; i++)
                {
                    string suoshuzhiwei = sl_yuangong.Rows[i]["suoshuzhiwei"].ToString();
                    string suoshubumen = sl_yuangong.Rows[i]["suoshubumen"].ToString();
                    DataTable sl_zhiwei = my_c.GetTable("select * from sl_zhiwei where id=" + suoshuzhiwei);
                    Regex reg = new Regex("{next}", RegexOptions.Singleline);
                    if (sl_zhiwei.Rows[0]["anniuquanxian"].ToString() == "")
                    {
                        string result = callback + "({\"date\":\"null\"})";
                        Response.Clear();
                        Response.Write(result);
                        Response.End();
                    }
                    string[] anniuquanxian = reg.Split(sl_zhiwei.Rows[0]["anniuquanxian"].ToString());
                    // HttpContext.Current.Response.Write(sl_zhiwei.Rows[0]["shujuxingquanxian"].ToString() + "<br>");
                    for (int j = 0; j < anniuquanxian.Length; j++)
                    {
                        #region 某一行
                        Regex reg1 = new Regex("{and}", RegexOptions.Singleline);
                        string[] yihang = reg1.Split(anniuquanxian[j].ToString());
                        // HttpContext.Current.Response.Write(shujuxingquanxian[i].ToString()+"<br>");
                        //   HttpContext.Current.Response.End();
                        if (yihang[1] == pageid)
                        {
                            string[] anniu = yihang[0].Split(',');
                            for (int j1 = 0; j1 < anniu.Length; j1++)
                            {
                                //Response.Write(yihang[0]);
                                //Response.End();
                                string result = callback + "({\"date\":\"ok\",\"anniu\":[" + yihang[0] + "]})";
                                Response.Clear();
                                Response.Write(result);
                                Response.End();
                            }
                        }
                        #endregion
                    }
                }
                #endregion
            }
            else if (type == "order_list")
            {
                #region 签单列表页
                //string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());
                //yonghuming = my_hs.get_yonghuming(yonghuming, "yonghuming");
                //DataTable sl_qiandan =  my_c.GetTable("select * from sl_qiandan where  (" + yonghuming + ")");
                //StringBuilder str = new StringBuilder();
                //for (int i = 0; i < sl_qiandan.Rows.Count; i++)
                //{
                //    DataTable sl_zuodan = my_c.GetTable("select * from sl_zuodan where qiandan in (" + sl_qiandan.Rows[i]["id"].ToString() + ")");
                //    for (int j = 0; j < sl_zuodan.Rows.Count; j++)
                //    {

                //    }
                //}
                string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());
                string id = "";
                try
                {
                    id = my_b.c_string(Request.QueryString["id"].ToString());
                }
                catch { }
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "sl_qiandan";
                string[] table_name_ = table_name.Split(',');
                yonghuming = my_hs.get_yonghuming(yonghuming, "yonghuming");
                //Response.Write(yonghuming);
                //Response.End();
                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");
                qu_str = my_hs.qucanshu(qu_str, "yonghuming");
                //Response.Write(qu_str);
                //Response.End();
                qu_str = my_h.get_quer_sql(qu_str, table_name);
          
                string quer_sql = "";
                if (id == "")
                {
                    if (yonghuming != "")
                    {
                        quer_sql = " (" + yonghuming + ")";

                    }
                }
                else
                {
                    quer_sql = " (id in (" + id + "))";
                }
                string quer_diy_sql = "";
                #region 读审核表数据
                if (quer_diy_sql == "")
                {
                    if (yonghuming == "")
                    {
                        // quer_sql = "where (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and xianshi='是'))";
                    }
                    else
                    {
                        quer_diy_sql = " (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and xianshi='是' and (" + yonghuming + ")))";
                    }

                }
                else
                {
                    if (yonghuming == "")
                    {
                        //quer_sql = quer_sql + " or (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and  xianshi='是'))";
                    }
                    else
                    {
                        quer_diy_sql = quer_diy_sql + " or (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and  xianshi='是' and (" + yonghuming + ")))";
                    }

                }
                #endregion
                #region 读做单表数据
                if (quer_diy_sql == "")
                {
                    if (yonghuming == "")
                    {
                        //  quer_sql = "where (id in (select qiandan from sl_zuodan where  shanchuzhuangtai>-1))";
                    }
                    else
                    {
                        quer_diy_sql = " (id in (select qiandan from sl_zuodan where  shanchuzhuangtai>-1 and (" + yonghuming + ")))";
                    }

                }
                else
                {
                    if (yonghuming == "")
                    {
                        // quer_sql = quer_sql + " or (id in (select qiandan from sl_zuodan where shanchuzhuangtai>-1 ))";
                    }
                    else
                    {
                        quer_diy_sql = quer_diy_sql + " or (id in (select qiandan from sl_zuodan where shanchuzhuangtai>-1 and (" + yonghuming + ")))";
                    }

                }
                #endregion
         
                #region 记录删除状态

                string biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (qu_str != "")
                    {
                        qu_str = "("+qu_str.Substring(5, qu_str.Length-5) +") and ";
                    }
        
                    if (my_c.GetTable("select id from sl_field where u1='shanchuzhuangtai' and Model_id in (select id from sl_Model where u1='" + table_name_[i] + "')").Rows.Count > 0)
                    {
                        if (quer_sql == "")
                        {
                            if (quer_diy_sql == "")
                            {
                                quer_sql = "where "+ qu_str + "(" + table_name_[i] + ".shanchuzhuangtai>-1)";
                               
                             
                            }
                            else
                            {
                                quer_sql = "where " + qu_str + "(" + quer_diy_sql + ") and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                            }
                        }
                        else
                        {
                            if (quer_diy_sql == "")
                            {
                                quer_sql = " where " + qu_str + "" + quer_sql + " and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                            }
                            else
                            {
                                quer_sql = " where " + qu_str + "(" + quer_sql + " or " + quer_diy_sql + ") and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                            }

                        }
                    }

                }

                #endregion
                //Response.Write(quer_sql);
                //Response.End();
                string sql = "";
                string ordertype = "id";
                string page = "1";
                #region 拼接字段
                string liemingcheng = "";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch
                {
                    liemingcheng = table_name.Split(',')[0].ToString() + ".id";
                    #region 默认字段
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                        string table_name1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable Field = new DataTable();
                        Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from sl_Model where u1='" + table_name1 + "') and  U5='是'   order by u9,id");
                        for (int i2 = 0; i2 < Field.Rows.Count; i2++)
                        {
                            liemingcheng = liemingcheng + "," + table_name1 + "." + Field.Rows[i2]["u1"].ToString();
                        }
                    }

                    #endregion
                }
                liemingcheng = liemingcheng.Replace("20%", " ");
                #endregion
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
                try
                {
                    page = my_b.c_string(Request.QueryString["page"].ToString());
                }
                catch { }
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }


                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }
                string sql1 = "";

                if (number == "")
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    //Response.Write(table_name);
                    //Response.End();
                    sql1 = "select count(" + table_name.Split(',')[0].ToString() + ".id) as count_id from " + table_name + " " + quer_sql;
                    if (page == "1")
                    {
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                    }
                    else
                    {
                        int d_page = int.Parse(number) * (int.Parse(page) - 1);
                        // sql = "select top " + number + " " + liemingcheng + " from " + table_name + " where id not in (select top " + d_page + " " + table_name.Split(',')[0].ToString() + ".id" + " from " + table_name + " " + quer_sql + order_str + ") " + quer_sql.Replace("where", "and") + order_str;
                        quer_sql = quer_sql.Trim();
                        string quer_sql1 = quer_sql.Substring(0, 5);
                        if (quer_sql1 == "where")
                        {
                            quer_sql1 = quer_sql.Substring(5, quer_sql.Length - 5);
                            quer_sql1 = " and " + quer_sql1;
                        }
                        //Response.Write(quer_sql1);
                        //Response.End();
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " where id not in (select top " + d_page + " " + table_name.Split(',')[0].ToString() + ".id" + " from " + table_name + " " + quer_sql + order_str + ") " + quer_sql1 + order_str;
                    }

                }
                #region 关键词权限
                string Field_biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (Field_biaoming == "")
                    {
                        Field_biaoming = "u1='" + table_name_[i] + "'";
                    }
                    else
                    {
                        Field_biaoming = Field_biaoming + " or u1='" + table_name_[i] + "'";
                    }

                    if (biaoming == "")
                    {
                        biaoming = "sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                    else
                    {
                        biaoming = biaoming + " or sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                }
                DataTable sl_Field = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");

                DataTable Field_dt = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.Write("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.End();
                #endregion

                if (Print == "yes")
                {
                    Response.Write(sql);
                    Response.End();
                }
                string result = my_js.OutJson(sql, callback, sql1, sl_Field, Field_dt, "order_list");

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "zuodanjiesuan")
            {
                #region 做单结算列表页
                //string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());
                //yonghuming = my_hs.get_yonghuming(yonghuming, "yonghuming");
                //DataTable sl_qiandan =  my_c.GetTable("select * from sl_qiandan where  (" + yonghuming + ")");
                //StringBuilder str = new StringBuilder();
                //for (int i = 0; i < sl_qiandan.Rows.Count; i++)
                //{
                //    DataTable sl_zuodan = my_c.GetTable("select * from sl_zuodan where qiandan in (" + sl_qiandan.Rows[i]["id"].ToString() + ")");
                //    for (int j = 0; j < sl_zuodan.Rows.Count; j++)
                //    {

                //    }
                //}

                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "";
                string[] table_name_ = my_b.set_url_css(Request.QueryString["t"].ToString()).Split(',');
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (table_name == "")
                    {
                        table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                    }
                    else
                    {
                        table_name = table_name + "," + ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                    }
                }


                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");
                string quer_sql = my_h.get_quer_sql(qu_str, table_name);
            
                //Response.Write(quer_sql);
                //Response.End();
                #region 记录删除状态

                string biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (my_c.GetTable("select id from sl_field where u1='shanchuzhuangtai' and Model_id in (select id from sl_Model where u1='" + table_name_[i] + "')").Rows.Count > 0)
                    {
                        if (quer_sql == "")
                        {
                            quer_sql = "where (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                        }
                        else
                        {
                            quer_sql = quer_sql + " and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                        }
                    }

                }

                #endregion
                string sql = "";
                string ordertype = "id";
                string page = "1";
                if (int.Parse(page) == 1)
                {
                    #region 统计所有做单状态为申请放款
                    DataTable sl_zuodan = my_c.GetTable("select *  from sl_zuodan where (zhuangtai='确认放款' or zhuangtai='失败') and 	shanchuzhuangtai>-1");
                    for (int i2 = 0; i2 < sl_zuodan.Rows.Count; i2++)
                    {
                        DataTable sl_qiandan = my_c.GetTable("select * from sl_qiandan where id in (" + sl_zuodan.Rows[i2]["qiandan"].ToString() + ")");
                        string zuodanid = sl_zuodan.Rows[i2]["id"].ToString();
                        string kehu = sl_qiandan.Rows[0]["kehu"].ToString();
                        string chanpin = sl_qiandan.Rows[0]["daikuanchanpin"].ToString();
                        if (chanpin == "")
                        {
                            chanpin = "0";
                        }
                        string zhuangtai = "待结算";
                        float zongshouru = 0;
                        try
                        {
                            zongshouru = float.Parse(my_c.GetTable("select sum(jine) as jine from sl_zuodanshouzhi where zuodanid=" + zuodanid + " and jine>0 and zhuangtai='已支付' and shenhe='已通过'  and 	shanchuzhuangtai>-1").Rows[0]["jine"].ToString());
                        }
                        catch { }
                        float zongzhichu = 0;
                        try
                        {
                            zongzhichu = float.Parse(my_c.GetTable("select sum(jine) as jine from sl_zuodanshouzhi where zuodanid=" + zuodanid + " and jine<0 and zhuangtai='已支付' and shenhe='已通过'  and 	shanchuzhuangtai>-1").Rows[0]["jine"].ToString());
                        }
                        catch { }
                        float lirun = zongshouru + zongzhichu;
                        string qiandanshijian = sl_qiandan.Rows[0]["dtime"].ToString();

                        DataTable sl_zuodanjiesuan = my_c.GetTable("select * from sl_zuodanjiesuan where zuodanid=" + zuodanid + "");
                        string zuodanjiesuansql = "insert into sl_zuodanjiesuan(kehu,chanpin,zhuangtai,zongshouru,zongzhichu,lirun,qiandanshijian,zuodanid) values(" + kehu + "," + chanpin + ",'" + zhuangtai + "'," + zongshouru + "," + zongzhichu + "," + lirun + ",'" + qiandanshijian + "'," + zuodanid + ")";
                        if (sl_zuodanjiesuan.Rows.Count > 0)
                        {
                            if (sl_zuodanjiesuan.Rows[0]["zhuangtai"].ToString() != "完成")
                            {
                                my_c.genxin("delete from sl_zuodanjiesuan where id=" + sl_zuodanjiesuan.Rows[0]["id"].ToString());//删除状态不是完成的记录
                                my_c.genxin(zuodanjiesuansql);
                            }
                        }
                        else
                        {
                            my_c.genxin(zuodanjiesuansql);
                        }


                    }
                    #endregion
                }

                #region 拼接字段
                string liemingcheng = "";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch
                {
                    liemingcheng = table_name.Split(',')[0].ToString() + ".id";
                    #region 默认字段
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                        string table_name1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable Field = new DataTable();
                        Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from sl_Model where u1='" + table_name1 + "') and  U5='是'   order by u9,id");
                        for (int i2 = 0; i2 < Field.Rows.Count; i2++)
                        {
                            liemingcheng = liemingcheng + "," + table_name1 + "." + Field.Rows[i2]["u1"].ToString();
                        }
                    }

                    #endregion
                }
                liemingcheng = liemingcheng.Replace("20%", " ");
                #endregion
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
                try
                {
                    page = my_b.c_string(Request.QueryString["page"].ToString());
                }
                catch { }
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }


                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }
                string sql1 = "";

                if (number == "")
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    //Response.Write(table_name);
                    //Response.End();
                    sql1 = "select count(" + table_name.Split(',')[0].ToString() + ".id) as count_id from " + table_name + " " + quer_sql;
                    if (page == "1")
                    {
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                    }
                    else
                    {
                        int d_page = int.Parse(number) * (int.Parse(page) - 1);
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " where id not in (select top " + d_page + " " + table_name.Split(',')[0].ToString() + ".id" + " from " + table_name + " " + quer_sql + order_str + ") " + quer_sql.Replace("where", "and") + order_str;
                    }

                }
                #region 关键词权限
                string Field_biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (Field_biaoming == "")
                    {
                        Field_biaoming = "u1='" + table_name_[i] + "'";
                    }
                    else
                    {
                        Field_biaoming = Field_biaoming + " or u1='" + table_name_[i] + "'";
                    }

                    if (biaoming == "")
                    {
                        biaoming = "sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                    else
                    {
                        biaoming = biaoming + " or sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                }
                DataTable sl_Field = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");

                DataTable Field_dt = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.Write("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.End();
                #endregion

                //Response.Write(sql);
                //Response.End();
                string result = my_js.OutJson(sql, callback, sql1, sl_Field, Field_dt, "zuodanjiesuan");

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "qiandanjiesuan")
            {
                #region 签单结算列表页
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "sl_qiandan";
                string[] table_name_ = table_name.Split(',');

                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 

                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");

                string quer_sql = my_h.get_quer_sql(qu_str, table_name);
                //已退单、完成、结算中
                if (quer_sql == "")
                {
                    quer_sql = "where (zhuangtai='已退单' or zhuangtai='完成' or zhuangtai='结算中')";
                }
                //Response.Write(quer_sql);
                //Response.End();
                #region 记录删除状态

                string biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (my_c.GetTable("select id from sl_field where u1='shanchuzhuangtai' and Model_id in (select id from sl_Model where u1='" + table_name_[i] + "')").Rows.Count > 0)
                    {
                        if (quer_sql == "")
                        {
                            quer_sql = "where (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                        }
                        else
                        {
                            quer_sql = quer_sql + " and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                        }
                    }

                }

                #endregion
                string sql = "";
                string ordertype = "id";
                string page = "1";
                if (int.Parse(page) == 1)
                {
                    #region 统计所有签单状态为结算中、退单中
                    DataTable sl_qiandan = my_c.GetTable("select *  from sl_qiandan where (zhuangtai='结算中' or zhuangtai='退单中') and 	shanchuzhuangtai>-1");
                    for (int i2 = 0; i2 < sl_qiandan.Rows.Count; i2++)
                    {
                        float zongshouru = 0;
                        try
                        {
                            zongshouru = float.Parse(my_c.GetTable("select sum(jine) as jine from sl_zuodanshouzhi where zuodanid in (select id from sl_zuodan where qiandan in (" + sl_qiandan.Rows[i2]["id"].ToString() + ")) and jine>0 and zhuangtai='已支付' and shenhe='已通过'  and 	shanchuzhuangtai>-1").Rows[0]["jine"].ToString());
                        }
                        catch { }
                        float zongzhichu = 0;
                        try
                        {
                            zongzhichu = float.Parse(my_c.GetTable("select sum(jine) as jine from sl_zuodanshouzhi where zuodanid in (select id from sl_zuodan where qiandan in (" + sl_qiandan.Rows[i2]["id"].ToString() + ")) and jine<0 and zhuangtai='已支付' and shenhe='已通过'  and 	shanchuzhuangtai>-1").Rows[0]["jine"].ToString());
                        }
                        catch { }
                        float lirun = zongshouru + zongzhichu;

                        my_c.genxin("update sl_qiandan set zongshouru=" + zongshouru + ",zongzhichu=" + zongzhichu + ",lirun=" + lirun + " where id=" + sl_qiandan.Rows[i2]["id"].ToString());//更新


                    }
                    #endregion
                }

                #region 拼接字段
                string liemingcheng = "";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch
                {
                    liemingcheng = table_name.Split(',')[0].ToString() + ".id";
                    #region 默认字段
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                        string table_name1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable Field = new DataTable();
                        Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from sl_Model where u1='" + table_name1 + "') and  U5='是'   order by u9,id");
                        for (int i2 = 0; i2 < Field.Rows.Count; i2++)
                        {
                            liemingcheng = liemingcheng + "," + table_name1 + "." + Field.Rows[i2]["u1"].ToString();
                        }
                    }

                    #endregion
                }
                liemingcheng = liemingcheng.Replace("20%", " ");
                #endregion
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
                try
                {
                    page = my_b.c_string(Request.QueryString["page"].ToString());
                }
                catch { }
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }


                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }
                string sql1 = "";

                if (number == "")
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    //Response.Write(table_name);
                    //Response.End();
                    sql1 = "select count(" + table_name.Split(',')[0].ToString() + ".id) as count_id from " + table_name + " " + quer_sql;
                    if (page == "1")
                    {
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                    }
                    else
                    {
                        int d_page = int.Parse(number) * (int.Parse(page) - 1);
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " where id not in (select top " + d_page + " " + table_name.Split(',')[0].ToString() + ".id" + " from " + table_name + " " + quer_sql + order_str + ") " + quer_sql.Replace("where", "and") + order_str;
                    }

                }
                #region 关键词权限
                string Field_biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (Field_biaoming == "")
                    {
                        Field_biaoming = "u1='" + table_name_[i] + "'";
                    }
                    else
                    {
                        Field_biaoming = Field_biaoming + " or u1='" + table_name_[i] + "'";
                    }

                    if (biaoming == "")
                    {
                        biaoming = "sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                    else
                    {
                        biaoming = biaoming + " or sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                }
                DataTable sl_Field = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");

                DataTable Field_dt = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.Write("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.End();
                #endregion

                //Response.Write(sql);
                //Response.End();
                string result = my_js.OutJson(sql, callback, sql1, sl_Field, Field_dt, "qiandanjiesuan");

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "order_list_show")
            {
                #region 签单详细页
                //string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());
                //yonghuming = my_hs.get_yonghuming(yonghuming, "yonghuming");
                //DataTable sl_qiandan =  my_c.GetTable("select * from sl_qiandan where  (" + yonghuming + ")");
                //StringBuilder str = new StringBuilder();
                //for (int i = 0; i < sl_qiandan.Rows.Count; i++)
                //{
                //    DataTable sl_zuodan = my_c.GetTable("select * from sl_zuodan where qiandan in (" + sl_qiandan.Rows[i]["id"].ToString() + ")");
                //    for (int j = 0; j < sl_zuodan.Rows.Count; j++)
                //    {

                //    }
                //}
                string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());
                string id = "";
                try
                {
                    id = my_b.c_string(Request.QueryString["id"].ToString());
                }
                catch { }
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "sl_qiandan";
                string[] table_name_ = table_name.Split(',');
                yonghuming = my_hs.get_yonghuming(yonghuming, table_name_[0] + ".yonghuming");
                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");
                string quer_sql = "";
                if (id == "")
                {
                    if (yonghuming != "")
                    {
                        quer_sql = "where (" + yonghuming + ")";
                    }
                }
                else
                {
                    quer_sql = "where (id in (" + id + "))";
                }
                //Response.Write(quer_sql);
                //Response.End();
                #region 记录删除状态

                string biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (my_c.GetTable("select id from sl_field where u1='shanchuzhuangtai' and Model_id in (select id from sl_Model where u1='" + table_name_[i] + "')").Rows.Count > 0)
                    {
                        if (quer_sql == "")
                        {
                            quer_sql = "where (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                        }
                        else
                        {
                            quer_sql = quer_sql + " and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                        }
                    }

                }

                #endregion
                string sql = "";
                string ordertype = "id";
                string page = "1";
                #region 拼接字段
                string liemingcheng = "";
                try
                {
                    liemingcheng = my_b.c_string(Request.QueryString["liemingcheng"].ToString()).ToLower();
                }
                catch
                {
                    liemingcheng = table_name.Split(',')[0].ToString() + ".id";
                    #region 默认字段
                    for (int i1 = 0; i1 < table_name_.Length; i1++)
                    {
                        string table_name1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + table_name_[i1].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                        DataTable Field = new DataTable();
                        Field = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from sl_Model where u1='" + table_name1 + "') and  U5='是'   order by u9,id");
                        for (int i2 = 0; i2 < Field.Rows.Count; i2++)
                        {
                            liemingcheng = liemingcheng + "," + table_name1 + "." + Field.Rows[i2]["u1"].ToString();
                        }
                    }

                    #endregion
                }
                liemingcheng = liemingcheng.Replace("20%", " ");
                #endregion
                try
                {
                    ordertype = my_b.c_string(Request.QueryString["ordertype"].ToString());
                }
                catch { }
                try
                {
                    page = my_b.c_string(Request.QueryString["page"].ToString());
                }
                catch { }
                string orderby = "desc";
                try
                {
                    orderby = my_b.c_string(Request.QueryString["orderby"].ToString());
                }
                catch { }


                string order_str = "";
                if (ordertype != "" || orderby != "")
                {
                    order_str = " order by " + ordertype + " " + orderby;
                }
                string sql1 = "";

                if (number == "")
                {
                    sql = "select " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                }
                else
                {
                    //Response.Write(table_name);
                    //Response.End();
                    sql1 = "select count(" + table_name.Split(',')[0].ToString() + ".id) as count_id from " + table_name + " " + quer_sql;
                    if (page == "1")
                    {
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " " + quer_sql + order_str;
                    }
                    else
                    {
                        int d_page = int.Parse(number) * (int.Parse(page) - 1);
                        sql = "select top " + number + " " + liemingcheng + " from " + table_name + " where id not in (select top " + d_page + " " + table_name.Split(',')[0].ToString() + ".id" + " from " + table_name + " " + quer_sql + order_str + ") " + quer_sql.Replace("where", "and") + order_str;
                    }

                }
                #region 关键词权限
                string Field_biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (Field_biaoming == "")
                    {
                        Field_biaoming = "u1='" + table_name_[i] + "'";
                    }
                    else
                    {
                        Field_biaoming = Field_biaoming + " or u1='" + table_name_[i] + "'";
                    }

                    if (biaoming == "")
                    {
                        biaoming = "sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                    else
                    {
                        biaoming = biaoming + " or sl_guanjianciquanxian.biaoming='" + table_name_[i] + "'";
                    }
                }
                DataTable sl_Field = my_c.GetTable("select sl_field.u1 as u1,sl_guanjianciquanxian.kejian as kejian from sl_field,sl_guanjianciquanxian where sl_field.id=sl_guanjianciquanxian.ziduanid and (" + biaoming + ")");

                DataTable Field_dt = my_c.GetTable("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.Write("select * from sl_field where Model_id in (select id from sl_Model where " + Field_biaoming + ")");
                //Response.End();
                #endregion


                string result = my_js.OutJson(sql, callback, sql1, sl_Field, Field_dt, type);

                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "order_list_count")
            {
                #region 统计签单类型
                string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());
                string zhuangtai = my_b.c_string(Request.QueryString["zhuangtai"].ToString());
 
                string[] ziduan = zhuangtai.Split('|');
                #region 获取sql语句
         
                string id = "";
                try
                {
                    id = my_b.c_string(Request.QueryString["id"].ToString());
                }
                catch { }
                string number = "";
                try
                {
                    number = my_b.c_string(Request.QueryString["number"].ToString());
                }
                catch { }
                string table_name = "sl_qiandan";
                string[] table_name_ = table_name.Split(',');
                yonghuming = my_hs.get_yonghuming(yonghuming, "yonghuming");
                //Response.Write(yonghuming);
                //Response.End();
                //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
                Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
                string qu_str = reg.Replace(Request.QueryString.ToString(), "");
                string quer_sql = "";
                if (id == "")
                {
                    if (yonghuming != "")
                    {
                        quer_sql = " (" + yonghuming + ")";
                    }
                }
                else
                {
                    quer_sql = " (id in (" + id + "))";
                }
                string quer_diy_sql = "";
                #region 读审核表数据
                if (quer_diy_sql == "")
                {
                    if (yonghuming == "")
                    {
                        // quer_sql = "where (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and xianshi='是'))";
                    }
                    else
                    {
                        quer_diy_sql = " (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and xianshi='是' and (" + yonghuming + ")))";
                    }

                }
                else
                {
                    if (yonghuming == "")
                    {
                        //quer_sql = quer_sql + " or (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and  xianshi='是'))";
                    }
                    else
                    {
                        quer_diy_sql = quer_diy_sql + " or (id in (select laiyuanbianhao from sl_shenhe where leixing='签单' and  xianshi='是' and (" + yonghuming + ")))";
                    }

                }
                #endregion
                #region 读做单表数据
                if (quer_diy_sql == "")
                {
                    if (yonghuming == "")
                    {
                        //  quer_sql = "where (id in (select qiandan from sl_zuodan where  shanchuzhuangtai>-1))";
                    }
                    else
                    {
                        quer_diy_sql = " (id in (select qiandan from sl_zuodan where  shanchuzhuangtai>-1 and (" + yonghuming + ")))";
                    }

                }
                else
                {
                    if (yonghuming == "")
                    {
                        // quer_sql = quer_sql + " or (id in (select qiandan from sl_zuodan where shanchuzhuangtai>-1 ))";
                    }
                    else
                    {
                        quer_diy_sql = quer_diy_sql + " or (id in (select qiandan from sl_zuodan where shanchuzhuangtai>-1 and (" + yonghuming + ")))";
                    }

                }
                #endregion
                //Response.Write(quer_sql);
                //Response.End();
                #region 记录删除状态

                string biaoming = "";
                for (int i = 0; i < table_name_.Length; i++)
                {
                    if (my_c.GetTable("select id from sl_field where u1='shanchuzhuangtai' and Model_id in (select id from sl_Model where u1='" + table_name_[i] + "')").Rows.Count > 0)
                    {
                        if (quer_sql == "")
                        {
                            if (quer_diy_sql == "")
                            {
                                quer_sql = "where (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                            }
                            else
                            {
                                quer_sql = "where (" + quer_diy_sql + ") and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                            }
                        }
                        else
                        {
                            if (quer_diy_sql == "")
                            {
                                quer_sql = " where " + quer_sql + " and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                            }
                            else
                            {
                                quer_sql = " where (" + quer_sql + " or " + quer_diy_sql + ") and (" + table_name_[i] + ".shanchuzhuangtai>-1)";
                            }

                        }
                    }

                }

                #endregion
                #endregion
                //Response.Write("select id,zhuangtai from sl_qiandan   " + quer_sql + "");
                //Response.End();
                DataTable sl_qiandan = new DataTable();
                if (yonghuming != "")
                {
                    //Response.Write("select id,zhuangtai from sl_qiandan where  (" + yonghuming + ")");
                    //Response.End();
                    sl_qiandan = my_c.GetTable("select id,zhuangtai from sl_qiandan   " + quer_sql + "");

                }
                else
                {

                    sl_qiandan = my_c.GetTable("select id,zhuangtai from sl_qiandan");
                }

                //Response.Write(ziduan[0]);
                //Response.End();
                StringBuilder str = new StringBuilder();
                str.Append("[");
                str.Append("{\"title\":\"全部\",\"value\":\"" + sl_qiandan.Rows.Count + "\"}");
                for (int i = 0; i < ziduan.Length; i++)
                {
                    DataRow[] rows = sl_qiandan.Select("zhuangtai='" + ziduan[i] + "'");
                    str.Append(",");
                    str.Append("{\"title\":\"" + ziduan[i] + "\",\"value\":\"" + rows.Length + "\"}");
                }


                str.Append("]");
                string result = callback + "(" + str + ")";
                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "yuangong_list_count")
            {
                #region 统计员工状态
                string yuangongzhuangtai = my_b.c_string(Request.QueryString["yuangongzhuangtai"].ToString());
                Regex reg = new Regex("|", RegexOptions.Singleline);
                string[] ziduan = yuangongzhuangtai.Split('|');
                DataTable sl_yuangong = new DataTable();
                sl_yuangong = my_c.GetTable("select id,yuangongzhuangtai from sl_yuangong");

                //Response.Write(ziduan[0]);
                //Response.End();
                StringBuilder str = new StringBuilder();
                str.Append("[");
                str.Append("{\"title\":\"全部\",\"value\":\"" + sl_yuangong.Rows.Count + "\"}");
                for (int i = 0; i < ziduan.Length; i++)
                {
                    DataRow[] rows = sl_yuangong.Select("yuangongzhuangtai='" + ziduan[i] + "'");
                    str.Append(",");
                    str.Append("{\"title\":\"" + ziduan[i] + "\",\"value\":\"" + rows.Length + "\"}");
                }


                str.Append("]");
                string result = callback + "(" + str + ")";
                Response.Clear();
                Response.Write(result);
                Response.End();
                #endregion
            }
            else if (type == "fenpei")
            {
                #region 设置分配情况
                string yonghuming = my_b.c_string(Request.QueryString["yonghuming"].ToString());//用户名
                string fenpeiquxiang = my_b.c_string(Request.QueryString["fenpeiquxiang"].ToString());//分配去向
                int pingjunshuliang = int.Parse(my_b.c_string(Request.QueryString["pingjunshuliang"].ToString())); //平均分配数
                string shifouhuishou = my_b.c_string(Request.QueryString["shifouhuishou"].ToString());//剩余分配数
                string bianhao = my_b.c_string(Request.QueryString["bianhao"].ToString());//编号

                string[] ziduan = fenpeiquxiang.Split('|');
                DataTable sl_daoru = my_c.GetTable("select * from sl_daoru where yonghuming in ('" + yonghuming + "') and bianhao='" + bianhao + "'");
                int kefenpeishu = int.Parse(sl_daoru.Rows[0]["kefenpei"].ToString()); //可分配数
                int yifenpei = int.Parse(sl_daoru.Rows[0]["yifenpei"].ToString());//已分配数
                int shengyushuliang = 0;//剩余数量
                int fenpeishu = 0;//本次分配数量
                if (ziduan[0].ToString() == "个人")
                {
                    #region 分配给个人
                    string renyuan = ziduan[1].ToString();
                    DataTable sl_yuangong = my_c.GetTable("select id from sl_yuangong where id in (" + renyuan + ")");
                    fenpeishu = pingjunshuliang * sl_yuangong.Rows.Count;
                    if (pingjunshuliang > kefenpeishu)
                    {
                        #region 可分配数小于要求分配数
                        Response.Write(callback + "({\"date\":\"10007\"})");
                        Response.End();
                        #endregion
                    }
                    else
                    {
                        #region 可分配
                        yifenpei = yifenpei + fenpeishu;
                        shengyushuliang = kefenpeishu - fenpeishu;
                        my_c.genxin("update sl_daoru set kefenpei=" + shengyushuliang + ", yifenpei=" + yifenpei + " where id=" + sl_daoru.Rows[0]["id"].ToString());//修改导入记录表
                        my_c.genxin("insert into sl_fenpeijilu(fenpeiquxiang,kefenpeishu,pingjunshuliang,shengyushuliang,shifouhuishou,yonghuming,bianhao) values('" + ziduan[0].ToString() + "'," + kefenpeishu + "," + pingjunshuliang + "," + shengyushuliang + ",'" + shifouhuishou + "','" + yonghuming + "','" + bianhao + "')");//插入记录到分配记录表
                        for (int i = 0; i < sl_yuangong.Rows.Count; i++)
                        {
                            my_c.genxin("update sl_kehu set leixing='个人公海客户',yonghuming='" + sl_yuangong.Rows[i]["id"].ToString() + "' where id in (select top " + pingjunshuliang + " id from sl_kehu where leixing='未分配客户' and chuliren in (" + yonghuming + ") and bianhao='" + bianhao + "' and shanchuzhuangtai>-1)");//把未分配客户分配到用户头上，并且修改类型为个人公海客户
                            my_c.genxin("insert into sl_fenpeirizhi (bianhao,jieshouren,yonghuming,fenpeikehuleixing,kehushuliang,zhuangtai) values('" + bianhao + "'," + sl_yuangong.Rows[i]["id"].ToString() + "," + yonghuming + ",'导入客户'," + pingjunshuliang + ",'已通过')");//插入记录到分配日志表

                        }
                        #endregion
                    }

                    #endregion
                }
                else
                {
                    #region 分配给部门
                    string bumen = ziduan[1].ToString();
                    DataTable sl_jigou = my_c.GetTable("select id from sl_jigou where id in (" + bumen + ")");
                    fenpeishu = pingjunshuliang * sl_jigou.Rows.Count;
                    if (pingjunshuliang > kefenpeishu)
                    {
                        #region 可分配数小于要求分配数
                        Response.Write(callback + "({\"date\":\"10007\"})");
                        Response.End();
                        #endregion
                    }
                    else
                    {
                        #region 可分配
                        yifenpei = yifenpei + fenpeishu;
                        shengyushuliang = kefenpeishu - fenpeishu;
                        my_c.genxin("update sl_daoru set kefenpei=" + shengyushuliang + ", yifenpei=" + yifenpei + " where id=" + sl_daoru.Rows[0]["id"].ToString());//修改导入记录表
                        my_c.genxin("insert into sl_fenpeijilu(fenpeiquxiang,kefenpeishu,pingjunshuliang,shengyushuliang,shifouhuishou,yonghuming,bianhao) values('" + ziduan[0].ToString() + "'," + kefenpeishu + "," + pingjunshuliang + "," + shengyushuliang + ",'" + shifouhuishou + "','" + yonghuming + "','" + bianhao + "')");//插入记录到分配记录表
                        for (int i = 0; i < sl_jigou.Rows.Count; i++)
                        {
                            my_c.genxin("update sl_kehu set leixing='部门公海客户',suoshubumen=" + sl_jigou.Rows[i]["id"].ToString() + " where id in (select top " + pingjunshuliang + " id from sl_kehu where leixing='未分配客户' and chuliren in (" + yonghuming + ") and bianhao='" + bianhao + "' and shanchuzhuangtai>-1)");//把未分配客户分配到用户头上，并且修改类型为个人公海客户
                            my_c.genxin("insert into sl_fenpeirizhi (bianhao,jieshoubumen,yonghuming,fenpeikehuleixing,kehushuliang,zhuangtai) values('" + bianhao + "'," + sl_jigou.Rows[i]["id"].ToString() + "," + yonghuming + ",'导入客户'," + pingjunshuliang + ",'已通过')");//插入记录到分配日志表

                        }
                        #endregion
                    }

                    #endregion
                }
                #endregion
                #region 可分配数小于要求分配数
                Response.Write(callback + "({\"date\":\"ok\"})");
                Response.End();
                #endregion
            }
            else if (type == "peoplechoose")
            {
                #region 获取组织人员选取数据
                string id = Request.QueryString["id"];
                string leixing = Request.QueryString["leixing"];
                string data = "";
                DataTable yuangong = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "yuangong where id=" + id);
                if (yuangong.Rows.Count > 0)
                {

                }
                #endregion
            }
            else if (type == "todaytongji")
            {
                #region 获取今日统计数据
                string yhm = Request.QueryString["yhm"];
                string data = "";
                string yonghuming = my_hs.get_yonghuming(yhm, "id");
                //Response.Write(yonghuming);
                //Response.End();
                DataTable sl_bumen = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "jigou where id in (select suoshubumen from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "yuangong where id=" + yhm + ")");
                if (sl_bumen.Rows.Count > 0)
                {
                    DataTable sl_yuangong = new DataTable();
                    if (yonghuming == "")
                    {
                        sl_yuangong = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "yuangong ");
                    }
                    else
                    {
                        sl_yuangong = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "yuangong where " + yonghuming);
                    }

                    string yuangong = "";
                    if (sl_yuangong.Rows.Count > 0)
                    {
                        int zongxinzengshu = 0;
                        int zongfenpeishu = 0;
                        int zongqiandanshu = 0;
                        int zonggenjinshu = 0;
                        int zongfangqishu = 0;
                        for (int i = 0; i < sl_yuangong.Rows.Count; i++)
                        {
                            #region 员工循环
                            string xinzeng = "";
                            string fenpei = "";
                            string qiandan = "";
                            string genjin = "";
                            string fangqi = "";

                            string xingzeng_count = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi where yonghuming=" + sl_yuangong.Rows[i]["id"].ToString() + " and leixing='转入意向' and datediff(day, dtime ,getdate()) = 0").Rows[0]["count_id"].ToString();

                            xinzeng = "\"xinzeng\":\"" + xingzeng_count + "\"";
                            zongxinzengshu = zongxinzengshu + int.Parse(xingzeng_count);
                            DataTable sl_fenpei = my_c.GetTable("select kehushuliang from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "fenpeirizhi where jieshouren=" + sl_yuangong.Rows[i]["id"].ToString() + " and zhuangtai='已通过' and datediff(day, dtime ,getdate()) = 0");
                            int fenpeishu = 0;
                            if (sl_fenpei.Rows.Count > 0)
                            {
                                for (int j = 0; j < sl_fenpei.Rows.Count; j++)
                                {
                                    fenpeishu = fenpeishu + int.Parse(sl_fenpei.Rows[j]["kehushuliang"].ToString());
                                }
                                fenpei = "\"fenpei\":\"" + fenpeishu + "\"";
                            }
                            else
                            {
                                fenpei = "\"fenpei\":\"0\"";
                            }
                            zongfenpeishu = zongfenpeishu + fenpeishu;

                            string qiandan_count = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "qiandan where yonghuming=" + sl_yuangong.Rows[i]["id"].ToString() + " and shanchuzhuangtai=0 and datediff(day, dtime ,getdate()) = 0").Rows[0]["count_id"].ToString();
                            qiandan = "\"qiandan\":\"" + qiandan_count + "\"";
                            zongqiandanshu = zongqiandanshu + int.Parse(qiandan_count);

                            string genjin_count = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "genjinjilu where yonghuming=" + sl_yuangong.Rows[i]["id"].ToString() + " and datediff(day, dtime ,getdate()) = 0").Rows[0]["count_id"].ToString();
                            genjin = "\"genjin\":\"" + genjin_count + "\"";
                            zonggenjinshu = zonggenjinshu + int.Parse(genjin_count);
                            string fenqi_count = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi where yonghuming=" + sl_yuangong.Rows[i]["id"].ToString() + " and leixing='放弃客户' and datediff(day, dtime ,getdate()) = 0").Rows[0]["count_id"].ToString();
                            
                            fangqi = "\"fangqi\":\"" + fenqi_count + "\"";
                            zongfangqishu = zongfangqishu + int.Parse(fenqi_count);
                            if (yuangong == "")
                            {
                                yuangong = "{\"xingming\":\"" + sl_yuangong.Rows[i]["yuangongxingming"].ToString() + "\",\"yuangongid\":\"" + sl_yuangong.Rows[i]["id"].ToString() + "\"," + xinzeng + "," + fenpei + "," + qiandan + "," + genjin + "," + fangqi + "}";
                            }
                            else
                            {
                                yuangong = yuangong + ",{\"xingming\":\"" + sl_yuangong.Rows[i]["yuangongxingming"].ToString() + "\",\"yuangongid\":\"" + sl_yuangong.Rows[i]["id"].ToString() + "\"," + xinzeng + "," + fenpei + "," + qiandan + "," + genjin + "," + fangqi + "}";
                            }
                            #endregion
                          
                        }
                     
                        data = "[{\"bumenmingcheng\":\"" + sl_bumen.Rows[0]["jigoumingcheng"].ToString() + "\",\"bumenid\":\"" + sl_bumen.Rows[0]["id"].ToString() + "\",\"_expanded\":true,\"zongxinzeng\":\"" + zongxinzengshu + "\",\"zongfenpei\":\"" + zongfenpeishu + "\",\"zongqiandan\":\"" + zongqiandanshu + "\",\"zonggenjin\":\"" + zonggenjinshu + "\",\"zongfangqi\":\"" + zongfangqishu + "\",\"yuangong\":[" + yuangong + "]}]";
                        //Response.Write(data);
                        //Response.End();
                    }
                }
                Response.Write(callback + "(" + data + ")");
                Response.End();
                #endregion
            }
            #endregion
        }
    }
}
