using MongoDB.Driver;
using OrderService.Configs;
using OrderService.Models;
using OrderService.V1.Models.CommandModels;

namespace OrderService.Commands;

public class ChangeStatusCommandHandler : ICommandHandler<ChangeStatusCommand, bool>
{
    private readonly DbContext _database;

    public ChangeStatusCommandHandler(DbContext database)
    {
        _database = database;
    }
    
    public async Task<bool> Handle(ChangeStatusCommand command)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.Id, command.OrderId);
        var update = Builders<Order>.Update
                                .Set(o => o.Status, command.NewStatus)
                                .Set(o => o.UpdatedAt, DateTime.Now);
        
        var result = await _database.Orders.UpdateOneAsync(filter, update);
        
        return result.ModifiedCount > 0;
    }
    
}