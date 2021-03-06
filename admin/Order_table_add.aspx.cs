﻿using System;
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
public partial class admin_Order_table_add : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    static DataTable Model_dt = new DataTable();
    biaodan db = new biaodan();
    my_html my_h = new my_html();
    my_order my_o = new my_order();
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }
        if (type != "del")
        {
            //读购物车
            if (Request.QueryString["Model_id"].ToString() != "")
            {

                Model_dt = my_c.GetTable("select top 5 * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id in(select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "cart') order by u9,id");

                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "cart where dingdanhao='" + my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString() + "").Rows[0]["dingdanhao"].ToString() + "'");

                Repeater1.DataSource = dt;
                Repeater1.DataBind();

            }
            //购物车完成
            //读操作记录
            if (Request.QueryString["Model_id"].ToString() != "")
            {


                DataTable dt = my_c.GetTable("select * from sl_system where u2 like '%" + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + "%' and u2 like '%" + my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString() + "").Rows[0]["dingdanhao"].ToString() + "%'");


                for (int d1 = 0; d1 < dt.Rows.Count; d1++)
                {
                    TableRow r = new TableRow();
                    TableCell c = new TableCell();
                    string neirong = "操作人员：" + dt.Rows[d1]["u1"].ToString() + "，事件：" + dt.Rows[d1]["u2"].ToString() + "，IP：" + dt.Rows[d1]["u3"].ToString() + "，时间：" + dt.Rows[d1]["dtime"].ToString() + "";
                    c.Controls.Add(new LiteralControl(neirong));
                    r.Cells.Add(c);
                    Table4.Rows.Add(r);
                }


            }

            //产品完成



        }



        if (type == "edit")
        {


            Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
            DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString());
            int numrows = 1;
            int numcells = 2;
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                for (int j = 0; j < numrows; j++)
                {
                    TableRow r = new TableRow();
                    for (int i = 0; i < numcells; i++)
                    {
                        TableCell c = new TableCell();
                        if (i == 0)
                        {
                            c.Controls.Add(new LiteralControl(Model_dt.Rows[d1]["u2"].ToString()));
                            c.CssClass = "tRight";
                        }
                        else if (i == 1)
                        {
                            db.set_kj_edit(c, dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString(), Model_dt,d1);
                        }
                        else
                        {

                        }


                        r.Cells.Add(c);

                    }
                    Table1.Rows.Add(r);
                }
            }


        }
        else if (type == "del")
        {
            if (Request.QueryString["id"].ToString() == "")
            {
                Response.Redirect("err.aspx?err=请选择要删除的信息！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
            }
            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString());
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                my_c.genxin("delete from " + table_name + " where id in (" + dt.Rows[i]["id"].ToString() + ")");
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
            }

            string laiyuanbianhao = "";
            try
            {
                laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
            }
            catch
            { }
            if (laiyuanbianhao == "")
            {
                my_b.tiaozhuan("删除信息成功！马上跳转到信息列表页面！", "Order_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "");
            }
            else
            {
                my_b.tiaozhuan("删除信息成功！马上跳转到信息列表页面！", "Order_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + laiyuanbianhao + "");
            }
        }
        else
        {
            Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
            int numrows = 1;
            int numcells = 2;
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                for (int j = 0; j < numrows; j++)
                {
                    TableRow r = new TableRow();
                    for (int i = 0; i < numcells; i++)
                    {
                        TableCell c = new TableCell();
                        if (i == 0)
                        {
                            c.Controls.Add(new LiteralControl(Model_dt.Rows[d1]["u2"].ToString()));
                            c.CssClass = "tRight";
                        }
                        else if (i == 1)
                        {
                            db.set_kj(c, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), Model_dt.Rows[d1]["u8"].ToString(), Model_dt.Rows[d1]["u3"].ToString());
                        }
                        else
                        {

                        }


                        r.Cells.Add(c);

                    }
                    Table1.Rows.Add(r);
                }
            }
            //设置来源编号
            try
            {
                TextBox txt = new TextBox();
                txt = (TextBox)form1.FindControl("laiyuanbianhao");
                txt.Text = Request.QueryString["laiyuanbianhao"].ToString();
                txt.Enabled = false;
            }
            catch { }

        }
        //设置按钮事件
        Button button = new Button();
        button = (Button)form1.FindControl("tqtp");
        try
        {
            button.Command += new CommandEventHandler(this.On_Button);
        }
        catch
        { }


    }
    protected void On_Button(Object sender, CommandEventArgs e)
    {
        Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
        DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " ");
        string t1 = "";
        int d1 = 0;
        for (d1 = 0; d1 < Model_dt.Rows.Count; d1++)
        {
            if (Model_dt.Rows[d1]["u6"].ToString() == "编辑器")
            {
                TextBox txt = new TextBox();
                txt = (TextBox)form1.FindControl(Model_dt.Rows[d1]["u1"].ToString());
                t1 = my_b.c_string(txt.Text.ToString());

            }
        }
        if (t1 != "")
        {
            for (d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (Model_dt.Rows[d1]["u6"].ToString() == "缩略图")
                {
                    TextBox txt = new TextBox();
                    txt = (TextBox)form1.FindControl(Model_dt.Rows[d1]["u1"].ToString());
                    txt.Text = my_b.get_images(t1, 1);

                }
            }
        }

    }



    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string laiyuanbianhao = "";
        try
        {
            laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
        }
        catch
        { }

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            string tablename = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "");
            string sql = "update " + tablename + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + db.get_kj(Table1, Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + db.get_kj(Table1, Model_dt, d1) + "";
                }
            }

            sql = sql + " where id=" + Request.QueryString["id"].ToString();

            DataTable dt = my_c.GetTable("select * from " + tablename + " where id=" + Request.QueryString["id"].ToString() + "");
            string dingdanhao = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString() + "").Rows[0]["dingdanhao"].ToString();

            RadioButtonList dr = new RadioButtonList();
            dr = (RadioButtonList)form1.FindControl("zhuangtai");
            //if (dt.Rows[0]["zhuangtai"].ToString() == "已发货")
            //{
            my_c.genxin(sql);

            //审核通过发短信
            if (dr.SelectedValue == "订单完成")
            {

                //处理积分
                my_o.set_order_jifen(dingdanhao, my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()));
                //end

                //发送短信

                //end
            }


            if (dr.SelectedValue == "已发货")
            {
                if (dt.Rows[0]["yonghuming"].ToString() != "")
                {
                    string wherestrig = "where id=" + dt.Rows[0]["id"].ToString() + "";

                    //  my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=order&tablename=" + tablename + "&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");

                }
            }
            //处理订单配送发送微信消息

            //}
            //else
            //{
            //    my_c.genxin(sql);
            //}

            string order_log = tablename + "表被修改，状态改为[" + dr.SelectedValue + "]，订单号:" + dingdanhao;

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("admin_id") + "','" + order_log + "','" + Request.UserHostAddress.ToString() + "','订单处理')");

            Response.Redirect("err.aspx?err=修改信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("Order_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
        else
        {
            string sql = "insert into " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " ";
            sql = sql + "(";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString();
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString();
                }
            }
            sql = sql + ") values (";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + db.get_kj(Table1, Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + db.get_kj(Table1, Model_dt, d1);
                }
            }
            sql = sql + ")";

            my_c.genxin(sql);
            Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("Order_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");


        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
