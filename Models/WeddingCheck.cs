using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomDataAnnotations;

namespace wedding_planner.Models
{
    public class WeddingCheck: BaseEntity
    {
        [Required]
        [MinLength(2)]
        [Display(Name = "Wedder One")]
        public string WedderOne {get; set;}
        [Required]
        [MinLength(2)]
        [Display(Name = "Wedder Two")]
        public string WedderTwo {get; set;}
        [Required]
        [CurrentDate(ErrorMessage = "Date needs to be current or after date.")]
        [DataType(DataType.DateTime)]
        public DateTime Date {get; set;}
        [Required]
        public string Address {get; set;}
    }
}