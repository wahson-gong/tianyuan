function setParam(name, value) {
    localStorage.setItem(name, value)
}

function getParam(name) {
    return localStorage.getItem(name)
}
function getApiUrl() {
    //测试地址
    // return "http://localhost:8095/App_Api/";
    //生产地址
    return "/inc/App_Api/";
}

function getWebUrl() {
    //测试地址
    // return "http://localhost:8095";
    //生产地址
    return "";

}

//得到打印一张票需要的价格===>打印费
function getDYF() {
    return 5;
}

function jsonpTojson(callback, jsonp) {
    jsonp = jsonp.replace(callback + "(", "");
    jsonp = jsonp.replace(")", "");
    return jsonp;
}

function queryJsonp(key, value, jsonp) {

    var data_Jsonp = JSON.parse(jsonp);
    var state = "";
    if (data_Jsonp == null) {
        return state;
    }
    $.each(data_Jsonp, function (i, v) {

        if (v[key] == value) {
            //alert("存在 ===》"+v[key]);
            state = "true";
        }

    });
    return state;
}
function queryJsonpReValue(key, value, re_key, jsonp) {

    var data_Jsonp = JSON.parse(jsonp);
    var state = "";
    if (data_Jsonp == null) {
        return state;
    }
    $.each(data_Jsonp, function (i, v) {

        if (v[key] == value) {
            state = v[re_key];
        }

    });
    return state;
}




//加载foot的内容
function load_foot(callback, div_id) {

    setParam("foot_str", $("#" + div_id).html());
    var foot_str = getParam('foot_str');
    if (foot_str == "" || foot_str == null) {
        $("#" + div_id).html(foot_str);
    }


    //预加载导航栏的几个页面
    var webviewShow_fenlei;
    var webviewShow_xuexiao;
    var webviewShow_xiazai;
    var webviewShow_index;

    var webviewShow_xs_index;
    var webviewShow_xs_dl;
    var webviewShow_ls_dl;
    var webviewShow_ls_index;

    webviewShow_fenlei = mui.preload({
        url: 'fenlei.html',
        id: 'fenlei',
        styles: {
            popGesture: "hide"
        }
    });
    webviewShow_xuexiao = mui.preload({
        url: 'xuexiao.html',
        id: 'xuexiao',
        styles: {
            popGesture: "hide"
        }
    });
    webviewShow_xiazai = mui.preload({
        url: 'xiazai.html',
        id: 'xiazai',
        styles: {
            popGesture: "hide"
        }
    });

    webviewShow_index = mui.preload({
        url: 'index.html',
        id: 'index',
        styles: {
            popGesture: "hide"
        }
    });

    if (getParam("user_type") == "xs") {
        webviewShow_xs_index = mui.preload({
            url: 'xs_index.html',
            id: 'xs_index',
            styles: {
                popGesture: "hide"
            }
        });
        webviewShow_xs_dl = mui.preload({
            url: 'xs_dl.html',
            id: 'xs_dl',
            styles: {
                popGesture: "hide"
            }
        });
    }
    if (getParam("user_type") == "ls") {
        webviewShow_ls_index = mui.preload({
            url: 'ls_index.html',
            id: 'ls_index',
            styles: {
                popGesture: "hide"
            }
        });
        webviewShow_ls_dl = mui.preload({
            url: 'ls_dl.html',
            id: 'ls_dl',
            styles: {
                popGesture: "hide"
            }
        });

    }

    //给每个底部分类加上安卓原生的跳转
    tiaozhuan_noParamForFoot("mn_l"); //android
}


//采用普通跳转
function tiaozhuan_noParamForFoot(class_name) {
    //点击分类跳转

    $(document).on("click", "." + class_name, function () {
        //alert(getUrlFilename());
        var tiaozhuan_url = "";
        tiaozhuan_url = $(this).attr("data-href");

        if (tiaozhuan_url == getUrlFilename()) {
            return;
        }
        if (getParam('user_name') == null || getParam('user_name') == "") {

            tiaozhuan_url == "xs_index.html" ? tiaozhuan_url = getParam("user_type") + "_dl.html" : getParam("user_type") + "_index.html";

        } else {
            tiaozhuan_url == "xs_index.html" ? tiaozhuan_url = getParam("user_type") + "_index.html" : tiaozhuan_url = tiaozhuan_url;
            //tiaozhuan_url = tiaozhuan_url.replace("xs_index.html", getParam("user_type") + "_index.html");

        }

        var temp_webview = "";
        temp_webview = tiaozhuan_url.replace(".html", "");
        if (temp_webview == "xs_index" || temp_webview == "xs_dl" || temp_webview == "ls_index" || temp_webview == "ls_dl") {
            plus.webview.create(tiaozhuan_url, temp_webview, {}).show('slide-in-right', 150);
            return;
        }
        plus.webview.getWebviewById(temp_webview).show('slide-in-right', 150);

        //location.href = tiaozhuan_url;
    })
}



