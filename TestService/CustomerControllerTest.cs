using CustomerService.Helpers;
using CustomerService.Services;
using CustomerService.V1.Controllers;
using CustomerService.V1.Models.RequestModels;
using CustomerService.V1.Models.ResponseModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CostumerService.Test;

public class CustomerControllerTest
{
    
    private readonly CustomerController _controller;
    private readonly Mock<ICustomerService> _mockService;
    private readonly Mock<ILogger<CustomerController>> _mockLogger;
    private readonly Mock<IValidator<CustomerCreateModel>> _mockValidator;
    
    public CustomerControllerTest()
    {
        _mockService = new Mock<ICustomerService>();
        _mockLogger = new Mock<ILogger<CustomerController>>();
        _mockValidator = new Mock<IValidator<CustomerCreateModel>>();

        _controller = new CustomerController(_mockService.Object, _mockLogger.Object, _mockValidator.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }
    

    [Fact]
    public async Task Create_Success()
    {
        //Arrange
        CustomerCreateModel testCustomer = new CustomerCreateModel
        {
            Name = "test",
            Email = "test@test.com",
            Addresses =
            [
                new CustomerCreateModel.AddressModel
                {
                    AddressLine = "Ankara Caddesi",
                    City = "Istanbul",
                    Country = "Turkiye",
                    CityCode = 34460
                },
                new CustomerCreateModel.AddressModel
                {
                    AddressLine = "Baglarbasi Caddesi",
                    City = "Istanbul",
                    Country = "Turkiye",
                    CityCode = 34460
                }
            ]
        };
        _mockValidator.Setup(v => v.ValidateAsync(testCustomer, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.Create(testCustomer)).ReturnsAsync("new-id");
        
        //Act
        var result = await _controller.Create(testCustomer);
        
        //Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal("Created ID: new-id", createdResult.Value);
    }

    [Fact]
    public async Task Create_InvalidModel()
    {
        //Arrange
        CustomerCreateModel testCustomer = new CustomerCreateModel
        {
            Name = "Ali",
            Email = "ali123ali.com",
            Addresses =
                [
                    new CustomerCreateModel.AddressModel
                    {
                        AddressLine = "Denizli Caddesi",
                        City = "Izmir",
                        Country = "Turkiye",
                        CityCode = 35600
                    }
                ]
        };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Email is required")
        });
        _mockValidator.Setup(v => v.ValidateAsync(testCustomer, default)).ReturnsAsync(validationResult);
        
        //Act
        var exception = await Assert.ThrowsAsync<CustomException>(() => _controller.Create(testCustomer));

        //Assert
        Assert.Equal("Email is required", exception.Message);
    }

    [Fact]
    public async Task Patch_Success()
    {
        //Arrange
        string customerId = "valid-id";
        CustomerPatchModel testCustomer = new CustomerPatchModel
        {
            Name = "New Updated Name"
        };

        _mockService.Setup(s => s.Update(customerId, testCustomer)).ReturnsAsync(true);

        // Act
        var result = await _controller.PatchById(customerId, testCustomer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Customer {customerId} is updated", okResult.Value);
    }

    [Fact]
    public async Task Patch_NotValid(){
        
        // Arrange
        string customerId = "valid-id";
        var patchModel = new CustomerPatchModel { Name = "Name Updated" };
        _mockService.Setup(s => s.Update(customerId, patchModel)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _controller.PatchById(customerId, patchModel));
        Assert.Equal("Update is not successful", exception.Message);
        
    }

    [Fact]
    public async Task GetAll_Success()
    {
        //Arrange
        var customerList = new List<CustomerGetModel>
        {
            new CustomerGetModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Ali",
                Email = "test@test.com",
                Addresses =
                [
                    new CustomerGetModel.AddressGetModel
                    {
                        AddressLine = "Denizli Caddesi",
                        City = "Izmir",
                        Country = "Turkiye",
                        CityCode = 35670
                    }
                ],
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
            new CustomerGetModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Ali2",
                Email = "test2@test2.com",
                Addresses =
                [
                    new CustomerGetModel.AddressGetModel
                    {
                        AddressLine = "Izmir Caddesi",
                        City = "Istanbul",
                        Country = "Turkiye",
                        CityCode = 34460
                    }
                ],
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
        };
        _mockService.Setup(s => s.GetAll()).ReturnsAsync(customerList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<CustomerGetModel>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);

    }
    
    [Fact]
    public async Task Get_Success()
    {
        //Arrage
        string customerId = "valid-id";
        var testCustomer = new CustomerGetModel
        {
            Id = customerId,
            Email = "test@test.com",
            Name = "test",
            Addresses =
            [
                new CustomerGetModel.AddressGetModel
                {
                    AddressLine = "Denizli Caddesi",
                    City = "Izmir",
                    Country = "Turkiye",
                    CityCode = 35670
                }
            ],
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };
        _mockService.Setup(
            service => service.GetById(customerId)).ReturnsAsync(testCustomer);
        
        //Act
        var result = await _controller.Get(customerId);
        
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<CustomerGetModel>(okResult.Value);
        Assert.Equal(customerId, returnValue.Id);

    }
    
    [Fact]
    public async Task Get_Exception()
    {
        string customerId = "invalid-id";
        _mockService.Setup(s => s.GetById(customerId)).ThrowsAsync(new Exception("Service error"));

        // Act
        var exception = await Assert.ThrowsAsync<CustomException>(() => _controller.Get(customerId));
        //Assert
        Assert.Equal("Error at Get endpoint", exception.Message);

    }

    [Fact]
    public async Task Validate_Success()
    {
        // Arrange
        string customerId = "valid-id";
        _mockService.Setup(s => s.Validate(customerId)).ReturnsAsync(true);

        // Act
        var result = await _controller.Validate(customerId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<bool>(okResult.Value);
        Assert.True(returnValue);
    }
    
    [Fact]
    public async Task Validate_NotValid()
    {
        string customerId = "invalid-id";
        _mockService.Setup(s => s.Validate(customerId)).ReturnsAsync(false);

        // Act
        var result = await _controller.Validate(customerId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Customer is not Valid", badRequestResult.Value);
    }
    
}