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
    
    public partial class JobCategory
    {
        public JobCategory()
        {
            this.JobPostings = new HashSet<JobPosting>();
        }
    
        public int JobCategoryId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<JobPosting> JobPostings { get; set; }
    }
}
