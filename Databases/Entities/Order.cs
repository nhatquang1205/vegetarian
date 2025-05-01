using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Databases.Entities
{
    public class Order : BaseEntity<int>
    {
        public required string OrderCode { get; set; }
        public int UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Note { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
    }
}