using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Core.Interfaces
{
    public interface IElasticSearch
    {
        public string Query(string searchString, string[] markets, int limit);
    }
}
