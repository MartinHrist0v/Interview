namespace Interview.Web.Models
{
    public class EmployeeDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public int? SupervisorId { get; set; }

        public string SupervisorName { get; set; }

        public int? BusinessPartnerId { get; set; }

        public string BusinessPartnerName { get; set; }
    }
}