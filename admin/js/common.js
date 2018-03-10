$(function() {
	//下拉菜单
	$(".drop-tag").click(function(e) {
		e.stopPropagation();
		var droplist = $(this).siblings(".drop-list");
		$(".drop-list").not(droplist).hide();
		droplist.slideToggle();
	})

	$(document).click(function(e) {
		$(".drop-list").slideUp();
//		$(".side-nav.collapse .sub").hide();
		$(".side-collapse .sub").hide();
	})

	//侧边栏菜单折叠
	$(".side-nav").find(".nLi h3").click(function(e) {
		//		$(".side-nav-ul .nLi h3").removeClass("down");
		e.stopPropagation();
		var sub = $(this).siblings(".sub");
		var _this = $(this);
		if ($(this).parents(".side-nav").hasClass("side-collapse")) {
			$(".side-nav .sub").not(sub).hide(300);
			sub.fadeToggle(300);
			$(".side-nav .nLi h3").not(_this).removeClass("on");
			$(this).toggleClass("on");
		} else {
			$(".side-nav .sub").not(sub).slideUp(300);
			sub.slideToggle(300);
			$(".side-nav .nLi h3").not(_this).removeClass("on");
			$(this).toggleClass("on");
		}

	});
    

	$(".side-nav-ul .sub").each(function () {
	    if ($(this).find("a.on").length > 0) {
	        $(this).parents(".nLi").addClass("on");
	    }
	})

	//控制侧边栏折叠
	$(".top-nav .ficon-menu").click(function(e) {
		e.stopPropagation();
		$(".side-nav").toggleClass("side-collapse");
		$("header").toggleClass("header-collapse");
		$(".body-content").toggleClass("content-collapse");

	})

	$(".app-ficon-menu").click(function() {
		$(".side-nav").toggleClass("app-side");
		$(".body-content").toggleClass("app-content");

	})

	
	
})


function setSideHei(){
	if($(window).width()>640){
		
		var hei = $(document).height() - $("header").height();
		var selfHei = $(".side-nav").height();
		var newHei = hei>selfHei?hei:selfHei;
		 $(".side-nav").css({"min-height":newHei,"_height":newHei});
//		 alert(newHei);
	}
}
	

$(window).load(function(){
	setSideHei();
})
$(window).resize(function(){
	setSideHei();
	if($(window).width()<=640){
		$(".side-nav").removeClass("side-collapse");
		$("header").removeClass("header-collapse");
		$(".body-content").removeClass("content-collapse");
	}
})

//进度条
function showProgressbar(div) {
	var showTag = div.attr("data-show");
	if (showTag == "hidden") {
		div.find(".progress").each(function() {
			var len = $(this).attr("data-width");
			$(this).animate({
				width: len + '%'
			}, 1000);
		})
		div.attr("data-show", "show");
	}

}

$(window).load(function() {
	var tag = $(".progressbar_container");
	var targetTop;

	function jadgeshow() {
		tag.each(function(i) {
			targetTop = $(this).offset().top - $(window).height();
			if ($(document).scrollTop() > targetTop) {

				showProgressbar($(this));
			}
		})
	}
	jadgeshow();
	$(window).scroll(function() {
		jadgeshow();
	})
})

//进度条 end


//模拟select
;(function($){  
  
    jQuery.fn.select = function(options){  
        return this.each(function(){  
            var $this = $(this);  
            var $shows = $this.find(".shows");  
            var $selectOption = $this.find(".selectOption");  
            var $el = $this.find("ul > li");  
                                      
            $this.click(function(e){  
                $(this).toggleClass("zIndex");  
                $(this).children("ul").toggleClass("dis");  
                e.stopPropagation();  
            });  
              
            $el.bind("click",function(){  
                var $this_ = $(this);  
                   
                $this.find("span").removeClass("gray");  
                $this_.parent().parent().find(".selectOption").text($this_.text());  
            });  
              
            $("body").bind("click",function(){  
                $this.removeClass("zIndex");  
                $this.find("ul").removeClass("dis");      
            })  
              
        //eahc End    
        });  
          
    }  
      
})(jQuery);

$(function(){
	$(".select-container").select();  
})

//模拟select end



