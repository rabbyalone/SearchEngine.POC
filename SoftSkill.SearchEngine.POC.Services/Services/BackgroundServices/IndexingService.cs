using Nest;

namespace SoftSkill.SearchEngine.POC.Services.Services.BackgroundServices
{

    public class IndexingService<TDocument> : BackgroundService where TDocument : class
    {
        private readonly string _indexName;
        private readonly Uri _elasticsearchUri;
        private readonly ElasticClient _elasticClient;

        public IndexingService(string indexName, Uri elasticsearchUri)
        {
            _indexName = indexName;
            _elasticsearchUri = elasticsearchUri;

            var connectionSettings = new ConnectionSettings(_elasticsearchUri)
                .DefaultIndex(_indexName);
            _elasticClient = new ElasticClient(connectionSettings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: Replace this with your own logic for monitoring new data in the database
            while (!stoppingToken.IsCancellationRequested)
            {
                // Check for new data in the database
                var newData = GetNewDataFromDatabase();

                if (newData.Count > 0)
                {
                    // Create the Elasticsearch index
                    var createIndexResponse = await _elasticClient.Indices.CreateAsync(_indexName, c => c
                        .Map(m => m
                            .AutoMap<TDocument>()
                        )
                    );

                    if (createIndexResponse.IsValid)
                    {
                        // Index the new data
                        var bulkResponse = await _elasticClient.BulkAsync(b => b
                            .Index(_indexName)
                            .IndexMany(newData)
                        );

                        if (bulkResponse.Errors)
                        {
                            // Handle indexing errors
                            foreach (var itemWithError in bulkResponse.ItemsWithErrors)
                            {
                                Console.WriteLine($"Failed to index {itemWithError.Id}: {itemWithError.Error}");
                            }
                        }
                    }
                    else
                    {
                        // Handle index creation errors
                        Console.WriteLine($"Failed to create Elasticsearch index {_indexName}: {createIndexResponse.DebugInformation}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        private List<TDocument> GetNewDataFromDatabase()
        {
            // TODO: Replace this with your own logic for retrieving new data from the database
            // This example returns a list of 10 documents with random values
            var random = new Random();
            return Enumerable.Range(1, 10)
                .Select(i => Activator.CreateInstance(typeof(TDocument), new object[] { i, $"Document {i}", random.NextDouble() }) as TDocument)
                .ToList();
        }
    }

}
