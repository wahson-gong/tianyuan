<%@ Page Language="C#" AutoEventWireup="true" CodeFile="up_table.aspx.cs" Inherits="admin_up_table" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>在线更新管理</title>
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
	    <div id="guide"> 您现在的位置：扩展功能 >> 在线更新管理</div>
		
	    <div class="pageInfo right light_gray"> <a href='up_table_add.aspx'>添加更新</a>&nbsp;&nbsp;<asp:TextBox ID="TextBox1" runat="server" Width="100px"></asp:TextBox>&nbsp;<asp:Button ID="Button1" runat="server" CssClass="btn" OnClick="Button1_Click" Text="查找" /></div>
		
    </div>
	<div id="content">
     <table class="cTable table tCenter">
	    <tr class="cTitle toolbarBg" id="Title">
		    <td width="6%"><span class="checkbox"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span></td>
		    <td width="5%"><span class="split ctSplit"></span><div>编号</div></td>
		    <td width="20%"><span class="split ctSplit"></span><div>更新路径</div></td>
		    <td><span class="split ctSplit"></span><div>功能说明</div></td>
			<td width="15%"><span class="split ctSplit"></span><div>时间</div></td>
			<td width="15%"><span class="split ctSplit"></span><div>操作</div></td>
	    </tr>
	 </table>
	<table class="cTable table tCenter" cellspacing="0" border="0" style="border-collapse:collapse;">
	     <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
		<tr>
			<td align="center" style="width:6%;white-space:nowrap;">
                <input type="checkbox" name="chk" value="<%#DataBinder.Eval(Container.DataItem, "id")%>" />
			</td>
			<td align="center" style="width:5%;white-space:nowrap;">
             <%#DataBinder.Eval(Container.DataItem, "id")%>
			</td>
			<td style="width:20%;"><a href="up_table_add.aspx?id=<%#DataBinder.Eval(Container.DataItem, "id")%>&type=edit"><%#DataBinder.Eval(Container.DataItem, "up_path").ToString()%></a></td>
			<td style=" text-align:left"><%#DataBinder.Eval(Container.DataItem, "contents").ToString()%></td>
			<td style="width:15%;"><%#DataBinder.Eval(Container.DataItem, "dtime").ToString()%></td>
			<td style="width:15%;"><a  href="up_table_add.aspx?type=edit&id=<%#DataBinder.Eval(Container.DataItem, "id")%>">修改</a> | <a onclick="return confirm('你确认将该频道删除到回收站?\r\n注意：相关栏目和内容也将一起删除到回收站');" id="repChannel_ctl01_btnDelete" href="up_table_add.aspx?type=del&id=<%#DataBinder.Eval(Container.DataItem, "id")%>">删除</a></td>
		</tr>
		</ItemTemplate>
                 </asp:Repeater>

	</table> 
    <div id="cEndToolbar" class="toolbarBg">
        <div class="pageList">
		<asp:Literal ID="Literal1" runat="server"></asp:Literal>
		
	    </div>
	    <div class="pageSelect"><span class="iptSl"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span><a href="javascript:selcheck('content');"><span class="btnTxt">选择</span><span class="btnIco btnDown"></span></a></div>
	    <span class="split cpSplit" ></span>
	    <div class="pageBtnDo">
	        <div class="icoBtn_do"><input type="image" name="Delete_Button" id="Delete_Button" class="btnIco btnDel" src="images/Empty.gif" onclick="chk1()" style="border-width:0px;" /></div>
	    </div>
	</div>	
    </div>
	<script type="text/javascript">
function chk1()
{
    var boxes = document.getElementsByName("chk"); 
    var t2=0;
    var t1="";
    for (var i = 0; i < boxes.length; i++)   
    {
     if (boxes[i].checked)   
     {
        t2=t2+1;
        if(t2==1)
        {
        t1=t1+boxes[i].value;
        }
        else
        {
        t1=t1+","+boxes[i].value;
        }
     }
    }
    window.location="up_table_add.aspx?type=del&id="+t1;

}
        </script> 
</form>
</body>
</html>
