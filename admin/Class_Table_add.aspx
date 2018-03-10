<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Class_Table_add.aspx.cs" Inherits="admin_Class_Table_add" ValidateRequest="false" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<%@ Register Src="ascx/FreeTextBox.ascx" TagName="FreeTextBox" TagPrefix="uc3" %>
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
              <script src="js/jquery.min.js" type="text/javascript" charset="utf-8"></script>
		<script src="js/jquery.SuperSlide.2.1.1.js" type="text/javascript" charset="utf-8"></script>
	<script type="text/javascript" src="ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="ueditor/ueditor.all.js"></script>
    <script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
    <script src="color/color.js"></script>
      <script type="text/javascript">
          (function ($) {
              $.fn.extend({
                  MultDropList: function (options) {
                      var op = $.extend({ wraperClass: "wraper", width: "", height: "", data: "", selected: "" }, options);
                      return this.each(function () {
                          var $this = $(this); //指向TextBox
                          var $hf = $(this).next(); //指向隐藏控件存
                          var conSelector = "#" + $this.attr("id") + ",#" + $hf.attr("id");
                          var $wraper = $(conSelector).wrapAll("<div><div></div></div>").parent().parent().addClass(op.wraperClass);

                          var $list = $('<div class="list"></div>').appendTo($wraper);
                          $list.css({ "width": op.width, "height": op.height });
                          //控制弹出页面的显示与隐藏
                          $this.click(function (e) {
                              $(".list").hide();
                              $list.toggle();
                              e.stopPropagation();
                          });

                          $(document).click(function () {
                              $list.hide();
                          });

                          $list.filter("*").click(function (e) {
                              e.stopPropagation();
                          });
                          //加载默认数据

                          $list.append('<ul></ul>');
                          var $ul = $list.find("ul");

                          //加载json数据
                          var listArr = op.data.split("|");

                          var jsonData;
                          for (var i = 0; i < listArr.length; i++) {
                              jsonData = eval("(" + listArr[i] + ")");

                              var reg = jsonData.v.split("—");
                              var kongge = "";
                              for (var j = 0; j < reg.length; j++) {
                                  kongge = kongge + "&nbsp;&nbsp;&nbsp;";
                              }
                              $ul.append('<li>' + kongge + '<input type="checkbox" value="' + jsonData.k + '" /><span>' + jsonData.v.replace("—", "") + '</span></li>');
                          }

                          //加载勾选项
                          var seledArr;
                          if (op.selected.length > 0) {
                              seledArr = selected.split(",");
                          }
                          else {
                              seledArr = $hf.val().split(",");
                          }

                          $.each(seledArr, function (index) {
                              $("li input[value='" + seledArr[index] + "']", $ul).attr("checked", "checked");

                              var vArr = new Array();
                              $("input[class!='selectAll']:checked", $ul).each(function (index) {
                                  vArr[index] = $(this).next().text();
                              });
                              $this.val(vArr.join(","));
                          });

                          //点击其它复选框时，更新隐藏控件值,文本框的值
                          $("li", $ul).click(function () {
                              var kArr = new Array();
                              var vArr = new Array();
                              //                          $(this).find("input[class!='selectAll']").attr("checked","checked");
                              $("input[class!='selectAll']:checked", $ul).each(function (index) {

                                  kArr[index] = $(this).val();
                                  vArr[index] = $(this).next().text();

                              });
                              $hf.val(kArr.join(","));
                              $this.val(vArr.join(","));
                          });
                      });
                  }
              });
          })(jQuery);
		</script>
		<!--[if lt IE 8]>
			<link rel="stylesheet" href="fonts/css/fontello-ie7.css">
		<![endif]-->
		<script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
		<!--[if lt IE 9]>
		      <script src="js/html5shiv.min.js"></script>
		      <script src="js/respond.min.js"></script>
		    <![endif]-->
		
		<title>编辑管理</title>
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
			    &nbsp;>&nbsp;<a href="Class_Table.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>">内容管理</a> &nbsp;>&nbsp;<asp:Literal ID="Literal1" runat="server"></asp:Literal>
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<table class="table table-info cs-table">
				<tr>
					<th  style="width:60px">上级栏目</th>
					<td>
						<asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged"  CssClass="select-container1">
            </asp:DropDownList><span id="RequiredFieldValidator1" style="color:Red;">*</span>
					</td>
				</tr>
				<tr>
					<th>列表标题</th>
					<td>
						<asp:TextBox ID="TextBox1" runat="server" MaxLength="100"
              CssClass="input"></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox1"
              ErrorMessage="列表名称不可以为空"></asp:RequiredFieldValidator>
					</td>
				</tr>
             <tr>
					<th>栏目图标</th>
					<td>
						 <asp:TextBox ID="TextBox2" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
             &nbsp;
                        <a href="/inc/webuploader/FileUploader.aspx?type=img&editname=TextBox2" target="_blank" class="btn btn-blue"><em class="ficon  ficon-uploading"></em> 上传图片</a>
                        
					</td>
				</tr>
                	<tr>
					<th>SEO标题</th>
					<td>
						<asp:TextBox ID="TextBox9" runat="server" MaxLength="100"
              CssClass="input"></asp:TextBox>
					</td>
				</tr>
                <tr>
					<th>栏目关键字</th>
					<td>
						<asp:TextBox ID="TextBox3" runat="server" CssClass="txtinput" TextMode="MultiLine"></asp:TextBox>
					</td>
				</tr>
                <tr>
					<th>栏目描述</th>
					<td>
                   <asp:TextBox ID="TextBox4" runat="server" CssClass="txtinput" TextMode="MultiLine" ></asp:TextBox>
					</td>
				</tr>
                <tr>
					<th>列表页模版</th>
					<td>
						<asp:DropDownList ID="DropDownList1" runat="server"  CssClass="select-container1">
          </asp:DropDownList>
					</td>
				</tr>
                <tr>
					<th>介绍页模板</th>
					<td>
					<asp:DropDownList ID="DropDownList2" runat="server"  CssClass="select-container1">
          </asp:DropDownList>
					</td>
				</tr>
                <tr>
					<th>栏目路径</th>
					<td>
					<asp:TextBox ID="TextBox5" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>&nbsp;&nbsp;
                        <asp:LinkButton ID="LinkButton3" CssClass="btn btn-green" runat="server" OnClick="LinkButton3_Click"><em class="ficon ficon-zhuanhuan"></em> 转换拼音</asp:LinkButton>
                       
					</td>
				</tr>
                 <tr>
					<th>缩略图大小</th>
					<td>
					<asp:TextBox ID="TextBox6" runat="server" MaxLength="100" CssClass="dinput" Text="120*80"></asp:TextBox>&nbsp;&nbsp;例：120*80
					</td>
				</tr>
                 <tr>
					<th>栏目简介</th>
					<td>
						 <uc3:FreeTextBox ID="FreeTextBox1" runat="server" Height="400" upfile="admin" Width="95%" />
					</td>
				</tr>
                 <tr>
					<th>内容图片大小</th>
					<td>
					<asp:TextBox ID="TextBox7" runat="server" MaxLength="100" CssClass="dinput" Text="800*0"></asp:TextBox>&nbsp;&nbsp;例：800*0，一般限制宽度
					</td>
				</tr>
                 <tr>
					<th>排序</th>
					<td>
						<asp:TextBox ID="TextBox8" runat="server" MaxLength="100" CssClass="dinput" Text="0"></asp:TextBox>&nbsp;&nbsp;例：0
					</td>
				</tr>
                <tr>
					<th>主栏目</th>
					<td>
						<div class="checkbox checkbox-inline checkbox-info">
                           <asp:CheckBox ID="CheckBox1" runat="server"/>
                            <label for="CheckBox1">主栏目</label>
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
