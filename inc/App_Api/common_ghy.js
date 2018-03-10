function setParam(name, value) {
    localStorage.setItem(name, value)
}

function getParam(name) {
    return localStorage.getItem(name)
}
function getApiUrl() {
    //���Ե�ַ
    // return "http://localhost:8095/App_Api/";
    //������ַ
    return "/inc/App_Api/";
}

function getWebUrl() {
    //���Ե�ַ
    // return "http://localhost:8095";
    //������ַ
    return "";

}

//�õ���ӡһ��Ʊ��Ҫ�ļ۸�===>��ӡ��
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
            //alert("���� ===��"+v[key]);
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




//����foot������
function load_foot(callback, div_id) {

    setParam("foot_str", $("#" + div_id).html());
    var foot_str = getParam('foot_str');
    if (foot_str == "" || foot_str == null) {
        $("#" + div_id).html(foot_str);
    }


    //Ԥ���ص������ļ���ҳ��
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

    //��ÿ���ײ�������ϰ�׿ԭ������ת
    tiaozhuan_noParamForFoot("mn_l"); //android
}


//������ͨ��ת
function tiaozhuan_noParamForFoot(class_name) {
    //���������ת

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



//������ͨ��ת
function tiaozhuan_noParam(class_name) {
    //���������ת

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
        temp_webview.addEventListener("loaded", function () { //ע����webview����������¼�
            nwaiting.close(); //��webview��������Ϻ�رյȴ���
            temp_webview.show("slide-in-right", 150); //����webview������ʾ��������ʾ����Ч��Ϊ�ٶ�150������Ҳ����붯��
        }, false);
        //location.href = tiaozhuan_url;
    })
}


//���һ��class ��div�ж��ٸ�
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

//���ð�׿ԭʼ����ת
function tiaozhuanByandroid(class_name) {

    //���������ת
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

    //���������ת
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
                autoShow: true, //�Զ���ʾ�ȴ���Ĭ��Ϊtrue
                //title:'���ڼ���...',//�ȴ��Ի�������ʾ����ʾ����

            }
        });
    })
}


//������ͨ��ת===>����������ת
function tiaozhuan(class_name, param_name, param_value) {
    //���������ת

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
                autoShow: true, //�Զ���ʾ�ȴ���Ĭ��Ϊtrue
                title: '���ڼ���...',//�ȴ��Ի�������ʾ����ʾ����

            }
        });

    })
}

//��õ�ǰҳ����������
function getUrlFilename() {
    var thisHREF = document.location.href;
    var tmpHPage = thisHREF.split("/");
    var thisHPage = tmpHPage[tmpHPage.length - 1];
    return thisHPage;
}

//��ȡurl�еĲ���
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //����һ������Ŀ�������������ʽ����
    var r = window.location.search.substr(1).match(reg); //ƥ��Ŀ�����
    if (r != null) return unescape(r[2]);
    return null; //���ز���ֵ
}

//����̨���ص�html��string���ص���Ӧ��div��

function getHtmlStr(callback, div_id, id, api_url, data_name) {
    $.ajax({
        url: api_url,
        data: data_name + "=" + id,
        dataType: "jsonp",
        jsonp: "callback", //���ݸ�����������ҳ��ģ����Ի��jsonp�ص��������Ĳ�����(һ��Ĭ��Ϊ:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            //����ǰ�Ĵ���  ������ڻ���Ͱѻ�����س���

            if (getParam(callback + "_" + id) != null) {
                $("#" + div_id).append(getParam(callback + "_" + id));
                console.log("����===����������");

            }
        },
        success: function (data) {

            var html = "";
            $.each(data, function (i, v) {

                html = v["str1"];

            });

            //���û���
            if (getParam(callback + "_" + id) != html || getParam(callback + "_" + id) == null) {

                setParam(callback + "_" + id, html);
                $("#" + div_id).append(html);
                console.log("����===��������");

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
                    //alert("�ѵ�¼");==>���¼���foot  div
                    //load_foot("get_foot","footer");


                } else {
                    location.href = user_type + "_dl.html";
                }
            });

        }
    })
}

