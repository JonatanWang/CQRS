using MediatR;
using Microsoft.EntityFrameworkCore;

// public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    // public static async Task<Order?> Handle(GetOrderByIdQuery query, AppDbContext context)
    // {
    //     return await context.Orders.FirstOrDefaultAsync(order => order.Id == query.OrderId);
    // }

    // private readonly AppDbContext _context;
    private readonly ReadDbContext _context;
    
    public GetOrderByIdQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        // var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == query.OrderId);
        var order = await _context.Orders
        .AsNoTracking()
        .FirstOrDefaultAsync(o => o.Id == request.OrderId);

        if(order == null)
        {
            return null;
        }    

        return new OrderDto
        (
            order.Id,
            order.FirstName,
            order.LastName,
            order.Status,
            order.CreatedAt,
            order.TotalCost
        );
    }

    // public async Task<OrderDto?> HandleAsync(GetOrderByIdQuery query)
    // {
    //     var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == query.OrderId);

    //     if(order == null)
    //     {
    //         return null;
    //     }    

    //     return new OrderDto
    //     (
    //         order.Id,
    //         order.FirstName,
    //         order.LastName,
    //         order.Status,
    //         order.CreatedAt,
    //         order.TotalCost
    //     );
    // }
}