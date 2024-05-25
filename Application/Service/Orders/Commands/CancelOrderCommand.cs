﻿using Application.DTO.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Orders.Commands
{
    public record CancelOrderCommand(Guid Id) : IRequest<ServiceResponse>;
}
