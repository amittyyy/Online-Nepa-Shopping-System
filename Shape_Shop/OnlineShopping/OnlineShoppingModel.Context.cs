﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OnlineShoppingEntity : DbContext
    {
        public OnlineShoppingEntity()
            : base("name=OnlineShoppingEntity")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<UserComment> UserComments { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<ProductThumb> ProductThumbs { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}