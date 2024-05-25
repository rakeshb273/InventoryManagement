using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.Categories
{
    public class DeleteLocationHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<DeleteCategoryCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var category = dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken).Result;
                if (category == null)
                {
                    return GeneralDbResponses.ItemNotFound("Category");
                }

                dbContext.Categories.Remove(category); 
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemDeleted(category.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
