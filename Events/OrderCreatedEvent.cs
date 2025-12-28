// public record OrderCreatedEvent
// (
//     int Orderid,
//     string FirstName,
//     string LastName,
//     decimal TotalCost
// );

using MediatR;

public record OrderCreatedEvent
(
    int Orderid,
    string FirstName,
    string LastName,
    decimal TotalCost
) : INotification;