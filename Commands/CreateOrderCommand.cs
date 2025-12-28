// public record CreateOrderCommand(string FirstName, string LastName, string Status, decimal TotalCost);
using MediatR;

public record CreateOrderCommand(string FirstName, string LastName, string Status, decimal TotalCost) : IRequest<OrderDto>;