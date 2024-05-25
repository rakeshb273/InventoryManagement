using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Application.Service.Products.Commands.Locations;
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
    public  class UpdateLocationHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<UpdateLocationCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var data = dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.LocationModel.Id, cancellationToken: cancellationToken).Result;
                if (data == null)
                {
                    return GeneralDbResponses.ItemNotFound(request.LocationModel.Name);
                }
                dbContext.Entry(data).State = EntityState.Detached;

                var adaptData = request.LocationModel.Adapt(new Location());
                dbContext.Categories.Remove(data);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemUpdated(data.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}