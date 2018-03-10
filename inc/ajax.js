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

if (!request)
{
  alert("Error initializing XMLHttpRequest!");
}
function ajax_getRootPath1() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (postPath);
}
     function ajax_set_ajax(t1)
	  {

	      var url = t1+"&sid=" + Math.random();
	        request.open("GET", url, true);
            request.onreadystatechange = ajax_loadPage;
            request.send(null);

	  }

    function ajax_loadPage() {
	        if (request.readyState == 4) 
			{
		   if (request.status == 200) 
		   {
			   	if(request.responseText!="")
				{
					var response = request.responseText.split("{fzw:next}");
					document.getElementById(response[0]).innerHTML=response[1];

					
				}
			
		   }
     

   }
   }
   
   
   function ajax_getRootPath1() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (postPath);
}
     function ajax_set_ajax(t1)
	  {

	      var url = t1+"&sid=" + Math.random();
	        request.open("GET", url, true);
            request.onreadystatechange = ajax_loadPage;
            request.send(null);

	  }

    function ajax_loadPage() {
	        if (request.readyState == 4) 
			{
		   if (request.status == 200) 
		   {
			   	if(request.responseText!="")
				{
					var response = request.responseText.split("{fzw:next}");
					document.getElementById(response[0]).innerHTML=response[1];

					
				}
			
		   }
    

   }
   }
   
   
    function ajax_getRootPath2() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (postPath);
}
     function ajax_set_ajax2(t1)
	  {

	      var url = "/count_table.aspx?type=pl&id="+t1+"&sid=" + Math.random();
		 
	        request.open("GET", url, true);
            request.onreadystatechange = ajax_loadPage2;
            request.send(null);

	  }

    function ajax_loadPage2() {
	        if (request.readyState == 4) 
			{
		   if (request.status == 200) 
		   {
			   	if(request.responseText!="")
				{
					var response = request.responseText.split("{fzw:next}");
					document.getElementById("pl_count").innerHTML=response[1]+"条评论";
					document.getElementById("pl_count_1").innerHTML=response[1];
					document.getElementById("pinglun_content").innerHTML=response[0];
					//document.writeln(request.responseText);
					
				}
			
		   }
    

   }
   }
     