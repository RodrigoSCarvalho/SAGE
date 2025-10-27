using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAGE.Application.ChangePlans.Commands.CreateChangePlan;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Application.ChangePlans.Queries.GetChangePlanById;
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
    public async Task<IActionResult> Create([FromBody] CreateChangePlanDto request)
  {
    var command = new CreateChangePlanCommand(request.Title, request.Description, request.CreatedBy);
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetById), new { id = result.Id}, result);
  }

  [HttpGet("{id}")]
  [SwaggerOperation(Summary = "Busca plano de mudança pelo ID")]
  [SwaggerResponse(200, "Plano de mudança encontrado.", typeof(ChangePlanResponseDto))]
  [SwaggerResponse(404, "Plano de mudança não encontrado")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var result = await _mediator.Send(new GetChangePlanByIdQuery(id));
        if (result == null)
        {
            return NotFound(new {message = "Plano de mudança não encontrado"});
        }
        return Ok(result);
  }
}