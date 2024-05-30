using MongoDB.Driver;
using OrderService.Configs;
using OrderService.Models;
using OrderService.V1.Models.CommandModels;

namespace OrderService.Commands;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand, bool>
{
    private readonly DbContext _database;

    public DeleteCommandHandler(DbContext database)
    {
        _database = database;
    }

    public async Task<bool> Handle(DeleteCommand command)
    {
        var result = await _database.Orders.DeleteOneAsync(x => x.Id == command.OrderId);

        return result.IsAcknowledged;
    }
}