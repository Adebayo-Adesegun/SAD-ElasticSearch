using Nest;
using SAD_ElasticSearch.Core.Models;
using SAD_ElasticSearch.Infrastructure;
using System;

namespace SAD_ElasticSearch.Indexer
{
    class Program
    {
        // INITIALIZE JSON File READER
        private static JSONFileReader<PropertyModel> PropertyJSONFileReader { get; set; }

        private static JSONFileReader<ManagementModel> ManagementJSONFileReader { get; set; }

        IElasticClient client = ElasticSearchConfig.GetClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Begin Indexing Process for Property and Management Data to Elastic Client");

            
            

        }

        private static void IndexManagementModel()
        {

        }

        private static void IndexPropertyModel()
        {

        }
    }
}
