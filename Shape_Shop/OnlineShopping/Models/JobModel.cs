using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineShopping.Models
{
    public class JobModel
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Display(Name = "Name")]
        public string UserName { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required, DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Salary")]
        public string Salary { get; set; }

        public string Experience { get; set; }

        public byte? Openings { get; set; }

        public string Education { get; set; }

        public string City { get; set; }

        public string Contact { get; set; }

        [EmailAddress, DisplayName("Email")]
        public string Email { get; set; }

        public string Url { get; set; }

        [Required, DisplayName("Posted Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Posted { get; set; }

        [DisplayName("Last Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LastDate { get; set; }

        public string Requirements { get; set; }

        public string Specifications { get; set; }

        [Required, DisplayName("Category")]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; }
    }

    public class JobEditModel
    {
        [Required]
        public int JobId { get; set; }

        [Required, DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required, DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Salary")]
        public string Salary { get; set; }

        public string Experience { get; set; }

        public byte? Openings { get; set; }

        public string Education { get; set; }

        public string City { get; set; }

        public string Contact { get; set; }

        [EmailAddress, DisplayName("Email")]
        public string Email { get; set; }

        public string Url { get; set; }

        [Required, DisplayName("Last Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime LastDate { get; set; }

        public string Requirements { get; set; }

        public string Specifications { get; set; }
    }
}