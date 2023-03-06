using Nest;

namespace SoftSkill.SearchEngine.POC.Services.Services
{
    public class SearchService<T> where T : ISearchableModel
    {
        private readonly IElasticClient _client;
        private readonly string _indexName;

        public SearchService(IElasticClient client, string indexName)
        {
            _client = client;
            _indexName = indexName;
        }

        public IEnumerable<T> Search(string query)
        {
            _client.


            var searchResponse = _client.Search<T>(s => s
                .Index(_indexName)
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(f => f
                            .Field("_all"))
                        .Query(query)
                        .Type(TextQueryType.MostFields))));

            var searchResults = searchResponse.Documents;

            return searchResults;
        }
    }
}
