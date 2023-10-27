using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
        public async Task<Prediction>  Get()
        {
            string urlData = _configuration["urldata"];
            Prediction po = await new DTO.DTO().GetDataAsync(urlData);
            return po;
        }

        
    }
}
