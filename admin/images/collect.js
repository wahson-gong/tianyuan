function feiye_leixin()
{
       var boxes = document.getElementsByName("RadioButtonList2"); 

       if(boxes[1].checked)
       {
			
			document.getElementById("feiye_leixin1").style.display = "";
			document.getElementById("feiye_leixin2").style.display = "";
       }
       else
       {
           document.getElementById("feiye_leixin1").style.display = "none";
           document.getElementById("feiye_leixin2").style.display = "none";
       }

}
function feiye_fenye(g1)
{
       var boxes = document.getElementsByName("div_Fields" + g1 + "_r"); 
       if(boxes[0].checked)
       {
			document.getElementById("div_Fields" + g1 + "_d_1").style.display="none";
			document.getElementById("div_Fields" + g1 + "_d_2").style.display="none";
			document.getElementById("div_Fields" + g1 + "_d_3").style.display="none";
			document.getElementById("div_Fields" + g1 + "_d_4").style.display="none";
       }
       else
       {
        	document.getElementById("div_Fields" + g1 + "_d_1").style.display="";
			document.getElementById("div_Fields" + g1 + "_d_2").style.display="";
			document.getElementById("div_Fields" + g1 + "_d_3").style.display="";
			document.getElementById("div_Fields" + g1 + "_d_4").style.display="";
       }

}

function feiye_Panel(g1)
{
	if(g1==3)
	{
		for(i=document.getElementById("ListBox1").options.length-1;i>=0;i--)document.getElementById("ListBox1").options.remove(i);
		collect_ajax(3);	
	}
	if(g1==5)
	{
		if(document.getElementById("Label4").innerHTML==""||document.getElementById("Label4").innerHTML=="加载中..")
		{
			collect_ajax(5);	
		}	
	}
	if(g1==6)
	{
		document.getElementById("Label5").innerHTML="加载中..";
		collect_ajax(6);	
	}
	if(g1==8)
	{
		document.getElementById("Label6").innerHTML="加载中..";
		collect_ajax(8);
	}
	if(g1==10)
	{
		document.getElementById("Label8").innerHTML="加载中..";
		collect_ajax(10);
	}
     for(i=1;i<=10;i++)
	 {
		if(g1==i)
		{
			document.getElementById("Panel"+i).style.display="";
			}
		else
		{
			document.getElementById("Panel"+i).style.display="none";
			}
	
	}
return false;
}

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


