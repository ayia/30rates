using Api.Models;
using HtmlAgilityPack;
using RestSharp;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Api.DTO
{
    public class DTO
    {
        private static HttpClient client = new HttpClient();

        private static async Task<string> GetHtmlAsync(string url)
        {
            var options = new RestClientOptions()
            {
                Timeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(url, Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            return (response.Content);
        }

        public async Task<Prediction> GetDataAsync(string url)
        {
            var prediction = new Prediction();

            string html = await GetHtmlAsync(url);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode h2Node = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']/h2");
            if (h2Node != null)
            {
                prediction.Date = h2Node.InnerText;

                // Extracting the date portion
                string dateFormat = "yyyy/MM/dd";
                string dateStr = prediction.Date.Substring(0, prediction.Date.IndexOf('.')).Trim();
                if (DateTime.TryParseExact(dateStr, dateFormat, null, DateTimeStyles.None, out var date))
                {
                    // Getting the date in a specific format
                    prediction.Date = date.ToString("yyyy-MM-dd");
                }
            }

            // Use XPath to select the <strong> element containing the decimal value
            HtmlNode strongNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']//strong");
            if (strongNode != null)
            {
                // Extract the decimal value using a regular expression
                string decimalValue = Regex.Match(strongNode.InnerHtml, @"\d+\.\d+").Value;
                if (decimal.TryParse(decimalValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var closingPrice))
                {
                    // Convert the decimal value to a decimal type
                    prediction.ClosingPrice = closingPrice;
                }
            }

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

                    if (decimal.TryParse(rangeStart, NumberStyles.Float, CultureInfo.InvariantCulture, out var lowPrice))
                    {
                        prediction.LowPrice = lowPrice;
                    }
                    if (decimal.TryParse(rangeEnd, NumberStyles.Float, CultureInfo.InvariantCulture, out var highPrice))
                    {
                        prediction.HighPrice = highPrice;
                    }
                }
            }

            //Direction
            // Find the <img> element within the <td> element
            HtmlNode imgNode = htmlDoc.DocumentNode.SelectSingleNode("//td/img");
            // Extract the value of the "alt" attribute
            prediction.Direction = imgNode?.GetAttributeValue("alt", "");

            return prediction;
        }
    }
}