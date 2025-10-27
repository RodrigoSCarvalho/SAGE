using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using SAGE.Domain.ChangePlans;

namespace SAGE.Infrastructure.Search;

public class ElasticsearchInitializer
{
    private readonly ElasticsearchClient _client;
    private readonly ILogger<ElasticsearchInitializer> _logger;

    public ElasticsearchInitializer(ElasticsearchClient client, ILogger<ElasticsearchInitializer> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        const string indexName = "changeplans";
        try
        {
            var existsResponse = await _client.Indices.ExistsAsync(indexName);
            if (existsResponse.Exists)
            {
                _logger.LogInformation("Índice '{IndexName}' já existe no Elasticsearch.", indexName);
                return;
            }


            var createResponse = await _client.Indices.CreateAsync(indexName, c => c
            .Mappings(m => m
            .Properties<ChangePlanDocument>(p => p
            .Keyword(k => k.Id)
            .Text(t => t.Title)
            .Text(t => t.Description)
            .Date(d => d.CreatedAt)
            .Keyword(k => k.CreatedBy)
            .Text(t => t.SearchText)
            .Keyword(k => k.Status)
            .Keyword(k => k.Tags)
            )));

            if (createResponse.IsValidResponse)
            {
                _logger.LogInformation("Índice '{IndexName}' criado com sucesso no Elasticsearch.", indexName);
            }
            else
            {
                _logger.LogError("Falha ao criar o índice '{IndexName}': {Error}", indexName, createResponse.DebugInformation);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao inicializar o Elasticsearch.");
        }
    }
}
