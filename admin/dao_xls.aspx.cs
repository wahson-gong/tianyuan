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
using System.Data.OleDb;
using System.Text.RegularExpressions;
public partial class admin_K_Table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    //获取
    public string get_kj(string type, string id, string txt_value, string sql_type)
    {
        txt_value = my_b.c_string(txt_value);
        // Response.Write(type+"<br>");

        if (sql_type == "add")
        {
            if (type == "分类" || type == "数字")
            {

                return "" + txt_value + "";
            }
            else if (type == "密码框")
            {

                return "'" + my_b.md5(txt_value) + "'";
            }
            else
            {
                return "'" + txt_value + "'";
            }
        }
        else
        {
            if (type == "分类" || type == "数字")
            {

                return " = " + txt_value + "";
            }
            else if (type == "密码框")
            {

                return " like '" + my_b.md5(txt_value) + "'";
            }
            else
            {
                return " like '" + txt_value + "'";
            }
        }

        return "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model  order by id");
            if (dt.Rows.Count > 0)
            {
                DropDownList1.DataSource = dt;
                DropDownList1.DataTextField = "u2";
                DropDownList1.DataValueField = "u1";
                DropDownList1.DataBind();
                Literal1.Text = dt.Rows[0]["u1"].ToString();
            }

            Panel1.Visible = false;



        }
    }
    public string get_text2(string zhivalue)
    {
        Regex reg1 = new Regex("{page}", RegexOptions.Singleline);
        string[] bb = reg1.Split(this.TextBox2.Text);
        for (int i = 0; i < bb.Length; i++)
        {
            Regex reg2 = new Regex("{next}", RegexOptions.Singleline);
            string[] cc = reg2.Split(bb[i]);

            if (cc[0] == zhivalue)
            {
                return cc[1];
            }
        }
        return zhivalue;
    }


    protected void Button1_Click1(object sender, EventArgs e)
    {
        DataTable Model_dt = new DataTable();
        Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + DropDownList1.SelectedValue.ToString() + "')");
        DataTable dt = getTable(my_b.c_string(this.TextBox1.Text));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (my_c.GetTable("select u3 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + DropDownList1.SelectedValue.ToString() + "'").Rows[0]["u3"].ToString() == "文章模型")
            {

            }
            else
            {
                #region 组织SQL语句
                string sql = "insert into " + DropDownList1.SelectedValue.ToString() + " ";
                sql = sql + "(";
                for (int d1 = 0; d1 < dt.Columns.Count; d1++)
                {
                    if (d1 == 0)
                    {
                        sql = sql + get_text2(dt.Columns[d1].ToString());
                    }
                    else
                    {
                        sql = sql + "," + get_text2(dt.Columns[d1].ToString());
                    }
                }
                sql = sql + ") values (";
                for (int d1 = 0; d1 < dt.Columns.Count; d1++)
                {

                    string text2 = duiying(dt.Columns[d1].ToString());

                    string u6 = my_c.GetTable("select u6 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + DropDownList1.SelectedValue.ToString() + "') and u1='" + text2 + "'").Rows[0]["u6"].ToString();
                    //   Response.Write(dt.Columns[d1].ToString() + "<br>");
                    if (d1 == 0)
                    {
                        sql = sql + get_kj(u6, text2, dt.Rows[i][dt.Columns[d1].ToString()].ToString(), "add");
                    }
                    else
                    {
                        sql = sql + "," + get_kj(u6, text2, dt.Rows[i][dt.Columns[d1].ToString()].ToString(), "add");
                    }
                }
                sql = sql + ")";
                #endregion

                #region 判断sql
                string panduansql = "select id from " + DropDownList1.SelectedValue.ToString() + " where ";
                for (int d1 = 0; d1 < dt.Columns.Count; d1++)
                {
                    string text2 = duiying(dt.Columns[d1].ToString());
                    string u6 = my_c.GetTable("select u6 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + DropDownList1.SelectedValue.ToString() + "') and u1='" + text2 + "'").Rows[0]["u6"].ToString();
                    if (d1 == 0)
                    {
                        panduansql = panduansql + " " + get_text2(dt.Columns[d1].ToString()) + get_kj(u6, text2, dt.Rows[i][dt.Columns[d1].ToString()].ToString(), "") + "";
                    }
                    else
                    {
                        panduansql = panduansql + " and " + get_text2(dt.Columns[d1].ToString()) + get_kj(u6, text2, dt.Rows[i][dt.Columns[d1].ToString()].ToString(), "") + "";
                    }
                }
                #endregion
                //Response.Write(sql);
                //Response.End();
                int heji_ = 0;
                try
                {
                    DataTable heji = new DataTable();
                    heji = my_c.GetTable(panduansql);
                    heji_ = heji.Rows.Count;
                }
                catch
                {
                    HttpContext.Current.Response.Write(panduansql);
                    HttpContext.Current.Response.End();
                }
                if (heji_ == 0)
                {
                    if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
                    {
                        string sta = "1";
                        try
                        {
                            my_c.genxin(sql);
                            sta = "0";
                        }
                        catch
                        {
                            HttpContext.Current.Response.Write(sql);
                            HttpContext.Current.Response.End();
                            sta = "1";
                        }
                        if (sta == "1")
                        {
                            Response.Write(sql + "行<span style='color:red'>失败</span><br>");
                            Response.Flush();
                        }
                        else
                        {
                            Response.Write(sql + "行成功<br>");
                            Response.Flush();
                        }
                    }
                    else
                    {
                        try
                        {
                            my_c.genxin(sql);
                            Response.Write(sql + "行成功<br>");
                            Response.Flush();
                        }
                        catch
                        {
                            Response.Write(sql + "行<span style='color:red'>失败</span><br>");
                            Response.Flush();
                        }
                    }

                }
                else
                {
                    Response.Write(sql + "行<span style='color:green'>重复</span><br>");
                    Response.Flush();
                }

                //end
            }
        }

        Response.Write("<script>window.location='err.aspx?count_s=2&err=导入数据成功！&errurl=dao_xls.aspx'</script>");


    }
    //获取对应ID
    public string duiying(string txt_value)
    {

        string[] aa = Regex.Split(this.TextBox2.Text, "{page}");
        for (int j = 0; j < aa.Length; j++)
        {
            string[] bb = Regex.Split(aa[j].ToString(), "{next}");
            if (bb[0].ToString() == txt_value)
            {
                return bb[1].ToString();
            }
        }
        return "";
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Literal1.Text = DropDownList1.SelectedValue.ToString();
        if (my_b.c_string(this.TextBox1.Text) == "")
        {
            Response.Redirect("err.aspx?err=文件不能为空！&errurl=" + my_b.tihuan("dao_xls.aspx", "&", "fzw123") + "");
        }
        set_xls();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        if (my_b.c_string(this.TextBox1.Text) == "")
        {
            Response.Redirect("err.aspx?err=文件不能为空！&errurl=" + my_b.tihuan("dao_xls.aspx", "&", "fzw123") + "");
        }
        set_xls();
        Panel1.Visible = true;

    }
    //提交时获取字段之内
    public void set_xls()
    {
        DataTable dt = getTable(my_b.c_string(this.TextBox1.Text));
        int numrows = 1;
        int numcells = 2;
        for (int d1 = 0; d1 < dt.Columns.Count; d1++)
        {

            for (int j = 0; j < numrows; j++)
            {
                TableRow r = new TableRow();
                for (int i = 0; i < numcells; i++)
                {
                    TableCell c = new TableCell();
                    if (i == 0)
                    {

                        c.Controls.Add(new LiteralControl(dt.Columns[d1].ToString()));
                        c.CssClass = "zuo1";
                    }
                    else if (i == 1)
                    {
                        DropDownList dro;
                        dro = new DropDownList();
                        dro.ID = dt.Columns[d1].ToString();
                        dro.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in (select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + DropDownList1.SelectedValue.ToString() + "')");
                        dro.DataTextField = "u2";
                        dro.DataValueField = "u1";
                        dro.DataBind();

                        for (int j1 = 0; j1 < dro.Items.Count; j1++)
                        {

                            if (dro.Items[j1].Text == dt.Columns[d1].ToString())
                            {
                                dro.Items[j1].Selected = true;
                            }
                        }
                        //  dro.AutoPostBack = true;
                        // dro.Attributes["onchange"] = "aa('" + dt.Columns[d1].ToString() + "', document.all." + dt.Columns[d1].ToString() + ".value)";
                        c.Controls.Add(dro);
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
    //获取xls
    private DataTable getTable(string g1)
    {
        //try
        //{
        // path即是excel文档的路径。
        string conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + Server.MapPath(g1) + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1';";
        //Sheet1为excel中表的名字
        string table_name = GetExcelTableName(g1);
        Literal2.Text = table_name;
        string sql = "select * from [" + table_name + "]";
        OleDbCommand cmd = new OleDbCommand(sql, new OleDbConnection(conn));
        OleDbDataAdapter ad = new OleDbDataAdapter(cmd);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        return ds.Tables[0];
        //}
        //catch (Exception ex)
        //{

        //    return null;
        //}
    }


    /// <summary>
    /// 获取EXCEL的表 表名字列 
    /// </summary>
    /// <param name="p_ExcelFile">Excel文件</param>
    /// <returns>数据表</returns>
    public string GetExcelTableName(string p_ExcelFile)
    {
        string conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + Server.MapPath(p_ExcelFile) + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1';";
        OleDbConnection _ExcelConn = new OleDbConnection(conn);
        _ExcelConn.Open();
        DataTable _Table = _ExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        _ExcelConn.Close();
        return _Table.Rows[0]["TABLE_NAME"].ToString();
    }

}
