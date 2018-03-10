<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Repair_data.aspx.cs" Inherits="admin_Repair_data" ValidateRequest="false" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>数据库备份</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
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
		<div style="float:left">
            <uc1:yanz ID="Yanz1" runat="server" />
            <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a><a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a>&nbsp;
         <span class="split ftSplit" ></span></div>
	    <div id="guide"> 您现在的位置：扩展功能 >> 数据替换</div>
		
	    <div class="pageInfo right light_gray">[<a href="sql_back.aspx">数据库备份</a>]&nbsp;[<a href="sql_Restore.aspx">在线恢复</a>]</div>
		
    </div>
	<div id="content">
     <table class="cTable table tCenter">
	    <tr class="cTitle toolbarBg" id="Title">
		    <td width="6%"><span class="checkbox"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span></td>
		    <td width="8%"><span class="split ctSplit"></span><div>编号</div></td>
		    <td><span class="split ctSplit"></span><div>文件名</div></td>
		    <td width="20%"><span class="split ctSplit"></span><div>新建表时间</div></td>
			<td width="20%"><span class="split ctSplit"></span><div>最后一次操作时间</div></td>
			<td width="20%"><span class="split ctSplit"></span><div>操作</div></td>
	    </tr>
	 </table>
	<table class="cTable table tCenter" cellspacing="0" border="0" style="border-collapse:collapse;">
	     <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
		<tr>
			<td align="center" style="width:6%;white-space:nowrap;">
                <input type="checkbox" name="chk" value=" <%#DataBinder.Eval(Container.DataItem, "id")%>" />
			</td>
			<td align="center" style="width:8%;white-space:nowrap;">
                <%#DataBinder.Eval(Container.DataItem, "id")%>
			</td>
			<td align="left" style="white-space:nowrap;"><%#DataBinder.Eval(Container.DataItem, "name")%>
			</td>
			<td style="width:20%;"><%#DataBinder.Eval(Container.DataItem, "crdate").ToString()%></td>
			<td style="width:20%;"><%#DataBinder.Eval(Container.DataItem, "refdate")%></td>
			<td style="width:20%;"><%--<a href="add_article.aspx?type=edit&id=<%#DataBinder.Eval(Container.DataItem, "id")%>">编辑</a> | <a href="add_article.aspx?type=edit&id=<%#DataBinder.Eval(Container.DataItem, "id")%>">预览 </a>--%></td>
		</tr>
		</ItemTemplate>
                 </asp:Repeater>
	  <asp:Repeater ID="Repeater2" runat="server">
                 <ItemTemplate>
		<tr>
			<td align="center" style="width:6%;white-space:nowrap;">
                <input type="checkbox" name="chk" value="<%=access_i %>" />
			</td>
			<td align="center" style="width:8%;white-space:nowrap;">
                <%=access_i %>
			</td>
			<td align="left" style="white-space:nowrap;"><%#DataBinder.Eval(Container.DataItem, "TABLE_NAME")%>
			</td>
			<td style="width:20%;"><%#DataBinder.Eval(Container.DataItem, "DATE_CREATED").ToString()%></td>
			<td style="width:20%;"><%#DataBinder.Eval(Container.DataItem, "DATE_MODIFIED")%></td>
			<td style="width:20%;"><%--<a href="#">编辑</a> | <a href="#">预览 </a>--%></td>
		</tr>
		<%
		access_i++;
		 %>
		</ItemTemplate>
                 </asp:Repeater>
			<tr>
		<td colspan="5" style="text-align:left">
            <asp:TextBox ID="TextBox1" runat="server" Height="80px" TextMode="MultiLine" Width="500px"></asp:TextBox>
            &nbsp;<asp:Button ID="Button1" runat="server" CssClass="button" Text="替换数据" OnClick="Button1_Click1" Width="160px" />
            </td>
		</tr>	
	</table> 
    <div id="cEndToolbar" class="toolbarBg">
        <div class="pageList">
		    <div style="float:left;width:48%;height:20px;text-align:right;">
				
			</div>
		
	    </div>
	    <div class="pageSelect"><span class="iptSl"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span><a href="javascript:selcheck('content');"><span class="btnTxt">选择</span><span class="btnIco btnDown"></span></a></div>
	    <span class="split cpSplit" ></span>
	    <div class="pageBtnDo">
	        <div class="icoBtn_do"></div>
	    </div>
	</div>	
    </div>
	
</form>
</body>
</html>


