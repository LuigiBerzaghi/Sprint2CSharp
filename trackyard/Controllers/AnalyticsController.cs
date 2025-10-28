using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprint1CSharp.Services.Prediction;

namespace Sprint1CSharp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = "ApiKey")]
[Route("api/v{version:apiVersion}/ml")] 
public class AnalyticsController : ControllerBase
{
    private readonly IPredictionService _service;
    public AnalyticsController(IPredictionService service) => _service = service;

    public record PredictRequest(int Ano, int Quilometragem);

    [HttpPost("predict-risk")]
    public ActionResult<object> PredictRisk([FromBody] PredictRequest req)
    {
        var result = _service.Predict(req.Ano, req.Quilometragem);
        return Ok(new { predicted = result.Predicted, probability = result.Probability });
    }
}

