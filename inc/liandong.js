var request = false;
try {
    request = new XMLHttpRequest();
} catch (trymicrosoft) {
    try {
        request = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (othermicrosoft) {
        try {
            request = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (failed) {
            request = false;
        }
    }
}

if (!request) {
    alert("Error initializing XMLHttpRequest!");
}
function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (postPath);
}
function liandong(classid,cengji,divstr) {

    var url = "/inc/liandong.aspx?classid=" + escape(classid) + "&cengji=" + escape(cengji) + "&divstr=" + escape(divstr) + "&sid=" + Math.random();
    document.write(url);
    request.open("GET", url, true);
    request.onreadystatechange = liandong_ajax;
    request.send(null);

}

function liandong_ajax() {
    if (request.readyState == 4) {
        if (request.status == 200) {
            if (request.responseText != "") {
                var response = request.responseText.split("{fzw:next}");
                document.getElementById(response[0]).innerHTML = document.getElementById(response[0]).innerHTML+response[1];

            }

        }


    }
}


