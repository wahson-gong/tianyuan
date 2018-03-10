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
using System.Text.RegularExpressions;


public partial class daochuexcle : System.Web.UI.Page
{
    public string user_sql(string quer_string)
    {
        string return_string = "";
        if (quer_string != "")
        {

            string[] quer_arr = quer_string.Split(',');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (return_string == "")
                {
                    return_string = "u1='" + quer_arr[i] + "'";
                }
                else
                {
                    return_string = return_string + " or u1='" + quer_arr[i] + "'";
                }
            }
        }

        return return_string;
    }

    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_json my_js = new my_json();
    biaodan bd = new biaodan();
    my_hanshu my_hs = new my_hanshu();
    my_html my_h = new my_html();
    DataTable dt = new DataTable();
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
    public void WriteExcel(DataTable dt, string filePath, string zhuangtai)
    {
        if (!string.IsNullOrEmpty(filePath) && null != dt && dt.Rows.Count > 0)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet(dt.TableName);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row2.CreateCell(j).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                }
            }
            if (zhuangtai == "是")
            {
                // 写入到客户端  
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    book.Write(ms);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;
                }
            }
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", filePath));
                Response.BinaryWrite(ms.ToArray());
                book = null;
                ms.Close();
                ms.Dispose();
            }


            //end
        }
    }
    #region 别名的处理
    public string setbieming(string g1)
    {
        g1 = g1.Replace("/", "");
        return g1;
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
            #region API接口验证
            set_api("");
            #endregion
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
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name.Split(',')[0].ToString() + "'");
            //ConfigurationSettings.AppSettings["Prefix"].ToString() + 
            Regex reg = new Regex("&_.*?", RegexOptions.Singleline);
            string qu_str = reg.Replace(Request.QueryString.ToString(), "");
            string quer_sql_diy = "";
            #region 根据表不同生成SQL
            quer_sql_diy += my_hs.get_diff_table_sql(table_name_[0].ToString());
            #endregion
            string quer_sql = my_h.get_quer_sql(qu_str, table_name);
            if (quer_sql_diy != "")
            {
                if (quer_sql == "")
                {
                    quer_sql = quer_sql + " where " + quer_sql_diy;
                }
                else
                {
                    quer_sql = quer_sql + " and " + quer_sql_diy;
                }

            }
            //Response.Write(quer_sql);
            //Response.End();
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

            }
            #region 配置字段
            string select_return = "id as 编号";
            DataTable dt2 = new DataTable();
            if (liemingcheng == "")
            {
                dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_dt.Rows[0]["id"] + " and u13<>'是' order by u9 ,id");
            }
            else
            {
                dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_dt.Rows[0]["id"] + " and (" + user_sql(liemingcheng) + ") order by u9 ,id");
            }
            //u13 设置在小屏下隐藏显示
            for (int i = 0; i < dt2.Rows.Count; i++)
            {

                string type = dt2.Rows[i]["u6"].ToString();
                if (dt2.Rows[i]["u1"].ToString() == "shenfenzhenghao")
                {
                    select_return = select_return + ",''''+cast(" + dt2.Rows[i]["u1"].ToString() + " as varchar(250)) as " + setbieming(dt2.Rows[i]["u2"].ToString()) + "";
                }
                else
                {
                    select_return = select_return + "," + dt2.Rows[i]["u1"].ToString() + " as " + setbieming(dt2.Rows[i]["u2"].ToString()) + "";
                }
            }
            liemingcheng = select_return;
            #endregion
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
                        liemingcheng = "convert(decimal(38, 0)," + aa[0] + ") as " + setbieming(aa[2]) + "";
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

            //end

            //Response.Write(sql);
            //Response.End();

            DataTable dt1 = my_c.GetTable(sql);

            biaoming = Model_dt.Rows[0]["u1"].ToString();
            if (Print == "yes")
            {
                biaoming = "/upfile/excle/" + biaoming + "_" + DateTime.Now.ToString().Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "");
                WriteExcel(dt1, Server.MapPath(biaoming) + ".xls", "是");
                string callback = "";
                try
                {
                    callback = Request.QueryString["jsoncallback"].ToString();
                }
                catch { }
                string result = callback + "({\"date\":\"" + biaoming + ".xls\"})";

                Response.Clear();
                Response.Write(result);
                Response.End();
            }
            else
            {
                WriteExcel(dt1, biaoming + "_" + DateTime.Now.ToString().Replace("-", "").Replace(" ", "") + ".xls", "否");
            }

        }
    }
}