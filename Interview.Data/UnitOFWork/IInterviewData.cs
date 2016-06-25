namespace Interview.Data.UnitOFWork
{
    using Interview.Data.Models;
    using Interview.Data.Repositories;

    public interface IInterviewData
    {
        IRepository<Employee> Employees { get; }

        IRepository<BusinessPartner> BusinessPartners { get; }

        void SaveChanges();
    }
}
