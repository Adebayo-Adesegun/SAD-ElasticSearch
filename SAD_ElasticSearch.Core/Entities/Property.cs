using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Core.Entities
{
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

}
