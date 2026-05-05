using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Extensions.Pagination;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Services.Interfaces.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Order.Queries.GetAllUserOrders;

public class GetAllUserOrdersQueryHandler(
    IAppDbContext dbContext, 
    ICurrentUserService currentUserService) : IRequestHandler<GetAllUserOrdersQuery, PaginatedQueryList<GetAllUserOrdersResponse>>
{
    public async Task<PaginatedQueryList<GetAllUserOrdersResponse>> Handle(GetAllUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var userid = currentUserService.UserId;
        
        var orders = await dbContext.Orders
            .Where(o => o.CustomerId == userid && (!request.Status.HasValue || o.Status == request.Status))
            .AsNoTracking()
            .ToGetAllUserOrdersResponse()
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        return orders;
    }
}