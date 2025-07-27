using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAGE.Application.ChangePlans.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace SAGE.Entrypoint.Controller
{
  [ApiController]
  [Route("api/[controller]")]
  public class ChangePlanController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ChangePlanController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo plano de mudanças", Description = "Endpoint para criar um novo plano de mudança")]
    [SwaggerResponse(201, "Plano de mudança criado com sucesso.")]
    [SwaggerResponse(400, "Requisição inválida")]
    public async Task<IActionResult> Create([FromBody] CreateChangePlanCommand command)
    {
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca plano de mudança pelo ID")]
    [SwaggerResponse(200, "Plano de mudança encontrado.")]
    [SwaggerResponse(404, "Plano de mudança não encontrado")]
    public IActionResult GetById(Guid id)
    {
      return Ok();
    }
  }
}