using Application.DTO.Response;
using Application.Service.Orders.Commands;
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
    public class UpdateClientOrderHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<UpdateClientOrderCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(UpdateClientOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var data = await dbContext.Orders.Where(_ => _.Id == request.Model.OrderId).FirstOrDefaultAsync();

                if (data == null) { return new ServiceResponse(false, "Order not found"); }
                data.OrderState = request.Model.OrderState;
                data.DeliveredDate = request.Model.DeliveringDate;
                await dbContext.SaveChangesAsync(cancellationToken);
                return new ServiceResponse(true, "Order updated successfully");
            }
            catch(Exception ex)
            {
                return new ServiceResponse(false, $"{ex.Message}");
            }
        }
    }
}
