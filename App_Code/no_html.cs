using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
/// <summary>
/// no_html 的摘要说明
/// </summary>
public class no_html
{
    my_basic my_b = new my_basic();
    public string set_url(string file_content, string home_url)
    {
        Regex reg = new Regex(@"href="".*?""|src="".*?""|background-image:url[\s\S]*?\)", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {
            string url_string = match.ToString();
            url_string = url_string.Replace("href=", "").Replace("src=", "").Replace("\"", "").Replace("background-image:url(", "").Replace(")", "");
            if (url_string.IndexOf("/") == 0 || url_string.IndexOf("http://") == -1)
            {
                if (url_string.IndexOf("/") == 0)
                {
                    file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, home_url + url_string));
                }
                else
                {
                    file_content = file_content.Replace(match.ToString(), match.ToString().Replace(url_string, home_url + "/" + url_string));
                }
            }

        }
        return file_content;
    }
    public string set_html(string g1, string g2, string home_url)
    {
        g2 = set_url(g2, home_url);
        if (g1 == "iframe： 过滤内联页")
        {
            Regex reg = new Regex("<iframe.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</iframe>", "");
        }
        else if (g1 == "Object：过滤Flash，控件等")
        {
            Regex reg = new Regex(@"<object [\s\S]*?</object>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "script： 过滤js，vbs等脚本")
        {
            Regex reg = new Regex("<script.*?</script>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "div： 过滤层")
        {
            Regex reg = new Regex("<div.*?</div>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "div： 过滤层（保留内容）")
        {
            Regex reg = new Regex("<div.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</div>", "");
        }
        else if (g1 == "style： 过滤样式")
        {
            Regex reg = new Regex(@"style="".*?""|style=.*?[\S]", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "class： 过滤类名")
        {
            Regex reg = new Regex(@"class="".*?""|class=.*?[\S]", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "span： 过滤行内元素span容器")
        {
            Regex reg = new Regex("<span.*?</span>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "span： 过滤行内元素span容器（保留内容）")
        {
            Regex reg = new Regex("<span.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</span>", "");
        }
        else if (g1 == "过滤表格属性")
        {
            Regex reg = new Regex("<table.*?</table>", RegexOptions.Singleline);
            g2 = reg.Replace(g2, "");
        }
        else if (g1 == "过滤表格属性（字留下，表格属性去掉）")
        {
            Regex reg = new Regex("<table.*?>", RegexOptions.Singleline);
            g2 = reg.Replace(g2, "").Replace("</table>", "");

            Regex reg1 = new Regex("<tr.*?>", RegexOptions.Singleline);
            g2 = reg1.Replace(g2, "").Replace("</tr>", "");

            Regex reg2 = new Regex("<td.*?>", RegexOptions.Singleline);
            g2 = reg2.Replace(g2, "").Replace("</td>", "");
        }
        else if (g1 == "font： 过滤字体样式 （字留下，样式去掉）")
        {
            Regex reg = new Regex("<font.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</font>", "");
        }
        else if (g1 == "a： 过滤链接")
        {
            Regex reg = new Regex("<a.*?</a>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "a： 过滤链接 （字留下，链接去掉）")
        {
            Regex reg = new Regex("<a.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</a>", "");
        }
        else if (g1 == "<>： 过滤HTML标签 （剩下纯文字）")
        {
            return my_b.NoHTML(g2);
        }
        else if (g1 == "img ： 过滤图片")
        {
            Regex reg = new Regex("<img.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        return "";
    }
    public string set_html_replace(string g1, string g2)
    {

        if (g1 == "iframe： 过滤内联页")
        {
            Regex reg = new Regex("<iframe.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</iframe>", "");
        }
        else if (g1 == "Object：过滤Flash，控件等")
        {
            Regex reg = new Regex(@"<object [\s\S]*?</object>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "script： 过滤js，vbs等脚本")
        {
            Regex reg = new Regex("<script.*?</script>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "div： 过滤层")
        {
            Regex reg = new Regex("<div.*?</div>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "div： 过滤层（保留内容）")
        {
            Regex reg = new Regex("<div.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</div>", "");
        }
        else if (g1 == "style： 过滤样式")
        {
            Regex reg = new Regex(@"style="".*?""|style=.*?[\S]", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "class： 过滤类名")
        {
            Regex reg = new Regex(@"class="".*?""|class=.*?[\S]", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "span： 过滤行内元素span容器")
        {
            Regex reg = new Regex("<span.*?</span>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "span： 过滤行内元素span容器（保留内容）")
        {
            Regex reg = new Regex("<span.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</span>", "");
        }
        else if (g1 == "过滤表格属性")
        {
            Regex reg = new Regex("<table.*?</table>", RegexOptions.Singleline);
            g2 = reg.Replace(g2, "");
        }
        else if (g1 == "过滤表格属性（字留下，表格属性去掉）")
        {
            Regex reg = new Regex("<table.*?>", RegexOptions.Singleline);
            g2 = reg.Replace(g2, "").Replace("</table>", "");

            Regex reg1 = new Regex("<tr.*?>", RegexOptions.Singleline);
            g2 = reg1.Replace(g2, "").Replace("</tr>", "");

            Regex reg2 = new Regex("<td.*?>", RegexOptions.Singleline);
            g2 = reg2.Replace(g2, "").Replace("</td>", "");
        }
        else if (g1 == "font： 过滤字体样式 （字留下，样式去掉）")
        {
            Regex reg = new Regex("<font.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</font>", "");
        }
        else if (g1 == "a： 过滤链接")
        {
            Regex reg = new Regex("<a.*?</a>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        else if (g1 == "a： 过滤链接 （字留下，链接去掉）")
        {
            Regex reg = new Regex("<a.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "").Replace("</a>", "");
        }
        else if (g1 == "<>： 过滤HTML标签 （剩下纯文字）")
        {
            return my_b.NoHTML(g2);
        }
        else if (g1 == "img ： 过滤图片")
        {
            Regex reg = new Regex("<img.*?>", RegexOptions.Singleline);
            return reg.Replace(g2, "");
        }
        return "";
    }
}
