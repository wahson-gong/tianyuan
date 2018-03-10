using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// my_dt 的摘要说明
/// 处理datetable 初始化等
/// author ghy
/// time 20180117
/// </summary>
public class my_dt
{
    public DataTable setdt(string[] dt_rows)
    {
        DataTable my_dt = new DataTable();
        foreach (string item in dt_rows)
        {
            //创建table的第一列 
            DataColumn myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            myColumn.ColumnName = item;//该列得名称 
            myColumn.DefaultValue = "";//该列得默认值 

            my_dt.Columns.Add(myColumn); // 将所有的列添加到table上
        }

        return my_dt;
    
    }

}