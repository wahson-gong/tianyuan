


//购物车的js
function set_cart(id, g1,guige) {
    var shuliang = 1;
    if (document.getElementById("shuliang")) {
        shuliang = document.getElementById("shuliang").value;
    }
    if (document.getElementById("kucun")) {
        var kucun = document.getElementById("kucun").innerHTML;
        if (parseFloat(shuliang) > parseFloat(kucun)) {
            layer.alert("存库不足，最多选择" + kucun + "件");
            return false;
        }
    }
    
    $.ajax({
        type: 'get',
        url: '/pay/set_cart.aspx?id=' + id + '&leixing=商品&shuliang=' + shuliang + '&type=' + escape(g1) + '&guige=' + escape(guige) + '&sid=' + Math.random(),
        success: function (data) {
            //返回
            var response = data.split("{fzw:next}");
            if (response[0] == "err") {
                layer.alert("请先登录");
           
                window.location.href = "single.aspx?m=login&tipurl=" + window.location.href.replace("&", "fzw123").replace("&", "fzw123");
                window.event.returnValue = false;
            }
            else if (response[0] == "quanxian") {
                layer.alert(response[1]);
            }
            else {
                if (response[1] == "1") {
                    window.location = "/single.aspx?m=order&id=" + response[2];
                    window.event.returnValue = false;
                }
                else {
                    if (parseFloat(response[0]) > 0) {
                        document.getElementById("order_count").style.display = "";
                        document.getElementById("order_count").innerHTML = response[0];
                    }
                    else {
                        document.getElementById("order_count").style.display = "none";
                        document.getElementById("order_count").innerHTML = response[0];
                    }

                    //  layer.alert("加入购物车成功");
                    layer.msg("加入购物车成功");
                }
            }
            //end
        }
    })


}
