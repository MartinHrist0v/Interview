namespace Interview.Data.UnitOFWork
{
    using System;
    using System.Collections.Generic;

    using Interview.Data.Models;
    using Interview.Data.Repositories;
    using System.Data.Entity;
    using Interview.Data;
    public class InterviewData : IInterviewData
    {
        private readonly DbContext dbContext;
        private readonly IDictionary<Type, object> repositories;
        public InterviewData()
            
        {
            
        }


        public InterviewData(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<BusinessPartner> BusinessPartners
        {
            get
            {
                return this.GetRepository<BusinessPartner>();
            }

        }

        public IRepository<Employee> Employees
        {
            get
            {
                return this.GetRepository<Employee>();
            }
        }


        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }
        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericEfRepository<T>);
                this.repositories.Add(
                    typeof(T),
                    Activator.CreateInstance(type, this.dbContext));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
