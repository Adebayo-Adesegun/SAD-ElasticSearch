using Nest;
using SAD_ElasticSearch.Core.Models;
using SAD_ElasticSearch.Infrastructure;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SAD_ElasticSearch.Indexer
{
    class Program
    {
        // INITIALIZE JSON File READER
        private static JSONFileReader<PropertyModel> PropertyJSONFileReader { get; set; }

        private static JSONFileReader<ManagementModel> ManagementJSONFileReader { get; set; }

        public static ElasticClient client = ElasticSearchConfig.GetClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Begin Indexing Process for Property and Management Data to Elastic Client");

            string managementFilePath = Path.Combine(@"C:\Users\ASUS\source\repos\SAD_Api\SAD_ElasticSearch.Indexer", "mgmt.json");

            ManagementJSONFileReader = new JSONFileReader<ManagementModel>(managementFilePath);

            string propertyFilePath = Path.Combine(@"C:\Users\ASUS\source\repos\SAD_Api\SAD_ElasticSearch.Indexer", "properties.json");

            PropertyJSONFileReader = new JSONFileReader<PropertyModel>(propertyFilePath);

            IndexManagementModel();

            IndexPropertyModel();

            client.Indices.Refresh(($"{ElasticSearchConfig.MANAGEMENT_INDEX_NAME},{ElasticSearchConfig.PROPERTY_INDEX_NAME}"));


            Console.WriteLine("End Indexing Process for Property and Management Data to Elastic Client");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void IndexManagementModel()
        {
            string indexName = ElasticSearchConfig.MANAGEMENT_INDEX_NAME;

            var managementData = ManagementJSONFileReader.GetData();

            if (client.Indices.Exists(indexName).Exists)
                client.Indices.Delete(indexName);


            var createResponse = client.Indices.Create(indexName, s => s
                .Settings(a => a
                    .Analysis(ElasticSearchConfig.Analysis))
                        .Map<ManagementModel>(map => map
                            .AutoMap()
                                .Properties(p => p
                                .Text(t => t.Name(n => n.Mgmt.Name)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                                .Text(t => t.Name(n => n.Mgmt.Market)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                                .Text(t => t.Name(n => n.Mgmt.State)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))

            )));


            var response  = client.IndexMany(managementData, indexName);

        }

        private static void IndexPropertyModel()
        {
            string indexName = ElasticSearchConfig.PROPERTY_INDEX_NAME;

            if (client.Indices.Exists(indexName).Exists)
                client.Indices.Delete(indexName);


            string propertyFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "properties.json");


            PropertyJSONFileReader = new JSONFileReader<PropertyModel>(propertyFilePath);

            var propertyData = PropertyJSONFileReader.GetData();

            var createResponse = client.Indices.Create(indexName,
                s => s.Settings(a => a
                 .Analysis(ElasticSearchConfig.Analysis))
                  .Map<PropertyModel>(map => map
                     .AutoMap()
                        .Properties(p => p
                            .Text(t => t.Name(n => n.Property.Name)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                            .Text(t => t.Name(n => n.Property.FormerName)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                            .Text(t => t.Name(n => n.Property.City)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                            .Text(t => t.Name(n => n.Property.Market)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                            .Text(t => t.Name(n => n.Property.State)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                            .Text(t => t.Name(n => n.Property.StreetAddress)
                                .Analyzer(ElasticSearchConfig.SMART_ANALYZER))
                            )));


            client.IndexMany(propertyData, indexName);

        }
    }
}
