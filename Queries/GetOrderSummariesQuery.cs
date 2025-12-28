// public record GetOrderSummariesQuery();
using MediatR;

public record GetOrderSummariesQuery() : IRequest<List<OrderSummaryDto>>;