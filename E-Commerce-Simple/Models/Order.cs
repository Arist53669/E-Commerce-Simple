﻿using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Simple.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Display(Name = "Order status")]
        public string OrderStatus { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public double Amount { get; set; }

        [Display(Name = "Order Items")]
        public List<OrderItem> OrderItems { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
