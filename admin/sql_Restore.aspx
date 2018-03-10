<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sql_Restore.aspx.cs" Inherits="admin_sql_Restore" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>在线恢复</title>
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
         <uc1:yanz ID="Yanz1" runat="server" />
		<a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a><a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 
         <span class="split ftSplit" ></span>
	    <div id="guide"> 您现在的位置：扩展功能 >> 在线恢复</div>
		
	    <div class="pageInfo right light_gray">[<a href="Repair_data.aspx">数据替换</a>]&nbsp;[<a href="sql_back.aspx">在线备份</a>]</div>
		
    </div>
	<div id="content">
     <table class="cTable table tCenter">
	    <tr class="cTitle toolbarBg" id="Title">
		    <td ><span class="split ctSplit"></span><div>文件名</div></td>
			<td width="20%"><span class="split ctSplit"></span><div>文件生成时间</div></td>
			<td width="10%"><span class="split ctSplit"></span><div>大小</div></td>
			<td width="20%"><span class="split ctSplit"></span><div>操作</div></td>
	    </tr>
	 </table>
	<table class="cTable table tCenter" cellspacing="0" border="0" style="border-collapse:collapse;">
	     <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
		<tr>
		    <td style="text-align:left"><a href="file_manage_view.aspx?file_path=<%# Container.DataItem %>"><%# Container.DataItem %></a></td>
			<td width="20%"><%# getfile(Container.DataItem.ToString())%></a></td>
			<td width="10%"><%# get_size(Container.DataItem.ToString())%></a></td>
			<td width="20%"><a href="sql_Restore.aspx?type=huifu&id=<%=i %>">恢复</a>&nbsp;&nbsp;<a href="sql_Restore.aspx?type=del&id=<%=i %>">删除</td>
		</tr>
		<%
                            i++;
                         %>
		</ItemTemplate>
                 </asp:Repeater>
			
	</table> 
    <div id="cEndToolbar" class="toolbarBg">
        <div class="pageList">
		    <div style="float:left;width:48%;height:20px;text-align:right;">
				
			</div>
		
	    </div>

	</div>	
    </div>
	
</form>
</body>
</html>