//控制css
$(function(){
	$(".table-hover tbody tr").hover(function(){
		$(this).addClass("bgcolor");
	},function(){
		$(this).removeClass("bgcolor");
	});
	
	
	$(".add-input").click(function (e) {
	    e.stopPropagation();
	    var par = $(this).parents("tr");
	    par.addClass("on").siblings("tr").removeClass("on");
	});
	$(".add-input").each(function () {
	    if ($(this).val() != '') {
	        $(this).siblings(".add-input-txt").hide();
	    } else {
	        $(this).siblings(".add-input-txt").show();
	    }

	});
	$(".add-input").blur(function () {
	    if ($(this).val() != '') {
	        $(this).siblings(".add-input-txt").hide();
	    } else {
	        $(this).siblings(".add-input-txt").show();
	    }
	})
	$(document).click(function(){
		$(".add-table tr").removeClass("on");
	})
})



//回车输入关键词

$(function(){
	$(".enterwords-box").each(function(){
		var box = $(this);
		var enterinput = box.find(".enterinput").eq(0);
		box.click(function(e){
			e.stopPropagation();
			box.addClass("on");
			enterinput.focus();
		});
		$(document).click(function(){
			box.removeClass("on");
		});
		
		var wordValue='';
		var oldlen = 0;
		enterinput.keyup(function(event){
			 wordValue = $.trim(enterinput.val());
			 if(event.keyCode!=8){
			 	 oldlen = wordValue.length;
			 }
			if(event.keyCode==13){ 
				if(wordValue.length>40){
					alert("请输入内容小于20个字");
				}else if(wordValue.length>0){
					enterinput.before("<span class=\"wordspan\"><em class=\"word\">"+ wordValue +"</em><em class=\"remove ficon ficon-cancel\"></em></span>").val('');
				}
   	   		} 
   	   		if(event.keyCode==8){
   	   			if(oldlen>-1){
   	   				oldlen = oldlen -1;
   	   			}
   	   			if(oldlen==-1){
   	   			enterinput.prev(".wordspan").remove();
   	   			}
   	   		}
		});
		
		
	})
	
//	$(document).on("click",".wordspan .remove",function(e){
//		e.stopPropagation();
//		$(this).parents(".wordspan").remove();
//	})
	

})

function selcheck(id) {
    var e = document.getElementById(id);
    var objs = e.getElementsByTagName("input");
    if (e.checked) {
        e.checked = false;
        for (var i = 0; i < objs.length; i++) {
            if (objs[i].type.toLowerCase() == "checkbox")
                objs[i].checked = false;
        }
    } else {
        e.checked = true;
        for (var i = 0; i < objs.length; i++) {
            if (objs[i].type.toLowerCase() == "checkbox")
                objs[i].checked = true;
        }
    }
}

function tijiao() {
    if (document.getElementById("fenlei").value == "") {
        alert("请选择分类");
        document.getElementById("fenlei_Test").focus();
        return false;
    }
}

function pic_cut(id, cc) {
    window.open("pic_cut.aspx?editname=" + id + "&pic_name=" + document.getElementById(id).value + "&cc=" + cc + "", "", "status=no,scrollbars=no,top=20,left=110,width=740,height=340");
}




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
                    $(this).find("input[class!='selectAll']").attr("checked", "checked");
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

//图片大小自动脚本
function AutoResizeImage(maxWidth, maxHeight, objImg) {
    var img = new Image();
    img.src = objImg.src;
    var hRatio;
    var wRatio;
    var Ratio = 1;
    var w = img.width;
    var h = img.height;
    wRatio = maxWidth / w;
    hRatio = maxHeight / h;
    if (maxWidth == 0 && maxHeight == 0) {
        Ratio = 1;
    } else if (maxWidth == 0) { //
        if (hRatio < 1) Ratio = hRatio;
    } else if (maxHeight == 0) {
        if (wRatio < 1) Ratio = wRatio;
    } else if (wRatio < 1 || hRatio < 1) {
        Ratio = (wRatio <= hRatio ? wRatio : hRatio);
    }
    if (Ratio < 1) {
        w = w * Ratio;
        h = h * Ratio;
    }
    objImg.height = h;
    objImg.width = w;
}


				function dyniframesize(down) { 
				    var pTar = null; 
				    if (document.getElementById){ 
				        pTar = document.getElementById(down); 
				    } 
				    else{ 
				        eval('pTar = ' + down + ';'); 
				    } 
				    if (pTar && !window.opera){ 
				        //begin resizing iframe 
				        pTar.style.display="block" 
				        if (pTar.contentDocument && pTar.contentDocument.body.offsetHeight){ 
				            //ns6 syntax 
				            pTar.height = pTar.contentDocument.body.offsetHeight ; 
				            pTar.width = pTar.contentDocument.body.scrollWidth; 
				        } 
				        else if (pTar.Document && pTar.Document.body.scrollHeight){ 
				            //ie5+ syntax 
				            pTar.height = pTar.Document.body.scrollHeight; 
				            pTar.width = pTar.Document.body.scrollWidth; 
				        } 
				    } 
				} 


				function tupian(editname, imgurl) {
				
				    document.getElementById(editname).value = imgurl;
				    if (document.getElementById(editname + "_show"))
				    {
				        document.getElementById(editname + "_show").src = imgurl;
				    }
				   // get_video_img(imgurl);
				}

				//function get_video_img(imgurl)
				//{
				//    $.ajax({
				//        type: "get",
				//        url: "/inc/tool/Comment.aspx?fileurl=" + imgurl + "",
				//        success: function (data) {
				//            alert(data)
				//        }
				//    });
