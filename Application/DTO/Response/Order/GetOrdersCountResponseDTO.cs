using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response.Order
{
    public record GetOrdersCountResponseDTO(int Processing,int Delivering, int Delivered, int Cancelled);
    
}