function collect_ajax(g1)
	  {
            var url = "";
			if(g1==3)
			{
					var u7=document.getElementById("TextBox6").value;
					var u8=document.getElementById("TextBox7").value;
					var u9=document.getElementById("TextBox8").value;
					var u10=document.getElementById("TextBox9").value;
					var web_url=document.getElementById("TextBox2").value;
					var boxes = document.getElementsByName("RadioButtonList1"); 
				    var bianma="";
				    for (var i = 0; i < boxes.length; i++)   
				    {
					     if (boxes[i].checked)   
					     {
						     bianma=boxes[i].value;
					     }
				    }
				
					url="collect_ajax.aspx?type="+g1+"&u7="+escape(u7)+"&u8="+escape(u8)+"&u9="+escape(u9)+"&u10="+escape(u10)+"&web_url="+escape(web_url)+"&bianma="+bianma+"&sid="+ Math.random();
					
}
			else if(g1==4)
			{
				for(i=document.getElementById("DropDownList2").options.length-1;i>=0;i--)document.getElementById("DropDownList2").options.remove(i);
				 var u11="";
					var list=document.getElementById("DropDownList1");
					for (var i=0; i<list.options.length; i++)
					{
						if(list.options[i].selected)
						{
							u11=list.options[i].value;
						}
					}	
					url="collect_ajax.aspx?type="+g1+"&u11="+escape(u11)+"&sid="+ Math.random();
					

			}
			else if(g1==5)
			{

				var boxes = document.getElementsByName("che_Fields"); 
				var u12="";
				for (var i = 0; i < boxes.length; i++)   
				{
			
					 if (boxes[i].checked)   
					 {
						 if(u12=="")
						 {
							u12=document.getElementById("div_Fields"+i).innerHTML+","+i;	 
						 }
						 else
						 {
							 u12=u12+"{fzw:che}"+document.getElementById("div_Fields"+i).innerHTML+","+i;
						}
					 }
				}
				var u11="";
					var list=document.getElementById("DropDownList1");
					for (var i=0; i<list.options.length; i++)
					{
						if(list.options[i].selected)
						{
							u11=list.options[i].value;
						}
					}	
					url="collect_ajax.aspx?type="+g1+"&u12="+escape(u12)+"&u11="+escape(u11)+"&sid="+ Math.random();
					
			}
			else if(g1==6)
			{
				var boxes = document.getElementsByName("che_Fields"); 
				var u12="";
			
				for (var i = 0; i < boxes.length; i++)   
				{
				    
					 if (boxes[i].checked)   
					 {

						 if(u12=="")
						 {
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
							
							     u12 = document.getElementById("div_Fields" + i).innerHTML + "{fzw:Field}" + i + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_qian").value + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_hou").value;
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
							  
							     u12 = document.getElementById("div_Fields" + i).innerHTML + "{fzw:Field}" + i + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_qian").value + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_hou").value;
							     //alert(u12);
							}
							
						 }
						 else
						 {
					
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
						
							     u12 = u12 + "{fzw:che}" + document.getElementById("div_Fields" + i).innerHTML + "{fzw:Field}" + i + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_qian").value + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_hou").value;
		
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
								
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
						        					
							     u12 = u12 + "{fzw:che}" + document.getElementById("div_Fields" + i).innerHTML + "{fzw:Field}" + i + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_qian").value + "{fzw:Field}" + document.getElementById("div_Fields" + i + "_hou").value;
					
							}
							 
						}
					 }
				} 
				url = "collect_ajax.aspx?type=" + g1 + "&u12=" + escape(u12) + "&u11=" + escape(document.getElementById("ListBox1").options[document.getElementById("ListBox1").selectedIndex].text) + "&sid=" + Math.random();
				//alert(url);
	//document.write(url);
					
			}
			else if(g1==8)
			{
				var boxes = document.getElementsByName("che_Fields"); 
				var u12="";
				for (var i = 0; i < boxes.length; i++)   
				{
					 if (boxes[i].checked)   
					 {
						 if(u12=="")
						 {
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
								 u12=document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;	
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
								u12=document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;	 
							}
							
						 }
						 else
						 {
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
								 u12=u12+"{fzw:che}"+document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;
								 
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
								u12=u12+"{fzw:che}"+document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;
							}
							 
						}
					 }
				}
				var u13=document.getElementById("TextBox10").value+"{fzw:Field}"+document.getElementById("TextBox11").value+"{fzw:Field}"+document.getElementById("TextBox12").value+"{fzw:Field}"+document.getElementById("TextBox13").value+"{fzw:Field}"+document.getElementById("TextBox14").value+"{fzw:Field}"+document.getElementById("TextBox15").value;
				
					url="collect_ajax.aspx?type="+g1+"&u12="+escape(u12)+"&u11="+escape(document.getElementById("ListBox1").options[document.getElementById("ListBox1").selectedIndex].text)+"&u13="+escape(u13)+"&sid="+ Math.random();
				
			}
			else if(g1==10)
			{
				var boxes = document.getElementsByName("che_Fields"); 
				var u12="";
				for (var i = 0; i < boxes.length; i++)   
				{
					 if (boxes[i].checked)   
					 {
						 if(u12=="")
						 {
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
								 u12=document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;	
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
								u12=document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;	 
							}
							
						 }
						 else
						 {
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
								 u12=u12+"{fzw:che}"+document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;
								 
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
								u12=u12+"{fzw:che}"+document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;
							}
							 
						}
					 }
				}
				var u13=document.getElementById("TextBox10").value+"{fzw:Field}"+document.getElementById("TextBox11").value+"{fzw:Field}"+document.getElementById("TextBox12").value+"{fzw:Field}"+document.getElementById("TextBox13").value+"{fzw:Field}"+document.getElementById("TextBox14").value+"{fzw:Field}"+document.getElementById("TextBox15").value;
				var boxes = document.getElementsByName("che_html"); 
				var u14="";
				for (var i = 0; i < boxes.length; i++)   
				{
					if (boxes[i].checked)   
					 {
						 if(u14=="")
						 {
							 u14=boxes[i].value;
						 }
						 else
						 {
								u14=u14+","+boxes[i].value; 
						}
					 }
					 
				}
					url="collect_ajax.aspx?type="+g1+"&u12="+escape(u12)+"&u11="+escape(document.getElementById("ListBox1").options[document.getElementById("ListBox1").selectedIndex].text)+"&u13="+escape(u13)+"&u14="+escape(u14)+"&sid="+ Math.random();

			}
			
	        request.open("GET", url, true);
            request.onreadystatechange = collect_loding;
            request.send(null);
	  }
   
    function collect_loding() {
	        if (request.readyState == 4) 
			{
		   if (request.status == 200) 
		   {
			   	if(request.responseText!="")
				{
					var response = request.responseText.split("{fzw:collect_ajax}"); 
					if(response[0]=="3")
					{
						if(response[1]=="ok")
						{
							  var osel = document.getElementById("ListBox1");
							 	var newarr = response[2].split(","); 
								for(var i=0; i<newarr.length; i++)
								{
									osel.options.add(new Option(newarr[i]))
								}
								osel.options[0].selected="selected";
								document.getElementById("Label1").innerHTML=document.getElementById("TextBox2").value;
								document.getElementById("Label2").innerHTML=newarr.length+"条";
						}
						else
						{
							alert("没有得到数据");
							feiye_Panel(1);
						}
					}
					else if(response[0]=="4")
					{
										var osel = document.getElementById("DropDownList2");
							 	var newarr = response[2].split("{fzw:che}"); 
								for(var i=0; i<newarr.length; i++)
								{
									osel.options.add(new Option(newarr[i].split("{fzw:Field}")[0],newarr[i].split("{fzw:Field}")[1]))
								}
								
						document.getElementById("Label3").innerHTML=response[1];
					}
					else if(response[0]=="5")
					{
						document.getElementById("Label4").innerHTML=response[1];
					}
					else if(response[0]=="6")
					{
						document.getElementById("Label5").innerHTML=response[1];
					}
					else if(response[0]=="8")
					{
						document.getElementById("Label6").innerHTML=response[1];
					}
					else if(response[0]=="10")
					{
						document.getElementById("Label8").innerHTML=response[1];
					}
				}
			
		   }
      else{
         alert("status is " + request.status);  
     	 }

   }
   }

