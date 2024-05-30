
using OrderService.Configs;
using OrderService.Models;
using OrderService.V1.Models.CommandModels;

namespace OrderService.Commands;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly DbContext _database;

    public CreateCommandHandler(DbContext database)
    {
        _database = database;
    }

    public async Task<string> Handle(CreateCommand command)
    {
        
        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = command.CustomerId,
            Quantity = command.Quantity,
            Price = command.Price,
            Status = "In process",
            Address = command.Address,
            Products = command.Products,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        await _database.Orders.InsertOneAsync(order);
        return order.Id;
    }
}