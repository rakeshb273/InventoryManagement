using Application.DTO.Response;
using Application.Extension;
using Application.Service.Orders.Commands;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Orders.Handlers
{
    public class CreateOrderhandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<CreateOrderCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var product=await dbContext.Products.FirstOrDefaultAsync(_=>_.Id==request.Model.ProductId,cancellationToken:cancellationToken);
                var data = request.Model.Adapt<Order>();
                data.TotalAmount = product.Price * data.Quantity;
                data.OrderState = OrderState.Processing;
                data.Price= product.Price;
                dbContext.Orders.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);
                return new ServiceResponse(true, "Order placed successfully");

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
