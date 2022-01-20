using System.Collections.Generic;

namespace SAD_ElasticSearch.Core.POCO
{
    public class SearchReq
    {
        public string SearchText { get; set; }
        public List<string> Markets { get; set; }
        public int Limit { get; set; }
    }
}
