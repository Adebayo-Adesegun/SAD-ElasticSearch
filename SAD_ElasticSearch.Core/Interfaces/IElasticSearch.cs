﻿using Nest;
using SAD_ElasticSearch.Core.Models;

namespace SAD_ElasticSearch.Core.Interfaces
{
    public interface IElasticSearch
    {
        public string Query(string searchString, string[] markets, int limit);

        public ClusterHealth ClusterHealth();


        // Viewing Indexed Property
        // https://search-smart-data-apartment-eqq7ihkqwzxew2dxyf7l7vut5u.eu-west-1.es.amazonaws.com/property/_search
    }
}
