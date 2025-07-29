using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAGE.Application.ChangePlans.Commands;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Application.ChangePlans.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace SAGE.Entrypoint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChangePlanController(IMediator mediator) : ControllerBase
{
  private readonly IMediator _mediator = mediator;

  [HttpPost]
  [SwaggerOperation(Summary = "Cria um novo plano de mudanças", Description = "Endpoint para criar um novo plano de mudança")]
  [SwaggerResponse(201, "Plano de mudança criado com sucesso.")]
  [SwaggerResponse(400, "Requisição inválida")]
  public async Task<IActionResult> Create([FromBody] ChangePlanDto request)
  {
    var command = new CreateChangePlanCommand(request);
    var id = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetById), new { id }, null);
  }

  [HttpGet("{id}")]
  [SwaggerOperation(Summary = "Busca plano de mudança pelo ID")]
  [SwaggerResponse(200, "Plano de mudança encontrado.")]
  [SwaggerResponse(404, "Plano de mudança não encontrado")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var result = await _mediator.Send(new GetChangePlanByIdQuery(id));
    return Ok(result);
  }
}