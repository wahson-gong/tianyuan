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
/// fuzhi 的摘要说明
/// </summary>
public class fuzhi
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string get_kj(string type, string id, DataTable dt, int i)
    {
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域"  || type == "编辑器")
        {
            return "'" + my_b.c_string(dt.Rows[i][id].ToString()) + "'";
        }
        else if (type == "数字"|| type == "货币")
        {
            int shu = 0;
            try
            {
                shu = int.Parse(dt.Rows[i][id].ToString());
            }
            catch { }
            return "" + shu + "";
        }
        else if (type == "时间框")
        {
            return "'" + DateTime.Now.ToString() + "'";
        }
        else if (type == "单选按钮组")
        {

            if (id == "shenhe")
            {
                //处理会员
                DataTable sl_user = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where yonghuming='" + my_b.k_cookie("admin_id") + "'");

                if (sl_user.Rows[0]["huiyuanzu"].ToString().IndexOf("管理员") == -1)
                {
                    return "'未审核'";
                }
                else
                {
                    return "'" + dt.Rows[i][id].ToString() + "'";
                }
                //end
            }
            else
            {
                return "'" + dt.Rows[i][id].ToString() + "'";
            }
        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(my_b.c_string(dt.Rows[i][id].ToString())) + "'";
        }
        else if (type == "分类")
        {
            return "" + dt.Rows[i][id].ToString() + "";
        }
        else
        {
            return "'" + dt.Rows[i][id].ToString() + "'";
        }
    }
    #region 复制数据，传输Model_id，ID可以为空，多个ID逗号隔开
    public void shujufuzhi(string Model_id,string id)
    {
        #region 复制
        DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
        string table_name = model_table.Rows[0]["u1"].ToString();
        DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " ");
        DataTable dt = new DataTable();
        if (id == "")
        {
            dt = my_c.GetTable("select top 20000 * from " + table_name + " ");
        }
        else
        {
            dt = my_c.GetTable("select  * from " + table_name + " where id in (" + id+ ") ");
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
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

            if (model_table.Rows[0]["u3"].ToString() == "文章模型" || model_table.Rows[0]["u3"].ToString() == "新闻模型")
            {
                sql = sql + ",Filepath) values (";
            }
            else
            {
                sql = sql + ") values (";
            }

            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + get_kj(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), dt, i);
                }
                else
                {
                    sql = sql + "," + get_kj(Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), dt, i);
                }
            }
            if (model_table.Rows[0]["u3"].ToString() == "文章模型" || model_table.Rows[0]["u3"].ToString() == "新闻模型")
            {
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                string filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + dt.Rows[i]["classid"] + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "')";
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
                HttpContext.Current.Response.Write(sql);
                HttpContext.Current.Response.End();
            }


        }
        #endregion
    }
    #endregion
}