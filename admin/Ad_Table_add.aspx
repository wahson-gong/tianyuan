<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ad_Table_add.aspx.cs" Inherits="admin_Ad_Table_add"  ValidateRequest="false"%>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<%@ Register Src="ascx/FreeTextBox.ascx" TagName="FreeTextBox" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>添加广告</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
         function change(){
 
 	if(document.getElementById("TextBox1").value=="")
	{
		alert("广告名不可以为空");
		document.getElementById("TextBox1").focus();
		return false;
	}
	if(document.getElementById("TextBox2").value=="")
	{
		alert("广告介绍不可以为空");
		document.getElementById("TextBox2").focus();
		return false;
	}
  }
function on_page()
{
alert(document.body.scrollHeight);
parent.document.getElementById("main_data").style.height = document.body.scrollHeight; 
}
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg"> 
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 
        <span class="split ftSplit" ></span>
	<div id="guide" class="light_orange">您现在的位置：扩展功能 >> <a href="Ad_Table.aspx"  style="color:#000">广告管理 </a> >>添加广告</div>
	<div class="pageInfo right light_gray"></div>
    </div>
<span id="p1">
    <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>基本信息</div></td>
		<td colspan="2"></td>
	</tr>
	<tr>
		<td class="tRight">广告名：</td>
		<td colspan="2"><asp:TextBox ID="TextBox1" runat="server" CssClass="txtwidth" Enabled="False"></asp:TextBox><font color="red">*请用英文加数字命名</font>
            </td>
	</tr>
	<tr>
		<td class="tRight">广告介绍：</td>
		<td colspan="2"><asp:TextBox ID="TextBox2" runat="server" CssClass="txtwidth"></asp:TextBox>
            </td>
	</tr>
	<tr>
		<td  colspan="3">广告内容：</td>
	</tr>
	  <tr>
        <td colspan="3" ><uc2:FreeTextBox ID="FreeTextBox1" runat="server" Height="400" upfile="admin" Width="95%" /></td>
        </tr>	
	<tr>
		<td class="tRight">添加时间：</td>
		<td colspan="2"><asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
            </td>
	</tr>
	<tr>
		<td class="tRight">结束时间：</td>
		<td colspan="2"><asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
            </td>
	</tr>
	<tr>
		<td class="tRight">
            结束内容：</td>
		<td colspan="2">
           <asp:TextBox ID="TextBox7" runat="server" Height="80px" TextMode="MultiLine"
                Width="450px"></asp:TextBox><asp:TextBox ID="TextBox3" runat="server" Height="200px" TextMode="MultiLine" Width="450px" style="display:none"></asp:TextBox></td>
	</tr>


	  	      </table>
	  	          <div id="cEndToolbar" class="toolbarBg">
      <div class="caButCase">
          <asp:Button ID="Button1" runat="server" CssClass="button" Text="确认操作" OnClick="Button1_Click1" />
		    
	        <input type="reset" name="Submit2" value="清空重填" class="button">
      </div>
    </div>
</span>
<span id="p2">
 <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>基本信息</div></td>
		<td colspan="2"></td>
	</tr>
