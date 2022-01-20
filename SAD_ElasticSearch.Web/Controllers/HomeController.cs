using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAD_ElasticSearch.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(string search, string markets)
        {
            var url = $"https://localhost:44380/api/elasticsearch?searchtext={search}&markets={markets}";
            SearchResponse resp = new();

            List<string> autoCompleteResp = new();


            using (HttpClient client = new())
            {
                var response = client.GetAsync(url).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;

                var deserializedResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString);
                resp = deserializedResponse;
            }

            foreach (var mgm in resp.Data.Select(m => m.Mgmt).ToList())
            {
                if (mgm != null)
                {
                    autoCompleteResp.Add("Mgmt | " + mgm.Name);
                }
            }



            foreach (var prope in resp.Data.Select(m => m.Property).ToList())
            {
                if (prope != null)
                {
                    autoCompleteResp.Add("Prop | " + prope.Name);
                }
            }


            //Note : you can bind same list from database  

            //Searching records from list using LINQ query  

            return Json(autoCompleteResp);

            //return Json(new { success = false, responseText = "Nothing Selected" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
