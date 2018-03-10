<%@ Control Language="C#" AutoEventWireup="true" CodeFile="top.ascx.cs" Inherits="ascx_top" %>
<%@ Register src="../yanz.ascx" tagname="yanz" tagprefix="uc1" %>
<header>
			<div class="logo">
			    <a href="default.aspx"><img src="images/logo.png" height="100%"/></a>
			</div>
			<span class="ficon ficon-menu app-ficon-menu"></span>
			<div class="top-nav">
				<div class="nav-right">
					<!--通知列表-->
				    <ul class="notice-list">
                        <li>
				    		<a href="/" class="drop-tag" title="返回首页" target="_blank">
				    		<span class="ficon ficon-shouye"></span>
				    		</a>
				    	</li>
				    	<li id="shuju_li">
				    		<a href="#" class="drop-tag">
    <span class="ficon ficon-gouwuche"></span>
    <em class="notice-num" id="cartcount">0</em>
</a>
<div class="drop-list" id="shuju">
   
</div>
				    	</li>
				    	<li id="smsshuju_li">
				    		<a href="sms_table.aspx?Model_id=72&u1=管理员&leixing=未读" class="drop-tag">
				    		<span class="ficon ficon-duanxin"></span>
				    		<em class="notice-num" id="smsshuju">0</em>
				    		</a>
				    	</li>
				    	<li class="top-user-info">
				    		<a href="#" class="drop-tag">
				    		<img src="<%=touxiang %>" class="user-img"  onerror='this.src="images/touxiang.jpg"'/>&nbsp;&nbsp;<span class="user-name"><%=my_b.k_cookie("admin_id") %> </span> 
				    		</a>
				    		<div class="drop-list">
				    		    <dl>
				    		    	<dd><a href="#"><em class="ficon ficon-yonghuzhongxin"></em> 用户中心</a></dd>
				    		    	<dd><a href="get_password.aspx"><em class="ficon ficon-shezhi"></em> 修改密码</a></dd>
				    		    	<dd><a href="Columns_Table.aspx"><em class="ficon ficon-lanmu"></em> 栏目设置</a></dd>
				    		    	<dd><a href="default.aspx?type=loginout"><em class="ficon  ficon-tuichu"></em> 退出</a></dd>
				    		    </dl>
				    		</div>
				    	</li>
				    </ul>
				</div>
			    <span class="ficon ficon-menu"></span>
			</div>
		</header>
 <script type="text/javascript">
                    
                    var timer;
                    function intopwd() {
                        $.ajax({
                            type: 'get',
                            url: 'single.aspx?m=temp/shuju',
                            success: function (data) {
                                if (data == "") {
                                    $("#shuju_li").remove();
                                }
                                else {
                                    $("#shuju").html(data);
                                }
                            
                            }
                        })

                        $.ajax({
                            type: 'get',
                            url: 'auto_load.aspx',
                            success: function (data) {

                               // $("#shuju").html(data);

                            }
                        })
                       
                        $.ajax({
                            type: 'get',
                            url: 'single.aspx?m=temp/sms',
                            success: function (data) {
                                if (data == "") {
                                    $("#smsshuju_li").remove();
                                }
                                else {
                                      $("#smsshuju").html(data);
                                }
                           
                            }
                        })
                    }
                    function aa() {
                        timer = setInterval("intopwd()", 60000);
                    }
                    aa();
                    intopwd();
                </script>
<uc1:yanz ID="yanz1" runat="server" />
