<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tongji_product.aspx.cs" Inherits="tongji_product" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1">
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
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
		
		<title>�̼�ͳ��</title>
		<style type="text/css">
			
		</style>
		  <script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
	</head>
	<body class="body-color">
        <form id="form1" runat="server">
		 <uc1:top ID="top1" runat="server" />
		<!--��߲˵���-->
		<aside class="side-nav">
			<div class="pos-relative">
			<ul class="side-nav-ul ">
				<%=Cache["date"] %>
			</ul>
			<em class="sj"></em>
			</div>
		</aside>
		<!--��߲˵��� end-->
		
		<!--���岿��-->
		<section class="body-content">
			<!--��ǰλ��-->
			<div class="site-location">
				&nbsp;&nbsp;
			    <span class="ficon ficon-weizhi"></span>&nbsp;<a href="default.aspx">�û���ҳ</a>
			    &nbsp;>&nbsp;ϵͳ���� &nbsp;>&nbsp;<a href="Parameter.aspx">��������</a>
			</div>
			<p class="silo_zw"></p>
			<!--��ǰλ�� end-->
			<br />
			<!--ɸѡ����-->
			<div class="filter">
				
				<div class="filter-left">
					<div class="form-control">
					ʱ�䣺��<asp:TextBox ID="TextBox1" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>��<asp:TextBox ID="TextBox2" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>
				    </div>
				    <div class="form-control">
					    ״̬��
					    <div class="select-container">  
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container1"></asp:DropDownList>
	 					</div>  
					    
					    &nbsp;&nbsp;
				    </div>
			
				</div>
				
				<div class="filter-right">
					<div class="form-control">
                        <asp:TextBox  ID="TextBox3" runat="server" class="input" placeholder="��������"></asp:TextBox>
				    	
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-blue" OnClick="Button1_Click1"
                Text="����" />
				    </div>
				
				</div>
				<br />
				<p class="clear-right"></p>
			</div>
			<!--ɸѡ���� end-->
			
			<!--����table-->
			<table class="table table-border table-hover" id="content">
				<thead>
				<tr>
       
					<th  style="width:auto">��Ʒ����</th>
					<th style="width:10%">����</th>
                    <th  style="width:10%">����</th>
                    <th  style="width:10%">С��</th>
				</tr>
				</thead>
				<tbody>
				 <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
				<tr>

					
					<td style="text-align:left; word-break:break-all; word-wrap:break-all;"><%#my_b.get_value("biaoti","sl_product","where id="+DataBinder.Eval(Container.DataItem, "laiyuanbianhao").ToString()+"")%></td>
			
					<td><%#DataBinder.Eval(Container.DataItem, "jine").ToString()%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "shuliang").ToString()%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "xiaoji").ToString()%></td>
				</tr>
</ItemTemplate>
                 </asp:Repeater>
				</tbody>
			</table>
			<!--����table  end-->
			<!--��������-->
			<div class="o-hidden padtb-20">
			    
			
				<div class="fl full-sm">
				    <div class="checkbox checkbox-success fl" style="margin-bottom: 0;">
	                    <input type="checkbox" id="check-all" onclick="selcheck('content')">
	                    <label for="check-all">ȫѡ</label>
	                 </div>
	                 <div class="fl ft14">
	                 	&nbsp;&nbsp;
	                     <a href="javascript:chk1()" title="ɾ��" class="operation gray"><span class="ficon ficon-qingkong"></span> ɾ��</a>
					
	                 </div>
	                 <p class="clear"></p>
				</div>
				<div class="fr full-sm">
				    <div class="pagelist">
				       <asp:Literal ID="Literal1" runat="server"></asp:Literal>
				    </div>
				</div>
			</div>
			<!--�������� end-->

			
			 <uc2:botton ID="botton1" runat="server" />
		</section>
		<p class="clear"></p>
		
		<!--<script src="js/bootstrap.min.js" type="text/javascript"></script>-->
		<script src="js/wow.min.js" type="text/javascript"></script>
		<script src="js/common.js" type="text/javascript"></script>
		
		<!--wow ��ʼ��-->
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
    window.location="Web_Blog.aspx?type=del&id="+t1;

}
        </script> 
		</form>
	</body>
</html>