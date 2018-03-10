<%@ Page Language="C#" AutoEventWireup="true" CodeFile="file_manage_main.aspx.cs" Inherits="admin_file_manage_main" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>WEBFTP</title>
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
		<a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a><a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 
         <uc1:yanz ID="Yanz1" runat="server" />
         <span class="split ftSplit" ></span>
	    <div id="guide"> 您现在的位置：扩展功能 >> WEBFTP</div>
		
	    <div class="pageInfo right light_gray"><a href="file_manage_main.aspx?file_path=<%=histroy_file_path %>">返回上一级目录</a></div>
		
    </div>
	<div id="content">
     <table class="cTable table tCenter">
	    <tr class="cTitle toolbarBg" id="Title">
		    <td width="6%"><span class="checkbox"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span></td>
		    <td width="8%"><span class="split ctSplit"></span><div>编号</div></td>
		    <td><span class="split ctSplit"></span><div>文件名</div></td>
		    <td width="20%"><span class="split ctSplit"></span><div>文件大小</div></td>
			<td width="20%"><span class="split ctSplit"></span><div>文件生成时间</div></td>
			<td width="20%"><span class="split ctSplit"></span><div>操作</div></td>
	    </tr>
	 </table>
	<table class="cTable table tCenter" cellspacing="0" border="0" style="border-collapse:collapse;">
	     <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
		<tr>
			<td align="center" style="width:6%;white-space:nowrap;">
                <input type="checkbox" name="chk" value="<%=file_int %>" />
			</td>
			<td align="center" style="width:8%;white-space:nowrap;">
              <%=file_int %>
			</td>
			<td align="left" style="white-space:nowrap;"> <%# GetDirectorytyp(Container.DataItem.ToString())%>
			</td>
			<td style="width:20%;"><%# GetDirectoryLength(Container.DataItem.ToString())%> KB</td>
			<td style="width:20%;"><%# getfile(Container.DataItem.ToString())%></td>
			<td style="width:20%;"><a href="file_manage_main.aspx">查看</a>&nbsp;|&nbsp;<a href="file_manage_main.aspx?type=del&file_path=<%# Container.DataItem%>"  onclick="return confirm('你确认将该频道删除到回收站?\r\n注意：相关栏目和内容也将一起删除到回收站');">删除</a>&nbsp;|&nbsp;<a href="file_manage_main.aspx?type=yasuo&file_path=<%# Container.DataItem%>" title="只可以压缩图片">压缩图片</a></td>
		</tr>
		<%
            file_int++;
		 %>
		</ItemTemplate>
                 </asp:Repeater>

	</table> 
    <div id="cEndToolbar" class="toolbarBg">
        <div class="pageList">
		    <div style="float:left;width:48%;height:20px;text-align:right;">
				
			</div>
		
	    </div>
	    <div class="pageSelect"><span class="iptSl"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span><a href="javascript:selcheck('content');"><span class="btnTxt">全选</span><span class="btnIco btnDown"></span></a></div>
	    <span class="split cpSplit" ></span>
	    <div class="pageBtnDo">
	        <div class="icoBtn_do"><%--<asp:Button ID="Button1" runat="server" CssClass="button" Text="在线备份" OnClick="Button1_Click1" />--%></div>
	    </div>
	</div>	
    </div>
	
</form>
</body>
</html>


