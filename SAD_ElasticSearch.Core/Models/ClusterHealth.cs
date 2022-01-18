using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Core.Models
{
    public class ClusterHealth
    {
        public string Status { get; set; }
        public string DebugInformation { get; set; }
        public string HttpMethod { get; set; }
        public int HttpStatusCode { get; set; }
        public string Uri { get; set; }
        public bool Success { get; set; }
    }
}
