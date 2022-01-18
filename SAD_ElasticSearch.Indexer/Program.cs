using Nest;
using SAD_ElasticSearch.Core.Models;
using SAD_ElasticSearch.Infrastructure;
using System;
using System.IO;
using System.Linq;
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

        private static readonly string currentManagementIndexName = ElasticSearchConfig.CreateIndexName(ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS);

        private static readonly string currentPropertyIndexName = ElasticSearchConfig.CreateIndexName(ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS);

        static void Main(string[] args)
        {
            Console.WriteLine("Begin Indexing Process for Property and Management Data to Elastic Client");

            string managementFilePath = Path.Combine(@"C:\Users\ASUS\source\repos\SAD_Api\SAD_ElasticSearch.Indexer", "mgmt.json");

            ManagementJSONFileReader = new JSONFileReader<ManagementModel>(managementFilePath);

            string propertyFilePath = Path.Combine(@"C:\Users\ASUS\source\repos\SAD_Api\SAD_ElasticSearch.Indexer", "properties.json");

            PropertyJSONFileReader = new JSONFileReader<PropertyModel>(propertyFilePath);


            //client.Indices.Delete("management*");

            IndexManagementModel();
            IndexPropertyModel();


            SwapPropertyIndexAlias();
            SwapManagementIndexAlias();

            client.Indices.Refresh(($"{ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS},{ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS}"));


            Console.WriteLine("End Indexing Process for Property and Management Data to Elastic Client");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void IndexManagementModel()
        {
           var managementData = ManagementJSONFileReader.GetData();
           string indexName = string.Empty;

            // check if index exist already
            var IndexExists = client.Indices.Exists(ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS).Exists;

            if (IndexExists)
            {
                indexName = currentManagementIndexName;
            }
            else
            {
                indexName = ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS;
            }
                

            client.Indices.Create(indexName, s => s
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

            client.IndexMany(managementData, indexName);

        }



        private static void IndexPropertyModel()
        {
            var propertyData = PropertyJSONFileReader.GetData();
            string indexName = string.Empty;

            // check if index exist already
            var IndexExists = client.Indices.Exists(ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS).Exists;

            if (IndexExists)
            {
                indexName = currentPropertyIndexName;
            }
            else
            {
                indexName = ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS;
            }


            client.Indices.Create(indexName,
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



        /// <summary>
        /// Alias index to enable versioning of index on production environment rather than deleting the index when a new index has to be uploaded to the elastic search server. he operation of swapping aliases is atomic, so the application will not incur any downtime in the process.
        /// </summary>
        private static void SwapManagementIndexAlias()
        {
            var IndexExists = client.Indices.Exists(ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS).Exists;

            client.Indices.BulkAlias(aliases =>
            {
                if (IndexExists)
                {
                    var indicesofAlias = client.GetIndicesPointingToAlias(ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS);
                    string firstIndices = string.Empty;

                    if (indicesofAlias.Count > 0)
                    {
                        firstIndices = indicesofAlias.First();

                        aliases.Add(a => a
                                .Alias(ElasticSearchConfig.OLD_MANAGEMENT_INDEX_ALIAS)
                                .Index(firstIndices));
                    }
                }


                return aliases
                .Remove(a => a.Alias(ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS).Index("*"))
                .Add(a => a.Alias(ElasticSearchConfig.LIVE_MANAGEMENT_INDEX_ALIAS).Index(currentManagementIndexName));
            });

             var oldIndices = client.GetIndicesPointingToAlias(ElasticSearchConfig.OLD_MANAGEMENT_INDEX_ALIAS)
                    .OrderByDescending(name => name)
                    .Skip(2);

            foreach (var oldIndex in oldIndices)
                client.Indices.Delete(oldIndex);

        }


        private static void SwapPropertyIndexAlias()
        {
            var IndexExists = client.Indices.Exists(ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS).Exists;

            client.Indices.BulkAlias(aliases =>
            {
                if (IndexExists)
                {
                    var indicesofAlias = client.GetIndicesPointingToAlias(ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS);
                    string firstIndices = string.Empty;

                    if (indicesofAlias.Count > 0)
                    {
                        firstIndices = indicesofAlias.First();
                        aliases.Add(a => a
                            .Alias(ElasticSearchConfig.OLD_PROPERTY_INDEX_ALIAS)
                            .Index(firstIndices));
                    }
                }


                return aliases.Remove(a => a
                .Alias(ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS).Index("*"))
                .Add(a => a.Alias(ElasticSearchConfig.LIVE_PROPERTY_INDEX_ALIAS).Index(currentManagementIndexName));
            });

            var oldIndices = client.GetIndicesPointingToAlias(ElasticSearchConfig.OLD_PROPERTY_INDEX_ALIAS)
                   .OrderByDescending(name => name)
                   .Skip(2);

            foreach (var oldIndex in oldIndices)
                client.Indices.Delete(oldIndex);

        }
    }
}
