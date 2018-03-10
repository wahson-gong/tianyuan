using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
/// <summary>
///my_conn 的摘要说明
/// </summary>
public class other_sql
{
    public string Getconn()
    {
        return ConfigurationSettings.AppSettings["othersql"].ToString();
    }
    //查询
    public DataTable GetTable(string g1)
    {
        try
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet dset = new DataSet();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Getconn();
            con.Open();
            SqlCommand cmd;
            cmd = new SqlCommand(g1, con);
            adapter.SelectCommand = cmd;
            dset.Tables.Add("xuesheng");
            adapter.Fill(dset, "xuesheng");
            con.Close();
            return dset.Tables["xuesheng"];
        }
        catch
        {
            HttpContext.Current.Response.Write(g1);
            HttpContext.Current.Response.End();
            DataSet dset = new DataSet();
            return dset.Tables["xuesheng"];
        }
    }

    //更新
    public void genxin(string g1)
    {
        //try
        //{
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Getconn();
            con.Open();
            SqlCommand cmd;
            cmd = new SqlCommand(g1, con);
            cmd.ExecuteNonQuery();
            con.Close();
        //}
        //catch
        //{
        //    HttpContext.Current.Response.Write(g1);
        //    HttpContext.Current.Response.End();
        //}
    }
    //通过手机号获取用户DM
    public string get_dm(string shouji)
    {
        DataTable V_CUSTOMER = GetTable("select * from V_CUSTOMER where SJ='" + shouji + "'");
        if (V_CUSTOMER.Rows.Count > 0)
        {
            return V_CUSTOMER.Rows[0]["DM"].ToString();
        }
        else
        {
            return "";
        }
    }

    //获取积分
    public string get_jifen(string shouji)
    {
        string dm = get_dm(shouji);
        DataTable V_VIPSET = GetTable("select * from V_VIPSET where GKDM='" + dm + "'");
        if (V_VIPSET.Rows.Count > 0)
        {
            if (V_VIPSET.Rows[0]["DQJF"].ToString() == "")
            {
                return "0";
            }
            else
            {
                return V_VIPSET.Rows[0]["DQJF"].ToString();
            }
        }
        return "";
    }
    //插入新会员
    public void adduser(string shouji)
    {
        string V_CUSTOMER = "insert into V_CUSTOMER(DM,QYDM,QDDM,CKDM,JDRQ,SJ,Hasvip) values('GK" + shouji + "','00005','000','8888','" + DateTime.Now.ToString() + "','" + shouji + "','" + shouji + "')";

        string DM = shouji;
        string MC = "";
        string ZJF = "";
        string GKDM = "GK" + shouji + "";
        string XLDM = "001";
        string KLDM = "004";
        string LBDM = "0";
        string QDDM = "000";
        string QYDM = "00005";
        string CKDM = "8888";
        string BZ = "";
        string QYBJ = "1";
        string TYBJ = "0";
        string QYRQ = DateTime.Now.ToString();
        string TYRQ = DateTime.Now.AddYears(10).ToString();
        string JDRQ = DateTime.Now.ToString();
        string XGRQ = "";
        string ZK = "1";//
        string XFJE = "0";//
        string XFSL = "0";//
        string TZBJ = "0";
        string TZJE = "0";//
        string DQJF = "0";
        string BYZD1 = "";
        string BYZD2 = "0";
        string BYZD3 = "";
        string BYZD4 = "";
        string BYZD5 = "888801";
        string BYZD6 = "888801";
        string BYZD7 = "";
        string BYZD8 = "0";//
        string BYZD9 = "50";//
        string BYZD10 = "0";//
        string BYZD11 = "0";//
        string BYZD12 = "0";//
        string BYZD13 = "0";//
        string BYZD14 = "";
        string BYZD15 = "";
        string ZK2 = "1";//
        string ZK_LIMIT = "1";//
        string Version = "";
        string Fkxfje = "0";//
        string Fkr = "";
        string Byzd16 = "";
        string Byzd17 = "";
        string Byzd18 = "0";
        string Byzd19 = "0";
        string Byzd20 = "0";//
        string Byzd21 = "0";//
        string LJTZJE = "0";//
        string TYYY = "";

        string V_VIPSET = "insert into V_VIPSET(DM,MC,ZJF,GKDM,XLDM,KLDM,LBDM,QDDM,QYDM,CKDM,BZ,QYBJ,TYBJ,QYRQ,TYRQ,JDRQ,XGRQ,ZK,XFJE,XFSL,TZBJ,TZJE,DQJF,BYZD1,BYZD2,BYZD3,BYZD4,BYZD5,BYZD6,BYZD7,BYZD8,BYZD9,BYZD10,BYZD11,BYZD12,BYZD13,BYZD14,BYZD15,Byzd16,Byzd17,Byzd18,Byzd19,Byzd20,Byzd21,ZK2,ZK_LIMIT,Version,Fkxfje,Fkr,LJTZJE,TYYY) values('" + DM + "','" + MC + "','" + ZJF + "','" + GKDM + "','" + XLDM + "','" + KLDM + "','" + LBDM + "','" + QDDM + "','" + QYDM + "','" + CKDM + "','" + BZ + "','" + QYBJ + "','" + TYBJ + "','" + QYRQ + "','" + TYRQ + "','" + JDRQ + "','" + XGRQ + "'," + ZK + "," + XFJE + "," + XFSL + ",'" + TZBJ + "'," + TZJE + ",'" + DQJF + "','" + BYZD1 + "','" + BYZD2 + "','" + BYZD3 + "','" + BYZD4 + "','" + BYZD5 + "','" + BYZD6 + "','" + BYZD7 + "'," + BYZD8 + "," + BYZD9 + "," + BYZD10 + "," + BYZD11 + "," + BYZD12 + "," + BYZD13 + ",'" + BYZD14 + "','" + BYZD15 + "','" + Byzd16 + "','" + Byzd17 + "','" + Byzd18 + "','" + Byzd19 + "'," + Byzd20 + "," + Byzd21 + "," + ZK2 + "," + ZK_LIMIT + ",'" + Version + "'," + Fkxfje + ",'" + Fkr + "'," + LJTZJE + ",'" + TYYY + "')";

        genxin(V_CUSTOMER);
       genxin(V_VIPSET);
        
    }
    //修改会员资料
    public void set_CUSTOMER(string shouji, string SEX, string GKMC, string SR, string DZ, string MARRY, string EDUCATION, string OCCUPATION, string Email)
    {
        genxin("update V_CUSTOMER set SEX='" + SEX + "',GKMC='" + GKMC + "',SR='" + SR + "',DZ='" + DZ + "',MARRY='" + MARRY + "',EDUCATION='" + EDUCATION + "',Email='" + Email + "' where SJ='" + shouji + "'");

      //  genxin("update V_CUSTOMER set SEX='" + SEX + "',GKMC='" + GKMC + "',SR='" + SR + "',DZ='" + DZ + "',MARRY='" + MARRY + "',EDUCATION='" + EDUCATION + "',OCCUPATION='" + OCCUPATION + "' where SJ='" + shouji + "'");
    }
    //加分
    public void jifen(string shoujihao, int jifen, string type,float jine)
    {
        string GKDM = "";
        string dm = "";
        DataTable V_CUSTOMER = GetTable("select * from V_CUSTOMER where SJ='" + shoujihao + "'");
        GKDM = V_CUSTOMER.Rows[0]["DM"].ToString();
        //HttpContext.Current.Response.Write(GKDM);
        //HttpContext.Current.Response.End();
        DataTable V_VIPSET = GetTable("select * from V_VIPSET where GKDM='" + GKDM + "'");
        dm = V_VIPSET.Rows[0]["dm"].ToString();
 
        int CUSTOMER_DQJF = int.Parse(V_CUSTOMER.Rows[0]["DQJF"].ToString()) + jifen;
        int VIPSET_DQJF = int.Parse(V_VIPSET.Rows[0]["DQJF"].ToString()) + jifen;


        float CUSTOMER_XFJE = float.Parse(V_CUSTOMER.Rows[0]["XFJE"].ToString()) + jine;
        float VIPSET_XFJE = float.Parse(V_VIPSET.Rows[0]["XFJE"].ToString()) + jine;


        genxin("update V_CUSTOMER set DQJF=" + CUSTOMER_DQJF + ",XFJE=" + CUSTOMER_XFJE + " where DM='" + V_CUSTOMER.Rows[0]["DM"].ToString() + "'");
        genxin("update V_VIPSET set DQJF=" + VIPSET_DQJF + ",XFJE=" + VIPSET_XFJE + " where DM='" + V_VIPSET.Rows[0]["DM"].ToString() + "'");


        DateTime dy = DateTime.Now;
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(999)) + 100;
        string DJBH = "TZD8888" + dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString() + Num1.ToString();
        string BZ = "";
        if (type == "jia")
        {
            BZ = "微信商城消费增加";
        }
        else
        {
            BZ = "微信商城兑换扣减";
        }
        string JFTZD = "insert into JFTZD(DJBH,RQ,TZLX,TZJF,JZR,ZDR,RQ_4,BZ,BYZD3) values('" + DJBH + "','" + dy.ToString() + "',0," + jifen + ",'系统管理员','系统管理员','" + dy.ToString() + "','" + BZ + "','" + dy.ToString() + "')";



        int MXBH = Convert.ToInt32(r.Next(99999)) + 10000;
        string JFTZDMX = "insert into JFTZDMX(DJBH,MIBH,VPDM,DQJF,TZJF,LJJF,BZ) values('" + DJBH + "',1,'" + dm + "'," + V_VIPSET.Rows[0]["DQJF"].ToString() + "," + jifen + "," + VIPSET_DQJF + ",'微信商城')";

        HttpContext.Current.Response.Write(JFTZD + "<br>");
        HttpContext.Current.Response.Write(JFTZDMX);

        genxin(JFTZD);

        genxin(JFTZDMX);

    }
    //减分
}
