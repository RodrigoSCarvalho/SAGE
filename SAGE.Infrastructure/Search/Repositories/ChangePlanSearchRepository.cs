using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using SAGE.Domain.ChangePlans;

namespace SAGE.Infrastructure.Search.Repositories;
public class ChangePlanSearchRepository : IChangePlanSearchRepository
{
    private const string IndexName = "changeplans";
    private readonly ElasticsearchClient _client;
    private readonly ILogger<ChangePlanSearchRepository> _logger;

    public ChangePlanSearchRepository(ElasticsearchClient client, ILogger<ChangePlanSearchRepository> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task IndexAsync(ChangePlanDocument document, CancellationToken cancellationToken = default)
    {
        try
        {
            document.SearchText = $"{document.Title} {document.Description}".ToLower();

            var response = await _client.IndexAsync(document, idx => idx
                .Index(IndexName)
                .Id(document.Id.ToString()), cancellationToken);

            if (!response.IsValidResponse)
            {
                _logger.LogError("Erro ao indexar documento {Id}: {Error}", document.Id, response.DebugInformation);
                throw new Exception($"Falha ao indexar: {response.DebugInformation}");
            }

            _logger.LogInformation("Documento {Id} indexado com sucesso", document.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao indexar documento { Id}", document.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.DeleteAsync(IndexName, id, cancellationToken);
            if (!response.IsValidResponse)
            {
                _logger.LogWarning("Documento {Id} não encontrado para deletar", id);
            }
            else
            {
                _logger.LogInformation("Documento {Id} removido do índice", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar documento {Id}", id);
            throw;
        }
    }

    public async Task<ChangePlanDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetAsync<ChangePlanDocument>(IndexName, id, cancellationToken);
            if (!response.IsValidResponse || !response.Found)
            {
                return null;
            }

            return response.Source;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter documento {Id}", id);
            throw;
        }
    }

    public async Task<List<ChangePlanDocument>> SearchAsync(
        string searchTerm, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var from = (page - 1) * pageSize;

            var response = await _client.SearchAsync<ChangePlanDocument>(s => s
                .Indices(IndexName)
                .From(from)
                .Size(pageSize)
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(searchTerm)
                        .Fields(new[] { "title", "description", "searchText" })
                        .Fuzziness(new Fuzziness("AUTO"))
                    )
                ).Sort(so => so
                .Score(s => s.Order(SortOrder.Desc))
                .Field(f => f.Field(d => d.CreatedAt).Order(SortOrder.Desc))
                ), cancellationToken);

            if (!response.IsValidResponse)
            {
                _logger.LogError("Erro na busca: {Error}", response.DebugInformation);
                return new List<ChangePlanDocument>();
            }
            return response.Documents.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar busca: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<List<ChangePlanDocument>> SearchWithFilterAsync(
        string? searchTerm, DateTime? createdAfter, DateTime? createdBefore, List<string>? tags, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var from = (page - 1) * pageSize;
            var mustQueries = new List<Query>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                mustQueries.Add(new MultiMatchQuery
                {
                    Query = searchTerm,
                    Fields = new[] { "title", "description", "searchText" },
                    Fuzziness = new Fuzziness("AUTO")
                });
            }

            if (createdAfter.HasValue)
            {
                mustQueries.Add(new DateRangeQuery(new Field("createdAt"))
                {
                    Gte = createdAfter.Value
                });
            }

            if (createdBefore.HasValue)
            {
                mustQueries.Add(new DateRangeQuery(new Field("createdAt"))
                {
                    Lte = createdBefore.Value
                });
            }

            if (tags?.Any() == true)
            {
                mustQueries.Add(new TermsQuery { Field = "tags.keyword", Terms = new TermsQueryField(tags.Select(t => FieldValue.String(t)).ToArray())});
            }

            var response = await _client.SearchAsync<ChangePlanDocument>(s => s
                .Indices(IndexName)
                .From(from)
                .Size(pageSize)
                .Query(q => q
                    .Bool(b => b
                        .Must(mustQueries.ToArray())
                    )
                ).Sort(so => so
                .Field(f => f.Field(d => d.CreatedAt).Order(SortOrder.Desc))
                ), cancellationToken);

            if (!response.IsValidResponse)
            {
                _logger.LogError("Erro na busca com filtro: {Error}", response.DebugInformation);
                return new List<ChangePlanDocument>();
            }

            return response.Documents.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar busca com filtros");
            throw;
        }
    }

}

