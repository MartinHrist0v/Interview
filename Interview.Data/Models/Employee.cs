namespace Interview.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        public int? SupervisorId { get; set; }

        public virtual Employee Supervisor { get; set; }

        public int? BusinessPartnerId { get; set; }

        public virtual BusinessPartner BusinessPartner { get; set; }

    }
}
