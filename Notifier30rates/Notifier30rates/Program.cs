using System.Globalization;

internal class Program
{
    static Prediction prediction=null;
    private static void Main(string[] args)
    {    Timer timer = new Timer(DoWorkWrapper, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
      
        // Keep the application running until the user presses any key
        Console.WriteLine("Press any key to stop the program...");
        Console.ReadKey();

        // Stop the timer
        timer.Dispose();
    }
    private static void DoWorkWrapper(object state)
    {
        // Call the asynchronous method synchronously using Wait()
        // This will block the timer thread until the async method completes
        DoWorkAsync().Wait();
    }
    private static async Task DoWorkAsync()
    {
        Prediction prediction2;
        var result = await ExecuteHttpGetRequestAsync("http://badrezouiri-001-site1.gtempurl.com/api/Predictions");

        if (result != null && prediction !=null)
        {

            prediction2 = result;
            if(! prediction2.Equals(prediction) ) {
                prediction = prediction2;
            double a1 = (prediction.ClosingPrice + prediction.LowPrice) / 2;
            double a2 = (prediction.ClosingPrice + prediction.HighPrice) / 2;
            double a3=(a1 + a2)/2;
                Console.WriteLine("==============================");
                Console.WriteLine("Hight Limits  Limits " + a2);
                Console.WriteLine("%oy price " + a3);
                Console.WriteLine("Low  Limits " + a1);
                BeepAlert();
            }
        }

        if (result != null && prediction == null)
        {
            prediction = result;
            double a1 = (prediction.ClosingPrice + prediction.LowPrice) / 2;
            double a2 = (prediction.ClosingPrice + prediction.HighPrice) / 2;
            double a3 = (a1 + a2) / 2;
            Console.WriteLine("Hight Limits  Limits " + a2);
            Console.WriteLine("%oy price " + a3);
            Console.WriteLine("Low  Limits " + a1);

        }
    }
    static void BeepAlert()
        {
            // This loop will beep 10 times with a delay of 500 milliseconds between each beep
            for (int i = 0; i < 10; i++)
            {
                Console.Beep();
                Thread.Sleep(500);
            }
        }
      public static async Task<Prediction> ExecuteHttpGetRequestAsync(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("accept", "text/plain");

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseContent = await response.Content.ReadAsStringAsync();


                        // Simulating response parsing for testing purposesresponseContent = $"{DateTime.Now}|UP|1.2134|1.2178|1.2160|1.2200";
                        // responseContent = "2023-11-01|DOWN|1.2135|1.2147|1.2152";
                        // Parse the prediction text
                        string[] parts = responseContent.Split('|', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 5)
                        {
                            throw new ArgumentException("Invalid prediction text format");
                        }

                        Prediction prediction = new Prediction();
                        if (DateTime.TryParse(parts[0], out var date))
                            prediction.Date = date;

                        prediction.Direction = parts[1];

                        if (double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var lowPrice))
                            prediction.LowPrice = lowPrice;

                        if (double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var closingPrice))
                            prediction.ClosingPrice = closingPrice;

                        if (double.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out var highPrice))
                            prediction.HighPrice = highPrice;

                        return prediction;
                    }
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
        }
    }


class Prediction
{
    public DateTime Date { get; set; }
    public string Direction { get; set; }
    public double LowPrice { get; set; }
    public double ClosingPrice { get; set; }
    public double HighPrice { get; set; }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Prediction other = (Prediction)obj;

        return 
               LowPrice == other.LowPrice &&
               ClosingPrice == other.ClosingPrice &&
               HighPrice == other.HighPrice;
    }
}