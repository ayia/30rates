using Api.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PredictionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<PredictionsController>
        [HttpGet]
        public async Task<string>  Get()
        {
            string urlData = _configuration["urldata"];
            Prediction po = await new DTO.DTO().GetDataAsync(urlData);
            return po.Date+"|"+po.Direction + "|" +po.LowPrice + "|" +po.ClosingPrice
                + "|" +po.HighPrice;
        }
        [HttpGet]
        [Route("PredictionTWo")]
        public async Task<string> PredictionTWo()
        {
            string urlData = _configuration["urldata"];
            Prediction po = await new DTO.DTO().GetDataAsync(urlData);
            return po.Date + "|" + po.Direction + "|" + ((po.LowPrice+ po.ClosingPrice)/2).ToString("F4") + "|" + po.ClosingPrice
                + "|" + ((po.HighPrice + po.ClosingPrice) / 2).ToString("F4");
        }


    }
}
