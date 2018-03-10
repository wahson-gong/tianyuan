<%@ Page Language="C#" AutoEventWireup="true" CodeFile="guige_table.aspx.cs" Inherits="admin_Fields" %>
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
		
		<title>规格管理</title>
		<style type="text/css">
			html,body{
                background:transparent;
			}
           
		</style>
		
	</head>
	<body class="body-color">
      
	<!--选择分类-->
	<a href="javascript:parent.guige_add();" class="btn btn_border">添加规格分类</a>
	<br/><br/>
			<table class="table table-guige">
                <tr>
                    <th width="20%">规格标题</th>
                    <th width="60%">规格内容</th>
                    <th width="20%">操作</th>
                </tr>
                <asp:Repeater ID="Repeater1" runat="server">
                	
                 <ItemTemplate>
                     
						<tr>
							<th><%#DataBinder.Eval(Container.DataItem, "biaoti").ToString()%> &nbsp;&nbsp;</th>
							<td>
                                <%#set_guige_canshu(DataBinder.Eval(Container.DataItem, "id").ToString())%>
								
							</td>
                            <td>
                                <a href="javascript:parent.guige_edit(<%#DataBinder.Eval(Container.DataItem, "id").ToString()%>);" class="txt-blue">编辑</a>&nbsp;&nbsp;
                                <a href="javascript:parent.guige_del(<%#DataBinder.Eval(Container.DataItem, "id").ToString()%>);" class="txt-red">删除</a>
                            </td>
						</tr>
					</ItemTemplate>
                    </asp:Repeater>
						<tr>
							<td colspan="3">
                                <a href="javascript:jiazhai('add')" class="btn btn_border">确认操作</a>&nbsp;&nbsp;
								<a href="javascript:jiazhai('del')" class="btn btn_border red">清空重填</a>
							
							</td>
						</tr>
					</table>
					<!--选择分类  end-->
					<br /><br />
				<!--价格表-->
           <span id="neirong">
               <asp:Literal ID="Literal1" runat="server"></asp:Literal></span>
			<!--价格表 end-->
			
        <div class="fl full-sm">
				    <div class="checkbox checkbox-success fl" style="margin-bottom: 0;">
	                    <input type="checkbox" id="check-all" onclick="selcheck('content')">
	                    <label for="check-all">全选</label>
	                 </div>
	                 <div class="fl ft14">
	                 	&nbsp;&nbsp;
	                     <a href="javascript:chk1()" title="删除" class="operation gray"><span class="ficon ficon-qingkong"></span> 删除</a>
                         <!--<a href="javascript:chk2('')" title="修改" class="operation gray"><span class="ficon ficon-qiehuan"></span> 修改</a>-->
                       
	                 </div>
	                 <p class="clear"></p>
				</div>
        <br /><br />
			<!--修改价格-->
		<span id="biaodan"></span>
			<!--修改价格  end-->
		
		<!--<script src="js/bootstrap.min.js" type="text/javascript"></script>-->
		<script src="js/wow.min.js" type="text/javascript"></script>
		<script src="js/common.js" type="text/javascript"></script>
		 <script src="/inc/layer/layer.js"></script>
		
		<!--wow 初始化-->
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
     $.ajax({
                        type: "get",
                        url: "guige_table_add.aspx?Model_id=<%=jigege_model_id %>&type=del&id=" + t1,
                        success: function (data) {
                         //提交完成
                            $("#biaodan").html("");
                             $.ajax({
                        type: 'get',
                        url: 'guige_table.aspx?editname=chanpinjiage&Model_id=<%=Request.QueryString["Model_id"].ToString()%>&classid=<%=Request.QueryString["classid"].ToString()%>&laiyuanbianhao=<%=Request.QueryString["laiyuanbianhao"].ToString()%>&type=xiugai',
                        success: function (data) {
                          //  alert(data);
                            $("#neirong").html(data);
                        }
                    })
                            //提交完成
                        }
                    });

}
function chk2(type)
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
    set_canshu(t1, <%=jigege_model_id %>)
<%--window.location = "guige_table_add.aspx?Model_id=<%=jigege_model_id %>&type=edit&id=" + t1;
    window.event.returnValue = false;--%>
}



        </script> 
		<script>
			if (!(/msie [6|7|8|9]/i.test(navigator.userAgent))) {
				new WOW().init();
			};
		</script>
            <script type="text/javascript">
                function set_canshu(id, Model_id)
                {
                    $.ajax({
                        type: 'get',
                        url: 'guige_table_add.aspx?id=' + id + '&Model_id=' + Model_id,
                        success: function (data) {

                            $("#biaodan").html(data);
                        }
                    })
                }
                function xiugai(id)
                {  
                  
                    var formData = $("#fff2").serialize();
                   
                    $.ajax({
                        type: "POST",
                        url: "/Execution.aspx?type=edit_noyonghuming&t=guigejiage&tipurl_type=ajax&id="+id,
                        data: formData,
                        success: function (data) {
                          
                         //提交完成
                            $("#biaodan").html("");
                             $.ajax({
                        type: 'get',
                        url: 'guige_table.aspx?editname=chanpinjiage&Model_id=<%=Request.QueryString["Model_id"].ToString()%>&classid=<%=Request.QueryString["classid"].ToString()%>&laiyuanbianhao=<%=Request.QueryString["laiyuanbianhao"].ToString()%>&type=xiugai',
                        success: function (data) {
                          //  alert(data);
                            $("#neirong").html(data);
                        }
                    })
                            //提交完成
                        }
                    });
                    return false;
                    //reg完成
                }
                function jiazhai(type)
                {
                  //  alert(parent.document.getElementById("jiage").value);
                    var canshu = "";
                    var jiage="0";
                    if(parent.document.getElementById("jiage"))
                    {
                        jiage=parent.document.getElementById("jiage").value;
                    }

                    $("input[name='canshu']:checked").each(function () {
                       
                        if (canshu == "") {
                            canshu = $(this).attr('value');
                        }
                        else {
                            canshu = canshu + "," + $(this).attr('value');
                        }
                        
                    })
        
                    if(type=="del")
                    {
                       

                     $.ajax({
                        type: 'get',
                        url: 'guige_table.aspx?editname=chanpinjiage&Model_id=<%=Request.QueryString["Model_id"].ToString()%>&classid=<%=Request.QueryString["classid"].ToString()%>&laiyuanbianhao=<%=Request.QueryString["laiyuanbianhao"].ToString()%>&canshu=' + canshu + '&jiage='+jiage+'&type=' + type,
                        success: function (data) {

                            $("#neirong").html(data);
                        }
                    })
                    }
                    if (canshu != "")
                    {
                         $.ajax({
                        type: 'get',
                        url: 'guige_table.aspx?editname=chanpinjiage&Model_id=<%=Request.QueryString["Model_id"].ToString()%>&classid=<%=Request.QueryString["classid"].ToString()%>&laiyuanbianhao=<%=Request.QueryString["laiyuanbianhao"].ToString()%>&canshu=' + canshu + '&jiage='+jiage+'&type=' + type,
                        success: function (data) {
                           
                            $("#neirong").html(data);
                        }
                    })
                        //end
                    }
                   
                }
            </script>
             
	
	</body>
</html>
