using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;  

/// <summary>
/// my_json_ghy 的摘要说明
/// </summary>
public class my_json_ghy
{
	public my_json_ghy()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public string DataTableToJsonWithJavaScriptSerializer(DataTable table)
    {
        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
        Dictionary<string, object> childRow;
        foreach (DataRow row in table.Rows)
        {
            childRow = new Dictionary<string, object>();
            foreach (DataColumn col in table.Columns)
            {
                childRow.Add(col.ColumnName, row[col]);
            }
            parentRow.Add(childRow);
        }
        return jsSerializer.Serialize(parentRow);
    }



    public dynamic JsonTextToArray(string json_str)
    {
        dynamic json = Newtonsoft.Json.Linq.JToken.Parse(json_str) as dynamic;

        //string name = json.Name;
        //string company = json.Company;
 
        return json;

    }

}


