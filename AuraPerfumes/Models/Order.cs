using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuraPerfumes.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int OrderStatusId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Notes { get; set; }
        public string? PaymentMethod { get; set; }

        public string? CourierName { get; set; }
        public double ShippingPrice { get; set; }
        public double Discount { get; set; }
        public double TotalPrice { get; set; }

        public string ShippingStatus { get; set; } = "Processing";
        public DateTime EstimatedDeliveryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }
}