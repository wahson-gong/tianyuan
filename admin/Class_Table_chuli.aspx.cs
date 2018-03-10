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

public partial class admin_Class_Table : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();

    public void dr1(string t1)
    {

        DataTable dt1 = my_c.GetTable("select id,u1,u2,sort_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + "");

        if (dt1.Rows.Count > 0)
        {
           
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                string u1 = "";
                u1 = my_c.GetTable("select u7 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + dt1.Rows[j]["sort_id"].ToString() + "").Rows[0]["u7"].ToString() + "/" + my_b.pinyin(dt1.Rows[j]["u1"].ToString(), 20) + "/";
                u1 = u1.Replace("//", "/");
                my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort set u7='" + u1 + "' where id=" + dt1.Rows[j]["id"].ToString());
                dr1(dt1.Rows[j]["id"].ToString());
            }
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable sl_Model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u3 like'文章模型'");
            for (int i = 0; i < sl_Model.Rows.Count; i++)
            {
                DataTable sl_sort = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id =" + sl_Model.Rows[i]["id"].ToString() + " and Sort_id=0");
                for (int j = 0; j < sl_sort.Rows.Count; j++)
                {
                    string u1 = "/"+ my_b.pinyin(sl_sort.Rows[j]["u1"].ToString(), 20) +"/";
                    my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort set u7='" + u1 + "' where id=" + sl_sort.Rows[j]["id"].ToString());
                    dr1(sl_sort.Rows[j]["id"].ToString());
                }


                DataTable dt = my_c.GetTable("select id,dtime,classid from  " + sl_Model.Rows[i]["u1"].ToString() + "");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string Filepath = "";
                    try
                    {
                        Filepath = my_c.GetTable("select u7 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + dt.Rows[j]["classid"].ToString() + "").Rows[0]["u7"].ToString();
                    }
                    catch
                    {
                        Response.Write("表名："+sl_Model.Rows[i]["u1"].ToString()+"&nbsp;SQL："+ "select u7 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + dt.Rows[j]["classid"].ToString() + "");
                        Response.End();
                    }
                    DateTime dtime = DateTime.Parse(dt.Rows[j]["dtime"].ToString());
                    
                    if (Filepath == "")
                    {
                        Filepath = "/"+ my_b.chuli_lujing();
                    }
                    else
                    {
                        Filepath = Filepath + "/" + my_b.chuli_lujing();
                        Filepath = Filepath.Replace("//", "/");
                    }
                    my_c.genxin("update " + sl_Model.Rows[i]["u1"].ToString() + " set Filepath='" + Filepath + "' where id=" + dt.Rows[j]["id"].ToString() + "");
                    //end
                }

            }

            Response.Redirect("err.aspx?err=所有栏目及数据重新生成完成！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
        }
    }

}
