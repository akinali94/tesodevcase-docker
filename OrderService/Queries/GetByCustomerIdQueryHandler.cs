using MongoDB.Driver;
using OrderService.Configs;
using OrderService.Models;
using OrderService.V1.Models.QueryModels;

namespace OrderService.Queries;

public class GetByCustomerIdQueryHandler : IQueryHandler<GetByCustomerIdQuery, IEnumerable<Order>>
{
    private readonly DbContext _database;

    public GetByCustomerIdQueryHandler(DbContext database)
    {
        _database = database;
    }
    
    public async Task<IEnumerable<Order>> Handle(GetByCustomerIdQuery query)
    {
        var orders = 
            await _database.Orders.Find(x => x.CustomerId == query.CustomerId).ToListAsync();

        return orders;
    }
}