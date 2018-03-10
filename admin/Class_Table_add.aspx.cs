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
public partial class admin_Class_Table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    DataTable dt = new DataTable();
    int jj = 0;
    int i = 0;

    public void dr1(string t1, int t2)
    {

        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + " and Model_id=" + Request.QueryString["Model_id"].ToString() + "");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "— ";
                }
                DropDownList3.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                DropDownList3.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1);
            }
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            string[] file_arr = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString());
            for (i = 0; i < file_arr.Length; i++)
            {
                DropDownList1.Items.Insert(0, file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1));
                DropDownList1.Items[0].Value = file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1);
                DropDownList2.Items.Insert(0, file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1));
                DropDownList2.Items[0].Value = file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1);
            }
            dr1("0", 0);
            DropDownList3.Items.Insert(0, "顶级目录");
            DropDownList3.Items[0].Value = "0";
            DropDownList1.Items.Insert(0, "选择模板");
            DropDownList1.Items[0].Value = "";
            DropDownList2.Items.Insert(0, "选择模板");
            DropDownList2.Items[0].Value = "";



            if (type == "edit")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                this.TextBox2.Text = dt.Rows[0]["u2"].ToString();
                this.TextBox3.Text = dt.Rows[0]["u3"].ToString();
                this.TextBox4.Text = dt.Rows[0]["u4"].ToString();
                this.TextBox5.Text = dt.Rows[0]["u7"].ToString();
                this.TextBox6.Text = dt.Rows[0]["u8"].ToString();
                this.TextBox7.Text = dt.Rows[0]["u10"].ToString();
                this.TextBox8.Text = dt.Rows[0]["paixu"].ToString();
                this.TextBox9.Text = dt.Rows[0]["seotitle"].ToString();

                FreeTextBox1.Text = dt.Rows[0]["u9"].ToString();
                if (dt.Rows[0]["type"].ToString() == "主栏目")
                {
                    CheckBox1.Checked = true;
                }
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == dt.Rows[0]["u5"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
                for (i = 0; i < DropDownList2.Items.Count; i++)
                {
                    if (DropDownList2.Items[i].Value == dt.Rows[0]["u6"].ToString())
                    {
                        DropDownList2.Items[i].Selected = true;
                    }
                }

                for (i = 0; i < DropDownList3.Items.Count; i++)
                {
                    if (DropDownList3.Items[i].Value == dt.Rows[0]["Sort_id"].ToString())
                    {
                        DropDownList3.Items[i].Selected = true;
                    }
                }

            }
            else if (type == "sc")
            {

            }
            else if (type == "del")
            {
                if (Request.QueryString["id"].ToString() == "")
                {
                    my_b.tiaozhuan("请选择要删除的信息！", Request.UrlReferrer.ToString());
                }
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id in (" + Request.QueryString["id"].ToString() + ")");

                if (dt.Rows.Count > 0)
                {
                    my_b.tiaozhuan("对不起还有下级分类，如果想删除分类必须从最底级分类开始删除！", Request.UrlReferrer.ToString());
                }
                else
                {
                    dt = my_c.GetTable("select * from " + my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString() + "").Rows[0]["u1"].ToString() + " where classid in(" + Request.QueryString["id"].ToString() + ")");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable dt_Field = my_c.GetTable("select u6,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString());
                        for (int j = 0; j < dt_Field.Rows.Count; j++)
                        {
                            if (dt_Field.Rows[j]["u6"].ToString() == "编辑器")
                            {
                                my_b.del_article_pic(dt.Rows[i][dt_Field.Rows[j]["u1"].ToString()].ToString());
                            }
                            if (dt_Field.Rows[j]["u6"].ToString() == "缩略图")
                            {
                                my_b.del_pic(dt.Rows[i][dt_Field.Rows[j]["u1"].ToString()].ToString());
                            }
                            if (dt_Field.Rows[j]["u6"].ToString() == "文件框")
                            {
                                my_b.del_pic(dt.Rows[i][dt_Field.Rows[j]["u1"].ToString()].ToString());
                            }
                        }

                        my_c.genxin("delete from " + my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString() + "").Rows[0]["u1"].ToString() + " where id =" + dt.Rows[i]["id"].ToString());
                    }
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + Request.QueryString["id"].ToString() + ")");
                    my_b.tiaozhuan("删除栏目及列表成功！", "articles.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "");
                }

            }
            else
            {

                for (i = 0; i < DropDownList3.Items.Count; i++)
                {
                    if (DropDownList3.Items[i].Value == Request.QueryString["classid"].ToString())
                    {
                        DropDownList3.Items[i].Selected = true;
                    }
                }
                if (Request.QueryString["classid"].ToString() == "0")
                {
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < DropDownList1.Items.Count; i++)
                        {
                            if (DropDownList1.Items[i].Value == dt.Rows[0]["u4"].ToString())
                            {
                                DropDownList1.Items[i].Selected = true;
                            }
                        }
                        for (i = 0; i < DropDownList2.Items.Count; i++)
                        {
                            if (DropDownList2.Items[i].Value == dt.Rows[0]["u5"].ToString())
                            {
                                DropDownList2.Items[i].Selected = true;
                            }
                        }
                        this.TextBox5.Text = dt.Rows[0]["u6"].ToString();
                        this.TextBox6.Text = dt.Rows[0]["u7"].ToString();

                    }
                }
                else
                {
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + DropDownList3.SelectedItem.Value + "");
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < DropDownList1.Items.Count; i++)
                        {
                            if (DropDownList1.Items[i].Value == dt.Rows[0]["u5"].ToString())
                            {
                                DropDownList1.Items[i].Selected = true;
                            }
                        }
                        for (i = 0; i < DropDownList2.Items.Count; i++)
                        {
                            if (DropDownList2.Items[i].Value == dt.Rows[0]["u6"].ToString())
                            {
                                DropDownList2.Items[i].Selected = true;
                            }
                        }
                        this.TextBox6.Text = dt.Rows[0]["u8"].ToString();
                        this.TextBox5.Text = dt.Rows[0]["u7"].ToString();
                        this.TextBox7.Text = dt.Rows[0]["u10"].ToString();
                    }
                }

            }
        }
    }


    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + DropDownList3.SelectedItem.Value + "");
        if (dt.Rows.Count > 0)
        {

            for (i = 0; i < DropDownList1.Items.Count; i++)
            {
                DropDownList1.Items[i].Selected = false;
                if (DropDownList1.Items[i].Value == dt.Rows[0]["u5"].ToString())
                {
                    DropDownList1.Items[i].Selected = true;
                }
            }
            for (i = 0; i < DropDownList2.Items.Count; i++)
            {
                DropDownList2.Items[i].Selected = false;
                if (DropDownList2.Items[i].Value == dt.Rows[0]["u6"].ToString())
                {
                    DropDownList2.Items[i].Selected = true;
                }
            }
            this.TextBox6.Text = dt.Rows[0]["u8"].ToString();
            this.TextBox5.Text = dt.Rows[0]["u7"].ToString();
        }
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string u2 = my_b.c_string(this.TextBox2.Text);
        string u3 = my_b.c_string(this.TextBox3.Text);
        string u4 = my_b.c_string(this.TextBox4.Text);
        string u5 = DropDownList1.SelectedItem.Value;
        string u6 = DropDownList2.SelectedItem.Value;
        string u7 = my_b.c_string(this.TextBox5.Text).Replace(" ", "");
        string u8 = this.TextBox6.Text;
        string u9 = my_b.c_string(this.FreeTextBox1.Text);
        string Sort_id = DropDownList3.SelectedItem.Value;
        string Model_id = Request.QueryString["Model_id"].ToString();
        string u10 = this.TextBox7.Text;
        string seotitle = my_b.c_string(this.TextBox9.Text);
        string type1 = "";
        if (CheckBox1.Checked)
        {
            type1 = "主栏目";
        }
        string paixu = my_b.c_string(this.TextBox8.Text);
        if (paixu == "")
        {
            paixu = "0";
        }
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }
        if (type == "edit")
        {
            #region 栏目所属文章路径处理
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            DataTable dt = my_c.GetTable("select * from " + biaoming + " where classid=" + Request.QueryString["id"].ToString() + "");
 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime dtime = DateTime.Parse(dt.Rows[i]["dtime"].ToString());
                string Filepath = u7 + dtime.Year.ToString() + dtime.Month.ToString() + dtime.Day.ToString() + "/";


                my_c.genxin("update " + biaoming + " set Filepath='" + Filepath + "' where id=" + dt.Rows[i]["id"].ToString() + "");
            }
            #endregion
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',u5='" + u5 + "',u6='" + u6 + "',u7='" + u7 + "',u8='" + u8 + "',u9='" + u9 + "',u10='" + u10 + "',seotitle='" + seotitle + "',paixu=" + paixu + ",Sort_id=" + Sort_id + ",Model_id=" + Model_id + ",type='" + type1 + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改栏目成功！马上跳转到栏目管理页面！&errurl=" + my_b.tihuan("Class_Table.aspx?Model_id=" + Model_id + "", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,seotitle,paixu,Sort_id,Model_id,type) values('" + u1 + "','" + u2 + "','" + u3 + "','" + u4 + "','" + u5 + "','" + u6 + "','" + u7 + "','" + u8 + "','" + u9 + "','" + u10 + "','" + seotitle + "'," + paixu + "," + Sort_id + "," + Model_id + ",'" + type1 + "')");
            Response.Redirect("err.aspx?err=添加栏目成功！马上跳转到栏目管理页面！&errurl=" + my_b.tihuan("Class_Table.aspx?Model_id=" + Model_id + "&classid=" + Sort_id + "", "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        string u1 = this.TextBox1.Text;
        if (u1 != "")
        {
            string file_path = this.TextBox5.Text + "/" + my_b.hanzi(u1, "_") + "/";
            this.TextBox5.Text = file_path.Replace("//", "/");
        }
    }
}
