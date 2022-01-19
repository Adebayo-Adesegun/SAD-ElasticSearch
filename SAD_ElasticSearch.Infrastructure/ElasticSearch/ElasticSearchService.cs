using Nest;
using Newtonsoft.Json;
using SAD_ElasticSearch.Core.Interfaces;
using SAD_ElasticSearch.Core.Models;
using System;
using System.Linq;

namespace SAD_ElasticSearch.Infrastructure.ElasticSearch
{
    public class ElasticSearchService : IElasticSearch
    {

        private readonly IElasticClient _client;
        private const string PROPERTY_MARKET_FIELD = "Property.Market";
        private const string MGMT_MARKET_FIELD = "Mgmt.Market";


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

        public string Query(string searchString, string[] markets, int limit = 25)
        {

            var search = _client.Search<object>(s => s
                    .Index(Indices.Index(ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS).And(ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS))
                    .Size(limit)
                        .Query
                            (q =>

                            (q.MultiMatch
                                (m => m.Fields(f => f
                                        .Field(Infer.Field<PropertyModel>(ff => ff.Property.FormerName))
                                        .Field(Infer.Field<PropertyModel>(ff => ff.Property.Name))
                                        .Field(Infer.Field<PropertyModel>(ff => ff.Property.City))
                                        .Field(Infer.Field<PropertyModel>(ff => ff.Property.State))
                                )
                                .Operator(Operator.Or)
                                .Query(searchString)) 
                                
                                && +q.Term("_index", ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS)) 
                            
                            
                            ||

                        (q.MultiMatch(m => m.Fields(f => f
                                    .Field(Infer.Field<ManagementModel> (ff => ff.Mgmt.Name))
                                    .Field(Infer.Field<ManagementModel>(ff => ff.Mgmt.State)))
                        .Operator(Operator.Or)
                        .Query(searchString))
                        
                        && +q.Term("_index", ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS))
                        
                        
                        ));

            var result = search.Documents?.ToList();

            var serializedData = JsonConvert.SerializeObject(result, Formatting.Indented);

            return serializedData;
        }

    }
}
