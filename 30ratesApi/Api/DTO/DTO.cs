
using Api.Models;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace Api.DTO
{
    public class DTO
    {

        private static string GetHtml(string url)
        {
            HttpClient client = new HttpClient();
            string v =  client.GetStringAsync(url).Result;
            return v;

        }

        public static Prediction GetData(string url) {

            Prediction a= new Prediction();
            string html = GetHtml(url);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode h2Node = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']/h2");
            a.Date= h2Node.InnerText;
            // Extracting the date portion
            string dateFormat = "yyyy/MM/dd";
            string dateStr = a.Date.Substring(0, a.Date.IndexOf('.')).Trim();

            DateTime date = DateTime.ParseExact(dateStr, dateFormat, null);
            // Getting the date in a specific format
            a.Date = date.ToString("yyyy-MM-dd");

            // Use XPath to select the <strong> element containing the decimal value
            HtmlNode strongNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']//strong");

            // Extract the decimal value using a regular expression
            string decimalValue = Regex.Match(strongNode.InnerHtml, @"\d+\.\d+").Value;

            // Convert the decimal value to a decimal type
            decimal decimalResult = decimal.Parse(decimalValue);

            return a;
        }

    }
}
