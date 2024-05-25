using Application.DTO.Response;
using Application.DTO.Response.Order;
using Application.Extension;
using Application.Extension.Identity;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 

namespace Infrastructure.Repository.Orders.Handlers
{
    public class GetAllOrdersHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory, UserManager<ApplicationUser> userManager) : IRequestHandler<GetAllOrdersQuery, IEnumerable<GetOrderResponseDTO>>
    {
        public async Task<IEnumerable<GetOrderResponseDTO>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            var users = await userManager.Users.ToListAsync();
            return orders.Select(order => new GetOrderResponseDTO
            {

                Id = order.Id,
                ProductName = products.FirstOrDefault(_ => _.Id == order.ProductID).Name,
                Price = order.Price,
                DeliveryingDate = order.DeliveredDate,
                ProductId = order.ProductID,
                ProductImage = products.FirstOrDefault(_ => _.Id == order.ProductID).Base64Image,
                Quantity = order.Quantity,
                State = order.OrderState,
                ClientID = order.ClientID,
                ClientName = users.FirstOrDefault(_ => _.Id.Equals(order.ClientID)).Name,
                SerialNumber = products.FirstOrDefault(_ => _.Id == order.ProductID).SerialNumber


            }).ToList();
        }
    }
}
