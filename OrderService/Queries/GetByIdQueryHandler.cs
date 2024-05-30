using MongoDB.Driver;
using OrderService.Configs;
using OrderService.Models;
using OrderService.V1.Models.QueryModels;

namespace OrderService.Queries;

public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, Order>
{
    private readonly DbContext _database;

    public GetByIdQueryHandler(DbContext database)
    {
        _database = database;
    }

    public async Task<Order> Handle(GetByIdQuery query)
    {
        return await _database.Orders.Find(o => o.Id == query.OrderId).FirstOrDefaultAsync();
    }
}