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

    int jishu = 1;
    DataTable dt = new DataTable();
    int jj = 0;
    int i = 0;

    public void qingkong(int t2)
    {

        for (int i = t2 + 1; i <= 20; i++)
        {
            DropDownList dr = new DropDownList();
            dr = (DropDownList)Form.FindControl("DropDownList" + i);
            dr.Items.Clear();
            dr.Visible = false;
        }




    }

    public void set_dr(int t2, int j, DataTable dt1)
    {
        t2 = t2 + 1;
        DropDownList dr = new DropDownList();
        dr = (DropDownList)Page.FindControl("DropDownList" + t2);

        dr.Items.Insert(j, dt1.Rows[j]["u1"].ToString());
        dr.Items[j].Value = dt1.Rows[j]["id"].ToString();
        dr.Visible = true;

        //Response.Write("<script> window.parent.document.getElementById('" + Request.QueryString["textBox"].ToString() + "').value='" + dr.SelectedValue + "';</script>");
        //Response.Write("<script> if(window.parent.document.getElementById('dingmulu')){window.parent.document.getElementById('dingmulu').value='" + my_b.get_ding_dir(dr.SelectedValue) + "'};</script>");
        qingkong(t2);
    }

    public void dr1(string t1, int t2)
    {
        t1 = t1.Split('|')[0].ToString();
        if (t1 != "")
        {
            //t1的值不是空
            DataTable dt1 = new DataTable();
            if (notclassid != "")
            {
                dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=" + t1 + " and id not in (" + notclassid + ") order by u3 desc,id asc");
            }
            else
            {
                dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=" + t1 + " order by u3 desc,id asc");
            }

            string classid = "";
            if (dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        classid = dt1.Rows[j]["id"].ToString();
                        qingkong(t2);
                    }

                    set_dr(t2, j, dt1);

                }

                t2 = t2 + 1;
                DropDownList dr = new DropDownList();
                dr = (DropDownList)Page.FindControl("DropDownList" + t2);
                dr.Items.Insert(0, "选择模板");
                dr.Items[0].Value = "|" + t1;
                if (classid != "")
                {
                    dr1(classid, t2);
                }

            }
            else
            {
                DropDownList dr = new DropDownList();
                dr = (DropDownList)Page.FindControl("DropDownList" + t2);
                //Response.Write("<script> window.parent.document.getElementById('" + Request.QueryString["textBox"].ToString() + "').value='" + dr.SelectedValue + "';</script>");
                //Response.Write("<script> if(window.parent.document.getElementById('dingmulu')){window.parent.document.getElementById('dingmulu').value='" + my_b.get_ding_dir(dr.SelectedValue) + "'};</script>");
                qingkong(t2);
            }
            //end
        }
        

    }

    //修改
    string shifou = "";
    int set_dr2_paixu = 0;
    int ppx = 1;
    public void set_dr2(int t2, int j, DataTable dt1, string id)
    {

        DropDownList dr = new DropDownList();
        dr = (DropDownList)Page.FindControl("DropDownList" + t2);

        dr.Items.Insert(j, dt1.Rows[j]["u1"].ToString());
        dr.Items[j].Value = dt1.Rows[j]["id"].ToString();

        if (id == dt1.Rows[j]["id"].ToString())
        {

            if (shifou == "")
            {
                for (int i = 0; i < dr.Items.Count; i++)
                {
                    dr.Items[i].Selected = false;
                }
                shifou = "yes";
                dr.Items[j].Selected = true;

                if (ppx == 1)
                {
                    //Response.Write("<script> window.parent.document.getElementById('" + Request.QueryString["textBox"].ToString() + "').value='" + dr.SelectedValue + "';</script>");
                    //Response.Write("<script> if(window.parent.document.getElementById('dingmulu')){window.parent.document.getElementById('dingmulu').value='" + my_b.get_ding_dir(dr.SelectedValue) + "'};</script>");
                }
                ppx = ppx + 1;
            }

        }
        else
        {
            shifou = "";
        }

        dr.Visible = true;
        if (set_dr2_paixu == 0)
        {

            //Response.Write("<script> window.parent.document.getElementById('" + Request.QueryString["textBox"].ToString() + "').value='" + dr.SelectedValue + "';</script>");
            //Response.Write("<script> if(window.parent.document.getElementById('dingmulu')){window.parent.document.getElementById('dingmulu').value='" + my_b.get_ding_dir(dr.SelectedValue) + "'};</script>");


            set_dr2_paixu = set_dr2_paixu + 1;
        }

    }
    string neirong = "";
    int neirongjishu = 0;

    public void getjishu(string t1)
    {
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + t1 + "");

        if (dt1.Rows.Count > 0)
        {
            neirongjishu = neirongjishu + 1;
            if (dt1.Rows[0]["classid"].ToString() != "0")
            {
                getjishu(dt1.Rows[0]["classid"].ToString());
            }
        }

    }
    string notclassid = "";
    public void dr2(string t1)
    {
        DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + t1 + "");

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["classid"].ToString() != "0")
            {
                DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=" + dt.Rows[0]["classid"].ToString() + "");
                if (dt1.Rows.Count > 0)
                {
                    neirongjishu = neirongjishu - 1;
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        if (j == 0)
                        {

                            // qingkong(t2);
                        }

                        //       Response.Write(dt.Rows[0]["classid"].ToString() + "|"+neirongjishu+"<br>");

                        set_dr2(neirongjishu, j, dt1, t1);

                    }



                }
                DropDownList dr = new DropDownList();
                dr = (DropDownList)Page.FindControl("DropDownList" + neirongjishu);
                dr.Items.Insert(0, "选择模板");
                dr.Items[0].Value = "|" + dt.Rows[0]["classid"].ToString();
                dr2(dt.Rows[0]["classid"].ToString());



            }

        }


    }
    //end
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                notclassid = Request.QueryString["notclassid"].ToString();
            }
            catch { }
            string classid = my_b.c_string(Request.QueryString["classid"].ToString());
            try
            {
                neirong = my_b.c_string(Request.QueryString["neirong"].ToString());
            }
            catch { }
            if (neirong == "")
            {
                dr1(classid, 0);
                Response.Write("<script> window.parent.document.getElementById('" + Request.QueryString["textBox"].ToString() + "').value='" + classid + "';</script>");
            }
            else
            {
                getjishu(neirong);

                dr2(neirong);

            }


        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList1.SelectedValue, 1);
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList2.SelectedValue, 2);
    }
    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList3.SelectedValue, 3);
    }
    protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList4.SelectedValue, 4);
    }
    protected void DropDownList5_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList5.SelectedValue, 5);
    }
    protected void DropDownList6_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList6.SelectedValue, 6);
    }
    protected void DropDownList7_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList7.SelectedValue, 7);
    }
    protected void DropDownList8_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList8.SelectedValue, 8);
    }
    protected void DropDownList9_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList9.SelectedValue, 9);
    }
    protected void DropDownList10_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList10.SelectedValue, 10);
    }
    protected void DropDownList11_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList11.SelectedValue, 11);
    }
    protected void DropDownList12_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList12.SelectedValue, 12);
    }
    protected void DropDownList13_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList13.SelectedValue, 13);
    }
    protected void DropDownList14_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList14.SelectedValue, 14);
    }
    protected void DropDownList15_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList15.SelectedValue, 15);
    }
    protected void DropDownList16_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList16.SelectedValue, 16);
    }
    protected void DropDownList17_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList17.SelectedValue, 17);
    }
    protected void DropDownList18_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList18.SelectedValue, 18);
    }
    protected void DropDownList19_SelectedIndexChanged(object sender, EventArgs e)
    {
        dr1(DropDownList19.SelectedValue, 19);
    }

}
