using Nest;
using SAD_ElasticSearch.Core.Models;
using System;
using System.Text;

namespace SAD_ElasticSearch.Infrastructure
{
    /// <summary>
    /// Define the configuration for the IElastic Client, the Key credentials have been specified as environment variables
    /// </summary>
    public static class ElasticSearchConfig
    {
        private static string AWS_OPENSEARCH_URL = Environment.GetEnvironmentVariable("AWS_OPENSEARCH_URL");
        private static string AWS_OPENSEARCH_USERNAME = Environment.GetEnvironmentVariable("AWS_OPENSEARCH_USERNAME");
        private static string AWS_OPENSEARCH_PASSWORD = Environment.GetEnvironmentVariable("AWS_OPENSEARCH_PASSWORD");

        private static int MIN_GRAM = int.Parse(Environment.GetEnvironmentVariable("MIN_GRAM"));
        private static int MAX_GRAM = int.Parse(Environment.GetEnvironmentVariable("MAX_GRAM"));

        private static string AUTOCOMPLETE_SEARCH = "autocomplete-search";
        private static string STOP_WORDS = "stop-words";
        private static string LANGUAGE = "_english_";


        public const string SMART_ANALYZER = "smart-analyzer";
        public const string MANAGEMENT_INDEX_NAME = "management";
        public const string PROPERTY_INDEX_NAME = "property";


        public static ElasticClient GetClient()
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri(AWS_OPENSEARCH_URL))
                .BasicAuthentication(AWS_OPENSEARCH_USERNAME, AWS_OPENSEARCH_PASSWORD)
                .DefaultIndex(PROPERTY_INDEX_NAME)
                .DefaultMappingFor<Prop>(p => p.IndexName(PROPERTY_INDEX_NAME).IdProperty(i => i.PropertyID))
                .DefaultMappingFor<Mgmt>(m => m.IndexName(MANAGEMENT_INDEX_NAME).IdProperty(i => i.MgmtID))
                .DefaultFieldNameInferrer(i => i)
                .OnRequestCompleted(response =>
                {
                    // Log the response from Elastic Client

                    StringBuilder builderLog = new();

                    builderLog.Append($"URI {response.Uri} : METHOD {response.HttpMethod} : CODE {response.HttpStatusCode}");

                    builderLog.AppendLine();

                    builderLog.Append($"AUDIT TRAIL");
                    builderLog.AppendLine();

                    builderLog.AppendLine($"DEBUG INFO : {response.DebugInformation}");
                    builderLog.AppendLine();
                    builderLog.AppendLine();

                    foreach (var trail in response.AuditTrail)
                    {
                        builderLog.AppendLine($"EVENT : {trail.Event}");
                        builderLog.AppendLine();

                        builderLog.AppendLine($"EVENT : {trail.Started}");
                        builderLog.AppendLine();


                        builderLog.AppendLine($"EVENT : {trail.Ended}");
                        builderLog.AppendLine();


                        builderLog.AppendLine($"EVENT : {trail.Exception}");
                        builderLog.AppendLine();
                    }
                    Console.WriteLine(builderLog);
                });


            return new ElasticClient(settings);
        }

        /// <summary>
        /// Function Delegate Analysis Descriptor
        /// </summary>
        /// <param name="analysis"></param>
        /// <returns></returns>
        public static AnalysisDescriptor Analysis(AnalysisDescriptor analysis) => analysis

             .Analyzers(analyzers => analyzers
                .Custom(SMART_ANALYZER, c => c
                    .Tokenizer(AUTOCOMPLETE_SEARCH)
                    .Filters(STOP_WORDS)
                 ))

                // The following confgurations speaks to the Natural Language Processing part of ELastic Search 

                // tokenizing returns to root words and stop words are e.g. the, and, I etc. 


                // Setup Edge n-gram tokenizer for autocomplete
                .Tokenizers(tokenizer => tokenizer
                    .EdgeNGram(AUTOCOMPLETE_SEARCH, e => e
                        .MinGram(MIN_GRAM)
                        .MaxGram(MAX_GRAM)
                        .TokenChars(TokenChar.Letter, TokenChar.Digit)
                            ))

                // Setup Stop Token Filter to remove stop words
                .TokenFilters(tokenfilters => tokenfilters
                    .Stop(STOP_WORDS, w => w
                        .StopWords(LANGUAGE))
                            );


        public static (int, int) ConfirmIndexUpload()
        {
            ElasticClient client = GetClient();

            var indices_stats = client.Indices.Stats();

            return (10, 10);
        }
    }
}