//采用普通跳转
function tiaozhuan_noParam(class_name) {
    //点击分类跳转

    $(document).on("click", "." + class_name, function () {
        //alert(getUrlFilename());
        var tiaozhuan_url = "";
        tiaozhuan_url = $(this).attr("data-href");

        if (tiaozhuan_url == getUrlFilename()) {
            return;
        }
        if (getParam('user_name') == null || getParam('user_name') == "") {

            tiaozhuan_url == "xs_index.html" ? tiaozhuan_url = getParam("user_type") + "_dl.html" : getParam("user_type") + "_index.html";

        } else {
            tiaozhuan_url == "xs_index.html" ? tiaozhuan_url = getParam("user_type") + "_index.html" : tiaozhuan_url = tiaozhuan_url;
            //tiaozhuan_url = tiaozhuan_url.replace("xs_index.html", getParam("user_type") + "_index.html");
        }

        var nwaiting = plus.nativeUI.showWaiting();
        var temp_webview_id = "";
        var temp_webview = "";
        temp_webview_id = tiaozhuan_url.replace(".html", "");
        temp_webview = plus.webview.create(tiaozhuan_url, temp_webview_id, { hardwareAccelerated: true });
        temp_webview.addEventListener("loaded", function () { //注册新webview的载入完成事件
            nwaiting.close(); //新webview的载入完毕后关闭等待框
            temp_webview.show("slide-in-right", 150); //把新webview窗体显示出来，显示动画效果为速度150毫秒的右侧移入动画
        }, false);
        //location.href = tiaozhuan_url;
    })
}


//获得一个class 的div有多少个
function getClassCount(class_name) {
    var j = 0;
    $("." + class_name).each(function () {

        j++;
    })
    return j;
}

function getOneArray_num(arrey, array_value) {
    var i = 0;
    for (i; i < arrey.length; i++) {
        if (array_value[i] == array_value)
            break;
    }
    return i;
}

//采用安卓原始的跳转
function tiaozhuanByandroid(class_name) {

    //点击分类跳转
    $(document).on("click", "." + class_name, function () {
        var tiaozhuan_url = "";
        tiaozhuan_url = $(this).attr("data-href");
        mui.openWindow({
            url: tiaozhuan_url,
            //id:'info'
        });
    })
}
function tiaozhuan_url(tiaozhuan_url) {

    mui.openWindow({
        createNew: false,
        url: tiaozhuan_url,
        //id:'info'
    });
}

function tiaozhuanByandroidForFoot(class_name) {

    //点击分类跳转
    $(document).on("click", "." + class_name, function () {
        var tiaozhuan_url = "";
        tiaozhuan_url = $(this).attr("data-href");
        if (tiaozhuan_url == getUrlFilename()) {
            return;
        }
        mui.openWindow({
            url: tiaozhuan_url,
            //id:'info'
            createNew: false,
            waiting: {
                autoShow: true, //自动显示等待框，默认为true
                //title:'正在加载...',//等待对话框上显示的提示内容

            }
        });
    })
}


//采用普通跳转===>带参数的跳转
function tiaozhuan(class_name, param_name, param_value) {
    //点击分类跳转

    $(document).on("click", "." + class_name, function () {
        //alert(getUrlFilename());
        var tiaozhuan_url = "";
        tiaozhuan_url = $(this).attr("data-href");
        if (tiaozhuan_url == getUrlFilename()) {
            return;
        }
        //location.href=tiaozhuan_url+"?"+param_name+"="+param_value;
        console.log(tiaozhuan_url + "?" + param_name + "=" + param_value);
        mui.openWindow({
            url: tiaozhuan_url + "?" + param_name + "=" + param_value,
            //id:'info'
            createNew: false,
            waiting: {
                autoShow: true, //自动显示等待框，默认为true
                title: '正在加载...',//等待对话框上显示的提示内容

            }
        });

    })
}

