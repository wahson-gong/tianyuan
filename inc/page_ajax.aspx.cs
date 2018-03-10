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
public partial class admin_page_ajax : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string get_lable(string g1, string g2)
    {
        g1 = Regex.Match(g1, ""+g2+"="+@""".*?""|"+g2+"="+@".*?[\S]", RegexOptions.Singleline).ToString();
        g1 = g1.Replace(g2 + "=", "");
        g1 = g1.Replace("\"", "");
        return g1;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string t1 = HttpUtility.UrlDecode(Request.QueryString["t1"].ToString());
        
            string type = get_lable(t1, "type");
            string span_id = get_lable(t1, "span_id");
            string value = get_lable(t1, "value");
            string sql = get_lable(t1, "sql");
           
            if (type == "+")
            {
                
                if (sql == "")
                {
                    string table = get_lable(t1, "table");
                    string id = get_lable(t1, "id");
                   
                    string nn_read = my_c.GetTable("select " + value + " from " + table + " where id=" + id + "").Rows[0][value].ToString();
                    if (nn_read == "")
                    {
                        nn_read = "0";
                    }
                    int n_read = int.Parse(nn_read) + 1;
                    my_c.genxin("update " + table + " set " + value + "=" + n_read + " where id=" + id);
                    Response.Write(type);
                    Response.Write("{fzw:next}");
                    Response.Write(span_id);
                    Response.Write("{fzw:next}");
                    Response.Write(n_read);
                }
                else
                {
                    DataTable dt = my_c.GetTable(sql);
                    Response.Write(type);
                    Response.Write("{fzw:next}");
                    Response.Write(span_id);
                    Response.Write("{fzw:next}");
                    Response.Write(dt.Rows[0][value].ToString());
                }
            }
            else if (type == "-")
            {
 
                if (sql == "")
                {
                    string table = get_lable(t1, "table");
                    string id = get_lable(t1, "id");
                    int n_read = int.Parse(my_c.GetTable("select " + value + " from " + table + " where id=" + id + "").Rows[0][value].ToString()) - 1;
                    my_c.genxin("update " + table + " set " + value + "=" + n_read + " where id=" + id);
                    Response.Write(type);
                    Response.Write("{fzw:next}");
                    Response.Write(span_id);
                    Response.Write("{fzw:next}");
                    Response.Write(n_read);
                }
                else
                {
                    DataTable dt = my_c.GetTable(sql);
                    Response.Write(type);
                    Response.Write("{fzw:next}");
                    Response.Write(span_id);
                    Response.Write("{fzw:next}");
                    Response.Write(dt.Rows[0][value].ToString());
                }
            }
            else
            {

                if (sql == "")
                {
                    string table = get_lable(t1, "table");
                    string id = get_lable(t1, "id");
                    int n_read = int.Parse(my_c.GetTable("select " + value + " from " + table + " where id=" + id + "").Rows[0][value].ToString());
                    Response.Write(type);
                    Response.Write("{fzw:next}");
                    Response.Write(span_id);
                    Response.Write("{fzw:next}");
                    Response.Write(n_read);
                }
                else
                {
                    DataTable dt = my_c.GetTable(sql);
                    Response.Write(type);
                    Response.Write("{fzw:next}");
                    Response.Write(span_id);
                    Response.Write("{fzw:next}");
                    Response.Write(dt.Rows[0][value].ToString());
                }
            }



        }
    }
}
