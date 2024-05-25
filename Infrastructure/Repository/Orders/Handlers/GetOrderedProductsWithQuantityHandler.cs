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
    public class GetOrderedProductsWithQuantityHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<GetOrderedProductsWithQuantityQuery, IEnumerable<GetOrderedProductsWithQuantityResponseDTO>>
    {
        public async Task<IEnumerable<GetOrderedProductsWithQuantityResponseDTO>> Handle(GetOrderedProductsWithQuantityQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var orders = new List<Order>();
            var data = new List<GetOrderedProductsWithQuantityResponseDTO>();
            if(!string.IsNullOrEmpty(request.UserId))
                orders= await dbContext.Orders.AsNoTracking().Where(x => x.ClientID.ToString() == request.UserId.ToString()).ToListAsync(cancellationToken: cancellationToken);
            else
                orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);


            var selectedOrdersWithin12Months=orders.Where(o=>o.DateOrdered.Date>=DateTime.Today.AddMonths(12))
                .GroupBy(g=> new {name=g.ProductID}).ToList();

            foreach(var order in selectedOrdersWithin12Months)
            {
                data.Add(new GetOrderedProductsWithQuantityResponseDTO
                {
                    ProductName = (await dbContext.Products.FirstOrDefaultAsync(_ => _.Id == order.Key.name, cancellationToken: cancellationToken)).Name,
                    QuantityOrdered = order.Sum(x => x.Quantity)
                });
            }

            return data;
        }
    }
}
