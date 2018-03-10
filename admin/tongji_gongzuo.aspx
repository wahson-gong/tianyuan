<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tongji_gongzuo.aspx.cs" Inherits="admin_Ad_Table" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>工作量统计</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
     <script src="js/jquery.min.js" type="text/javascript" charset="utf-8"></script>
		<script src="js/jquery.SuperSlide.2.1.1.js" type="text/javascript" charset="utf-8"></script>

     <link rel="stylesheet" type="text/css" href="/inc/chart/css/jquery.jqChart.css" />
    <link rel="stylesheet" type="text/css" href="/inc/chart/css/jquery.jqRangeSlider.css" />
    <link rel="stylesheet" type="text/css" href="/inc/chart/themes/smoothness/jquery-ui-1.10.4.css" />
    <script src="/inc/chart/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/inc/chart/js/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="/inc/chart/js/jquery.jqChart.min.js" type="text/javascript"></script>
    <script src="/inc/chart/js/jquery.jqRangeSlider.min.js" type="text/javascript"></script>
    <!--[if IE]><script lang="javascript" type="text/javascript" src="/inc/chart/js/excanvas.js"></script><![endif]-->
</head>
<body>
<form id="form1" runat="server">
<script language="javascript" type="text/javascript"> 
function selcheck(id) {
   var e =document.getElementById(id);
   var objs = e.getElementsByTagName("input");
   if (e.checked){
      e.checked = false;
      for(var i=0; i<objs.length; i++) {
         if(objs[i].type.toLowerCase() == "checkbox" )
         objs[i].checked = false;
      }
   }   else   {
      e.checked = true;
      for(var i=0; i<objs.length; i++) {
         if(objs[i].type.toLowerCase() == "checkbox" )
         objs[i].checked = true;
      }
   }
}
</script>

     <div id="frame_Toolbar" class="toolbarBg"> 
		<div style="float:left">
            <uc1:yanz ID="Yanz1" runat="server" />
            <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a><a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a>&nbsp;
         <span class="split ftSplit" ></span></div>
	    <div id="guide"> 您现在的位置：统计分析 >> 工作量统计</div>
		
	    <div class="pageInfo right light_gray"> <asp:TextBox ID="TextBox1" runat="server" Width="100px"></asp:TextBox>&nbsp;<asp:Button ID="Button1" runat="server" CssClass="btn" OnClick="Button1_Click" Text="查找" /></div>
		
    </div>
	<div id="content">
        <div id="jqChart" style="width: 95%; height: 300px; padding:20px 20px 20px 20px; margin-left:auto; margin-right:auto">
        </div>
        <script lang="javascript" type="text/javascript">
        var model = [
            <%=model%>
        ];

        $(document).ready(function () {
            $('#jqChart').jqChart({
                title: { text: '栏目信息统计' },
                animation: { duration: 1 },
                shadows: {
                    enabled: true
                },
                dataSource: model,
                series: [
                    {
                        type: 'column',
                        xValuesField: {
                            name: 'date',
                            type: 'string' // string, numeric, dateTime
                        },
                        yValuesField: 'value'
                    }
                ]
            });
        });
    </script>
     <table class="cTable table tCenter">
	    <tr class="cTitle toolbarBg" id="Title">
		
		    <td width="20%"><span class="split ctSplit"></span><div>添加人 </div></td>
		    <td><span class="split ctSplit"></span><div>信息量 </div></td>
		    <td width="20%"><span class="split ctSplit"></span><div>最新上传时间</div></td>
			
	    </tr>
	 </table>
	<table class="cTable table tCenter" cellspacing="0" border="0" style="border-collapse:collapse;">
	     <asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
		<tr>
		
			
			<td style="width:20%;"><%#DataBinder.Eval(Container.DataItem, "yonghuming").ToString()%></td>
			<td style=" text-align:left"><%#my_b.get_count("sl_wenzhang","where yonghuming='"+DataBinder.Eval(Container.DataItem, "yonghuming").ToString()+"'")%></td>
			<td style="width:20%;"><%#DataBinder.Eval(Container.DataItem, "dtime", "{0:yyyy-MM-dd}")%></td>

		
		</tr>
		</ItemTemplate>
                 </asp:Repeater>

	</table> 
    <div id="cEndToolbar" class="toolbarBg">
        <div class="pageList">
		<asp:Literal ID="Literal1" runat="server"></asp:Literal>
		
	    </div>
	   
	</div>	
    </div>
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
   // window.location="Ad_Table_add.aspx?type=del&id="+t1;
var tiao_url = "Ad_Table_add.aspx?type=del&id="+t1;
             window.location.href = tiao_url;
        window.event.returnValue = false;
}
//注意：下面的代码是放在/Poster/rightgp调用
$(window.parent.document).find("#main_data").load(function () {
    var main = $(window.parent.document).find("#main_data");
    var mainheight = $(document).height() + 30;
    main.height(mainheight);
});
        </script> 
</form>

</body>
</html>


