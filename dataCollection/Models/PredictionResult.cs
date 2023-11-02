using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataCollection.Models
{
    public class PredictionResult
    {
        public DateTime Date { get; set; }
        public string Direction { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double ClosingPrice
        {
            get; set;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PredictionResult other = (PredictionResult)obj;

            return Date == other.Date &&
                   Direction == other.Direction &&
                   HighPrice == other.HighPrice &&
                   LowPrice == other.LowPrice &&
                   ClosingPrice == other.ClosingPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Direction, HighPrice, LowPrice, ClosingPrice);
        }
    }
}