<tr>
		<td class="tRight">广告名：</td>
		<td colspan="2"><asp:TextBox ID="TextBox5" runat="server" CssClass="txtwidth" Enabled="False"></asp:TextBox><font color="red">*请用英文加数字命名</font>
            </td>
	</tr>
	<tr>
		<td class="tRight">广告介绍：</td>
		<td colspan="2"><asp:TextBox ID="TextBox8" runat="server" CssClass="txtwidth"></asp:TextBox>
            </td>
	</tr>
	<tr>
		<td class="tRight">图片地址：<asp:TextBox ID="TextBox9" runat="server" CssClass="txtwidth" style="display:none"></asp:TextBox></td>
		<td style="width:650px">
            <span id="xiangce_content"><asp:Literal ID="Literal1" runat="server"></asp:Literal></span></td>
            <td>
            <input id="Button6" class="button" type="button" value="增加一个"  onClick="xiangce()"/>
            </td>
	</tr>
	<tr>
		<td class="tRight">结束时间：</td>
		<td colspan="2"><asp:TextBox ID="TextBox10" runat="server" CssClass="pwdwidth"></asp:TextBox>
            </td>
	</tr>
	<tr>
		<td class="tRight">
            图片大小：</td>
		<td colspan="2"><asp:TextBox ID="TextBox11" runat="server" CssClass="txtwidth">308*242</asp:TextBox>
            </td>
	</tr>


	</table>
	    <div id="Div1" class="toolbarBg">
      <div class="caButCase">
          <asp:Button ID="Button2" runat="server" CssClass="button" Text="确认操作" OnClick="Button2_Click" OnClientClick="return test1()"/>
		    
	        <input type="reset" name="Submit2" value="清空重填" class="button">
      </div>
    </div>
</span>
<span id="p3">
 <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>请选择</div></td>
		<td colspan="2"></td>
	</tr>
	<tr>
		<td class="tRight">广告类型：</td>
		<td colspan="2">
            <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                <asp:ListItem Value="图文" >图文</asp:ListItem>
                <asp:ListItem Value="换图（样式一）">换图（样式一）</asp:ListItem>
                <asp:ListItem Value="换图（样式二）">换图（样式二）</asp:ListItem>
                <asp:ListItem Value="换图（样式三）">换图（样式三）</asp:ListItem>
            </asp:RadioButtonList></td>
	</tr>

	</table>
	    <div id="Div2" class="toolbarBg">
      <div class="caButCase">
          <asp:Label ID="Label1" runat="server" Text="1" style="display:none"></asp:Label>
          <asp:Button ID="Button3" runat="server" CssClass="button" Text="确认操作" OnClick="Button3_Click"/>
		    
	        <input type="reset" name="Submit2" value="清空重填" class="button">
      </div>
    </div>
</span>

<script type="text/javascript">
for(i=1;i<=3;i++)
{
    if(i==<%=i1 %>)
    {
        document.getElementById("p"+i).style.display="";
    }
    else
    {
        document.getElementById("p"+i).style.display="none";
    }
}

function xiangce()
{
	var Label1=document.getElementById("Label1").innerHTML;
	Label1=parseInt(Label1)+1;
	var xiangce_content="";
	xiangce_content="图名：<input type=\"text\" name=\"xiangce_title"+Label1+"\" id=\"xiangce_title"+Label1+"\" />&nbsp;链接：<input type=\"text\" name=\"xiangce_name"+Label1+"\" id=\"xiangce_name"+Label1+"\" />&nbsp;图片地址：<input type=\"text\" name=\"xiangce_pic"+Label1+"\" id=\"xiangce_pic"+Label1+"\" />&nbsp;&nbsp;<input id=\"Button2\" class=\"button\" type=\"button\" value=\"上传图片\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=xiangce_pic"+Label1+"','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/><br>";
	document.getElementById("xiangce_content").innerHTML=document.getElementById("xiangce_content").innerHTML+xiangce_content;
	document.getElementById("Label1").innerHTML=Label1;
}

function test1()
{
	var x_t1="";
		var Label1=document.getElementById("Label1").innerHTML;
		for(i=1;i<=parseInt(Label1);i++)
		{
			if(document.getElementById("xiangce_pic"+i).value!="")
			{
				if(x_t1=="")
				{
				    x_t1=document.getElementById("xiangce_title"+i).value+"yiban"+document.getElementById("xiangce_name"+i).value+"yiban"+document.getElementById("xiangce_pic"+i).value;
				}
				else
				{
				    x_t1=x_t1+"yige"+document.getElementById("xiangce_title"+i).value+"yiban"+document.getElementById("xiangce_name"+i).value+"yiban"+document.getElementById("xiangce_pic"+i).value;
				}
			}
		}

		document.getElementById("TextBox9").value=x_t1;

        return true;
}

</script>
    </form>
</body>
</html>