//�����û������жϵ�¼ҳ��
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
    setParam("is_fristLoad", "��");
    layer.msg("���Ѱ�ȫ�˳�");
    location.href = "index.html";
}


//�����ж��ַ����Ƿ�Ϊ��ĸ������
function checknum(value) {
    var Regx = /^[A-Za-z0-9]*$/;
    if (Regx.test(value)) {
        return true;
    } else {
        return false;
    }
}

//����ͳһ��ʶid�ĺ���
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



//֧����֧��
function alipay(jine) {
    var channel = null;
    // 1. ��ȡ֧��ͨ��
    function plusReady() {
        // ��ȡ֧��ͨ��
        plus.payment.getChannels(function (channels) {
            channel = channels[0];
        }, function (e) {
            alert("��ȡ֧��ͨ��ʧ�ܣ�" + e.message);
        });
    }
    document.addEventListener('plusready', plusReady, false);

    var ALIPAYSERVER = 'http://demo.dcloud.net.cn/helloh5/payment/alipay.php?total=' + jine;
    var WXPAYSERVER = 'http://demo.dcloud.net.cn/helloh5/payment/wxpay.php?total=' + jine;
    // 2. ����֧������
    function pay(id) {
        // �ӷ���������֧������
        var PAYSERVER = '';
        if (id == 'alipay') {
            PAYSERVER = ALIPAYSERVER;
        } else if (id == 'wxpay') {
            PAYSERVER = WXPAYSERVER;
        } else {
            plus.nativeUI.alert("��֧�ִ�֧��ͨ����", null, "����");
            return;
        }
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            switch (xhr.readyState) {
                case 4:
                    if (xhr.status == 200) {
                        plus.payment.request(channel, xhr.responseText, function (result) {
                            plus.nativeUI.alert("֧���ɹ���", function () {
                                back();
                            });
                        }, function (error) {
                            plus.nativeUI.alert("֧��ʧ�ܣ�" + error.code);
                        });
                    } else {
                        alert("��ȡ������Ϣʧ�ܣ�");
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

//û���ҵ�ͼƬ
function errorImg(img) {
    img.src = "../images/banner.png";
    img.onerror = null;
}


//�������������в�Ϊ�յ�һ��
function re_notTwoOfOneNull(str1, str2) {
    //name = v["xingming"] == "" ? name = v["yonghuming"] : name = v["xingming"];
    var temp_str = "";
    temp_str = str1 == "" ? temp_str = str2 : temp_str = str1;
    return temp_str;
}

//�Ͱٷֱ�
function Percentage(num, total)
{ return (Math.round(num / total * 10000) / 100.00 + "%"); }

//��json�������һ������
function add_oneRow_json(json_name, add_value) {
    var temp_json = "";
    temp_json = getParam(json_name);

    console.log("temp_json===>" + temp_json);
    console.log("add_value===>" + add_value.substring(1, add_value.length - 1));

    if (temp_json == null || temp_json == "") {
        temp_json = add_value;
        setParam(json_name, temp_json);
        console.log("��ӳɹ�");
        return "��ʼ����";
    } else {
        //�ж��Ƿ��Ѿ�������Ҫ��ӵ�string
        if (temp_json.indexOf(add_value.substring(1, add_value.length - 1)) > 0) {
            console.log("�����Ѿ����ڣ��������");
            return "����ӵ�����������";
        } else {

            temp_json = temp_json.substring(0, temp_json.length - 1) + "," + add_value.substring(1, add_value.length);

            setParam(json_name, temp_json);
            console.log("��ӳɹ�");
            return "��ʼ����";
        }
    }

}



//���url����ļ���
function getUrlFileName(fileUrl_ghy) {
    var arr_ghy = new Array();
    var fileName_ghy = ""; //���ص��ļ���
    arr_ghy = fileUrl_ghy.split('/');
    //console.log(arr_ghy[arr_ghy.length - 1]);
    fileName_ghy = arr_ghy[arr_ghy.length - 1];
    //console.log("fileName_ghy�ļ���===>" + fileName_ghy);
    return fileName_ghy;
}

//top :��ʾǰ����
//sql_str ����ѯ���ַ���������id=1andpaixu=3�����������and����
//table ����ѯ���ݱ������
//re_param ��������Щ�ֶΣ���Ϊ�վͷ���ȫ��(idandpaixu)���ض��������and����
//query_str ��ģ����ѯ�ֶΣ���Ϊ�վͲ�ģ����ѯ
function requert_json2(callback, table, top, sql_str, re_param, query_str) {
    $.ajax({
        url: getApiUrl() + 'Execution.ashx',
        data: "table=" + table + "&top=" + top + "&sql_str=" + sql_str + "&re_param=" + re_param + "&query_str=" + query_str,
        dataType: "jsonp",
        jsonp: "callback", //���ݸ�����������ҳ��ģ����Ի��jsonp�ص��������Ĳ�����(һ��Ĭ��Ϊ:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("�����С�����");
        },
        success: function (data) {
            return data;
        }
    });
}

//��������Ĳ���������ӿ�
//sql_str ��$����
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
        async: false,//ȡ���첽����
        jsonp: "callback", //���ݸ�����������ҳ��ģ����Ի��jsonp�ص��������Ĳ�����(һ��Ĭ��Ϊ:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json����תstring
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //���������
            console.log("error");

        }
    });
    return re_str;

}


