using System;
using System.Collections.Generic;

namespace dataCollection.Models;

public partial class Rates30
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public double ClosingPrice { get; set; }

    public double HighPrice { get; set; }

    public double LowPrice { get; set; }
}
