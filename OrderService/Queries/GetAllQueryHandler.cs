using MongoDB.Driver;
using OrderService.Configs;
using OrderService.Models;
using OrderService.V1.Models.QueryModels;

namespace OrderService.Queries;

public class GetAllQueryHandler : IQueryHandler<GetAllQuery, IEnumerable<Order>>
{
    private readonly DbContext _database;

    public GetAllQueryHandler(DbContext database)
    {
        _database = database;
    }

    public async Task<IEnumerable<Order>> Handle(GetAllQuery query)
    {
        var result = await _database.Orders.Find(o => true).ToListAsync();
        return result;
    }
}