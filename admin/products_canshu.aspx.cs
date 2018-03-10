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
public partial class admin_Fields_add : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    int i = 0;
    DataTable dt1 = new DataTable();
    DataTable dt2 = new DataTable();
    int z_hang = 0;
    //设置颜色显示
    public string str_color(string yansezhileixing, string yansezhi)
    {
        string color_str = "";
        if (yansezhileixing == "代码")
        {
            color_str = "<div style=\" width:75px; height:25px; background-color:" + yansezhi + "; float:left\"></div>";
        }
        else
        {
            color_str = "<img src='" + yansezhi + "'  style=\" float:left\"/>";
        }
        return color_str;
    }
        //end
    //获取规格
    public void get_guige(string chanpinbianhao)
    {

        DataTable yanse_dt = my_c.GetTable("select * from sl_yanse where chanpinbianhao=" + chanpinbianhao + "");
        DataTable chicun_dt = my_c.GetTable("select * from sl_chicun where chanpinbianhao=" + chanpinbianhao + "");
        if (yanse_dt.Rows.Count > 0 && chicun_dt.Rows.Count > 0)
        {  //颜色和尺寸都有数据
            z_hang = 0;
            for (int i = 0; i < yanse_dt.Rows.Count; i++)
            {

                for (int j = 0; j < chicun_dt.Rows.Count; j++)
                {
                    z_hang = z_hang + 1;

                    string yansebianhao = yanse_dt.Rows[i]["yanseming"].ToString();
                    string chicunbianhao = chicun_dt.Rows[j]["chicunming"].ToString();
                    string jiage = "";
                    string shuliang = "";
                    string shangjiabianma = "";
          
                    TextBox txt1 = new TextBox();
                    
                    txt1 = (TextBox)Page.FindControl("jiage" + z_hang);
                    jiage = txt1.Text;

                    TextBox txt2 = new TextBox();
                    txt2 = (TextBox)Page.FindControl("shuliang" + z_hang);
                    shuliang = txt2.Text;

                    TextBox txt3 = new TextBox();
                    txt3 = (TextBox)Page.FindControl("shangjiabianma" + z_hang);
                    shangjiabianma = txt3.Text;

                    string sql = "insert into sl_jiage (yansebianhao,chicunbianhao,jiage,shuliang,shangjiabianma,chanpinbianhao) values('" + yansebianhao + "','" + chicunbianhao + "'," + jiage + "," + shuliang + ",'" + shangjiabianma + "'," + chanpinbianhao + ")";
                    my_c.genxin(sql);
                }
            }
            //
        }
        else if (yanse_dt.Rows.Count > 0 )
        {  //颜色有数据
            z_hang = 0;
            for (int i = 0; i < yanse_dt.Rows.Count; i++)
            {

                z_hang = z_hang + 1;

                string yansebianhao = yanse_dt.Rows[i]["yanseming"].ToString();
                string chicunbianhao = "";
                string jiage = "";
                string shuliang = "";
                string shangjiabianma = "";

                TextBox txt1 = new TextBox();

                txt1 = (TextBox)Page.FindControl("jiage" + z_hang);
                jiage = txt1.Text;

                TextBox txt2 = new TextBox();
                txt2 = (TextBox)Page.FindControl("shuliang" + z_hang);
                shuliang = txt2.Text;

                TextBox txt3 = new TextBox();
                txt3 = (TextBox)Page.FindControl("shangjiabianma" + z_hang);
                shangjiabianma = txt3.Text;

                string sql = "insert into sl_jiage (yansebianhao,chicunbianhao,jiage,shuliang,shangjiabianma,chanpinbianhao) values('" + yansebianhao + "','" + chicunbianhao + "'," + jiage + "," + shuliang + ",'" + shangjiabianma + "'," + chanpinbianhao + ")";
                my_c.genxin(sql);
            }
            //
        }
        else  if (chicun_dt.Rows.Count > 0)
        {  //颜色和尺寸都有数据
            z_hang = 0;
           for (int j = 0; j < chicun_dt.Rows.Count; j++)
                {
                    z_hang = z_hang + 1;

                    string yansebianhao = "";
                    string chicunbianhao = chicun_dt.Rows[j]["chicunming"].ToString();
                    string jiage = "";
                    string shuliang = "";
                    string shangjiabianma = "";
          
                    TextBox txt1 = new TextBox();
                    
                    txt1 = (TextBox)Page.FindControl("jiage" + z_hang);
                    jiage = txt1.Text;

                    TextBox txt2 = new TextBox();
                    txt2 = (TextBox)Page.FindControl("shuliang" + z_hang);
                    shuliang = txt2.Text;

                    TextBox txt3 = new TextBox();
                    txt3 = (TextBox)Page.FindControl("shangjiabianma" + z_hang);
                    shangjiabianma = txt3.Text;

                    string sql = "insert into sl_jiage (yansebianhao,chicunbianhao,jiage,shuliang,shangjiabianma,chanpinbianhao) values('" + yansebianhao + "','" + chicunbianhao + "'," + jiage + "," + shuliang + ",'" + shangjiabianma + "'," + chanpinbianhao + ")";
                    my_c.genxin(sql);
                }
            //
        }

    }
    //配置规格
    public void set_guige(string chanpinbianhao)
    {
        DataTable yanse_dt = my_c.GetTable("select * from sl_yanse where chanpinbianhao=" + chanpinbianhao + "");
        DataTable chicun_dt = my_c.GetTable("select * from sl_chicun where chanpinbianhao=" + chanpinbianhao + "");
        //颜色和尺寸都有数据
        if (yanse_dt.Rows.Count > 0 && chicun_dt.Rows.Count > 0)
        {
            TableRow top_r = new TableRow();
            TableCell top_c1 = new TableCell();
            top_c1.Controls.Add(new LiteralControl("<strong>颜色</strong>"));
            top_r.Cells.Add(top_c1);

            TableCell top_c2 = new TableCell();
            top_c2.Controls.Add(new LiteralControl("<strong>尺寸</strong>"));
            top_r.Cells.Add(top_c2);

            TableCell top_c3 = new TableCell();
            top_c3.Controls.Add(new LiteralControl("<strong>价格</strong>"));
            top_r.Cells.Add(top_c3);

            TableCell top_c4 = new TableCell();
            top_c4.Controls.Add(new LiteralControl("<strong>数量</strong>"));
            top_r.Cells.Add(top_c4);

            TableCell top_c5 = new TableCell();
            top_c5.Controls.Add(new LiteralControl("<strong>商家编码</strong>"));
            top_r.Cells.Add(top_c5);

            Table1.Rows.Add(top_r);
            for (int i = 0; i < yanse_dt.Rows.Count; i++)
            {

                for (int j = 0; j < chicun_dt.Rows.Count; j++)
                {
                    z_hang = z_hang + 1;
                    TableRow r = new TableRow();
                    TableCell c1 = new TableCell();
                    c1.Controls.Add(new LiteralControl(str_color(yanse_dt.Rows[i]["yansezhileixing"].ToString(), yanse_dt.Rows[i]["yansezhi"].ToString()) + "<div style=\" float:left\">" + yanse_dt.Rows[i]["yanseming"].ToString() + "</div>"));

                    r.Cells.Add(c1);

                    TableCell c2 = new TableCell();
                    c2.Controls.Add(new LiteralControl(chicun_dt.Rows[j]["chicunming"].ToString()));
                    r.Cells.Add(c2);

                    dt1 = my_c.GetTable("select * from sl_jiage where yansebianhao='" + yanse_dt.Rows[i]["yanseming"].ToString() + "' and chicunbianhao='" + chicun_dt.Rows[j]["chicunming"].ToString() + "' and chanpinbianhao=" + chanpinbianhao + "");
                    dt2 = my_c.GetTable("select * from sl_product where id=" + chanpinbianhao + "");

                    TableCell c3 = new TableCell();
                    TextBox txt1;
                    //创建TextBox 
                    txt1 = new TextBox();
                    txt1.ID = "jiage" + z_hang;
                    txt1.Width = 60;
                    if (dt1.Rows.Count > 0)
                    {
                        txt1.Text = dt1.Rows[0]["jiage"].ToString();
                    }
                    else
                    {
                        txt1.Text = dt2.Rows[0]["jifen"].ToString();
                    }
                    //添加控件到容器 
                    c3.Controls.Add(txt1);
                    r.Cells.Add(c3);

                    TableCell c4 = new TableCell();
                    TextBox txt2;
                    //创建TextBox 
                    txt2 = new TextBox();
                    txt2.ID = "shuliang" + z_hang;
                    txt2.Width = 60;
                    if (dt1.Rows.Count > 0)
                    {
                        txt2.Text = dt1.Rows[0]["shuliang"].ToString();
                    }
                    else
                    {
                        txt2.Text = dt2.Rows[0]["kucun"].ToString();
                    }
                    //添加控件到容器 
                    c4.Controls.Add(txt2);
                    r.Cells.Add(c4);

                    TableCell c5 = new TableCell();
                    TextBox txt3;
                    //创建TextBox 
                    txt3 = new TextBox();
                    txt3.ID = "shangjiabianma" + z_hang;
                    txt3.Width = 100;
                    if (dt1.Rows.Count > 0)
                    {
                        txt3.Text = dt1.Rows[0]["shangjiabianma"].ToString();
                    }
                    else
                    {
                        txt3.Text = "";
                    }
                    //添加控件到容器 
                    c5.Controls.Add(txt3);
                    r.Cells.Add(c5);


                    Table1.Rows.Add(r);
                }

            }
            //end
        }
        else if (yanse_dt.Rows.Count > 0)
        {
            TableRow top_r = new TableRow();
            TableCell top_c1 = new TableCell();
            top_c1.Controls.Add(new LiteralControl("<strong>颜色</strong>"));
            top_r.Cells.Add(top_c1);



            TableCell top_c3 = new TableCell();
            top_c3.Controls.Add(new LiteralControl("<strong>价格</strong>"));
            top_r.Cells.Add(top_c3);

            TableCell top_c4 = new TableCell();
            top_c4.Controls.Add(new LiteralControl("<strong>数量</strong>"));
            top_r.Cells.Add(top_c4);

            TableCell top_c5 = new TableCell();
            top_c5.Controls.Add(new LiteralControl("<strong>商家编码</strong>"));
            top_r.Cells.Add(top_c5);

            Table1.Rows.Add(top_r);

            //颜色有数据
            for (int i = 0; i < yanse_dt.Rows.Count; i++)
            {

                z_hang = z_hang + 1;
                TableRow r = new TableRow();
                TableCell c1 = new TableCell();
                c1.Controls.Add(new LiteralControl(yanse_dt.Rows[i]["yanseming"].ToString()));
                r.Cells.Add(c1);



                dt1 = my_c.GetTable("select * from sl_jiage where yansebianhao='" + yanse_dt.Rows[i]["yanseming"].ToString() + "' and chanpinbianhao=" + chanpinbianhao + "");
                dt2 = my_c.GetTable("select * from sl_product where id=" + chanpinbianhao + "");

                TableCell c3 = new TableCell();
                TextBox txt1;
                //创建TextBox 
                txt1 = new TextBox();
                txt1.ID = "jiage" + z_hang;
                txt1.Width = 60;
                if (dt1.Rows.Count > 0)
                {
                    txt1.Text = dt1.Rows[0]["jiage"].ToString();
                }
                else
                {
                    txt1.Text = dt2.Rows[0]["jifen"].ToString();
                }
                //添加控件到容器 
                c3.Controls.Add(txt1);
                r.Cells.Add(c3);

                TableCell c4 = new TableCell();
                TextBox txt2;
                //创建TextBox 
                txt2 = new TextBox();
                txt2.ID = "shuliang" + z_hang;
                txt2.Width = 60;
                if (dt1.Rows.Count > 0)
                {
                    txt2.Text = dt1.Rows[0]["shuliang"].ToString();
                }
                else
                {
                    txt2.Text = dt2.Rows[0]["kucun"].ToString();
                }
                //添加控件到容器 
                c4.Controls.Add(txt2);
                r.Cells.Add(c4);

                TableCell c5 = new TableCell();
                TextBox txt3;
                //创建TextBox 
                txt3 = new TextBox();
                txt3.ID = "shangjiabianma" + z_hang;
                txt3.Width = 100;
                if (dt1.Rows.Count > 0)
                {
                    txt3.Text = dt1.Rows[0]["shangjiabianma"].ToString();
                }
                else
                {
                    txt3.Text = "";
                }
                //添加控件到容器 
                c5.Controls.Add(txt3);
                r.Cells.Add(c5);


                Table1.Rows.Add(r);

            }
            //end
        }
        else if (chicun_dt.Rows.Count > 0)
        {
            TableRow top_r = new TableRow();


            TableCell top_c2 = new TableCell();
            top_c2.Controls.Add(new LiteralControl("<strong>尺寸</strong>"));
            top_r.Cells.Add(top_c2);

            TableCell top_c3 = new TableCell();
            top_c3.Controls.Add(new LiteralControl("<strong>价格</strong>"));
            top_r.Cells.Add(top_c3);

            TableCell top_c4 = new TableCell();
            top_c4.Controls.Add(new LiteralControl("<strong>数量</strong>"));
            top_r.Cells.Add(top_c4);

            TableCell top_c5 = new TableCell();
            top_c5.Controls.Add(new LiteralControl("<strong>商家编码</strong>"));
            top_r.Cells.Add(top_c5);

            Table1.Rows.Add(top_r);
            //尺寸有数据
            for (int j = 0; j < chicun_dt.Rows.Count; j++)
            {
                z_hang = z_hang + 1;
                TableRow r = new TableRow();


                TableCell c2 = new TableCell();
                c2.Controls.Add(new LiteralControl(chicun_dt.Rows[j]["chicunming"].ToString()));
                r.Cells.Add(c2);

                dt1 = my_c.GetTable("select * from sl_jiage where  chicunbianhao='" + chicun_dt.Rows[j]["chicunming"].ToString() + "' and chanpinbianhao=" + chanpinbianhao + "");
                dt2 = my_c.GetTable("select * from sl_product where id=" + chanpinbianhao + "");

                TableCell c3 = new TableCell();
                TextBox txt1;
                //创建TextBox 
                txt1 = new TextBox();
                txt1.ID = "jiage" + z_hang;
                txt1.Width = 60;
                if (dt1.Rows.Count > 0)
                {
                    txt1.Text = dt1.Rows[0]["jiage"].ToString();
                }
                else
                {
                    txt1.Text = dt2.Rows[0]["jifen"].ToString();
                }
                //添加控件到容器 
                c3.Controls.Add(txt1);
                r.Cells.Add(c3);

                TableCell c4 = new TableCell();
                TextBox txt2;
                //创建TextBox 
                txt2 = new TextBox();
                txt2.ID = "shuliang" + z_hang;
                txt2.Width = 60;
                if (dt1.Rows.Count > 0)
                {
                    txt2.Text = dt1.Rows[0]["shuliang"].ToString();
                }
                else
                {
                    txt2.Text = dt2.Rows[0]["kucun"].ToString();
                }
                //添加控件到容器 
                c4.Controls.Add(txt2);
                r.Cells.Add(c4);

                TableCell c5 = new TableCell();
                TextBox txt3;
                //创建TextBox 
                txt3 = new TextBox();
                txt3.ID = "shangjiabianma" + z_hang;
                txt3.Width = 100;
                if (dt1.Rows.Count > 0)
                {
                    txt3.Text = dt1.Rows[0]["shangjiabianma"].ToString();
                }
                else
                {
                    txt3.Text = "";
                }
                //添加控件到容器 
                c5.Controls.Add(txt3);
                r.Cells.Add(c5);


                Table1.Rows.Add(r);
            }
            //end
            //end
        }
        

        //
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      

        //显示价格
        set_guige(Request.QueryString["chanpinbianhao"].ToString());
        //
        if (!Page.IsPostBack)
        {
            Repeater1.DataSource = my_c.GetTable("select * from sl_yanse where chanpinbianhao=" + Request.QueryString["chanpinbianhao"].ToString() + "");
            Repeater1.DataBind();

            Repeater2.DataSource = my_c.GetTable("select * from sl_chicun where chanpinbianhao=" + Request.QueryString["chanpinbianhao"].ToString() + "");
            Repeater2.DataBind();

            Repeater3.DataSource = my_c.GetTable("select * from sl_product where id=" + Request.QueryString["chanpinbianhao"].ToString() + "");
            Repeater3.DataBind();

            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }

            if (type == "yanseedit")
            {
                DataTable dt = my_c.GetTable("select * from sl_yanse where id=" + Request.QueryString["id"].ToString() + "");
                this.TextBox1.Text = dt.Rows[0]["yanseming"].ToString();
                this.TextBox2.Text = dt.Rows[0]["yansezhi"].ToString();
                this.TextBox3.Text = dt.Rows[0]["duiyingtupian"].ToString();

                for (i = 0; i < RadioButtonList1.Items.Count; i++)
                {
                    if (RadioButtonList1.Items[i].Value == dt.Rows[0]["yansezhileixing"].ToString())
                    {
                        RadioButtonList1.Items[i].Selected = true;
                    }
                }

            }
            else if (type == "yansedel")
            {
                my_c.genxin("delete from sl_yanse where id=" + Request.QueryString["id"].ToString() + "");
                Response.Redirect("products_canshu.aspx?chanpinbianhao=" + Request.QueryString["chanpinbianhao"].ToString() + "");
            }
            else if (type == "chicunedit")
            {
                DataTable dt = my_c.GetTable("select * from sl_chicun where id=" + Request.QueryString["id"].ToString() + "");
                this.TextBox4.Text = dt.Rows[0]["chicunming"].ToString();
            }
            else if (type == "chicundel")
            {
                my_c.genxin("delete from sl_chicun where id=" + Request.QueryString["id"].ToString() + "");
                Response.Redirect("products_canshu.aspx?chanpinbianhao=" + Request.QueryString["chanpinbianhao"].ToString() + "");
            }
        }
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
     
        string chanpinbianhao = Request.QueryString["chanpinbianhao"].ToString();
        string yanseming = this.TextBox1.Text;
        string yansezhileixing = RadioButtonList1.SelectedValue;
        string yansezhi = this.TextBox2.Text;
        string duiyingtupian = this.TextBox3.Text;
        
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch { }

        if (type == "yanseedit")
        {

            my_c.genxin("update sl_yanse set yanseming='" + yanseming + "',yansezhileixing='" + yansezhileixing + "',yansezhi='" + yansezhi + "',duiyingtupian='" + duiyingtupian + "' where id=" + Request.QueryString["id"].ToString() + "");
        }
        else
        {
            my_c.genxin("insert into sl_yanse (chanpinbianhao,yanseming,yansezhileixing,yansezhi,duiyingtupian) values(" + chanpinbianhao + ",'" + yanseming + "','" + yansezhileixing + "','" + yansezhi + "','" + duiyingtupian + "')");
        }

        Response.Redirect("products_canshu.aspx?chanpinbianhao=" + chanpinbianhao + "");

    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        string chanpinbianhao = Request.QueryString["chanpinbianhao"].ToString();
        string chicunming = this.TextBox4.Text;
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch { }

        if (type == "chicunedit")
        {
            my_c.genxin("update sl_chicun set chicunming='" + chicunming + "' where id=" + Request.QueryString["id"].ToString() + "");
        }
        else
        {
            my_c.genxin("insert into sl_chicun (chanpinbianhao,chicunming) values(" + chanpinbianhao + ",'" + chicunming + "')");
        }
       
        Response.Redirect("products_canshu.aspx?chanpinbianhao=" + chanpinbianhao + "");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        string chanpinbianhao = Request.QueryString["chanpinbianhao"].ToString();
        my_c.genxin("delete from sl_jiage where chanpinbianhao=" + Request.QueryString["chanpinbianhao"].ToString() + "");
        get_guige(Request.QueryString["chanpinbianhao"].ToString());
        Response.Redirect("products_canshu.aspx?chanpinbianhao=" + chanpinbianhao + "");
    }
}
