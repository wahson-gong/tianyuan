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
    DataTable dt = new DataTable();
    public string classid = "0";
    int jj = 0;
    string list_id = "";
    public string chankan(string id,string u5)
    {
        if (u5 == "")
        {
            return "javascript:alert('无列表模板，不可以预览。')";
        }
        else
        {
            return "/list.aspx?id="+ id + "&Model_id="+Request.QueryString["Model_id"].ToString() +"";
        }
    }
    public void dr1(string t1, int t2, string t3)
    {

        DataTable dt1 = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id in (" + t1 + ") and Model_id=" + t3 + "");
        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (list_id == "")
                {
                    list_id = dt1.Rows[j]["id"].ToString();
                }
                else
                {
                    list_id = list_id + "," + dt1.Rows[j]["id"].ToString();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1, t3);
            }
        }
    }
    public string getstring(string g1)
    {
        if (g1 == "0")
        {
            return "顶级栏目";
        }
        else
        {
            return my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + g1).Rows[0]["u1"].ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_b.set_admin_url();
            try
            {
                classid = Request.QueryString["classid"].ToString();
            }
            catch
            { }

            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + Request.QueryString["Model_id"].ToString() + " and Sort_id=" + classid + " order by paixu asc,id ");
            if (dt.Rows.Count > 0)
            {
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                if (classid != "0")
                {
               
                    Response.Redirect(my_b.get_value("u2", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter", "where u1='" + my_b.get_value("u3", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + " ") + "' and classid=11") + "?Model_id=" + Request.QueryString["Model_id"].ToString()+ "&classid=" + classid + "");
                }
            }
          Literal1.Text = my_b.set_weizhi(classid, "Class_Table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=$classid$", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string classid = "0";
        try
        {
            classid = Request.QueryString["classid"].ToString();
        }
        catch { }
        //   dr1(classid, 0, Request.QueryString["Model_id"].ToString());
        if (list_id == "")
        {
            try
            {
                list_id = Request.QueryString["classid"].ToString();
            }
            catch { }
        }

        Response.Redirect(my_b.get_value("u2", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter", "where u1='" + my_b.get_value("u3", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + " ") + "' and classid=11") + "?classid=" + list_id + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + my_b.c_string(this.TextBox1.Text) + "");
    }
}
