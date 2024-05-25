﻿using Application.DTO.Response;
using Application.DTO.Response.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Orders.Queries
{
    public record GetGenericOrdersCountQuery(string UserId ,bool IsAdmin=false ):IRequest<GetOrdersCountResponseDTO>;
}
