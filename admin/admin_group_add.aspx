<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin_group_add.aspx.cs" Inherits="admin_usergroup_addd" Debug="true" %>
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
		
		<title>权限添加</title>
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
			    &nbsp;>&nbsp;<a href="admin_table.aspx">管理员管理</a> &nbsp;>&nbsp;<a href="admin_group.aspx">权限分组</a> &nbsp;>&nbsp;权限添加
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<table class="table table-info cs-table">
				<tr>
					<th  style="width:60px" onclick="admin_js()">组名</th>
					<td>
						<asp:TextBox ID="TextBox1" runat="server" CssClass="input"></asp:TextBox><font color="red">*</font>
                         <asp:TextBox ID="TextBox3" runat="server" CssClass="yincang"></asp:TextBox><font color="red"></font>&nbsp;&nbsp;[<a href="javascript:admin_select(0)">全选</a>]&nbsp;[<a href="javascript:admin_select(1)">反选</a>]
					</td>
				</tr>
				<tr>
					<th>参数</th>
					<td>
						 <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
               
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align:left">
                <tr>
                    <td style=" font-weight:700; font-size:14px">
                     <%#DataBinder.Eval(Container.DataItem, "u1")%>
                    </td>
                    <td>
                </tr>
            </table>
           <%#set_dt(DataBinder.Eval(Container.DataItem, "id").ToString())%>
            </ItemTemplate>
            </asp:Repeater>
					</td>
				</tr>
            
              
				<tr>
					<th></th>
					<td>
						<asp:LinkButton ID="LinkButton1" CssClass="btn btn-blue" runat="server" OnClick="LinkButton1_Click" OnClientClick="return admin_js()" ><em class="ficon ficon-queren"></em> 确认操作</asp:LinkButton>&nbsp;&nbsp;
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
            <script type="text/javascript">
    var che_list=<%=i1 %>;
    function admin_js()
    {
        var t1="";
        var t2="";
        for(i=1;i<che_list;i++)
        {
			t2="";
            var boxes = document.getElementsByName("qx_box_"+i); 
            for (var j = 0; j < boxes.length; j++)   
            {
                 if (boxes[j].checked)   
                {
                       if(j==0)
                       {
                                t2=t2+boxes[j].value;
                       }
                       else
                       {
                                t2=t2+","+boxes[j].value; 
                       }
                }
                else
                {
                        if(j==0)
                       {
                                t2=t2+"";
                       }
                       else
                       {
                                t2=t2+","+""; 
                       } 
                }
               
            }
			t2=document.getElementById("qx_value_"+i).innerHTML+"{fzw:zu}"+t2;
			if(t1=="")
			{
			 t1=t1+t2;
			}
			else
			{
			t1=	t1+"{fzw:dui}"+t2;
			}
        }
    // alert(t1);
       document.getElementById("TextBox3").value=t1;
    }
    
	
	function admin_select(g1)
	{
			for(i=1;i<che_list;i++)
        	{
				 var boxes = document.getElementsByName("qx_box_"+i); 
				for (var j = 0; j < boxes.length; j++)   
				{
					if(g1=="0")
					{
						boxes[j].checked=true;
					}
					else
					{
						if (boxes[j].checked)   
						{
							boxes[j].checked=false;
						}
						else
						{
							boxes[j].checked=true;
						}
					}
				}
			}
	}
</script>
		</form>
	</body>
</html>