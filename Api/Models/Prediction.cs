namespace Api.Models
{
    public class Prediction
    {
        public string Date { get; set; }
        public string Direction { get; set; }
        public decimal HighPrice { get; set; } = 0;
        public decimal LowPrice { get; set; } = 0;
        public decimal ClosingPrice { get; set;}
    }
}
