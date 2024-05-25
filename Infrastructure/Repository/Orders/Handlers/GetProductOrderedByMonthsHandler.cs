using Application.DTO.Response.Order;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Orders.Handlers
{
    public class GetProductOrderedByMonthsHandler(DataAccess.IDbContextFactory<AppDBContext> contextFactory) : IRequestHandler<GetProductOrderedByMonthsQuery, IEnumerable<GetProductOrderedByMonthsResponseDTO>>
    {
        public async Task<IEnumerable<GetProductOrderedByMonthsResponseDTO>> Handle(GetProductOrderedByMonthsQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var orders = new List<Order>();
            var data = new List<GetProductOrderedByMonthsResponseDTO>();
            if (!string.IsNullOrEmpty(request.UserId))
                orders = await dbContext.Orders.AsNoTracking().Where(x => x.ClientID.ToString() == request.UserId.ToString()).ToListAsync(cancellationToken: cancellationToken);
            else
                orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

            var selectedOrdersWithin12Months = orders.Where(o => o.DateOrdered.Date >= DateTime.Today.AddMonths(12))
               .GroupBy(g => new { month = g.DateOrdered.Month }).ToList();

            foreach (var order in selectedOrdersWithin12Months.OrderBy(x=>x.Key.month))
            {
                data.Add(new GetProductOrderedByMonthsResponseDTO
                {
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(order.Key.month),
                    TotalAmount = order.Sum(x => x.TotalAmount)
                });
            }

            return data;

        }
    }
}
