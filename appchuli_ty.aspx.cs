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
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;
using System.Collections.Generic;
using ThoughtWorks.QRCode.Codec;
using Newtonsoft.Json.Linq;

public partial class Execution : System.Web.UI.Page
{
    my_hanshu my_hs = new my_hanshu();
    my_order my_o = new my_order();
    biaodan bd = new biaodan();
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    my_json_ghy my_json_ghy = new my_json_ghy();
    my_dt my_dt = new my_dt();//处理datatable类


    #region 判断登录
    public void set_login()
    {
        try
        {
            if (my_b.c_string(Request["yonghuming"].ToString()) == "")
            {
                tiaozhuan("请登陆后操作", Request.UrlReferrer.ToString(), "");

            }
        }
        catch
        {
            tiaozhuan("请登陆后操作", Request.UrlReferrer.ToString(), "");
        }
    }
    #endregion
    #region 处理验证码
    public void set_yanzhengma()
    {
         
    }
    #endregion
    #region 处理输入频率问题
    public void shurupinlv()
    {
        TimeSpan st = new TimeSpan();
        DateTime dy1 = DateTime.Now;
        DateTime dy2 = DateTime.Now;
        try
        {
            dy2 = DateTime.Parse(my_c.GetTable("select top 1 * from sl_system where u3='" + Request.UserHostAddress.ToString() + "' and u4='提交操作' order by dtime desc").Rows[0]["dtime"].ToString());

            st = dy1.Subtract(dy2);

            if (st.TotalMilliseconds < 1)
            {
                //  tiaozhuan(st.TotalMilliseconds.ToString(), "/", "");
                //  Response.Redirect("/err.aspx?err=1分钟内只能一次。&&errurl=" + my_b.tihuan("/", "&&", "fzw123") + "");
            }
        }
        catch
        { }
    }
    #endregion
    #region 记录操作日志
    public void set_system(string type)
    {
        string yonghuming = my_b.get_yonghuming();

        string u3 = "";
        try
        {
            u3 = Request.UrlReferrer.ToString();
        }
        catch
        {
            u3 = Request.Url.ToString();
        }
        //删除3天前的记录
        //Delete * FROM sl_rizhi Where (((dtime)<DateAdd("d",-3,Date())))
        try
        { my_c.genxin("Delete * FROM sl_rizhi Where (((dtime)<DateAdd('d',-3,Date())))"); }
        catch { }
        try
        { 
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi (yonghuming,miaoshu,ip,leixing) values(" + yonghuming + ",'" + u3 + "','" + Request.UserHostAddress.ToString() + "','提交操作')");
        }
        catch { }
       
        //yonghuming = yonghuming + " " + type + "  ";
        
        
        
    }
    #endregion
    #region 记录文章审核
    public void set_shenhe(string table_name, string Model_id)
    {
        string yonghuming = my_b.get_yonghuming();
        string article_new_id = my_c.GetTable("select top 1 id from " + table_name + " order by id desc").Rows[0]["id"].ToString();
        string article_log = table_name + "{fenge}" + article_new_id + "{fenge}" + Model_id;

        my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "rizhi (yonghuming,miaoshu,ip,leixing) values(" + yonghuming + ",'" + article_log + "','" + Request.UserHostAddress.ToString() + "','文章审核')");
    }
    #endregion
    #region 设置跳转
    public void tiaozhuan(string tip_string, string tipurl, string tipurl_type)
    {
        if (tip_string == "")
        {
            try
            {
                tip_string = Request.QueryString["tip_string"].ToString();
            }
            catch
            {
                tip_string = "ok";
            }
        }

        string callback = Request["jsoncallback"];

        string result = callback + "({\"neirong\":\"" + tip_string + "\"})";

        Response.Clear();
        Response.Write(result);
        Response.End();
    }
    #endregion
    #region 发送微信消息
    public void set_weixin()
    {
        //处理预订发送微信消息
        //if (Request["yonghuming"].ToString() != "")
        //{
        //    string wherestrig = "where laiyuanbianhao='" + my_b.c_string(Request["laiyuanbianhao"]) + "' and yonghuming='" + Request["yonghuming"].ToString() + "'";

        //    my_b.getWebFile(my_b.get_Domain() + "weixinmb.aspx?moban=yuyue&&tablename=" + table_name + "&&wherestrig=" + HttpUtility.UrlEncode(wherestrig) + "");
        //}
    }
    #endregion
    #region API接口验证
    public void set_api(string Model_id, string type)
    {
        if (type != "login")
        {
            string sign = "";
            try
            {
                sign = my_b.c_string(Request["sign"].ToString());
            }
            catch { }
            if (sign != "")
            {
                string timestamp = my_b.c_string(Request["timestamp"].ToString());
                DataTable Model_dt = new DataTable();
                if (Request.QueryString.ToString() == "")
                {
                    Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + "  order by u9,id");
                }
                else
                {
                    Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and (" + user_sql(Request.QueryString.ToString()) + ")  order by u9,id");
                }

                string appid = my_c.GetTable("select top 1 * from sl_xitongpeizhi").Rows[0]["appid"].ToString();
                string _sign = my_api.SampleCode.test(Model_dt, timestamp, appid);
                //HttpContext.Current.Response.Write(_sign);
                //HttpContext.Current.Response.End();
                if (sign == _sign)
                {
                    //tiaozhuan("1", "", "");
                }
                else
                {
                    tiaozhuan("0", "", "");
                }
            }

        }

    }
    #endregion

    #region 整体输入提交
    public void set_user_sql(string type)
    {
        #region 处理验证码
        set_yanzhengma();
        #endregion
        #region 处理输入频率问题
        shurupinlv();
        #endregion
        #region 记录操作日志
        set_system(type);
        #endregion

        //string table_name = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.set_url_css(Request.QueryString["t"].ToString());
        //DataTable model_table = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + table_name + "'");
        //string Model_id = model_table.Rows[0]["id"].ToString();
        //#region API接口验证
        //set_api(Model_id,type);
        //#endregion
         string status = "false";
         string msg = "查询失败";
        
        if (type == "qingdan")
        {
            //配送清单接口
            /*
             *派单时间
             *用户名
             *
             */
  
            #region 配送清单接口
            if (is_login())
            {

                DataTable dt = new DataTable();
                //初始化配送清单datatable
                string[] dt_araay = { "Name", "State", "Num" };
                dt = my_dt.setdt(dt_araay);

                string BeginTime = string.Empty; //派送时间
                string UserID = string.Empty;//当前用户
                string sql_BeginTime = string.Empty;//查询派单日期
                string State = string.Empty;//派单状态
                string sql_State = string.Empty;//查询派单状态
                try
                {
                    BeginTime = Request.Params["BeginTime"];
                     
                }
                catch { }
                try
                {
                     
                    UserID = this.getUserid();
                }
                catch { }

                try
                {
                    State = Request.Params["State"];

                }
                catch { }

                if (!string.IsNullOrEmpty(BeginTime))
                {
                    sql_BeginTime = "  and datediff(DAY, BeginTime ,'" + BeginTime + "') = 0  ";
                }

                if (!string.IsNullOrEmpty(State))
                {
                    sql_State = "  and State = "+State+"  ";
                }
               

                //商品的状态 正常 处理中 完成 取消 （0，1，2，3）
                string sql_state = "select State   from Service where ServiceID='" + UserID + "' " + sql_BeginTime + " group by State";
                DataTable dt_state = my_c.GetTable(sql_state, "sql_conn7");
                //商品名称
                string sql_name = "select Name   from Service where ServiceID='" + UserID + "'   " + sql_BeginTime + " group by Name";
                DataTable dt_name = my_c.GetTable(sql_name, "sql_conn7");

                
                //遍历用户配送商品的状态 正常 处理中 完成 取消 （0，1，2，3）
                foreach (DataRow dr  in dt_state.Rows)
                {
                    
                    //遍历用户配送的商品名称
                    foreach (DataRow dr2  in dt_name.Rows)
                    {
                       // Response.Write(dr["State"].ToString() + "    " + dr2["Name"].ToString());
                        string sql = string.Empty;
                        sql = "select  sum(Num) as num   from Service as s1 where  s1.ServiceID='" + UserID + "'  and s1.State=" + dr["State"].ToString() + " and s1.Name='" + dr2["Name"].ToString() + "'  " + sql_BeginTime + " " + sql_State + " ";
                        //Response.Write(sql + "  <br>");
                        //Response.Flush();
                        DataTable dt_service = my_c.GetTable(sql, "sql_conn7");

                        DataRow dr3 = dt_service.Rows[0];
                        if (!string.IsNullOrEmpty(dr3["Num"].ToString()))
                        {
                            DataRow dr4 = dt.NewRow();
                            dr4["Name"] = dr2["Name"].ToString();
                            dr4["State"] = dr["State"].ToString();
                            dr4["Num"] = dr3["Num"].ToString();
                            //插入返回数据
                            dt.Rows.Add(dr4);
                        }
                        

                       

                    }


                }


               


                //dt = my_c.GetTable(sql, "sql_conn7");
                status = "true";
                msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt);
            }
            else
            {
                msg = "请登录后再操作";
            } 
            #endregion
 
        }
        else if (type == "login")
        {
            //登录接口
            /*
             *手机号
             *验证码
             *密码
             *
             */
            #region 登录接口

            string Phone = string.Empty;
            string PassWord = string.Empty;
            string yzm = string.Empty;

            try
            {
                yzm = my_b.c_string(Request.Params["yzm"]);
                Phone = my_b.c_string(Request.Params["phone"]);
                
                
            }
            catch { }
            try
            {
                
                PassWord = my_b.c_string(Request.Params["password"]);

            }
            catch { }
           

            if (!string.IsNullOrEmpty(yzm) && !string.IsNullOrEmpty(Phone) && my_b.ProcessSqlStr(Phone) && my_b.ProcessSqlStr(yzm))
            {
                 
                //通过验证码登录
                string shoujiyzm = string.Empty;
                 
                try
                {
                    //从数据表里读取 5分钟以内有效
                    //shoujiyzm = my_b.k_cookie("shoujiyzm");
                    string _sql = "select * from sl_rizhi where yonghuming='" + Phone + "'  and   DateDiff(\"n\", dtime ,now()) <=5 order by id desc    ";
                    DataTable rizhi = new DataTable();
                    // Response.Write(_sql); Response.End();
                    rizhi = my_c.GetTable(_sql);
                    if (rizhi.Rows.Count > 0)
                    {
                        shoujiyzm = rizhi.Rows[0]["miaoshu"].ToString();
                        
                    }

                }
                catch { }
                 

                if (!string.IsNullOrEmpty(shoujiyzm))
                {
                    //验证码是否正确
                    if (shoujiyzm == yzm)
                    {
                        //查询 用户绑定的配送权限和凭证扫描权限
                        string sql = "select * from [User] where phone='" + Phone + "' and State=0 ";
                        //Response.Write(sql);
                        //Response.End();
                        DataTable user = new DataTable();
                        user = my_c.GetTable(sql, "sql_conn12");
                        //用户名存在，则登录成功
                        if (user.Rows.Count > 0)
                        {
                            System.Random a = new Random(System.DateTime.Now.Millisecond);
                            int RandKey = a.Next(100000, 999999);
                            string _token = my_b.get_bianhao() + RandKey.ToString();
                            //插入新的token记录
                            string sql_token = "INSERT INTO sl_token (yonghuming, token,ip ) VALUES ('" + Phone + "', '" + _token + "','" + Request.UserHostAddress.ToString() + "')";
                            my_c.genxin(sql_token);
                            //生成新的token end


                            status = "true";
                            msg = _token;
                           
                            //保存用户 UserID
                            string UserID = string.Empty;
                            UserID = user.Rows[0]["ID"].ToString();   

                            //插入接口后台用户sl_user数据表
                            string _sql = "INSERT INTO sl_user (phone, openid,userid ) VALUES ('" + Phone + "', '"+this.getOpenid()+"','"+UserID+"')";
                            my_c.genxin(_sql);
                            //my_b.c_cookie(UserID, "UserID");
                        }
                        else
                        {
                            status = "false";
                            msg = "手机号不存在或账号已被禁用";

                        }
                    }
                    else
                    {
                        status = "false";
                        msg = "验证码不正确";
                    
                    }

                }
                else
                {
                   
                    status = "false";
                    msg = "验证码已失效";

                }
                //Response.Write(shoujiyzm);
                //Response.End();

            }
            else if (!string.IsNullOrEmpty(Phone) && !string.IsNullOrEmpty(PassWord) && my_b.ProcessSqlStr(Phone) && my_b.ProcessSqlStr(PassWord))
            {
                //通过手机号和密码登录
                try
                {
                   

                    if (my_b.ProcessSqlStr(Phone) && my_b.ProcessSqlStr(PassWord))
                    {
                        string sql = "select * from [User] where phone='" + Phone + "' and [PassWord]='" + PassWord + "'";
                        DataTable user = new DataTable();
                        user = my_c.GetTable(sql, "sql_conn12");
                        if (user.Rows.Count > 0)
                        {
                            status = "true";
                            msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(user);
                        }
                        else
                        {
                            status = "false";
                            msg = "手机号或者密码错误";

                        }
                    }
                    else
                    {
                        status = "false";
                        msg = "手机号或者密码含有非法字符串";

                    }

                }
                catch
                {
                    status = "false";
                    msg = "手机号或者密码为空";
                }


            }
            else
            {
                status = "false";
                msg = "手机号或者密码不能为空";
                
            } 
            #endregion

        }
        else if (type == "autologin")
        {
            //小程序集成登录接口
            /*
             *手机号
             *验证码
             *密码
             *
             */
            #region 小程序集成登录接口

            string code = string.Empty;
            

            try
            {
                code = my_b.c_string(Request.Params["code"]);
                 
            }
            catch { }


            if (!string.IsNullOrEmpty(code) && my_b.ProcessSqlStr(code)  )
            {
                //string json_temp = "{\"session_key\":\"te5uTuUhrW0qoP9wESC4mA==\",\"openid\":\"o3gDW5fjD1q-0TR_lVccsyH2xpes\"}";
                string json_temp = GetOpenidByCode(code);
                string _openid = "";

                JObject jsonObj = JObject.Parse(json_temp);
                string errmsg = "";
                try
                {
                     errmsg = jsonObj["errmsg"].ToString();
                }
                catch { }
                if (!string.IsNullOrEmpty(errmsg))
                {
                    status = "false";
                    msg = errmsg;
                }
                else
                {
                    _openid = jsonObj["openid"].ToString(); 
                    //检查当前openid 是否已经在系统中存在 
                    string sql_user = "select * from sl_user where openid='" + _openid + "' ";
                    DataTable dt_user = new DataTable();
                    dt_user = my_c.GetTable(sql_user);
                     
                    //建立code和openid的关系
                    string sql_openid_and_code = "INSERT INTO sl_openid (code, openid  ) VALUES ('" + code + "', '" + _openid + "' )  ";
                    my_c.genxin(sql_openid_and_code);

                    if (dt_user.Rows.Count > 0)
                    {
                        //生成新的token start
                        System.Random a = new Random(System.DateTime.Now.Millisecond);
                        int RandKey = a.Next(100000, 999999);
                        string _token = my_b.get_bianhao() + RandKey.ToString();
                        //插入新的token记录
                        string  sql_token = "INSERT INTO sl_token (yonghuming, token,ip ) VALUES ('" + dt_user.Rows[0]["phone"].ToString() + "', '" + _token + "','" + Request.UserHostAddress.ToString() + "')";
                        my_c.genxin(sql_token);
                        //生成新的token end
                        //openid 存在
                        string phone = dt_user.Rows[0]["phone"].ToString();
                        if (this.IsPeisongyuan(phone))
                        {
                            string sql = "select Number,UserName,Name,Phone,State from [User] where phone='" + phone + "' and State=0 ";

                            DataTable user = new DataTable();
                            user = my_c.GetTable(sql, "sql_conn12");
                            //用户名存在，则登录成功
                            if (user.Rows.Count > 0)
                            {
                                DataTable dt_return = new DataTable();
                                //初始化datatable
                                string[] dt_araay = { "Number", "UserName", "Name", "Phone", "State", "Token" };
                                dt_return = my_dt.setdt(dt_araay);

                                DataRow dr_return = dt_return.NewRow();
                                dr_return["Number"] = user.Rows[0]["Number"].ToString();
                                dr_return["UserName"] = user.Rows[0]["UserName"].ToString();
                                dr_return["Name"] = user.Rows[0]["Name"].ToString();
                                dr_return["Phone"] = user.Rows[0]["Phone"].ToString();
                                dr_return["State"] = user.Rows[0]["State"].ToString();
                                dr_return["Token"] = _token;
                                //插入返回数据
                                dt_return.Rows.Add(dr_return);

                                status = "true";
                                msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_return);

                            }
                            else
                            {
                                status = "false";
                                msg = "手机号不存在或账号已被禁用";

                            }
                        }
                        else
                        {
                            status = "false";
                            msg = "无配送权限";
                        
                        }

                        
                    }
                    else
                    {
                        //openid 不存在
                        status = "false";
                        msg = "用户尚未绑定小程序";
                    }
                        


                         

                }
               

                ////查询 用户绑定的配送权限和凭证扫描权限
                //string sql = "select * from [User] where phone='" + Phone + "' and State=0 ";
                ////Response.Write(sql);
                ////Response.End();
                //DataTable user = new DataTable();
                //user = my_c.GetTable(sql, "sql_conn12");
                ////用户名存在，则登录成功
                //if (user.Rows.Count > 0)
                //{
                //    status = "true";
                //    msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(user);
                //    //清除验证码cookie
                //    my_b.admin_o_cookie("shoujiyzm");
                //    //保存用户 UserID
                //    string UserID = string.Empty;
                //    UserID = user.Rows[0]["ID"].ToString();

                //    //插入接口后台用户sl_user数据表
                //    string _sql = "INSERT INTO sl_user (phone, openid,userid ) VALUES ('" + Phone + "', '" + this.getOpenid() + "','" + UserID + "')";
                //    my_c.genxin(_sql);
                //    //my_b.c_cookie(UserID, "UserID");
                //}
                //else
                //{
                //    status = "false";
                //    msg = "手机号不存在或账号已被禁用";

                //}
                //Response.Write(shoujiyzm);
                //Response.End();

            } 
            else
            {
                status = "false";
                msg = "code不能为空";

            }
            #endregion

        }
        else if (type == "peisong")
        {
            //我的配送接口
            /*
             *派单时间
             *用户名
             *
             */

            #region 配送清单接口
            if (is_login())
            {

                DataTable dt = new DataTable();
                //初始化配送清单datatable
                string[] dt_araay = { "Number", "UserName", "Names", "Address", "State", "BeginTime" };
                dt = my_dt.setdt(dt_araay);

                
                string UserID = string.Empty;//当前用户

                string sql_BeginTime = string.Empty;//查询派单日期
                string BeginTime = string.Empty;//派单状态

                string sql_State = string.Empty;//查询派单状态
                string State = string.Empty;//派单状态

                int Page = 1;//页数
                int PageSize = 1000;//每页需要多少条数据
                try
                {
                    Page = int.Parse(Request.Params["Page"]);

                }
                catch { }

                try
                {
                    PageSize = int.Parse(Request.Params["PageSize"]);

                }
                catch { }

                try
                {
                    BeginTime = Request.Params["BeginTime"];

                }
                catch { }

                try
                {
                    State = Request.Params["State"];

                }
                catch { }


                try
                {

                    UserID = this.getUserid();
                }
                catch { }

                if (!string.IsNullOrEmpty(BeginTime))
                {
                    sql_BeginTime = "  and datediff(DAY, UpTime ,'" + BeginTime + "') = 0  ";
                }
                if (!string.IsNullOrEmpty(State))
                {
                    sql_State = "  and State = " + State + "  ";
                }

               

                //查询配送用所在配送点下有哪些客户
                //string kehu_ids = "";
                //string sql_UserGroupLocation = "select userId from UserGroupLocation where GroupLocationID=(select GroupLocationID from UserGroupLocation where userid='" + UserID + "' )";
                //DataTable dt_UserGroupLocation = my_c.GetTable(sql_UserGroupLocation, "sql_conn6");
                //foreach (DataRow dr_UserGroupLocation in dt_UserGroupLocation.Rows)
                //{
                //    if (kehu_ids == "")
                //    {
                //        kehu_ids = dr_UserGroupLocation["userid"].ToString();
                //    }
                //    else
                //    {
                //        kehu_ids =kehu_ids+","+ dr_UserGroupLocation["userid"].ToString();
                //    }

                //}
                //Response.Write(kehu_ids); Response.End();

                //派单列表 
                string sql_number = "select distinct    Number,BeginTime from (select Number,UserName,Address ,State,UpTime as BeginTime  ,UserID  from Service where ServiceID='" + UserID + "' " + sql_BeginTime + sql_State + "   group by Number,UserName,Address ,State,UpTime,UserID ) as t order by BeginTime desc,Number desc";
               
                DataTable dt_number = my_c.GetTable(sql_number, "sql_conn7");


                //翻页
                dt_number = this.GetPagedTable(dt_number, Page, PageSize);
               

                string ServiceIDs = string.Empty;
                //查询所有配送单 service 
                DataTable ServiceAll = my_c.GetTable("select ID,Number,UserName,UserID,Address,State, UpTime , BeginTime,ServiceID,Name,Num  from Service where ServiceID='" + UserID + "' " + sql_BeginTime + sql_State + " ", "sql_conn7");

                //新的配单号
                string[] dt_number1_araay = { "ID","Number", "UserName", "UserID", "Address", "State", "UpTime", "BeginTime", "ServiceID", "Name", "Num" }; 
                DataTable dt_number1 = my_dt.setdt(dt_number1_araay);

                //遍历用户配送的派单列表ID 
                foreach (DataRow dr in dt_number.Rows)
                {
                    //if (string.IsNullOrEmpty(ServiceIDs))
                    //{
                    //    ServiceIDs = ServiceAll.Select(" Number='" + dr["Number"].ToString() + "'")[0]["ID"].ToString();
                    //}
                    //else
                    //{
                    //    ServiceIDs = ServiceIDs+","+ ServiceAll.Select(" Number='" + dr["Number"].ToString() + "'")[0]["ID"].ToString();
                    //}
                    // string sql_number1 = ServiceAll.Select("ID='" + ServiceAll.Select(" Number='" + dr["Number"].ToString() + "'")[0]["ID"].ToString() + "'") 
                    //"select  Number,UserName,UserID,Address,State,BeginTime  from  Service where  ID='" + ServiceAll.Select(" Number='" + dr["Number"].ToString() + "'")[0]["ID"].ToString() + "' order by UpTime desc,Number desc ";
                    ServiceIDs = ServiceAll.Select(" Number='" + dr["Number"].ToString() + "'")[0]["ID"].ToString();
                    //Response.Write(ServiceIDs); Response.Flush();
                    dt_number1.Rows.Add(ServiceAll.Select("ID='" + ServiceIDs + "'")[0].ItemArray );
                }
                 //Response.Write(dt_number1.Rows.Count.ToString()); Response.Flush();

                //string sql_number1 = "select  Number,UserName,UserID,Address,State,BeginTime  from  Service where  charindex(','+ID+',','," + ServiceIDs + ",') > 0  order by UpTime desc,Number desc ";
                // Response.Write(sql_number); Response.End();
                //DataTable dt_number1 = my_c.GetTable(sql_number1, "sql_conn7");

                //遍历用户配送的派单列表
                foreach (DataRow dr in dt_number1.Rows)
                {
                    
                    DataRow dr1 = dt.NewRow();
                    //"UserName", "Names", "Address", "State", "BeginTime"
                    dr1["Number"] = dr["Number"].ToString();
                    dr1["UserName"] = dr["UserName"].ToString();

                    //查询下单用户的地址
                    string Address = "";
                    DataTable dt_user_detailed = my_c.GetTable("select * from [UserDetailed] where UserID='" + dr["UserID"].ToString() + "' and Detailed='详细地址' ", "sql_conn12");
                    try
                    {
                        Address = dt_user_detailed.Rows[0]["value"].ToString();
                    }
                    catch { }
                    //Address
                    if (string.IsNullOrEmpty(dr["Address"].ToString()))
                    {
                        dr1["Address"] = Address;
                    }
                    else
                    {
                        dr1["Address"] = dr["Address"].ToString();
                    }
                    


                    dr1["State"] = dr["State"].ToString();
                    dr1["BeginTime"] = dr["UpTime"].ToString();

                    //查询该派单下的所有商品
                    string sql = string.Empty;
                    sql = " ServiceID='" + UserID + "'  and Number='" + dr["Number"].ToString() + "' ";
                    //Response.Write(sql + "  <br>");
                    //Response.Flush();
                    //所有商品列表
                    DataTable dt2 = this.ToDataTable(ServiceAll.Select(sql));
                    dr1["Names"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt2);
                     
                    //插入返回数据
                    dt.Rows.Add(dr1);

                   // Response.Write("<br/>"+dr["BeginTime"].ToString()+"<br/>"); Response.Flush();
                }
                 
                //dt = my_c.GetTable(sql, "sql_conn7");
                status = "true";
                msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt);
            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion

        }
        else if (type == "is_user")
        {
            //查询用户是否存在 如果用户存在，会自动发送验证码
            /*
             *手机号
             */

            #region 发送验证码接口
            string Phone = string.Empty;


            try
            {
                Phone = Request.Params["phone"];
            }
            catch { }
           
            if (!string.IsNullOrEmpty(Phone) && my_b.ProcessSqlStr(Phone))
            {

                if (this.IsPeisongyuan(Phone))
                {
                    //手机号码存在 则发送验证码
                    string sql = "select * from [User] where phone='" + Phone + "' ";
                    DataTable user = new DataTable();
                    user = my_c.GetTable(sql, "sql_conn12");

                    if (user.Rows.Count > 0)
                    {

                        //判断用户是否被禁用
                        if (user.Rows[0]["State"].ToString() != "0")
                        {
                            status = "false";
                            msg = "账号已禁用";
                        }
                        else
                        {
                            //手机号码存在 
                            //发送验证码    
                            //Response.Write("https://" + Request.ServerVariables["SERVER_NAME"] + "/Execution.aspx?t=fasong&&leixingbiaoti=登录&&type=shouji_yzm&&shouji=" + Phone);
                            //Response.End();
                            msg = my_b.getWebFile1("https://" + Request.ServerVariables["SERVER_NAME"] + "/Execution.aspx?t=fasong&leixingbiaoti=登录&type=shouji_yzm&shouji=" + Phone);

                            status = "true";
                            msg = "短信已发送";
                        }

                        

                    }
                    else
                    {
                        //手机号码不存在
                        status = "false";
                        msg = "手机号不存在";

                    }
                }
                else
                {
                    status = "false";
                    msg = "未绑定配送员权限";
                
                }
                

            }
            else
            {
                status = "false";
                msg = "手机号不能为空";

            } 
            #endregion

        }
        else if (type == "peisongdian")
        {
            //配送地点列表接口
            /*
             *派单时间
             *用户名
             *
             */

            #region 配送地点列表接口
            if (is_login())
            {

                DataTable dt = new DataTable();
                //DataTable dt_UserGroupLocation =new DataTable();//用户和配送点的关联表
                DataTable dt_GroupLocation = new DataTable();//配送点表
                DataTable dt_kehu = new DataTable();
                //初始化配送地点 
                string[] dt_araay = { "Id", "Name", "State", "Latitude", "Longitude" };
                dt = my_dt.setdt(dt_araay);

                string BeginTime = string.Empty; //派送时间
                string UserID = string.Empty;//当前用户
                string sql_BeginTime = string.Empty;//查询派单日期
                
                string Long = string.Empty;//经度
                string Lat = string.Empty;//纬度
                try
                {
                    BeginTime = Request.Params["BeginTime"];

                }
                catch { }
                try
                {

                    UserID = this.getUserid();
                }
                catch { }

                if (!string.IsNullOrEmpty(BeginTime))
                {
                    sql_BeginTime = "  and datediff(DAY, BeginTime ,'" + BeginTime + "') = 0  ";
                }

                try
                {
                    Long = Request.Params["Long"];

                }
                catch { }

                try
                {
                    Lat = Request.Params["Lat"];

                }
                catch { }

                if (string.IsNullOrEmpty(Long))
                {
                    msg= "long经度参数不能为空";
                    retrun_ajax(status, msg);
                    return;
                }

                if (string.IsNullOrEmpty(Lat))
                {
                    msg = "Lat经度参数不能为空";
                    retrun_ajax(status, msg);
                    return;
                }

                //某天内的客户===》配送中的客户
                string sql_kehu = "select UserID   from Service where ServiceID='" + UserID + "' " + sql_BeginTime + " and  ( State=0 )  group by UserID";
                dt_kehu = my_c.GetTable(sql_kehu, "sql_conn7");
                //查询客户对应的配送点
                if (dt_kehu.Rows.Count > 0)
                {
                    foreach (DataRow dr_kehu in dt_kehu.Rows)
                    {
                        string kehu_ID = dr_kehu["UserID"].ToString();
                        string temp_sql = "select top 1 g.Name,g.Latitude,g.Longitude,g.ID from UserGroupLocation as u , GroupLocation as g where u.UserID='" + kehu_ID + "' and g.ID= u.GroupLocationID";//查询配送点的详情
                        dt_GroupLocation = my_c.GetTable(temp_sql, "sql_conn6");

                        if (dt_GroupLocation.Rows.Count > 0)
                        {
                            //{ "Name", "State", "Latitude", "Longitude" };
                            DataRow temp_dr = dt.NewRow();
                            temp_dr["State"] = "配送中";
                            temp_dr["ID"] = dt_GroupLocation.Rows[0]["ID"].ToString();
                            temp_dr["Name"] = dt_GroupLocation.Rows[0]["Name"].ToString();
                            temp_dr["Latitude"] = dt_GroupLocation.Rows[0]["Latitude"].ToString();
                            temp_dr["Longitude"] = dt_GroupLocation.Rows[0]["Longitude"].ToString();
                            //插入返回数据
                            //如果记录已存在则不再插入数据
                            if (dt.Select("ID='" + dt_GroupLocation.Rows[0]["ID"].ToString() + "'").Length == 0)
                            {
                                dt.Rows.Add(temp_dr);
                            }
                        }
                        
                        

                    }
                }
                


                ////某天内的客户===》已配送的客户
                //string sql_kehu1 = "select UserID   from Service where ServiceID='" + UserID + "' " + sql_BeginTime + " and ( State =2 )  group by UserID";
                //dt_kehu = my_c.GetTable(sql_kehu1, "sql_conn7");
                ////查询客户对应的配送点

                //if (dt_kehu.Rows.Count > 0)
                //{
                //    foreach (DataRow dr_kehu in dt_kehu.Rows)
                //    {
                //        string kehu_ID = dr_kehu["UserID"].ToString();
                //        string temp_sql = "select top 1 g.Name,g.Latitude,g.Longitude,g.ID from UserGroupLocation as u , GroupLocation as g where u.UserID='" + kehu_ID + "' and g.ID= u.GroupLocationID";//查询配送点的详情
                //        dt_GroupLocation = my_c.GetTable(temp_sql, "sql_conn6");

                //        //{ "Name", "State", "Latitude", "Longitude" };
                //        DataRow temp_dr = dt.NewRow();
                //        temp_dr["State"] = "2";

                //        if (dt_GroupLocation.Rows.Count > 0)
                //        {
                //            temp_dr["ID"] = dt_GroupLocation.Rows[0]["ID"].ToString();
                //            temp_dr["Name"] = dt_GroupLocation.Rows[0]["Name"].ToString();
                //            temp_dr["Latitude"] = dt_GroupLocation.Rows[0]["Latitude"].ToString();
                //            temp_dr["Longitude"] = dt_GroupLocation.Rows[0]["Longitude"].ToString();

                //            //插入返回数据
                //            //如果记录已存在则不再插入数据
                //            if (dt.Select("ID='" + dt_GroupLocation.Rows[0]["ID"].ToString() + "'").Length == 0)
                //            {
                //                dt.Rows.Add(temp_dr);
                //            }

                //        }
                //        else
                //        {
                //            temp_dr["ID"] = "";
                //            temp_dr["Name"] = "";
                //            temp_dr["Latitude"] = "";
                //            temp_dr["Longitude"] = "";

                        
                //        }
                        

                //    }
                //}


                ////某天内的客户===》已取消的客户
                //string sql_kehu2 = "select UserID   from Service where ServiceID='" + UserID + "' " + sql_BeginTime + " and ( State =3 )  group by UserID";
                //dt_kehu = my_c.GetTable(sql_kehu2, "sql_conn7");
                ////查询客户对应的配送点

                //if (dt_kehu.Rows.Count > 0)
                //{
                //    foreach (DataRow dr_kehu in dt_kehu.Rows)
                //    {
                //        string kehu_ID = dr_kehu["UserID"].ToString();
                //        string temp_sql = "select top 1 g.Name,g.Latitude,g.Longitude,g.ID from UserGroupLocation as u , GroupLocation as g where u.UserID='" + kehu_ID + "' and g.ID= u.GroupLocationID";//查询配送点的详情
                //        dt_GroupLocation = my_c.GetTable(temp_sql, "sql_conn6");

                //        //{ "Name", "State", "Latitude", "Longitude" };
                //        DataRow temp_dr = dt.NewRow();
                //        temp_dr["State"] = "3";
                //        temp_dr["ID"] = dt_GroupLocation.Rows[0]["ID"].ToString();
                //        temp_dr["Name"] = dt_GroupLocation.Rows[0]["Name"].ToString();
                //        temp_dr["Latitude"] = dt_GroupLocation.Rows[0]["Latitude"].ToString();
                //        temp_dr["Longitude"] = dt_GroupLocation.Rows[0]["Longitude"].ToString();
                //        //插入返回数据
                //        //如果记录已存在则不再插入数据
                //        if (dt.Select("ID='" + dt_GroupLocation.Rows[0]["ID"].ToString() + "'").Length == 0)
                //        {
                //            dt.Rows.Add(temp_dr);
                //        }

                //    }
                //}

                //Response.Write(my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt));
                //Response.End();

                //根据当前位置的经纬度排序
                DataTable temp_dt = my_dt.setdt(dt_araay);//处理距离排序后的dt
                double   lat2,  lng2;
                //初始化第一个坐标点
                double lat1, lng1;
                lat1 = double.Parse(Lat);
                lng1 = double.Parse(Long);

                
                int dt_count = dt.Rows.Count;
                string temp_IDs = "";
                while (dt_count > 0)
                {
                    double temp_distance = 0; 
                    dt_count--;
                    string temp_ID = "";
                    DataRow temp_dr1 = dt.NewRow();
                    //Response.Write("<br/>"+temp_IDs+"<br/>");
                    foreach (DataRow temp_dr in dt.Select(temp_IDs))
                    {
                        

                        lat2 = double.Parse(temp_dr["Latitude"].ToString());
                        lng2 = double.Parse(temp_dr["Longitude"].ToString());

                       

                        double temp_distance1 = this.GetDistance(lat1, lng1, lat2, lng2);

                        //Response.Write("" + string.Format("{0}  ,{1}  ,{2}  ,{3},  temp_distance1= {4},  {5}  ", lat1, lng1, lat2, lng2, temp_distance1, temp_dr["Name"].ToString()) + "  " + "<br/>");
                        //Response.Flush();

                        //Response.Write("" + temp_distance1 + "  " + temp_distance + "<br/>");
                        //Response.Flush();

                        if (temp_distance == 0)
                        {
                             
                            temp_distance = temp_distance1;
                            temp_ID = temp_dr["ID"].ToString();


                        }
                        else
                        {
                            
                            if (temp_distance1 < temp_distance)
                            { 
                                temp_distance = temp_distance1;
                                temp_ID = temp_dr["ID"].ToString();
                                
                            }
                        }


                         


                    }
                     

                    DataRow[] DrCount1 = dt.Select("ID='" + temp_ID + "' ");
                    foreach (DataRow row in DrCount1)
                    {
                        temp_dt.Rows.Add(row.ItemArray);
                        //排除不需要排序的配送点
                        if (string.IsNullOrEmpty(temp_IDs))
                        {
                            temp_IDs = "ID <> '" + row["ID"].ToString() + "'";
                        }
                        else
                        {
                            
                            temp_IDs = temp_IDs + " and " + "ID <> '" + row["ID"].ToString() + "'";
                        }

                        //更换下一次的起始点
                        Long = row["Longitude"].ToString();
                        Lat = row["Latitude"].ToString();
                        lat1 = double.Parse(Lat);
                        lng1 = double.Parse(Long);

                        //dt.Rows.Remove(row);
                        //Response.Write("" + string.Format("<br/>{0},{1} ==>选择 {2}", lat1, lng1, row["name"].ToString()) + "<br/>");
                        //Response.Flush();
                    }
                    
                    

                   

                    ////删除原有dt的行
                    //DataRow[] DrCount = dt.Select("ID='" + temp_ID + "'");
                    //foreach (DataRow row in DrCount)
                    //{
                    //    dt.Rows.Remove(row);

                    //}
                   

                }


                status = "true";
                msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(temp_dt);
                //Response.Write(my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt));
                //Response.End();



            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion

        }
        else if (type == "peisongdianxiangxi")
        {
            //配送地点下所属的配送单列表接口
            /*
             *配送地点ID 
             *用户名
             *
             */

            #region 配送地点下所属的配送单列表接口
            if (is_login())
            {
                DataTable dt = new DataTable();
                //初始化配送清单datatable
                string[] dt_araay = { "Number", "UserName", "Names", "Address", "State", "BeginTime", "Long", "Lat","Distance" };
                dt = my_dt.setdt(dt_araay);


                string UserID = string.Empty;//当前用户
                string GroupLocationID = string.Empty;//配送地点ID 

                string sql_BeginTime = string.Empty;//查询派单日期
                string BeginTime = string.Empty;//派单状态

                string sql_State = string.Empty;//查询派单状态
                string State = string.Empty;//派单状态

                string Long = string.Empty;//经度
                string Lat = string.Empty;//纬度

                int Page = 1;//页数
                int PageSize = 1000;//每页需要多少条数据
                try
                {
                    Page = int.Parse(Request.Params["Page"]);

                }
                catch { }

                try
                {
                    PageSize = int.Parse(Request.Params["PageSize"]);

                }
                catch { }


                try
                {
                    State = Request.Params["State"];

                }
                catch { }

                try
                {
                    BeginTime = Request.Params["BeginTime"];

                }
                catch { }

                if (!string.IsNullOrEmpty(BeginTime))
                {
                    sql_BeginTime = "  and datediff(DAY, UpTime ,'" + BeginTime + "') = 0  ";
                }

                if (!string.IsNullOrEmpty(State))
                {
                    sql_State = "  and State = " + State + "  ";
                }

                try
                {

                    UserID = this.getUserid();
                }
                catch { }

                try
                {

                    GroupLocationID = Request.Params["groupLocationid"];
                }
                catch { }


                try
                {
                    Long = Request.Params["Long"];

                }
                catch { }

                try
                {
                    Lat = Request.Params["Lat"];

                }
                catch { }

                if (string.IsNullOrEmpty(Long))
                {
                    msg = "long经度参数不能为空";
                    retrun_ajax(status, msg);
                    return;
                }

                if (string.IsNullOrEmpty(Lat))
                {
                    msg = "Lat纬度参数不能为空";
                    retrun_ajax(status, msg);
                    return;
                }



                if (!string.IsNullOrEmpty(GroupLocationID))
                {
                    // charindex(','+ID+',','," + ServiceIDs + ",') > 0 
                    string sql_number_all = "select  distinct * from (select Number,UserName,Address ,State,CONVERT(varchar(100), UpTime , 23) as BeginTime  ,UserID,Name,Num,ServiceID  from Service where ServiceID='" + UserID + "'  " + sql_BeginTime + sql_State + " group by Number,UserName,Address ,State,UpTime,UserID,Name,Num,ServiceID) as t order by BeginTime desc,State asc";
                    DataTable dt_number_all = my_c.GetTable(sql_number_all, "sql_conn7");
                     
                    //遍历配送点对应的客户
                    DataTable dt_UserGroupLocation = my_c.GetTable(" select UserID from  UserGroupLocation     where  GroupLocationID='" + GroupLocationID + "'   ", "sql_conn6");
                    //查找这个配送点下面的对应客户
                    for (int i = dt_UserGroupLocation.Rows.Count - 1; i >= 0; i--)
                    {
                        string kehuID_temp = dt_UserGroupLocation.Rows[i]["UserID"].ToString();
                        if (dt_number_all.Select("UserID='" + kehuID_temp + "' ").Length == 0)
                        {
                            dt_UserGroupLocation.Rows.RemoveAt(i);
                        }
                        else
                        {
                            //Response.Write("1<br/>" + kehuID_temp);
                        }
                        

                    }
                    //Response.End();

                    foreach (DataRow dr_kehu in dt_UserGroupLocation.Rows)
                    {

                        //Response.Write(dr_kehu["UserID"].ToString()); Response.Flush();
                        //得到客户的ID
                        string kehuID = dr_kehu["UserID"].ToString();
                        //查询service表中 该配送点的下单客户  UserID=kehuID
                        //当前派单属于当前的配送员   ServiceID = userID
                        //派单列表 
                        //string sql_number = "select Number,UserName,Address ,State,BeginTime  from Service where ServiceID='" + UserID + "' and UserID='" + kehuID + "' " + sql_State + " group by Number,UserName,Address ,State,BeginTime ";
                        string sql_number = "select  distinct * from (select Number,UserName,Address ,State,CONVERT(varchar(100), UpTime , 23) as BeginTime  ,UserID  from Service where ServiceID='" + UserID + "' and UserID='"+kehuID+"' " + sql_BeginTime + sql_State + " group by Number,UserName,Address ,State,UpTime,UserID ) as t order by BeginTime desc,State asc";

                        string sql_UserDetailed = "select  * from UserDetailed where UserID='" + kehuID + "' and  Detailed='详细地址' ";  //
                        string sql_Long = "select top 1  * from UserDetailed where UserID='" + kehuID + "' and  Detailed='经度' ";  //
                        string sql_Lat = "select top 1 * from UserDetailed where UserID='" + kehuID + "' and  Detailed='维度' ";  //
                        //Response.Write(sql_UserDetailed); Response.End();
                        //DataTable dt_number = my_c.GetTable(sql_number, "sql_conn7");
                        DataTable dt_number =this.ToDataTable(dt_number_all.Select("UserID='" + kehuID + "' "));
                        DataTable dt_UserDetailed = my_c.GetTable(sql_UserDetailed, "sql_conn12");

                        DataTable dt_UserLong = my_c.GetTable(sql_Long, "sql_conn12");
                        DataTable dt_UserLat = my_c.GetTable(sql_Lat, "sql_conn12");

                        //dt_number = this.GetPagedTable(dt_number,Page,PageSize);

                        //遍历用户配送的派单列表
                        foreach (DataRow dr in dt_number.Rows)
                        { 
                            DataRow dr1 = dt.NewRow();
                            //"UserName", "Names", "Address", "State", "BeginTime"
                            dr1["Number"] = dr["Number"].ToString();
                            dr1["UserName"] = dr["UserName"].ToString();


                            try
                            {
                                dr1["Address"] = dt_UserDetailed.Rows[0]["value"].ToString();
                            }
                            catch
                            {
                                dr1["Address"] = dr["Address"].ToString();
                            }

                            try
                            {
                                dr1["Long"] = dt_UserLong.Rows[0]["value"].ToString();
                                dr1["Lat"] = dt_UserLat.Rows[0]["value"].ToString();
                                //Distance
                                dr1["Distance"] = this.GetDistance(double.Parse(Long), double.Parse(Lat), double.Parse(dr1["Long"].ToString()), double.Parse(dr1["Lat"].ToString()));
                            }
                            catch {
                                dr1["Long"] = "0";
                                dr1["Lat"] = "0";
                                dr1["Distance"] = "0";
                            }
                            //dr1["Address"] = dt_UserDetailed.Rows[0]["value"].ToString();
                           

                            dr1["State"] = dr["State"].ToString();
                            dr1["BeginTime"] = dr["BeginTime"].ToString();

                            //查询该派单下的所有商品
                            string sql = string.Empty;
                            sql = "select Name,Num   from Service as s1 where  s1.ServiceID='" + UserID + "'  and s1.Number='" + dr["Number"].ToString() + "' ";
                            //Response.Write(sql + "  <br>");
                            //Response.Flush();
                            //所有商品列表
                            
                            DataTable dt2 = this.ToDataTable(dt_number_all.Select(" ServiceID='" + UserID + "'  and  Number='" + dr["Number"].ToString() + "'"));
                            dr1["Names"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt2);

                            //插入返回数据
                            dt.Rows.Add(dr1);


                        }

                    }
 
                    //根据距离排序
                    dt.DefaultView.Sort = "Distance DESC";
                    dt = dt.DefaultView.ToTable();

                    dt = this.GetPagedTable(dt,Page,PageSize);
                    //dt = my_c.GetTable(sql, "sql_conn7");
                    status = "true";
                    msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt);

                }
                else
                {
                    msg = "配送地点ID 不能为空";
                }

               
                


            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion

        }
        else if (type == "paidanxiangxi")
        {
            //配送点下的具体一个派单的详细数据接口
            /*
             *派单时间
             *用户名
             *
             */
            #region 配送点下的具体一个派单的详细数据接口
            if (is_login())
            {
                string Number = string.Empty;//派单ID 
                string UserID = string.Empty;//当前用户
                
                try
                {

                    Number = Request.Params["number"];
                }
                catch { }

                try
                {

                    UserID = this.getUserid();
                }
                catch { }
               

                if (!string.IsNullOrEmpty(Number))
                {
                    DataTable dt = new DataTable();
                    //初始化派单的详细数据datatable
                    string[] dt_araay = { "ServiceDetail", "Commodity", "ServiceRemarks", "UserNumber", "Address", "Long", "Lat" };
                    dt = my_dt.setdt(dt_araay);

 
                    //查询当前派单的详情 （配单号；状态；时间；地址 ；用户姓名；用户电话；配送员姓名；配送员电话；）
                    //string sql_service = "select ID,UserID, Number, CONVERT(varchar(100), Uptime, 121),ServiceName,ServicePhone,Address,State,UserName,UserPhone,FromValue as OrderID  from Service where ServiceID='" + UserID + "' and Number='" + Number + "' group by ID,UserID ,Number,Uptime,ServiceName,ServicePhone,Address,State,UserName,UserPhone,FromValue";
                    string sql_service = "select ID,UserID, Number, CONVERT(varchar(100), Uptime, 121),ServiceName,ServicePhone,Address,State,UserName,UserPhone,FromValue as OrderID  from Service where  Number='" + Number + "' group by ID,UserID ,Number,Uptime,ServiceName,ServicePhone,Address,State,UserName,UserPhone,FromValue";
                    DataTable dt_service = my_c.GetTable(sql_service, "sql_conn7");
                    //查询下单用户编号
                    string kehuID ="";
                    try
                    {
                         kehuID = dt_service.Rows[0]["UserID"].ToString();
                    }
                    catch { }
                    //Response.Write(kehuID);
                    //Response.End();
                    //
                    string sql_UserDetailed = "select  * from UserDetailed where UserID='" + kehuID + "' and  Detailed='详细地址' ";  //
                    DataTable dt_UserDetailed = my_c.GetTable(sql_UserDetailed, "sql_conn12");

                    //查询客户的经纬度
                    string sql_Long = "select top 1  * from UserDetailed where UserID='" + kehuID + "' and  Detailed='经度' ";  //
                    string sql_Lat = "select top 1 * from UserDetailed where UserID='" + kehuID + "' and  Detailed='维度' ";  //
                     
                    DataTable dt_UserLong = my_c.GetTable(sql_Long, "sql_conn12");
                    DataTable dt_UserLat = my_c.GetTable(sql_Lat, "sql_conn12");

                    //
                    string Address = "";
                    try
                    {
                        Address = dt_UserDetailed.Rows[0]["value"].ToString();
                    }
                    catch { }

                    string UserNumber = "";
                    try
                    {
                        UserNumber = my_c.GetTable("select Number from [user] where id='" + kehuID + "'", "sql_conn12").Rows[0]["Number"].ToString();
                    }
                    catch { }

                     

                    //查询当前派单的商品列表
                    //string sql_commodity = "select Name,Body,State from Service where  ServiceID='" + UserID + "' and Number='" + Number + "' ";
                    string sql_commodity = "select Name,Body,State from Service where Number='" + Number + "' ";

                    DataTable dt_commodity = my_c.GetTable(sql_commodity, "sql_conn7");
                    
                    //备注
                    string temp_ServiceID = string.Empty;
                    try
                    {
                        temp_ServiceID = dt_service.Rows[0]["ID"].ToString();
                    }
                    catch {
                        temp_ServiceID = "no data";
                    }
                    string sql_serviceremarks = "select Body,CONVERT(varchar(100), InTime, 121) as InTime from ServiceRemarks where  ServiceID='" + temp_ServiceID + "' ";


                    DataTable dt_serviceremarks = my_c.GetTable(sql_serviceremarks, "sql_conn7");

                    DataRow temp_dr = dt.NewRow();
                    temp_dr["ServiceDetail"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_service);
                    temp_dr["Commodity"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_commodity);
                    temp_dr["ServiceRemarks"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_serviceremarks);
                    temp_dr["UserNumber"] = UserNumber;
                    temp_dr["Address"] = Address;

                    try
                    {
                        temp_dr["Long"] = dt_UserLong.Rows[0]["value"].ToString();
                        temp_dr["Lat"] = dt_UserLat.Rows[0]["value"].ToString();
                         
                    }
                    catch
                    {
                        temp_dr["Long"] = "0";
                        temp_dr["Lat"] = "0";
                         
                    }

                    //插入返回数据
                    dt.Rows.Add(temp_dr);

                    status = "true";
                    msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt);
                }
                else
                {
                    msg = "派单ID 不能为空";
                }


            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion



        }
        else if (type == "dingdanxiangxi")
        {
            //查询订单详细数据接口
            /*
             *派单时间
             *用户名
             *
             */
            #region 查询订单详细数据接口
            if (is_login())
            {
                string FromValue = string.Empty;//订单号
                string UserID = string.Empty;//当前用户

                try
                {

                    FromValue = Request.Params["orderid"];
                }
                catch { }

                try
                {

                    UserID = this.getUserid();
                }
                catch { }


                if (!string.IsNullOrEmpty(FromValue))
                {
                    DataTable dt = new DataTable();
                    //初始化派单的详细数据datatable 配送单编号ServerNumber
                    string[] dt_araay = { "OrderDetail", "Recode", "Commodity", "ServiceNumber" };
                    dt = my_dt.setdt(dt_araay);

                    //查询订单详情
                    DataTable dt_Order = my_c.GetTable("select ID,Number,State,paystate, CONVERT(varchar(100), UpTime, 121) as UpTime,Freight as yunfei   from [Order] where id='" + FromValue + "' or Number='" + FromValue + "' ", "sql_conn2");
                    if (dt_Order.Rows.Count > 0)
                    {
                        //订单表主键
                        string ID = dt_Order.Rows[0]["ID"].ToString();
                        //生成二维码
                        string dingdanhao = dt_Order.Rows[0]["Number"].ToString();
                        string erweima_url_str = "http://tyindex.cqtyrl.com/indexf.aspx?OrderID=" + dingdanhao;
                        string erweima_url_img = CreateCode_Simple(erweima_url_str);
                        //通过订单ID查询配送单的number字段
                        DataTable dt_Service = my_c.GetTable("select Number   from [Service] where  FromValue='"+ID+"' ", "sql_conn7");
                        
                        string ServiceNumber = "";
                        if (dt_Service.Rows.Count > 0)
                        {
                             ServiceNumber = dt_Service.Rows[0]["Number"].ToString();
                        }
                        //订单包含商品
                        DataTable dt_Commodity = my_c.GetTable("select Name, (NowPrice * Num) as Price, NowPrice as danjia ,ImgFile,Num  from OrderCommodity where OrderID='" + ID + "'", "sql_conn2");



                        DataRow temp_dr = dt.NewRow();
                        temp_dr["OrderDetail"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_Order);
                        temp_dr["Recode"] = erweima_url_img;
                        temp_dr["Commodity"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_Commodity);
                        temp_dr["ServiceNumber"] = ServiceNumber;
                        //插入返回数据
                        dt.Rows.Add(temp_dr);

                        status = "true";
                        msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt);
                    }
                    else {
                        status = "false";
                        msg = "订单号不存在";
                    
                    }
                    
                }
                else
                {
                    msg = "订单ID 不能为空";
                }


            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion



        }
        else if (type == "quanxian")
        {
            //查询用户是否具有配送员和凭证权限
            /*
             *手机号
             */
            #region 是否具有配送员和凭证权限
            string Phone = string.Empty;

            try
            {
                Phone = Request.Params["phone"];
            }
            catch { }
            if (!string.IsNullOrEmpty(Phone) && my_b.ProcessSqlStr(Phone))
            {
                string sql = "select ID from [User] where phone='" + Phone + "'  ";
                DataTable user = new DataTable();
                user = my_c.GetTable(sql, "sql_conn12");
                if (user.Rows.Count > 0)
                {
                    string UserID = user.Rows[0]["ID"].ToString();
                    string ps_UserIDs = string.Empty;//全部配送用户的ID
                    string pz_UserIDs = string.Empty;//全部凭证用户的ID

                    try
                    {

                        ps_UserIDs = my_b.getWebFile1(ConfigurationSettings.AppSettings["ps_url"].ToString());//全部配送用户的ID
                        pz_UserIDs = my_b.getWebFile1(ConfigurationSettings.AppSettings["pj_url"].ToString());//全部凭证用户的ID
                    }
                    catch { }

                    if (!string.IsNullOrEmpty(ps_UserIDs) && !string.IsNullOrEmpty(pz_UserIDs))
                    {
                        DataTable dt = new DataTable();
                        //初始化配送清单datatable
                        string[] dt_araay = { "peisong_quanxian", "pingzheng_quanxian" };
                        dt = my_dt.setdt(dt_araay);

                        string temp_peisong_quanxian = "false";
                        string temp_pingzheng_quanxian = "false";
                        if (ps_UserIDs.IndexOf(UserID) >= 0)
                        {
                            temp_peisong_quanxian = "true";
                        }
                        if (pz_UserIDs.IndexOf(UserID) >= 0)
                        {
                            temp_pingzheng_quanxian = "true";
                        }

                        DataRow temp_dr = dt.NewRow();
                        temp_dr["peisong_quanxian"] = temp_peisong_quanxian;
                        temp_dr["pingzheng_quanxian"] = temp_pingzheng_quanxian;
                        //插入返回数据
                        dt.Rows.Add(temp_dr);


                        status = "true";
                        msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt);

                    }
                    else
                    {
                        status = "false";
                        msg = "全部配送用户的ID 或 全部凭证用户的ID 不存在";

                    }



                }
                else
                {
                    status = "false";
                    msg = "用户不存在";

                }
            }
            else
            {
                status = "false";
                msg = "参数phone不能为空";

            } 
            #endregion


        }
        else if (type == "pingjuxiangxi")
        {
            //查询凭据详细数据接口
            /*
             *凭据令牌==> password
             *用户名
             *
             */
            #region 查询订单详细数据接口
            if (is_login())
            {
                string Ticket = string.Empty;//凭据的password
                  
                try
                {

                    Ticket = Request.Params["Ticket"];//password
                }
                catch { }

                if (!string.IsNullOrEmpty(Ticket))
                {
                    DataTable dt = new DataTable();
                    //初始化派单的详细数据datatable
                    string[] dt_araay = { "TicketDetail", "UserDetail","OrderID"};
                    dt = my_dt.setdt(dt_araay);

                    //查询凭据详情
                    //
                    DataTable dt_Ticket = my_c.GetTable("select  t.ID,t.UserID,t.Number,t.Body,t.State,t.Name,d.Value,l.userurl    from Ticket as t,TicketDetailed as d,TicketLable as l  where t.password='" + Ticket + "' and d.TicketID=t.ID and l.TicketID=t.ID", "sql_conn5");
                    DataTable dt_user = new DataTable();

                    //select Number,Name,Phone from [User] where ID='7e6c62d2-3118-4975-99bb-beeb1097e6a3'
                    string UserID = "";
                    string OrderID = "";
                    try
                    {
                        UserID = dt_Ticket.Rows[0]["UserID"].ToString();
                        OrderID = dt_Ticket.Rows[0]["userurl"].ToString();//订单ID
                        var uri = new Uri(OrderID);
                        var query = HttpUtility.ParseQueryString(uri.Query);
                        OrderID = query.Get("ID");
                    }
                    catch { }
                     
                    if (!string.IsNullOrEmpty(UserID))
                    {
                        dt_user = my_c.GetTable("select Number,Name,Phone from [User] where ID='" + UserID + "'", "sql_conn12");
                    
                    }

                    

                    DataRow temp_dr = dt.NewRow();
                    temp_dr["TicketDetail"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_Ticket);
                    temp_dr["UserDetail"] = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_user);
                    temp_dr["OrderID"] = OrderID; 
                    //插入返回数据
                    dt.Rows.Add(temp_dr);

                    status = "true";
                    msg = my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt);
                }
                else
                {
                    msg = "凭据令牌PassWord 不能为空";
                }


            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion



        }
        else if (type == "pingjushiyong")
        {
            //使用扫描获得的凭据数据接口
            /*
             *凭据id TicketID 
             *用户名 token
             *
             */
            #region 使用扫描获得的凭据数据接口
            if (is_login())
            {
                string TicketID = string.Empty;//凭据令牌

                try
                {

                    TicketID = Request.Params["TicketID"];
                }
                catch { }

                if (!string.IsNullOrEmpty(TicketID))
                {
                    
                    //查询凭据详情
                    //
                    DataTable dt_Ticket = my_c.GetTable("select * from Ticket where ID='" + TicketID + "' ", "sql_conn5");

                    //凭据当前状态，只有State==0 才能修改状态
                    if (dt_Ticket.Rows.Count > 0)
                    {
                        string State = "";
                        State = dt_Ticket.Rows[0]["State"].ToString();

                        if (State=="0")
                        {
                            //查询当前使用凭据的配送员
                            string UserID = this.getUserid();
                            DataTable dt_user = my_c.GetTable("");
                            dt_user = my_c.GetTable("select Number,Name,Phone from [User] where ID='" + UserID + "'", "sql_conn12");
                            string Number = "";
                            string Name = "";
                            if (dt_user.Rows.Count > 0)
                            {
                                Number = dt_user.Rows[0]["Number"].ToString();
                                Name = dt_user.Rows[0]["Name"].ToString();
                            }

                            string sql_update_ticket = "update Ticket set State=1 , UseRemarks='" + Name + "（" + Number + "）' ,UseTime = getdate() where id='" + TicketID + "'";

                            my_c.genxin(sql_update_ticket, "sql_conn5");
                            status = "true";
                            msg = "使用成功";
                           
                        }
                        else
                        {
                            status = "false";
                            msg = "状态" + State + ",不能使用";
                        }
                    }
                    else
                    {
                        msg = "凭据不存在，请稍后重试";
                    }
                     
                    
                }
                else
                {
                    msg = "凭据id 不能为空";
                }


            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion

        }
        else if (type == "shenqingpeisong")
        {
            //使用扫描获得的凭据数据接口
            /*
             *订单主键id orderid 
             *用户名 token
             *
             */
            #region 查询订单详细数据接口
            if (is_login())
            {
                string orderid = string.Empty;// 

                try
                {

                    orderid = Request.Params["orderid"];
                }
                catch { }

                if (!string.IsNullOrEmpty(orderid))
                {
                    string sql_Service = "";
                    string sql_ServiceLable = "";
                    string sql_ServiceLog = "";
                    string ID, Number, Type, BeginTime, ServiceBodyID, Name, Body, Num, FromType, FromValue, UserID, UserName, UserPhone, ServiceID, ServiceName, ServicePhone, Address, State, EndUrl, OutUrl, UpTime, InTime;

                    //插入Service记录
                    sql_Service =sql_Service+ "INSERT INTO Service (";
                    sql_Service = sql_Service + "ID, Number,Type,BeginTime,ServiceBodyID,Name,Body,Num,FromType,FromValue,UserID,UserName,UserPhone,ServiceID,ServiceName,ServicePhone,Address,State,EndUrl,OutUrl,UpTime,InTime ";
                    sql_Service = sql_Service + ") VALUES (";
                    sql_Service = sql_Service + "'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}','{21}' ";
                    sql_Service = sql_Service + ")";

                    //查询订单详情
                     
                    DataTable dt_Order = my_c.GetTable("select * , CONVERT(varchar(100), UpTime, 121) as UpTime   from [Order] where id='" + orderid + "' or Number='" + orderid + "' ", "sql_conn2");
                    if (dt_Order.Rows.Count > 0)
                    {
                        //查询订单的状态
                        string order_State = dt_Order.Rows[0]["State"].ToString();
                        if (order_State != "0")
                        {
                            status = "false";
                            msg = "订单已分配";
                        }
                        else
                        {
                           
                            //统一数据
                           
                            Number ="SE"+ my_b.get_bianhao();
                            Type = "0";
                            BeginTime = DateTime.Now.ToString();
                            ServiceBodyID = "";
                            FromType = "天源物流配送";
                            FromValue = dt_Order.Rows[0]["ID"].ToString();
                            UserID = dt_Order.Rows[0]["UserID"].ToString();
                            ServiceID = this.getUserid();
                            State = "2";//该配送单直接就是已经完成配送
                            EndUrl = "http://tylogistics.cqtyrl.com/WebApi/EndService.ashx?OrderID=" + dt_Order.Rows[0]["ID"].ToString();
                            OutUrl = "http://tylogistics.cqtyrl.com/WebApi/OutService.ashx?OrderID=" + dt_Order.Rows[0]["ID"].ToString();
                            UpTime = DateTime.Now.ToString();
                            InTime = DateTime.Now.ToString();

                            //查询用户详情
                            DataTable dt_user = my_c.GetTable("select * from [User] where id='" + UserID + "'", "sql_conn12");
                            DataTable dt_user_detailed = my_c.GetTable("select * from [UserDetailed] where UserID='" + UserID + "' and Detailed='详细地址' ", "sql_conn12");
                            UserName = dt_user.Rows[0]["Name"].ToString();
                            UserPhone = dt_user.Rows[0]["Phone"].ToString();
                            try
                            {
                                Address = dt_user_detailed.Rows[0]["value"].ToString();
                            }
                            catch { }
                            Address = "";
                            //查询配送员详情

                            DataTable dt_user_peisongyuan = my_c.GetTable("select * from [User] where id='" + ServiceID + "'", "sql_conn12");
                            ServiceName = dt_user_peisongyuan.Rows[0]["Name"].ToString();
                            ServicePhone = dt_user_peisongyuan.Rows[0]["Phone"].ToString(); ;


                            //订单包含商品
                            DataTable dt_Commodity = my_c.GetTable("select ID,Name, NowPrice as Price,ImgFile,Num,Explain  from OrderCommodity where OrderID='" + dt_Order.Rows[0]["ID"].ToString() + "'", "sql_conn2");

                            //更新订单状态
                            string sql_order_state = "update [order] set state=3  where  id='" + orderid + "' or Number='" + orderid + "' ";
                            my_c.genxin(sql_order_state, "sql_conn2");

                            foreach (DataRow dr_Commodity in dt_Commodity.Rows)
                            {

                                ID = my_b.md5(dr_Commodity["ID"].ToString());
                                Name = dr_Commodity["Name"].ToString();
                                //Body = "数量：" + dr_Commodity["Num"].ToString() + "  备注：" + dr_Commodity["Explain"].ToString() + "  单价：" + dr_Commodity["Price"].ToString() + "  总价：" + (float.Parse(dr_Commodity["Num"].ToString()) * float.Parse(dr_Commodity["Price"].ToString())).ToString() + "";
                                Body = "数量：" + dr_Commodity["Num"].ToString()  + "  单价：" + dr_Commodity["Price"].ToString() + "  总价：" + (float.Parse(dr_Commodity["Num"].ToString()) * float.Parse(dr_Commodity["Price"].ToString())).ToString() + "";

                                Num = dr_Commodity["Num"].ToString();

                                //更新service表
                               string  sql_Service1 = string.Format(sql_Service, ID, Number, Type, BeginTime, ServiceBodyID, Name, Body, Num, FromType, FromValue, UserID, UserName, UserPhone, ServiceID, ServiceName, ServicePhone, Address, State, EndUrl, OutUrl, UpTime, InTime);
                               my_c.genxin(sql_Service1, "sql_conn7");
                               //Response.Write("<br/>" + sql_Service1);
                               // Response.Flush();

                            }


                            string time = System.DateTime.Now.ToString();
                            string sql_OrderLog = "insert into  OrderLog (ID,OrderID,Body,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + dt_Order.Rows[0]["ID"].ToString() + "','订单状态由 已创建 改变为 处理中  ','" + time + "' )";
                            string sql_OrderLog2 = "insert into  OrderLog (ID,OrderID,Body,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + dt_Order.Rows[0]["ID"].ToString() + "','订单状态由 处理中 改变为 完成  ','" + time + "' )";

                            string sql_service_detail = "select  * from [Service]  where  FromValue='" + dt_Order.Rows[0]["ID"].ToString() + "'  ";
                            string ServiceNumber = "";
                            try
                            {
                                ServiceNumber = my_c.GetTable(sql_service_detail, "sql_conn7").Rows[0]["Number"].ToString();
                            }
                            catch { }
                            string sql_OrderLable = "insert into  OrderLable (ID,OrderID,Name,UserUrl,AdminUrl,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + dt_Order.Rows[0]["ID"].ToString() + "','配送','http://tyservice.cqtyrl.com/Show/ServiceShow.aspx?ID=" + ServiceNumber + "','http://tyservice.cqtyrl.com/Management/ServiceShow.aspx?ID=" + ServiceNumber + "','" + time + "' )";

                            string serviceID = "";
                            string serviceBody = "";
                            try
                            {
                                DataTable dt_service1 = my_c.GetTable("select * from [Service] where   FromValue='" + dt_Order.Rows[0]["ID"].ToString() + "'", "sql_conn7");
                                serviceID = dt_service1.Rows[0]["id"].ToString();
                                serviceBody = dt_service1.Rows[0]["Body"].ToString();

                            }
                            catch { }

                             sql_ServiceLog = "insert into  ServiceLog (ID,ServiceID,Name,Body,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + serviceID + "','创建配送','" + serviceBody + "','" + time + "' )";
                            string sql_ServiceLog1 = "insert into  ServiceLog (ID,ServiceID,Name,Body,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + serviceID + "','全部完成','全部完成','" + time + "' )";
                            sql_ServiceLable = "insert into  ServiceLable (ID,ServiceID,Name,UserUrl,AdminUrl,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + serviceID + "','订单','http://tyorder.cqtyrl.com/Show/OrderShow.aspx?ID=" + dt_Order.Rows[0]["ID"].ToString() + "','http://tyorder.cqtyrl.com/Management/OrderShow.aspx?ID=" + dt_Order.Rows[0]["ID"].ToString() + "','" + time + "' )";

                            //更改OrderLog
                            my_c.genxin(sql_OrderLog, "sql_conn2");
                            my_c.genxin(sql_OrderLog2, "sql_conn2");
                            //更改OrderLable
                            my_c.genxin(sql_OrderLable, "sql_conn2");
                            //插入ServiceLable记录
                            my_c.genxin(sql_ServiceLable, "sql_conn7");

                            //插入ServiceLog记录
                            my_c.genxin(sql_ServiceLog, "sql_conn7");
                            my_c.genxin(sql_ServiceLog1, "sql_conn7");

                            status = "true";
                            msg = "分配成功";
                    
                        }
                        
                    }
                    

                   

                    

                    
                    


                }
                else
                {
                    msg = "订单主键 不能为空";
                }


            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion

        }
        else if (type == "peisongwancheng")
        {
            //使用扫描获得的凭据数据接口
            /*
             *订单主键id orderid 
             *用户名 token
             *
             */
            #region 使用扫描获得的凭据数据接口
            if (is_login())
            {
                string orderid = string.Empty;// 
                string State = string.Empty;// 
                try
                {

                    orderid = Request.Params["orderid"];
                }
                catch { }
                try
                {

                    State = Request.Params["State"];
                }
                catch { }

                if (!string.IsNullOrEmpty(orderid))
                {
                    
                    //查询订单详情

                    DataTable dt_Order = my_c.GetTable("select *  from [Order] where id='" + orderid + "' or Number='" + orderid + "' ", "sql_conn2");
                    if (dt_Order.Rows.Count > 0)
                    {
                        //查询订单的状态
                        string order_State = dt_Order.Rows[0]["State"].ToString();
                        if (order_State != "1")
                        {
                            string service_state_old = "";
                            if (order_State == "0")
                            {
                                service_state_old = "正常";
                            }
                            else if (order_State == "1")
                            {
                                service_state_old = "服务中";
                            }
                            else if (order_State == "2")
                            {
                                service_state_old = "完成";
                            }
                            else if (order_State == "3")
                            {
                                service_state_old = "结束";
                            }
                            else if (order_State == "4")
                            {
                                service_state_old = "取消";
                            }

                            status = "false";
                            msg = "错误:订单已" + service_state_old;
                        }
                        else
                        {

                            string order_state = State == "完成" ? "3" : "6";
                            string service_state = State == "完成" ? "2" : "3";

                            string sql_order_state = "update [order] set state=" + order_state + "  where  id='" + orderid + "' or Number='" + orderid + "' ";

                            string sql_service_state = "update [Service] set state=" + service_state + "  where  FromValue='" + dt_Order.Rows[0]["ID"].ToString() + "'  ";
                            string service_state_old = "";//0正常,1服务中,2完成,3结束,4取消
                            if (order_State == "0")
                            {
                                service_state_old = "正常";
                            }
                            else if (order_State == "1")
                            {
                                service_state_old = "服务中";
                            }
                            else if (order_State == "2")
                            {
                                service_state_old = "完成";
                            }
                            else if (order_State == "3")
                            {
                                service_state_old = "结束";
                            }
                            else if (order_State == "4")
                            {
                                service_state_old = "取消";
                            }
                            string time = System.DateTime.Now.ToString();
                            string sql_OrderLog = "insert into  OrderLog (ID,OrderID,Body,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + dt_Order.Rows[0]["ID"].ToString() + "','订单状态由 " + service_state_old + " 改变为 " + State + "  ','" + time + "' )";

                            string sql_service_detail = "select top 1 * from [Service]  where  FromValue='" + dt_Order.Rows[0]["ID"].ToString() + "'  ";
                            string ServiceNumber = "";
                            try
                            {
                                ServiceNumber = my_c.GetTable(sql_service_detail, "sql_conn7").Rows[0]["Number"].ToString();
                            }
                            catch { }
                            string sql_OrderLable = "insert into  OrderLable (ID,OrderID,Name,UserUrl,AdminUrl,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + dt_Order.Rows[0]["ID"].ToString() + "','配送','http://tyservice.cqtyrl.com/Show/ServiceShow.aspx?ID=" + ServiceNumber + "','http://tyservice.cqtyrl.com/Management/ServiceShow.aspx?ID=" + ServiceNumber + "','" + time + "' )";

                            string serviceID = "";
                            string serviceBody = "";
                            try
                            {
                                DataTable dt_service1 = my_c.GetTable("select * from [Service] where   FromValue='" + dt_Order.Rows[0]["ID"].ToString() + "'", "sql_conn7");
                                serviceID = dt_service1.Rows[0]["id"].ToString();
                                serviceBody = dt_service1.Rows[0]["Body"].ToString();

                            }
                            catch { }

                             
                            string sql_ServiceLog = "insert into  ServiceLog (ID,ServiceID,Name,Body,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + serviceID + "','全部完成','全部完成','" + time + "' )";
                            string sql_ServiceLable = "insert into  ServiceLable (ID,ServiceID,Name,UserUrl,AdminUrl,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + serviceID + "','订单','http://tyorder.cqtyrl.com/Show/OrderShow.aspx?ID=" + dt_Order.Rows[0]["ID"].ToString() + "','http://tyorder.cqtyrl.com/Management/OrderShow.aspx?ID=" + dt_Order.Rows[0]["ID"].ToString() + "','" + time + "' )";



                            //更新订单状态
                            my_c.genxin(sql_order_state, "sql_conn2");
                            //更改service配送表状态
                            my_c.genxin(sql_service_state, "sql_conn7");
                                //插入ServiceLable记录
                            //    my_c.genxin(sql_ServiceLable, "sql_conn7");

                            //    //插入ServiceLog记录
                            //    my_c.genxin(sql_ServiceLog, "sql_conn7");
                            

                            ////更改OrderLog
                            my_c.genxin(sql_OrderLog, "sql_conn2");
                            ////更改OrderLable
                            //my_c.genxin(sql_OrderLable, "sql_conn2");

                            status = "true";
                            msg = "操作成功";

                        }

                    }

                } 
                else
                {
                    msg = "订单主键 不能为空";
                }


            } 
            else
            {
                msg = "请登录后再操作";
            }
            #endregion

        }
        else if (type == "peisongjuli")
        {
            #region 订单的配送距离 单位 米

            string peisongjuli = ConfigurationSettings.AppSettings["peisongjuli"].ToString();
            Response.Write(peisongjuli);
            Response.End(); 
            #endregion
        }
        else if (type == "peisongwanchengrizhi")
        {
            #region 配送完成后记录配送员的位置信息
            string orderid = string.Empty;// 
            int peisongyichangjuli = int.Parse(ConfigurationSettings.AppSettings["peisongyichangjuli"].ToString());
            string Long = string.Empty;//经度
            string Lat = string.Empty;//纬度
            try
            {

                orderid = Request.Params["orderid"];
            }
            catch { }

            try
            {
                Long = Request.Params["Long"];

            }
            catch { }

            try
            {
                Lat = Request.Params["Lat"];

            }
            catch { }

            if (string.IsNullOrEmpty(Long))
            {
                msg = "long经度参数不能为空";
                retrun_ajax(status, msg);
                return;
            }

            if (string.IsNullOrEmpty(Lat))
            {
                msg = "Lat经度参数不能为空";
                retrun_ajax(status, msg);
                return;
            }

            //查询订单
            DataTable dt_Order = my_c.GetTable("select *  from [Order] where id='" + orderid + "' or Number='" + orderid + "' ", "sql_conn2");
            if (dt_Order.Rows.Count > 0)
            {
                //查询配送点
                DataTable dt_peisongdian = my_c.GetTable("select top 1 g.Name,g.Latitude,g.Longitude,g.ID from UserGroupLocation as u , GroupLocation as g where u.UserID='" + dt_Order.Rows[0]["UserID"].ToString() + "' and g.ID= u.GroupLocationID", "sql_conn6");
                //比较配送点和配送员当前位置
                double lat1, lng1;
                double lat2, lng2;
                lat1 = double.Parse(Lat);
                lng1 = double.Parse(Long);
                lat2 = double.Parse(dt_peisongdian.Rows[0]["Latitude"].ToString());
                lng2 = double.Parse(dt_peisongdian.Rows[0]["Longitude"].ToString());
                double temp_distance = this.GetDistance(lat1, lng1, lat2, lng2);
                //Response.Write(temp_distance);
                if (temp_distance > peisongyichangjuli)
                {
                    //写入配送异常数据表
                    string peisongyuan, dingdanhao, caozuoweizhi, zhixianjuli;
                    //查询配送员
                    string sql_service = "select *  from Service where FromValue='" + dt_Order.Rows[0]["id"].ToString() + "' ";
                    DataTable dt_service = my_c.GetTable(sql_service, "sql_conn7");
                    peisongyuan = dt_service.Rows[0]["ServiceID"].ToString();
                    //peisongyuan = this.getUserid();
                    dingdanhao = dt_Order.Rows[0]["id"].ToString();
                    caozuoweizhi = Long + "," + Lat;
                    zhixianjuli = temp_distance.ToString();
                    string sql_peisongyichangrizhi = "insert into  sl_peisongyichangrizhi (peisongyuan,dingdanhao,caozuoweizhi,zhixianjuli) values('" + peisongyuan + "','" + peisongyuan + "','" + caozuoweizhi + "','" + zhixianjuli + "')";
                    my_c.genxin(sql_peisongyichangrizhi);
                    status = "true";
                    msg = "操作成功,以记录当前位置信息" + temp_distance.ToString();
                }
                else
                {
                    status = "true";
                    msg = "操作成功";
                }


            }
            else
            {
                status = "false";
                msg = "订单号不存在";
            } 
            #endregion


        }
        else if (type == "addbeizhu")
        {
            //添加配送点备注数据接口
            /*
             *派单时间
             *用户名
             *
             */
            #region 配送点下的具体一个派单的详细数据接口
            if (is_login())
            {
                string Number = string.Empty;//派单ID 
                string UserID = string.Empty;//当前用户
                string Body = string.Empty;//当前用户

                try
                {

                    Number = Request.Params["number"];
                }
                catch { }

                try
                {

                    UserID = this.getUserid();
                }
                catch { }

                try
                {

                    Body = Request.Params["Body"];
                }
                catch { }


                if (!string.IsNullOrEmpty(Number) && !string.IsNullOrEmpty(Body))
                {
                    
                    //查询当前派单的详情 （配单号；状态；时间；地址 ；用户姓名；用户电话；配送员姓名；配送员电话；）
                    string sql_service = "select ID  from Service where  Number='" + Number + "' ";
                    DataTable dt_service = my_c.GetTable(sql_service, "sql_conn7");
                    //查询下单用户编号
                     

                    string serviceID = "";
                    try
                    {
                        serviceID = dt_service.Rows[0]["ID"].ToString();
                    }
                    catch { }
                    //
                    string sql_User = "select  * from [User] where ID='" + UserID + "' ";  //
                    //Response.Write(sql_User);
                    //Response.End();
                    DataTable dt_User = my_c.GetTable(sql_User, "sql_conn12");
                    //
                    string UserName = "";
                    try
                    {
                        UserName = dt_User.Rows[0]["name"].ToString();
                    }
                    catch { }

                    //备注
                    string time = System.DateTime.Now.ToString();
                    string sql_serviceremarks = "insert into  ServiceRemarks (ID,ServiceID,Body,InTime) values('" + my_b.md5(my_b.get_bianhao()) + "','" + serviceID + "','" + UserName + "：" + Body + "','" + time + "' )";

                    try
                    {
                        my_c.genxin(sql_serviceremarks, "sql_conn7");
                        status = "true";
                        msg = "操作成功";
                    }
                    catch {
                        status = "false";
                        msg = "操作失败：" + sql_serviceremarks;
                    }
                    
                     
                   
                }
                else
                {
                    msg = "派单ID 不能为空";
                }


            }
            else
            {
                msg = "请登录后再操作";
            }
            #endregion



        }
        else if (type == "yonghuweizhirizhi")
        {
            #region 记录没有经纬度客户的信息
            string xingming = string.Empty;// 
            string shoujihao = string.Empty;//
            string yonghuid = string.Empty;//
            string zhuangtai = string.Empty;//
            string bianhao = string.Empty;//
            string kehuID = string.Empty;
            try
            {

                shoujihao = Request.Params["shoujihao"];
            }
            catch { }



            if (string.IsNullOrEmpty(shoujihao))
            {
                msg = "shoujihao客户手机号参数不能为空";
                retrun_ajax(status, msg);
                return;
            }

            string sql_UserDetailed = "select  * from [User] where Phone='" + shoujihao + "'  ";  //
            DataTable dt_UserDetailed = my_c.GetTable(sql_UserDetailed, "sql_conn12");
            //Response.Write(sql_UserDetailed); Response.End();

            if (dt_UserDetailed.Rows.Count > 0)
            {
                kehuID=dt_UserDetailed.Rows[0]["ID"].ToString();

                xingming = dt_UserDetailed.Rows[0]["Name"].ToString();
                shoujihao = dt_UserDetailed.Rows[0]["Phone"].ToString();
                yonghuid = dt_UserDetailed.Rows[0]["ID"].ToString();
                zhuangtai = "未处理";
                bianhao = dt_UserDetailed.Rows[0]["Number"].ToString();

                //插入新的sql_yonghujingweiduyichangjilu记录
                string sql_yonghujingweiduyichangjilu = "INSERT INTO sl_yonghujingweiduyichangjilu (xingming, shoujihao,yonghuid ,zhuangtai,bianhao) VALUES ('" + xingming + "', '" + shoujihao + "','" + yonghuid + "' ,'" + zhuangtai + "','" + bianhao + "'  )";
                my_c.genxin(sql_yonghujingweiduyichangjilu);


                msg = "提交成功";
                status = "true";
            }
            else
            {
                msg = "当前客户的手机号不存在";
                retrun_ajax(status, msg);
                return;
            
            }
             

 
            #endregion


        }
        else
        {
            retrun_ajax(status, msg);
        }

        retrun_ajax(status, msg);
        //完
        #region 跳转
       // tiaozhuan("", "", "");
        #endregion
    }
    #endregion
    #region 判断来源
    public void set_laiyuan()
    {
        if (Request.UrlReferrer.ToString().ToLower().IndexOf(ConfigurationSettings.AppSettings["web_url"].ToString()) == -1)
        {
            tiaozhuan("来源不对", "", "");
        }
    }
    #endregion


    #region 是否已经登录
    public bool is_login()
    {
        return this.isLogin();
        
    } 
    #endregion


    #region 拆分Request.QueryString的值，组成where部分
    public string user_sql(string quer_string)
    {
        string return_string = "";
        if (quer_string != "")
        {

            string[] quer_arr = quer_string.Split('&');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (quer_arr[i].ToString().IndexOf("y=") == -1 && quer_arr[i].ToString().IndexOf("x=") == -1)
                {
                    string[] sql_arr = quer_arr[i].ToString().Split('=');
                    if (HttpUtility.UrlDecode(sql_arr[1]) != "")
                    {

                        if (return_string == "")
                        {
                            return_string = "u1='" + sql_arr[0] + "'";
                        }
                        else
                        {
                            return_string = return_string + " or u1='" + sql_arr[0] + "'";
                        }
                    }
                }
            }
        }

        return return_string;
    }

    public string user_sql1(string quer_string)
    {
        string return_string = "";
        if (quer_string != "")
        {

            string[] quer_arr = quer_string.Split('&');
            for (int i = 0; i < quer_arr.Length; i++)
            {
                if (quer_arr[i].ToString().IndexOf("y=") == -1 && quer_arr[i].ToString().IndexOf("x=") == -1)
                {
                    string[] sql_arr = quer_arr[i].ToString().Split('=');
                    if (return_string == "")
                    {
                        return_string = "u1='" + sql_arr[0] + "'";
                    }
                    else
                    {
                        return_string = return_string + " or u1='" + sql_arr[0] + "'";
                    }
                }
            }
        }

        return return_string;
    }
    #endregion

     
    #region 根据类型获取input值，在返回sql需要的
    public string get_kj(string type, string id)
    {
        if (type == "文本框" || type == "文件框" || type == "缩略图" || type == "文本域" || type == "时间框")
        {
            return "'" + my_b.c_string(Request[id].ToString()) + "'";
        }
        else if (type == "编辑器" || type == "子编辑器")
        {
            string classid = "0";
            try
            {
                classid = Request["classid"].ToString();
            }
            catch { }
            DataTable sort_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id=" + classid + "");
            string pic_width = "800*0";
            if (sort_dt.Rows.Count > 0)
            {
                pic_width = sort_dt.Rows[0]["u10"].ToString();
            }

            return "'" + my_b.c_string(my_b.set_pic_size(Request[id].ToString(), pic_width)) + "'";
        }
        else if (type == "数字")
        {
            return "" + my_b.c_string(Request[id].ToString()) + "";
        }
        else if (type == "密码框")
        {
            return "'" + my_b.md5(my_b.c_string(Request[id].ToString())) + "'";
        }
        else
        {
            return "'" + Request[id].ToString() + "'";
        }
    }

    public string cookie_get_kj(string type, string id)
    {
        if (type == "密码框")
        {
            return "" + my_b.md5(my_b.c_string(Request[id].ToString())) + "";
        }
        else
        {
            return "" + my_b.c_string(Request[id].ToString()) + "";
        }
    }
    #endregion


    /// <summary>
    /// 根据小程序的openid 反查用户的手机号
    /// </summary>
    /// <param name="openid"></param>
    /// <returns></returns>
    public string getPhoneByOpenid(string openid)
    {
        string phone = "";
        string sql = "select * from sl_user where openid='" + openid + "' ";
        DataTable user = new DataTable();
        user = my_c.GetTable(sql);
        if (user.Rows.Count > 0)
        {
            phone = user.Rows[0]["openid"].ToString();
        }

        return phone;
    }

    /// <summary>
    /// 根据小程序的openid 反查用户的手机号
    /// </summary>
    /// <param name="openid"></param>
    /// <returns></returns>
    public string getPhoneByToken(string token)
    {
        string phone = "";
        string sql = "select * from sl_token where token='" + token + "' ";
        DataTable user = new DataTable();
        user = my_c.GetTable(sql);
        if (user.Rows.Count > 0)
        {
            phone = user.Rows[0]["yonghuming"].ToString();
             
        }

        if (!string.IsNullOrEmpty(phone))
        {
            //判断用户是否被禁用
            //查询 用户绑定的配送权限和凭证扫描权限
            string sql_user_temp = "select * from [User] where phone='" + phone + "'  ";
            //Response.Write(sql);
            //Response.End();
            DataTable user_temp = new DataTable();
            user_temp = my_c.GetTable(sql_user_temp, "sql_conn12");
            //用户名存在，则登录成功
            if (user_temp.Rows[0]["state"].ToString()!="0")
            {
                phone = "";
            }

        }
        

        return phone;
    }

    public string getUseridByToken(string token)
    {
        //查询openid
        string yonghuming = "";
        string userid = "";
        string _sql = "select * from sl_token where token='" + token + "' ";
        DataTable _user = new DataTable();
        _user = my_c.GetTable(_sql);
        if (_user.Rows.Count > 0)
        {
           
            yonghuming = _user.Rows[0]["yonghuming"].ToString();
            string sql = "select * from sl_user where phone='" + yonghuming + "' ";
            DataTable user = new DataTable();
            user = my_c.GetTable(sql); 
            if (user.Rows.Count > 0)
            {
                userid = user.Rows[0]["userid"].ToString();
            }
        }
 
        

        return userid;
    }


    /// <summary>
    /// 判断当前openid是否已经存在系统中，如果存在就为登录
    /// </summary>
    /// <returns></returns>
    public bool isLogin()
    {
        string token = string.Empty;

        try
        {
            token = Request.Params["token"];
        }
        catch { }

        if (!string.IsNullOrEmpty(token) && my_b.ProcessSqlStr(token) && !string.IsNullOrEmpty(this.getPhoneByToken(token)))
        {
            return true;
        }
        else
        {
            return false;
        }

        return false;
        
    }

    

    /// <summary>
    /// 得到当前用户的手机号
    /// </summary>
    /// <returns></returns>
    public string getPhone()
    {
         string openid = string.Empty;

        try
        {
            openid = Request.Params["openid"];
        }
        catch { }

        if (!string.IsNullOrEmpty(openid) && my_b.ProcessSqlStr(openid) )
        {
            openid = this.getPhoneByOpenid(openid);
        }
         return openid;
    }


    /// <summary>
    /// 得到当前用户的Userid
    /// </summary>
    /// <returns></returns>
    public string getUserid()
    {
        string token = string.Empty;
        string userid = string.Empty;

        try
        {
            token = Request.Params["token"];
        }
        catch { }

        if (!string.IsNullOrEmpty(token) && my_b.ProcessSqlStr(token))
        { 
            userid = this.getUseridByToken(token); 
        }
        return userid;
    }


    /// <summary>
    /// 接收openid
    /// </summary>
    /// <returns></returns>
    public string getOpenid()
    {
         string code = string.Empty;
         string Openid = "";
        try
        {
            code = Request.Params["code"];
        }
        catch { }

        if (!string.IsNullOrEmpty(code) && my_b.ProcessSqlStr(code))
        { 
            string sql = "select * from sl_openid where code='" + code + "' ";
            DataTable openid = new DataTable();
            openid = my_c.GetTable(sql);
            if (openid.Rows.Count > 0)
            {
                Openid = openid.Rows[0]["openid"].ToString();
            }
            else { 
                string json_temp = GetOpenidByCode(code); 
                JObject jsonObj = JObject.Parse(json_temp);
                string errmsg = "";
                try
                {
                     errmsg = jsonObj["errmsg"].ToString();
                }
                catch { }
                if (string.IsNullOrEmpty(errmsg))
                {
                    Openid = jsonObj["openid"].ToString();
                }
                else {
                    Response.Write(errmsg);
                    Response.End();
                }
            }
           
           
        }
       
        return Openid;
    }


    /// <summary>
    /// 通过临时code获取openID
    /// </summary>
    /// <param name="loginCode">小程序登录返回的code</param>
    /// <param name="model">小程序Model</param>
    /// <returns></returns>
    public static string GetOpenidByCode(string Code)
    {
        my_basic my_b = new my_basic();
        string appid = ConfigurationSettings.AppSettings["wx_xcx_appid"].ToString();
        string secret = ConfigurationSettings.AppSettings["wx_xcx_secret"].ToString();

        string url = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&grant_type=authorization_code&js_code={2}";
        url = string.Format(url, appid, secret, Code);
        string json_str = my_b.getWebFile(url);

        return json_str;
    }

    /// <summary>
    /// 查询用户是否具有配送员权限
    /// </summary>
    /// <param name="peisongyuanPhone"></param>
    /// <returns></returns>
    public bool IsPeisongyuan(string peisongyuanPhone)
    {
        //查询用户是否具有配送员和凭证权限
        /*
         *手机号
         */
        bool status = false;

        #region 是否具有配送员和凭证权限
        string Phone = peisongyuanPhone;

        try
        {
            //Phone = Request.Params["phone"];
        }
        catch { }
        if (!string.IsNullOrEmpty(Phone) && my_b.ProcessSqlStr(Phone))
        {
            string sql = "select ID from [User] where phone='" + Phone + "'  ";
            DataTable user = new DataTable();
            user = my_c.GetTable(sql, "sql_conn12");
            if (user.Rows.Count > 0)
            {
                string UserID = user.Rows[0]["ID"].ToString();
                string ps_UserIDs = string.Empty;//全部配送用户的ID
                 

                try
                {

                    ps_UserIDs = my_b.getWebFile1(ConfigurationSettings.AppSettings["ps_url"].ToString());//全部配送用户的ID
                     
                }
                catch { }

                if (!string.IsNullOrEmpty(ps_UserIDs) )
                {
                    if (ps_UserIDs.IndexOf(UserID) >= 0)
                    {
                       status = true;
                    }
                     
                }
                else
                {
                    status = false;
                   
                }

            }
            else
            {
                status = false;
                
            }
        }
        else
        {
            status = false;
            
        }

        return status;
        #endregion
    
    }



    /// <summary>
    /// 返回接口数据 ghy
    /// </summary>
    /// <param name="status"></param>
    /// <param name="msg"></param>
    public void retrun_ajax(string status="false", string msg="操作失败")
    {
        string[] a_return = { "status", "msg" };
        DataTable dt_return = my_dt.setdt(a_return);

        DataRow return_dr = dt_return.NewRow();
        return_dr["status"] = status;
        return_dr["msg"] = msg;

        //插入返回数据
        dt_return.Rows.Add(return_dr);

        Response.Write(my_json_ghy.DataTableToJsonWithJavaScriptSerializer(dt_return));
        Response.End();
    
    }

     
    /// <summary>
    /// 生成二维码方法 ghy
    /// </summary>
    /// <param name="nr"></param>
    private string CreateCode_Simple(string nr)
    {
        //
        string _url = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/upfile/";
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        qrCodeEncoder.QRCodeScale = 4;
        qrCodeEncoder.QRCodeVersion = 8;
        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        //System.Drawing.Image image = qrCodeEncoder.Encode("4408810820 深圳－广州 小江");
        System.Drawing.Image image = qrCodeEncoder.Encode(nr);
        string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
        string filepath = Server.MapPath(@"~\upfile") + "\\" + filename;
        System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
        image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

        fs.Close();
        image.Dispose();
        //二维码解码
        //var codeDecoder = CodeDecoder(filepath);

        return _url + filename;
    }



    #region 计算两个坐标点之间的距离
    private const double EARTH_RADIUS = 6378.137; //地球半径
    private static double rad(double d)
    {
        return d * Math.PI / 180.0;
    }

    /// <summary>
    /// 计算两个坐标点之间的距离
    /// </summary>
    /// <param name="lng1">第一个点的经度</param>
    /// <param name="lat1">第一个点的纬度</param>
    /// <param name="lng2">第二个点的经度</param>
    /// <param name="lat2">第二个点的纬度</param>
    /// <returns></returns>
    public double GetDistance(double lat1, double lng1, double lat2, double lng2)
    {
        double radLat1 = rad(lat1);
        double radLat2 = rad(lat2);
        double a = radLat1 - radLat2;
        double b = rad(lng1) - rad(lng2);
        double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
         Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
        s = s * EARTH_RADIUS;
        s = Math.Round(s * 10000) / 10000;
        return s*1000;
    }
    #endregion


    /// <summary>
    /// datarows 转 datatable
    /// </summary>
    /// <param name="rows"></param>
    /// <returns></returns>
    private DataTable ToDataTable(DataRow[] rows)
    {
        if (rows == null || rows.Length == 0) return null;
        DataTable tmp = rows[0].Table.Clone(); // 复制DataRow的表结构
        foreach (DataRow row in rows)
        {

            tmp.ImportRow(row); // 将DataRow添加到DataTable中
        }
        return tmp;
    }
    /// <summary>
    /// //PageIndex表示第几页，PageSize表示每页的记录数
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="PageIndex">第几页</param>
    /// <param name="PageSize">每页的记录数</param>
    /// <returns></returns>
    public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
    {
        if (PageIndex == 0)
            return dt;//0页代表每页数据，直接返回
        DataTable newdt = dt.Copy();
        newdt.Clear();//copy dt的框架
        int rowbegin = (PageIndex - 1) * PageSize;
        int rowend = PageIndex * PageSize;
        if (rowbegin >= dt.Rows.Count)
            return newdt;//源数据记录数小于等于要显示的记录，直接返回dt
        if (rowend > dt.Rows.Count)
            rowend = dt.Rows.Count;
        for (int i = rowbegin; i <= rowend - 1; i++)
        {
            DataRow newdr = newdt.NewRow();
            DataRow dr = dt.Rows[i];
            foreach (DataColumn column in dt.Columns)
            {
                newdr[column.ColumnName] = dr[column.ColumnName];
            }
            newdt.Rows.Add(newdr);
        }
        return newdt;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Response.Write(Request.QueryString.ToString());
            //Response.End();
            string type = my_b.set_url_css(Request.QueryString["type"].ToString());
            set_user_sql(type);

            //try {
            //    string type = my_b.set_url_css(Request.QueryString["type"].ToString());
            //    set_user_sql(type);
            //}
            //catch{
            //    Response.Write("{\"status\":\"false\",\"msg\",\"接口查询错误\"}");
            //    Response.End();
            //}

        }
    }
}