using CustomerService.Helpers;
using CustomerService.Models;
using CustomerService.Services;
using CustomerService.V1.Models.RequestModels;
using CustomerService.V1.Models.ResponseModels;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.V1.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;
    private readonly ILogger<CustomerController> _logger;
    private readonly IValidator<CustomerCreateModel> _validator;

    public CustomerController(ICustomerService service, ILogger<CustomerController> logger, IValidator<CustomerCreateModel> validator)
    {
        _service = service;
        _logger = logger;
        _validator = validator;
    }

    [HttpGet("IsWorking")]
    public async Task<IActionResult> Working()
    {
        return Ok("This server is working!");
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CustomerCreateModel createModel)
    {
        
        var validationResult = await _validator.ValidateAsync(createModel);
        if (!validationResult.IsValid)
        {
            throw new CustomException($"{validationResult.Errors[0].ErrorMessage}");
            //return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);
        }
        
        string id = await _service.Create(createModel);

        return Created(nameof(Create), $"Created ID: {id}");
        /*
        try
        {
            string id = await _service.Create(createModel);

            return Created(nameof(Create), $"Created ID: {id}");
        }
        catch(Exception ex)
        {
            _logger.LogError("Error at Create endpoint");
            throw new AppException("Error at Create endpoint");
        }
        */

    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchById(string id, [FromBody] CustomerPatchModel patchModel)
    {
        if (patchModel == null)
        {
            _logger.LogError("Update Model is empty");
            return BadRequest("Update Model is empty");
        }
            

        var success = await _service.Update(id, patchModel);

        if (!success)
        {
            _logger.LogError("Update is not successful");
            throw new CustomException("Update is not successful");
        }
            
        
        return Ok($"Customer {id} is updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        try
        {
            await _service.Delete(id);
        
            return Ok($"Customer {id} is deleted");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new CustomException("Error at Delete endpoint");
        }

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerGetModel>> Get([FromRoute] string id)
    {

        try
        {
            var getCustomer = await _service.GetById(id);
            return Ok(getCustomer);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new CustomException("Error at Get endpoint");
        }
    }
    
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<CustomerGetModel>>> GetAll()
    {
        try
        {
            var customers = await _service.GetAll();
            return Ok(customers);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new CustomException($"Error at GetAll endpoint: {ex.Message}");
        }
    }

    [HttpGet("Validate/{id}")]
    public async Task<ActionResult<bool>> Validate([FromRoute] string id)
    {
        var validate = await _service.Validate(id);
        
        if (!validate)
        {
            _logger.LogError("Customer could not be validated at Validate endpoint");
            return BadRequest("Customer is not Valid");
        }
        //return Ok($"{id} is Valid");
        return Ok(validate);
    }
}