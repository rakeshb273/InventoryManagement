using Application.DTO.Response.Order;
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
    public class GetOrdersByIdHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<GetOrdersByIdQuery, IEnumerable<GetOrderResponseDTO>>
    {
        public async Task<IEnumerable<GetOrderResponseDTO>> Handle(GetOrdersByIdQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var orders = await dbContext.Orders.AsNoTracking().Where(x => x.ClientID.ToString() == request.UserId.ToString()).ToListAsync(cancellationToken: cancellationToken);

            var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

            return orders.Select(order => new GetOrderResponseDTO
            {
                Id = order.Id,
                Price = order.Price,
                DeliveryingDate = order.DeliveredDate,
                OrderDate = order.DateOrdered,
                ProductId = order.ProductID,
                Quantity = order.Quantity,
                State = order.OrderState,
                ProductName = products.FirstOrDefault(_ => _.Id == order.ProductID).Name,
                ProductImage = products.FirstOrDefault(_ => _.Id == order.ProductID).Base64Image,
                SerialNumber = products.FirstOrDefault(_ => _.Id == order.ProductID).SerialNumber,

            });
        }
    }
}
