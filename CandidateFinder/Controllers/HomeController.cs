using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using CandidateFinder.Business;


namespace CandidateFinder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("get")]
        public async Task<IEnumerable<Job>> Get()
        {
            try
            {
                DataProcessing dtProcessor = new DataProcessing(_config);
                return await dtProcessor.SearchOpenPosition();
            }
            catch
            {
                //Write Log
                return new List<Job>();
            }

        }
    }
}
