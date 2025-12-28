
// public class OrderCreatedProjectionHandler(ReadDbContext context) : IEventHandler<OrderCreatedEvent>
using MediatR;

public class OrderCreatedProjectionHandler(ReadDbContext context) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = notification.Orderid,
            FirstName = notification.FirstName,
            LastName = notification.LastName,
            Status = "Created",
            CreatedAt = DateTime.Now,
            TotalCost = notification.TotalCost
        };

        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync();
    }

    // public async Task HandleAsync(OrderCreatedEvent evt)
    // {
    //     var order = new Order
    //     {
    //         Id = evt.Orderid,
    //         FirstName = evt.FirstName,
    //         LastName = evt.LastName,
    //         Status = "Created",
    //         CreatedAt = DateTime.Now,
    //         TotalCost = evt.TotalCost
    //     };

    //     await context.Orders.AddAsync(order);
    //     await context.SaveChangesAsync();
    // }
}