using dataCollection.DTO;
using dataCollection.Models;
using System;
using System.Linq;
using System.Threading;

DTO dTO = new DTO();
TreadContext treadContext = new TreadContext();
PredictionResult default0 = await dTO.getPredictionResultAsync();
dTO.Save(default0, treadContext);

// Create a timer to repeat the process every 1 minute
Timer timer = new Timer(async (state) =>
{
    // Check if the result is different from PredictionResult default0
    PredictionResult newResult = await dTO.getPredictionResultAsync();

    if (! newResult.Equals(default0) && newResult !=null)
    {
        // Modify the old value with the new value
        default0 = newResult;
        // Save the new value
        dTO.Save(default0, treadContext);
    }

}, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

// Keep the application running
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

// Dispose the timer when the application is done
timer.Dispose();
