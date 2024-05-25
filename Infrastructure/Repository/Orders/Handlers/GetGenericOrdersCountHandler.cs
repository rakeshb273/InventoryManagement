using Application.DTO.Response.Order;
using Application.Extension;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Orders.Handlers
{
    public class GetGenericOrdersCountHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<GetGenericOrdersCountQuery, GetOrdersCountResponseDTO>
    {
        public async Task<GetOrdersCountResponseDTO> Handle(GetGenericOrdersCountQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var list = new List<Order>();
            if (!request.IsAdmin)
                list = await dbContext.Orders.AsNoTracking().Where(x => x.ClientID.ToString() == request.UserId.ToString()).ToListAsync(cancellationToken:cancellationToken);
            else
                list = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

            int Processing = list.Count(_ => _.OrderState == OrderState.Processing);
            int Cancelled = list.Count(_ => _.OrderState == OrderState.Cancelled);
            int Delivered = list.Count(_ => _.OrderState == OrderState.Delevered);
            int Delivering = list.Count(_ => _.OrderState == OrderState.Delevering);

            return new GetOrdersCountResponseDTO(Processing, Delivering, Delivered, Cancelled);


        }
    }
}
