using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAD_ElasticSearch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IElasticSearch _elasticSearch;

       public TestController(IElasticSearch elasticSearch)
        {
            _elasticSearch = elasticSearch;
        }
        public IActionResult Post()
        {
            return Ok();
        }
    }
}
