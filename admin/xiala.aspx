<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xiala.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
 <script src="js/jquery-1.11.2.min.js" type="text/javascript" charset="utf-8"></script>
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
                            $ul.append('<li><input type="checkbox" value="' + jsonData.k + '" /><span>' + jsonData.v + '</span></li>');
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
                            $(this).find("input[class!='selectAll']").attr("checked","checked");
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
        
        $(document).ready(function () {
            $("#txtTest").MultDropList({ data: $("#hfddlList").val() });
            $("#txtTest1").MultDropList({ data: $("#hfddlList").val() });
        });
        
        
		</script>
		
		<style type="text/css">
		.box{
			float: left;
			margin-right: 10px;
		}
			 .wraper
        {
        }
        
        .list
        {
           min-width: 160px;
           _width:160px;
            overflow: auto;
            position: absolute;
            border: 1px solid #ddd;
            display: none;
            float: left;
            z-index: 1000000;
            background: #fff;
        }
        .list ul li
        {
            padding-left: 10px;
            padding-top: 4px;
            padding-bottom: 4px;
            border-bottom: 1px solid #ddd;
            line-height: 24px;
        }

ul
{
    list-style:none outside none;
    padding: 10px;
    margin: 0;
    }
    .showtext{
    	height: 26px;
    	border: 1px solid #ddd;
    	outline: none;
    	width: 160px;
    }
		</style>
</head>
<body>
    <form id="form1" runat="server">
 <input type="hidden" id="hfddlList" value='{k:1,v:"南京"}|{k:2,v:"上海"}|{k:3,v:"扬州"}|{k:4,v:"苏州"}|{k:5,v:"无锡"}|{k:6,v:"常州"}|{k:7,v:"盐城"}|{k:8,v:"徐州"}|{k:9,v:"泰州"}|{k:10,v:"淮安"}' />

        <div class="testContainer">
           
            <div class="box">
                <input type="text" name="txtTest" id="txtTest" class="showtext"/>
                <input type="hidden" name="hfTest" id="hfTest" />
            </div>
            <div class="box">
                <input type="text" name="txtTest1" id="txtTest1" class="showtext"/>
                <input type="hidden" name="hfTest" id="hfTest1" />
            </div>
            
        </div>
    </form>
</body>
</html>
