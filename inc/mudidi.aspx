<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mudidi.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
  <link rel="stylesheet" href="/admin/assets/css/amazeui.min.css">
  <link rel="stylesheet" href="/admin/assets/css/app.css">
<style type="text/css">
	.am-selected{float:left;margin-right:5px;}
	.am-btn{font-size:12px;}
	.am-selected-list{font-size:12px;}
</style>
     <script src="/admin/js/jquery.min.js" type="text/javascript" charset="utf-8"></script>
		<script src="/admin/js/jquery.SuperSlide.2.1.1.js" type="text/javascript" charset="utf-8"></script>
</head>
<body>
    <form id="form1" runat="server">
    
  <div class="next">
      <asp:Literal ID="Literal1" runat="server"></asp:Literal>

 
</div>

<!--[if lt IE 9]>
<script src="http://libs.baidu.com/jquery/1.11.1/jquery.min.js" charset="utf-8"></script>
<script src="http://cdn.staticfile.org/modernizr/2.8.3/modernizr.js" charset="utf-8"></script>
<script src="assets/js/polyfill/rem.min.js" charset="utf-8"></script>
<script src="assets/js/polyfill/respond.min.js" charset="utf-8"></script>
<script src="assets/js/amazeui.legacy.js" charset="utf-8"></script>
<![endif]-->
<!--[if (gte IE 9)|!(IE)]><!-->
	<script src="http://libs.baidu.com/jquery/1.7.2/jquery.min.js"></script>
<script src="/admin/assets/js/amazeui.min.js" charset="utf-8"></script>
<!--<![endif]-->
<script type="text/javascript">
var id=0;

$(function () {
    $('#main').selected();
    var rom = 0;
    $('.end').live('change', function () {
        if ($(this).val() != null) {
            index = $(this).index('.end') + 1;
            if (index == 0) {
                index = 1;
            }
            console.log(index + 1);
            $('.end').slice(index, 999).remove();
            $('.am-selected').slice(index, 999).remove();
            $.ajax({
                type: "GET",
                url: "mudidi.aspx?type=view&classid=" + $(this).val(),
                success: function (msg) {
                    msg = msg.split('{view}');
                    rom++;
                    if (msg[0] != "") {
                        $('.next').append('<select multiple class=\"end\" id=\"rr' + rom + '\">' + msg[0] + '</select>');
                        $('#rr' + rom).selected();
                    }
                }
            })
        } else {
            index = $(this).index('.end') + 1;
            if (index == 0) {
                index = 1;
            }
            $('.end').slice(index, 999).remove();
            $('.am-selected').slice(index, 999).remove();
        }
        var str = "";
        $('.end').each(function () {
            if ($(this).val() != null) {
                if ($(this).index('.end') == 0) {
                    str = $(this).val();
                } else {
                    str = str + ',' + $(this).val();
                }
            }
        })
        window.parent.document.all('<%=editname %>').value = str;
        window.parent.setchanpin(str);
         
    })

})
    var mainheight_ = "";
    function setheight() {

   var main = $(window.parent.document).find("#<%=editname %>_");
   var mainheight = $(document).height();
 
   if (parseInt($(document).height()) != parseInt(mainheight_))
   {
       //alert(mainheight_ + "|" + $(document).height());
       mainheight_ = mainheight;
       main.height(mainheight);
   }
}

var xunhuan = setInterval("setheight()", 1000);

//  $(function () {
//      $('#main').change(function () {
//          var duixiang = $(this);
//          id=0;
//          $.ajax({
//              type: "GET",
//              url: "mudidi.aspx?type=view&classid=" + $(this).val(),
//              success: function (msg) {
//              		id++;
//              		$('.am-selected').slice(1,999).remove();
//              		$('.end').remove();
//      $('.next').append('<select multiple id=\"Select'+id+'\" title=\"'+id+'\" class=\"end\"></select>');
//                  msg = msg.split("{view}");
//                  $("#Select"+id).html(msg[0]).selected().bind('change',function(){
//                  	  if ($(this).next().next().attr('id')==undefined){
//                  	  	$.ajax({
//			           type: "GET",
//			            url: "mudidi.aspx?type=view&classid=" + $(this).val(),
//			           success: function (msg) {
//			          id++;
//			          $('.next').append('<select multiple id=\"Select'+id+'\" title=\"'+id+'\" class=\"end\"></select>');
//			                    msg = msg.split("{view}");
//			         $("#Select"+id).html(msg[0]).selected().bind('change',function(){
//                  	  if ($(this).next().next().attr('id')==undefined){
//                  	  	$.ajax({
//			           type: "GET",
//			            url: "mudidi.aspx?type=view&classid=" + $(this).val(),
//			           success: function (msg) {
//			          id++;
//			          $('.next').append('<select multiple id=\"Select'+id+'\" title=\"'+id+'\" class=\"end\"></select>');
//			                    msg = msg.split("{view}");
//			                    $("#Select"+id).html(msg[0]).selected().bind('change',function(){
//			                    	
//			                    });
//			                    }
//			              });
//                  	  }else{
//                  	 		if ($(this).val()==null){
//			                 $("#Select"+id).html('').selected();
//                  	  	}else{
//                  	  		$.ajax({
//				           type: "GET",
//				            url: "mudidi.aspx?type=view&classid=" + $(this).val(),
//				           success: function (msg) {
//			                    msg = msg.split("{view}");
//			                    $("#Select"+id).next().remove();
//			                    $("#Select"+id).remove();
//			                   $('.next').append('<select multiple id=\"Select'+id+'\" title=\"'+id+'\" class=\"end\">'+msg[0]+'</select>');
//			                    $("#Select"+id).selected();
//			                    }
//			              });
//                  	  	}
//                  	  };
//                  
//			                    });
//			                    }
//			              });
//                  	  }else{
//                  	 		if ($(this).val()==null){
//			                 $("#Select"+id).html('').selected();
//                  	  	}else{
//                  	  		$.ajax({
//				           type: "GET",
//				            url: "mudidi.aspx?type=view&classid=" + $(this).val(),
//				           success: function (msg) {
//			                    msg = msg.split("{view}");
//			                    $("#Select"+id).next().remove();
//			                    $("#Select"+id).remove();
//			                   $('.next').append('<select multiple id=\"Select'+id+'\" title=\"'+id+'\" class=\"end\">'+msg[0]+'</select>');
//			                    $("#Select"+id).selected();
//			                    }
//			              });
//                  	  	}
//
//                  	  	
//                  	  };
//                  });
//                  }
//            });
//          });
//   });
    
//注意：下面的代码是放在/Poster/rightgp调用

</script>
   
    </form>
</body>
</html>
