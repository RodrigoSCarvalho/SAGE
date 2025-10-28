using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAGE.Application.ChangePlans.Commands.CreateChangePlan;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Application.ChangePlans.Queries.GetChangePlanById;
using SAGE.Application.ChangePlans.Queries.SearchChangePlans;
using Swashbuckle.AspNetCore.Annotations;

namespace SAGE.API.Controllers;

[ApiController]
[Route("v1/change-plans")]
public class ChangePlanController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo plano de mudanças", Description = "Endpoint para criar um novo plano de mudança")]
    [SwaggerResponse(201, "Plano de mudança criado com sucesso.", typeof(ChangePlanResponseDto))]
    [SwaggerResponse(400, "Dados inválidos")]
    [ProducesResponseType(typeof(ChangePlanResponseDto), 201)]
    public async Task<IActionResult> Create([FromBody] CreateChangePlanDto request, CancellationToken cancellationToken)
    {
        var command = new CreateChangePlanCommand(request.Title, request.Description, request.CreatedBy);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca plano de mudança pelo ID")]
    [SwaggerResponse(200, "Plano de mudança encontrado.", typeof(ChangePlanResponseDto))]
    [SwaggerResponse(404, "Plano de mudança não encontrado")]
    [ProducesResponseType(typeof(ChangePlanResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetChangePlanByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result == null)
        {
            return NotFound(new { message = "Plano de mudança não encontrado", id });
        }
        return Ok(result);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Busca planos de mudança com filtros")]
    [SwaggerResponse(200, "Planos de mudança encontrados.", typeof(SearchResultDto<ChangePlanResponseDto>))]
    [SwaggerResponse(400, "Parâmetros de busca inválidos")]
    [ProducesResponseType(typeof(SearchResultDto<ChangePlanResponseDto>), 200)]
    public async Task<IActionResult> Search(
        [FromQuery, SwaggerParameter("Termo de busca (título, descrição)")] string? searchTerm,
        [FromQuery, SwaggerParameter("Filtrar planos criados após esta data")] DateTime? createdAfter,
        [FromQuery, SwaggerParameter("Filtrar planos criados antes desta data")] DateTime? createdBefore,
        [FromQuery, SwaggerParameter("Filtrar por tags")] List<string>? tags,
        [FromQuery, SwaggerParameter("Número da página(padrão: 1)")] int page = 1,
        [FromQuery, SwaggerParameter("\"Itens por página (padrão: 10)")] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
            return BadRequest(new { message = "O número da página deve ser maior ou igual a 1." });

        if (pageSize < 1 || pageSize > 100)
            return BadRequest(new { message = "O tamanho da página deve estar entre 1 e 100." });

        var query = new SearchChangePlansQuery(
            searchTerm,
            createdAfter,
            createdBefore,
            tags,
            page,
            pageSize);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista todos os planos")]
    [SwaggerResponse(200, "Planos de mudança encontrados.", typeof(SearchResultDto<ChangePlanResponseDto>))]
    [ProducesResponseType(typeof(SearchResultDto<ChangePlanResponseDto>), 200)]
    public async Task<IActionResult> GetAll(
        [FromQuery, SwaggerParameter("Número da página(padrão: 1)")] int page = 1,
        [FromQuery, SwaggerParameter("Itens por página (padrão: 10)")] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
            return BadRequest(new { message = "O número da página deve ser maior ou igual a 1." });
        if (pageSize < 1 || pageSize > 100)
            return BadRequest(new { message = "O tamanho da página deve estar entre 1 e 100." });

        var query = new SearchChangePlansQuery(
            null,
            null,
            null,
            null,
            page,
            pageSize);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}