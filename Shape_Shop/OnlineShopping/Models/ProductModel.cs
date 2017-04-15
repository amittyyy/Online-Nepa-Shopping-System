using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Models
{
    public class ProductModel
    {
        [Required, DisplayName("Category")]
        public int MainCategoryId { get; set; }

        [DisplayName("Sub-category")]
        public int SubCategoryId { get; set; }

        [Required, DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required, Range(1, 9999999)]
        public float Price { get; set; }

        [Required, DisplayName("Expiry Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ExpiryDate { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [Required, StringLength(4000, MinimumLength = 5)]
        public string Description { get; set; }

        [DisplayName("Is Used")]
        public bool IsUsed { get; set; }

        //[IgnoreDataMember]
        public List<SelectListItem> Categories { get; set; }

        //[IgnoreDataMember]
        public List<SelectListItem> SubCategories { get; set; }
    }

    public class ProductComment
    {
        [Required]
        public long UserCommentId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, DisplayName("Name")]
        public string FullName { get; set; }

        [Required, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required, StringLength(4000, MinimumLength = 3)]
        public string Comment { get; set; }
    }

    public class ProductThumbModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class ProductListingModel
    {
        public ProductListingModel()
        {
            Thumbs = new ProductInfoModel();
        }

        [Required]
        public int ProductId { get; set; }

        [Required, DisplayName("Category")]
        public string MainProductCategory { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, DisplayName("Name")]
        public string FullName { get; set; }

        [Required, DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required, DisplayName("Mobile")]
        public string ContactNumber { get; set; }

        [Required, EmailAddress, DisplayName("Email")]
        public string Email { get; set; }

        public string Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, DisplayName("Date Added")]
        public DateTime Added { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        public ProductInfoModel Thumbs { get; set; }
    }

    public class ProductEditModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required, DisplayName("Product Name")]
        public string ProductName { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        [Required, DisplayName("Expiry Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ExpiryDate { get; set; }
    }

    public class ProductInfoModel
    {
        public int ProductId { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }
        public bool? YourResponse { get; set; }
        public int Comments { get; set; }
    }

    public class ProductScrollModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        public string Image { get; set; }
    }
}