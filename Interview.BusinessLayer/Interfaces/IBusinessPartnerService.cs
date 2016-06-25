namespace Interview.BusinessLayer.Interfaces
{
    using Interview.Data.Models;
    using Interview.Data.Repositories;
    public interface IBusinessPartnerService : IService, IRepository<BusinessPartner>
    {     
        /// <summary>
        /// Check if partner has any relations with Employee. If has -> change the BuissinessPartner column of all Employees
        /// </summary>
        /// <param name="partner"></param>
        void CheckIfAnyPartnerIsRelatedWithEmployee(BusinessPartner partner);

    }
}
