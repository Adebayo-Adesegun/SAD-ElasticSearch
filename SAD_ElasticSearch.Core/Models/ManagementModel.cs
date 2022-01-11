using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Core.Models
{
    public class ManagementModel
    {
        public Mgmt Mgmt { get; set; }
    }


    public class Mgmt
    {
        public int MgmtID { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }
}
