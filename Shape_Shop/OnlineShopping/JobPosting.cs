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
    
    public partial class JobPosting
    {
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public int JobCategoryId { get; set; }
        public string Salary { get; set; }
        public string Experience { get; set; }
        public Nullable<byte> Openings { get; set; }
        public string Education { get; set; }
        public string City { get; set; }
        public string Requirements { get; set; }
        public string Specifications { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime Added { get; set; }
    
        public virtual JobCategory JobCategory { get; set; }
        public virtual UserLogin UserLogin { get; set; }
    }
}
