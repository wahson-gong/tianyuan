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
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
public partial class admin_sql_back : System.Web.UI.Page
{
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    public int access_i = 0;
    public int back_data(string g1)
    {
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString() + Num1.ToString();
        SqlConnection con = new SqlConnection();
        con.ConnectionString = my_c.Getconn();
        string sql = "BACKUP DATABASE " + g1 + " to DISK ='" + HttpContext.Current.Request.PhysicalApplicationPath + "back_data/" + tt1 + "'";
        SqlCommand cmd = new SqlCommand(sql, con);
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch
        {
            return 0;
        }
    }
    public DataTable GetSchemaTable()
    {
        using (OleDbConnection connection = new
                   OleDbConnection(my_c.Getconn()))
        {
            connection.Open();
            DataTable schemaTable = connection.GetOleDbSchemaTable(
                OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });
            return schemaTable;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                Repeater2.Visible = false;
                Repeater1.DataSource = my_c.GetTable("SELECT name,id,crdate,refdate FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0").DefaultView;
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.Visible = false;
                Repeater2.DataSource = GetSchemaTable();
                Repeater2.DataBind();
            }


        }
    }


    protected void Button2_Click1(object sender, EventArgs e)
    {
        if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
        {
            string all_string = "";
            DataTable all_table = my_c.GetTable("SELECT name,id,crdate,refdate FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0");
            for (int i = 0; i < all_table.Rows.Count; i++)
            {
                all_string = all_string + "Drop Table [" + all_table.Rows[i]["name"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "]sqlnextsql";
                DataTable all_ziduan = my_c.GetTable("SELECT Name,xtype FROM SysColumns WHERE id=Object_Id('" + all_table.Rows[i]["name"].ToString() + "') ");
                all_string = all_string + "CREATE TABLE [" + all_table.Rows[i]["name"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";
                string insert_sql = "SET IDENTITY_INSERT [" + all_table.Rows[i]["name"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] ON insert into [" + all_table.Rows[i]["name"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";

                for (int j = 0; j < all_ziduan.Rows.Count; j++)
                {
                    if (all_ziduan.Rows[j]["name"].ToString() == "id" && all_ziduan.Rows[j]["xtype"].ToString() == "56")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] [int] IDENTITY (1, 1) NOT NULL";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] [int] IDENTITY (1, 1) NOT NULL";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "35")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "]  [text] NULL ";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] [text] NULL ";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "61")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] [datetime] default (getdate())";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] [datetime] default (getdate())";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "56")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] [int] default 0";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] [int] default 0";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "62")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] [float] default 0";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] [float] default 0";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] [varchar] (250) NULL";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] [varchar] (250) NULL";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }

                }
                all_string = all_string + ")" + "sqlnextsql";

                string insert_value = "";
                DataTable dt = my_c.GetTable("select * from " + all_table.Rows[i]["name"].ToString() + " order by id");
                for (int j1 = 0; j1 < dt.Rows.Count; j1++)
                {
                    insert_value = insert_value + insert_sql + ") values(";
                    for (int j2 = 0; j2 < all_ziduan.Rows.Count; j2++)
                    {
                        if (all_ziduan.Rows[j2]["name"].ToString() == "id" && all_ziduan.Rows[j2]["xtype"].ToString() == "56")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "35")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "61")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "62")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "56")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                        }
                        else
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                        }


                    }


                    insert_value = insert_value + ")sqlnextsql";

                }

                all_string = all_string + insert_value;

            }
            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\sql\");
            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\sql\sql_sql" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("-", "").Replace("/", "") + ".txt", all_string);
            Response.Redirect("err.aspx?err=数据库已经成功备份！&errurl=" + my_b.tihuan("sql_back.aspx", "&", "fzw123") + "");
        }
        else
        {
            string all_string = "";
            OleDbConnection conn = new OleDbConnection(my_c.Getconn());
            conn.Open();
            DataTable all_table = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "table" });
            conn.Close();
            for (int i = 0; i < all_table.Rows.Count; i++)
            {
                if (all_table.Rows[i]["TABLE_NAME"].ToString().IndexOf(ConfigurationSettings.AppSettings["Prefix"].ToString()) > -1)
                {
                    all_string = all_string + "Drop Table " + all_table.Rows[i]["TABLE_NAME"].ToString() + "sqlnextsql";
                    OleDbConnection conn1 = new OleDbConnection(my_c.Getconn());
                    conn1.Open();
                    DataTable all_ziduan = conn1.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { null, null, all_table.Rows[i]["TABLE_NAME"].ToString(), null });
                    conn1.Close();
                    all_string = all_string + "CREATE TABLE [" + all_table.Rows[i]["TABLE_NAME"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";
                    string insert_sql = "SET IDENTITY_INSERT [" + all_table.Rows[i]["TABLE_NAME"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] ON insert into [" + all_table.Rows[i]["TABLE_NAME"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";

                    for (int j = 0; j < all_ziduan.Rows.Count; j++)
                    {
                        if (all_ziduan.Rows[j]["COLUMN_NAME"].ToString() == "id" && all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "3")
                        {
                            if (j == 0)
                            {
                                all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [int] IDENTITY (1, 1) NOT NULL";
                                insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                            else
                            {
                                all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [int] IDENTITY (1, 1) NOT NULL";
                                insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                        }
                        else if (all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "130" && all_ziduan.Rows[j]["COLUMN_FLAGS"].ToString() == "234")
                        {
                            if (j == 0)
                            {
                                all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [text] NULL";
                                insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                            else
                            {
                                all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [text] NULL";
                                insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                        }
                        else if (all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "7")
                        {
                            if (j == 0)
                            {
                                all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [datetime] default (getdate())";
                                insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                            else
                            {
                                all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [datetime] default (getdate())";
                                insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                        }
                        else if (all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "3")
                        {
                            if (j == 0)
                            {
                                all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [int] default 0";
                                insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                            else
                            {
                                all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [int] default 0";
                                insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [varchar] (250) NULL";
                                insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                            else
                            {
                                all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] [varchar] (250) NULL";
                                insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                            }
                        }

                    }
                    all_string = all_string + ")" + "sqlnextsql";

                    string insert_value = "";
                    DataTable dt = my_c.GetTable("select * from " + all_table.Rows[i]["TABLE_NAME"].ToString() + " order by id");
                    for (int j1 = 0; j1 < dt.Rows.Count; j1++)
                    {
                        insert_value = insert_value + insert_sql + ") values(";
                        for (int j2 = 0; j2 < all_ziduan.Rows.Count; j2++)
                        {
                            if (all_ziduan.Rows[j2]["COLUMN_NAME"].ToString() == "id" && all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "3")
                            {
                                if (j2 == 0)
                                {
                                    insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                                }
                                else
                                {
                                    insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                                }
                            }
                            else if (all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "130" && all_ziduan.Rows[j2]["COLUMN_FLAGS"].ToString() == "234")
                            {
                                if (j2 == 0)
                                {
                                    insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                                }
                                else
                                {
                                    insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                                }
                            }
                            else if (all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "7")
                            {
                                if (j2 == 0)
                                {
                                    insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                                }
                                else
                                {
                                    insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                                }
                            }
                            else if (all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "3")
                            {
                                if (j2 == 0)
                                {
                                    insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                                }
                                else
                                {
                                    insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                                }
                            }
                            else
                            {
                                if (j2 == 0)
                                {
                                    insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                                }
                                else
                                {
                                    insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                                }
                            }


                        }


                        insert_value = insert_value + ")sqlnextsql";

                    }

                    all_string = all_string + insert_value;

                }


            }

            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\sql\");
            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\sql\access_sql" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("-", "").Replace("/", "") + ".txt", all_string);
            Response.Redirect("err.aspx?err=数据库已经成功备份！&errurl=" + my_b.tihuan("sql_back.aspx", "&", "fzw123") + "");
        }
        //备份成sql需要
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
        {
            string all_string = "";
            DataTable all_table = my_c.GetTable("SELECT name,id,crdate,refdate FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0");
            for (int i = 0; i < all_table.Rows.Count; i++)
            {

                all_string = all_string + "Drop Table " + all_table.Rows[i]["name"].ToString() + "sqlnextsql";
                DataTable all_ziduan = my_c.GetTable("SELECT Name,xtype FROM SysColumns WHERE id=Object_Id('" + all_table.Rows[i]["name"].ToString() + "') ");
                all_string = all_string + "CREATE TABLE [" + all_table.Rows[i]["name"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";
                string insert_sql = "insert into [" + all_table.Rows[i]["name"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";

                for (int j = 0; j < all_ziduan.Rows.Count; j++)
                {
                    if (all_ziduan.Rows[j]["name"].ToString() == "id" && all_ziduan.Rows[j]["xtype"].ToString() == "56")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] autoincrement(1,1)";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] autoincrement(1,1)";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "35")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] text";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] text";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "61")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] datetime default now()";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] datetime default now()";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "56")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] int default 0";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] int default 0";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["xtype"].ToString() == "62")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] float default 0";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] float default 0";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["name"].ToString() + "] varchar";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["name"].ToString() + "] varchar";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["name"].ToString() + "]";
                        }
                    }

                }
                all_string = all_string + ")" + "sqlnextsql";

                string insert_value = "";
                DataTable dt = my_c.GetTable("select * from " + all_table.Rows[i]["name"].ToString() + " order by id");
                for (int j1 = 0; j1 < dt.Rows.Count; j1++)
                {
                    insert_value = insert_value + insert_sql + ") values(";
                    for (int j2 = 0; j2 < all_ziduan.Rows.Count; j2++)
                    {
                        if (all_ziduan.Rows[j2]["name"].ToString() == "id" && all_ziduan.Rows[j2]["xtype"].ToString() == "56")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "35")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "61")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "56")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                        }
                        else if (all_ziduan.Rows[j2]["xtype"].ToString() == "62")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString();
                            }
                        }
                        else
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["name"].ToString()].ToString() + "'";
                            }
                        }


                    }


                    insert_value = insert_value + ")sqlnextsql";

                }

                all_string = all_string + insert_value;
            }
            //end


            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\access\");
            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\access\sql_access" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("-", "").Replace("/", "") + ".txt", all_string);
            Response.Redirect("err.aspx?err=数据库已经成功备份！&errurl=" + my_b.tihuan("sql_back.aspx", "&", "fzw123") + "");
        }
        else
        {
            string all_string = "";
            OleDbConnection conn = new OleDbConnection(my_c.Getconn());
            conn.Open();
            DataTable all_table = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "table" });
            conn.Close();
            for (int i = 0; i < all_table.Rows.Count; i++)
            {



                all_string = all_string + "Drop Table " + all_table.Rows[i]["TABLE_NAME"].ToString() + "sqlnextsql";
                OleDbConnection conn1 = new OleDbConnection(my_c.Getconn());
                conn1.Open();
                DataTable all_ziduan = conn1.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { null, null, all_table.Rows[i]["TABLE_NAME"].ToString(), null });
                conn1.Close();
                all_string = all_string + "CREATE TABLE [" + all_table.Rows[i]["TABLE_NAME"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";
                string insert_sql = "insert into [" + all_table.Rows[i]["TABLE_NAME"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "$Prefix$") + "] (";

                for (int j = 0; j < all_ziduan.Rows.Count; j++)
                {
                    if (all_ziduan.Rows[j]["COLUMN_NAME"].ToString() == "id" && all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "3")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] autoincrement(1,1)";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] autoincrement(1,1)";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "130" && all_ziduan.Rows[j]["COLUMN_FLAGS"].ToString() == "234")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] text";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] text";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "7")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] datetime default now()";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] datetime default now()";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                    }
                    else if (all_ziduan.Rows[j]["DATA_TYPE"].ToString() == "3")
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] int default 0";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] int default 0";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            all_string = all_string + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] varchar";
                            insert_sql = insert_sql + "[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                        else
                        {
                            all_string = all_string + ", [" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "] varchar";
                            insert_sql = insert_sql + ",[" + all_ziduan.Rows[j]["COLUMN_NAME"].ToString() + "]";
                        }
                    }

                }
                all_string = all_string + ")" + "sqlnextsql";

                string insert_value = "";
                DataTable dt = my_c.GetTable("select * from " + all_table.Rows[i]["TABLE_NAME"].ToString() + " order by id");
                for (int j1 = 0; j1 < dt.Rows.Count; j1++)
                {
                    insert_value = insert_value + insert_sql + ") values(";
                    for (int j2 = 0; j2 < all_ziduan.Rows.Count; j2++)
                    {
                        if (all_ziduan.Rows[j2]["COLUMN_NAME"].ToString() == "id" && all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "3")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                            }
                        }
                        else if (all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "130" && all_ziduan.Rows[j2]["COLUMN_FLAGS"].ToString() == "234")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                            }
                        }
                        else if (all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "7")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                            }
                        }
                        else if (all_ziduan.Rows[j2]["DATA_TYPE"].ToString() == "3")
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                            }
                            else
                            {
                                insert_value = insert_value + "," + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString();
                            }
                        }
                        else
                        {
                            if (j2 == 0)
                            {
                                insert_value = insert_value + "'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                            }
                            else
                            {
                                insert_value = insert_value + ",'" + dt.Rows[j1][all_ziduan.Rows[j2]["COLUMN_NAME"].ToString()].ToString() + "'";
                            }
                        }


                    }


                    insert_value = insert_value + ")sqlnextsql";

                }

                all_string = all_string + insert_value;
            }




            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\access\");
            File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile\b_data\access\access_access" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("-", "").Replace("/", "") + ".txt", all_string);
            Response.Redirect("err.aspx?err=数据库已经成功备份！&errurl=" + my_b.tihuan("sql_back.aspx", "&", "fzw123") + "");
        }
        //备份成access需要
    }



    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}