//获得当前页面具体的名字
function getUrlFilename() {
    var thisHREF = document.location.href;
    var tmpHPage = thisHREF.split("/");
    var thisHPage = tmpHPage[tmpHPage.length - 1];
    return thisHPage;
}

//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) return unescape(r[2]);
    return null; //返回参数值
}

//将后台返回的html的string加载到对应的div中

function getHtmlStr(callback, div_id, id, api_url, data_name) {
    $.ajax({
        url: api_url,
        data: data_name + "=" + id,
        dataType: "jsonp",
        jsonp: "callback", //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            //请求前的处理  如果存在缓存就把缓存加载出来

            if (getParam(callback + "_" + id) != null) {
                $("#" + div_id).append(getParam(callback + "_" + id));
                console.log("加载===》缓存内容");

            }
        },
        success: function (data) {

            var html = "";
            $.each(data, function (i, v) {

                html = v["str1"];

            });

            //设置缓存
            if (getParam(callback + "_" + id) != html || getParam(callback + "_" + id) == null) {

                setParam(callback + "_" + id, html);
                $("#" + div_id).append(html);
                console.log("加载===》新内容");

            }
        }
    });
}

function isLogin(user_type) {
    var temp_userName = "";
    var Api_file = "";
    var user_type2 = "";
    user_type2 = getParam("user_type");
    if (user_type2 == "xs") {
        Api_file = "user";
    }
    else if (user_type2 == "ls") {
        Api_file = "mingshi";
    }
    temp_userName = getParam('user_name');
    $.ajax({
        type: "GET",
        url: getApiUrl() + Api_file + ".ashx",
        data: "user_name=" + temp_userName,
        dataType: "jsonp",
        jsonpCallback: "Is_nameReged",
        success: function (data) {
            var result_str = '';
            $.each(data, function (i, v) {
                result_str = v["str1"];
                if (result_str == "isReged") {
                    //alert("已登录");==>重新加载foot  div
                    //load_foot("get_foot","footer");


                } else {
                    location.href = user_type + "_dl.html";
                }
            });

        }
    })
}

//根据用户类型判断登录页面
function user_dl() {
    var Api_file = "";
    var user_type = "";
    user_type = getParam("user_type");
    if (user_type == "xs") {
        Api_file = "xs_dl.html";
    }
    else if (user_type == "ls") {
        Api_file = "ls_dl.html";
    }
    location.href = Api_file;
}

function out_login() {

    //	localStorage.removeItem("touxiang");
    //	localStorage.removeItem("jieshaoren");
    //	localStorage.removeItem("leixing");
    //	localStorage.removeItem("xingming");
    //	localStorage.removeItem("shengri");
    //	localStorage.removeItem("xingbie");
    //	localStorage.removeItem("shouji");
    //	localStorage.removeItem("user_name");
    //	localStorage.removeItem("foot_str");

    localStorage.clear();
    setParam("is_fristLoad", "否");
    layer.msg("您已安全退出");
    location.href = "index.html";
}


//正则判断字符串是否为字母或数字
function checknum(value) {
    var Regx = /^[A-Za-z0-9]*$/;
    if (Regx.test(value)) {
        return true;
    } else {
        return false;
    }
}

//生成统一标识id的函数
function uuid(lenth) {
    var s = [];
    var hexDigits = "0123456789";
    for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
    }
    s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01


    var uuid = s.join("");
    uuid = uuid.substring(0, lenth);
    return uuid;
}



//支付宝支付
function alipay(jine) {
    var channel = null;
    // 1. 获取支付通道
    function plusReady() {
        // 获取支付通道
        plus.payment.getChannels(function (channels) {
            channel = channels[0];
        }, function (e) {
            alert("获取支付通道失败：" + e.message);
        });
    }
    document.addEventListener('plusready', plusReady, false);

    var ALIPAYSERVER = 'http://demo.dcloud.net.cn/helloh5/payment/alipay.php?total=' + jine;
    var WXPAYSERVER = 'http://demo.dcloud.net.cn/helloh5/payment/wxpay.php?total=' + jine;
    // 2. 发起支付请求
    function pay(id) {
        // 从服务器请求支付订单
        var PAYSERVER = '';
        if (id == 'alipay') {
            PAYSERVER = ALIPAYSERVER;
        } else if (id == 'wxpay') {
            PAYSERVER = WXPAYSERVER;
        } else {
            plus.nativeUI.alert("不支持此支付通道！", null, "捐赠");
            return;
        }
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            switch (xhr.readyState) {
                case 4:
                    if (xhr.status == 200) {
                        plus.payment.request(channel, xhr.responseText, function (result) {
                            plus.nativeUI.alert("支付成功！", function () {
                                back();
                            });
                        }, function (error) {
                            plus.nativeUI.alert("支付失败：" + error.code);
                        });
                    } else {
                        alert("获取订单信息失败！");
                    }
                    break;
                default:
                    break;
            }
        }
        xhr.open('GET', PAYSERVER);
        xhr.send();
    }

}

