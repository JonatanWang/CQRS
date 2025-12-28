using MediatR;
using Microsoft.EntityFrameworkCore;

// public class GetOrderSummariesQueryHandler : IQueryHandler<GetOrderSummariesQuery, List<OrderSummaryDto>>
public class GetOrderSummariesQueryHandler : IRequestHandler<GetOrderSummariesQuery, List<OrderSummaryDto>>
{

    // private readonly AppDbContext _context;
    private readonly ReadDbContext _context;

    public GetOrderSummariesQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderSummaryDto>> Handle(GetOrderSummariesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
        .AsNoTracking()
        .Select(o => new OrderSummaryDto(
            o.Id,
            o.FirstName + ' ' + o.LastName,
            o.Status,
            o.TotalCost
        )).ToListAsync(cancellationToken);
    }

    // public async Task<List<OrderSummaryDto>?> HandleAsync(GetOrderSummariesQuery query)
    // {
    //     return await _context.Orders.Select(o => new OrderSummaryDto(
    //         o.Id,
    //         o.FirstName + ' ' + o.LastName,
    //         o.Status,
    //         o.TotalCost
    //     )).ToListAsync();
    // }
}
