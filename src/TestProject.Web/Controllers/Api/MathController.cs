using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestProject.CQRS.Math;
using TestProject.Web.Infrastucture.Authentication;
using TestProject.Web.Models.Requests;
using TestProject.Web.Models.Responses;

namespace TestProject.Web.Controllers.Api
{
  [ApiController]
  [Route("api/[controller]")]
  [BasicAuthorization]
  public class CalculationController : ControllerBase
  {
    private readonly ILogger<MathController> _logger;
    private readonly IMediator _mediator;

    public CalculationController(ILogger<MathController> logger, IMediator mediator)
    {
      _logger = logger;
      _mediator = mediator;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] AddRequest request)
    {
      _logger.LogInformation("Requested to add numbers {Number1} + {Number2}", request.Number1, request.Number2);

      var result = await _mediator.Send(new NumberAdd.Query()
      {
        Number1 = request.Number1,
        Number2 = request.Number2,
      });

      var response = new MathResult()
      {
        Result = result
      };
      return Ok(response);
    }

    [HttpPost("Minus")]
    public async Task<IActionResult> Minus([FromBody] MinusRequest request)
    {
      var result = await _mediator.Send(new NumberMinus.Query()
      {
        Number = request.Number,
        MinusWith = request.MinusWith,
      });

      var response = new MathResult()
      {
        Result = result
      };
      return Ok(response);
    }

    [HttpPost("Divide")]
    public async Task<IActionResult> Divide([FromBody] DivisionRequest request)
    {
      var result = await _mediator.Send(new NumberDivide.Query()
      {
        Number = request.Number,
        DivideBy = request.DivideBy,
      });

      var response = new MathResult()
      {
        Result = result
      };
      return Ok(response);
    }

    [HttpPost("Multiply")]
    public async Task<IActionResult> Multiply([FromBody] MultiplyRequest request)
    {
      var result = await _mediator.Send(new NumberMultiply.Query()
      {
        Number1 = request.Number1,
        Number2 = request.Number2,
      });

      var response = new MathResult()
      {
        Result = result
      };
      return Ok(response);
    }
  }
}