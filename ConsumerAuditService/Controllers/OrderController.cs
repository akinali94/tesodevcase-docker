using Microsoft.AspNetCore.Mvc;

namespace ConsumerAuditService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger _logger;

    public OrderController(ILogger logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult ProcessOrderCreated([FromBody] string EMINDEGILIM)
    {
        return Ok("Create order processed successfully.");
    }
}