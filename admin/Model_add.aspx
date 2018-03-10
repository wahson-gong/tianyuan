<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Model_add.aspx.cs" Inherits="admin_Model_add" ValidateRequest="false" %>
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
		
		<title>添加模型</title>
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
			    <span class="ficon ficon-weizhi"></span>&nbsp;<a href="default.aspx">用户首页</a>
			    &nbsp;>&nbsp;系统管理 &nbsp;>&nbsp;<a href="Model.aspx">模型管理</a>&nbsp;>&nbsp;添加模型
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<table class="table table-info cs-table">
				<tr>
					<th  style="width:60px">模型名</th>
					<td>
						<asp:TextBox ID="TextBox1" runat="server" CssClass="input" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
						<font color="red"><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></font>
					</td>
				</tr>
				<tr>
					<th>模型别名</th>
					<td>
						<asp:TextBox ID="TextBox2" runat="server" CssClass="input" AutoPostBack="True" OnTextChanged="TextBox2_TextChanged" ></asp:TextBox>
						<font color="red">*</font>
					</td>
				</tr>
             <tr>
					<th>模型类型</th>
					<td>
						
            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container1">
            </asp:DropDownList><font color="red">*</font>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
					</td>
				</tr>
                <tr>
					<th>栏目列表模版</th>
					<td>
						<asp:DropDownList ID="DropDownList2" runat="server" CssClass="select-container1">
          </asp:DropDownList>
					</td>
				</tr>
                <tr>
					<th>栏目内容模版</th>
					<td>
						<asp:DropDownList ID="DropDownList3" runat="server" CssClass="select-container1">
          </asp:DropDownList>
					</td>
				</tr>
                <tr>
					<th>栏目路径</th>
					<td>
						<asp:TextBox ID="TextBox3" runat="server" CssClass="input" Text="/"></asp:TextBox>
					</td>
				</tr>
                <tr>
					<th>缩略图大小</th>
					<td>
						<asp:TextBox ID="TextBox4" runat="server" CssClass="input" Text="120*80"></asp:TextBox>
					</td>
				</tr>
                <tr>
					<th>说明</th>
					<td>
						<asp:TextBox ID="TextBox5" runat="server" CssClass="txtinput"  TextMode="MultiLine"></asp:TextBox>
					</td>
				</tr>
                 <tr>
					<th>参照模型ID</th>
					<td>
						<asp:TextBox ID="TextBox6" runat="server" CssClass="input" ></asp:TextBox>
					</td>
				</tr>
                     <tr>
					<th>是否访问</th>
					<td>
						<asp:TextBox ID="TextBox7" runat="server" Text="是" CssClass="input" ></asp:TextBox>
					</td>
				</tr>
                     <tr>
					<th>访问分类</th>
					<td>
						<asp:TextBox ID="TextBox8" runat="server" Text="all" CssClass="input" ></asp:TextBox>
					</td>
				</tr>
                 <tr>
					<th>前置条件</th>
					<td>
						<asp:TextBox ID="TextBox9" runat="server" CssClass="txtinput" TextMode="MultiLine" ></asp:TextBox>
					</td>
				</tr>
                 <tr>
					<th>显示</th>
					<td>
						<div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox1" runat="server"/>
                            <label for="CheckBox1">是</label><asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*设置此字段在前台是否显示"></asp:Label>
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
		</script>
		</form>
	</body>
</html>
