<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fields.aspx.cs" Inherits="admin_Fields" %>
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
		
		<title>字段管理</title>
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
			    &nbsp;>&nbsp;系统管理 &nbsp;>&nbsp;<a href="Model.aspx">模型管理</a>&nbsp;>&nbsp;<a href="Fields.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>">字段管理</a>
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<!--筛选条件-->
			<div class="filter">
				
				<div class="filter-left">
					
				    <div class="form-control">
                         <asp:Repeater ID="Repeater2" runat="server">
                 <ItemTemplate>
					      表名：<a href="<%#get_url(my_b.get_value("u2", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter", "where u1='" + DataBinder.Eval(Container.DataItem, "u3").ToString() + "'").ToString(),DataBinder.Eval(Container.DataItem, "id").ToString(),"no")%>" title="点击上传"><%#DataBinder.Eval(Container.DataItem, "u1").ToString()%><em class="ficon  ficon-shangchuan"></em> </a>
					    </ItemTemplate></asp:Repeater>
					    &nbsp;&nbsp;
				    </div>
				    
				</div>
				
				<div class="filter-right">
					
				   
				    <a href="Fields_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>" class="btn btn-green"> <em class="ficon ficon-tianjia"></em> 添加</a>
				</div>
				<br />
				<p class="clear-right"></p>
			</div>
			<!--筛选条件 end-->
			
			<!--文章table-->
			<table class="table table-border table-hover">
				<thead>
				<tr>
					<th  style="width:8%">编号</th>
					<th style="width:20%">字段名称</th>
					<th>字段别名</th>
                    <th  style="width:6%">检索</th>
                    <th  style="width:6%">必填</th>
                    <th  style="width:6%">显示</th>
                    <th  style="width:6%">隐藏</th>
					<th class="hidden-xs" style="width:10%">字段类型</th>
                    <th class="hidden-xs" style="width:10%">列表CSS</th>
                    <th  style="width:6%">排序</th>
					<th>操作</th>
				</tr>
				</thead>
				<tbody>
				 <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
				<tr>
				
					<td>
						<%#DataBinder.Eval(Container.DataItem, "id").ToString()%>
					</td>
					<td style=""><a href="Fields_add.aspx?type=edit&id=<%#DataBinder.Eval(Container.DataItem, "id")%>&Model_id=<%#DataBinder.Eval(Container.DataItem, "Model_id")%>"><%#DataBinder.Eval(Container.DataItem, "u1").ToString()%></a></td>
			<td><%#DataBinder.Eval(Container.DataItem, "u2").ToString()%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "u10").ToString()%></td>
            <td><%#DataBinder.Eval(Container.DataItem, "u4").ToString()%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "u5").ToString()%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "u13").ToString()%></td>
			<td class="hidden-xs"><%#DataBinder.Eval(Container.DataItem, "u6").ToString()%></td>
                    <td class="hidden-xs"><%#DataBinder.Eval(Container.DataItem, "u11").ToString()%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "u9").ToString()%></td>
					<td>
                       
						<a href="Fields_add.aspx?type=edit&id=<%#DataBinder.Eval(Container.DataItem, "id")%>&Model_id=<%#DataBinder.Eval(Container.DataItem, "Model_id")%>" title="编辑" class="operation"><span class="ficon ficon-xiugai"></span></a>
						<a  onclick="return confirm('你确认将该频道删除到回收站?\r\n注意：相关栏目和内容也将一起删除到回收站');" id="repChannel_ctl01_btnDelete" href="Fields_add.aspx?type=del&id=<%#DataBinder.Eval(Container.DataItem, "id")%>&Model_id=<%#DataBinder.Eval(Container.DataItem, "Model_id")%>" title="删除" class="operation"><span class="ficon  ficon-shanchu"></span></a>
					</td>
					
				</tr>
</ItemTemplate>
                 </asp:Repeater>
				</tbody>
			</table>
			<!--文章table  end-->
			

			
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
