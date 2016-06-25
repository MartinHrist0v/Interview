namespace Interview.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        [Display(Name = "Supervisor Name")]
        public string SupervisorName { get; set; }

        [Display(Name = "Business Partner")]
        public string BusinessPartner { get; set; }
    }
}