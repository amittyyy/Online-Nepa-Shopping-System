using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class UserDisplayModel
    {
        [Required]
        public int UserId { get; set; }

        [Required, DisplayName("Name")]
        public string FullName { get; set; }

        [Required, DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        [Required, EmailAddress, DisplayName("Email")]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required, Display(Name = "Joined"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [DisplayName("Total Products Posted")]
        public int TotalProducts { get; set; }

        [DisplayName("Total Likes")]
        public int TotalLikes { get; set; }

        [DisplayName("Total Dislikes")]
        public int TotalDislikes { get; set; }

        [DisplayName("Total Job Postings")]
        public int TotalJobPostings { get; set; }
    }

    public class UserEditModel
    {
        [Required, DisplayName("Name")]
        public string FullName { get; set; }

        [Required, DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }
}