<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zz_table.aspx.cs" Inherits="admin_auto_table" %>
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
		
		<title>表单管理</title>
		<style type="text/css">
			
		</style>
			<script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
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
			    &nbsp;>&nbsp;内容管理 &nbsp;>&nbsp;<a href="Class_Table.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>">表单管理</a>
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<!--筛选条件-->
			<div class="filter">
				
				<div class="filter-left">
                    <div class="form-control">
					时间：从<asp:TextBox ID="TextBox1" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>至<asp:TextBox ID="TextBox2" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>
				    </div>
                     <%
                         if (sl_user.Rows[0]["u3"].ToString().IndexOf("管理员") > -1)
                         { %>
				    <div class="form-control">
					    审核状态：
                         <div class="select-container">  
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container">
                                <asp:ListItem>全部</asp:ListItem>
                                <asp:ListItem>已通过</asp:ListItem>
                                <asp:ListItem>未通过</asp:ListItem>
                                <asp:ListItem>待审核</asp:ListItem>

                            </asp:DropDownList>
	 					</div>  
					    &nbsp;&nbsp;
				    </div>
                    <%} %>
					<div class="form-control">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="input" placeholder="搜索条件"></asp:TextBox>
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-blue" OnClick="Button1_Click" Text="搜索" />
				    </div>
				</div>
				
				<div class="filter-right">
					<asp:Literal ID="Literal3" runat="server"></asp:Literal>
                     <a href="user_table_down.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>" target="_blank" class="btn btn-red"> <em class="ficon ficon-huifu"></em> 导出</a>
                    <a href="<%=add__url() %>" class="btn btn-green"> <em class="ficon ficon-tianjia"></em> 添加</a>
				</div>
				<br />
				<p class="clear-right"></p>
			</div>
			<!--筛选条件 end-->
			
			<!--文章table-->
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
			
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
						 <%
                             if (shenhe_sta == "yes")
                             {

                            
                             if (sl_user.Rows[0]["u3"].ToString().IndexOf("管理员")>-1)
                             { %>
                         <a href="javascript:chk3()" title="审核通过" class="operation gray"><span class="ficon ficon-renzhengtongguo"></span> 审核通过</a>
						<a href="javascript:chk4_()" title="审核不通过" class="operation gray"><span class="ficon ficon-liuchengbutongguo"></span> 审核不通过</a>
						<%} } %>
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
		  <script src="/inc/layer/layer.js"></script>
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

    if(t1!="")
    {
           var tiao_url ="zz_table_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>&type=del&id="+t1;
    setajax(tiao_url, "yes");
    }

}
  function chk3()
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
    window.location = "zz_table_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>&type=shenhe&id=" + t1;
window.event.returnValue = false;
    }
    function chk4_() {
        document.getElementById("div_ss").style.display = "";
        return false;
    }
    function chk4()
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
    window.location = "zz_table_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>&shenheshuoming=" + document.getElementById("shenheshuoming").value + "&type=shenhe1&id=" + t1;
window.event.returnValue = false;
    }
 </script> 
              <div class="blk" style="position: fixed;left: 0px;top: 0px;width: 100%;background: rgba(0,0,0,0.5);filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#7F000000', EndColorStr='#7F000000'); display:none" id="div_ss">
		    <div class="aaa" style="position: relative;width: 100%; ">

		        <div class="eee" style="width: 60%; margin:0 auto;border-radius: 6px;position: relative;position: relative; background-color: #fff;">
		        	
		        	<p class="close" style="width: 32px;height: 32px;font:bold 30px arial;color: #fff;text-align: center;line-height: 32px;position: absolute;right: -32px;top:-32px;cursor: pointer;border-radius: 6px;">×</p>
		        	
		        	<div style="width: 90%;margin: 0 auto;padding: 18px 0px;">
		        	    <textarea name="shenheshuoming" id="shenheshuoming" style="width: 100%;height: 100px; border: 1px solid #ddd;margin-bottom: 12px;"></textarea>
			            <input type="submit" name="" style="display: block; width: 130px;height:40px;font: 16px 'microsoft yahei';color: #fff;background: #004497;border-radius: 4px; margin: 0 auto;" value="提交审核说明"  onclick="return chk4()"/>
		        	</div>
			            
		            
		        </div>
				
		    </div>
		</div>
		<script type="text/javascript">
			var h=window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
			$(".blk").height(h);
			var b=$(".eee").height();
			var a=(h-b)/2
			$(".aaa").css("padding-top",a+"px");
			$(".close").click(function(){
				$(".blk").hide();
			})
		</script>
		</form>
	</body>
</html>
