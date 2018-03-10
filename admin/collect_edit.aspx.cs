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
using System.IO;
public partial class admin_collect_edit : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string type = "";
    int jj = 0;
    public void dr1(string t1, int t2,string t3)
    {

        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + " and Model_id=" + t3 + "");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "—";
                }
                DropDownList2.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                DropDownList2.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1,t3);
            }
        }

    }
    public string set_5(string u12,string u11)
    {

        string u12_b = u12;
        u12 = get_5(u12);
        Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
        string[] aa = reg.Split(u12);
        string t1 = "";
        for (int i = 0; i < aa.Length; i++)
        {
            DataTable dt = new DataTable();
            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + aa[i].ToString().Split(',')[0].ToString());
            if (dt.Rows[0]["u6"].ToString() == "编辑器")
            {
                if (get_5_value(u12_b, 4, aa[i].ToString().Split(',')[0].ToString()) != "")
                {
                    t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_type\">" + dt.Rows[0]["u6"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 2, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr>            <tr>                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "右边代码 ：</td>                <td colspan=\"2\">                    <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 3, aa[i].ToString().Split(',')[0].ToString()) + "</textarea>                </td>            </tr>  <tr>                <td class=\"tRight\">内容分页类型：</td>                <td colspan=\"2\"><input type=\"radio\" name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" value=\"1\" id=\"RadioGroup1_0\" onclick=\"feiye_fenye(" + aa[i].ToString().Split(',')[1].ToString() + ")\"  checked=\"checked\"/>不采集内容分页&nbsp;&nbsp;&nbsp;&nbsp; <input type=\"radio\" name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" value=\"1\" id=\"RadioGroup1_0\" onclick=\"feiye_fenye(" + aa[i].ToString().Split(',')[1].ToString() + ")\" checked=\"checked\"/>从内容中获取分页URL</td>            </tr> <tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_1\" >                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "内容分页左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 4, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_2\" >                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "内容分页右边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_2\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 5, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_3\" >                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "目标链接左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_3\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 6, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_4\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "目标链接右边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_4\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 7, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr>";
                }
                else
                {
                    t1 = t1 + " <tr>                <td class=\"tRight\"><span style='display:none' id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_type\">" + dt.Rows[0]["u6"].ToString() + "</span>                    " + dt.Rows[0]["u2"].ToString() + "左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 2, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr>            <tr>                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "右边代码 ：</td>                <td colspan=\"2\">                    <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 3, aa[i].ToString().Split(',')[0].ToString()) + "</textarea>                </td>            </tr>  <tr>                <td class=\"tRight\">内容分页类型：</td>                <td colspan=\"2\"><input type=\"radio\" name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" value=\"1\" id=\"RadioGroup1_0\" onclick=\"feiye_fenye(" + aa[i].ToString().Split(',')[1].ToString() + ")\"  checked=\"checked\"/>不采集内容分页&nbsp;&nbsp;&nbsp;&nbsp; <input type=\"radio\" name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r\" value=\"1\" id=\"RadioGroup1_0\" onclick=\"feiye_fenye(" + aa[i].ToString().Split(',')[1].ToString() + ")\"/>从内容中获取分页URL</td>            </tr> <tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_1\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "内容分页左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 4, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_2\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "内容分页右边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_2\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 5, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_3\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "目标链接左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_3\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 6, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr><tr id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_d_4\" style=\"display:none\">                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "目标链接右边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_1\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_r_4\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 7, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr>";
                }
            }
            else
            {
                t1 = t1 + "<span style='display:none' id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_type\">" + dt.Rows[0]["u6"].ToString() + "</span> <tr>                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "左边代码 ：</td>                <td colspan=\"2\">                <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_qian\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 2, aa[i].ToString().Split(',')[0].ToString()) + "</textarea></td></tr>            <tr>                <td class=\"tRight\">                    " + dt.Rows[0]["u2"].ToString() + "右边代码 ：</td>                <td colspan=\"2\">                    <textarea name=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" id=\"div_Fields" + aa[i].ToString().Split(',')[1].ToString() + "_hou\" cols=\"45\" rows=\"5\">" + get_5_value(u12_b, 3, aa[i].ToString().Split(',')[0].ToString()) + "</textarea>                </td>            </tr>";
            }
        }
        return "<table class=\"cTable_2 table\">            <tr class=\"cTitle toolbarBg\">                <td width=\"25%\">                    <div>                        内容页设置</div>                </td>                <td colspan=\"2\">                </td>            </tr>" + t1 + "</table>";
    }
    public string get_5(string u12)
    {
        string t1 = "";
        Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
        string[] aa = reg.Split(u12);
        for (int i = 0; i < aa.Length; i++)
        {
            Regex reg1 = new Regex("{fzw:Field}", RegexOptions.Singleline);
            string[] bb = reg1.Split(aa[i].ToString());
            if (t1 == "")
            {
                t1 = bb[0].ToString() + "," + bb[1].ToString();
            }
            else
            {
                t1 = t1 + "{fzw:che}" + bb[0].ToString() + "," + bb[1].ToString();
            }
        }
        return t1;
    }
    public string get_5_value(string u12,int j,string t1)
    {
        try
        {
            Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
            string[] aa = reg.Split(u12);
            for (int i = 0; i < aa.Length; i++)
            {
                Regex reg1 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                string[] bb = reg1.Split(aa[i].ToString());
                if (bb[0].ToString() == t1)
                {
                    return bb[j].ToString();
                }
            }
        }
        catch
        { }
        return "";
    }
    public string set_1 = " style=\"display:none\"";
    public string set_che(string u12,DataTable dt)
    {
        string t1 = "";
        string t2 = "";
        Regex reg = new Regex("{fzw:che}", RegexOptions.Singleline);
        string[] aa = reg.Split(u12);
        for (int i = 0; i < aa.Length; i++)
        {
            if (t2 == "")
            {
                t2 = aa[i].ToString().Substring(0, aa[i].ToString().IndexOf("{fzw:Field}"));
            }
            else
            {
                t2 = t2+"|"+aa[i].ToString().Substring(0, aa[i].ToString().IndexOf("{fzw:Field}"));
            }
        }

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if (t2.IndexOf(dt.Rows[j]["id"].ToString()) > -1)
            {
                t1 = t1 + "<input type=\"checkbox\" name=\"che_Fields\" value=\"" + dt.Rows[j]["u2"].ToString() + "\"  checked=\"checked\"/><span>" + dt.Rows[j]["u2"].ToString() + "</span><span style='display:none' id='div_Fields" + j.ToString() + "'>" + dt.Rows[j]["id"].ToString() + "</span>&nbsp;&nbsp;";
            }
            else
            {
                t1 = t1 + "<input type=\"checkbox\" name=\"che_Fields\" value=\"" + dt.Rows[j]["u2"].ToString() + "\"/><span>" + dt.Rows[j]["u2"].ToString() + "</span><span style='display:none' id='div_Fields" + j.ToString() + "'>" + dt.Rows[j]["id"].ToString() + "</span>&nbsp;&nbsp;";
            }
        }
        return t1;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable dt = new DataTable();
            int i = 0;



            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            feidaoru.Visible = true;
            daoru.Visible = false;
            if (type == "daoru")
            {
                feidaoru.Visible = false;
                daoru.Visible = true;
            }
            else if (type == "0")
            {
                my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect set u18=1 where id=" + Request.QueryString["id"].ToString());
                Response.Redirect("err.aspx?err=为零采集项目成功！马上跳转到采集项目列表页面！&errurl=" + my_b.tihuan("collect_list.aspx", "&", "fzw123") + "");
            }
            else if (type == "edit")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect where id=" + Request.QueryString["id"].ToString());
                string u1 = dt.Rows[0]["u1"].ToString();
                string u2 = dt.Rows[0]["u2"].ToString();
                string u3 = dt.Rows[0]["u3"].ToString();
                string u4 = dt.Rows[0]["u4"].ToString();
                string u5 = dt.Rows[0]["u5"].ToString();
                string u6 = dt.Rows[0]["u6"].ToString();
                string u7 = dt.Rows[0]["u7"].ToString();
                string u8 = dt.Rows[0]["u8"].ToString();
                string u9 = dt.Rows[0]["u9"].ToString();
                string u10 = dt.Rows[0]["u10"].ToString();
                string u11 = dt.Rows[0]["u11"].ToString();
                string u12 = dt.Rows[0]["u12"].ToString();
                string u13 = dt.Rows[0]["u13"].ToString();
                string u14 = dt.Rows[0]["u14"].ToString();
                string u15 = dt.Rows[0]["u15"].ToString();
                string u16 = dt.Rows[0]["u16"].ToString();
                string u17 = dt.Rows[0]["u17"].ToString();
                string u19 = dt.Rows[0]["u19"].ToString();
                if (u4 == "批量指定分页URL代码")
                {
                    set_1 = "";
                }
                this.TextBox1.Text = u1;
                for (i = 0; i < RadioButtonList1.Items.Count; i++)
                {
                    if (RadioButtonList1.Items[i].Value == u2)
                    {
                        RadioButtonList1.Items[i].Selected = true;
                    }
                }
                this.TextBox2.Text = u3;
                this.TextBox19.Text = u19;
                for (i = 0; i < RadioButtonList2.Items.Count; i++)
                {
                    if (RadioButtonList2.Items[i].Value == u4)
                    {
                        RadioButtonList2.Items[i].Selected = true;
                    }
                }
                this.TextBox3.Text = u5;
                Regex reg = new Regex("{fzw:Field}", RegexOptions.Singleline);
                string[] aa = reg.Split(u6);
                try
                {
                    this.TextBox4.Text = aa[0].ToString();
                    this.TextBox5.Text = aa[1].ToString();
                }
                catch
                { }
                this.TextBox6.Text = u7;
                this.TextBox7.Text = u8;
                this.TextBox8.Text = u9;
                this.TextBox9.Text = u10;

                DropDownList1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model order by id");
                DropDownList1.DataTextField = "u2";
                DropDownList1.DataValueField = "id";
                DropDownList1.DataBind();
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == u11)
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }

                if (DropDownList1.SelectedItem.Value == "")
                {
                    Response.Redirect("err.aspx?err=模型不可以为空！&errurl=" + my_b.tihuan("Model.aspx", "&", "fzw123") + "");
                }
                dr1("0", 0, DropDownList1.SelectedItem.Value);

                for (i = 0; i < DropDownList2.Items.Count; i++)
                {
                    if (DropDownList2.Items[i].Value == u17)
                    {
                        DropDownList2.Items[i].Selected = true;
                    }
                }
                Regex reg1 = new Regex("{fzw:che}", RegexOptions.Singleline);
                string[] bb = reg1.Split(u13);
                try
                {
                    Regex reg2 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] cc = reg2.Split(bb[0].ToString());
                    this.TextBox10.Text = cc[0].ToString();
                    this.TextBox11.Text = cc[1].ToString();

                    Regex reg3 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] dd = reg3.Split(bb[1].ToString());
                    this.TextBox12.Text = dd[0].ToString();
                    this.TextBox13.Text = dd[1].ToString();

                    Regex reg4 = new Regex("{fzw:Field}", RegexOptions.Singleline);
                    string[] ee = reg4.Split(bb[2].ToString());
                    this.TextBox14.Text = ee[0].ToString();
                    this.TextBox15.Text = ee[1].ToString();
                }
                catch
                { }

                Regex reg5 = new Regex("{fzw:che}", RegexOptions.Singleline);
                string[] ff = reg5.Split(u12);

                dt = my_c.GetTable("select u2,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + DropDownList1.SelectedItem.Value + " order by u9 , id");
                Label3.Text =  set_che(u12, dt);
                Label4.Text = set_5(u12, u11);



                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=15");
                string t1 = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (u14.IndexOf(dt.Rows[i]["id"].ToString()) > -1)
                    {
                        t1 = t1 + "<input type=\"checkbox\" name=\"che_html\" value=\"" + dt.Rows[i]["id"].ToString() + "\"  checked=\"checked\"/><span>" + dt.Rows[i]["u1"].ToString() + "</span><br>";
                    }
                    else
                    {
                        t1 = t1 + "<input type=\"checkbox\" name=\"che_html\" value=\"" + dt.Rows[i]["id"].ToString() + "\" /><span>" + dt.Rows[i]["u1"].ToString() + "</span><br>";
                    }
                }
                Label7.Text = t1;


                for (i = 0; i < CheckBoxList2.Items.Count; i++)
                {
                    if (u15.IndexOf(CheckBoxList2.Items[i].Value) > -1)
                    {
                        CheckBoxList2.Items[i].Selected = true;
                    }
                    else
                    {
                        CheckBoxList2.Items[i].Selected = false;
                    }
                }
                for (i = 0; i < RadioButtonList3.Items.Count; i++)
                {
                    if (RadioButtonList3.Items[i].Value == u16)
                    {
                        RadioButtonList3.Items[i].Selected = true;
                    }
                }






            }
            else if (type == "del")
            {
                my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect where id in(" + Request.QueryString["id"].ToString() + ")");
                Response.Redirect("err.aspx?err=删除采集项目成功！马上跳转到采集项目列表页面！&errurl=" + my_b.tihuan("collect_list.aspx", "&", "fzw123") + "");
            }
            else if (type == "copy")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect where id=" + Request.QueryString["id"].ToString());
                my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect (u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,u12,u13,u14,u15,u16,u17,u18,u19) values('重命名_" + my_b.c_string(dt.Rows[0]["u1"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u2"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u3"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u4"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u5"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u6"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u7"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u8"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u9"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u10"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u11"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u12"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u13"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u14"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u15"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u16"].ToString()) + "','" + my_b.c_string(dt.Rows[0]["u17"].ToString()) + "'," + my_b.c_string(dt.Rows[0]["u18"].ToString()) + ",'" + my_b.c_string(dt.Rows[0]["u19"].ToString()) + "')");
                Response.Redirect("err.aspx?err=复制采集项目成功！马上跳转到采集项目列表页面！&errurl=" + my_b.tihuan("collect_list.aspx", "&", "fzw123") + "");
            }
            else
            {

                DropDownList1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u3 like '文章模型' order by id");
                DropDownList1.DataTextField = "u2";
                DropDownList1.DataValueField = "id";
                DropDownList1.DataBind();

                if (DropDownList1.SelectedItem.Value == "")
                {
                    Response.Redirect("err.aspx?err=模型不可以为空！&errurl=" + my_b.tihuan("Model.aspx", "&", "fzw123") + "");
                }
                dr1("0", 0, DropDownList1.SelectedItem.Value);

                dt = my_c.GetTable("select u2,id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + DropDownList1.SelectedItem.Value + " order by u9 , id");
                string t1 = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    if (i > 0)
                    {
                        if (i % 6 == 0)
                        {
                            t1 = t1 + "<br>";
                        }
                    }

                    t1 = t1 + "<input type=\"checkbox\" name=\"che_Fields\" value=\"" + dt.Rows[i]["u2"].ToString() + "\" /><span>" + dt.Rows[i]["u2"].ToString() + "</span><span style='display:none' id='div_Fields" + i.ToString() + "'>" + dt.Rows[i]["id"].ToString() + "</span>&nbsp;&nbsp;";
                }
                Label3.Text = t1;

                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=15");
                t1 = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    t1 = t1 + "<input type=\"checkbox\" name=\"che_html\" value=\"" + dt.Rows[i]["id"].ToString() + "\" /><span>" + dt.Rows[i]["u1"].ToString() + "</span><br>";
                }
                Label7.Text = t1;


            }

        }
    }


    protected void Button17_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string u2 = RadioButtonList1.SelectedItem.Value;
        string u3 = my_b.c_string(this.TextBox2.Text);
        string u4 = RadioButtonList2.SelectedItem.Value;
        string u5 = my_b.c_string(this.TextBox3.Text);
        string u6 = my_b.c_string(this.TextBox4.Text + "{fzw:Field}" + this.TextBox5.Text);
        string u7 = my_b.c_string(this.TextBox6.Text);
        string u8 = my_b.c_string(this.TextBox7.Text);
        string u9 = my_b.c_string(this.TextBox8.Text);
        string u10 = my_b.c_string(this.TextBox9.Text);
        string u11 = DropDownList1.SelectedItem.Value;
        string u12 = my_b.c_string(Server.HtmlDecode(this.TextBox16.Text));
        string u13 = my_b.c_string(Server.HtmlDecode(this.TextBox17.Text));
        string u14 = my_b.c_string(Server.HtmlDecode(this.TextBox18.Text));
        string u19 = my_b.c_string(Server.HtmlDecode(this.TextBox19.Text));
        string u15 = "";
        for (int i = 0; i < CheckBoxList2.Items.Count; i++)
        {
            if (CheckBoxList2.Items[i].Selected)
            {
                if (u15 == "")
                {
                    u15 = CheckBoxList2.Items[i].Value;
                }
                else
                {
                    u15 = u15 + "{fzw:Field}" + CheckBoxList2.Items[i].Value;
                }
            }
        }
        string u16 = RadioButtonList3.SelectedItem.Value;
        string u17 = DropDownList2.SelectedItem.Value;

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {

            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',u5='" + u5 + "',u6='" + u6 + "',u7='" + u7 + "',u8='" + u8 + "',u9='" + u9 + "',u10='" + u10 + "',u11='" + u11 + "',u12='" + u12 + "',u13='" + u13 + "',u14='" + u14 + "',u15='" + u15 + "',u16='" + u16 + "',u17='" + u17 + "',u18=1,u19='" + u19 + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改采集项目成功！马上跳转到采集项目列表页面！&errurl=" + my_b.tihuan("collect_list.aspx", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "collect(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,u12,u13,u14,u15,u16,u17,u19,u18) values('" + u1 + "','" + u2 + "','" + u3 + "','" + u4 + "','" + u5 + "','" + u6 + "','" + u7 + "','" + u8 + "','" + u9 + "','" + u10 + "','" + u11 + "','" + u12 + "','" + u13 + "','" + u14 + "','" + u15 + "','" + u16 + "','" + u17 + "','" + u19 + "',1)");
            Response.Redirect("err.aspx?err=增加采集项目成功！马上跳转到采集项目列表页面！&errurl=" + my_b.tihuan("collect_list.aspx", "&", "fzw123") + "");
        }
      






    }
    protected void Button21_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string get_file = my_b.shangchuang(FileUpload1, ".txt");
        if (get_file == "1")
        {
            Literal1.Text = "文件类型不正确";
        }
        else if (get_file == "2")
        {
            Literal1.Text = "文件太大或者不存在";
        }
        else
        {
            my_c.genxin(File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath+get_file, System.Text.Encoding.UTF8));
            Response.Redirect("err.aspx?err=导入采集规则成功！马上跳转到采集项目列表页面！&errurl=" + my_b.tihuan("collect_list.aspx", "&", "fzw123") + "");
           
        }
    }
}
