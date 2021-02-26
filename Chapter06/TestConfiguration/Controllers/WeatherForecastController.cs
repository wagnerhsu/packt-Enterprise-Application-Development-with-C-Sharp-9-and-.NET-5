﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TestConfiguration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public Dictionary<string, string> Get()
        {

            return new Dictionary<string, string>{
                { "TestKey", _configuration["TestKey"]} ,
                { "TestKeyFromAdditionalConfigJSON", _configuration["TestKeyFromAdditionalConfigJSON"] },
                {"TestKeyFromAdditionalXMLConfig", _configuration["TestKeyFromAdditionalXMLConfig"] },
                { "TestSqlKey",  _configuration["TestSqlKey"] }
            };
        }


    }
}
