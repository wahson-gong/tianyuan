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
using System.Text.RegularExpressions;

public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (my_b.c_string(Request.QueryString["type"].ToString()) == "add")
            {
                string classid = my_b.c_string(Request.QueryString["classid"].ToString());
               
                DropDownList1.DataSource = my_c.GetTable("select u1,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter  where classid=" + classid + "");
                DropDownList1.DataTextField = "u1";
                DropDownList1.DataValueField = "id";
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, "—请选择—");
                DropDownList1.Items[0].Value = "";

                DropDownList2.Items.Insert(0, "—请选择上级—");
                DropDownList2.Items[0].Value = "";

                get_shuju();
                //end
            }
            if (my_b.c_string(Request.QueryString["type"].ToString()) == "edit")
            {
                string classid = my_b.c_string(Request.QueryString["classid"].ToString());
                string m_id = "";
                try
                {
                    m_id = my_b.c_string(Request.QueryString["m_id"].ToString());
                }
                catch { }
                DropDownList1.DataSource = my_c.GetTable("select u1,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter  where classid=" + classid + "");
                DropDownList1.DataTextField = "u1";
                DropDownList1.DataValueField = "id";
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, "—请选择—");
                DropDownList1.Items[0].Value = "";

                DropDownList2.Items.Insert(0, "—请选择上级—");
                DropDownList2.Items[0].Value = "";
                string[] aa = Request.QueryString["neirong"].ToString().Split('|');
                
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Text == aa[0].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
                try
                {
                    if (DropDownList1.SelectedItem.Value != "")
                    {
                        DropDownList2.DataSource = my_c.GetTable("select u1,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter  where classid=" + DropDownList1.SelectedItem.Value + "");
                        DropDownList2.DataTextField = "u1";
                        DropDownList2.DataValueField = "id";
                        DropDownList2.DataBind();
                    }
                }
                catch
                { }
                for (i = 0; i < DropDownList2.Items.Count; i++)
                {
                    try
                    {
                        if (DropDownList2.Items[i].Text == aa[1].ToString())
                        {
                            DropDownList2.Items[i].Selected = true;
                        }
                    }
                    catch { }
                }
                get_shuju();
                //end
            }
            else
            { 
            
            }



        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedItem.Value != "")
        {
            DropDownList2.DataSource = my_c.GetTable("select u1,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter  where classid=" + DropDownList1.SelectedItem.Value + "");
            DropDownList2.DataTextField = "u1";
            DropDownList2.DataValueField = "id";
            DropDownList2.DataBind();
            get_shuju();
        }
        else
        {
            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, "—请选择上级—");
            DropDownList2.Items[0].Value = "";
        }
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        get_shuju();
    }
    public void get_shuju()
    {
        string shuju = DropDownList1.SelectedItem.Text + "|" + DropDownList2.SelectedItem.Text;
        Response.Write("<script> window.parent.document.getElementById('" + Request.QueryString["textBox"].ToString() + "').value='" + shuju + "'</script>");
    }
}
