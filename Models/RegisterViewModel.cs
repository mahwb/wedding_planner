using System;
using System.ComponentModel.DataAnnotations;

namespace wedding_planner.Models
{
    public class RegisterViewModel: BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage="Must be alphabetical.")]
        [Display(Name = "First Name")]      
        public string FirstName {get; set;}

        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage="Must be alphabetical.")]
        [Display(Name = "Last Name")]                            
        public string LastName {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]        
        public string Confirm {get; set;}
    }
}