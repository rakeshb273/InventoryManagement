using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Application.Service.Products.Commands.Products;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.products
{
    public class DeleteProductHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<DeleteProductCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var Product = dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken).Result;
                if (Product == null)
                {
                    return GeneralDbResponses.ItemNotFound("Product");
                }

                dbContext.Categories.Remove(Product); 
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemDeleted(Product.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
