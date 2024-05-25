using Application.DTO.Response;
using Application.Extension;
using Application.Service.Orders.Commands;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Orders.Handlers
{
    public class CancelOrderHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<CancelOrderCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (order == null)
                    GeneralDbResponses.ItemNotFound("Order");
                order.OrderState = OrderState.Cancelled;
                await dbContext.SaveChangesAsync(cancellationToken);
                return new ServiceResponse(true, "Order cancellled successfully");

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
