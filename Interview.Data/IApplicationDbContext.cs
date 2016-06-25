namespace Interview.Data
{
    using System.Data.Entity;
    using Models;
    public interface IApplicationDbContext
    {
        IDbSet<BusinessPartner> BusinessPartners { get; set; }

        IDbSet<Employee> Employees { get; set; }

        void SaveChanges();

    }
}
