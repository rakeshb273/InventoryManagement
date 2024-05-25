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

namespace Infrastructure.Repository.Products.Handlers.locations
{
    public class CreateLocationHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<CreateLocationCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var Location = dbContext.Categories.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(request.LocationModel.Name.ToLower()), cancellationToken: cancellationToken).Result;
                if (Location != null)
                {
                    return GeneralDbResponses.ItemAlreadyExist(request.LocationModel.Name);
                }
                var data = request.LocationModel.Adapt(new Location());
                dbContext.Locations.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemCreated(request.LocationModel.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
