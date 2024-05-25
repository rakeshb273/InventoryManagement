using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Application.Service.Products.Commands.Locations;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.locations
{
    public class DeleteLocationHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<DeleteLocationCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var Location = dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken).Result;
                if (Location == null)
                {
                    return GeneralDbResponses.ItemNotFound("Location");
                }

                dbContext.Categories.Remove(Location); 
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemDeleted(Location.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
