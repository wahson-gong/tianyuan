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
public partial class admin_auto_table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    static DataTable Model_dt = new DataTable();
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
    public void set_kj(TableCell c, string type, string id, string txt_value, string u3)
    {
        if (txt_value == "当前时间")
        {
            DateTime dy = DateTime.Now;
            txt_value = dy.ToString();
        }
        if (type == "文本框" || type == "标签")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Text = txt_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "编号")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.Text = my_b.get_bianhao();

            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "会员卡")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.Text = my_b.get_huiyuanka();

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
            txt.CssClass = "txtwidth";
            txt.Text = txt_value;
            txt.Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
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
            txt.CssClass = "TextBoxModewidth";
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = txt_value;
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
            txt.CssClass = "txtwidth";
            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = txt_value;
            //添加控件到容器 
            c.Height = 300;
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<script>    KindEditor.ready(function (K) {        var editor1 = K.create('#" + txt.ClientID + "',{           width: '800',            height: '280',       cssPath: 'editor/plugins/code/prettify.css',            uploadJson: 'editor/asp.net/upload_json.ashx',            fileManagerJson: 'editor/asp.net/file_manager_json.ashx',            allowFileManager: true,            afterCreate: function () {                var self = this;                K.ctrl(document, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });                K.ctrl(self.edit.doc, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });            }        });        prettyPrint();    });	</script>"));
            my_b.Set_FreeTextBox("admin");
        }
        else if (type == "密码框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.TextMode = TextBoxMode.Password;
            txt.Attributes["value"] = txt_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "编辑器")
        {
            string pic_width = "800*0";
            //设置编辑上的东西
            get_bianjiqi(c);
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = txt_value;
            //添加控件到容器 
            c.Height = 600;
            c.Controls.Add(txt);
            //c.Controls.Add(new LiteralControl("<script type=\"text/javascript\">       CKEDITOR.replace('" + txt.ClientID + "',                {                height: '580',width: '100%',flashUploadAllowedExtensions: 'swf|fla|wmv|mp3|rm|rmvb|avi',linkUploadAllowedExtensions: 'txt|doc|rar|zip',toolbar:'Full',skin: 'blue',thumbnail: false,fileRecord: true,fieldName: 'Content',wordPic: false,flashUpload: true,imageUpload: true,linkUpload: true,foreground: false,moduleName: '',filebrowserBrowseUrl: 'ckfinder/ckfinder.html',filebrowserImageBrowseUrl: 'ckfinder/ckfinder.html?Type=Images',filebrowserFlashBrowseUrl: 'ckfinder/ckfinder.html?Type=Flash',filebrowserUploadUrl: 'ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',filebrowserImageUploadUrl: 'ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',filebrowserFlashUploadUrl: 'ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'               }         );     </script>"));
            // c.Controls.Add(new LiteralControl("<script type=\"text/javascript\">    var editor = new UE.ui.Editor();    editor.render(\"" + txt.ClientID + "\");</script>"));
            c.Controls.Add(new LiteralControl("<script>    KindEditor.ready(function (K) {        var editor1 = K.create('#" + txt.ClientID + "',{           width: '800',            height: '580',       cssPath: 'editor/plugins/code/prettify.css',            uploadJson: 'editor/asp.net/upload_json.ashx',            fileManagerJson: 'editor/asp.net/file_manager_json.ashx',            allowFileManager: true,            afterCreate: function () {                var self = this;                K.ctrl(document, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });                K.ctrl(self.edit.doc, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });            }        });        prettyPrint();    });	</script>"));
            my_b.Set_FreeTextBox("admin");
        }
        else if (type == "文件框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.Text = "";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>"));
        }
        else if (type == "相关选择")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            
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
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"选择\"  onClick=\"window.open('x_article.aspx?editname=" + id + "&listid='+document.getElementById('" + id + "').value+'&sql=" + HttpUtility.UrlEncode(txt_value) + "');\"/>"));
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "缩略图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传图片\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>&nbsp;&nbsp;<input id=\"Button5\" class=\"button\" type=\"button\" value=\"裁剪图片\"  onClick=\"pic_cut('" + id + "','100*100')\"/>&nbsp;&nbsp;"));
            Button bu1 = new Button();
            bu1.ID = "tqtp";
            bu1.CssClass = "button";
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
            txt.CssClass = "pwdwidth";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传图片\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>&nbsp;&nbsp;"));
         
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "三级联动")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.CssClass = "txtif";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<iframe width=\"400px\" height=\"24px\" src=\"liandong3.aspx?classid=" + txt_value + "&type=add&textBox=" + id + "\" scrolling=\"no\"  frameborder=\"0\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "二级联动")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.CssClass = "txtif";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<iframe width=\"400px\" height=\"24px\" src=\"liandong.aspx?classid=" + txt_value + "&type=add&textBox=" + id + "\" scrolling=\"no\"  frameborder=\"0\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "组图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Text = txt_value;
            txt.CssClass = "txtif";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<iframe width=\"800px\" height=\"50px\" src=\"uploadpic.aspx?classid=if" + id + "&textBox=" + id + "&type=add\" scrolling=\"no\"  frameborder=\"0\" id=\"if" + id + "\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "下拉框")
        {
            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
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
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + aa[1].ToString() + " " + aa[2].ToString();
                    dt = my_c.GetTable(sql);
                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dro.Items.Insert(0, dt.Rows[j][ziduan].ToString());
                    dro.Items[0].Value = dt.Rows[j][ziduan].ToString();
                }
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
                c.Controls.Add(dro);
            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "单选按钮组")
        {

            RadioButtonList che;
            che = new RadioButtonList();
            che.ID = id;
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
            che.Items[0].Selected = true;
            che.RepeatDirection = RepeatDirection.Horizontal;
            c.Controls.Add(che);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "多选按钮组")
        {
            CheckBoxList che;
            che = new CheckBoxList();
            che.ID = id;
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
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + aa[1].ToString() + " " + aa[2].ToString();
                    dt = my_c.GetTable(sql);

                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    che.Items.Insert(0, dt.Rows[j][ziduan].ToString());
                    che.Items[0].Value = dt.Rows[j][ziduan].ToString();
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
            che.RepeatDirection = RepeatDirection.Horizontal;
            c.Controls.Add(che);
            c.Controls.Add(new LiteralControl(u3));
        }
        else
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Text = txt_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
    }
    //获取相关选择中的表名
    public string get_tablename(string sql)
    {
        string sql_str = "";
        Regex reg = new Regex("from.*?order", RegexOptions.Singleline);
        Match sql_ma = reg.Match(sql);
        sql_str = sql_ma.ToString().Replace("order", "");
        sql_str = sql_str.ToString().Replace("from", "");
        sql_str = sql_str.Trim();
        return sql_str;
    }
    //end
    //获取相关选择中的字段名
    public string get_sql(int dijige,string sql)
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
    //修改时使用
    public void set_kj_edit(TableCell c, string type, string id, string txt_value, string user_value, string u3)
    {

        if (type == "文本框" || type == "标签")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
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
            txt.CssClass = "txtwidth";
            txt.Text = user_value;
            txt.Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
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
            txt.CssClass = "TextBoxModewidth";
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
            txt.CssClass = "txtwidth";
            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = user_value;
            //添加控件到容器 
            c.Height = 300;
            c.Controls.Add(txt);

            c.Controls.Add(new LiteralControl("<script>    KindEditor.ready(function (K) {        var editor1 = K.create('#" + txt.ClientID + "',{           width: '800',            height: '280',       cssPath: 'editor/plugins/code/prettify.css',            uploadJson: 'editor/asp.net/upload_json.ashx',            fileManagerJson: 'editor/asp.net/file_manager_json.ashx',            allowFileManager: true,            afterCreate: function () {                var self = this;                K.ctrl(document, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });                K.ctrl(self.edit.doc, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });            }        });        prettyPrint();    });	</script>"));
            my_b.Set_FreeTextBox("admin");
        }
        else if (type == "密码框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.TextMode = TextBoxMode.Password;
            txt.Attributes["value"] = "";
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("密码不修改的话请为空!"));
        }
        else if (type == "编辑器")
        {
            string pic_width = "800*0";
            //设置编辑上的东西
            get_bianjiqi(c);
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = user_value;
            //添加控件到容器 
            c.Height = 600;
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("<script>    KindEditor.ready(function (K) {        var editor1 = K.create('#" + txt.ClientID + "',{           width: '800',            height: '580',       cssPath: 'editor/plugins/code/prettify.css',            uploadJson: 'editor/asp.net/upload_json.ashx',            fileManagerJson: 'editor/asp.net/file_manager_json.ashx',            allowFileManager: true,            afterCreate: function () {                var self = this;                K.ctrl(document, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });                K.ctrl(self.edit.doc, 13, function () {                    self.sync();                    K('form[name=example]')[0].submit();                });            }        });        prettyPrint();    });	</script>"));
            my_b.Set_FreeTextBox("admin");

        }
        else if (type == "文件框")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            if (user_value == "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>"));
            }
            else
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "缩略图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>&nbsp;&nbsp;<input id=\"Button5\" class=\"button\" type=\"button\" value=\"裁剪图片\"  onClick=\"pic_cut('" + id + "','100*100')\"/>&nbsp;&nbsp;"));
            Button bu1 = new Button();
            bu1.ID = "tqtp";
            bu1.CssClass = "button";
            bu1.Text = "提取图片";
            c.Controls.Add(bu1);
            if (user_value != "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "头像")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "pwdwidth";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>&nbsp;&nbsp;"));
         
            if (user_value != "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "二级联动")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Text = user_value;
            txt.CssClass = "txtif";
            //添加控件到容器 
            c.Controls.Add(txt);
            DataTable Field_dt = my_c.GetTable("select * from  " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u1='" + id + "'");
            string field_u8 = "";
            if (Field_dt.Rows.Count > 0)
            {
                field_u8 = Field_dt.Rows[0]["u8"].ToString();
            }
            c.Controls.Add(new LiteralControl("<iframe width=\"400px\" height=\"24px\" src=\"liandong.aspx?classid=" + field_u8 + "&type=edit&textBox=" + id + "&neirong=" + user_value + "\" scrolling=\"no\"  frameborder=\"0\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "三级联动")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Text = user_value;
            txt.CssClass = "txtif";
            //添加控件到容器 
            c.Controls.Add(txt);
            DataTable Field_dt = my_c.GetTable("select * from  " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u1='" + id + "'");
            string field_u8 = "";
            if (Field_dt.Rows.Count > 0)
            {
                field_u8 = Field_dt.Rows[0]["u8"].ToString();
            }
            c.Controls.Add(new LiteralControl("<iframe width=\"400px\" height=\"24px\" src=\"liandong3.aspx?classid=" + field_u8 + "&type=edit&textBox=" + id + "&neirong=" + user_value + "\" scrolling=\"no\"  frameborder=\"0\"></iframe>"));
            c.Controls.Add(new LiteralControl(u3));

        }
        else if (type == "组图")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Height = 120;
            txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"上传" + txt_value + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=" + id + "&Extension=" + txt_value + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/>"));

            if (user_value != "")
            {
                c.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;<a href='" + user_value + "' target=_blank>已上传</a>"));
            }
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "相关选择")
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
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

            string table_name=get_tablename(txt_value);
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + user_value + ")");
            string dt_str = "";
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
            txt1.Text = dt_str;
            c.Controls.Add(txt1);
            //end

            c.Controls.Add(new LiteralControl("&nbsp;&nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"选择\"  onClick=\"window.open('x_article.aspx?editname=" + id + "&listid='+document.getElementById('" + id + "').value+'&sql=" + HttpUtility.UrlEncode(txt_value) + "');\"/>"));

            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "下拉框")
        {
            DropDownList dro;
            dro = new DropDownList();
            dro.ID = id;
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
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + aa[1].ToString() + " " + aa[2].ToString();
                    dt = my_c.GetTable(sql);
                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dro.Items.Insert(0, dt.Rows[j][ziduan].ToString());
                    dro.Items[0].Value = dt.Rows[j][ziduan].ToString();
                }
                for (int d = 0; d < dt.Rows.Count; d++)
                {
                    if (dro.Items[d].Value == user_value)
                    {
                        dro.Items[d].Selected = true;
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
                for (int d = 0; d < aa.Length; d++)
                {
                    if (dro.Items[d].Value == user_value)
                    {
                        dro.Items[d].Selected = true;
                    }
                }
            }

            c.Controls.Add(dro);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "单选按钮组")
        {
            RadioButtonList che;
            che = new RadioButtonList();
            che.ID = id;
            string[] aa = Regex.Split(txt_value, "\r\n");
            if (txt_value.IndexOf("\r\n") == -1)
            {
                aa = txt_value.Split('|');
            }
            for (int j = 0; j < aa.Length; j++)
            {
                che.Items.Insert(0, aa[j].ToString());
                che.Items[0].Value = aa[j].ToString();
                if (user_value == aa[j].ToString())
                {
                    che.Items[0].Selected = true;
                }

            }

            che.RepeatDirection = RepeatDirection.Horizontal;
            c.Controls.Add(che);
            c.Controls.Add(new LiteralControl(u3));
        }
        else if (type == "多选按钮组")
        {
            CheckBoxList che;
            che = new CheckBoxList();
            che.ID = id;
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
                    string sql = "select " + aa[3].ToString() + " from " + aa[0].ToString() + " " + aa[1].ToString() + " " + aa[2].ToString();
                    dt = my_c.GetTable(sql);

                    ziduan = aa[3].ToString();
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    che.Items.Insert(0, dt.Rows[j][ziduan].ToString());
                    che.Items[0].Value = dt.Rows[j][ziduan].ToString();
                    if (user_value.IndexOf(dt.Rows[j][ziduan].ToString()) > -1)
                    {
                        che.Items[0].Selected = true;
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
                    if (user_value.IndexOf(aa[j].ToString()) > -1)
                    {
                        che.Items[0].Selected = true;
                    }

                }

                che.RepeatDirection = RepeatDirection.Horizontal;
                c.Controls.Add(che);
                c.Controls.Add(new LiteralControl(u3));
            }
        }
        else
        {
            TextBox txt;
            //创建TextBox 
            txt = new TextBox();
            txt.ID = id;
            txt.CssClass = "txtwidth";
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
            c.Controls.Add(new LiteralControl(u3));
        }
    }
    string pic_m = "";
    public string get_kj(Table c, string type, string id)
    {
        if (type == "文本框" || type == "文本域" || type == "标签" || type == "时间框" || type == "组图")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)form1.FindControl(id);
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }

            return "'" + my_b.c_string(txt.Text.ToString()) + "'";
        }
        else if (type == "会员卡" || type == "编号")
        {
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {
                TextBox txt = new TextBox();
                txt = (TextBox)form1.FindControl(id);
                return "'" + my_b.c_string(txt.Text.ToString()) + "'";
            }
            else
            {
                TextBox txt = new TextBox();
                txt = (TextBox)form1.FindControl(id);
                if (my_c.GetTable("select id from sl_bianhao where bianhao='" + my_b.c_string(txt.Text.ToString()) + "'").Rows.Count > 0)
                {
                    Response.Redirect("err.aspx?err=会员卡号或编号重复，请确认后重新输入！&errurl=" + my_b.tihuan("user_table_add.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
                }
                else
                {
                    my_c.genxin("insert into sl_bianhao (bianhao) values('" + my_b.c_string(txt.Text.ToString()) + "')");
                }
                return "'" + my_b.c_string(txt.Text.ToString()) + "'";
            }
        }
        else if (type == "密码框")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)form1.FindControl(id);
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {
                DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString());
                if (my_b.c_string(txt.Text.ToString()) == "")
                {
                    return "'" + dt.Rows[0][id].ToString() + "'";
                }

            }
            else
            {
                if (my_b.c_string(txt.Text.ToString()) == "")
                {
                    return "'" + my_b.md5("23xbsm") + "'"; 
                }
            }

            return "'" + my_b.md5(my_b.c_string(txt.Text.ToString())) + "'";
        }
        else if (type == "缩略图" || type == "相关选择")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)form1.FindControl(id);
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }

            return "'" + my_b.c_string(txt.Text.ToString()) + "'";
        }
        else if (type == "编辑器")
        {
            string pic_width ="800*0";
            TextBox txt = new TextBox();
            txt = (TextBox)form1.FindControl(id);
            string t1 = my_b.c_string(txt.Text.ToString());
            CheckBox ch1 = new CheckBox();
            ch1 = (CheckBox)form1.FindControl("tu1");
            if (ch1.Checked)
            {
                t1 = my_b.Download_pic(t1);
            }
            ch1 = (CheckBox)form1.FindControl("tu2");
            if (ch1.Checked)
            {
                t1 = my_b.set_pic_size(t1, pic_width);

            }

            return "'" + my_b.c_string(t1) + "'";
        }
        else if (type == "数字")
        {
            TextBox txt = new TextBox();
            txt = (TextBox)c.FindControl(id);
            return my_b.c_string(txt.Text.ToString());
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
            return "'" + t1 + "'";
        }
        else if (type == "单选按钮组")
        {
            try
            {
                RadioButtonList rad = new RadioButtonList();
                rad = (RadioButtonList)c.FindControl(id);
                return "'" + rad.SelectedItem.Value + "'";
            }
            catch
            {
                return "";
            }
        }
        else if (type == "下拉框")
        {
            DropDownList dro = new DropDownList();
            dro = (DropDownList)c.FindControl(id);
            return "'" + dro.SelectedItem.Value + "'";
        }
        else
        {
            TextBox txt = new TextBox();
            txt = (TextBox)form1.FindControl(id);
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }

            return "'" + my_b.c_string(txt.Text.ToString()) + "'";
        }

        return "";
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            string id = "";
            try
            {
                id = Request.QueryString["id"].ToString();
            }
            catch {
                Response.Redirect("user_table_pwd.aspx?Model_id=4&type=edit&id=" + my_c.GetTable("select id from sl_user where yonghuming='" + my_b.k_cookie("admin_id") + "'").Rows[0]["id"].ToString() + "");
            }

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
                            set_kj_edit(c, dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString(), Model_dt,d1);
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
            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString());
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                my_c.genxin("delete from " + table_name + " where id=" + dt.Rows[i]["id"].ToString());
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

            Response.Redirect("err.aspx?err=删除信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("user_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
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
                            set_kj(c, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), Model_dt.Rows[d1]["u8"].ToString(), Model_dt.Rows[d1]["u3"].ToString());
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

    protected void Button1_Click1(object sender, EventArgs e)
    {

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        

        if (type == "edit")
        {
            string sql = "update " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + get_kj(Table1, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString()) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + get_kj(Table1, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString()) + "";
                }
            }

            sql = sql + " where id=" + Request.QueryString["id"].ToString();
            //Response.Write(sql);
            //Response.End();
            my_c.genxin(sql);

            Response.Redirect("err.aspx?err=修改信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
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
                    sql = sql + get_kj(Table1, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString());
                }
                else
                {
                    sql = sql + "," + get_kj(Table1, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString());
                }
            }
            sql = sql + ")";

            my_c.genxin(sql);
            Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
        }

    }
}
