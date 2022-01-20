using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Web.Models
{
    public class SearchResponse
    {
        public string Message { get; set; }
        public List<Data> Data { get; set; }
    }

    public class Data
    {
        public Property Property { get; set; }
        public Mgmt Mgmt { get; set; }
    }

    public class Property
    {
        public int PropertyID { get; set; }
        public string Name { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }

    public class Mgmt
    {
        public int MgmtID { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }

}