function return_c()
{
	var boxes = document.getElementsByName("che_Fields"); 
				var u12="";
				for (var i = 0; i < boxes.length; i++)   
				{
					 if (boxes[i].checked)   
					 {
						 if(u12=="")
						 {
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
								 u12=document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;	
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
								u12=document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;	 
							}
							
						 }
						 else
						 {
							 if(document.getElementById("div_Fields" + i + "_type").innerHTML=="编辑器")
							 {
								 u12=u12+"{fzw:che}"+document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;
								 
								 var boxes1 = document.getElementsByName("div_Fields" + i + "_r"); 
								   if(boxes1[1].checked)
								   {
									 u12=u12+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_1").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_2").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_3").innerHTML+"{fzw:Field}"+document.getElementById("div_Fields" + i + "_r_4").innerHTML; 
								   }
							}
							else
							{
								u12=u12+"{fzw:che}"+document.getElementById("div_Fields"+i).innerHTML+"{fzw:Field}"+i+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_qian").value+"{fzw:Field}"+document.getElementById("div_Fields"+i+"_hou").value;
							}
							 
						}
					 }
				}
		document.getElementById("TextBox16").value=unescape(u12);
		
		var u13=document.getElementById("TextBox10").value+"{fzw:Field}"+document.getElementById("TextBox11").value+"{fzw:Field}"+document.getElementById("TextBox12").value+"{fzw:Field}"+document.getElementById("TextBox13").value+"{fzw:Field}"+document.getElementById("TextBox14").value+"{fzw:Field}"+document.getElementById("TextBox15").value;
		document.getElementById("TextBox17").value=unescape(u13);
		
		var boxes = document.getElementsByName("che_html"); 
				var u14="";
				for (var i = 0; i < boxes.length; i++)   
				{
					if (boxes[i].checked)   
					 {
						 if(u14=="")
						 {
							 u14=boxes[i].value;
						 }
						 else
						 {
								u14=u14+","+boxes[i].value; 
						}
					 }
					 
				}
				document.getElementById("TextBox18").value=unescape(u14);
}






















