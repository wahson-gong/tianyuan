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
public partial class admin_show_auto_table : System.Web.UI.Page
{
    public string get_url(string content)
    {
        Regex reg = new Regex("<.*?>", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(content);
        foreach (Match match in matches)
        {
            string t1 = match.ToString().Replace("<", "").Replace(">", "");
            content = content.Replace(match.ToString(), HttpContext.Current.Request.QueryString[t1].ToString());
        }

        return content;
    }
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    //user_value  为数据库默认数据  txt_value为上传数据
    public void set_kj_edit(TableCell c, string type, string id, string txt_value, string user_value)
    {
        if (type == "文件框")
        {
            Literal txt;
            //创建TextBox 
            txt = new Literal();
            txt.Text = user_value + "&nbsp;&nbsp;<a href='" + user_value + "' target='_blank'>下载文件</a>";
            //添加控件到容器 
            c.Controls.Add(txt);
        }
        else if (type == "下拉框")
        {
            #region 输出列表时的下拉框处理
            string t1 = "";
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + user_value + ") " + aa[2].ToString();
                    ziduan = aa[3].ToString();
                    if (ziduan.Split(',')[0].ToString() != "id")
                    {
                        sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and " + ziduan.Split(',')[0].ToString() + " ='" + user_value + "' " + aa[2].ToString();
                    }
                    else
                    {
                        sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + user_value + ") " + aa[2].ToString();
                    }
                    string sql1 = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();

                    try
                    {
                        dt = my_c.GetTable(sql);
                        int hangshu = dt.Rows.Count;
                        if (dt.Rows.Count > 0)
                        {
                            if (ziduan.Split(',').Length > 1)
                            {
                                t1 = t1 + dt.Rows[0][ziduan.Split(',')[1].ToString()].ToString();
                            }
                            else
                            {
                                t1 = dt.Rows[0][ziduan].ToString();
                            }

                        }

                    }
                    catch
                    {
                        //  dt = my_c.GetTable(sql1);
                        t1 = user_value;
                    }

                    ziduan = aa[3].ToString();

                }



            }
            else
            {
                t1 = user_value;
            }
            //end
            Literal txt;
            //创建TextBox 
            txt = new Literal();
            txt.Text = t1;
            c.Controls.Add(txt);
            #endregion
        }
        else if (type == "缩略图" || type == "头像")
        {
            Literal txt;
            //创建TextBox 
            txt = new Literal();
            txt.Text = "<a href='showpic.aspx?url=" + user_value + "' target=_blank><img onerror=\"this.src='images/nopic.jpg';\"  src='" + user_value + "' height='160px'></a>";
            //添加控件到容器 
            c.Controls.Add(txt);
        }
        else if (type == "多条数据")
        {
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + txt_value + " and u5='是' order by u9,id");
            DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + txt_value) + " where laiyuanbianhao=" + user_value);
            int numrows = 1;
            int numcells = 2;
            string neirong = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tiaoshu = "";
                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    tiaoshu = tiaoshu + "<td>" + Model_dt.Rows[d1]["u2"].ToString() + "</td><td>" + dt.Rows[i][Model_dt.Rows[d1]["u1"].ToString()].ToString() + "</td>";
                }
                neirong = neirong + "<tr>" + tiaoshu + "</tr>";

            }
            c.Controls.Add(new LiteralControl("<table>" + neirong + "</table>"));
        }
        else
        {
            Literal txt;
            //创建TextBox 
            txt = new Literal();
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + "  order by u9,id");
            DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString());
            int numrows = 1;
            int numcells = 2;
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                for (int j = 0; j < numrows; j++)
                {
                    TableRow r = new TableRow();
                    for (int i = 0; i < numcells; i++)
                    {
                        TableCell c = new TableCell();
                        if (i == 0)
                        {
                            c.Controls.Add(new LiteralControl(Model_dt.Rows[d1]["u2"].ToString()));
                            c.CssClass = "tRight";
                        }
                        else if (i == 1)
                        {
                            set_kj_edit(c, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), Model_dt.Rows[d1]["u8"].ToString(), dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString());
                        }
                        else
                        {

                        }


                        r.Cells.Add(c);

                    }
                    Table1.Rows.Add(r);
                }
            }




        }
    }
}