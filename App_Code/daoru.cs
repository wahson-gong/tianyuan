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

/// <summary>
/// daoru 的摘要说明
/// </summary>
public class daoru
{ /// <summary>  
  /// datatable去重  
  /// </summary>  
  /// <param name="dtSource">需要去重的datatable</param>  
  /// <returns></returns>  
    public DataTable GetDistinctTable(DataTable dtSource)
    {
        DataTable distinctTable = null;
        try
        {
            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                string[] columnNames = GetTableColumnName(dtSource);
                DataView dv = new DataView(dtSource);
                distinctTable = dv.ToTable(true, columnNames);
            }
        }
        catch (Exception ee)
        {
            //MessageBox.Show(ee.ToString());
        }
        return distinctTable;
    }
    #region 获取表中所有列名  
    public static string[] GetTableColumnName(DataTable dt)
    {
        string cols = string.Empty;
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            cols += (dt.Columns[i].ColumnName + ",");
        }
        cols = cols.TrimEnd(',');
        return cols.Split(',');
    }
    #endregion
    my_basic my_b = new my_basic();
    #region 对应字段
    public string get_text2(string zhivalue,string ziduan)
    {
        Regex reg1 = new Regex("{page}", RegexOptions.Singleline);
        string[] bb = reg1.Split(ziduan);
        for (int i = 0; i < bb.Length; i++)
        {
            Regex reg2 = new Regex("{next}", RegexOptions.Singleline);
            string[] cc = reg2.Split(bb[i]);

            if (cc[1] == zhivalue)
            {
                return cc[0];
            }
        }
        return zhivalue;
    }
    #endregion
    #region 获取对应ID
    public string duiying(string txt_value,string ziduan)
    {

        string[] aa = Regex.Split(ziduan, "{page}");
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
    #endregion
    #region 获取数据库字段
    public string get_data_Field(string ziduan)
    {
        string txt_value = "";
        string[] aa = Regex.Split(ziduan, "{page}");
        for (int j = 0; j < aa.Length; j++)
        {
            string[] bb = Regex.Split(aa[j].ToString(), "{next}");
            if (txt_value == "")
            {
                txt_value = "u1='" + bb[1].ToString() + "'";
            }
            else
            {
                txt_value = txt_value + " or u1='" + bb[1].ToString() + "'";
            }
        }
        return txt_value;
    }
    #endregion
    public DataTable Exceltable(string Path)
    {
        //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Microsoft.ACE.OLEDB.12.0;";
        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
        OleDbConnection conn = new OleDbConnection(strConn);
        conn.Open();
        DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

        //string tableName = schemaTable.Rows[0][2].ToString().Trim();
        //Response.Write(tableName);
        //Response.End();

        conn.Dispose();
        conn.Close();
        return schemaTable;
    }
    public DataTable ExcelToDS(string Path, string table_name, int number)
    {
        //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Microsoft.ACE.OLEDB.12.0;";
        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
        OleDbConnection conn = new OleDbConnection(strConn);
        conn.Open();
        conn.Dispose();
        conn.Close();
        string strExcel = "";
        OleDbDataAdapter myCommand = null;
        DataSet ds = null;
        if (number == 0)
        {
            strExcel = "select * from [" + table_name + "]";
        }
        else
        {
            strExcel = "select top " + number + " * from [" + table_name + "]";
        }
        myCommand = new OleDbDataAdapter(strExcel, strConn);
        ds = new DataSet();
        myCommand.Fill(ds, "table1");
        return ds.Tables[0];
    }

}