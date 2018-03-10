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

public partial class admin_Fields : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    biaodan db = new biaodan();
    #region 显示列表
    public string jigege_model_id = "";
    public void set_listpage(string biaoming,string type)
    {
        DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + biaoming + "'");
        string model_id = Model_dt.Rows[0]["id"].ToString();
        jigege_model_id = model_id;
        DataTable dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + model_id + " and U5='是' order by u9 ,id");
      
        string select_return = "";
        string t1 = "<table class=\"table table-border table - hover\" id=\"content\"><thead><tr><th style='width:5%'  class=\"hidden-xs\">选择</th>";
        int i = 0;
        #region 处理表头
        t1 = t1 + db.list_biaotou_caozuo(dt2);
        for (i = 0; i < dt2.Rows.Count; i++)
        {
            select_return = select_return + "," + dt2.Rows[i]["u1"].ToString();
        }
        #endregion
        DataTable dt1 = my_c.GetTable("select * from "+biaoming+" where laiyuanbianhao="+Request.QueryString["laiyuanbianhao"].ToString() +"");
        //Response.Write(dt1.Rows.Count);
        //Response.End();
        if (type == "")
        {
            Literal1.Text = db.set_list_caozuo(t1, dt2, dt1, i, "", "articles_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + Request.QueryString["classid"].ToString() + "");
        }
        else
        {
            Response.Write(db.set_list_caozuo(t1, dt2, dt1, i, "", "articles_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=" + Request.QueryString["classid"].ToString() + ""));
            Response.End();
        }
       
    }
    #endregion 显示列表
    public string guigefenlei = ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigefenlei";
    public string guigejiage = ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigejiage";
    public string set_guige_canshu(string classid)
    {
        DataTable dt= my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where classid=" + classid + " order by paixu asc,id desc");
        string list = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            CheckBox che;
            che = new CheckBox();
            che.ID = "canshu";
            che.Text = dt.Rows[i]["biaoti"].ToString();
           
           list = list + "<div class=\"checkbox checkbox-inline checkbox-success\"><input name=\"canshu\" type=\"checkbox\" id=\"" + dt.Rows[i]["id"].ToString() + "\" value=\"" + dt.Rows[i]["id"].ToString()+ "\"><label for=\"" + dt.Rows[i]["id"].ToString() + "\">" + dt.Rows[i]["biaoti"].ToString() + "</label></div>";
        }
        return list;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }
            if (type == "add")
            {
                #region 规格增加
                string laiyuanbianhao = Request["laiyuanbianhao"].ToString();
                string Model_id = Request["Model_id"].ToString();
                my_c.genxin("delete from " + guigejiage + " where laiyuanbianhao=" + laiyuanbianhao + " and Model_id=" + Model_id + "");
                my_c.genxin("delete from " + guigefenlei + " where laiyuanbianhao=" + laiyuanbianhao + " and Model_id=" + Model_id + "");
                string canshu = Request["canshu"].ToString();
                DataTable sl_guigecanshu = my_c.GetTable("select distinct classid from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where id in (" + canshu + ") ");
                //Response.Write("select distinct classid from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where id in (" + canshu + ") ");
                //Response.End();
                int canshu_count = 1;
                //处理Model_id数据
                string jiage = "0";

                try
                {
                    jiage = Request.QueryString["jiage"].ToString();
                }
                catch { }

       
                
                //处理Model_id数据
                DataTable canshu1_dt = new DataTable();
                DataTable canshu2_dt = new DataTable();
                DataTable canshu3_dt = new DataTable();
                DataTable canshu4_dt = new DataTable();

                for (int i = 0; i < sl_guigecanshu.Rows.Count; i++)
                {


                    if (i == 0)
                    {
                        canshu1_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where id in (" + canshu + ")  and classid in (" + sl_guigecanshu.Rows[i]["classid"].ToString() + ")");
                        canshu_count = canshu_count * canshu1_dt.Rows.Count;
                        canshufenlei(canshu1_dt, laiyuanbianhao, sl_guigecanshu.Rows[i]["classid"].ToString());


                    }
                    if (i == 1)
                    {
                        canshu2_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where id in (" + canshu + ")  and classid in (" + sl_guigecanshu.Rows[i]["classid"].ToString() + ")");
                        canshu_count = canshu_count * canshu2_dt.Rows.Count;
                        canshufenlei(canshu2_dt, laiyuanbianhao, sl_guigecanshu.Rows[i]["classid"].ToString());
                    }
                    if (i == 2)
                    {
                        canshu3_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where id in (" + canshu + ")  and classid in (" + sl_guigecanshu.Rows[i]["classid"].ToString() + ")");
                        canshu_count = canshu_count * canshu3_dt.Rows.Count;
                        canshufenlei(canshu3_dt, laiyuanbianhao, sl_guigecanshu.Rows[i]["classid"].ToString());
                    }
                    if (i == 3)
                    {
                        canshu4_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where id in (" + canshu + ")  and classid in (" + sl_guigecanshu.Rows[i]["classid"].ToString() + ")");
                        canshu_count = canshu_count * canshu4_dt.Rows.Count;
                        canshufenlei(canshu4_dt, laiyuanbianhao, sl_guigecanshu.Rows[i]["classid"].ToString());
                    }
                    //for (int j = 0; j < dt1.Rows.Count; j++)
                    //{
                    //    //string jiage = "0";
                    //    //string canshu_ = "";
                    //    //string sql = "insert into sl_guigejiage(laiyuanbianhao,jiage,fenlei,canshu) values(" + laiyuanbianhao + "," + jiage + "," + dt.Rows[i]["classid"].ToString() + ",''" + canshu_ + ")";

                    //}

                    //end
                }

                //  Response.Write(canshu_count);
                #region 处理入库
                string classid = "";
                string canshu_ = "";
                if (sl_guigecanshu.Rows.Count == 1)
                {
                    for (int i = 0; i < canshu1_dt.Rows.Count; i++)
                    {
                        classid = canshu1_dt.Rows[i]["classid"].ToString();
                        canshu_ = canshu1_dt.Rows[i]["biaoti"].ToString();
                        string sql = "insert into " + guigejiage + "(laiyuanbianhao,jiage,fenlei,canshu,Model_id) values(" + laiyuanbianhao + "," + jiage + ",'" + classid + "','" + canshu_ + "'," + Model_id + ")";
                        my_c.genxin(sql);
                    }
                }

                if (sl_guigecanshu.Rows.Count == 2)
                {
                    for (int i = 0; i < canshu1_dt.Rows.Count; i++)
                    {
                        canshu_ = canshu1_dt.Rows[i]["biaoti"].ToString();
                        classid = canshu1_dt.Rows[i]["classid"].ToString();
                        for (int j = 0; j < canshu2_dt.Rows.Count; j++)
                        {
                            //  canshu_= canshu_+","+ canshu2_dt.Rows[j]["biaoti"].ToString();
                            //      classid = classid + "," + canshu2_dt.Rows[j]["classid"].ToString();
                            string sql = "insert into " + guigejiage + "(laiyuanbianhao,jiage,fenlei,canshu,Model_id) values(" + laiyuanbianhao + "," + jiage + ",'" + classid + "," + canshu2_dt.Rows[j]["classid"].ToString() + "','" + canshu_ + "," + canshu2_dt.Rows[j]["biaoti"].ToString() + "'," + Model_id + ")";
                            my_c.genxin(sql);
                            //Response.Write(sql);
                            //Response.End();
                        }



                    }
                }

                if (sl_guigecanshu.Rows.Count == 3)
                {
                    for (int i = 0; i < canshu1_dt.Rows.Count; i++)
                    {
                        canshu_ = canshu1_dt.Rows[i]["biaoti"].ToString();
                        classid = canshu1_dt.Rows[i]["classid"].ToString();
                        for (int j = 0; j < canshu2_dt.Rows.Count; j++)
                        {

                            for (int d = 0; d < canshu3_dt.Rows.Count; d++)
                            {
                                string sql = "insert into " + guigejiage + "(laiyuanbianhao,jiage,fenlei,canshu,Model_id) values(" + laiyuanbianhao + "," + jiage + ",'" + classid + "," + canshu2_dt.Rows[j]["classid"].ToString() + "," + canshu3_dt.Rows[d]["classid"].ToString() + "','" + canshu_ + "," + canshu2_dt.Rows[j]["biaoti"].ToString() + "," + canshu3_dt.Rows[d]["biaoti"].ToString() + "'," + Model_id + ")";
                                my_c.genxin(sql);
                                //Response.Write(sql);
                                //Response.End();
                            }
                        }



                    }
                }

                if (sl_guigecanshu.Rows.Count == 4)
                {
                    for (int i = 0; i < canshu1_dt.Rows.Count; i++)
                    {
                        canshu_ = canshu1_dt.Rows[i]["biaoti"].ToString();
                        classid = canshu1_dt.Rows[i]["classid"].ToString();
                        for (int j = 0; j < canshu2_dt.Rows.Count; j++)
                        {

                            for (int d = 0; d < canshu3_dt.Rows.Count; d++)
                            {

                                for (int f = 0; f < canshu4_dt.Rows.Count; f++)
                                {
                                    string sql = "insert into " + guigejiage + "(laiyuanbianhao,jiage,,fenlei,canshu,Model_id) values(" + laiyuanbianhao + "," + jiage + ",'" + classid + "," + canshu2_dt.Rows[j]["classid"].ToString() + "," + canshu3_dt.Rows[d]["classid"].ToString() + "," + canshu4_dt.Rows[f]["classid"].ToString() + "','" + canshu_ + "," + canshu2_dt.Rows[j]["biaoti"].ToString() + "," + canshu3_dt.Rows[d]["biaoti"].ToString() + "," + canshu4_dt.Rows[f]["biaoti"].ToString() + "'," + Model_id + ")";
                                    my_c.genxin(sql);
                                    //Response.Write(sql);
                                    //Response.End();
                                }

                            }
                        }



                    }
                }
                #endregion 处理入库
                //jieshu

                set_listpage(guigejiage, type);
                #endregion
            }
            else if (type == "del")
            {
                #region 规格删除
                string laiyuanbianhao = Request["laiyuanbianhao"].ToString();
                string Model_id = Request["Model_id"].ToString();

                my_c.genxin("delete from " + guigejiage + " where laiyuanbianhao=" + laiyuanbianhao + " and Model_id=" + Model_id + "");
                my_c.genxin("delete from " + guigefenlei + " where laiyuanbianhao=" + laiyuanbianhao + " and Model_id=" + Model_id + "");
                set_listpage(guigejiage, type);
                #endregion
            }
            else if (type == "xiugai")
            {
                set_listpage(guigejiage, type);
            }
            else
            {
                #region 默认情况
                Repeater1.DataSource = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu where classid=0 order by paixu asc,id desc");
                Repeater1.DataBind();

                string laiyuanbianhao = Request["laiyuanbianhao"].ToString();
                string Model_id = Request["Model_id"].ToString();
                set_listpage(guigejiage, type);
                #endregion
            }


        }
    }


  
    public void canshufenlei(DataTable dt, string laiyuanbianhao,string classid)
    {
        string Model_id = Request["Model_id"].ToString();
        string canshu = "";
        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if (canshu == "")
            {
                canshu = dt.Rows[j]["biaoti"].ToString();
            }
            else
            {
                canshu = canshu+","+ dt.Rows[j]["biaoti"].ToString();
            }
        }
        my_c.genxin("insert into "+guigefenlei+"(model_id,fenlei,laiyuanbianhao,canshu) values(" + Model_id + "," + classid + "," + laiyuanbianhao + ",'" + canshu + "')");
    }

   
}
