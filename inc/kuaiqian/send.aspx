<script Language="C#" runat="server">
/**
 * @Description: ��Ǯ�����֧�����ؽӿڷ���
 * @Copyright (c) �Ϻ���Ǯ��Ϣ�������޹�˾
 * @version 2.0
 */
    my_basic my_b = new my_basic();

void Page_Load(Object sender, EventArgs E){  

//����������˻���
///���¼��Ǯϵͳ��ȡ�û���ţ��û���ź��01��Ϊ����������˻��š�
    merchantAcctId.Value = my_b.get_pay("��Ǯ", "hezuoid");

//�����������Կ
///���ִ�Сд.�����Ǯ��ϵ��ȡ
    String key = my_b.get_pay("��Ǯ", "zhifukeyma");

//�ַ���.�̶�ѡ��ֵ����Ϊ�ա�
///ֻ��ѡ��1��2��3.
///1����UTF-8; 2����GBK; 3����gb2312
///Ĭ��ֵΪ1
///�����web.config�ļ��������˱��뷽ʽ������<globalization requestEncoding="utf-8" responseEncoding="utf-8"/>����δ����Ĭ��Ϊutf-8����
///��ô��inputCharset��ȡֵӦ�������õı��뷽ʽ��һ��
inputCharset.Value="3";


//����������֧������ĺ�̨��ַ.��[pageUrl]����ͬʱΪ�ա������Ǿ��Ե�ַ��
///��Ǯͨ�����������ӵķ�ʽ�����׽�����͵�[bgUrl]��Ӧ��ҳ���ַ�����̻�������ɺ������<result>���Ϊ1��ҳ���ת��<redirecturl>��Ӧ�ĵ�ַ��
///�����Ǯδ���յ�<redirecturl>��Ӧ�ĵ�ַ����Ǯ����֧�����GET��[pageUrl]��Ӧ��ҳ�档
bgUrl.Value = my_b.get_Domain() + "/inc/kuaiqian/receive.aspx";
	
//���ذ汾.�̶�ֵ
///��Ǯ����ݰ汾�������ö�Ӧ�Ľӿڴ�������
///������汾�Ź̶�Ϊv2.0
version.Value="v2.0";

//��������.�̶�ѡ��ֵ��
///ֻ��ѡ��1��2��3
///1�������ģ�2����Ӣ��
///Ĭ��ֵΪ1
language.Value="1";

//ǩ������.�̶�ֵ
///1����MD5ǩ��
///��ǰ�汾�̶�Ϊ1
signType.Value="1";
   
//֧��������
///��Ϊ���Ļ�Ӣ���ַ�
payerName.Value = "֧����";

//֧������ϵ��ʽ����.�̶�ѡ��ֵ
///ֻ��ѡ��1
///1����Email
payerContactType.Value="1";

//֧������ϵ��ʽ
///ֻ��ѡ��Email���ֻ���
payerContact.Value="";

//�̻�������
///����ĸ�����֡���[-][_]���
orderId.Value = Request.QueryString["v_oid"].ToString();

//�������
///�Է�Ϊ��λ����������������
///�ȷ�2������0.02Ԫ
orderAmount.Value = Request.QueryString["v_amount"].ToString();
	
//�����ύʱ��
///14λ���֡���[4λ]��[2λ]��[2λ]ʱ[2λ]��[2λ]��[2λ]
///�磻20080101010101
orderTime.Value=DateTime.Now.ToString("yyyyMMddHHmmss");

//��Ʒ����
///��Ϊ���Ļ�Ӣ���ַ�
productName.Value = "Ӣ���̳�";

//��Ʒ����
///��Ϊ�գ��ǿ�ʱ����Ϊ����
productNum.Value="1";

//��Ʒ����
///��Ϊ�ַ���������
productId.Value="";

//��Ʒ����
productDesc.Value = "���������̳�";
	
//��չ�ֶ�1
///��֧��������ԭ�����ظ��̻�
ext1.Value="";

//��չ�ֶ�2
///��֧��������ԭ�����ظ��̻�
ext2.Value="";
	
//֧����ʽ.�̶�ѡ��ֵ
///ֻ��ѡ��00��10��11��12��13��14
///00�����֧��������֧��ҳ����ʾ��Ǯ֧�ֵĸ���֧����ʽ���Ƽ�ʹ�ã�10�����п�֧��������֧��ҳ��ֻ��ʾ���п�֧����.11���绰����֧��������֧��ҳ��ֻ��ʾ�绰֧����.12����Ǯ�˻�֧��������֧��ҳ��ֻ��ʾ��Ǯ�˻�֧����.13������֧��������֧��ҳ��ֻ��ʾ����֧����ʽ��
payType.Value="00";


//ͬһ������ֹ�ظ��ύ��־
///�̶�ѡ��ֵ�� 1��0
///1����ͬһ������ֻ�����ύ1�Σ�0��ʾͬһ��������û��֧���ɹ���ǰ���¿��ظ��ύ��Ρ�Ĭ��Ϊ0����ʵ�ﹺ�ﳵ�������̻�����0�������Ʒ���̻�����1
redoFlag.Value="0";

//��Ǯ�ĺ��������˻���
///��δ�Ϳ�Ǯǩ����������Э�飬����Ҫ��д������
pid.Value="";


	//���ɼ���ǩ����
	///����ذ�������˳��͹�����ɼ��ܴ���
	String signMsgVal="";
	signMsgVal=appendParam(signMsgVal,"inputCharset",inputCharset.Value);
	signMsgVal=appendParam(signMsgVal,"bgUrl",bgUrl.Value);
	signMsgVal=appendParam(signMsgVal,"version",version.Value);
	signMsgVal=appendParam(signMsgVal,"language",language.Value);
	signMsgVal=appendParam(signMsgVal,"signType",signType.Value);
	signMsgVal=appendParam(signMsgVal,"merchantAcctId",merchantAcctId.Value);
	signMsgVal=appendParam(signMsgVal,"payerName",payerName.Value);
	signMsgVal=appendParam(signMsgVal,"payerContactType",payerContactType.Value);
	signMsgVal=appendParam(signMsgVal,"payerContact",payerContact.Value);
	signMsgVal=appendParam(signMsgVal,"orderId",orderId.Value);
	signMsgVal=appendParam(signMsgVal,"orderAmount",orderAmount.Value);
	signMsgVal=appendParam(signMsgVal,"orderTime",orderTime.Value);
	signMsgVal=appendParam(signMsgVal,"productName",productName.Value);
	signMsgVal=appendParam(signMsgVal,"productNum",productNum.Value);
	signMsgVal=appendParam(signMsgVal,"productId",productId.Value);
	signMsgVal=appendParam(signMsgVal,"productDesc",productDesc.Value);
	signMsgVal=appendParam(signMsgVal,"ext1",ext1.Value);
	signMsgVal=appendParam(signMsgVal,"ext2",ext2.Value);
	signMsgVal=appendParam(signMsgVal,"payType",payType.Value);	
	signMsgVal=appendParam(signMsgVal,"redoFlag",redoFlag.Value);
	signMsgVal=appendParam(signMsgVal,"pid",pid.Value);
	signMsgVal=appendParam(signMsgVal,"key",key);

    //�����web.config�ļ��������˱��뷽ʽ������<globalization requestEncoding="utf-8" responseEncoding="utf-8"/>����δ����Ĭ��Ϊutf-8����
    //��ô��inputCharset��ȡֵӦ�������õı��뷽ʽ��һ�£�
    //ͬʱ��GetMD5()�����������ݵı��뷽ʽҲ������˱���һ�¡�
    signMsg.Value = GetMD5(signMsgVal, "gb2312").ToUpper();



//��ӡ�ύ��Ϣ
Lab_orderId.Text=orderId.Value;
Lab_orderAmount.Text=orderAmount.Value;
Lab_payerName.Text=payerName.Value;
Lab_productName.Text=productName.Value;

}
   

	//���ܺ�����������ֵ��Ϊ�յĲ�������ַ���
	String appendParam(String returnStr,String paramId,String paramValue){

		if(returnStr!=""){
			
				if(paramValue!=""){
					
					returnStr+="&"+paramId+"="+paramValue;
				}
			
		}else{
			
			if(paramValue!=""){
				returnStr=paramId+"="+paramValue;
			}
		}
		
		return returnStr;
	}
	//���ܺ�����������ֵ��Ϊ�յĲ�������ַ���������



    //���ܺ��������ַ������б����ʽת����������MD5���ܣ�Ȼ�󷵻ء���ʼ
    private static string GetMD5(string dataStr, string codeType)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(codeType).GetBytes(dataStr));
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        for (int i = 0; i < t.Length; i++)
        {
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        }
        return sb.ToString();
    }
    //���ܺ��������ַ������б����ʽת����������MD5���ܣ�Ȼ�󷵻ء�����
    
