
using Api.Models;
using HtmlAgilityPack;
using System.Globalization;
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
            a.ClosingPrice= decimal.Parse(decimalValue, CultureInfo.InvariantCulture);
            HtmlNode rangeNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']//p");


            if (rangeNode != null && rangeNode.InnerText.Contains("Today's range:"))
            
                {
                string rangeText = rangeNode.InnerText;
                string pattern = @"(\d+\.\d+)-(\d+\.\d+)";
                Match match = Regex.Match(rangeText, pattern);

                if (match.Success && match.Groups.Count >= 3)
                {
                    string rangeStart = match.Groups[1].Value;
                    string rangeEnd = match.Groups[2].Value;

                    a.LowPrice = Decimal.Parse(rangeStart, CultureInfo.InvariantCulture);
                    a.HighPrice  = Decimal.Parse(rangeEnd, CultureInfo.InvariantCulture);
                   
                }
            }

            return a;
        }

    }
}