//没有找到图片
function errorImg(img) {
    img.src = "../images/banner.png";
    img.onerror = null;
}


//返回两个变量中不为空的一个
function re_notTwoOfOneNull(str1, str2) {
    //name = v["xingming"] == "" ? name = v["yonghuming"] : name = v["xingming"];
    var temp_str = "";
    temp_str = str1 == "" ? temp_str = str2 : temp_str = str1;
    return temp_str;
}

//就百分比
function Percentage(num, total)
{ return (Math.round(num / total * 10000) / 100.00 + "%"); }

//给json对象添加一行内容
function add_oneRow_json(json_name, add_value) {
    var temp_json = "";
    temp_json = getParam(json_name);

    console.log("temp_json===>" + temp_json);
    console.log("add_value===>" + add_value.substring(1, add_value.length - 1));

    if (temp_json == null || temp_json == "") {
        temp_json = add_value;
        setParam(json_name, temp_json);
        console.log("添加成功");
        return "开始下载";
    } else {
        //判断是否已经包含了要添加的string
        if (temp_json.indexOf(add_value.substring(1, add_value.length - 1)) > 0) {
            console.log("内容已经存在，不再添加");
            return "已添加到下载任务中";
        } else {

            temp_json = temp_json.substring(0, temp_json.length - 1) + "," + add_value.substring(1, add_value.length);

            setParam(json_name, temp_json);
            console.log("添加成功");
            return "开始下载";
        }
    }

}



//获得url里的文件名
function getUrlFileName(fileUrl_ghy) {
    var arr_ghy = new Array();
    var fileName_ghy = ""; //下载的文件名
    arr_ghy = fileUrl_ghy.split('/');
    //console.log(arr_ghy[arr_ghy.length - 1]);
    fileName_ghy = arr_ghy[arr_ghy.length - 1];
    //console.log("fileName_ghy文件名===>" + fileName_ghy);
    return fileName_ghy;
}

//top :显示前几条
//sql_str ：查询的字符串（比如id=1andpaixu=3）多个条件用and隔开
//table ：查询数据表的名称
//re_param ：返回那些字段，若为空就返回全部(idandpaixu)返回多个参数用and隔开
//query_str ：模糊查询字段，若为空就不模糊查询
function requert_json2(callback, table, top, sql_str, re_param, query_str) {
    $.ajax({
        url: getApiUrl() + 'Execution.ashx',
        data: "table=" + table + "&top=" + top + "&sql_str=" + sql_str + "&re_param=" + re_param + "&query_str=" + query_str,
        dataType: "jsonp",
        jsonp: "callback", //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("请求中。。。");
        },
        success: function (data) {
            return data;
        }
    });
}

//根据请求的参数名请求接口
//sql_str 用$隔开
function requert_json(callback, table, sql_str) {

    var temp_sql = "";
    var temp_arr = new Array();
    var arr = sql_str.split('$');
    var re_str = "";
    for (var i = 0; i < arr.length; i++) {
        // arr[i] = arr[i] / 2.0;
        temp_arr = arr[i].split('=');
        temp_sql = temp_sql + "&" + temp_arr[0] + "=" + temp_arr[1];
    }
    if (sql_str == "" || sql_str == null) {
        temp_sql = "";
    }
    //alert(temp_sql);
    $.ajax({
        url: getApiUrl() + 'Execution.ashx',
        data: "table=" + table + temp_sql,
        dataType: "jsonp",
        async: false,//取消异步请求
        jsonp: "callback", //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json对象转string
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //请求出错处理
            console.log("error");

        }
    });
    return re_str;

}


