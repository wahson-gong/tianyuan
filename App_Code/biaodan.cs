using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
/// <summary>
/// no_html 的摘要说明
/// </summary>
public class biaodan
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    jiami jm = new jiami();
    #region listboxr的设置
    //listboxr的设置
    int jj = 0;
    int i = 0;
    string parameter_list = "";
    public void dr1(string t1, int t2, DropDownList drop1)
    {

        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + " and Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "— ";
                }
                try
                {
                    drop1.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                    drop1.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                }
                catch
                {
                    HttpContext.Current.Response.Write(jj);
                    HttpContext.Current.Response.End();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1, drop1);
            }
        }

    }
    public void fenlei_class(string t1, int t2, DropDownList drop1, string biaoming)
    {
        string Field_str = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + " and U5='是' order by u9 ,id").Rows[0]["u1"].ToString();
        DataTable dt1 = my_c.GetTable("select * from " + biaoming + " where classid=" + t1 + "");
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                string bb = "";
                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "— ";
                }
                try
                {
                    drop1.Items.Insert(jj, bb + dt1.Rows[j][Field_str].ToString());
                    drop1.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                }
                catch
                {
                    HttpContext.Current.Response.Write(jj);
                    HttpContext.Current.Response.End();
                }
                jj = jj + 1;
                int tt1 = t2 + 1;
                fenlei_class(dt1.Rows[j]["id"].ToString(), tt1, drop1, biaoming);
            }
        }

    }
    #region 列出最顶级目录
    //列出最顶级目录
    static string get_ding_dir_str = "";
    public string Parameterweizhi(string classid)
    {
        if (classid == "")
        {
            return "";
        }
        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + classid + " and classid>0");
        //HttpContext.Current.Response.Write("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + classid + " and classid>0");
        //HttpContext.Current.Response.End();
        if (dt1.Rows.Count > 0)
        {
            if (get_ding_dir_str == "")
            {
                get_ding_dir_str = dt1.Rows[0]["id"].ToString();
            }
            else
            {
                get_ding_dir_str = dt1.Rows[0]["id"].ToString() + "," + get_ding_dir_str;
            }

            if (dt1.Rows[0]["classid"].ToString() != "184")
            {
                Parameterweizhi(dt1.Rows[0]["classid"].ToString());

            }
        }

        return get_ding_dir_str.ToString();
    }
    //列出最顶级目录 end
    #endregion 列出最顶级目录 
    public void Parameter_class(string classid, int t2)
    {
        DataTable dt1 = my_c.GetTable("select id,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=" + classid + "  order by u3 asc,id asc");
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                string bb = "";
                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "—";
                }
                if (parameter_list == "")
                {
                    parameter_list = "{k:" + dt1.Rows[j]["id"].ToString() + ",v:'" + bb + dt1.Rows[j]["u1"].ToString() + "'}";
                }
                else
                {
                    parameter_list = parameter_list + "|" + "{k:" + dt1.Rows[j]["id"].ToString() + ",v:'" + bb + dt1.Rows[j]["u1"].ToString() + "'}";
                }

                jj = jj + 1;
                int tt1 = t2 + 1;
                Parameter_class(dt1.Rows[j]["id"].ToString(), tt1);
            }
        }

    }
    public void ListBox_set(string t1, int t2, ListBox listb)
    {

        DataTable dt1 = my_c.GetTable("select id,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=" + t1 + " ");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "— ";
                }

                listb.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                listb.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                jj = jj + 1;
                int tt1 = t2 + 1;
                ListBox_set(dt1.Rows[j]["id"].ToString(), tt1, listb);
            }
        }

    }
    //end listbox
    #endregion listbox
    my_html my_h = new my_html();
    #region 插入搜索汇总表
    public void charu_search(string Model_id, string id)
    {
        string search_sql = "u6 like '标题' or u6 like '文本框' or u6 like '文本域' or u6 like '编辑器' or u6 like '下拉框' or u6 like '单选按钮组' or u6 like '子编辑器'";
        DataTable sl_Model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1 like '%search%'");


        if (sl_Model.Rows.Count > 0)
        {
            DataTable sl_sort = my_c.GetTable("select * from sl_sort where  u6<>''");
            #region 处理搜索表
            sl_Model = my_c.GetTable("select * from sl_Model where id=" + Model_id);
            //HttpContext.Current.Response.Write(sl_Model.Rows.Count);
            //HttpContext.Current.Response.End();
            for (int i = 0; i < sl_Model.Rows.Count; i++)
            {
                DataTable dt = my_c.GetTable("select * from " + sl_Model.Rows[i]["u1"].ToString() + " where id=" + id);
                DataTable sl_Field = my_c.GetTable("select * from sl_Field where Model_id=" + sl_Model.Rows[i]["id"].ToString() + " and (" + search_sql + ")");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow[] rows = sl_sort.Select("id=" + dt.Rows[j]["classid"].ToString());
                    if (rows.Length > 0)
                    {
                        #region 开始循环
                        string sql = "";
                        string neirong = "";
                        string biaoti = "";
                        for (int j1 = 0; j1 < sl_Field.Rows.Count; j1++)
                        {
                            if (j1 == 0)
                            {
                                biaoti = dt.Rows[j][sl_Field.Rows[j1]["u1"].ToString()].ToString();
                            }
                            if (sl_Field.Rows[j1]["u6"].ToString() == "标题")
                            {
                                biaoti = dt.Rows[j][sl_Field.Rows[j1]["u1"].ToString()].ToString();
                            }

                            neirong = neirong + dt.Rows[j][sl_Field.Rows[j1]["u1"].ToString()];

                        }
                        sql = "insert into sl_search(biaoti,neirong,classid,laiyuanbianhao,Filepath,dtime) values('" + my_b.c_string(biaoti) + "','" + my_b.c_string(neirong) + "'," + dt.Rows[j]["classid"].ToString() + "," + dt.Rows[j]["id"].ToString() + ",'" + dt.Rows[j]["Filepath"].ToString() + "','" + dt.Rows[j]["dtime"].ToString() + "')";

                        my_c.genxin("delete from sl_search where classid=" + dt.Rows[j]["classid"].ToString() + " and laiyuanbianhao=" + dt.Rows[j]["id"].ToString() + "");
                        my_c.genxin(sql);  //插入
                        #endregion
                    }

                }
            }
            #endregion
        }
    }
    #endregion
    #region 设置编辑上的东西
    //设置编辑上的东西
    public void get_bianjiqi(TableCell c)
    {

        DataTable xml_dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");

        if (xml_dt.Rows[0]["u37"].ToString().IndexOf("远程抓图") > -1)
        {
            CheckBox ch1;
            //创建TextBox 
            ch1 = new CheckBox();
            ch1.ID = "tu1";
            ch1.Checked = true;
            ch1.Text = "远程抓图";
            c.Controls.Add(ch1);
        }
        else
        {
            CheckBox ch1;
            //创建TextBox 
            ch1 = new CheckBox();
            ch1.ID = "tu1";
            ch1.Text = "远程抓图";
            c.Controls.Add(ch1);
        }


        if (xml_dt.Rows[0]["u37"].ToString().IndexOf("自动改变图片大小") > -1)
        {
            CheckBox ch1;
            //创建TextBox 
            ch1 = new CheckBox();
            ch1.ID = "tu2";
            ch1.Checked = true;
            ch1.Text = "自动改变图片大小";
            c.Controls.Add(ch1);
        }
        else
        {
            CheckBox ch1;
            //创建TextBox 
            ch1 = new CheckBox();
            ch1.ID = "tu2";
            ch1.Text = "自动改变图片大小";
            c.Controls.Add(ch1);
        }

        if (xml_dt.Rows[0]["u37"].ToString().IndexOf("设置第一张为缩略图") > -1)
        {
            CheckBox ch1;
            //创建TextBox 
            ch1 = new CheckBox();
            ch1.ID = "tu3";
            ch1.Checked = true;
            ch1.Text = "设置第一张为缩略图";
            c.Controls.Add(ch1);
        }
        else
        {
            CheckBox ch1;
            //创建TextBox 
            ch1 = new CheckBox();
            ch1.ID = "tu3";
            ch1.Text = "设置第一张为缩略图";
            c.Controls.Add(ch1);
        }
    }
    #endregion
    #region 设置标题上的东西
    //设置标题上的东西
    //ubb {color:#ffff}{b:yes}
    public string get_ubb(string type, string content)
    {
        Regex reg = new Regex("{" + type + ".*?}", RegexOptions.Singleline);
        Match matches = reg.Match(content);
        string t1 = matches.ToString().Replace(type, "").Replace("{", "").Replace("}", "").Replace(":", "");
        return t1;
    }
    #endregion
    public string set_ziduan(DataTable dt, string ziduan, int j)
    {
        string[] aa = ziduan.Split(',');
        if (ziduan.IndexOf("+") > -1)
        {
            aa = ziduan.Split('+');
        }

        string zhi = "";
        for (int j1 = 0; j1 < aa.Length; j1++)
        {
            if (zhi == "")
            {
                zhi = dt.Rows[j][aa[j1]].ToString();
            }
            else
            {
                zhi = zhi + "-" + dt.Rows[j][aa[j1]].ToString();
            }
        }
        return zhi;
    }
    public string get_url(string content)
    {
        Regex reg = new Regex("<.*?>", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(content);

        foreach (Match match in matches)
        {
            string t1 = match.ToString().Replace("<", "").Replace(">", "");
            if (t1 == "u1")
            {
                DataTable sl_sort = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + HttpContext.Current.Request.QueryString["classid"].ToString() + "");
                if (sl_sort.Rows.Count > 0)
                {
                    content = content.Replace(match.ToString(), sl_sort.Rows[0]["u1"].ToString());
                }

            }
            else
            {
                content = content.Replace(match.ToString(), HttpContext.Current.Request.QueryString[t1].ToString());
            }

        }

        return content;
    }
    public void get_biaoti(TableCell c, string neirong)
    {
        string fzw_yanse = "";
        string fzw_jiacu = "";
        if (neirong != "")
        {
            fzw_yanse = get_ubb("color", neirong);
            fzw_jiacu = get_ubb("b", neirong);
        }

        TextBox txt;
        //创建TextBox 
        txt = new TextBox();
        txt.ID = "fzw_yanse";
        txt.Width = 100;
        txt.Text = fzw_yanse;
        txt.CssClass = "dinput";
        txt.Attributes.Add("onClick", "Jcolor(this).color();");
        c.Controls.Add(new LiteralControl("&nbsp;&nbsp;颜色："));
        c.Controls.Add(txt);

        CheckBox ch1;
        //创建TextBox 
        ch1 = new CheckBox();
        ch1.ID = "fzw_jiacu";
        ch1.Text = "加粗";
        if (fzw_jiacu == "yes")
        {
            ch1.Checked = true;
        }
        c.Controls.Add(ch1);
    }
    #region 处理默认数据
    public void set_moren(string txt_value, TextBox txt, string id)
    {
        string type = "";
        try
        {
            type = HttpContext.Current.Request.QueryString["type"].ToString();
        }
        catch { }
        string moren_str = "";
        #region 通用
        if (txt_value == "更新时间")
        {
            DateTime dy = DateTime.Now;
            moren_str = dy.ToString();
            txt.Text = moren_str;
        }
        #endregion
        if (type == "edit")
        {
            #region 编辑中
            #endregion
        }
        else
        {
            #region 非编辑情况
            if (txt_value == "当前时间")
            {
                DateTime dy = DateTime.Now;
                moren_str = dy.ToString();
            }
            else if (txt_value.IndexOf("随机数") > -1)
            {
                int weishu = int.Parse(txt_value.Replace("随机数:", ""));
                moren_str = my_b.get_suiji(weishu);
            }
            else if (txt_value.IndexOf("用户名") > -1)
            {
                moren_str = my_b.k_cookie("admin_id");
            }
            txt.Text = moren_str;
            #endregion
        }

    }
    #endregion
    #region 增加输入表单
    public void set_kj(TableCell c, string type, string id, string txt_value, string u3)
    {
        if (type == "分类")
        {

            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
            jj = 0;
            dr1("0", 0, dro);
            for (int d = 0; d < dro.Items.Count; d++)
            {
                if (dro.Items[d].Value == HttpContext.Current.Request.QueryString["classid"].ToString())
                {
                    dro.Items[d].Selected = true;
                    break;
                }
            }
            c.Controls.Add(dro);
        }
        else if (type == "数字" || type == "货币")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = txt_value;

            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "上级")
        {

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString());
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
            jj = 0;
            fenlei_class("0", 0, dro, biaoming);
            dro.Items.Insert(0, "顶级目录");
            dro.Items[0].Value = "0";
            for (int d = 0; d < dro.Items.Count; d++)
            {
                try
                {
                    if (dro.Items[d].Value == HttpContext.Current.Request.QueryString["classid"].ToString())
                    {
                        dro.Items[d].Selected = true;
                        break;
                    }
                }
                catch { }
            }
            c.Controls.Add(dro);
        }
        else if (type == "框架")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";

            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<iframe width=\"80%\"  frameborder=\"0\" src=\"\" scrolling=\"no\" id=\"" + id + "_\"></iframe><script type=\"text/javascript\">        document.getElementById('" + id + "_').src = \"" + my_b.kuangjiaurl(txt_value) + "editname=" + id + "&listid=\"+document.getElementById('" + id + "').value+\"\";</script>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "多条数据")
        {
            string bianhao = "";
            try
            {
                bianhao = my_b.k_cookie("bianhao");
            }
            catch
            {
                bianhao = my_b.get_bianhao();
                my_b.c_cookie(bianhao, "bianhao");
            }

            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.Text = bianhao;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<iframe width=\"80%\"  frameborder=\"0\" src=\"daan_table.aspx?editname=" + id + "&Model_id=" + txt_value + "&laiyuanbianhao=" + my_b.k_cookie("bianhao") + "\" scrolling=\"auto\" id=\"" + id + "_\"  onload=\"javascript:dyniframesize('" + id + "_'); \"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "规格")
        {
            my_b.c_cookie(my_b.get_bianhao(), "bianhao");
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.Text = my_b.k_cookie("bianhao");
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);
      
            c.Controls.Add(new LiteralControl("<iframe width=\"80%\"  frameborder=\"0\" src=\"guige_table.aspx?editname=" + id + "&classid=" + txt_value + "&Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + my_b.k_cookie("bianhao") + "\" scrolling=\"auto\" id=\"" + id + "_\"  onload=\"javascript:dyniframesize('" + id + "_'); \"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "链接")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";

            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            //显示名称
            TextBox txt1;
            //创建TextBox 
            txt1 = new TextBox();
            txt1.ID = id + "_";
            txt1.Width = 400;
            c.Controls.Add(txt1);
            //end
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"选择\"  onClick=\"window.open('" + txt_value + "?editname=" + id + "&listid='+document.getElementById('" + id + "').value+'');\"/>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "编号")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = my_b.get_bianhao();

            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "兑换卡")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = my_b.get_duihuanka();

            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "列表框")
        {

            ListBox txt;
            //创建ListBox 
            txt = new ListBox();
            txt.CssClass = "select-container1";
            txt.ID = id;
            txt.Height = 200;
            txt.SelectionMode = ListSelectionMode.Multiple;
            ListBox_set("209", 0, txt);
            //添加控件到容器 
            c.Controls.Add(txt);

        }
        else if (type == "标题")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            set_moren(txt_value, txt, id);//设置文本框的默认数据
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
            get_biaoti(c, "");
        }
        else if (type == "sql")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";

            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "文本框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;

            txt.CssClass = "input";
            set_moren(txt_value, txt, id);//设置文本框的默认数据
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "时间框")
        {
           
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            set_moren(txt_value, txt, id);//设置文本框的默认数据
            txt.Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd'})");
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "坐标")
        {

            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = txt_value;
            txt.Attributes.Add("onClick", "window.open('/inc/ditu.aspx?editname=" + id + "','','status=no,scrollbars=no,top=20,left=110,width=500,height=450');");
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "文本域")
        {

            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtinput";
            txt.TextMode = TextBoxMode.MultiLine;
            set_moren(txt_value, txt, id);//设置文本框的默认数据
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "子编辑器")
        {

            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            // txt.CssClass = "input";
            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = txt_value;
            //添加控件到容器 
            c.Height = 600;
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<script> var ue = UE.getEditor('" + txt.ClientID + "');	</script>"));
            my_b.Set_FreeTextBox("admin");
        }
        else if (type == "密码框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            //txt.TextMode = TextBoxMode.Password;
            txt.Attributes["onfocus"] = "this.type='password'";
            txt.Attributes["value"] = txt_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "编辑器")
        {

            string pic_width = "800*0";
            try
            {
                pic_width = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + HttpContext.Current.Request.QueryString["classid"] + "").Rows[0]["u10"].ToString();
            }
            catch { }
            //设置编辑上的东西
            get_bianjiqi(c);

            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            //  txt.CssClass = "input";
            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = txt_value;
            //添加控件到容器 
            c.Height = 600;
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<script> var ue = UE.getEditor('" + txt.ClientID + "');	</script>"));
            my_b.Set_FreeTextBox("admin");
        }
        else if (type == "文件框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = "";
            //添加控件到容器 
            c.Controls.Add(txt);
            if (txt_value == "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp; <a href=\"/inc/webuploader/FileUploader.aspx?type=soft&editname=" + id + "&kuangao=" + txt_value + "\" target=\"_blank\" class=\"btn btn-blue\"><em class=\"ficon  ficon-uploading\"></em> 上传" + txt_value + "</a>"));
            }
            else
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp; <a href=\"/inc/webuploader/FileUploader.aspx?type=" + txt_value + "&editname=" + id + "&kuangao=" + txt_value + "\" target=\"_blank\" class=\"btn btn-blue\"><em class=\"ficon  ficon-uploading\"></em> 上传" + txt_value + "</a>"));
            }

        }
        else if (type == "七牛上传")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = "";
            //添加控件到容器 
            c.Controls.Add(txt);
            if (txt_value == "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp; <a href=\"/inc/webuploader/videouploader_houtai.aspx?type=soft&editname=" + id + "&kuangao=" + txt_value + "\" target=\"_blank\" class=\"btn btn-blue\"><em class=\"ficon  ficon-uploading\"></em> 上传" + txt_value + "</a>"));
            }
            else
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp; <a href=\"/inc/webuploader/videouploader_houtai.aspx?type=" + txt_value + "&editname=" + id + "&kuangao=" + txt_value + "\" target=\"_blank\" class=\"btn btn-blue\"><em class=\"ficon  ficon-uploading\"></em> 上传" + txt_value + "</a>"));
            }

        }
        else if (type == "相关选择")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;

            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            //显示名称
            TextBox txt1;
            //创建TextBox 
            txt1 = new TextBox();
            txt1.ID = id + "_";
            txt1.CssClass = "input";
            c.Controls.Add(txt1);
            //end
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"选择\"  onClick=\"window.open('x_article.aspx?editname=" + id + "&listid='+document.getElementById('" + id + "').value+'&sql=" + HttpUtility.UrlEncode(txt_value) + "');\"/>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "缩略图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;&nbsp;"));
            Button bu1 = new Button();
            bu1.ID = "tqtp";
            bu1.CssClass = "btn btn-blue";
            bu1.Text = "提取图片";
            c.Controls.Add(bu1);
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "头像")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;&nbsp;"));
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "标签")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            Button bu1 = new Button();
            bu1.ID = "tqlable";
            bu1.CssClass = "button";
            bu1.Text = "提取标签";
            c.Controls.Add(bu1);

        }
        else if (type == "联动")
        {
            #region 新增联动
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<iframe width=\"100%\" height=\"24px\" src=\"/inc/liandong_table.aspx?classid=" + txt_value + "&type=add&textBox=" + id + "\" scrolling=\"no\"  frameborder=\"0\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));
            #endregion
        }
        else if (type == "组图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "yincang";
            txt.Text = txt_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<iframe width=\"800px\" height=\"50px\" src=\"uploadpic.aspx?id=if" + id + "&textBox=" + id + "&type=add\" scrolling=\"no\"  frameborder=\"0\" id=\"if" + id + "\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "单条数据")
        {
            #region 添加时的单条数据
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            string shuju = "<table class=\"table table-info cs-table\">";
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(get_url(aa[1].ToString())) + " " + aa[2].ToString();


                    dt = my_c.GetTable(sql);
                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    shuju = shuju + "<tr><th width=\"60\">" + dt.Rows[j][ziduan].ToString() + "</th><td><input name=\"" + dt.Rows[j][ziduan].ToString() + "_" + j + "\" type=\"text\" value=\"\" id=\"" + dt.Rows[j][ziduan].ToString() + "_" + j + "\" class=\"input\"></td></tr>";
                }
                shuju = shuju + "</table>";
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }
                for (int j = 0; j < aa.Length; j++)
                {
                    shuju = shuju + "<tr><th width=\"60\">" + aa[j] + "</th><td><input name=\"" + aa[j].ToString() + "_" + j + "\" type=\"text\" value=\"\" id=\"" + aa[j].ToString() + "_" + j + "\" class=\"input\"></td></tr>";
                }
                shuju = shuju + "</table>";
            }
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "yincang";
            txt.Attributes.Add("data-type", "单条数据");
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(shuju));
            c.Controls.Add(new LiteralControl(u3));
            #endregion
        }
        else if (type == "下拉框")
        {
            #region 添加时的下拉框
            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
            dro.CssClass = "select-container1";
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(get_url(aa[1].ToString())) + " " + aa[2].ToString();

                    dt = my_c.GetTable(sql);
                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    string zhi = set_ziduan(dt, ziduan, j);
                    if (ziduan.Split(',').Length > 1)
                    {
                        #region 输入单个结果
                        dro.Items.Insert(0, dt.Rows[j][ziduan.Split(',')[1].ToString()].ToString());
                        dro.Items[0].Value = dt.Rows[j][ziduan.Split(',')[0].ToString()].ToString();
                        #endregion
                    }
                    else if (ziduan.IndexOf("+") > -1)
                    {
                        #region 输入相连的结果
                        dro.Items.Insert(0, dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString());
                        if (ziduan.Split('+')[0].ToString() == "id")
                        {
                            dro.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                        {
                            dro.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else
                        {

                            dro.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        dro.Items.Insert(0, zhi);
                        dro.Items[0].Value = zhi;
                    }

                }
                dro.Items.Insert(0, "");
                dro.Items[0].Value = "";
                c.Controls.Add(dro);
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }
                for (int j = 0; j < aa.Length; j++)
                {
                    dro.Items.Insert(0, aa[j].ToString());
                    dro.Items[0].Value = aa[j].ToString();
                }
                dro.Items.Insert(0, "");
                dro.Items[0].Value = "";
                c.Controls.Add(dro);
            }
            c.Controls.Add(new LiteralControl(u3));
            #endregion
        }
        else if (type == "多选下拉框")
        {
            TextBox txt;
            //创建TextBox 

            //添加控件到容器 

            c.Controls.Add(new LiteralControl("<div class=\"testContainer\">            <div class=\"box\">  "));
            //第三个
            txt = new TextBox();
            txt.ID = id + "_list";
            Parameter_class(txt_value, 0);
            txt.Text = parameter_list;
            txt.CssClass = "yincang";
            c.Controls.Add(txt);

            //第二个
            txt = new TextBox();
            txt.ID = id + "_Test";
            txt.CssClass = "input";
            c.Controls.Add(txt);
            //第一个
            txt = new TextBox();
            txt.ID = id;

            txt.CssClass = "yincang";
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("</div>        </div> <script type=\"text/javascript\"> $(document).ready(function () {            $(\"#" + id + "_Test\").MultDropList({ data: $(\"#" + id + "_list\").val() }); });</script> "));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "单选按钮组")
        {

            RadioButtonList che;
            che = new RadioButtonList();
            che.ID = id;
            che.RepeatColumns = 5;
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();
                    dt = my_c.GetTable(sql);

                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string zhi = set_ziduan(dt, ziduan, j);
                    che.Items.Insert(0, zhi);
                    che.Items[0].Value = zhi;
                }
                che.RepeatDirection = RepeatDirection.Horizontal;
                c.Controls.Add(che);
                c.Controls.Add(new LiteralControl(u3));
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }
                for (int j = 0; j < aa.Length; j++)
                {
                    che.Items.Insert(0, aa[j].ToString());
                    che.Items[0].Value = aa[j].ToString();
                }
            }
            che.Items[0].Selected = true;
            if (id == "shenhe")
            {
                DataTable sl_user = new DataTable();
                sl_user = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + my_b.k_cookie("admin_id") + "'");
                if (sl_user.Rows[0]["u3"].ToString().IndexOf("管理员") == -1)
                {
                    che.Enabled = false;
                }

            }
            che.RepeatDirection = RepeatDirection.Horizontal;
            c.Controls.Add(che);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "多选按钮组")
        {
            #region 增加多选按钮组
            CheckBoxList che;
            che = new CheckBoxList();
            che.ID = id;
            che.RepeatColumns = 3;
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(get_url(aa[1].ToString())) + " " + aa[2].ToString();

                    dt = my_c.GetTable(sql);
                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    string zhi = set_ziduan(dt, ziduan, j);
                    if (ziduan.Split(',').Length > 1)
                    {
                        #region 输入单个结果
                        che.Items.Insert(0, dt.Rows[j][ziduan.Split(',')[1].ToString()].ToString());
                        che.Items[0].Value = dt.Rows[j][ziduan.Split(',')[0].ToString()].ToString();
                        #endregion
                    }
                    else if (ziduan.IndexOf("+") > -1)
                    {
                        #region 输入相连的结果
                        che.Items.Insert(0, dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString());
                        if (ziduan.Split('+')[0].ToString() == "id")
                        {
                            che.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                        {
                            che.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else
                        {

                            che.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        che.Items.Insert(0, zhi);
                        che.Items[0].Value = zhi;
                    }

                }

                c.Controls.Add(che);
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }
                for (int j = 0; j < aa.Length; j++)
                {
                    che.Items.Insert(0, aa[j].ToString());
                    che.Items[0].Value = aa[j].ToString();
                }
                che.RepeatDirection = RepeatDirection.Horizontal;
                c.Controls.Add(che);
                c.Controls.Add(new LiteralControl(u3));
            }
            #endregion
        }
        else
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            set_moren(txt_value, txt, id);//设置文本框的默认数据
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
    }
    #endregion
    #region 修改输入表单
    //修改时使用
    public void set_kj_edit(TableCell c, string user_value, DataTable Model_dt, int d1)
    {
        string type = Model_dt.Rows[d1]["u6"].ToString();
        string id = Model_dt.Rows[d1]["u1"].ToString();
        string txt_value = Model_dt.Rows[d1]["u8"].ToString();
        string u3 = Model_dt.Rows[d1]["u3"].ToString();
        string jiami = Model_dt.Rows[d1]["jiami"].ToString();
        user_value = string_jiemi(jiami, user_value);
        if (type == "分类")
        {
            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
            jj = 0;
            dr1("0", 0, dro);
            for (int d = 0; d < dro.Items.Count; d++)
            {
                if (dro.Items[d].Value == user_value)
                {
                    dro.Items[d].Selected = true;
                    break;
                }
            }
            c.Controls.Add(dro);
        }
        else if (type == "数字" || type == "货币")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = user_value;

            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "上级")
        {

            DataTable Model_dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString());
            string biaoming = Model_dt1.Rows[0]["u1"].ToString();
            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
            jj = 0;
            fenlei_class("0", 0, dro, biaoming);
            dro.Items.Insert(0, "顶级目录");
            dro.Items[0].Value = "0";
            for (int d = 0; d < dro.Items.Count; d++)
            {

                if (dro.Items[d].Value == user_value)
                {
                    dro.Items[d].Selected = true;
                    break;
                }
            }
            c.Controls.Add(dro);
        }
        else if (type == "框架")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<iframe width=\"100%\"  frameborder=\"0\" src=\"\" scrolling=\"auto\" id=\"" + id + "_\"></iframe><script type=\"text/javascript\">        document.getElementById('" + id + "_').src = \"" + my_b.kuangjiaurl(txt_value) + "editname=" + id + "&listid=\"+document.getElementById('" + id + "').value+\"\";</script>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "多条数据")
        {
            if (user_value == "")
            {
                user_value = my_b.get_bianhao();
            }
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<iframe width=\"80%\"  frameborder=\"0\" src=\"daan_table.aspx?editname=" + id + "&Model_id=" + txt_value + "&laiyuanbianhao=" + user_value + "\" scrolling=\"auto\" id=\"" + id + "_\"  onload=\"javascript:dyniframesize('" + id + "_'); \"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "规格")
        {
            if (user_value == "")
            {
                user_value = my_b.get_bianhao();
            }
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<iframe width=\"80%\"  frameborder=\"0\" src=\"guige_table.aspx?editname=" + id + "&classid=" + txt_value + "&Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + user_value + "\" scrolling=\"auto\" id=\"" + id + "_\"  onload=\"javascript:dyniframesize('" + id + "_'); \"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "链接")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            //显示名称
            TextBox txt1;
            //创建TextBox 
            txt1 = new TextBox();
            txt1.ID = id + "_";
            txt1.Width = 400;
            DataTable dt = new DataTable();
            if (user_value != "")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in (" + user_value + ")");
            }
            string dt_str = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt_str == "")
                {
                    dt_str = dt.Rows[i]["u1"].ToString();
                }
                else
                {
                    dt_str = dt_str + "," + dt.Rows[i]["u1"].ToString();
                }
            }
            txt1.Text = dt_str;
            c.Controls.Add(txt1);
            //end
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"选择\"  onClick=\"window.open('" + txt_value + "?editname=" + id + "&listid='+document.getElementById('" + id + "').value+'');\"/>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "列表框")
        {

            ListBox txt;
            //创建ListBox 
            txt = new ListBox();
            txt.ID = id;
            txt.Height = 200;
            txt.SelectionMode = ListSelectionMode.Multiple;
            ListBox_set("209", 0, txt);
            string[] aa = user_value.Split(',');
            for (i = 0; i < aa.Length; i++)
            {
                for (int j = 0; j < txt.Items.Count; j++)
                {
                    if (txt.Items[j].Value == aa[i])
                    {
                        txt.Items[j].Selected = true;
                        break;
                    }
                }
            }
            //添加控件到容器 
            c.Controls.Add(txt);

        }
        else if (type == "标题")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            Regex reg = new Regex("{.*?}", RegexOptions.Singleline);
            string biaoti = reg.Replace(user_value, "");
            txt.Text = biaoti;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
            get_biaoti(c, user_value);
        }
        else if (type == "sql")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "文本框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "时间框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            set_moren(txt_value, txt, id);//设置文本框的默认数据
            txt.Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "坐标")
        {

            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            txt.Attributes.Add("onClick", "window.open('/inc/ditu.aspx?editname=" + id + "','','status=no,scrollbars=no,top=20,left=110,width=500,height=450');");
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "文本域")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtinput";
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "子编辑器")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;

            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = user_value;
            //添加控件到容器 
            c.Height = 600;
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<script> var ue = UE.getEditor('" + txt.ClientID + "');</script>"));
            my_b.Set_FreeTextBox("admin");
        }
        else if (type == "密码框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            // txt.TextMode = TextBoxMode.Password;
            txt.Attributes["onfocus"] = "this.type='password'";
            txt.Attributes["value"] = "";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("密码不修改的话请为空!"));
        }
        else if (type == "编辑器")
        {

            string pic_width = "800*0";
            try
            {
                pic_width = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + HttpContext.Current.Request.QueryString["classid"] + "").Rows[0]["u10"].ToString();
            }
            catch { }
            //设置编辑上的东西
            get_bianjiqi(c);
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;

            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = user_value;
            //添加控件到容器 
            c.Height = 600;
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<script>var ue = UE.getEditor('" + txt.ClientID + "');</script>"));
            my_b.Set_FreeTextBox("admin");

        }
        else if (type == "文件框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            if (user_value == "")
            {
                if (txt_value == "")
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=soft&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>"));
                }
                else
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=" + txt_value + "&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>"));
                }
            }
            else
            {
                if (txt_value == "")
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=soft&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
                }
                else
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=" + txt_value + "&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
                }

            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "七牛上传")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            if (user_value == "")
            {
                if (txt_value == "")
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=soft&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>"));
                }
                else
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=" + txt_value + "&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>"));
                }
            }
            else
            {
                if (txt_value == "")
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/videouploader_houtai.aspx?type=soft&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
                }
                else
                {
                    c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/videouploader_houtai.aspx?type=" + txt_value + "&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
                }

            }
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "缩略图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;&nbsp;"));
            Button bu1 = new Button();
            bu1.ID = "tqtp";
            bu1.CssClass = "btn btn-blue";
            bu1.Text = "提取图片";
            c.Controls.Add(bu1);
            if (user_value != "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;<a href='showpic.aspx?url=" + user_value + "' target=_blank><img onerror=\"this.src='images/nopic.jpg';\"  src='" + user_value + "' height='60px'></a>"));
            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "头像")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "dinput";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&kuangao=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');\"/>&nbsp;&nbsp;"));

            if (user_value != "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;<a href='showpic.aspx?url=" + user_value + "' target=_blank><img onerror=\"this.src='images/nopic.jpg';\"  src='" + user_value + "' height='60px'></a>"));
            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "标签")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            Button bu1 = new Button();
            bu1.ID = "tqlable";
            bu1.CssClass = "button";
            bu1.Text = "提取标签";
            c.Controls.Add(bu1);

        }
        else if (type == "联动")
        {
            #region 修改联动
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            DataTable Field_dt = my_c.GetTable("select * from  " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + " and u1='" + id + "'");
            string field_u8 = "";
            if (Field_dt.Rows.Count > 0)
            {
                field_u8 = Field_dt.Rows[0]["u8"].ToString();
            }
            if (user_value == field_u8)
            {
                user_value = "";
            }
            txt.Text = user_value;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            if (user_value == "0")
            {
                user_value = "";
            }
            c.Controls.Add(new LiteralControl("<iframe width=\"100%\" height=\"24px\" src=\"/inc/liandong_table.aspx?classid=" + field_u8 + "&type=edit&textBox=" + id + "&neirong=" + user_value + "\" scrolling=\"no\"  frameborder=\"0\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));
            #endregion
        }
        else if (type == "组图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "yincang";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<iframe width=\"800px\" height=\"50px\" src=\"uploadpic.aspx?id=if" + id + "&textBox=" + id + "&type=edit\" scrolling=\"no\"  frameborder=\"0\" id=\"if" + id + "\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "相关选择")
        {

            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;

            txt.Text = user_value;
            txt.CssClass = "yincang";
            //添加控件到容器 
            c.Controls.Add(txt);

            //显示名称
            TextBox txt1;
            //创建TextBox 
            txt1 = new TextBox();
            txt1.ID = id + "_";
            txt1.CssClass = "input";
            string table_name = get_tablename(txt_value);
            DataTable dt = new DataTable();

            if (user_value != "")
            {

                if (table_name.IndexOf("where") > -1)
                {
                    dt = my_c.GetTable("select * from " + table_name + " and id in (" + user_value + ")");
                }
                else
                {
                    dt = my_c.GetTable("select * from " + table_name + " where id in (" + user_value + ")");
                }

            }
            string dt_str = "";

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt_str == "")
                    {
                        dt_str = dt.Rows[i][get_sql(1, txt_value)].ToString();
                    }
                    else
                    {
                        dt_str = dt_str + "," + dt.Rows[i][get_sql(1, txt_value)].ToString();
                    }
                }
            }
            catch
            {

            }

            txt1.Text = dt_str;
            c.Controls.Add(txt1);
            //end

            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"btn btn-blue\" type=\"button\" value=\"选择\"  onClick=\"window.open('x_article.aspx?editname=" + id + "&listid='+document.getElementById('" + id + "').value+'&sql=" + HttpUtility.UrlEncode(txt_value) + "');\"/>"));

            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "单条数据")
        {
            #region 修改时的单条数据
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            string shuju = "<table class=\"table table-info cs-table\">";
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(get_url(aa[1].ToString())) + " " + aa[2].ToString();


                    dt = my_c.GetTable(sql);
                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    shuju = shuju + "<tr><th width=\"60\">" + dt.Rows[j][ziduan].ToString() + "</th><td><input name=\"" + dt.Rows[j][ziduan].ToString() + "_" + j + "\" type=\"text\" value=\"\" id=\"" + dt.Rows[j][ziduan].ToString() + "_" + j + "\" class=\"input\"></td></tr>";
                }
                shuju = shuju + "</table>";
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }
                for (int j = 0; j < aa.Length; j++)
                {
                    shuju = shuju + "<tr><th width=\"60\">" + aa[j] + "</th><td><input name=\"" + aa[j].ToString() + "_" + j + "\" type=\"text\" value=\"\" id=\"" + aa[j].ToString() + "_" + j + "\" class=\"input\"></td></tr>";
                }
                shuju = shuju + "</table>";
            }
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "yincang";
            txt.Text = user_value;
            txt.Attributes.Add("data-type", "单条数据");
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(shuju));
            c.Controls.Add(new LiteralControl(u3));
            #endregion
        }
        else if (type == "下拉框")
        {
            #region 修改时的下拉框
            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
            dro.CssClass = "select-container1";
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();

                    dt = my_c.GetTable(sql);

                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string zhi = set_ziduan(dt, ziduan, j);
                    if (ziduan.Split(',').Length > 1)
                    {
                        #region 输入单个结果
                        dro.Items.Insert(0, dt.Rows[j][ziduan.Split(',')[1].ToString()].ToString());
                        dro.Items[0].Value = dt.Rows[j][ziduan.Split(',')[0].ToString()].ToString();
                        #endregion
                    }
                    else if (ziduan.IndexOf("+") > -1)
                    {
                        #region 输入相连的结果
                        dro.Items.Insert(0, dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString());
                        if (ziduan.Split('+')[0].ToString() == "id")
                        {
                            dro.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                        {
                            dro.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else
                        {

                            dro.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        dro.Items.Insert(0, zhi);
                        dro.Items[0].Value = zhi;
                    }

                }
                dro.Items.Insert(0, "");
                dro.Items[0].Value = "";


                for (int d = 0; d < dro.Items.Count; d++)
                {
                    if (dro.Items[d].Value == user_value.Trim())
                    {
                        dro.Items[d].Selected = true;
                        break;
                    }
                }
                c.Controls.Add(dro);
                c.Controls.Add(new LiteralControl(u3));
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }
                for (int j = 0; j < aa.Length; j++)
                {
                    dro.Items.Insert(0, aa[j].ToString());
                    dro.Items[0].Value = aa[j].ToString();
                }
                dro.Items.Insert(0, "");
                dro.Items[0].Value = "";
                for (int d = 0; d < dro.Items.Count; d++)
                {
                    if (dro.Items[d].Value == user_value)
                    {
                        dro.Items[d].Selected = true;
                        break;
                    }
                }
            }

            c.Controls.Add(dro);
            c.Controls.Add(new LiteralControl(u3));
            #endregion
        }
        else if (type == "多选下拉框")
        {
            TextBox txt;
            //创建TextBox 

            //添加控件到容器 

            c.Controls.Add(new LiteralControl("<div class=\"testContainer\">            <div class=\"box\">  "));
            //第三个
            txt = new TextBox();
            txt.ID = id + "_list";
            Parameter_class(txt_value, 0);
            txt.Text = parameter_list;
            txt.CssClass = "yincang";
            c.Controls.Add(txt);

            //第二个
            txt = new TextBox();
            txt.ID = id + "_Test";
            txt.CssClass = "input";
            c.Controls.Add(txt);
            //第一个
            txt = new TextBox();
            txt.ID = id;
            txt.Text = user_value;
            txt.CssClass = "yincang";
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("</div>        </div> <script type=\"text/javascript\"> $(document).ready(function () {            $(\"#" + id + "_Test\").MultDropList({ data: $(\"#" + id + "_list\").val() }); });</script> "));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "单选按钮组")
        {
            RadioButtonList che;
            che = new RadioButtonList();
            che.ID = id;
            che.RepeatColumns = 5;
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();
                    dt = my_c.GetTable(sql);

                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string zhi = set_ziduan(dt, ziduan, j);
                    che.Items.Insert(0, zhi);
                    che.Items[0].Value = zhi;
                }
                for (int j = 0; j < che.Items.Count; j++)
                {
                    if (user_value == che.Items[j].Value)
                    {
                        che.Items[j].Selected = true;
                        break;
                    }
                }
                if (id == "shenhe")
                {
                    DataTable sl_user = new DataTable();
                    sl_user = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + my_b.k_cookie("admin_id") + "'");
                    if (sl_user.Rows[0]["u3"].ToString().IndexOf("管理员") == -1)
                    {
                        che.Enabled = false;
                    }

                }
                che.RepeatDirection = RepeatDirection.Horizontal;
                c.Controls.Add(che);
                c.Controls.Add(new LiteralControl(u3));
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }

                for (int j = 0; j < aa.Length; j++)
                {
                    che.Items.Insert(0, aa[j].ToString());
                    che.Items[0].Value = aa[j].ToString();
                }
                for (int j = 0; j < che.Items.Count; j++)
                {
                    if (user_value == che.Items[j].Value)
                    {
                        che.Items[j].Selected = true;
                        break;
                    }
                }


                che.RepeatDirection = RepeatDirection.Horizontal;
                c.Controls.Add(che);
                c.Controls.Add(new LiteralControl(u3));
            }
        }
        else if (type == "多选按钮组")
        {
            #region 修改时多选按钮组
            CheckBoxList che;
            che = new CheckBoxList();
            che.ID = id;
            che.RepeatColumns = 3;
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {
                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {
                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();

                    dt = my_c.GetTable(sql);

                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string zhi = set_ziduan(dt, ziduan, j);
                    if (ziduan.Split(',').Length > 1)
                    {
                        #region 输入单个结果
                        che.Items.Insert(0, dt.Rows[j][ziduan.Split(',')[1].ToString()].ToString());
                        che.Items[0].Value = dt.Rows[j][ziduan.Split(',')[0].ToString()].ToString();
                        #endregion
                    }
                    else if (ziduan.IndexOf("+") > -1)
                    {
                        #region 输入相连的结果
                        che.Items.Insert(0, dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString());
                        if (ziduan.Split('+')[0].ToString() == "id")
                        {
                            che.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                        {
                            che.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString();
                        }
                        else
                        {

                            che.Items[0].Value = dt.Rows[j][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j][ziduan.Split('+')[1].ToString()].ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        che.Items.Insert(0, zhi);
                        che.Items[0].Value = zhi;
                    }

                }



                for (int d = 0; d < che.Items.Count; d++)
                {
                    Regex reg3 = new Regex("我是分隔符", RegexOptions.Singleline);
                    string[] cc = reg3.Split(user_value.Trim().Replace("$我是分隔符$", "我是分隔符"));

                    for (int j1 = 0; j1 < cc.Length; j1++)
                    {
                        if (che.Items[d].Value == cc[j1])
                        {
                            che.Items[d].Selected = true;
                            break;
                        }
                    }

                }
                c.Controls.Add(che);
                c.Controls.Add(new LiteralControl(u3));
            }
            else
            {
                string[] aa = Regex.Split(txt_value, "\r\n");
                if (txt_value.IndexOf("\r\n") == -1)
                {
                    aa = txt_value.Split('|');
                }
                for (int j = 0; j < aa.Length; j++)
                {
                    che.Items.Insert(0, aa[j].ToString());
                    che.Items[0].Value = aa[j].ToString();


                }
                for (int j = 0; j < che.Items.Count; j++)
                {
                    if (user_value.IndexOf(che.Items[j].Value) > -1)
                    {
                        che.Items[j].Selected = true;
                        //      break;
                    }
                }

                che.RepeatDirection = RepeatDirection.Horizontal;
                c.Controls.Add(che);
                c.Controls.Add(new LiteralControl(u3));
            }
            #endregion
        }
        else
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "input";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
    }
    #endregion
    string pic_m = "";
    #region 处理验证和非空
    public void kj_yanzheng(string id, string u4, string u7, string neirong)
    {
        if (u4 == "是")
        {
            Regex regex = new Regex(@"^\s*$");

            if (regex.IsMatch(neirong))
            {
                //HttpContext.Current.Response.Write("<script>kj_yanzheng('不可以为空')</script>");
                //HttpContext.Current.Response.End();
                //空的
            }

        }
        else
        {

        }
    }
    #endregion
    #region 执行加密
    public string string_jiami(string jiami, string zhi)
    {
        zhi = my_b.c_string(zhi);
        if (jiami == "是")
        {
            return jm.Encrypt(zhi);
        }
        else
        {
            return zhi;
        }
    }
    #endregion
    #region 执行解密
    public string string_jiemi(string jiami, string zhi)
    {
        if (jiami == "是")
        {
            return jm.Decrypt(zhi);
        }
        else
        {
            return zhi;
        }
    }
    #endregion
    #region 获取表单
    public string get_kj(Table c, DataTable Model_dt, int d1)
    {
        string type = Model_dt.Rows[d1]["u6"].ToString();
        string id = Model_dt.Rows[d1]["u1"].ToString();
        string u4 = Model_dt.Rows[d1]["u4"].ToString();
        string u7 = Model_dt.Rows[d1]["u7"].ToString();
        string jiami = Model_dt.Rows[d1]["jiami"].ToString();

        if (type == "子编辑器")
        {

            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            kj_yanzheng(id, u4, u7, txt.Text.ToString());
            string t1 = my_b.c_string(txt.Text.ToString());

            return "'" + string_jiami(jiami, t1) + "'";
        }
        else if (type == "子编辑器")
        {

            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            kj_yanzheng(id, u4, u7, txt.Text.ToString());
            string t1 = my_b.c_string(txt.Text.ToString());
            t1 = my_b.shuiyin_list(t1);
            return "'" + string_jiami(jiami, t1) + "'";
        }
        else if (type == "时间框")
        {

            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (my_b.c_string(txt.Text.ToString()) == "")
            {
                return "null";
            }
            else
            {
                return "'" + string_jiami(jiami, txt.Text.ToString()) + "'";
            }

        }
        else if (type == "多条数据")
        {
            my_b.admin_o_cookie("bianhao");
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            kj_yanzheng(id, u4, u7, txt.Text.ToString());
            return "'" + string_jiami(jiami, txt.Text.ToString()) + "'";
        }
        else if (type == "会员卡" || type == "编号" || type == "兑换卡")
        {
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {
                TextBox txt = new TextBox();
                txt = (TextBox)c.FindControl(id);
                return "'" + string_jiami(jiami, txt.Text.ToString()) + "'";
            }
            else
            {
                TextBox txt = new TextBox();
                txt = (TextBox)c.FindControl(id);
                //if (my_c.GetTable("select id from sl_bianhao where bianhao='" + my_b.c_string(txt.Text.ToString()) + "'").Rows.Count > 0)
                //{
                //    HttpContext.Current.Response.Redirect("err.aspx?err=会员卡号或编号重复，请确认后重新输入！&errurl=" + my_b.tihuan("auto_table_add.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
                //}
                //else
                //{
                //    my_c.genxin("insert into sl_bianhao (bianhao) values('" + my_b.c_string(txt.Text.ToString()) + "')");
                //}
                kj_yanzheng(id, u4, u7, txt.Text.ToString());
                return "'" + string_jiami(jiami, txt.Text.ToString()) + "'";
            }
        }
        else if (type == "分类")
        {
            DropDownList dr = new DropDownList();
            dr = (DropDownList)c.FindControl(id);
            return string_jiami(jiami, dr.SelectedValue);
        }
        else if (type == "上级")
        {
            DropDownList dr = new DropDownList();
            dr = (DropDownList)c.FindControl(id);
            if (dr.SelectedValue == "")
            {
                return "0";
            }
            return string_jiami(jiami, dr.SelectedValue);
        }
        else if (type == "多选下拉框")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            string txt_text = "";
            get_ding_dir_str = "";
            string[] aa = txt.Text.Split(',');
            for (int i = 0; i < aa.Length; i++)
            {
                Parameterweizhi(aa[i]);
            }
            if (get_ding_dir_str != "")
            {
                DataTable sl_Parameter = my_c.GetTable("select id from sl_Parameter where id in (" + get_ding_dir_str + ")");
                get_ding_dir_str = "";
                for (int i = 0; i < sl_Parameter.Rows.Count; i++)
                {
                    if (get_ding_dir_str == "")
                    {
                        get_ding_dir_str = sl_Parameter.Rows[i]["id"].ToString();
                    }
                    else
                    {
                        get_ding_dir_str = get_ding_dir_str + "," + sl_Parameter.Rows[i]["id"].ToString();
                    }
                }
            }
            //Response.Write(get_ding_dir_str);
            //Response.End();
            txt_text = get_ding_dir_str;
            kj_yanzheng(id, u4, u7, txt_text);
            return "'" + string_jiami(jiami, txt_text) + "'";
        }
        else if (type == "标签")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            string biaoqian = my_b.c_string(txt.Text.ToString());
            if (biaoqian == "")
            {
                DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + HttpContext.Current.Request.QueryString["classid"] + "");

                string neirong = "";
                DataTable Field_dt = my_c.GetTable("select * from  " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + " and u6 like '编辑器'");


                string bianjiqi_id = "";
                if (Field_dt.Rows.Count > 0)
                {
                    bianjiqi_id = Field_dt.Rows[0]["u1"].ToString();
                    TextBox txt1 = new TextBox();
                    txt1 = (TextBox)c.FindControl(bianjiqi_id);
                    neirong = my_b.c_string(txt1.Text.ToString());
                }
                //Response.Write(Label1.Text);
                //Response.End();

                biaoqian = my_b.get_Keywords(neirong);
            }
            kj_yanzheng(id, u4, u7, biaoqian);
            return "'" + string_jiami(jiami, biaoqian) + "'";
        }
        else if (type == "密码框")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {
                DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString()) + " where id=" + HttpContext.Current.Request.QueryString["id"].ToString());
                if (my_b.c_string(txt.Text.ToString()) == "")
                {
                    return "'" + dt.Rows[0][id].ToString() + "'";
                }

            }
            kj_yanzheng(id, u4, u7, txt.Text.ToString());
            return "'" + my_b.md5(my_b.c_string(txt.Text.ToString())) + "'";
        }
        else if (type == "标题")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            string fzw_yanse = "";
            string fzw_jiacu = "";
            string biaoti = txt.Text;
            kj_yanzheng(id, u4, u7, txt.Text.ToString());

            TextBox txt1 = new TextBox();
            txt1 = (TextBox)c.FindControl("fzw_yanse");
            fzw_yanse = my_b.c_string(txt1.Text.ToString());

            CheckBox ch1 = new CheckBox();
            ch1 = (CheckBox)c.FindControl("fzw_jiacu");
            if (ch1.Checked)
            {
                fzw_jiacu = "yes";
            }
            if (fzw_yanse != "")
            {
                biaoti = biaoti + "{color:" + fzw_yanse + "}";
            }
            if (fzw_jiacu != "")
            {
                biaoti = biaoti + "{b:" + fzw_jiacu + "}";
            }

            return "'" + string_jiami(jiami, biaoti) + "'";
        }
        else if (type == "列表框")
        {
            ListBox txt = new ListBox();
            txt = (ListBox)c.FindControl(id);
            string t1 = "";
            for (i = 0; i < txt.Items.Count; i++)
            {
                if (txt.Items[i].Selected)
                {
                    if (t1 == "")
                    {
                        t1 = txt.Items[i].Value;
                    }
                    else
                    {
                        t1 = t1 + "," + txt.Items[i].Value;
                    }
                }
            }
            kj_yanzheng(id, u4, u7, t1);
            return "'" + string_jiami(jiami, t1) + "'";
        }
        else if (type == "缩略图")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);

            //如果有图直接返回
            if (txt.Text != "")
            {
                return "'" + txt.Text + "'";
            }
            //end
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            string classid = "";
            try
            {
                classid = HttpContext.Current.Request.QueryString["classid"].ToString();
            }
            catch { }
            string img_top = my_b.c_string(txt.Text.ToString());
            if (classid != "")
            {
                //如果classid不存在
                DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + HttpContext.Current.Request.QueryString["classid"] + "");



                string bianjiqi = "";
                DataTable Field_dt = my_c.GetTable("select * from  " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + " and u6 like '编辑器'");


                string bianjiqi_id = "";
                if (Field_dt.Rows.Count > 0)
                {
                    bianjiqi_id = Field_dt.Rows[0]["u1"].ToString();
                    TextBox txt1 = new TextBox();
                    txt1 = (TextBox)c.FindControl(bianjiqi_id);
                    bianjiqi = my_b.c_string(txt1.Text.ToString());
                }
                //Response.Write(Label1.Text);
                //Response.End();

                CheckBox ch1 = new CheckBox();
                ch1 = (CheckBox)c.FindControl("tu3");
                try
                {
                    if (ch1.Checked)
                    {

                        string slt_size = sort_dt.Rows[0]["u8"].ToString();
                        if (slt_size != "")
                        {
                            try
                            {
                                img_top = my_b.set_images(my_b.get_images(bianjiqi, 1), slt_size);
                            }
                            catch { }

                            string pic_width = sort_dt.Rows[0]["u8"].ToString();
                            if (img_top != "")
                            {
                                img_top = my_b.set_onepic_size(img_top, pic_width);
                            }
                        }

                    }
                    else
                    {
                        string pic_width = sort_dt.Rows[0]["u8"].ToString();
                        if (img_top != "")
                        {
                            img_top = my_b.set_onepic_size(img_top, pic_width);
                        }
                    }
                }
                catch
                {
                    string pic_width = sort_dt.Rows[0]["u8"].ToString();
                    if (img_top != "")
                    {
                        img_top = my_b.set_onepic_size(img_top, pic_width);
                    }
                }
                //如果classid不存在
            }


            kj_yanzheng(id, u4, u7, img_top);
            return "'" + img_top + "'";
        }
        else if (type == "编辑器")
        {
            string pic_width = "800*0";
            string classid = "";
            try
            {
                classid = HttpContext.Current.Request.QueryString["classid"];
                DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + "");

                if (sort_dt.Rows.Count > 0)
                {
                    pic_width = sort_dt.Rows[0]["u10"].ToString();
                }
            }
            catch { }


            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            string t1 = my_b.c_string(txt.Text.ToString());

            CheckBox ch1 = new CheckBox();
            ch1 = (CheckBox)c.FindControl("tu1");
            if (ch1.Checked)
            {
                t1 = my_b.Download_pic(t1);
            }
            ch1 = (CheckBox)c.FindControl("tu2");
            if (ch1.Checked)
            {
                t1 = my_b.set_pic_size(t1, pic_width);
            }
            t1 = my_b.shuiyin_list(t1);
            kj_yanzheng(id, u4, u7, t1);
            return "'" + string_jiami(jiami, t1) + "'";
        }
        else if (type == "数字" || type == "货币" || type == "联动")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            if (my_b.c_string(txt.Text.ToString()) == "")
            {
                return string_jiami(jiami, "0");
            }
            else
            {
                return string_jiami(jiami, txt.Text.ToString());
            }

        }
        else if (type == "多选按钮组")
        {
            CheckBoxList che = new CheckBoxList();
            che = (CheckBoxList)c.FindControl(id);
            string t1 = "";
            for (int i = 0; i < che.Items.Count; i++)
            {
                if (che.Items[i].Selected)
                {
                    if (t1 == "")
                    {
                        t1 = che.Items[i].Text;
                    }
                    else
                    {
                        t1 = t1 + "$我是分隔符$" + che.Items[i].Text;
                    }
                }
            }
            kj_yanzheng(id, u4, u7, t1);
            return "'" + string_jiami(jiami, t1) + "'";
        }
        else if (type == "单选按钮组")
        {
            try
            {
                RadioButtonList rad = new RadioButtonList();
                rad = (RadioButtonList)c.FindControl(id);
                return "'" + string_jiami(jiami, rad.SelectedItem.Value) + "'";
            }
            catch
            {
                return "''";
            }
        }
        else if (type == "下拉框")
        {
            DropDownList dro = new DropDownList();
            dro = (DropDownList)c.FindControl(id);
            return "'" + string_jiami(jiami, dro.SelectedItem.Value) + "'";
        }
        else
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            try
            {
                type = HttpContext.Current.Request.QueryString["type"].ToString();
            }
            catch
            { }
            kj_yanzheng(id, u4, u7, txt.Text.ToString());
            return "'" + string_jiami(jiami, txt.Text.ToString()) + "'";
        }

        return "";
    }
    #endregion
    #region 获取前台页面GET POST值
    public string page_get_kj(DataTable Model_dt, int d1)
    {
        string type = Model_dt.Rows[d1]["u6"].ToString();
        string id = Model_dt.Rows[d1]["u1"].ToString();
        string jiami = Model_dt.Rows[d1]["jiami"].ToString();
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框")
        {

            return "'" + string_jiami(jiami, HttpContext.Current.Request[id].ToString()) + "'";
        }
        else if (type == "编辑器" || type == "子编辑器")
        {
            string classid = "0";
            try
            {
                classid = HttpContext.Current.Request["classid"].ToString();
            }
            catch { }
            DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + "");
            string pic_width = "800*0";
            if (sort_dt.Rows.Count > 0)
            {
                pic_width = sort_dt.Rows[0]["u10"].ToString();
            }

            return "'" + string_jiami(jiami, my_b.set_pic_size(HttpContext.Current.Request[id].ToString(), pic_width)) + "'";
        }
        else if (type == "数字")
        {
            return "" + string_jiami(jiami, HttpContext.Current.Request[id].ToString()) + "";
        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(my_b.c_string(HttpContext.Current.Request[id].ToString())) + "'";
        }
        else
        {
            return "'" + string_jiami(jiami, HttpContext.Current.Request[id].ToString()) + "'";
        }
    }
    #endregion
    #region 获取前台页面传输的值
    public string value_get_kj(DataTable Model_dt, int d1, string txt_value)
    {
        string type = Model_dt.Rows[d1]["u6"].ToString();
        string id = Model_dt.Rows[d1]["u1"].ToString();
        string jiami = Model_dt.Rows[d1]["jiami"].ToString();
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域")
        {

            return "'" + string_jiami(jiami, txt_value) + "'";
        }
        else if (type == "编辑器" || type == "子编辑器")
        {
            string classid = "0";
            try
            {
                classid = HttpContext.Current.Request["classid"].ToString();
            }
            catch { }
            DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + "");
            string pic_width = "800*0";
            if (sort_dt.Rows.Count > 0)
            {
                pic_width = sort_dt.Rows[0]["u10"].ToString();
            }

            return "'" + string_jiami(jiami, my_b.set_pic_size(txt_value, pic_width)) + "'";
        }
        else if (type == "货币" || type == "数字" || type == "分类")
        {
            try
            {
                float huobi = float.Parse(txt_value);
                return "" + string_jiami(jiami, txt_value) + "";
            }
            catch
            {
                return "0";
            }

        }
        else if (type == "时间框")
        {
            DateTime shijian = new DateTime();
            try
            {
                shijian = DateTime.Parse(txt_value);
                return "'" + string_jiami(jiami, txt_value) + "'";
            }
            catch
            {
                return "null";
            }

        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(my_b.c_string(txt_value)) + "'";
        }
        else if (type == "联动")
        {
            try
            {
                float huobi = float.Parse(txt_value);
                return "'" + txt_value + "'";
            }
            catch
            {
                if (id == "shebeifenleibianma")
                {
                    #region shebeifenleibianma
                    if (txt_value != "")
                    {
                        string[] cc = txt_value.Split('-');
                        if (cc.Length == 3)
                        {
                            #region 三层
                            DataTable dt = my_c.GetTable("select * from sl_jichushuju where classid in (select id from sl_jichushuju where biaoti='" + cc[1] + "' and classid in (select id from sl_jichushuju where biaoti='" + cc[0] + "')) and biaoti='" + cc[2] + "'");
                            if (dt.Rows.Count > 0)
                            {
                                return "'" + dt.Rows[0]["id"].ToString() + "'";
                            }
                            else
                            {
                                return "'0'";
                            }
                            #endregion
                        }
                        if (cc.Length == 2)
                        {
                            #region 两层
                            DataTable dt = my_c.GetTable("select * from sl_jichushuju where classid in (select id from sl_jichushuju where biaoti='" + cc[0] + "' ) and biaoti='" + cc[1] + "'");
                            if (dt.Rows.Count > 0)
                            {
                                return "'" + dt.Rows[0]["id"].ToString() + "'";
                            }
                            else
                            {
                                return "'0'";
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        return "'0'";
                    }
                    #endregion
                }
                else
                {
                    #region 其它字段
                    return "'0'";
                    #endregion
                }

            }
            return "'0'";
            //liandong end
        }
        else
        {
            return "'" + string_jiami(jiami, txt_value) + "'";
        }
    }
    #endregion
    //获取相关选择中的表名
    public string get_tablename(string sql)
    {
        string sql_str = "";
        Regex reg = new Regex("from.*?order|from.*", RegexOptions.Singleline);
        Match sql_ma = reg.Match(sql);
        sql_str = sql_ma.ToString().Replace("order", "");
        sql_str = sql_str.ToString().Replace("from", "");
        sql_str = sql_str.Trim();
        return sql_str;
    }
    //end
    //获取相关选择中的字段名
    public string get_sql(int dijige, string sql)
    {

        string sql_str = "";
        Regex reg = new Regex("select.*?from", RegexOptions.Singleline);
        Match sql_ma = reg.Match(sql);
        sql_str = sql_ma.ToString().Replace("select", "");
        sql_str = sql_str.ToString().Replace("from", "");
        sql_str = sql_str.Trim();
        string[] aa = sql_str.Split(',');
        return aa[dijige];
    }
    //处理列表页问题
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
    #region 列表头部
    public string list_biaotou(DataTable dt2)
    {

        string biaotou = "";
        for (i = 0; i < dt2.Rows.Count; i++)
        {
            string u11 = dt2.Rows[i]["u11"].ToString();
            if (my_b.set_fangwen() == 1)
            {
                if (u11.Split(',').Length > 1)
                {
                    u11 = u11.Split(',')[1].ToString();
                }
            }
            string xianshi = "";
            if (dt2.Rows[i]["u13"].ToString() == "是")
            {
                xianshi = "style=' word-break:break-all' class=\"hidden-xs\"";
            }
            else
            {
                if (dt2.Rows[i]["u6"].ToString() != "编辑器" || dt2.Rows[i]["u6"].ToString() != "子编辑器")
                {

                    xianshi = "style=' width:" + u11 + "'";
                }
                else
                {
                    xianshi = "style=' width:" + u11 + "; '";
                }

            }
            if (i + 1 == dt2.Rows.Count)
            {
                biaotou = biaotou + "<th " + xianshi + ">" + dt2.Rows[i]["u2"].ToString() + "</th><th>操作</th></tr></thead>";
            }
            else
            {
                biaotou = biaotou + "<th " + xianshi + ">" + dt2.Rows[i]["u2"].ToString() + "</th>";
            }

        }
        return biaotou;
    }
    public string list_canshu_biaotou(DataTable dt2)
    {

        string biaotou = "";
        for (i = 0; i < dt2.Rows.Count; i++)
        {
            string u11 = dt2.Rows[i]["u11"].ToString();
            if (my_b.set_fangwen() == 1)
            {
                if (u11.Split(',').Length > 1)
                {
                    u11 = u11.Split(',')[1].ToString();
                }
            }
            string xianshi = "";
            if (dt2.Rows[i]["u13"].ToString() == "是")
            {
                xianshi = "style=' word-break:break-all' class=\"hidden-xs\"";
            }
            else
            {
                if (dt2.Rows[i]["u6"].ToString() != "编辑器" || dt2.Rows[i]["u6"].ToString() != "子编辑器")
                {

                    xianshi = "style=' width:" + u11 + "'";
                }
                else
                {
                    xianshi = "style=' width:" + u11 + "; '";
                }

            }
            if (i == 0)
            {
                biaotou = biaotou + "<th " + xianshi + ">" + dt2.Rows[i]["u2"].ToString() + "</th><th>下级</th>";
            }
            else if (i + 1 == dt2.Rows.Count)
            {
                biaotou = biaotou + "<th " + xianshi + ">" + dt2.Rows[i]["u2"].ToString() + "</th><th>操作</th></tr></thead>";
            }
            else
            {
                biaotou = biaotou + "<th " + xianshi + ">" + dt2.Rows[i]["u2"].ToString() + "</th>";
            }

        }
        return biaotou;
    }

    public string list_biaotou_caozuo(DataTable dt2)
    {

        string biaotou = "";
        for (i = 0; i < dt2.Rows.Count; i++)
        {
            string u11 = dt2.Rows[i]["u11"].ToString();
            if (my_b.set_fangwen() == 1)
            {
                if (u11.Split(',').Length > 1)
                {
                    u11 = u11.Split(',')[1].ToString();
                }
            }
            string xianshi = "";
            if (dt2.Rows[i]["u13"].ToString() == "是")
            {
                xianshi = "style=' word-break:break-all' class=\"hidden-xs\"";
            }
            else
            {
                if (dt2.Rows[i]["u6"].ToString() != "编辑器" || dt2.Rows[i]["u6"].ToString() != "子编辑器")
                {

                    xianshi = "style=' width:" + u11 + "'";
                }
                else
                {
                    xianshi = "style=' width:" + u11 + "; '";
                }

            }
            if (i + 1 == dt2.Rows.Count)
            {
                biaotou = biaotou + "<th " + xianshi + ">" + dt2.Rows[i]["u2"].ToString() + "</th></tr></thead>";
            }
            else
            {
                biaotou = biaotou + "<th " + xianshi + ">" + dt2.Rows[i]["u2"].ToString() + "</th>";
            }

        }
        return biaotou;
    }
    #endregion
    #region 处理列表页页的字段展示 
    //u6是字段名  u13是否在手机上隐藏 u11展示的宽度 zhi内容 id编号 u12自定义链接  u8默认值
    public string list_from(string zhi, string id, DataTable dt1, int j, DataTable Model_dt_Field, int Field_d1)
    {
        string u6 = Model_dt_Field.Rows[Field_d1]["u6"].ToString();
        string u13 = Model_dt_Field.Rows[Field_d1]["u13"].ToString();
        string u11 = Model_dt_Field.Rows[Field_d1]["u11"].ToString();
        string u12 = Model_dt_Field.Rows[Field_d1]["u12"].ToString();
        string u8 = Model_dt_Field.Rows[Field_d1]["u8"].ToString();
        string jiami = Model_dt_Field.Rows[Field_d1]["jiami"].ToString();
        string liebiaocaozuo = Model_dt_Field.Rows[Field_d1]["liebiaocaozuo"].ToString();
        zhi = string_jiemi(jiami, zhi);
        if (my_b.set_fangwen() == 1)
        {
            if (u11.Split(',').Length > 1)
            {
                u11 = u11.Split(',')[1].ToString();
            }
        }
        string from_str = "";
        string xianshi = "";
        if (u13 == "是")
        {
            xianshi = "style=' word-break:break-all' class=\"hidden-xs\"";
        }
        else
        {
            if (u6 != "编辑器" || u6 != "子编辑器")
            {
                xianshi = "style=' width:" + u11 + "; text-align:center'";
            }
            else
            {
                xianshi = "style=' width:" + u11 + "; '";
            }

        }


        if (u6 == "缩略图" || u6 == "头像")
        {
            #region 缩略图头像
            from_str = "<td " + xianshi + "><a href='" + my_b.set_ApplicationPath(zhi.Replace("$我是分隔符$", "|")) + "' target='_blank'><img onerror=\"this.src='images/nopic.jpg';\"  src='" + my_b.set_ApplicationPath(zhi.Replace("$我是分隔符$", "|")) + "' height='60px'></a></td>";
            #endregion
        }
        else if (u6 == "联动")
        {
            #region 联动
            #region 获取联动的字段和表名
            string table_name = "";
            string ziduan = "";
            string classid = "";
            string[] id_ = my_b.set_url_css(u8).Split('|');
            classid = id_[0].ToString();

            if (id_.Length > 1)
            {
                table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + id_[1].ToString().Replace("sl_", "");
                DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
                string Model_id = model_table.Rows[0]["id"].ToString();
                DataTable Model_dt = new DataTable();
                Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + "  order by u9,id");
                ziduan = Model_dt.Rows[0]["u1"].ToString();
            }
            else
            {
                table_name = "sl_Parameter";
                ziduan = "u1";
            }
            #endregion
            if (zhi == "")
            {
                from_str = "<td " + xianshi + "></td>";
            }
            else
            {
                string liandong = "";
                DataTable sl_Parameter = my_c.GetTable("select * from " + table_name + " where id=" + zhi + "");
                //HttpContext.Current.Response.Write("select * from " + table_name + " where id=" + zhi + "");
                //HttpContext.Current.Response.End();
                if (sl_Parameter.Rows.Count > 0)
                {
                    DataTable sl_Parameter1 = my_c.GetTable("select * from " + table_name + " where id=" + sl_Parameter.Rows[0]["classid"].ToString() + "");
                    if (sl_Parameter1.Rows.Count > 0)
                    {
                        liandong = sl_Parameter1.Rows[0][ziduan].ToString() + "-" + sl_Parameter.Rows[0][ziduan].ToString();
                    }
                }
                from_str = "<td " + xianshi + ">" + set_m_url(u12, dt1, j, u12, liandong) + "</td>";
            }

            #endregion
        }
        else if (u6 == "时间框")
        {
            #region 时间框
            from_str = "<td " + xianshi + ">" + set_m_url(u12, dt1, j, u12, my_b.set_time(zhi.Replace("$我是分隔符$", "|"), "yyyy-MM-dd")) + "</td>";
            #endregion
        }
        else if (u6 == "多选下拉框")
        {
            #region 多选下拉框
            if (zhi == "")
            {
                from_str = "<td " + xianshi + "></td>";
            }
            else
            {
                DataTable sl_Parameter = my_c.GetTable("select id,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id in (" + zhi + ")");
                string Parameter_str = "";
                for (int b1 = 0; b1 < sl_Parameter.Rows.Count; b1++)
                {
                    if (Parameter_str == "")
                    {
                        Parameter_str = sl_Parameter.Rows[b1]["u1"].ToString();
                    }
                    else
                    {

                        Parameter_str = Parameter_str + "," + sl_Parameter.Rows[b1]["u1"].ToString();
                    }
                }
                from_str = "<td " + xianshi + ">" + Parameter_str + "</td>";
            }
            #endregion
        }
        else if (u6 == "文本域")
        {
            if (liebiaocaozuo == "是")
            {
                #region 列表页文本域操作权限
                from_str = "<td " + xianshi + "><textarea name=\"" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + id + "\" id=\"" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + id + "\" class=\"listtxtinput\" cols=\"\" rows=\"\"  onchange=\"list_Fields('" + id + "','" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + "','" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "')\">" + zhi + "</textarea></td>";
                #endregion
            }
            else
            {
                #region 非操作文本域
                from_str = "<td " + xianshi + ">" + set_m_url(u12, dt1, j, u12, zhi.Replace("$我是分隔符$", "|")) + "</td>";
                #endregion
            }
        }
        else if (u6 == "数字")
        {
            if (liebiaocaozuo == "是")
            {
                #region 列表页文本域操作权限
                from_str = "<td " + xianshi + "><input type=\"text\" name=\"" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + id + "\" id=\"" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + id + "\" class=\"listdinput\"   onchange=\"list_Fields('" + id + "','" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + "','" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "')\" value=\"" + zhi + "\" /></td>";
                #endregion
            }
            else
            {
                #region 非操作文本域
                from_str = "<td " + xianshi + ">" + set_m_url(u12, dt1, j, u12, zhi.Replace("$我是分隔符$", "|")) + "</td>";
                #endregion
            }
        }
        else if (u6 == "下拉框")
        {
            zhi = zhi.Trim();
            if (liebiaocaozuo == "是")
            {

                #region 列表页下拉框操作权限
                string selectstr = "<select name=\"" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + id + "\" id=\"" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + id + "\" class=\"select-container3\"  onchange=\"list_Fields('" + id + "','" + Model_dt_Field.Rows[Field_d1]["u1"].ToString() + "','" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "')\"><option value=\"\"></option>";
                Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
                MatchCollection matches = reg.Matches(u8);
                if (matches.Count > 0)
                {
                    DataTable dt = new DataTable();
                    string ziduan = "";
                    foreach (Match match in matches)
                    {
                        string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                        string[] aa = match1.Split('|');
                        string sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(get_url(aa[1].ToString())) + " " + aa[2].ToString();

                        dt = my_c.GetTable(sql);
                        ziduan = aa[3].ToString();
                    }
                    for (int j1 = 0; j1 < dt.Rows.Count; j1++)
                    {

                        string zhi1 = set_ziduan(dt, ziduan, j1);
                        if (ziduan.Split(',').Length > 1)
                        {
                            #region 输入单个结果
                            string selected = "";
                            if (dt.Rows[j1][ziduan.Split(',')[0].ToString()].ToString() == zhi)
                            {
                                selected = "selected=\"selected\"";
                            }
                            selectstr = selectstr + "<option value=\"" + dt.Rows[j1][ziduan.Split(',')[0].ToString()].ToString() + "\" " + selected + ">" + dt.Rows[j1][ziduan.Split(',')[1].ToString()].ToString() + "</option>";
                            #endregion
                        }
                        else if (ziduan.IndexOf("+") > -1)
                        {

                            #region 输入相连的结果
                            if (ziduan.Split('+')[0].ToString() == "id")
                            {
                                string selected = "";
                                if (dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() == zhi)
                                {
                                    selected = "selected=\"selected\"";
                                }
                                selectstr = selectstr + "<option value=\"" + dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() + "\" " + selected + ">" + dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j1][ziduan.Split('+')[1].ToString()].ToString() + "</option>";
                            }
                            else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                            {
                                string selected = "";
                                if (dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() == zhi)
                                {
                                    selected = "selected=\"selected\"";
                                }
                                selectstr = selectstr + "<option value=\"" + dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() + "\" " + selected + ">" + dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j1][ziduan.Split('+')[1].ToString()].ToString() + "</option>";
                            }
                            else
                            {
                                string selected = "";
                                if (dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j1][ziduan.Split('+')[1].ToString()].ToString() == zhi)
                                {
                                    selected = "selected=\"selected\"";
                                }
                                selectstr = selectstr + "<option value=\"" + dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j1][ziduan.Split('+')[1].ToString()].ToString() + "\" " + selected + ">" + dt.Rows[j1][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[j1][ziduan.Split('+')[1].ToString()].ToString() + "</option>";

                            }
                            #endregion
                        }
                        else
                        {
                            string selected = "";
                            if (zhi1 == zhi)
                            {
                                selected = "selected=\"selected\"";
                            }
                            selectstr = selectstr + "<option value=\"" + zhi1 + "\" " + selected + ">" + zhi1 + "</option>";
                        }

                    }
                    //输出
                    from_str = "<td " + xianshi + ">" + selectstr + "</select></td>";
                    //
                }
                else
                {
                    string[] aa = Regex.Split(u8, "\r\n");
                    if (u8.IndexOf("\r\n") == -1)
                    {
                        aa = u8.Split('|');
                    }
                    for (int j1 = 0; j1 < aa.Length; j1++)
                    {
                        string selected = "";
                        if (aa[j1].ToString() == zhi)
                        {
                            selected = "selected=\"selected\"";
                        }
                        selectstr = selectstr + "<option value=\"" + aa[j1].ToString() + "\" " + selected + ">" + aa[j1].ToString() + "</option>";
                    }
                    //输出
                    from_str = "<td " + xianshi + ">" + selectstr + "</select></td>";
                    //
                }
                #endregion
            }
            else
            {
                #region 非操作下拉框
                Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
                string txt_value = u8;
                MatchCollection matches = reg.Matches(txt_value);
                if (matches.Count > 0)
                {

                    DataTable dt = new DataTable();
                    string ziduan = "";
                    foreach (Match match in matches)
                    {

                        string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                        string[] aa = match1.Split('|');
                        string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi + ") " + aa[2].ToString();
                        ziduan = aa[3].ToString();
                        if (ziduan.IndexOf(",") > -1)
                        {
                            #region 针对逗号的处理
                            if (ziduan.Split(',')[0].ToString() == "id")
                            {
                                #region 值为空
                                if (zhi == "")
                                {
                                    zhi = "0";
                                }

                                sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi + ") " + aa[2].ToString();
                                #endregion
                            }
                            else if (ziduan.Split(',')[0].ToString() == "yonghuming")
                            {
                                sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and yonghuming ='" + zhi + "' " + aa[2].ToString();
                            }
                            else
                            {
                                sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and " + ziduan.Split(',')[0].ToString() + " ='" + zhi + "' " + aa[2].ToString();

                            }
                            string sql1 = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();

                            try
                            {
                                dt = my_c.GetTable(sql);
                                int hangshu = dt.Rows.Count;
                                if (dt.Rows.Count > 0)
                                {
                                    if (ziduan.Split(',').Length > 1)
                                    {
                                        from_str = "<td " + xianshi + ">" + dt.Rows[0][ziduan.Split(',')[1].ToString()].ToString() + "</td>";
                                    }
                                    else
                                    {
                                        from_str = "<td " + xianshi + ">" + dt.Rows[0][ziduan].ToString() + "</td>";
                                    }

                                }
                                else
                                {
                                    from_str = "<td " + xianshi + "></td>";
                                }

                            }
                            catch
                            {

                                //  dt = my_c.GetTable(sql1);
                                from_str = "<td " + xianshi + ">" + zhi + "</td>";
                            }
                            #endregion
                        }
                        else
                        {

                            #region 针对加号的处理

                            if (ziduan.Split('+')[0].ToString() == "id")
                            {
                                sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi + ") " + aa[2].ToString();
                            }
                            else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                            {

                                sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and yonghuming ='" + zhi + "' " + aa[2].ToString();

                            }
                            else
                            {
                                sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and " + ziduan.Split('+')[0].ToString() + " ='" + zhi + "' " + aa[2].ToString();

                            }


                            dt = my_c.GetTable(sql);
                            int hangshu = dt.Rows.Count;
                            if (dt.Rows.Count > 0)
                            {
                                if (ziduan.Split('+').Length > 1)
                                {
                                    from_str = "<td " + xianshi + ">" + dt.Rows[0][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[0][ziduan.Split('+')[1].ToString()].ToString() + "</td>";
                                }
                                else
                                {
                                    from_str = "<td " + xianshi + ">" + dt.Rows[0][ziduan].ToString() + "</td>";
                                }

                            }
                            else
                            {
                                from_str = "<td " + xianshi + "></td>";
                            }
                            #endregion
                        }


                        ziduan = aa[3].ToString();

                    }



                }
                else
                {
                    from_str = "<td " + xianshi + ">" + set_m_url(u12, dt1, j, u12, zhi.Replace("$我是分隔符$", "|")) + "</td>";
                }
                //end
                #endregion
            }

        }
        else if (u6 == "多选按钮组")
        {
            #region 多选按钮组
            Regex reg = new Regex("sql{.*?}", RegexOptions.Singleline);
            string txt_value = u8;
            MatchCollection matches = reg.Matches(txt_value);
            if (matches.Count > 0)
            {

                DataTable dt = new DataTable();
                string ziduan = "";
                foreach (Match match in matches)
                {

                    string match1 = match.ToString().Replace("sql{", "").Replace("}", "");
                    string[] aa = match1.Split('|');
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi + ") " + aa[2].ToString();
                    ziduan = aa[3].ToString();

                    if (ziduan.IndexOf(",") > -1)
                    {
                        #region 针对逗号的处理
                        if (ziduan.Split(',')[0].ToString() == "id")
                        {
                            #region 值为空
                            if (zhi == "")
                            {
                                zhi = "0";
                            }

                            sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi.Replace("$我是分隔符$", ",") + ") " + aa[2].ToString();
                            #endregion
                        }
                        else if (ziduan.Split(',')[0].ToString() == "yonghuming")
                        {
                            sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and yonghuming ='" + zhi + "' " + aa[2].ToString();
                        }
                        else
                        {
                            sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();

                        }
                        string sql1 = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();
                        //HttpContext.Current.Response.Write(sql);
                        //HttpContext.Current.Response.End();
                        try
                        {
                            dt = my_c.GetTable(sql);
                            int hangshu = dt.Rows.Count;
                            if (dt.Rows.Count > 0)
                            {
                                #region 组成数据
                                string ziduanstr = "";
                                for (int d = 0; d < dt.Rows.Count; d++)
                                {
                                    if (ziduan.Split(',').Length > 1)
                                    {
                                        if (ziduanstr == "")
                                        {
                                            ziduanstr = dt.Rows[0][ziduan.Split(',')[1].ToString()].ToString();
                                        }
                                        else
                                        {
                                            ziduanstr = ziduanstr + "<br>" + dt.Rows[0][ziduan.Split(',')[1].ToString()].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (ziduanstr == "")
                                        {
                                            ziduanstr = dt.Rows[0][ziduan].ToString();
                                        }
                                        else
                                        {
                                            ziduanstr = ziduanstr + "<br>" + dt.Rows[0][ziduan].ToString();
                                        }
                                    }
                                }
                                #endregion

                                if (ziduan.Split(',').Length > 1)
                                {
                                    from_str = "<td " + xianshi + ">" + ziduanstr + "</td>";
                                }
                                else
                                {
                                    from_str = "<td " + xianshi + ">" + ziduanstr + "</td>";
                                }

                            }
                            else
                            {
                                from_str = "<td " + xianshi + "></td>";
                            }

                        }
                        catch
                        {

                            //  dt = my_c.GetTable(sql1);
                            from_str = "<td " + xianshi + ">" + zhi + "</td>";
                        }
                        #endregion
                    }
                    else
                    {

                        #region 针对加号的处理

                        if (ziduan.Split('+')[0].ToString() == "id")
                        {
                            sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and id in (" + zhi.Replace("$我是分隔符$", ",") + ") " + aa[2].ToString();
                        }
                        else if (ziduan.Split('+')[0].ToString() == "yonghuming")
                        {

                            sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " and yonghuming ='" + zhi + "' " + aa[2].ToString();

                        }
                        else
                        {
                            sql = "select " + aa[3].ToString().Replace("+", ",") + " from " + aa[0].ToString() + " " + get_url(aa[1].ToString()) + " " + aa[2].ToString();

                        }


                        dt = my_c.GetTable(sql);
                        int hangshu = dt.Rows.Count;
                        if (dt.Rows.Count > 0)
                        {
                            #region 组成数据
                            string ziduanstr = "";
                            for (int d = 0; d < dt.Rows.Count; d++)
                            {
                                if (ziduan.Split('+').Length > 1)
                                {
                                    if (ziduanstr == "")
                                    {
                                        ziduanstr = dt.Rows[0][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[0][ziduan.Split('+')[1].ToString()].ToString();
                                    }
                                    else
                                    {
                                        ziduanstr = ziduanstr + "<br>" + dt.Rows[0][ziduan.Split('+')[0].ToString()].ToString() + "：" + dt.Rows[0][ziduan.Split('+')[1].ToString()].ToString();
                                    }
                                }
                                else
                                {
                                    if (ziduanstr == "")
                                    {
                                        ziduanstr = dt.Rows[0][ziduan].ToString();
                                    }
                                    else
                                    {
                                        ziduanstr = ziduanstr + "<br>" + dt.Rows[0][ziduan].ToString();
                                    }
                                }
                            }
                            #endregion
                            if (ziduan.Split('+').Length > 1)
                            {
                                from_str = "<td " + xianshi + ">" + ziduanstr + "</td>";
                            }
                            else
                            {
                                from_str = "<td " + xianshi + ">" + ziduanstr + "</td>";
                            }

                        }
                        else
                        {
                            from_str = "<td " + xianshi + "></td>";
                        }
                        #endregion
                    }


                    ziduan = aa[3].ToString();

                }



            }
            else
            {
                from_str = "<td " + xianshi + ">" + set_m_url(u12, dt1, j, u12, zhi.Replace("$我是分隔符$", "<br>")) + "</td>";
            }
            //end
            #endregion
        }
        else if (u6 == "分类")
        {
            #region 分类
            if (zhi == "")
            {
                from_str = "<td " + xianshi + "></td>";
            }
            else
            {
                DataTable sl_Parameter = my_c.GetTable("select id,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + zhi + ")");
                string Parameter_str = "";
                for (int b1 = 0; b1 < sl_Parameter.Rows.Count; b1++)
                {
                    if (Parameter_str == "")
                    {
                        Parameter_str = sl_Parameter.Rows[b1]["u1"].ToString();
                    }
                    else
                    {

                        Parameter_str = Parameter_str + "," + sl_Parameter.Rows[b1]["u1"].ToString();
                    }
                }
                from_str = "<td " + xianshi + ">" + Parameter_str + "</td>";
            }
            #endregion
        }
        else if (u6 == "上级")
        {
            #region 上级
            if (zhi == "")
            {
                from_str = "<td " + xianshi + "></td>";
            }
            else
            {
                string tablename = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString());
                string Field_str = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + " and U5='是' order by u9 ,id").Rows[0]["u1"].ToString();
                DataTable sl_Parameter = my_c.GetTable("select id," + Field_str + " from " + tablename + " where id in (" + zhi + ")");
                string Parameter_str = "";
                for (int b1 = 0; b1 < sl_Parameter.Rows.Count; b1++)
                {
                    if (Parameter_str == "")
                    {
                        Parameter_str = sl_Parameter.Rows[b1]["" + Field_str + ""].ToString();
                    }
                    else
                    {

                        Parameter_str = Parameter_str + "," + sl_Parameter.Rows[b1]["" + Field_str + ""].ToString();
                    }
                }
                from_str = "<td " + xianshi + ">" + Parameter_str + "</td>";
            }
            #endregion
        }
        else if (u6 == "多条数据")
        {
            #region 多条数据
            if (zhi == "")
            {
                from_str = "<td " + xianshi + "></td>";
            }
            else
            {
                DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + u8 + " and u5='是' and u13='否' and u1<>'laiyuanbianhao' order by u9,id");

                DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + u8) + " where laiyuanbianhao=" + zhi);
                int numrows = 1;
                int numcells = 2;
                string neirong = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string tiaoshu = "";
                    for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                    {
                        tiaoshu = tiaoshu + "<td>" + Model_dt.Rows[d1]["u2"].ToString() + "</td><td>" + dt.Rows[i][Model_dt.Rows[d1]["u1"].ToString()].ToString() + "</td>";
                    }
                    neirong = neirong + "<tr>" + tiaoshu + "</tr>";

                }

                from_str = "<td " + xianshi + "><table>" + neirong + "</table></td>";
            }

            #endregion
        }
        else
        {
            #region 其它
            from_str = "<td " + xianshi + ">" + set_m_url(u12, dt1, j, u12, zhi.Replace("$我是分隔符$", "|")) + "</td>";
            #endregion
        }
        return from_str;

    }
    #endregion
    #region 后台列表默认版
    public string set_list(string t1, DataTable dt2, DataTable dt1, int i, string edit_url, string pageurl)
    {
        #region 处理模型表中的说明
        string Model_id = "";
        try
        {
            Model_id = HttpContext.Current.Request.QueryString["Model_id"].ToString();
        }
        catch { }
        if (Model_id != "")
        {

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            if (Model_dt.Rows.Count > 0)
            {

                if (Model_dt.Rows[0]["qianzhi"].ToString() != "")
                {
                    if (Model_dt.Rows[0]["qianzhi"].ToString().IndexOf("update") > -1)
                    {
                        my_c.genxin(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.Write(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.End();
                    }
                }
            }
        }
        #endregion 处理模型表中的说明
        string test_align = "";

        for (int j = 0; j < dt1.Rows.Count; j++)
        {
            t1 = t1 + "<tr><td  style='width:8%' class=\"hidden-xs\"><div class=\"checkbox checkbox-inline checkbox-success\"><input type=\"checkbox\" name=\"chk\" value=\"" + dt1.Rows[j]["id"].ToString() + "\"><label>" + dt1.Rows[j]["id"].ToString() + "</label></td>";
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {
                    #region 最后一行
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i) + "<td>" + my_b.set_canshu(dt1.Rows[j]["id"].ToString(), HttpContext.Current.Request.QueryString["Model_id"].ToString()) + "<a href='page_display.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&id=" + dt1.Rows[j]["id"].ToString() + "' target=_blank>查看</a> | <a href='" + pageurl + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "'>修改</a> | <a onclick=\"return confirm('你确认将该信息删除？注意：相关图片和资源也将删除。');\" href='" + pageurl + "&type=del&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "' >删除</a></td></tr>";
                    #endregion
                }
                else
                {
                    #region 其它行 start
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i);
                    #endregion

                }
            }
        }

        t1 = t1 + "</table>";

        return t1; //返回
    }

    #endregion
    #region 后台列表默认版
    public string set_list2(string t1, DataTable dt2, DataTable dt1, int i, string edit_url, string pageurl)
    {
        #region 处理模型表中的说明
        string Model_id = "";
        try
        {
            Model_id = HttpContext.Current.Request.QueryString["Model_id"].ToString();
        }
        catch { }
        if (Model_id != "")
        {

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            if (Model_dt.Rows.Count > 0)
            {

                if (Model_dt.Rows[0]["qianzhi"].ToString() != "")
                {
                    if (Model_dt.Rows[0]["qianzhi"].ToString().IndexOf("update") > -1)
                    {
                        my_c.genxin(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.Write(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.End();
                    }
                }
            }
        }
        #endregion 处理模型表中的说明
        string test_align = "";

        for (int j = 0; j < dt1.Rows.Count; j++)
        {
            t1 = t1 + "<tr><td  style='width:8%' class=\"hidden-xs\"><div class=\"checkbox checkbox-inline checkbox-success\"><input type=\"checkbox\" name=\"chk\" value=\"" + dt1.Rows[j]["id"].ToString() + "\"><label>" + dt1.Rows[j]["id"].ToString() + "</label></td>";
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {
                    #region 最后一行
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i) + "<td>" + my_b.set_canshu(dt1.Rows[j]["id"].ToString(), HttpContext.Current.Request.QueryString["Model_id"].ToString()) + "<a href='page_display.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&id=" + dt1.Rows[j]["id"].ToString() + "' target=_blank>查看</a> | <a href='" + pageurl + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "'>修改</a> | <a onclick=\"return confirm('你确认将该信息删除？注意：相关图片和资源也将删除。');\" href='" + pageurl + "&type=del&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "' >删除</a></td></tr>";
                    #endregion
                }
                else
                {
                    #region 其它行 start
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i);
                    #endregion

                }
            }
        }

        t1 = t1 + "</table>";

        return t1; //返回
    }

    #endregion
    #region 后台参数列表
    public string set_canshu_list(string t1, DataTable dt2, DataTable dt1, int i, string edit_url, string pageurl)
    {
        #region 处理模型表中的说明
        string Model_id = "";
        try
        {
            Model_id = HttpContext.Current.Request.QueryString["Model_id"].ToString();
        }
        catch { }
        if (Model_id != "")
        {
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            if (Model_dt.Rows.Count > 0)
            {

                if (Model_dt.Rows[0]["qianzhi"].ToString() != "")
                {
                    if (Model_dt.Rows[0]["qianzhi"].ToString().IndexOf("update") > -1)
                    {
                        my_c.genxin(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.Write(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.End();
                    }
                }
            }
        }
        #endregion 处理模型表中的说明
        string test_align = "";
        for (int j = 0; j < dt1.Rows.Count; j++)
        {
            t1 = t1 + "<tr><td  style='width:8%' class=\"hidden-xs\"><div class=\"checkbox checkbox-inline checkbox-success\"><input type=\"checkbox\" name=\"chk\" value=\"" + dt1.Rows[j]["id"].ToString() + "\"><label>" + dt1.Rows[j]["id"].ToString() + "</label></td>";
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i == 0)
                {
                    DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString());
                    string biaoming = Model_dt.Rows[0]["u1"].ToString();
                    #region 第一行 start
                    string zhi = "<a href='fenlei_table.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&classid=" + dt1.Rows[j]["id"].ToString() + "'>" + dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString() + "</a>";
                    t1 = t1 + list_from(zhi, dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i) + "<td>[下级参数：" + my_c.GetTable("select count(id) as count_id from " + biaoming + " where classid=" + dt1.Rows[j]["id"].ToString() + "").Rows[0]["count_id"].ToString() + "]</td>";
                    #endregion
                }
                else if (i + 1 == dt2.Rows.Count)
                {
                    #region 最后一行
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i) + "<td><a href=\"fenlei_table_add.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&classid=" + dt1.Rows[j]["id"].ToString() + "\" title=\"添加子类\" class=\"operation\"><span class=\"ficon ficon-zoomout\"></span></a>		 <a href=\"" + pageurl + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "\" title=\"编辑\" class=\"operation\"><span class=\"ficon ficon-xiugai\"></span></a>				<a href=\"page_display.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&id=" + dt1.Rows[j]["id"].ToString() + "\" title=\"查看\" class=\"operation\" target=_blank><span class=\"ficon ficon-chakan\"></span></a>						<a  onclick=\"return confirm(''你确认将该信息删除？注意：相关图片和资源也将删除。');\" id=\"repChannel_ctl01_btnDelete\" href=\"" + pageurl + "&type=del&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "\" title=\"删除\" class=\"operation\"><span class=\"ficon  ficon-shanchu\"></span></a></td></tr>";
                    #endregion
                }
                else
                {
                    #region 其它行 start
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i);
                    #endregion

                }
            }
        }

        t1 = t1 + "</table>";

        return t1; //返回
    }

    #endregion
    #region 后台列表没有操作功能
    public string set_list_caozuo(string t1, DataTable dt2, DataTable dt1, int i, string edit_url, string pageurl)
    {
        #region 处理模型表中的说明
        string Model_id = "";
        try
        {
            Model_id = HttpContext.Current.Request.QueryString["Model_id"].ToString();
        }
        catch { }
        if (Model_id != "")
        {

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            if (Model_dt.Rows.Count > 0)
            {

                if (Model_dt.Rows[0]["qianzhi"].ToString() != "")
                {
                    if (Model_dt.Rows[0]["qianzhi"].ToString().IndexOf("update") > -1)
                    {
                        my_c.genxin(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.Write(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.End();
                    }
                }
            }
        }
        #endregion 处理模型表中的说明
        string test_align = "";

        for (int j = 0; j < dt1.Rows.Count; j++)
        {
            t1 = t1 + "<tr><td  style='width:8%' class=\"hidden-xs\"><div class=\"checkbox checkbox-inline checkbox-success\"><input type=\"checkbox\" name=\"chk\" value=\"" + dt1.Rows[j]["id"].ToString() + "\"><label>" + dt1.Rows[j]["id"].ToString() + "</label></td>";
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {
                    #region 最后一行
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i) + "</tr>";
                    #endregion
                }
                else
                {
                    #region 其它行 start
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i);
                    #endregion

                }
            }
        }

        t1 = t1 + "</table>";

        return t1; //返回
    }
    #endregion
    #region 后台列表多条数据版
    public string set_duo_list(string t1, DataTable dt2, DataTable dt1, int i, string edit_url, string pageurl)
    {
        #region 处理模型表中的说明
        string Model_id = "";
        try
        {
            Model_id = HttpContext.Current.Request.QueryString["Model_id"].ToString();
        }
        catch { }
        if (Model_id != "")
        {

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            if (Model_dt.Rows.Count > 0)
            {

                if (Model_dt.Rows[0]["qianzhi"].ToString() != "")
                {
                    if (Model_dt.Rows[0]["qianzhi"].ToString().IndexOf("update") > -1)
                    {
                        my_c.genxin(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.Write(Model_dt.Rows[0]["qianzhi"].ToString());
                        //HttpContext.Current.Response.End();
                    }
                }
            }
        }
        #endregion 处理模型表中的说明
        string test_align = "";

        for (int j = 0; j < dt1.Rows.Count; j++)
        {
            t1 = t1 + "<tr><td  style='width:8%' class=\"hidden-xs\"><div class=\"checkbox checkbox-inline checkbox-success\"><input type=\"checkbox\" name=\"chk\" value=\"" + dt1.Rows[j]["id"].ToString() + "\"><label>" + dt1.Rows[j]["id"].ToString() + "</label></td>";
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                if (i + 1 == dt2.Rows.Count)
                {
                    #region 最后一行
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i) + "<td>" + my_b.set_canshu(dt1.Rows[j]["id"].ToString(), HttpContext.Current.Request.QueryString["Model_id"].ToString()) + "<a href='page_display.aspx?Model_id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString() + "&id=" + dt1.Rows[j]["id"].ToString() + "' target=_blank>查看</a> | <a href='javascript:layer_open(\"" + pageurl + "&type=edit&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "\",\"编辑数据\")'>修改</a> | <a onclick=\"return confirm('你确认将该信息删除？注意：相关图片和资源也将删除。');\" href='" + pageurl + "&type=del&id=" + dt1.Rows[j]["id"].ToString() + "" + edit_url + "' >删除</a></td></tr>";
                    #endregion
                }
                else
                {
                    #region 其它行 start
                    t1 = t1 + list_from(dt1.Rows[j][dt2.Rows[i]["u1"].ToString()].ToString(), dt1.Rows[j]["id"].ToString(), dt1, j, dt2, i);
                    #endregion

                }
            }
        }

        t1 = t1 + "</table>";

        return t1; //返回
    }

    #endregion
    //处理列表页问题
    //分类框
    public string fenlei_kuang(string txt_value, string id, Page c)
    {
        string fenlei_str = "<div class=\"testContainer\">            <div class=\"box\">  ";

        //第三个
        //txt = new TextBox();
        //txt.ID = id + "_list";
        //Parameter_class(txt_value, 0);
        //txt.Text = parameter_list;
        //txt.CssClass = "yincang";
        //c.Controls.Add(txt);
        Parameter_class(txt_value, 0);
        fenlei_str = fenlei_str + " <input name=\"" + id + "_list\" type=\"text\" value=\"" + parameter_list + "\" id=\"" + id + "_list\" class=\"yincang\" />";
        ////第二个
        //txt = new TextBox();
        //txt.ID = id + "_Test";
        //txt.CssClass = "input";
        //c.Controls.Add(txt);
        ////第一个
        //txt = new TextBox();
        //txt.ID = id;

        //txt.CssClass = "yincang";
        //c.Controls.Add(txt);
        fenlei_str = fenlei_str + "<input name=\"" + id + "_Test\" type=\"text\" id=\"" + id + "_Test\" class='fenleikuang' /><input name=\"" + id + "\" type=\"text\" id=\"" + id + "\" class=\"yincang\" />";

        fenlei_str = fenlei_str + "</div>        </div> <script type=\"text/javascript\"> $(document).ready(function () {            $(\"#" + id + "_Test\").MultDropList({ data: $(\"#" + id + "_list\").val() }); });</script>";
        return fenlei_str;

    }
    //获取多选下拉框值
    public string get_xiaolai(string zhi)
    {
        string txt_text = "";
        get_ding_dir_str = "";
        string[] aa = zhi.Split(',');
        for (int i = 0; i < aa.Length; i++)
        {
            Parameterweizhi(aa[i]);
        }
        //Response.Write(get_ding_dir_str);
        //Response.End();
        txt_text = get_ding_dir_str;
        return txt_text;
    }
    //所有的结束
    #region 读操作记录
    public void read_log(string Model_id, string laiyuanbiaohao, Table Table4)
    {
        if (Model_id != "")
        {
            DataTable sl_Model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            DataTable dt = my_c.GetTable("select top 5 * from sl_system where laiyuanbianhao='" + sl_Model.Rows[0]["u1"].ToString() + "_" + laiyuanbiaohao + "'");
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
    }
    #endregion
    #region 写操作记录
    public void write_log(string Model_id, string laiyuanbiaohao, DataTable Model_dt, Table Table1)
    {

        DataTable sl_Model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
        DataTable dt = my_c.GetTable("select * from " + sl_Model.Rows[0]["u1"].ToString() + " where id=" + laiyuanbiaohao + "");
        string order_log = sl_Model.Rows[0]["u2"].ToString() + "[" + sl_Model.Rows[0]["u1"].ToString() + "]表被修改，";
        string sql = "";
        for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
        {
            string type = Model_dt.Rows[d1]["u6"].ToString();
            if (type == "时间框" || type == "文本框" || type == "文本域" || type == "下拉框" || type == "单选按钮组" || type == "标题" || type == "多选按钮组")
            {
                string zhi = get_kj(Table1, Model_dt, d1);
                if (zhi != "null")
                {
                    if ("'" + dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString() + "'" != zhi)
                    {
                        if (sql == "")
                        {
                            sql = sql + Model_dt.Rows[d1]["u2"].ToString() + "：" + zhi + "";
                        }
                        else
                        {
                            sql = sql + "、" + Model_dt.Rows[d1]["u2"].ToString() + "：" + zhi + "";
                        }
                    }
                }


            }
            if (type == "数字" || type == "货币")
            {
                string zhi = get_kj(Table1, Model_dt, d1);
                if (zhi != "null")
                {

                    if (dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString() != zhi)
                    {
                        if (sql == "")
                        {
                            sql = sql + Model_dt.Rows[d1]["u2"].ToString() + "：" + zhi + "";
                        }
                        else
                        {
                            sql = sql + "、" + Model_dt.Rows[d1]["u2"].ToString() + "：" + zhi + "";
                        }
                    }
                }


            }
            //end
        }

        if (sql != "")
        {
            order_log = order_log + sql;
            order_log = my_b.c_string(order_log);
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4,laiyuanbianhao) values('" + my_b.k_cookie("admin_id") + "','" + order_log + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','资料修改','" + sl_Model.Rows[0]["u1"].ToString() + "_" + laiyuanbiaohao + "')");
        }

    }
    #endregion
}
