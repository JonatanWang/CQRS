using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.OpenApi.Generated;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("BaseConnection"))); 
builder.Services.AddDbContext<ReadDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ReadDbConnection"))); 
builder.Services.AddDbContext<WriteDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("WriteDbConnection"))); 

// builder.Services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
// builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto>, GetOrderByIdQueryHandler>();
builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
// builder.Services.AddScoped<IQueryHandler<GetOrderSummariesQuery, List<OrderSummaryDto>>, GetOrderSummariesQueryHandler>();
// builder.Services.AddScoped<IEventPublisher, ConsoleEventPublisher>();
// builder.Services.AddScoped<IEventPublisher, InProcessEventPublisher>();
// builder.Services.AddScoped<IEventHandler<OrderCreatedEvent>, OrderCreatedProjectionHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Order API";
        options.Theme = ScalarTheme.DeepSpace;
        options.Layout = ScalarLayout.Modern;
        options.HideClientButton = true;
    });
}

// app.MapPost("/api/orders", async (AppDbContext context, Order order) =>
// app.MapPost("/api/orders", async (AppDbContext context, CreateOrderCommand command) =>
// app.MapPost("/api/orders", async (ICommandHandler<CreateOrderCommand, OrderDto> handler, CreateOrderCommand command) =>
app.MapPost("/api/orders", async (IMediator mediator, CreateOrderCommand command) =>
{
    // await context.Orders.AddAsync(order);
    // await context.SaveChangesAsync();

    try
    {
    // var createOrder = await CreateOrderCommandHandler.Handle(command, context);
    // var createOrder = await handler.HandleAsync(command);
    var createOrder = await mediator.Send(command);
    if(createOrder == null)
    {
        return Results.BadRequest("Failed to create order. ");
    }
    // return Results.Created($"/api/orders{order.Id}", order);
    return Results.Created($"/api/orders{createOrder.Id}", createOrder);
        
    }
    catch (ValidationException e)
    {
        var errors = e.Errors.Select(e => new {e.PropertyName, e.ErrorMessage});
        return Results.BadRequest(errors);
    }
});

// app.MapGet("/api/orders/{id}", async (AppDbContext context, int id) =>
// app.MapGet("/api/orders/{id}", async (IQueryHandler<GetOrderByIdQuery, OrderDto> handler, int id) =>
app.MapGet("/api/orders/{id}", async (IMediator mediator, int id) =>
{
    // var order = await context.Orders.FirstOrDefaultAsync(order => order.Id == id);
    // var order = await GetOrderByIdQueryHandler.Handle(new GetOrderByIdQuery(id), context);
    // var order = await handler.HandleAsync(new GetOrderByIdQuery(id));
    var order = await mediator.Send(new GetOrderByIdQuery(id));
    if(order == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(order);
});

// app.MapGet("/api/orders", async(IQueryHandler<GetOrderSummariesQuery, List<OrderSummaryDto>> handler) =>
app.MapGet("/api/orders", async(IMediator mediator) =>
{
    // var summaries = await handler.HandleAsync(new GetOrderSummariesQuery());
    var summaries = await mediator.Send(new GetOrderSummariesQuery());

    return Results.Ok(summaries);
});

app.Run();