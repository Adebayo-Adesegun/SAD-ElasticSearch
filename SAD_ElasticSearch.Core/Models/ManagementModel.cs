using Nest;
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
        [Text(Analyzer = "smart-analyzer", Name = nameof(Name))]
        public string Name { get; set; }
        [Text(Analyzer = "smart-analyzer", Name = nameof(Market))]
        public string Market { get; set; }
        [Text(Analyzer = "smart-analyzer", Name = nameof(State))]
        public string State { get; set; }

       // public CompletionField Suggest { get; set; }
    }
}
