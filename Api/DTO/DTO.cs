﻿
using Api.Models;
using HtmlAgilityPack;


using System.Globalization;

using System.Net.Http.Headers;

using System.Text.RegularExpressions;

using System.Web;

namespace Api.DTO
{
    public class DTO
    {
       
        private async Task<string> GetHtmlAsync(string url)
        {


            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://app.scrapingbee.com/api/v1/?api_key=X7TFNDUFVT6ZIGRY5SKPB4PE7EPUK6AT0BWEN6DVFH64K1ED4RARTMAAD1DVUJX323492A7JWU029D1G&url="+ HttpUtility.UrlEncode(url));
             var content = new StringContent(string.Empty);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            request.Content = content;
            var response =  client.Send(request);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadAsStringAsync());

        }

        public async Task<Prediction> GetDataAsync(string url) {

            Prediction a= new Prediction();

            string html = await GetHtmlAsync(url);
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