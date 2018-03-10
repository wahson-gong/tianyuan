<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Columns_Table_add.aspx.cs" Inherits="admin_Columns_Table_add" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1">
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<!--<link rel="stylesheet" type="text/css" href="css/bootstrap.min.css"/>-->
		<link rel="stylesheet" type="text/css" href="css/animate.min.css" />
		<link rel="stylesheet" type="text/css" href="fonts/css/fontello.css"/>
		<link rel="stylesheet" type="text/css" href="css/components.css"/>
		<link rel="stylesheet" type="text/css" href="css/common.css"/>
		<!--[if lt IE 8]>
			<link rel="stylesheet" href="fonts/css/fontello-ie7.css">
		<![endif]-->
		<script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
		<!--[if lt IE 9]>
		      <script src="js/html5shiv.min.js"></script>
		      <script src="js/respond.min.js"></script>
		    <![endif]-->
		
		<title>网站栏目添加</title>
		<style type="text/css">
			
		</style>
		
	</head>
	<body class="body-color">
        <form id="form1" runat="server">
		<uc1:top ID="top1" runat="server" />
		<!--侧边菜单栏-->
		<aside class="side-nav">
			<div class="pos-relative">
			<ul class="side-nav-ul ">
				<%=Cache["date"] %>
			</ul>
			<em class="sj"></em>
			</div>
		</aside>
		<!--侧边菜单栏 end-->
		
		<!--主体部分-->
		<section class="body-content">
			<!--当前位置-->
			<div class="site-location">
				&nbsp;&nbsp;
			    <span class="ficon ficon-weizhi"></span>&nbsp;<a href="default.aspx">用户首页</a>&nbsp;>&nbsp;系统管理
			    &nbsp;>&nbsp;<a href="Columns_Table.aspx">栏目管理</a>&nbsp;>&nbsp;网站栏目添加
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<table class="table table-info cs-table">
				<tr>
					<th  style="width:60px">上级栏目</th>
					<td>
						<div>  
                            <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged" CssClass="select-container1">
            </asp:DropDownList>  
			
	 					</div>  
					</td>
				</tr>
				<tr>
					<th>栏目名</th>
					<td>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="input"></asp:TextBox>
					</td>
				</tr>
                <asp:Panel ID="Panel1" runat="server">
				<tr>
					<th>排放</th>
					<td>
						<div>  
                             <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container1">
                <asp:ListItem Value="1">一行一个</asp:ListItem>
                <asp:ListItem Value="2">一行两个</asp:ListItem>
            </asp:DropDownList>
				          
	 					</div>  
					</td>
				</tr>
</asp:Panel>
                 <asp:Panel ID="Panel2" runat="server">
                     <tr>
					<th>链接地址</th>
					<td>
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="input"></asp:TextBox>
					</td>
				</tr>
                     <tr>
					<th>颜色</th>
					<td>
                         <asp:DropDownList ID="DropDownList2" runat="server" CssClass="select-container1">
                             <asp:ListItem Value="">选择颜色</asp:ListItem>
                            <asp:ListItem Value="bg-skyblue" class="bg-skyblue"></asp:ListItem>
                             <asp:ListItem Value="bg-green" class="bg-green"></asp:ListItem>
                              <asp:ListItem Value="bg-yellow" class="bg-yellow"></asp:ListItem>
                              <asp:ListItem Value="bg-blue" class="bg-blue"></asp:ListItem>
                              <asp:ListItem Value="bg-red" class="bg-red"></asp:ListItem>
                              <asp:ListItem Value="bg-watermelon-red" class="bg-watermelon-red"></asp:ListItem>
                             <asp:ListItem Value="bg-purple" class="bg-purple"></asp:ListItem>
                        </asp:DropDownList>
                      
					</td>
				</tr>
                  </asp:Panel>
				<tr>
					<th>显示方式</th>
					<td>
						<asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
              <asp:ListItem Selected="True">显示</asp:ListItem>
              <asp:ListItem>隐藏</asp:ListItem>
          </asp:RadioButtonList>
                    
					</td>
				</tr>
                 <tr>
					<th>选择图标</th>
					<td>
                          
                      <div class="enterwords-box fl">
                           <em class="ficon <%=TextBox3_pic %>" style="font-size:20px" id="TextBox3_pic"></em>&nbsp;
						<asp:TextBox ID="TextBox3" runat="server" CssClass="dinput" Text="ficon-lanmu" Width="160px"></asp:TextBox>
						</div>
						<div class="fl" style="padding: 10px 0 0 12px;">
							<p class="marb-20"><a href="fonts/demo.aspx?editname=TextBox3" target="_blank" class="btn btn-blue"><em class="ficon  ficon-uploading"></em> 选择图标</a></p>
						
						</div>
                          
					</td>
				</tr>
				<tr>
					<th></th>
					<td>
                        <asp:LinkButton ID="LinkButton1" CssClass="btn btn-blue" runat="server" OnClick="LinkButton1_Click"><em class="ficon ficon-queren"></em> 确认操作</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="LinkButton2" CssClass="btn btn-green" runat="server" OnClick="LinkButton2_Click"><em class="ficon ficon-shanchu"></em> 清空重填</asp:LinkButton>
						
					</td>
				</tr>
			</table>
			
			<uc2:botton ID="botton1" runat="server" />
		</section>
		<p class="clear"></p>
		
		<!--<script src="js/bootstrap.min.js" type="text/javascript"></script>-->
		<script src="js/wow.min.js" type="text/javascript"></script>
		<script src="js/common.js" type="text/javascript"></script>
		
		<!--wow 初始化-->
		<script>
			if (!(/msie [6|7|8|9]/i.test(navigator.userAgent))) {
				new WOW().init();
			};

			$(".yanse").click(function () {
			    var title = $(this).data('title');
			    $("#TextBox4").val(title);
			});
		</script>
		
		
		</form>
	</body>
</html>
