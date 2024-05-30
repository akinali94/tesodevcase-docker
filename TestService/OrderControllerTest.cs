using OrderService.Commands;
using OrderService.Configs;
using OrderService.Configs.HttpConfig;
using OrderService.Helpers;
using OrderService.Models;
using OrderService.Queries;
using OrderService.V1.Controllers;
using OrderService.V1.Models.CommandModels;
using OrderService.V1.Models.QueryModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace OrderService.Test;

public class OrderControllerTest
{
    private readonly Mock<ICommandHandler<CreateCommand, string>> _mockCreateCommandHandler;
    private readonly Mock<ICommandHandler<UpdateCommand, bool>> _mockUpdateCommandHandler;
    private readonly Mock<ICommandHandler<DeleteCommand, bool>> _mockDeleteCommandHandler;
    private readonly Mock<ICommandHandler<ChangeStatusCommand, bool>> _mockChangeStatusCommandHandler;
    private readonly Mock<IQueryHandler<GetAllQuery, IEnumerable<Order>>> _mockGetAllQueryHandler;
    private readonly Mock<IQueryHandler<GetByIdQuery, Order>> _mockGetByIdQueryHandler;
    private readonly Mock<IQueryHandler<GetByCustomerIdQuery, IEnumerable<Order>>> _mockGetByCustomerIdQueryHandler;
    private readonly Mock<IHttpHandler> _mockHttpHandler;
    private readonly Mock<IKafkaProducerConfig> _mockProducerConfig;
    private readonly Mock<ILogger<OrderController>> _mockLogger;

    private readonly OrderController _controller;

    public OrderControllerTest()
    {
        _mockCreateCommandHandler = new Mock<ICommandHandler<CreateCommand, string>>();
        _mockUpdateCommandHandler = new Mock<ICommandHandler<UpdateCommand, bool>>();
        _mockDeleteCommandHandler = new Mock<ICommandHandler<DeleteCommand, bool>>();
        _mockChangeStatusCommandHandler = new Mock<ICommandHandler<ChangeStatusCommand, bool>>();
        _mockGetAllQueryHandler = new Mock<IQueryHandler<GetAllQuery, IEnumerable<Order>>>();
        _mockGetByIdQueryHandler = new Mock<IQueryHandler<GetByIdQuery, Order>>();
        _mockGetByCustomerIdQueryHandler = new Mock<IQueryHandler<GetByCustomerIdQuery, IEnumerable<Order>>>();
        _mockHttpHandler = new Mock<IHttpHandler>();
        _mockProducerConfig = new Mock<IKafkaProducerConfig>();
        _mockLogger = new Mock<ILogger<OrderController>>();
        

        _controller = new OrderController(
            _mockCreateCommandHandler.Object,
            _mockUpdateCommandHandler.Object,
            _mockDeleteCommandHandler.Object,
            _mockChangeStatusCommandHandler.Object,
            _mockGetAllQueryHandler.Object,
            _mockGetByIdQueryHandler.Object,
            _mockGetByCustomerIdQueryHandler.Object,
            _mockProducerConfig.Object,
            _mockLogger.Object,
            _mockHttpHandler.Object);
    }

    [Fact]
    public async Task Create_Success()
    {
        // Arrange
        var address = new Address
        {
            AddressLine = "Izmir Caddesi",
            City = "Denizli",
            Country = "Turkiye",
            CityCode = 20160
        };
        var product1 = new Product
        { 
            Id = "new-id", 
            ImageUrl = "www.Url.com",
            Name = "Banana"
        };
        var product2 = new Product
        {
            Id = "new-id2",
            ImageUrl = "www.Url.com",
            Name = "Grape"
        };

        var productList = new List<Product>();
        productList.Add(product1);
        productList.Add(product2);
        
        var command = new CreateCommand("customer-id", 1, 10, address, productList); 
        var orderId = "order-id";
            
        var customerResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        _mockHttpHandler.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(customerResponseMessage);
        _mockCreateCommandHandler.Setup(handler => handler.Handle(command)).ReturnsAsync(orderId);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal($"Created ID: {orderId}", createdResult.Value);
    }
        
        [Fact]
        public async Task Create_InvalidCustomer()
        {
            // Arrange
            // Arrange
            var address = new Address
            {
                AddressLine = "Izmir Caddesi",
                City = "Denizli",
                Country = "Turkiye",
                CityCode = 20160
            };
            var product1 = new Product
            {
                Id = "new-id",
                ImageUrl = "www.Url.com",
                Name = "Banana"
            };

            var productList = new List<Product>();
            productList.Add(product1);

            var command = new CreateCommand("invalid-id", 1, 10, address, productList);
            var customerResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            _mockHttpHandler.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(customerResponseMessage);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Customer Id is not valid", badRequestResult.Value);
        }
        

        [Fact]
        public async Task GetById_Success()
        {
            // Arrange
            var orderId = "valid-id";
            var order = new Order { Id = orderId };
            _mockGetByIdQueryHandler.Setup(handler => handler.Handle(It.IsAny<GetByIdQuery>())).ReturnsAsync(order);

            // Act
            var result = await _controller.GetById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(orderId, returnValue.Id);
        }
        
        [Fact]
        public async Task Update_Success()
        {
            // Arrange
            var address = new Address
            {
                AddressLine = "Izmir Caddesi",
                City = "Denizli",
                Country = "Turkiye",
                CityCode = 20160
            };
            var product1 = new Product
            {
                Id = "new-id",
                ImageUrl = "www.Url.com",
                Name = "Banana"
            };

            var productList = new List<Product>();
            productList.Add(product1);
            
            var orderId = "valid-id";
            var customerId = "valid-cust-id";
            var updateCommand = new UpdateCommand(orderId,customerId,1,address,productList);
            _mockUpdateCommandHandler.Setup(handler => handler.Handle(updateCommand)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(updateCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Order {orderId} is updated", okResult.Value);
        }
        
        [Fact]
        public async Task Update_Failure()
        {
            // Arrange
            var address = new Address
            {
                AddressLine = "Izmir Caddesi",
                City = "Denizli",
                Country = "Turkiye",
                CityCode = 20160
            };
            var product1 = new Product
            {
                Id = "new-id",
                ImageUrl = "www.Url.com",
                Name = "Banana"
            };

            var productList = new List<Product>();
            productList.Add(product1);
            
            var orderId = "valid-id";
            var customerId = "valid-cust-id";
            var updateCommand = new UpdateCommand(orderId, customerId, 1, address, productList);
            _mockUpdateCommandHandler.Setup(handler => handler.Handle(updateCommand)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(updateCommand);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Update is not successful", notFoundResult.Value);
        }
        
        [Fact]
        public async Task GetAll_Success()
        {
            // Arrange
            var orders = new List<Order> { new Order { Id = "order1" }, new Order { Id = "order2" } };
            _mockGetAllQueryHandler.Setup(handler => handler.Handle(It.IsAny<GetAllQuery>())).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            
        }
        
        [Fact]
        public async Task GetAll_NoOrders_ThrowsCustomException()
        {
            // Arrange
            _mockGetAllQueryHandler.Setup(handler => handler.Handle(It.IsAny<GetAllQuery>())).ReturnsAsync(new List<Order>());

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _controller.GetAll());
        }
}