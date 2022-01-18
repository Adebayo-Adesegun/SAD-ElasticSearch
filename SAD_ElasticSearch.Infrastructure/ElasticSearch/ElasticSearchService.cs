using Nest;
using SAD_ElasticSearch.Core.Interfaces;
using SAD_ElasticSearch.Core.Models;
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

        public ClusterHealth ClusterHealth()
        {
            ClusterHealthResponse elasticHealth = _client.Cluster.Health();
            ClusterHealth healthCluster;

            if (elasticHealth != null)
            {
                healthCluster = new ClusterHealth
                {
                    Status = Convert.ToString(elasticHealth.Status),
                    HttpStatusCode = (int)elasticHealth.ApiCall?.HttpStatusCode,
                    HttpMethod = Convert.ToString(elasticHealth.ApiCall?.HttpMethod),
                    DebugInformation = Convert.ToString(elasticHealth?.DebugInformation),
                    Success = (bool)elasticHealth.ApiCall?.Success,
                    Uri = Convert.ToString(elasticHealth.ApiCall?.Uri)
                };

                return healthCluster;
            }

            return new ClusterHealth { DebugInformation = "No response received from cluster on NEST Client" };
        }

        public string Query(string searchString, string[] markets, int limit)
        {
            //var res = _client.Search(s => s.Query(q => q.Match(m => m.OnField(f => f.Title).Query("test post 123"))));

            throw new NotImplementedException();
        }

        public bool IndexProduct(string indexName, PropertyModel property)
        {
            //_client.Index(property, p => p.Id(property.Property.PropertyID).Refresh());

            throw new NotImplementedException();
        }


        //public dynamic ViewIndex(string indexName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
