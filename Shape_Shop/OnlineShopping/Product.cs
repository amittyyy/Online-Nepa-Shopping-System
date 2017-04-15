//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShopping
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.ProductThumbs = new HashSet<ProductThumb>();
            this.UserComments = new HashSet<UserComment>();
            this.Notifications = new HashSet<Notification>();
        }
    
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public System.DateTime Added { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public int ProductCategoryId { get; set; }
    
        public virtual UserLogin UserLogin { get; set; }
        public virtual ICollection<ProductThumb> ProductThumbs { get; set; }
        public virtual ICollection<UserComment> UserComments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
