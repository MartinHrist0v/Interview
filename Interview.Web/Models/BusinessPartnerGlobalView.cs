namespace Interview.Web.Models
{
    using System.Collections.Generic;
    public class BusinessPartnerGlobalView
    {
        public BusinessPartnerBindingModel AddBusinessPartner { get; set; }

        public ICollection<BusinessPartnerDetailsView> PartnerDetails { get; set; }
    }
}