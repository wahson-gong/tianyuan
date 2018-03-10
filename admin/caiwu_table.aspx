<%@ Page Language="C#" AutoEventWireup="true" CodeFile="caiwu_table.aspx.cs" Inherits="caiwu_table" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=no">
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
        <script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
		<!--[if lt IE 9]>
		      <script src="js/html5shiv.min.js"></script>
		      <script src="js/respond.min.js"></script>
		    <![endif]-->
		
		<title>������</title>
		<style type="text/css">
			
		</style>
		
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
			    &nbsp;>&nbsp;���ݹ��� &nbsp;>&nbsp;<a href="Class_Table.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>">������</a>
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
					    ���ͣ�
					    <div class="select-container">  
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container"></asp:DropDownList>
	 					</div>  
					    
					    &nbsp;&nbsp;
                        ״̬��
					    <div class="select-container">  
                            <asp:DropDownList ID="DropDownList2" runat="server" CssClass="select-container">
                                <asp:ListItem>ȫ��</asp:ListItem>
                                <asp:ListItem>����</asp:ListItem>
                                <asp:ListItem>����</asp:ListItem>
                            </asp:DropDownList>
	 					</div>  
					    
					    &nbsp;&nbsp;
				    </div>
				</div>
				
				<div class="filter-right">
                    <div class="form-control">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="input" placeholder="��������"></asp:TextBox>
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-blue" OnClick="Button1_Click" Text="����" />
				    </div>
					<asp:Literal ID="Literal3" runat="server"></asp:Literal>
                    <a href="<%=add__url() %>" class="btn btn-green"> <em class="ficon ficon-tianjia"></em> ���</a>
				</div>
				<br />
				<p class="clear-right"></p>
			</div>
			<!--ɸѡ���� end-->
			
			<!--����table-->
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
			
			<!--����table  end-->
			<!--��������-->
            <div class="o-hidden padtb-20">
				<div class="fl full-sm">
				    <asp:Literal ID="Literal5" runat="server"></asp:Literal>
				</div>
			</div>
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
		 <script src="/inc/layer/layer.js"></script>
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

    if(t1!="")
    {
        var tiao_url = "auto_table_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>&type=del&id=" + t1;
        setajax(tiao_url, "yes");
    }

}

 </script> 
		</form>
	</body>
</html>