//根据请求的参数名请求接口  ===>带有where 条件
//sql_str 用$隔开
function requert_json3(callback, table, sql_str, where_sql) {

    var temp_sql = "";
    var temp_sql1 = "";//带有where条件的
    var temp_arr = new Array();
    var arr = sql_str.split('$');
    var re_str = "";
    for (var i = 0; i < arr.length; i++) {
        // arr[i] = arr[i] / 2.0;
        temp_arr = arr[i].split('=');
        temp_sql = temp_sql + "&" + temp_arr[0] + "=" + temp_arr[1];
    }
    if (where_sql != "" || where_sql != null) {
        temp_sql1 = " &where=" + where_sql;

    }

    //alert(temp_sql);
    $.ajax({
        url: getApiUrl() + 'Execution.ashx',
        data: "table=" + table + temp_sql + temp_sql1,
        dataType: "jsonp",
        async: false,//取消异步请求
        jsonp: "callback", //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json对象转string
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //请求出错处理
            console.log("error");

        }
    });
    return re_str;

}
/////////////////////////
//专为供应商提供的接口
/////////////////////////


//根据请求的参数名请求接口
//sql_str 用$隔开
function requert_json_b(callback, table, sql_str) {

    var temp_sql = "";
    var temp_arr = new Array();
    var arr = sql_str.split('$');
    var re_str = "";
    for (var i = 0; i < arr.length; i++) {
        // arr[i] = arr[i] / 2.0;
        temp_arr = arr[i].split('=');
        temp_sql = temp_sql + "&" + temp_arr[0] + "=" + temp_arr[1];
    }
    if (sql_str == "" || sql_str == null) {
        temp_sql = "";
    }
    //alert(temp_sql);
    $.ajax({
        url: getApiUrl() + 'Execution_b.ashx',
        data: "table=" + table + temp_sql,
        dataType: "jsonp",
        async: false,//取消异步请求
        jsonp: "callback", //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json对象转string
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //请求出错处理
            console.log("error");

        }
    });
    return re_str;

}


//根据请求的参数名请求接口  ===>带有where 条件
//sql_str 用$隔开
function requert_json3_b(callback, table, sql_str, where_sql) {

    var temp_sql = "";
    var temp_sql1 = "";//带有where条件的
    var temp_arr = new Array();
    var arr = sql_str.split('$');
    var re_str = "";
    for (var i = 0; i < arr.length; i++) {
        // arr[i] = arr[i] / 2.0;
        temp_arr = arr[i].split('=');
        temp_sql = temp_sql + "&" + temp_arr[0] + "=" + temp_arr[1];
    }
    if (where_sql != "" || where_sql != null) {
        temp_sql1 = " &where=" + where_sql;

    }

    //alert(temp_sql);
    $.ajax({
        url: getApiUrl() + 'Execution_b.ashx',
        data: "table=" + table + temp_sql + temp_sql1,
        dataType: "jsonp",
        async: false,//取消异步请求
        jsonp: "callback", //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json对象转string
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //请求出错处理
            console.log("error");

        }
    });
    return re_str;

}

//删除数组中重复的元素
function delect_same_param(arr) {
    var new_arr = [];
    for (var i = 0; i < arr.length; i++) {


        var items = arr[i];

        //判断元素是否存在于new_arr中，如果不存在则插入到new_arr的最后


        if ($.inArray(items, new_arr) == -1) {


            new_arr.push(items);


        }

    }

    return no_null_array(new_arr);
}

//去掉空元素
function no_null_array(array) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] == "" || typeof (array[i]) == "undefined" || array[i] == "0") {
            array.splice(i, 1);
            i = i - 1;
        }
    }
    return array;
}


//得到状态列表
function get_zhuangtai(id, u1) {
    var re_str = "";
    if (id == "" && u1 == "") {
        re_str = requert_json_b("getZT" + uuid(4), "Parameter", "");
    }
    else {
        var zhuangtai_data1 = JSON.parse(requert_json_b("getZT" + uuid(4), "Parameter", ""));
        $.each(zhuangtai_data1, function (j, v1) {
            if (u1.indexOf(v1["u1"]) == 0) {
                console.log("返回状态id===>" + v1["id"]);
                re_str = v1["id"];
            }
            else if (id.indexOf(v1["id"]) == 0) {
                console.log("返回状态名===>" + v1["u1"]);
                // alert(v1["u1"]);
                re_str = v1["u1"];
            }
            else {
                console.log("状态查询无结果");

            }


        });
    }
    return re_str;

}



//点击添加新地址按钮
$("#add_address").click(function () {
    $(".add-wzw").hide();
    $(".submit-i").show();

});


