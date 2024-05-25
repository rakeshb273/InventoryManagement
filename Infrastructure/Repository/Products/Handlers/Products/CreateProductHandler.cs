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
    public class CreateProductHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<CreateProductCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var product = dbContext.Categories.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(request.ProductModel.Name.ToLower()), cancellationToken: cancellationToken).Result;
                if (product != null)
                {
                    return GeneralDbResponses.ItemAlreadyExist(request.ProductModel.Name);
                }
                var data = request.ProductModel.Adapt(new Product());
                dbContext.Products.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemCreated(request.ProductModel.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
