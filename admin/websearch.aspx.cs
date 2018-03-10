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
using System.Text.RegularExpressions;
public partial class admin_websearch : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_c.genxin("delete from sl_search");
            //start
            DataTable sl_sort = my_c.GetTable("select * from sl_sort where  u6<>''");
            DataTable sl_Model = my_c.GetTable("select * from sl_Model where u3 like '新闻模型' or u3 like '文章模型'");
            for (int i = 0; i < sl_sort.Rows.Count; i++)
            {

                DataTable dt = my_c.GetTable("select * from " + sl_Model.Rows[i]["u1"].ToString() + " order by id desc");
                DataTable sl_Field = my_c.GetTable("select * from sl_Field where Model_id=" + sl_Model.Rows[i]["id"].ToString() + " and (u6 like '标题' or u6 like '文本框' or u6 like '文本域' or u6 like '编辑器' or u6 like '下拉框' or u6 like '单选按钮组' or u6 like '子编辑器')");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow[] rows = sl_sort.Select("id=" + dt.Rows[j]["classid"].ToString());
                    if (rows.Length > 0)
                    {
                        #region 如栏目存在，可生成
                        string sql = "";
                        string neirong = "";
                        string biaoti = "";
                        for (int j1 = 0; j1 < sl_Field.Rows.Count; j1++)
                        {
                            if (j1 == 0)
                            {
                                biaoti = dt.Rows[j][sl_Field.Rows[j1]["u1"].ToString()].ToString();
                            }
                            if (sl_Field.Rows[j1]["u6"].ToString() == "标题")
                            {
                                biaoti = dt.Rows[j][sl_Field.Rows[j1]["u1"].ToString()].ToString();
                            }

                            neirong = neirong + dt.Rows[j][sl_Field.Rows[j1]["u1"].ToString()];

                        }
                        sql = "insert into sl_search(biaoti,neirong,classid,laiyuanbianhao,Filepath,dtime) values('" + my_b.c_string(biaoti) + "','" + my_b.c_string(neirong) + "'," + dt.Rows[j]["classid"].ToString() + "," + dt.Rows[j]["id"].ToString() + ",'" + dt.Rows[j]["Filepath"].ToString() + "','" + dt.Rows[j]["dtime"].ToString() + "')";
                        if (my_c.GetTable("select id from sl_search where classid=" + dt.Rows[j]["classid"].ToString() + " and laiyuanbianhao=" + dt.Rows[j]["id"].ToString() + "").Rows.Count > 0)
                        {
                            Response.Write("<span style='color:green'>" + sl_Model.Rows[i]["u1"].ToString() + "表中ID：" + dt.Rows[j]["id"].ToString() + "已处理</span><br>");
                            Response.Flush();
                        }
                        else
                        {
                            //Response.Write(sql);
                            //Response.End();
                            //
                            try
                            {
                                my_c.genxin(sql);
                                Response.Write(sl_Model.Rows[i]["u1"].ToString() + "表中ID：" + dt.Rows[j]["id"].ToString() + "处理成功<br>");
                                Response.Flush();
                            }
                            catch
                            {
                                Response.Write(sql);
                                Response.End();
                                Response.Write("<span style='color:red'>" + sl_Model.Rows[i]["u1"].ToString() + "表中ID：" + dt.Rows[j]["id"].ToString() + "处理失败</span><br>");
                                Response.Flush();

                            }
                            //end
                        }
                        #endregion
                    }


                }

            }
            //end
        }
    }
}