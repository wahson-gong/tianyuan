<%@ Page Language="C#" AutoEventWireup="true" CodeFile="template.aspx.cs" Inherits="admin_template" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>模板管理</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
	    <div id="guide"> 您现在的位置：系统设置 >> 模板管理</div>
		
	    <div class="pageInfo right light_gray">通过FTP等方式把模板文件夹上传到网站根目录中的template文件夹下。</div>
		
    </div>
	<div id="content">
        
     
	<table class="cTable table tCenter" cellspacing="0" border="0" style="border-collapse:collapse;">
	    <tr>
	        <td>
	              <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
	            <div style="width:330px; float:left; padding:10px 10px 10px 10px">
	                 <table border="0" cellpadding="0" cellspacing="0" style="width: 310px">
                    <tr>
                        <td style="">
                        <a href="template.aspx?type=edit&file_path=<%# Container.DataItem%>"><img src="<%# Container.DataItem%>\default.jpg"  width="300" height="231" onerror="src='images/nopic.jpg'"  style="<%# set_border(Getfile_name(Container.DataItem.ToString()))%>"/></a>
                        </td>
                    </tr>
                    <tr>
                        <td style="">
                        <a href="template.aspx?type=edit&file_path=<%# Container.DataItem%>"><%# Getfile_name(Container.DataItem.ToString())%></a>&nbsp;&nbsp;<a onclick="return confirm('你确认将该模板删除到回收站?\r\n注意：删除后该模板所有文件不可以恢复。');" id="repChannel_ctl01_btnDelete" href="template.aspx?type=del&file_path=<%# Container.DataItem%>">DEL</a>
                        </td>
                    </tr>
                </table>
	            </div>
	            </ItemTemplate>
	            </asp:Repeater>
               
	        </td>
	    </tr>
	</table> 

    </div>

</form>

</body>
</html>


