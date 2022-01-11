using Nest;
using System;

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
        private static string SMART_ANALYZER = "smart-analyzer";
        private static string STOP_WORDS = "stop-words";
        private static string LANGUAGE = "_english_";


        public static string MANAGEMENT_INDEX_NAME => "management";
        public static string PROPERTY_INDEX_NAME => "property";


        public static IElasticClient GetClient()
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri(AWS_OPENSEARCH_URL)).BasicAuthentication(AWS_OPENSEARCH_USERNAME, AWS_OPENSEARCH_PASSWORD);
            return new ElasticClient(settings);
        }

        public static AnalysisDescriptor Analysis()
        {
            var analysis = new AnalysisDescriptor();

            return analysis.Analyzers(analyzers => analyzers
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
        }
    }
}
