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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    protected void Page_Load(object sender, EventArgs e)
    {
        string secret = "a8c6307b6426a55975be09de233d13b1";//请把XXXXXX修改成您在快递100网站申请的APIKey
		string expno = Request["data"];
        string typeCom = Request["com"];

		string nu=Request["nu"];
	

		


        string apiurl = "http://api.ickd.cn/?id=112315&secret="+ secret + "+&com="+ typeCom + "&nu="+ nu + "&type=text&ord=desc&encode=gbk&ver=2&button=%CC%E1%BD%BB";
       //Response.Write (apiurl);
       //Response.End();
        WebRequest request = WebRequest.Create(@apiurl);
        WebResponse response = request.GetResponse();
        Stream stream = response.GetResponseStream();
        Encoding encode = Encoding.Default;
        StreamReader reader = new StreamReader(stream, encode);
        string detail = reader.ReadToEnd();
        string[] aa = Regex.Split(detail, "\r\n");
        string shuchu = "<ul class=\"yB_ul\">";
        for (int j = 0; j < aa.Length; j++)
        {
            
            string[] bb = Regex.Split(aa[j].ToString(), "  ");
           
            if (j == 0)
            {
                try
                {
                    shuchu = shuchu + "<li class=\"on\"><p> " + bb[0].ToString() + "</p> <p> " + bb[1].ToString() + "</p></li> ";
                }
                catch { }
            }
            else
            {
                try
                {
                    shuchu = shuchu + "<li><p> " + bb[0].ToString() + "</p> <p> " + bb[1].ToString() + "</p></li> ";
                }
                catch { }
            }
         
        }
        shuchu = shuchu + "</ul>";
        Response.Write(shuchu);
    }
}