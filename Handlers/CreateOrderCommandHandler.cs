using FluentValidation;
using MediatR;

// public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    // public static async Task<Order> Handle(CreateOrderCommand command, AppDbContext context)
    // {
    //     var order = new Order
    //     {
    //         FirstName = command.FirstName,
    //         LastName = command.LastName,
    //         Status = command.Status,
    //         CreatedAt = DateTime.Now,
    //         TotalCost = command.TotalCost
    //     };

    //     await context.Orders.AddAsync(order);
    //     await context.SaveChangesAsync();

    //     return order;
    // }

    // private readonly AppDbContext _context;
    private readonly WriteDbContext _context;
    private readonly IValidator<CreateOrderCommand> _validator;
    private readonly IMediator _mediator;

    // private readonly IEventPublisher _eventPublisher;

    public CreateOrderCommandHandler(
        WriteDbContext context, 
        IValidator<CreateOrderCommand> validator, 
        // IEventPublisher publisher,
        IMediator mediator)
    {
        _context = context;
        _validator = validator;
        // _eventPublisher = publisher;
        _mediator = mediator;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // var validationResult = await _validator.ValidateAsync(command);
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if(!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var order = new Order
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Status = request.Status,
            CreatedAt = DateTime.Now,
            TotalCost = request.TotalCost
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        var orderCreatedEvent = new OrderCreatedEvent
        (
            order.Id,
            order.FirstName,
            order.LastName,
            order.TotalCost
        );

        // await _eventPublisher.PublishAsync(orderCreatedEvent);
        await _mediator.Publish(orderCreatedEvent);
        
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
    

    // public async Task<OrderDto> HandleAsync(CreateOrderCommand command)
    // {
    //     var validationResult = await _validator.ValidateAsync(command);
    //     if(!validationResult.IsValid)
    //     {
    //         throw new ValidationException(validationResult.Errors);
    //     }

    //     var order = new Order
    //     {
    //         FirstName = command.FirstName,
    //         LastName = command.LastName,
    //         Status = command.Status,
    //         CreatedAt = DateTime.Now,
    //         TotalCost = command.TotalCost
    //     };

    //     await _context.Orders.AddAsync(order);
    //     await _context.SaveChangesAsync();

    //     var orderCreatedEvent = new OrderCreatedEvent
    //     (
    //         order.Id,
    //         order.FirstName,
    //         order.LastName,
    //         order.TotalCost
    //     );

    //     await _eventPublisher.PublishAsync(orderCreatedEvent);
        
    //     return new OrderDto
    //         (
    //             order.Id,
    //             order.FirstName,
    //             order.LastName,
    //             order.Status,
    //             order.CreatedAt,
    //             order.TotalCost
    //         );
    // }
}