namespace Interview.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BusinessPartnerBindingModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone number :)")]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

    }

}