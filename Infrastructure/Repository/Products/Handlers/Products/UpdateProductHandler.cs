using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Application.Service.Products.Commands.Products;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.products
{
    public class UpdateProductHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<UpdateProductCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var data = dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.ProductModel.Id, cancellationToken: cancellationToken).Result;
                if (data == null)
                {
                    return GeneralDbResponses.ItemNotFound(request.ProductModel.Name);
                }
                dbContext.Entry(data).State = EntityState.Detached;
                var adaptData = request.ProductModel.Adapt(new Product());
                dbContext.Products.Update(adaptData);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemUpdated(request.ProductModel.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}