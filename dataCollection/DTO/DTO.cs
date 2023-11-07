using dataCollection.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCollection.DTO
{
    public class DTO
    {

        public async Task<PredictionResult> getPredictionResultAsync()
        {
            try { 
            // Make the HTTP request to get the JSON response
            string url = "http://localhost:5000/api/Predictions";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            PredictionResult res= JsonConvert.DeserializeObject<PredictionResult>(jsonResponse);
            // Deserialize the JSON into a Rates30 object
            return res;
            }catch (Exception ex) {
                return null;
            }
            }
        
        public void Save(PredictionResult pr, TreadContext treadContext)
        {
            Rates30 hj=new Rates30()
            {
                Date = DateTime.Now,
                HighPrice = pr.HighPrice,
                LowPrice = pr.LowPrice,
                ClosingPrice = pr.ClosingPrice,

            };
            treadContext.Rates30s.Add(hj);
            treadContext.SaveChanges();

        }
    }
}
