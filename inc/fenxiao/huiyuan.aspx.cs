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
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();

    public string huiyuan = "";
    public string if_if(string t1, string t2)
    {
        return t1+"<br>"+t2;
    }
    //列出最顶级目录
    public void get_user(string yonghuming)
    {

        DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where jieshaoren='" + yonghuming + "' and yonghuming<>'" + yonghuming + "' order by id desc");
        if (dt.Rows.Count > 0)
        {
            for(int i=0;i<dt.Rows.Count;i++)
            {
                user_count = 0;
            huiyuan = huiyuan+ "<tr><td><img src=\""+ dt.Rows[i]["touxiang"].ToString() + "\" class=\"tx\"/><br>" + if_if(dt.Rows[i]["yonghuming"].ToString(), dt.Rows[i]["xingming"].ToString()) + " </td ><td> " + dt.Rows[i]["dtime"].ToString() + "</td><td><a href = \"/search.aspx?m=user_fenxiao_order&yonghuming=" + dt.Rows[i]["yonghuming"].ToString()+ "\" class=\"btn btn-default\">交易</a></td>		                    		<td><p class=\"yE_p1\"><a href='/single.aspx?m=user_fenxiao_user&yonghuming1=" + dt.Rows[i]["yonghuming"].ToString() + "' class=\"btn btn-default\">" + get_user_count(dt.Rows[i]["yonghuming"].ToString()) + "人</a></p></td></tr>";
               // get_user(dt.Rows[i]["yonghuming"].ToString());
            }
        }

        
    }
    //列出最顶级目录
    int user_count = 0;
    public int get_user_count(string yonghuming)
    {

        DataTable dt = my_c.GetTable("select yonghuming from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where jieshaoren='" + yonghuming + "' and yonghuming<>'" + yonghuming + "' order by id desc");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                user_count = user_count + 1;
                 get_user_count(dt.Rows[i]["yonghuming"].ToString());
            }
        }
        return user_count;


    }
    public DataTable sl_user = new DataTable();
    public string yonghuming = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
   
            try
            {
                yonghuming = Request.QueryString["yonghuming"].ToString();
                if (yonghuming == "")
                {
                    Response.End();
                }
                get_user(Request.QueryString["yonghuming"].ToString());
              
                sl_user = my_c.GetTable("select * from sl_user where yonghuming='" + Request.QueryString["yonghuming"].ToString() + "'");
            }
            catch {
                Response.End();
            }



        }
    }
}
