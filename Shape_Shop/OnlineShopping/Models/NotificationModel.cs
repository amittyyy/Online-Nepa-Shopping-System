using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class NotificationModel
    {
        [Required]
        public long NotificationId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required, DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public byte NotificationTypeId { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        public bool IsNew { get; set; }
    }
}