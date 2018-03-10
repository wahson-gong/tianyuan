<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fields_add.aspx.cs" Inherits="admin_Fields_add" ValidateRequest="false" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=no">
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
		
		<title>添加字段</title>
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
			    &nbsp;>&nbsp;系统管理 &nbsp;>&nbsp;<a href="Model.aspx">模型管理</a>&nbsp;>&nbsp;<a href="Fields.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>">字段管理</a>&nbsp;>&nbsp;添加字段
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<table class="table table-info cs-table">
                <tr>
					<th style="width:10%">字段名称</th>
					<td>
						<asp:TextBox ID="TextBox2" runat="server" CssClass="input" AutoPostBack="True" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
                            <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*字段名称的拼音自动设置成数据库字段名"></asp:Label>
					</td>
				</tr>
				<tr>
					<th >数据库字段名</th>
					<td>
						<asp:TextBox ID="TextBox1" runat="server" CssClass="input" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
            <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*这里的名称跟表中字段一样"></asp:Label>
					</td>
				</tr>
				
             <tr>
					<th>字段提示</th>
					<td>
						<asp:TextBox ID="TextBox3" runat="server" CssClass="input"></asp:TextBox>
					</td>
				</tr>
                <tr>
					<th>是否必填</th>
					<td>
						<div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox1" runat="server"/>
                            <label for="CheckBox1">是</label><asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*设置此字段在提交是否必填"></asp:Label>
                        </div>
					</td>
				</tr>
                <tr>
					<th>是否显示</th>
					<td>
                    <div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox2" runat="server"/>
                            <label for="CheckBox2">是</label><asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*设置是否在后台列表显示"></asp:Label>
                      
                        </div>
					</td>
				</tr>
                  
                <tr>
					<th>是否检索</th>
					<td>
						<div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox3" runat="server"/>
                            <label for="CheckBox3">是</label><asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*设置是否成为可搜索的字段"></asp:Label>
                        </div>
					</td>
				</tr>
                <tr>
					<th>是否隐藏</th>
					<td>
						<div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox4" runat="server"/>
                            <label for="CheckBox4">是</label>   <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*设置在小屏下隐藏显示"></asp:Label>
                        </div>
					</td>
				</tr>
                 <tr>
					<th>是否加密</th>
					<td>
						<div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox5" runat="server"/>
                            <label for="CheckBox5">是</label>   <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*设置在数据库加密显示"></asp:Label>
                        </div>
					</td>
				</tr>
                 <tr>
					<th>列表操作</th>
					<td>
						<div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox6" runat="server"/>
                            <label for="CheckBox6">是</label>   <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*设置在列表页操作"></asp:Label>
                        </div>
					</td>
				</tr>
                 <tr>
					<th>是否导入</th>
					<td>
                    <div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox7" runat="server"/>
                            <label for="CheckBox7">是</label><asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*设置是否导入"></asp:Label>
                      
                        </div>
					</td>
				</tr>
                <tr>
					<th>字段类型</th>
					<td>
					
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"  CssClass="select-container1">
            </asp:DropDownList><asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
					</td>
				</tr>
                <tr>
					<th>数据验证</th>
					<td>
						<asp:TextBox ID="TextBox4" runat="server" CssClass="input"></asp:TextBox>
					</td>
				</tr>
                 <tr>
					<th>默认值</th>
					<td>
						<asp:TextBox ID="TextBox5" runat="server" CssClass="txtinput" TextMode="MultiLine"></asp:TextBox>
                        <p>
                            多个值，回车隔开<br />
         默认命令：当前时间 自动获取<br />
         sql语句用法：sql{表名=表名|查询条件=where id=1|排序=order by id desc|返回字段=title,id}<br />
                    函数的用法：hanshu{type}
                        </p>
					</td>
				</tr>
                 <tr>
					<th>列表CSS</th>
					<td>
						<asp:TextBox ID="TextBox6" runat="server" CssClass="input" Text="10%"></asp:TextBox>
					</td>
				</tr>
                 <tr>
					<th>排序</th>
					<td>
						<asp:TextBox ID="TextBox7" runat="server" CssClass="input" Text="0"></asp:TextBox>
					</td>
				</tr>
                 <tr>
					<th>默认链接</th>
					<td>
						<asp:TextBox ID="TextBox8" runat="server" TextMode="MultiLine" CssClass="txtinput"></asp:TextBox>
					</td>
				</tr>
                 <tr>
					<th>事件</th>
					<td>
						<asp:TextBox ID="TextBox9" runat="server" TextMode="MultiLine" CssClass="txtinput"></asp:TextBox>
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
