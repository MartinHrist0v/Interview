namespace Interview.Web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class EmplyeeBindingModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        [Display(Name= "Supervisor Name")]
        public string SupervisorName { get; set; }

        [Display(Name = "Business Partner")]
        public string BusinessPartner { get; set; }
    }

}