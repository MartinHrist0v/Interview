namespace Interview.BusinessLayer
{
    using Interfaces;
    using Data.Models;
    using Data;
    using Data.Repositories;
    using System.Linq;
    using System.Collections.Generic;
    /// <summary>
    /// All Services about related with Buiseness Partner
    /// </summary>
    public class BusinessPartnerService : IBusinessPartnerService
    {
        private readonly IRepository<BusinessPartner> _partnerRepository;
        private ApplicationDbContext context = new ApplicationDbContext();
        public BusinessPartnerService()
        {
            _partnerRepository = new GenericEfRepository<BusinessPartner>(context);

        }
        /// <summary>
        /// Create / Add a new Business Partner
        /// </summary>
        /// <param name="entity"></param>
        public void Add(BusinessPartner entity)
        {
            _partnerRepository.Add(entity);
            _partnerRepository.SaveChanges();
        }
        /// <summary>
        /// Take all Business Partners 
        /// </summary>
        /// <returns>IEnumerable of Business Partner</returns>
        public IEnumerable<BusinessPartner> All()
        {
            return _partnerRepository.All();
        }
        /// <summary>
        /// Find an Business Partner by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Business Partner</returns>
        public BusinessPartner Find(object id)
        {
            return _partnerRepository.Find(id);
        }
        /// <summary>
        /// Delete an Business Partner by given Id
        /// </summary>
        /// <param name="id">Id</param>
        public void Remove(object id)
        {
            var partner = _partnerRepository.Find(id);
            _partnerRepository.Remove(partner);
            _partnerRepository.SaveChanges();
        }
        /// <summary>
        /// Delete / Remove an Business Partner by given the whole object
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(BusinessPartner entity)
        {
            var partner = _partnerRepository.Find(entity.Id);
            CheckIfAnyPartnerIsRelatedWithEmployee(entity);
            _partnerRepository.Remove(partner);
            _partnerRepository.SaveChanges();
        }

        public void SaveChanges()
        {
            _partnerRepository.SaveChanges();
        }
        /// <summary>
        /// Update/Change an Business Partner
        /// </summary>
        /// <param name="entity"></param>
        public void Update(BusinessPartner entity)
        {
            var changeElement= _partnerRepository.Find(entity.Id);
            changeElement.Email = entity.Email;
            changeElement.Name = entity.Name;
            changeElement.PhoneNumber = entity.PhoneNumber;
            changeElement.Id = entity.Id;
            _partnerRepository.SaveChanges();
        }
        /// <summary>
        /// Check if partner has any relations with Employee. If has -> change the BuissinessPartner column of all Employees to null
        /// </summary>
        /// <param name="partner"></param>
        public void CheckIfAnyPartnerIsRelatedWithEmployee(BusinessPartner partner)
        {
            var emp = new EmployeeService();
            var allrealations = emp.All().Where(e => e.BusinessPartnerId == partner.Id);
            if (allrealations.Any())
            {
                foreach (var item in allrealations)
                {
                    item.BusinessPartner = null;
                    item.BusinessPartnerId = null;
                }
                emp.SaveChanges();
            }
            
        }
    }
}
