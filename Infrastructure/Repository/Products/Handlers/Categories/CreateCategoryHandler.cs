using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
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

namespace Infrastructure.Repository.Products.Handlers.Categories
{
    public class CreateCategoryHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<CreateCategoryCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext= contextFactory.CreateDbContext();
                var category = dbContext.Categories.FirstOrDefaultAsync(x=>x.Name.ToLower().Equals(request.CategoryModel.Name.ToLower()),cancellationToken:cancellationToken).Result;
                if (category != null)
                {
                    return GeneralDbResponses.ItemAlreadyExist(request.CategoryModel.Name);
                }
                var data = request.CategoryModel.Adapt(new Category());
                dbContext.Categories.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemCreated(request.CategoryModel.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
