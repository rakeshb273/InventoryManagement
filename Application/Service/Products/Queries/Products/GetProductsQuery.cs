using Application.DTO.Response.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Products.Queries.Products
{
    public record GetProductsQuery : IRequest<IEnumerable<GetProductResponseDTO>> { };
     
}
