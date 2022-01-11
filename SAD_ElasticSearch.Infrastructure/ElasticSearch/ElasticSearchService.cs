using Nest;
using SAD_ElasticSearch.Core.Interfaces;
using System;
using System.Collections.Generic;


namespace SAD_ElasticSearch.Infrastructure.ElasticSearch
{
    public class ElasticSearchService : IElasticSearch
    {

        private readonly IElasticClient _client;
        public ElasticSearchService()
        {
            _client = ElasticSearchConfig.GetClient();
        }

        public bool CreateIndex(string indexName)
        {
            if (!_client)
        }

        public string Query(string searchString, string[] markets, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
