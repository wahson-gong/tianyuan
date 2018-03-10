<%@ Page Language="C#" AutoEventWireup="true" CodeFile="x_article.aspx.cs" Inherits="admin_x_article" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>相关内容管理</title>
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
	    <div id="guide"> 您现在的位置：内容管理 >> 相关内容管理</div>
		
	    <div class="pageInfo right light_gray"><asp:TextBox ID="TextBox1" runat="server" Width="100px"></asp:TextBox>&nbsp;<asp:Button ID="Button1" runat="server" CssClass="btn" OnClick="Button1_Click" Text="查找" /></div>
		
    </div>
	<div id="content">
     <table class="cTable table tCenter">
	    <tr class="cTitle toolbarBg" id="Title">
		    <td width="6%"><span class="checkbox"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span></td>
		    <td width="5%"><span class="split ctSplit"></span><div>编号</div></td>
		    <td><span class="split ctSplit"></span><div>姓名</div></td>

	    </tr>
	 </table>
	<table class="cTable table tCenter" cellspacing="0" border="0" style="border-collapse:collapse;">
	     <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
		<tr>
			<td align="center" style="width:6%;white-space:nowrap;">
                <input type="checkbox" name="chk" value="<%#DataBinder.Eval(Container.DataItem, get_sql(0))%>|<%#DataBinder.Eval(Container.DataItem, get_sql(1)).ToString()%>" <%#set_che(DataBinder.Eval(Container.DataItem, get_sql(0)).ToString())%>/>
			</td>
			<td align="center" style="width:5%;white-space:nowrap;">
             <%#DataBinder.Eval(Container.DataItem, get_sql(0))%>
			</td>
			<td style=" text-align:left"><%#DataBinder.Eval(Container.DataItem, get_sql(1)).ToString()%></td>
	
		</tr>
		</ItemTemplate>
                 </asp:Repeater>

	</table> 
    <div id="cEndToolbar" class="toolbarBg">

	    <div class="pageSelect"><span class="iptSl"><input type="checkbox" name="chkall" onclick="selcheck('content')" /></span><a href="javascript:selcheck('content');"><span class="btnTxt">选择</span><span class="btnIco btnDown"></span></a></div>
	    <span class="split cpSplit" ></span>
	    <div class="pageBtnDo">
	        <div class="icoBtn_do"><input type="image" name="Delete_Button" id="Delete_Button" class="btnIco btnChange" src="images/Empty.gif" onclick="chk1()" style="border-width:0px;" /></div>
	    </div>
	</div>	
    </div>
	<script type="text/javascript">
	

function chk1()
{
    var boxes = document.getElementsByName("chk"); 
    var t2=0;
    var t1 = "";
    var t3 = "";
    for (var i = 0; i < boxes.length; i++)   
    {
     if (boxes[i].checked)   
     {
        t2=t2+1;
        if(t2==1)
        {
            t1 = t1 + boxes[i].value.split("|")[0];
            t3 = t3 + boxes[i].value.split("|")[1];
        }
        else
        {
            t1 = t1 + "," + boxes[i].value.split("|")[0];
            t3 = t3 + "," + boxes[i].value.split("|")[1];
        }
     }
}

window.opener.document.all('<%=editname %>').value = "0," + t1 + ",0";
    window.opener.document.all('<%=editname %>_').value = t3;
    window.opener.changpin();
    window.close(); 

}
        </script> 
</form>

</body>
</html>


