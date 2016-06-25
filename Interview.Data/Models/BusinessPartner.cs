namespace Interview.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BusinessPartner
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

    }
}
