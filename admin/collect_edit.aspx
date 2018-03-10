<%@ Page Language="C#" AutoEventWireup="true" CodeFile="collect_edit.aspx.cs" Inherits="admin_collect_edit" ValidateRequest="false"  enableEventValidation="false"%>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>添加采集项目</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="images/collect.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg" style="height:1500px"> 
    <div style="float:left">
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a>&nbsp;
        <span class="split ftSplit" ></span></div>
	<div id="guide" class="light_orange">您现在的位置：采集管理 >> <a href="collect_list.aspx"  style="color:#000">采集项目管理 </a> >> 添加采集项目</div>
<asp:Panel ID="feidaoru" runat="server">
        <asp:Panel ID="Panel1" runat="server">
        <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>采集项目基本设置</div></td>
		<td colspan="2"></td>
	</tr>
	<tr>
		<td class="tRight">项目名称 ：</td>
		<td colspan="2"><asp:TextBox ID="TextBox1" runat="server" Width="350px"></asp:TextBox><font color="red">*</font>
            </td>
	</tr>
	<tr>
		<td class="tRight">网页编码 ：</td>
		<td colspan="2">
            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True">gb2312</asp:ListItem>
                <asp:ListItem>utf-8</asp:ListItem>
                <asp:ListItem>gbk</asp:ListItem>
            </asp:RadioButtonList></td>
	</tr>
	<tr>
		<td class="tRight">列表页URL ：</td>
		<td colspan="2"><asp:TextBox ID="TextBox2" runat="server" Width="350px"></asp:TextBox><font color="red">*</font>
            </td>
	</tr>
	<tr>
		<td class="tRight">选择分页类型 ：</td>
		<td colspan="2">
            <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal" onclick="feiye_leixin()">
                <asp:ListItem Selected="True">不采集列表分页</asp:ListItem>
                <asp:ListItem>批量指定分页URL代码</asp:ListItem>
            </asp:RadioButtonList></td>
	</tr>
	<tr id="feiye_leixin1"<%=set_1 %>>
		<td class="tRight">列表分页URL ：</td>
		<td colspan="2"><asp:TextBox ID="TextBox3" runat="server" Width="350px"></asp:TextBox><font color="red">*例：http://www.8844k.com/sort_{$ID}.html     {$ID} 为变量 </font>
            </td>
	</tr>
	<tr id="feiye_leixin2" <%=set_1 %>>
		<td class="tRight">ID范围 ：</td>
		<td colspan="2"><asp:TextBox ID="TextBox4" runat="server" CssClass="pwdwidth"></asp:TextBox>
            To<asp:TextBox ID="TextBox5" runat="server" CssClass="pwdwidth"></asp:TextBox>&nbsp;&nbsp;步值：<asp:TextBox ID="TextBox20" runat="server" Width="30px" Text="1"></asp:TextBox><font color="red">*支持： 1~999，999~1；01~99，99~01；001~999，999~001 </font></td>
	</tr>
	  	      </table>
    <div id="cEndToolbar" class="toolbarBg">
      <div class="caButCase">
          <input id="Button1" type="button" value="下一步" class="button" onclick="return feiye_Panel(2)"/></div>
    </div>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server">
        <table class="cTable_2 table">
            <tr class="cTitle toolbarBg">
                <td width="25%">
                    <div>
                        列表页中目标内容页URL设置</div>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    列表左边代码 ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox6" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    列表右边代码 ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox7" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    目标链接左边代码 ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox8" runat="server" Width="350px"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    目标链接右边代码 ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox9" runat="server" Width="350px"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
           
        </table>
            <div id="Div1" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button3" type="button" value="上一步" class="button" onclick="return feiye_Panel(1)"/><input id="Button2" type="button" value="下一步" class="button" onclick="return feiye_Panel(3)"/>
                    </div>
    
    </div>
        </asp:Panel>
        <asp:Panel ID="Panel3" runat="server">
          <table class="cTable_2 table">
            <tr class="cTitle toolbarBg">
                <td width="25%">
                    <div>
                        捕获列表页中目标内容页URL测试</div>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    测试列表页URL ：</td>
                <td colspan="2">
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="加载中.."></asp:Label></td>
            </tr>
            <tr>
                <td class="tRight">
                    获取到的内容页列表：</td>
                <td colspan="2">
                    <asp:ListBox ID="ListBox1" runat="server" Rows="30"></asp:ListBox></td>
            </tr>
          <tr>
                <td class="tRight">
                    获取记录条数：</td>
                <td colspan="2">
                    &nbsp;<asp:Label ID="Label2" runat="server" Text="加载中.."></asp:Label></td>
            </tr>
           
        </table>
            <div class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button5" type="button" value="上一步" class="button" onclick="return feiye_Panel(2)"/><input id="Button4" type="button" value="下一步" class="button" onclick="return feiye_Panel(4)"/></div>
    
    </div>
        </asp:Panel>
   
          <asp:Panel ID="Panel4" runat="server">
          <table class="cTable_2 table">
            <tr class="cTitle toolbarBg">
                <td width="25%">
                    <div>
                        模型设置</div>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    模型 ：</td>
                <td style="width:300px; vertical-align:top">
                    <asp:DropDownList ID="DropDownList1" runat="server" Onchange="collect_ajax(4)">
                    </asp:DropDownList>
                    <asp:DropDownList ID="DropDownList2" runat="server">
                    </asp:DropDownList></td>
                 <td style="vertical-align:top">
                 <asp:Label ID="Label3" runat="server"></asp:Label>
                 </td>
            </tr>

           
        </table>
            <div id="Div4" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button9" type="button" value="上一步" class="button" onclick="return feiye_Panel(3)"/><input id="Button8" type="button" value="下一步" class="button" onclick="return feiye_Panel(5)"/></div>
    
    </div>
        </asp:Panel>
          <asp:Panel ID="Panel5" runat="server">
          
       <asp:Label ID="Label4" runat="server" Text="加载中.."></asp:Label>           
     
            <div id="Div5" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button11" type="button" value="上一步" class="button" onclick="return feiye_Panel(4)"/><input id="Button10" type="button" value="下一步" class="button" onclick="return feiye_Panel(6)"/></div>
    
    </div>
        </asp:Panel>
        <asp:Panel ID="Panel6" runat="server">
          
       <asp:Label ID="Label5" runat="server" Text="加载中.."></asp:Label>           
     
            <div id="Div3" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button6" type="button" value="上一步" class="button" onclick="return feiye_Panel(5)"/><input id="Button7" type="button" value="下一步" class="button" onclick="return feiye_Panel(7)"/></div>
    
    </div>
        </asp:Panel>
        
         <asp:Panel ID="Panel7" runat="server">
          
        <table class="cTable_2 table">
            <tr class="cTitle toolbarBg">
                <td width="25%">
                    <div>
                        正文过滤选项设置（支持正则表达式）</div>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    替换原始字符A：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox10" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    替换结果字符A ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox11" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td class="tRight">
                   替换原始字符B ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox12" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    替换结果字符B ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox13" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
             <tr>
                <td class="tRight">
                    替换原始字符C ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox14" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    替换结果字符C ：</td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox15" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox><font color="red">*</font>
                </td>
            </tr>
           
        </table>           
     
            <div id="Div6" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button12" type="button" value="上一步" class="button" onclick="return feiye_Panel(6)"/><input id="Button13" type="button" value="下一步" class="button" onclick="return feiye_Panel(8)"/></div>
    
    </div>
        </asp:Panel>
         <asp:Panel ID="Panel8" runat="server">
          
       <asp:Label ID="Label6" runat="server"  Text="加载中.."></asp:Label>           
     
            <div id="Div7" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button14" type="button" value="上一步" class="button" onclick="return feiye_Panel(7)"/><input id="Button19" type="button" value="下一步" class="button" onclick="return feiye_Panel(9)"/></div>
    
    </div>
        </asp:Panel>
        
           <asp:Panel ID="Panel9" runat="server">
          
        <table class="cTable_2 table">
            <tr class="cTitle toolbarBg">
                <td width="25%">
                    <div>
                        正文过滤选项设置</div>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="tRight">
                    内容过滤选项：</td>
                <td colspan="2">
                    &nbsp;<asp:Label ID="Label7" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="tRight">
                    采集图片设置 ：</td>
                <td colspan="2">
                    <asp:CheckBoxList ID="CheckBoxList2" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">保存远程图片</asp:ListItem>
                        <asp:ListItem>图片增加水印</asp:ListItem>
                        <asp:ListItem Selected="True">为第一张图片创建缩略图</asp:ListItem>
                        <asp:ListItem>文件地址</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td class="tRight">
                    <span>文章采集</span> ：</td>
                <td colspan="2">
                    <asp:RadioButtonList ID="RadioButtonList3" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">智能覆盖</asp:ListItem>
                        <asp:ListItem>完全替换</asp:ListItem>
                        <asp:ListItem>跳过此条</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            	<tr>
		<td class="tRight">缩略图大小 ：</td>
		<td colspan="2"><asp:TextBox ID="TextBox19" runat="server" CssClass="pwdwidth">180*195</asp:TextBox><font color="red">*如果缩略图字段为空，可自动生成</font>
            </td>
	</tr>
        </table>           
     
            <div id="Div9" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button15" type="button" value="上一步" class="button" onclick="return feiye_Panel(8)"/><input id="Button18" type="button" value="下一步" class="button" onclick="return feiye_Panel(10)"/></div>
    
    </div>
        </asp:Panel>
        
         <asp:Panel ID="Panel10" runat="server">
          
       <asp:Label ID="Label8" runat="server"></asp:Label>           
     
            <div id="Div8" class="toolbarBg">
                <div class="caButCase">
                    
                     <input id="Button16" type="button" value="上一步" class="button" onclick="return feiye_Panel(9)"/>
                    <asp:Button ID="Button17" runat="server" OnClick="Button17_Click" Text="确认保存" CssClass="button"  OnClientClick="return return_c()"/>
                    <span style="display:none"><asp:TextBox ID="TextBox16" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:TextBox ID="TextBox17" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:TextBox ID="TextBox18" runat="server" TextMode="MultiLine"></asp:TextBox></span>
                    </div>
    
    </div>
        </asp:Panel>
        
</asp:Panel>
<asp:Panel ID="daoru" runat="server">
 <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>导入采集规则</div></td>
		<td colspan="2"></td>
	</tr>
	<tr>
		<td class="tRight">上传文件 ：</td>
		<td>
            <asp:FileUpload ID="FileUpload1" runat="server" /></td>
            <td>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></td>
	</tr>
	</table>
            <div id="Div2" class="toolbarBg">
                <div class="caButCase">
  
                    <asp:Button ID="Button21" runat="server"  Text="确认保存" CssClass="button" OnClick="Button21_Click"  />

                    </div>
    
    </div>
</asp:Panel>

        
    </div>

<script type="text/javascript">
feiye_Panel(1);
</script>
    </form>
</body>
</html>


