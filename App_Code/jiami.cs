using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Web.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Cryptography;

/// <summary>
/// jiami 的摘要说明
/// </summary>
public class jiami
{
    /// <summary>
    /// 有密码的AES加密 
    /// </summary>
    /// <param name="text">加密字符</param>
    /// <param name="password">加密的密码</param>
    /// <param name="iv">密钥</param>
    /// <returns></returns>
    public string Encrypt(string toEncrypt)
    {
        string key = ConfigurationSettings.AppSettings["loginkey"].ToString();
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt.Replace(" ", "+"));

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="text"></param>
    /// <param name="password"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public string Decrypt(string toDecrypt)
    {
        try
        {
            #region 龚华尧 补充base64解密函数
            toDecrypt = toDecrypt.Replace(" ", "+");
            int mod4 = toDecrypt.Length % 4;
            if (mod4 > 0)
            {
                toDecrypt += new string('=', 4 - mod4);
            }

            #endregion

            string key = ConfigurationSettings.AppSettings["loginkey"].ToString();
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray).Replace("+", " ");
        }
        catch
        {
            return toDecrypt;
        }
    }
}