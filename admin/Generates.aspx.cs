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
public partial class admin_Generates : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    public void set_html(string g1, string g2, string g3,string g4)
    {
        DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + g3 + ") and u6 <>''");
        if (dt.Rows.Count > 0)
        {
            string file_path = g2;
            try
            {
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + g2);
            }
            catch { }

            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + dt.Rows[0]["u6"].ToString()))
            {
                Response.Write(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + dt.Rows[0]["u6"].ToString() + "<br>");
                Response.Flush();
                my_h.set_content(File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + dt.Rows[0]["u6"].ToString(), Encoding.UTF8), g4, g1, file_path);
            }
        }
     
    }
    int jj = 0;
    string list_id = "";
    public void dr1(string t1, int t2, string t3)
    {

        DataTable dt1 = my_c.GetTable("select id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id in ("+t1+") and Model_id=" + t3 + "");
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
                    list_id = list_id+","+ dt1.Rows[j]["id"].ToString();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1, t3);
            }
        }

    }
    public void set_html2(string u7, string u6, string id)
    {
        File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath+u7, my_h.set_Single(File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + u6, Encoding.UTF8), "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single", id), Encoding.UTF8);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            int page = 0;
            int dt_count = 0;
            try
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }
            catch
            { }

            if (Request.QueryString["type"].ToString() == "列表页")
            {
                DataTable dt = new DataTable();
                if (Request.QueryString["classid"].ToString() == "0")
                {
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u5<>'选择模板' and u5<>''");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        my_h.set_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()), dt.Rows[i]["id"].ToString());
                        Response.Write(dt.Rows[i]["u1"].ToString() + "完成<br>");
                        Response.Flush();
                    }
                }
                else
                {
                    dr1(Request.QueryString["classid"].ToString(), 0, Request.QueryString["Model_id"].ToString());
                    if (list_id == "")
                    {
                        list_id = Request.QueryString["classid"].ToString();
                    }
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u5<>'选择模板'  and u5<>'' and id in (" + list_id + ")");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        my_h.set_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()), dt.Rows[i]["id"].ToString());
                        Response.Write(dt.Rows[i]["u1"].ToString() + "完成<br>");
                        Response.Flush();
                    }
                }
                Response.Write("<script>window.location='err.aspx?count_s=2&err=列表页所有生成完成！&errurl=Generate.aspx'</script>");
            }
            else if (Request.QueryString["type"].ToString() == "all")
            {

                DataTable dt = new DataTable();
                string s_type = "";
                try
                {
                    s_type = Request.QueryString["s_type"].ToString();
                }
                catch
                { }
               
                if (s_type == "")
                {
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single ");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string file_path = HttpContext.Current.Request.PhysicalApplicationPath+dt.Rows[i]["u7"].ToString().Substring(0, dt.Rows[i]["u7"].ToString().LastIndexOf("/") + 1);
                        Directory.CreateDirectory(file_path);
                        set_html2(dt.Rows[i]["u7"].ToString(), dt.Rows[i]["u6"].ToString(), dt.Rows[i]["id"].ToString());

                    }
                    Response.Redirect("err.aspx?err=单页面生成成功，马上主栏目页！&errurl=" + my_b.tihuan("Generates.aspx?type=all&s_type=zhu", "&", "fzw123") + "");
                }
                else if (s_type == "zhu")
                {
                   
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where u5<>'选择模板' and u5<>'' and type='主栏目' order by id desc");
                  
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list_id = "";
                        dr1(dt.Rows[i]["id"].ToString(), 0, dt.Rows[i]["Model_id"].ToString());
                        if (list_id == "")
                        {
                            list_id = dt.Rows[i]["id"].ToString();
                        }
                       

                        if (int.Parse(my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + dt.Rows[i]["Model_id"].ToString() + " and u5<>'选择模板' and id in (" + dt.Rows[i]["id"].ToString() + ")").Rows[0]["count_id"].ToString()) > 0)
                        {
                            my_h.set_zhu_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[i]["Model_id"].ToString()), list_id, dt.Rows[i]["id"].ToString());
                            Response.Write(my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + dt.Rows[i]["Model_id"].ToString() + " and u5<>'选择模板' and id in (" + dt.Rows[i]["id"].ToString() + ")").Rows[0]["u1"].ToString() + "完成<br>");
                            Response.Flush();
                        }


                    }
                 
                 
                    Response.Write("<script>window.location='err.aspx?err=主栏目生成成功，马上生成列表页！&errurl=" + my_b.tihuan("Generates.aspx?type=all&s_type=list", "&", "fzw123") + "'</script>");

                }
                else if (s_type == "list")
                {
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where u5<>'选择模板' and u5<>''");
             
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (int.Parse(my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + dt.Rows[i]["Model_id"].ToString() + " and u5<>'选择模板' and sort_id in (" + dt.Rows[i]["id"].ToString() + ")").Rows[0]["count_id"].ToString()) ==0)
                        {
                            try
                            {
                                my_h.set_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[i]["Model_id"].ToString() + ""), dt.Rows[i]["id"].ToString());
                            }
                            catch
                            {
                                Response.Write(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[i]["Model_id"].ToString() + ""));
                                Response.Flush();
                            }
                            Response.Write(dt.Rows[i]["u1"].ToString() + "完成<br>");
                            Response.Flush();
                        }
                    }
                 
                    Response.Write("<script>window.location='err.aspx?err=列表页生成成功，马上生成文章页！&errurl=" + my_b.tihuan("Generates.aspx?type=all&s_type=artilce", "&", "fzw123") + "'</script>");
                }
                else if (s_type == "artilce")
                {
                   
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u3 like '文章模型'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string atitle = my_c.GetTable("select top 1 * from sl_Field where Model_id =" + dt.Rows[i]["id"].ToString() + " order by u9,id").Rows[0]["u1"].ToString();

                        DataTable dt1 = my_c.GetTable("select id,classid," + atitle + ",Filepath from " + dt.Rows[i]["u1"].ToString() + " order by id desc");
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            if (my_b.get_value("u6", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + dt1.Rows[j]["classid"].ToString() + "") != "")
                            {
                                set_html(dt1.Rows[j]["id"].ToString(), dt1.Rows[j]["Filepath"].ToString(), dt1.Rows[j]["classid"].ToString(), dt.Rows[i]["u1"].ToString());
                                Response.Write(dt1.Rows[j][atitle].ToString() + "完成<br>");
                                Response.Flush();
                            }
                        }
                        Response.Clear();
                        Dispose();
                        

                    }
                    Response.Write("<script>window.location='err.aspx?err=文章页生成成功，所有生成完成！&errurl=" + my_b.tihuan("Generate.aspx", "&", "fzw123") + "'</script>");
                }


            }
            else if (Request.QueryString["type"].ToString() == "主栏目")
            {
              
                dr1(Request.QueryString["classid"].ToString(), 0, Request.QueryString["Model_id"].ToString());
                if (list_id == "")
                {
                    list_id = Request.QueryString["classid"].ToString();
                }
             
          
                if (int.Parse(my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u5<>'选择模板' and id in (" + Request.QueryString["classid"].ToString() + ")").Rows[0]["count_id"].ToString()) > 0)
                {
                    my_h.set_zhu_list(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()), list_id, Request.QueryString["classid"].ToString());
                    Response.Write(my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u5<>'选择模板' and id in (" + Request.QueryString["classid"].ToString() + ")").Rows[0]["u1"].ToString() + "完成<br>");
                    Response.Flush();
                }
                Response.Write("<script>window.location='err.aspx?count_s=2&err=主栏目生成完成！&errurl=Generate.aspx'</script>");
            }
            else
            {
                string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString());
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string order_type = Request.QueryString["order_type"].ToString();
                string order_by = Request.QueryString["order_by"].ToString();
                string Generate_count = Request.QueryString["Generate_count"].ToString();
                string order_string = "order by ";
                if (order_type == "添加时间")
                {
                    order_string = order_string + "dtime";
                }
                else
                {
                    order_string = order_string + "id";
                }
                if (order_by == "倒序")
                {
                    order_string = order_string + " desc";
                }
                if (int.Parse(Generate_count) > 0)
                {
                    if (page * 10 >= int.Parse(Generate_count))
                    {
                        Response.Write("<script>window.location='err.aspx?count_s=2&err=内容页所有生成完成！&errurl=Generate.aspx'</script>");
                    }
                }
                if (Request.QueryString["classid"].ToString() == "0")
                {
                    dt_count = my_b.get_count(table_name, "");
                    if (dt_count - page * 10 > 0)
                    {
                        if (page > 0)
                        {
                            int n_page = page;

                            dt1 = my_c.GetTable("select top 10 id,Filepath,classid from " + table_name + " where id not in (select top " + (n_page) * 10 + " id from " + table_name + "  " + order_string + ") " + order_string + "");

                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                try
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath+dt1.Rows[j]["Filepath"].ToString());
                                }
                                catch
                                { }
                                set_html(dt1.Rows[j]["id"].ToString(), dt1.Rows[j]["Filepath"].ToString(), dt1.Rows[j]["classid"].ToString(), table_name);
                            }
                        }
                        else
                        {
                            dt1 = my_c.GetTable("select top 10 id,Filepath,classid from " + table_name + "  " + order_string + "");
                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                try
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath+dt1.Rows[j]["Filepath"].ToString());
                                }
                                catch
                                { }
                                set_html(dt1.Rows[j]["id"].ToString(), dt1.Rows[j]["Filepath"].ToString(), dt1.Rows[j]["classid"].ToString(), table_name);
                            }
                        }
                        Response.Write("<script>window.location='err.aspx?count_s=2&err=总记录：" + dt_count + "条，已生成：" + (page + 1) * 10 + "条！&errurl=" + my_b.tihuan("Generates.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + Request.QueryString["classid"].ToString() + "&type=" + Request.QueryString["type"].ToString() + "&page=" + (page + 1) + "&order_type=" + Request.QueryString["order_type"].ToString() + "&order_by=" + Request.QueryString["order_by"].ToString() + "&Generate_count=" + Request.QueryString["Generate_count"].ToString() + "", "&", "fzw123") + "'</script>");
                    }
                    else
                    {
                        Response.Write("<script>window.location='err.aspx?count_s=2&err=内容页所有生成完成！&errurl=Generate.aspx'</script>");
                    }
                }
                else
                {
                    dr1(Request.QueryString["classid"].ToString(), 0, Request.QueryString["Model_id"].ToString());
                    if (list_id == "")
                    {
                        list_id = Request.QueryString["classid"].ToString();
                    }

                    dt_count = my_b.get_count(table_name, "where classid in (" + list_id + ")");

                    if (dt_count - page * 10 > 0)
                    {
                        if (page > 0)
                        {
                            int n_page = page;
                            dt1 = my_c.GetTable("select top 10 id,Filepath,classid from " + table_name + " where  classid in (" + list_id + ") and id not in (select top " + (n_page) * 10 + " id from " + table_name + " where  classid in (" + list_id + ") " + order_string + ")  " + order_string + "");

                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                try
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath+dt1.Rows[j]["Filepath"].ToString());
                                }
                                catch
                                { }
                                set_html(dt1.Rows[j]["id"].ToString(), dt1.Rows[j]["Filepath"].ToString(), dt1.Rows[j]["classid"].ToString(), table_name);
                            }
                        }
                        else
                        {
                            dt1 = my_c.GetTable("select top 10 id,Filepath,classid from " + table_name + " where  classid in (" + list_id + ")  " + order_string + "");
                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                try
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath+dt1.Rows[j]["Filepath"].ToString());
                                }
                                catch
                                { }
                                set_html(dt1.Rows[j]["id"].ToString(), dt1.Rows[j]["Filepath"].ToString(), dt1.Rows[j]["classid"].ToString(), table_name);
                            }
                        }
                        Response.Write("<script>window.location='err.aspx?count_s=2&err=总记录：" + dt_count + "条，已生成：" + (page + 1) * 10 + "条！&errurl=" + my_b.tihuan("Generates.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + Request.QueryString["classid"].ToString() + "&type=" + Request.QueryString["type"].ToString() + "&page=" + (page + 1) + "&order_type=" + Request.QueryString["order_type"].ToString() + "&order_by=" + Request.QueryString["order_by"].ToString() + "&Generate_count=" + Request.QueryString["Generate_count"].ToString() + "", "&", "fzw123") + "'</script>");
                    }
                    else
                    {
                        Response.Write("<script>window.location='err.aspx?count_s=2&err=内容页所有生成完成！&errurl=Generate.aspx'</script>");
                    }
                }






            }




        }
    }
}

