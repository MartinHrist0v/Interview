namespace Interview.Web.Models
{
    using System.Collections.Generic;

    public class EmployeesGlobalViewModel
    {
        public EmplyeeBindingModel AddEmployee { get; set; }

        public ICollection<EmployeeDetailsViewModel> EmployeesDetails { get; set; }
    }
}