//��������Ĳ���������ӿ�  ===>����where ����
//sql_str ��$����
function requert_json3(callback, table, sql_str, where_sql) {

    var temp_sql = "";
    var temp_sql1 = "";//����where������
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
        async: false,//ȡ���첽����
        jsonp: "callback", //���ݸ�����������ҳ��ģ����Ի��jsonp�ص��������Ĳ�����(һ��Ĭ��Ϊ:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json����תstring
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //���������
            console.log("error");

        }
    });
    return re_str;

}
/////////////////////////
//רΪ��Ӧ���ṩ�Ľӿ�
/////////////////////////


//��������Ĳ���������ӿ�
//sql_str ��$����
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
        async: false,//ȡ���첽����
        jsonp: "callback", //���ݸ�����������ҳ��ģ����Ի��jsonp�ص��������Ĳ�����(һ��Ĭ��Ϊ:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json����תstring
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //���������
            console.log("error");

        }
    });
    return re_str;

}


//��������Ĳ���������ӿ�  ===>����where ����
//sql_str ��$����
function requert_json3_b(callback, table, sql_str, where_sql) {

    var temp_sql = "";
    var temp_sql1 = "";//����where������
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
        async: false,//ȡ���첽����
        jsonp: "callback", //���ݸ�����������ҳ��ģ����Ի��jsonp�ص��������Ĳ�����(һ��Ĭ��Ϊ:callback)
        jsonpCallback: callback,
        beforeSend: function () {
            console.log("loading");
        },
        success: function (data) {
            console.log("success");
            //setParam(callback, JSON.stringify(data)); //json����תstring
            re_str = JSON.stringify(data);
            // return JSON.stringify(data);
        },
        error: function () {
            //���������
            console.log("error");

        }
    });
    return re_str;

}

//ɾ���������ظ���Ԫ��
function delect_same_param(arr) {
    var new_arr = [];
    for (var i = 0; i < arr.length; i++) {


        var items = arr[i];

        //�ж�Ԫ���Ƿ������new_arr�У��������������뵽new_arr�����


        if ($.inArray(items, new_arr) == -1) {


            new_arr.push(items);


        }

    }

    return no_null_array(new_arr);
}

//ȥ����Ԫ��
function no_null_array(array) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] == "" || typeof (array[i]) == "undefined" || array[i] == "0") {
            array.splice(i, 1);
            i = i - 1;
        }
    }
    return array;
}


//�õ�״̬�б�
function get_zhuangtai(id, u1) {
    var re_str = "";
    if (id == "" && u1 == "") {
        re_str = requert_json_b("getZT" + uuid(4), "Parameter", "");
    }
    else {
        var zhuangtai_data1 = JSON.parse(requert_json_b("getZT" + uuid(4), "Parameter", ""));
        $.each(zhuangtai_data1, function (j, v1) {
            if (u1.indexOf(v1["u1"]) == 0) {
                console.log("����״̬id===>" + v1["id"]);
                re_str = v1["id"];
            }
            else if (id.indexOf(v1["id"]) == 0) {
                console.log("����״̬��===>" + v1["u1"]);
                // alert(v1["u1"]);
                re_str = v1["u1"];
            }
            else {
                console.log("״̬��ѯ�޽��");

            }


        });
    }
    return re_str;

}



