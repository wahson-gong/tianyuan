﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="collect_list.aspx.cs" Inherits="admin_collect_list" %>
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
		
		<title>参数管理</title>
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
			    &nbsp;>&nbsp;系统管理 &nbsp;>&nbsp;<a href="Parameter.aspx">参数管理</a>
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<!--筛选条件-->
			<div class="filter">
				
				<div class="filter-left">
					<div class="form-control">
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="input" placeholder="搜索条件"></asp:TextBox>
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-blue" OnClick="Button1_Click" Text="搜索" />
				    </div>
				</div>
				
				<div class="filter-right">
					<a href="collect_edit.aspx?type=daoru" class="btn btn-green"> <em class="ficon ficon-tianjia"></em> 导入采集规则</a>
				   &nbsp;
				    <a href="collect_edit.aspx" class="btn btn-green"> <em class="ficon ficon-tianjia"></em> 添加采集项目</a>
				</div>
				<br />
				<p class="clear-right"></p>
			</div>
			<!--筛选条件 end-->
			
			<!--文章table-->
			<table class="table table-border table-hover" id="content">
				<thead>
				<tr>
                    <th  style="width:5%">&nbsp;</th>
					<th  style="width:6%">编号</th>
					<th>项目名</th>
					<th style="width:20%">列表页</th>
					<th>模型</th>
                    <th>目录</th>
                    <th>时间</th>
					<th style="width:20%">操作</th>
				</tr>
				</thead>
				<tbody>
				 <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
				<tr>
				    <td>
                        &nbsp;&nbsp;
						<div class="checkbox checkbox-inline checkbox-success">
                            <input type="checkbox" name="chk" value="<%#DataBinder.Eval(Container.DataItem, "id")%>" />
                            <label></label>
                        </div>
				    </td>
					<td>
						<%#DataBinder.Eval(Container.DataItem, "id").ToString()%>
					</td>
					<td style="text-align: left;"><%#DataBinder.Eval(Container.DataItem, "u1").ToString()%></td>
					<td>
						<a href="<%#DataBinder.Eval(Container.DataItem, "u3").ToString()%>" target="_blank"><%#my_b.jiequ("yes", DataBinder.Eval(Container.DataItem, "u3").ToString(),30)%></a>
					</td>
					<td>
						<%#my_b.get_value("u2", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + DataBinder.Eval(Container.DataItem, "u11").ToString() + "")%>
					</td>
                    <td>
						<%#my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + DataBinder.Eval(Container.DataItem, "u17").ToString() + "")%>
					</td>
                    <td><%#DataBinder.Eval(Container.DataItem, "dtime", "{0:yyyy-MM-dd}")%></td>
					<td>
                        <a href="collect.aspx?id=<%#DataBinder.Eval(Container.DataItem, "id")%>&page=<%#DataBinder.Eval(Container.DataItem, "u18")%>" title="采集" class="operation" target="_blank"><span class="ficon ficon-shangchuan"></span></a>
						<a href="collect_edit.aspx?type=0&id=<%#DataBinder.Eval(Container.DataItem, "id")%>" title="为零" class="operation"><span class="ficon ficon-yunying"></span></a>
                        <a href="collect_edit.aspx?type=copy&id=<%#DataBinder.Eval(Container.DataItem, "id")%>" title="复制" class="operation"><span class="ficon ficon-fuzhi"></span></a>
                        <a href="dao_collect.aspx?id=<%#DataBinder.Eval(Container.DataItem, "id")%>" title="导出" class="operation"><span class="ficon ficon-yuncipancds"></span></a> <a  href="collect_edit.aspx?type=edit&id=<%#DataBinder.Eval(Container.DataItem, "id")%>" title="编辑" class="operation"><span class="ficon ficon-xiugai"></span></a>
						<a  onclick="return confirm('你确认将该频道删除到回收站?\r\n注意：相关栏目和内容也将一起删除到回收站');" id="repChannel_ctl01_btnDelete" href="Parameter_add.aspx?type=del&id=<%#DataBinder.Eval(Container.DataItem, "id")%>" title="删除" class="operation"><span class="ficon  ficon-shanchu"></span></a>
					</td>
					
				</tr>
</ItemTemplate>
                 </asp:Repeater>
				</tbody>
			</table>
			<!--文章table  end-->
			<!--操作部分-->
			<div class="o-hidden padtb-20">
			    
			
				<div class="fl full-sm">
				    <div class="checkbox checkbox-success fl" style="margin-bottom: 0;">
	                    <input type="checkbox" id="check-all" onclick="selcheck('content')">
	                    <label for="check-all">全选</label>
	                 </div>
	                 <div class="fl ft14">
	                 	&nbsp;&nbsp;
	                     <a href="javascript:chk1()" title="删除" class="operation gray"><span class="ficon ficon-qingkong"></span> 删除</a>
					
	                 </div>
	                 <p class="clear"></p>
				</div>
				<div class="fr full-sm">
				    <div class="pagelist">
				       <asp:Literal ID="Literal1" runat="server"></asp:Literal>
				    </div>
				</div>
			</div>
			<!--操作部分 end-->

			
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
    window.location="collect_edit.aspx?type=del&id="+t1;
    window.event.returnValue = false;
}
        </script> 
		</form>
	</body>
</html>
