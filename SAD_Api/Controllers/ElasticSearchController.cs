using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAD_ElasticSearch.Core.Interfaces;
using SAD_ElasticSearch.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchController : ControllerBase
    {
        private readonly IElasticSearch _elasticSearch;

       public ElasticSearchController(IElasticSearch elasticSearch)
        {
            _elasticSearch = elasticSearch;
        }

        [HttpPost]
        public IActionResult Post([FromQuery]string[] markets, [FromQuery]string searchText, [FromQuery]int limit = 25)
        {
            //var response = new GenericAPIResponse<object>
            //{

            //    Data = _elasticSearch.Query(searchText, markets, limit),
            //    Message = "fetched data"
            //};

            return Ok(_elasticSearch.Query(searchText, markets, limit));
        }
    }
}
