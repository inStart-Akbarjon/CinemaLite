using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Domain.Enums;
using MediatR;

namespace CinemaLite.Application.CQRS.Order.Queries.GetAllUserOrders;

public record GetAllUserOrdersQuery(OrderStatus? Status, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedQueryList<GetAllUserOrdersResponse>>
{
}