using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Compat.Web;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace TurkishLanguageLibraryCore
{
    public class HTMLParser
    {
        public static HtmlNodeCollection GetElementsByClassName(HtmlAgilityPack.HtmlDocument htmlDocument, string className)
        {
            string xpath =
                String.Format(
                    "//*[contains(concat(' ', normalize-space(@class), ' '), ' {0} ')]",
                    className);
            return htmlDocument.DocumentNode.SelectNodes(xpath);
        }

        public static HtmlNodeCollection GetElementsById(HtmlAgilityPack.HtmlDocument htmlDocument, string ID)
        {
            string xpath =
                String.Format(
                    "//*[contains(concat(' ', normalize-space(@id), ' '), ' {0} ')]",
                    ID);
            return htmlDocument.DocumentNode.SelectNodes(xpath);
        }
        public static HtmlNodeCollection GetElementsByName(HtmlAgilityPack.HtmlDocument htmlDocument, string name)
        {
            string xpath =
                String.Format(
                    "//*[contains(concat(' ', normalize-space(@name), ' '), ' {0} ')]",
                    name);
            return htmlDocument.DocumentNode.SelectNodes(xpath);
        }
        public static HtmlNodeCollection GetElementsByTagName(HtmlAgilityPack.HtmlDocument htmlDocument, string TagName)
        {
            string xpath =
                String.Format(
                    "//{0}",
                    TagName);
            return htmlDocument.DocumentNode.SelectNodes(xpath); 
        }

        public static HtmlNodeCollection GetElementsByClassName(string htmlDocument, string className)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlDocument);
            string xpath =
                String.Format(
                    "//*[contains(concat(' ', normalize-space(@class), ' '), ' {0} ')]",
                    className);
            return document.DocumentNode.SelectNodes(xpath);
        }

        public static HtmlNodeCollection GetElementsById(string htmlDocument, string ID)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlDocument);
            string xpath =
                String.Format(
                    "//*[contains(concat(' ', normalize-space(@id), ' '), ' {0} ')]",
                    ID);
            return document.DocumentNode.SelectNodes(xpath);
        }
        public static HtmlNodeCollection GetElementsByName(string htmlDocument, string name)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlDocument);
            string xpath =
                String.Format(
                    "//*[contains(concat(' ', normalize-space(@name), ' '), ' {0} ')]",
                    name);
            return document.DocumentNode.SelectNodes(xpath);
        }
        public static HtmlNodeCollection GetElementsByTagName(string htmlDocument, string TagName)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlDocument);
            string xpath =
                String.Format(
                    "//{0}",
                    TagName);
            return document.DocumentNode.SelectNodes(xpath);
        }

        public static HtmlNodeCollection GetElementsByXpath(string htmlDocument, string xpath)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlDocument);
            return document.DocumentNode.SelectNodes(xpath);
        }

        public static string DecodeHtml(string input)
        {
            input = input.Replace("\\\"", "\"");
            input = input.Replace("\\u003C", "<");
            input = input.Replace("&quot;", "\"");
            input = input.Replace("&#123", "{");
            input = input.Replace("&#125", "}");
            input = input.Replace("&#039", "'");
            input = input.Replace("&amp;", "&");
            input = input.Replace("\\/", "/");
            input = HttpUtility.HtmlDecode(input);
            input = input.Replace("<!--", "").Replace("-->", "");
            input = input.Replace("\\u00e7", "ç");
            input = input.Replace("\\u00f6", "ö");
            input = input.Replace("\\u015f", "ş");
            input = input.Replace("\\u00fc", "ü");
            input = input.Replace("\\u011f", "ğ");
            input = input.Replace("\\u0131", "ı");
            input = input.Replace("\\u00c7", "Ç");
            input = input.Replace("\\u00d6", "Ö");
            input = input.Replace("\\u0130", "İ");
            input = input.Replace("\\u015e", "Ş");
            input = input.Replace("\\u00dc", "Ü");
            input = input.Replace("\\u011e", "Ğ");
            input = DecodeHtmlCharCodes(input);
            try
            {
                input = Regex.Unescape(input);
            }
            catch (Exception)
            {
            }

            return input;
        }

        public string GetCssClassByName(string htmlDocument,string className)
        {
            var result = Regex.Match(htmlDocument, "/\\.([\\w\\d\\.-]+)[^{}]*{[^}]*}/");

            return result.Value;
        }

        public static List<HtmlNode> GetAllChildrenElements(string htmlContent)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);

            var elementsAll = document.DocumentNode.Descendants().ToList();
            return elementsAll;
        }

        public static string RemoveHtmlElementByTagName(string htmlContent, string tagName)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);

            var nodeList = document.DocumentNode.SelectNodes("//ul");
            if (nodeList == null)
                return htmlContent;

            foreach (var node in nodeList)
            {
                node.ParentNode.RemoveChild(node);
            }
            return document.DocumentNode.InnerHtml;
        }

        public static string ClearHtmlElements(string htmlContent)
        {
           return Regex.Replace(htmlContent, "<.*?>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        public string GetStyleWithClassName(string htmlDocument, string className, string styleName)
        {
            var result = Regex.Match(htmlDocument, "(\\.|#)(" + className + ")({.*?})", RegexOptions.Multiline).Value;

            var stylesMaches = Regex.Matches(result, "([\\w-]+)\\s*:\\s*([^;]+)\\s*;?");

            foreach (var match in stylesMaches)
            {
                var name = match.ToString().Split(':')[0];
                var value = match.ToString().Split(':')[1].Replace(";", "");

                if (name.ToLower() == styleName.ToLower())
                    return value;
            }

            return null;
        }

        public static string DecodeHtmlCharCodes(string htmlContent)
        {
            try
            {

                var matches = Regex.Matches(htmlContent, "&#x(.*?);");
                foreach (Match match in matches)
                {
                    int unicode = Convert.ToInt32(match.Groups[1].Value, 16);
                    char character = (char)unicode;
                    htmlContent = htmlContent.Replace(match.Groups[0].Value, character.ToString());
                }

                return htmlContent;
            }
            catch (Exception)
            {

                return htmlContent;
            }
        }

    }
}
