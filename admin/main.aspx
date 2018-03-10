<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="admin_main" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>管理首页</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
body {
	font-size: 12px;
	font-family: "宋体";
	color:#333333;
}
ul,ol,li,img,div,p{ margin:0px; padding:0px; border:0px;}
a {color:#333333; text-decoration:none}
a:hover{ color:#ff0000;text-decoration: none;}

a.dh1 {color:#1B85C6; text-decoration:none}
a.dh1:hover{ color:#ff0000;text-decoration: none;}

#kk { width:1025px; height:auto}
#kkz { width:735px; height:auto; float:left}
#kky { width:282px; height:auto; float:right}

.kkzk { width:100%; border:1px solid #A2B2C1; height:auto; overflow:hidden;}
.kkzt { background-images:url(../images/bj.jpg); width:100%; height:29px}
.kkzb { float:left; padding:6px 0px 0px 10px}
.kkzw { float:left; padding:8px 0px 0px 10px}
.kkzw2 { float:right; padding:8px 10px 0px 0px}
.kkzk2 { background-color:#F6F9FE; height:auto; overflow:hidden; width:100%}
.kkzk3 { width:715px; height:auto; overflow:hidden; padding:10px; background-color:#F6F9FE; margin-top:10px; border:1px solid #A2B2C1}
.kkyk { padding:10px 0px 0px 20px; margin-bottom:10px}
.kkyk ul { list-style:none;}
.kkyk ul li{ line-height:22px}
.kkyk2 { background-color:#EAF1F7; width:246px; height:auto; overflow:hidden; margin-left:auto; margin-right:auto; margin-bottom:20px; padding:10px}

</style>
</head>
<body>
<form id="form1" runat="server">
<script language="javascript" type="text/javascript"> 
function selcheck(id) {
   var e =document.getElementById(id);
   var objs = e.getElementsByTagName("input");
   if (e.checked){
      e.checked = false;
      for(var i=0; i<objs.length; i++) {
         if(objs[i].type.toLowerCase() == "checkbox" )
         objs[i].checked = false;
      }
   }   else   {
      e.checked = true;
      for(var i=0; i<objs.length; i++) {
         if(objs[i].type.toLowerCase() == "checkbox" )
         objs[i].checked = true;
      }
   }
}
</script>

     <div id="frame_Toolbar" class="toolbarBg">
         <uc1:yanz ID="Yanz1" runat="server" />
         <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 

         <span class="split ftSplit" ></span>
	    <div id="guide"> 您现在的位置：管理首页</div>
		
	    <div class="pageInfo right light_gray"></div>
		
    </div>
	<div id="content">
     <table cellpadding="0" cellspacing="0" style="width:100%">
	    <tr>
		    <td style="text-align:left; padding-left:20px; height:60px"><span style="font-weight:700; font-size:14px; color:#1c85c7">上午好，<%=ConfigurationSettings.AppSettings["gs_name"].ToString()%>管理员：<%=my_b.k_cookie("admin_id") %></span></td>
	    </tr>
	 </table>
     <table cellpadding="0" cellspacing="0" style="width:100%">
	    <tr>
		    <td style="padding-left:5px"><div id="kk">
  <div id="kkz">
    <div class="kkzk">
	  <div class="kkzt">
	    <div class="kkzb"><img src="images/tb1.jpg" /></div>
		<div class="kkzw">在线更新</div>
		<div class="kkzw2">>> 
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">马上更新</asp:LinkButton></div>
	  </div>
	  <div class="kkzk2">
<table width="100%" border="0" cellspacing="0" cellpadding="8" style="margin:5px 0px 5px 0px;">
<asp:Repeater ID="Repeater1" runat="server">
  <ItemTemplate>
      <tr>
        <td align="right" bgcolor="#EAF1F7"><img src="images/tb10.jpg" width="4" height="7" /></td>
        <td bgcolor="#EAF1F7"><%#DataBinder.Eval(Container.DataItem, "contents").ToString()%></td>
        <td align="center" bgcolor="#EAF1F7"><%#DataBinder.Eval(Container.DataItem, "dtime", "{0:yyyy-MM-dd}")%></td>
      </tr>
  </ItemTemplate>
  </asp:Repeater>
</table>
	  </div>
	</div>
	
	<div class="kkzk" style="margin-top:10px">
	  <div class="kkzt">
	    <div class="kkzb"><img src="images/tb2.jpg" /></div>
		<div class="kkzw">联系我们</div>
	  </div>
	  <div class="kkzk2">
<table width="100%" border="0" cellspacing="0" cellpadding="8" style="margin:5px 0px 5px 0px;">
  <tr>
    <td width="6%" align="right" bgcolor="#EAF1F7"></td>
    <td width="51%" bgcolor="#EAF1F7">产品开发：思乐科技</td>
    <td width="43%" align="center" bgcolor="#EAF1F7">&nbsp;</td>
  </tr>
  <tr>
    <td align="right"></td>
    <td>地　　址：成都市武候区洗面桥30号交投大厦B栋13楼D座</td>
    <td align="center">&nbsp;</td>
  </tr>
  <tr>
    <td align="right" bgcolor="#EAF1F7"></td>
    <td bgcolor="#EAF1F7">邮　　编：610041</td>
    <td bgcolor="#EAF1F7">传　　真：028-85530019</td>
  </tr>
  <tr>
    <td align="right"></td>
    <td>总机电话：028-85530019 </td>
    <td>客服电话：028-85530019  15982851365</td>
  </tr>
  <tr>
    <td align="right" bgcolor="#EAF1F7"></td>
    <td bgcolor="#EAF1F7">官方网站：<a href="http://www..com" target=_blank>http://www..com</a></td>
    <td bgcolor="#EAF1F7">官方论坛：<a href="http://bbs..com" target=_blank>http://bbs..com</a></td>
  </tr>
</table>
	  </div>
	</div>
	<div class="kkzk3"><img src="images/tb3.jpg" /> 技术开发团队：网事如风   Admin㊣   淘金者   老人海口 大师熊</div>
  </div>
  <div id="kky">
    <div class="kkzk">
      <div class="kkzt">
	    <div class="kkzb"><img src="images/tb4.jpg" /></div>
	    <div class="kkzw">网站探针</div>
	  </div>
      <div class="kkzk2">
	    <div class="kkyk">
		  <ul>
		  <li>IP地址：<asp:Literal ID="Literal1" runat="server"></asp:Literal>
          </li>
		  <li>域名：<asp:Literal ID="Literal2" runat="server"></asp:Literal>
          </li>
		  <li>端口：<asp:Literal ID="Literal3" runat="server"></asp:Literal>
          </li>
		  <li>本文件所在路径：<asp:Literal ID="Literal4" runat="server"></asp:Literal>
          </li>
		  <li>操作系统：<asp:Literal ID="Literal5" runat="server"></asp:Literal>
          </li>
		  <li>.NET Framework 版本：<asp:Literal ID="Literal6" runat="server"></asp:Literal>
          </li>
		  <li>脚本超时时间：<asp:Literal ID="Literal7" runat="server"></asp:Literal> 秒
          </li>
		  <li>启动到现在已运行： <asp:Literal ID="Literal8" runat="server"></asp:Literal> 分钟
          </li>
          <li>CPU 数量： <asp:Literal ID="Literal9" runat="server"></asp:Literal>
          </li>
          <li>CPU类型： <asp:Literal ID="Literal10" runat="server"></asp:Literal>
          </li>
          <li>ASP.NET所站内存： <asp:Literal ID="Literal11" runat="server"></asp:Literal> M
          </li>
          <li>ASP.NET所占CPU： <asp:Literal ID="Literal12" runat="server"></asp:Literal> %
          </li>
         
		  </ul>
		</div>
		<div class="kkyk2">
<table width="100%" border="0" cellspacing="0" cellpadding="5">
  <tr>
    <td align="center"><img src="images/tb5.jpg" width="23" height="21" /></td>
    <td align="center"><a href="http://www..com" target="_blank" class="dh1">查看维护记录</a></td>
    <td align="center"><img src="images/tb8.jpg" width="23" height="21" /></td>
    <td align="center"><a href="http://www..com" target="_blank" class="dh1">获取技术支持</a></td>
  </tr>
  <tr>
    <td align="center"><img src="images/tb6.jpg" width="24" height="20" /></td>
    <td align="center"><a href="http://www..com" target="_blank" class="dh1">查看优化排名</a></td>
    <td align="center"><img src="images/tb9.jpg" width="24" height="21" /></td>
    <td align="center"><a href="http://www..com" target="_blank" class="dh1">其他增值服务</a></td>
  </tr>
  <tr>
    <td align="center"><img src="images/tb7.jpg" width="22" height="21" /></td>
    <td align="center"><a href="http://www..com" target="_blank" class="dh1">意见与建议</a></td>
    <td align="center"><img src="images/tb6.gif" width="22" height="21" /></td>
    <td align="center"><a href="http://www..com" target="_blank" class="dh1">网站模板交流</a></td>
  </tr>
</table>
		</div>
	  
	  </div>
	</div>
  </div>

</div></td>
	    </tr>
	 </table>
 </div>
</form>
</body>
</html>


