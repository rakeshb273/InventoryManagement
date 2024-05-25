using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response.Order
{
    public class GetOrdersByMonthResponseDTO
    {
        public string MonthName { get; set; }
        public Decimal TotalAmount{ get; set; }
        public string FormattedTotalAmount => TotalAmount.ToString("#,##0.00");
    }
}