</script>

<!doctype html public "-//w3c//dtd html 4.0 transitional//en" >
<html>
	<head>
		<title>ʹ�ÿ�Ǯ֧��</title>

	</head>
	
<body  onLoad="javascript:document.kqPay.submit()">
	
	<div align="center">
		<table width="259" border="0" cellpadding="1" cellspacing="1" bgcolor="#CCCCCC" >
			<tr bgcolor="#FFFFFF">
				<td width="80">֧����ʽ:</td>
				<td >��Ǯ[99bill]</td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td >�������:</td>
				<td ><asp:label id="Lab_orderId" RunAt="Server"/></td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td>�������:</td>
				<td><asp:label id="Lab_orderAmount" RunAt="Server"/></td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td>֧����:</td>
				<td><asp:label id="Lab_payerName" RunAt="Server"/></td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td>��Ʒ����:</td>
				<td><asp:label id="Lab_productName" RunAt="Server"/></td>
			</tr>
			<tr>
				<td></td>
				<td></td>
			</tr>
	  </table>
	</div>


		<form name="kqPay" method="post" action="https://www.99bill.com/gateway/recvMerchantInfoAction.htm"/>
			<input type="hidden" id="inputCharset"  runat="server"/>
			<input type="hidden" id="bgUrl"  runat="server"/>
			<input type="hidden" id="version"  runat="server"/>
			<input type="hidden" id="language"  runat="server"/>
			<input type="hidden" id="signType"  runat="server"/>
			<input type="hidden" id="signMsg"  runat="server"/>
			<input type="hidden" id="merchantAcctId"  runat="server"/>
			<input type="hidden" id="payerName"  runat="server"/>
			<input type="hidden" id="payerContactType"  runat="server"/>
			<input type="hidden" id="payerContact"  runat="server"/>
			<input type="hidden" id="orderId"  runat="server"/>
			<input type="hidden" id="orderAmount"  runat="server"/>
			<input type="hidden" id="orderTime"  runat="server"/>
			<input type="hidden" id="productName"  runat="server"/>
			<input type="hidden" id="productNum"  runat="server"/>
			<input type="hidden" id="productId"  runat="server"/>
			<input type="hidden" id="productDesc"  runat="server"/>
			<input type="hidden" id="ext1"  runat="server"/>
			<input type="hidden" id="ext2"  runat="server"/>
			<input type="hidden" id="payType"  runat="server"/>
			<input type="hidden" id="redoFlag"  runat="server"/>	
			<input type="hidden" id="pid"  runat="server"/>	
		</form>		

	
</body>
</html>