//点击撤销按钮
$(document).on("click", ".quxiao", function () {

    // alert(piao_data[0].id);//得到选中票对象的详情   $(this).attr("data-xingchengdanhao")
    if (confirm('确定撤销?') == false) return false;
    var xcd_data = JSON.parse(requert_json3_b("edit" + uuid(4), "xingchengdan", "xianshi=不显示", "xingchengdanhao='" + $(this).attr("data-xingchengdanhao") + "'"));
    var piao_data = JSON.parse(requert_json3_b("edit" + uuid(4), "piao", "xianshi=不显示", "xingchengdanhao='" + $(this).attr("data-xingchengdanhao") + "'"));
    if (xcd_data[0].str1 == "success" && piao_data[0].str1 == "success") {
        alert("撤销成功");
        window.location.href = window.location.href;
    }


});


//申请票号作废
$(document).on("click", ".shenqingfeipiao", function () {
    // alert(1);
    //$(".add_kuaididanhao").css({ "display": "block" });


    var piao_data;
    var piao_re_data = JSON.parse(requert_json("query11111" + uuid(4), "piao", "zhuangtai=" + get_zhuangtai("273", "") + "$xingchengdanhao=" + $(this).attr("data-xingchengdanhao")));
    $.each(piao_re_data, function (i, v) {
        console.log("piaohao===>" + v["id"]);
        JSON.parse(requert_json("edit" + uuid(4), "piao", "zhuangtai=" + get_zhuangtai("274", "") + "$id=" + v["id"]));

    });
    var xcd_data = JSON.parse(requert_json3("edit22222" + uuid(4), "xingchengdan", "zhuangtai=" + get_zhuangtai("274", ""), "xingchengdanhao='" + $(this).attr("data-xingchengdanhao") + "'"));

    if (xcd_data[0].str1 == "success") {
        alert("申请已提交");
        window.location.href = "/single.aspx?m=xcd_detail&xcd_bianhao=" + $(this).attr("data-xingchengdanhao");
    }


});

//type 为打印费付款type=dyf  还是遗失行程单付款type=lost_xcd

function GoToPay(xcd_bianhao, type) {
    var jine = 0;
    var miaoshu = "";
    if (type == "dyf") {
        //alert("为打印费付款====>" + xcd_bianhao);
        miaoshu = "为打印费付款";
    }
    else if (type == "lost_xcd") {
        //alert("为遗失行程单付款====>" + xcd_bianhao);
        miaoshu = "为遗失行程单付款";
    }
    var xcd_data = JSON.parse(requert_json("query" + uuid(4), "xingchengdan", "xingchengdanhao=" + xcd_bianhao));

    var cai_re_data = JSON.parse(requert_json("querycaiwu" + uuid(4), "caiwu", "dingdanhao=" + xcd_bianhao));
    //如果财务记录已存在则直接去支付
    if (cai_re_data.length > 0) {
        window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=支付宝&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
    }
    if (xcd_data[0].xingchengdanhao == xcd_bianhao) {
        //添加一条财务记录
        var caiwu_data = requert_json("add", "caiwu", "jine=" + jine + "$leixing=消费" + "$miaoshu=" + miaoshu + "$dingdanhao=" + xcd_bianhao + "$zhuangtai=未付款");
        if (caiwu_data[0].str1 == "success") {
            window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=支付宝&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
        }
    }
    else {
        alert("查无此单");
    }

}
//为打印费付款
function GoToPay(xcd_bianhao) {
    var xcd_data = JSON.parse(requert_json("query" + uuid(4), "xingchengdan", "xingchengdanhao=" + xcd_bianhao));

    var cai_re_data = JSON.parse(requert_json("querycaiwu" + uuid(4), "caiwu", "dingdanhao=" + xcd_bianhao));
    //如果财务记录已存在则直接去支付

    if (cai_re_data.length > 0) {
        window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=支付宝&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
    } else if (xcd_data[0].xingchengdanhao == xcd_bianhao) {
        //添加一条财务记录
        var caiwu_data = JSON.parse(requert_json("add", "caiwu", "jine=" + xcd_data[0].jine + "$leixing=消费" + "$miaoshu=为打印费付款" + "$dingdanhao=" + xcd_bianhao + "$zhuangtai=未付款"));

        if (caiwu_data[0].str1 == "success") {
            window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=支付宝&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
        }
    }
    else {
        alert("查无此单");
    }



}

