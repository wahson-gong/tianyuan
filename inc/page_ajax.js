function set_ajax(t1) {
    var url = "/inc/page_ajax.aspx?t1=" + escape(t1) + "&sid=" + Math.random();
    $.ajax({
        type: 'get',
        url: url,
        success: function (data) {
            //返回
            var response = data.split("{fzw:next}");
            if (response[0] == "+") {
                if (document.getElementById(response[1])) {
                    document.getElementById(response[1]).innerHTML = response[2];
                }

            }
            if (response[0] == "-") {
                document.getElementById(response[1]).innerHTML = response[2];
            }
            else if (response[0] == "=") {
                document.getElementById(response[1]).innerHTML = response[2];
            }
            //end
        }
    })


}