//�������µ�ַ��ť
$("#add_address").click(function () {
    $(".add-wzw").hide();
    $(".submit-i").show();

});


//���������ť
$(document).on("click", ".quxiao", function () {

    // alert(piao_data[0].id);//�õ�ѡ��Ʊ���������   $(this).attr("data-xingchengdanhao")
    if (confirm('ȷ������?') == false) return false;
    var xcd_data = JSON.parse(requert_json3_b("edit" + uuid(4), "xingchengdan", "xianshi=����ʾ", "xingchengdanhao='" + $(this).attr("data-xingchengdanhao") + "'"));
    var piao_data = JSON.parse(requert_json3_b("edit" + uuid(4), "piao", "xianshi=����ʾ", "xingchengdanhao='" + $(this).attr("data-xingchengdanhao") + "'"));
    if (xcd_data[0].str1 == "success" && piao_data[0].str1 == "success") {
        alert("�����ɹ�");
        window.location.href = window.location.href;
    }


});


//����Ʊ������
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
        alert("�������ύ");
        window.location.href = "/single.aspx?m=xcd_detail&xcd_bianhao=" + $(this).attr("data-xingchengdanhao");
    }


});

//type Ϊ��ӡ�Ѹ���type=dyf  ������ʧ�г̵�����type=lost_xcd

function GoToPay(xcd_bianhao, type) {
    var jine = 0;
    var miaoshu = "";
    if (type == "dyf") {
        //alert("Ϊ��ӡ�Ѹ���====>" + xcd_bianhao);
        miaoshu = "Ϊ��ӡ�Ѹ���";
    }
    else if (type == "lost_xcd") {
        //alert("Ϊ��ʧ�г̵�����====>" + xcd_bianhao);
        miaoshu = "Ϊ��ʧ�г̵�����";
    }
    var xcd_data = JSON.parse(requert_json("query" + uuid(4), "xingchengdan", "xingchengdanhao=" + xcd_bianhao));

    var cai_re_data = JSON.parse(requert_json("querycaiwu" + uuid(4), "caiwu", "dingdanhao=" + xcd_bianhao));
    //��������¼�Ѵ�����ֱ��ȥ֧��
    if (cai_re_data.length > 0) {
        window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=֧����&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
    }
    if (xcd_data[0].xingchengdanhao == xcd_bianhao) {
        //���һ�������¼
        var caiwu_data = requert_json("add", "caiwu", "jine=" + jine + "$leixing=����" + "$miaoshu=" + miaoshu + "$dingdanhao=" + xcd_bianhao + "$zhuangtai=δ����");
        if (caiwu_data[0].str1 == "success") {
            window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=֧����&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
        }
    }
    else {
        alert("���޴˵�");
    }

}
//Ϊ��ӡ�Ѹ���
function GoToPay(xcd_bianhao) {
    var xcd_data = JSON.parse(requert_json("query" + uuid(4), "xingchengdan", "xingchengdanhao=" + xcd_bianhao));

    var cai_re_data = JSON.parse(requert_json("querycaiwu" + uuid(4), "caiwu", "dingdanhao=" + xcd_bianhao));
    //��������¼�Ѵ�����ֱ��ȥ֧��

    if (cai_re_data.length > 0) {
        window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=֧����&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
    } else if (xcd_data[0].xingchengdanhao == xcd_bianhao) {
        //���һ�������¼
        var caiwu_data = JSON.parse(requert_json("add", "caiwu", "jine=" + xcd_data[0].jine + "$leixing=����" + "$miaoshu=Ϊ��ӡ�Ѹ���" + "$dingdanhao=" + xcd_bianhao + "$zhuangtai=δ����"));

        if (caiwu_data[0].str1 == "success") {
            window.location.href = "/pay/userorder.aspx?type=pay&zhifufangshi=֧����&dingdanhao=" + xcd_bianhao + "&tablename=caiwu";
        }
    }
    else {
        alert("���޴˵�");
    }



}

