<%@ Page Language="C#" AutoEventWireup="true" CodeFile="uploadpic.aspx.cs" Inherits="admin_uploadpic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
<link href="images/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
 <table>
  <tr>
		<td valign="top">上传图片<span style="color:#cc0000">*</span></td>
		<td colspan="2">
        	<iframe src="Default2.aspx?editname=u7" width="600px" height="30px" frameborder="0" scrolling="no"></iframe>
        </td>
	  </tr>
	  <tr style="display:none" id="shangc">
		<td valign="top" onclick="Uploadworks()">已上传的</td>
		<td colspan="2" id="xiangce_content">
        </td>
	  </tr>
	  <tr>
	    <td colspan="3">
        <input type="text" name="u7" id="u7" value="" style="display:none" /><input type="text" name="u8" id="u8" value="" style=" display:none"/>
        </td>
	  </tr>
 </table>
 
 <script type="text/javascript">
function get_edit()
{
    var t1="<%=Request.QueryString["type"].ToString() %>";
    if(t1=="edit")
    {
 
        document.getElementById("u7").value=window.parent.document.getElementById("<%=Request.QueryString["textBox"].ToString() %>").value;
    }
}
get_edit();

function xiangce()
{
	if(document.getElementById("u7").value!="")
	{
	    document.getElementById("shangc").style.display="";
		var pic_content=document.getElementById("u7").value;
		var a=pic_content.split("{next}");  
			var xiangce_content="";
		for(i=0;i<a.length;i++)
		{
			var b=a[i].split("{title}");  
			xiangce_content="<table width=\"559px\" height=\"95px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"text-align:center\" id=\"xiangce_name"+i+"\">		  <tr>		    <td style=\"width:85px\"><a href=\"javascript:next_xiangce("+i+",'shang')\"><img src=\"images/ico30_03.jpg\" /></a>&nbsp;<a href=\"javascript:next_xiangce("+i+",'xia')\"><img src=\"images/ico30_05.jpg\" /></a>&nbsp;<a href=\"javascript:del_xiangce("+i+")\"><img src=\"images/ico30_07.jpg\" /></a></td>		    <td style=\"width:105px\"><img src=\""+b[0]+"\"  width=\"100px\" height=\"90px\"/></td>		    <td><textarea  style=\"border:1px solid #BFBFBF; background-color:#F5F5F5; width:310px; height:75px;\" id=\"xiangce_name_t"+i+"\"  onchange=\"get_xiangce()\">"+b[1]+"</textarea></td>		    </tr>	  </table>"+xiangce_content;
		}
	
		
		document.getElementById("xiangce_content").innerHTML=xiangce_content;
	}
	else
	{
	document.getElementById("shangc").style.display="none";
		document.getElementById("xiangce_content").innerHTML="";	
	}
	
	 window.parent.document.getElementById("<%=Request.QueryString["textBox"].ToString() %>").value=document.getElementById("u7").value;
  window.parent.document.getElementById("<%=Request.QueryString["id"].ToString() %>").style.height=document.documentElement.scrollHeight+"px";
 }
function del_xiangce(g1)
{
		var pic_content=document.getElementById("u7").value;
	
		var a=pic_content.split("{next}");  
		var  p_content="";	
		
		for(i=0;i<a.length;i++)
		{
			if(g1!=i)
			{
				var b=a[i].split("{title}");  
			if(i==a.length-1)
				{
			document.getElementById("u8").value=b[0];
				}
			
				if(p_content!="")
				{
					p_content+="{next}"+b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
				}
				else
				{
					p_content=b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
				}
			}
		}
		
		document.getElementById("u7").value=p_content;
		xiangce();
}
function next_xiangce(g1,g2)
{
		var pic_content=document.getElementById("u7").value;

		var a=pic_content.split("{next}");  
		var  p_content="";	
		if(g2=="shang")
		{	
		
			if(g1<a.length-1)	
			{
				
				for(i=0;i<a.length;i++)
				{
						if(g1==i-1)
						{
							
							var b=a[i].split("{title}");  
							if(i==a.length-1)
				            {
            						
				            }
						    p_content=b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value+"{next}"+p_content;
						}
						else
						{
							var b=a[i].split("{title}");  
							if(i==a.length-1)
				            {
            		
				            }
						
							if(p_content!="")
							{
								p_content=p_content+"{next}"+b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
							}
							else
							{
								p_content=b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
							}	
						}
				}
				//end
					document.getElementById("u7").value=p_content;
					get_xiangce();
		xiangce();

			}
		
		}
		if(g2=="xia")
		{	
		
			if(g1>0)	
			{
				
				for(i=0;i<a.length;i++)
				{
						if(g1==i)
						{
							
							var b=a[i].split("{title}");  
							if(i==a.length-1)
				{
	
				}
						     p_content=b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value+"{next}"+p_content;
							
						}
						else
						{
							var b=a[i].split("{title}");  
							if(i==a.length-1)
				{

				}
						
							if(p_content!="")
							{
									p_content=p_content+"{next}"+b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
							}
							else
							{
								p_content=b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
							}	
						}
				}
					
				//end
				document.getElementById("u7").value=p_content;
			
				get_xiangce();
		        xiangce();
			}
			
		}
		//shang end
		
		
			var a=document.getElementById("u7").value.split("{next}");  

			document.getElementById("u8").value=a[a.length-1].replace("{title}","");	
			
}

function get_xiangce()
{

		var pic_content=document.getElementById("u7").value;
	
		var a=pic_content.split("{next}");  
		var  p_content="";	
		
		for(i=0;i<a.length;i++)
		{
				
				var b=a[i].split("{title}");  
				if(i==a.length-1)
				{
			if(document.getElementById("u8").value=="")
			{
					document.getElementById("u8").value=b[0];
			}
				}
		
			if(p_content!="")
			{
				p_content+="{next}"+b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
			}
			else
			{
				p_content=b[0]+"{title}"+document.getElementById('xiangce_name_t'+i).value;
			}
		}
		document.getElementById("u7").value=p_content;
	    xiangce();
}


xiangce();
</script>
</body>
</html>