//}
//列表自动编辑
				function list_Fields(id, Fields, Model_id)
				{
				    $.ajax({
				        type: 'get',
				        url: "list_edit.aspx?id=" + id + "&Fields=" + Fields + "&zhi=" + $("#" + Fields+id).val() + "&Model_id=" + Model_id + "",
				        success: function (data) {
				            if (data == "ok")
				            {
				               // layer.alert("修改成功");
				                location.reload();
				            }
				        }
				    })
				}
//执行ajax
				function setajax(url,tiaozhuan)
				{
				    $.ajax({
				        type: 'get',
				        url: url+"&tiaozhuan=ajax",
				        success: function (data) {
				            if (tiaozhuan == "yes") {
				                layer.alert(data, function () {
				                    self.location.reload();

				                });
				            }
				            else {
				                layer.alert(data);
				            }
				           
				        }
				})
				}

				function setCookie(c_name, value, expiredays) {
				    var exdate = new Date()
				    exdate.setDate(exdate.getDate() + expiredays)
				    document.cookie = c_name + "=" + escape(value) +
                    ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString())
				}

				function getCookie(c_name) {
				    if (document.cookie.length > 0) {
				        c_start = document.cookie.indexOf(c_name + "=")
				        if (c_start != -1) {
				            c_start = c_start + c_name.length + 1
				            c_end = document.cookie.indexOf(";", c_start)
				            if (c_end == -1) c_end = document.cookie.length
				            return unescape(document.cookie.substring(c_start, c_end))
				        }
				    }
				    return ""
				}
				

				

				function readNavOn() {
				    var url = getCookie('linkurl');
				    var tagA = $(".side-nav-ul a[href='" + url + "']");
				    tagA.addClass("on").parents(".nLi").addClass("on");
				   
				}

				$(function () {
				    $(".side-nav-ul a").on('click', function () { //侧边导航点击时 写cookie
				        var url = $(this).attr("href");
				        console.log(url);
				        if (url != '' && url != '#' && url != 'javascript:;') {
				            setCookie('linkurl', url, 11);
				        }
				    });
				    readNavOn(); //加载页面时读cookie

				})



function guige_edit(id){
	layer.open({
	  type: 2,
	  title:"编辑规格",
	  area: ['700px', '450px'],
	  fixed: false, //不固定
	  maxmin: true,
	  content: 'single.aspx?m=guige_edit&id=' +id,
	  cancel: function(){ 
	    $('#guige_').attr('src', $('#guige_').attr('src'));
	  }
	});
}
function guige_add(){
	layer.open({
	  type: 2,
	  title:"新增规格",
	  area: ['700px', '350px'],
	  fixed: false, //不固定
	  maxmin: true,
	  content: 'single.aspx?m=guige_add',
	  cancel: function(){ 
	    //window.location.reload();
	  }
	});
	
}

function guige_del(id){
	var lay = layer.confirm('确认删除该规格分类？', {
		  btn: ['取消','确认'] //按钮
		}, function(){
		  	layer.close(lay);
		}, function(){
		  
			$.ajax({
			type:"get",
			url:"/Execution.aspx?t=guigecanshu&type=del_noyonghuming&id=" + id,
			async:false,
			success:function(data){
				//parent.window.location.reload();
				//parent.layer.close(index);
				 $('#guige_').attr('src', $('#guige_').attr('src'));
			}
			});
		  
		  
		});
}

function layer_open(src,title){
	parent.layer.open({
	  type: 2,
	  title:title,
	  area: ['700px', '450px'],
	  fixed: false, //不固定
	  maxmin: true,
	  content: src,
	  cancel: function(){ 
	    //window.location.reload();
	  }
	});
}

function loaddaan(id){
	id = id + "_";
	$('#'+id).attr('src', $('#'+id).attr('src'));
	layer.closeAll();
}
function reloadiframe(id){
	$('#'+id).attr('src', $('#'+id).attr('src'));
}
