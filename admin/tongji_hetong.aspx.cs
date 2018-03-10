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
public partial class admin_Order_table : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    public string add__url()
    {
        string laiyuanbianhao = "";
        try
        {
            laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
        }
        catch
        { }
        if (laiyuanbianhao != "")
        {
            return "hetong_table_add.aspx?laiyuanbianhao=" + laiyuanbianhao + "&Model_id=" + Request.QueryString["Model_id"].ToString();
        }
        else
        {
            return "hetong_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString();
        }
    }

    public string set_m_url(string g1, DataTable dt1, int j, string u12, string g2)
    {
    
        if (u12 != "")
        {
            Regex reg = new Regex(@"{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(g1);
            foreach (Match match in matches)
            {
                string id_name = match.ToString().Replace("{", "").Replace("}", "").Trim();

                g1 = g1.Replace(match.ToString(), dt1.Rows[j][id_name].ToString().Replace("$我是分隔符$", "|"));
            }
            return g1;
        }
        else
        {
            return g2;
        }

    }
    public string shijian = "30";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_b.set_admin_url();

            //my_c.genxin("update sl_hetong set zhuangtai='开发完成已收款' where zhuangtai='已续费'  and  DateDiff(day,hetongdaoqishijian,getdate())<=30 and DateDiff(day,hetongdaoqishijian,getdate())>=0");

            DropDownList1.DataSource = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=260 order by u3 asc,id desc");
            DropDownList1.DataTextField = "u1";
            DropDownList1.DataValueField = "u1";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, "全部");
            DropDownList1.Items[0].Value = "";
            string laiyuanbianhao = "";
            try
            {
                laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
            }
            catch
            { }
            string t1 = "<table class=\"table table-border table-hover\" id=\"content\"><thead><tr><th style='width:8%'>编号</th>";

            string search_sql = "id>0";
            string page = "1";
            string u1 = "";
            string zhuangtai = "开发完成已收款";
          
            try
            {
                shijian = Request.QueryString["shijian"].ToString();
               
            }
            catch
            { }
            this.TextBox1.Text = shijian;
            try
            {
                zhuangtai = Request.QueryString["zhuangtai"].ToString();
                
            }
            catch
            {

            }
            for (int i = 0; i < DropDownList1.Items.Count; i++)
            {
                if (DropDownList1.Items[i].Value == zhuangtai)
                {
                    DropDownList1.Items[i].Selected = true;
                }
            }


            search_sql = "zhuangtai like '%" + zhuangtai + "%'";

            try
            {
                page = Request.QueryString["page"].ToString();
            }
            catch
            { }
            try
            {
                u1 = Request.QueryString["u1"].ToString();
                this.TextBox3.Text = u1;
            }
            catch
            { }
            //mddel
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString());
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            if (Model_dt.Rows[0]["u8"].ToString() != "")
            {
                Literal3.Text = Model_dt.Rows[0]["u8"].ToString() + "&nbsp;&nbsp;";
            }
            //end

            string select_return = "id";
            dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and U5='是' order by u9 ,id");
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {
                    t1 = t1 + "<th>" + dt2.Rows[i]["u2"].ToString() + "</th><th>操作</th></tr></thead>";
                }
                else
                {
                    t1 = t1 + "<th  style='width:" + dt2.Rows[i]["u11"].ToString() + "'>" + dt2.Rows[i]["u2"].ToString() + "</th>";
                }

                select_return = select_return + "," + dt2.Rows[i]["u1"].ToString();
            }
            dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and U10='是'");
            string search_sql1 = "";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (search_sql1 == "")
                {
                    search_sql1 = dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
                }
                else
                {
                    search_sql1 = search_sql1 + " or " + dt.Rows[i]["u1"].ToString() + " like '%" + u1 + "%'";
                }
            }
           
            search_sql = search_sql+" and DateDiff(day,getdate(),hetongdaoqishijian)<=" +shijian+" and DateDiff(day,getdate(),hetongdaoqishijian)>=0";

            if (search_sql == "")
            {
                search_sql = "  (" + search_sql1 + ")";
            }
            else
            {
                search_sql = search_sql + " and (" + search_sql1 + ")";
            }
          
            string edit_url = "";

            if (laiyuanbianhao != "")
            {
                edit_url = "&laiyuanbianhao=" + laiyuanbianhao + "";
                if (search_sql == "")
                {
                    search_sql = "laiyuanbianhao=" + laiyuanbianhao + "";
                }
                else
                {
                    search_sql = search_sql + " and laiyuanbianhao=" + laiyuanbianhao + "";
                }
            }

            //Response.Write(search_sql);
            //Response.End();
            //计算金额
            Literal5.Text = my_c.GetTable("select sum(xufeijine) as xufeijine from sl_hetong where zhuangtai='" + zhuangtai + "' and  DateDiff(day,getdate(),hetongdaoqishijian)<=" + shijian + " and DateDiff(day,getdate(),hetongdaoqishijian)>=0").Rows[0]["xufeijine"].ToString() + "（" + my_c.GetTable("select sum(hetongzongjine) as hetongzongjine from sl_hetong where zhuangtai='" + zhuangtai + "' and  DateDiff(day,getdate(),hetongdaoqishijian)<=" + shijian + " and DateDiff(day,getdate(),hetongdaoqishijian)>=0").Rows[0]["hetongzongjine"].ToString() + "）";

            Literal6.Text = my_c.GetTable("select sum(xufeijine) as xufeijine from sl_hetong where (zhuangtai like '%开发完成已收款%') ").Rows[0]["xufeijine"].ToString();
            //end
            DataTable dt1 = my_b.art_list(biaoming, search_sql, select_return, "order by hetongdaoqishijian asc,id desc", 20, int.Parse(page), "?Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + u1 + "&zhuangtai=" + zhuangtai + "&shijian=" + shijian + "&page=$page$", Literal1);
            string test_align = "";

            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                t1 = t1 + "<tr>			<td style='width:5%'>&nbsp;&nbsp;<div class=\"checkbox checkbox-inline checkbox-success\"><input type=\"checkbox\" name=\"chk\" value=\"" + dt1.Rows[j]["id"].ToString() + "\"><label></label></td>			<td  style='width:6%'>" + dt1.Rows[j]["id"].ToString() + "</td>";
                for (i = 0; i < dt2.Rows.Count; i++)
                {
                    if (dt2.Rows[i]["u6"].ToString() != "编辑器" || dt2.Rows[i]["u6"].ToString() != "子编辑器")
                    {
                        test_align = " text-align:left";
                    }
                    if (i + 1 == dt2.Rows.Count)
                    {

                        if (dt2.Rows[i]["u6"].ToString() == "缩略图" || dt2.Rows[i]["u6"].ToString() == "头像")
                        {

                            t1 = t1 + "<td  style='" + dt2.Rows[i]["u11"].ToString() + "; "+ test_align + "'><a href='" + my_b.set_ApplicationPath(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString().Replace("$我是分隔符$", "|")) + "' target='_blank'><img src='" + my_b.set_ApplicationPath(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString().Replace("$我是分隔符$", "|")) + "' height='60px'></a></td><td>" + my_b.set_canshu(dt1.Rows[j]["id"].ToString(), Request.QueryString["Model_id"].ToString()) + "<a href='xufei.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "'>续费</a> | <a href='hetong_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "'>修改</a> | <a onclick=\"return confirm('你确认将该信息删除到回收站?   注意：相关图片和资源也将一起删除到回收站');\" href='hetong_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&type=del&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "' >删除</a></td></tr>";
                        }
                        else
                        {
                            t1 = t1 + "<td  style='width:" + dt2.Rows[i]["u11"].ToString() + "; word-break:break-all'>" + set_m_url(dt2.Rows[i]["u12"].ToString(), dt1, j, dt2.Rows[i]["u12"].ToString(), dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString().Replace("$我是分隔符$", "|")) + "</td><td>" + my_b.set_canshu(dt1.Rows[j]["id"].ToString(), Request.QueryString["Model_id"].ToString()) + "<a href='xufei.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "'>续费</a> | <a href='hetong_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "'>修改</a> | <a onclick=\"return confirm('你确认将该信息删除到回收站?   注意：相关图片和资源也将一起删除到回收站');\" href='hetong_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&type=del&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "' >删除</a></td></tr>";
                        }

                    }
                    else
                    {
                        if (dt2.Rows[i]["u6"].ToString() == "缩略图" || dt2.Rows[i]["u6"].ToString() == "头像")
                        {
                            t1 = t1 + "<td  style='" + dt2.Rows[i]["u11"].ToString() + "'><a href='" + my_b.set_ApplicationPath(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString().Replace("$我是分隔符$", "|")) + "' target='_blank'><img src='" + my_b.set_ApplicationPath(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString().Replace("$我是分隔符$", "|")) + "' height='60px'></a></td>";
                        }
                        else
                        {
                            if (dt2.Rows[i]["u6"].ToString() == "时间框")
                            {
                                t1 = t1 + "<td  style='" + dt2.Rows[i]["u11"].ToString() + "; word-break:break-all'>" + set_m_url(dt2.Rows[i]["u12"].ToString(), dt1, j, dt2.Rows[i]["u12"].ToString(), my_b.set_time(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString().Replace("$我是分隔符$", "|"),"yyyy-MM-dd")) + "</td>";
                            }
                            else
                            {
                                t1 = t1 + "<td  style='" + dt2.Rows[i]["u11"].ToString() + "; word-break:break-all'>" + set_m_url(dt2.Rows[i]["u12"].ToString(), dt1, j, dt2.Rows[i]["u12"].ToString(), dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString().Replace("$我是分隔符$", "|")) + "</td>";
                            }
                           
                        }


                    }
                }
            }

            t1 = t1 + "</table>";
            Literal2.Text = t1;

        }
    }



    protected void Button1_Click1(object sender, EventArgs e)
    {

        try
        {
            Response.Redirect("tongji_hetong.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + Request.QueryString["laiyuanbianhao"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&zhuangtai=" + DropDownList1.SelectedValue + "&shijian=" + this.TextBox1.Text + "");
        }
        catch
        {
            Response.Redirect("tongji_hetong.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&u1=" + my_b.c_string(this.TextBox3.Text) + "&zhuangtai=" + DropDownList1.SelectedValue + "&shijian=" + this.TextBox1.Text + "");
        }
    }
}
