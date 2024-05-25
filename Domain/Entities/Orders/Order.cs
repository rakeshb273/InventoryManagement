using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Orders
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public DateTime DeliveredDate { get; set; }
        public Guid ProductID { get; set; }
        public string ClientID { get; set; }
        public int  Quantity{ get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderState { get; set; }

    }
}
