using AuraPerfumes.Models;
using System.ComponentModel.DataAnnotations;

namespace AuraPerfumes.Models.DTOs
{
    public class CheckoutVM
    {
        public ShoppingCart Cart { get; set; } = null!;

        [Required]
        public string CourierName { get; set; } = "Speedy";

        public double ShippingPrice { get; set; }

        [Range(0, 100000)]
        public double Discount { get; set; }

        public double Subtotal { get; set; }
        public double GrandTotal { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public string PhoneNumber { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string AddressLine { get; set; } = "";

        [Required]
        public string City { get; set; } = "";

        public string PostalCode { get; set; } = "";

        public string Notes { get; set; } = "";

        [Required]
        public string PaymentMethod { get; set; } = "CashOnDelivery";
        public string PromoCode { get; set; } = "";
        public string CardHolderName { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public string ExpiryDate { get; set; } = "";
        public string CVV { get; set; } = "";
    }
}