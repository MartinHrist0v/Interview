namespace Interview.BusinessLayer
{
    using Data.Models;
    using Data;
    using Data.Repositories;
    using System.Linq;
    using System.Collections.Generic;
    /// <summary>
    /// All services related with Employee 
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private ApplicationDbContext context = new ApplicationDbContext();
        public EmployeeService() {
            _employeeRepository = new GenericEfRepository<Employee>(context);
        }
        /// <summary>
        /// Create / Add a new employee
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Employee entity)
        {
            _employeeRepository.Add(entity);

            _employeeRepository.SaveChanges();
        }
        /// <summary>
        /// Take all employees
        /// </summary>
        /// <returns>IEnumerable of Employee</returns>
        public IEnumerable<Employee> All()
        {
            return _employeeRepository.All();
        }

        public Employee Find(object id)
        {
            return _employeeRepository.Find(id);
        }
        /// <summary>
        /// Remove / Delete Employee by given id
        /// </summary>
        /// <param name="id">id</param>
        public void Remove(object id)
        {
            var partner = _employeeRepository.Find(id);

            ChangeSupervisorStatusToEntitiesThatRelateThatEmployee(partner);

            _employeeRepository.Remove(partner);
            _employeeRepository.SaveChanges();
        }
        /// <summary>
        ///Remove/Delete Employee by given the whole object
        /// </summary>
        /// <param name="entity">Employee object</param>
        public void Remove(Employee entity)
        {
            var partner = _employeeRepository.Find(entity.Id);

            ChangeSupervisorStatusToEntitiesThatRelateThatEmployee(partner);

            _employeeRepository.Remove(partner);
            _employeeRepository.SaveChanges();
        }

        public void SaveChanges()
        {
            _employeeRepository.SaveChanges();
        }
        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Employee entity)
        {
            var changeElement = _employeeRepository.Find(entity.Id);

            changeElement.BusinessPartner = entity.BusinessPartner;
            changeElement.BusinessPartnerId = entity.BusinessPartnerId;
            changeElement.Id = entity.Id;
            changeElement.Name = entity.Name;
            changeElement.Position = entity.Position;
            changeElement.Supervisor = entity.Supervisor;

            changeElement.SupervisorId = entity.SupervisorId;
            _employeeRepository.SaveChanges();
        }
        /// <summary>
        /// This prevent if two employee want to become supervisors to each other
        ///or any employee try to become supervisor to himself
        /// </summary>
        /// <param name="emp1"></param>
        /// <param name="emp2"></param>
        /// <returns></returns>
        public bool checkIfCanBeSupervisor(Employee emp1, Employee emp2)
        {   
            if ((emp1.Id == emp2.SupervisorId && emp2.Id==emp1.SupervisorId) || 
                (emp1.Id==emp1.SupervisorId) || (emp2.Id ==emp2.SupervisorId))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// On deleting an employee, change the status of all entities that relate that employee
        /// Change their supervisor cell to null
        /// </summary>
        /// <param name="entity">Employee that is about to delete</param>
        public void ChangeSupervisorStatusToEntitiesThatRelateThatEmployee(Employee entity)
        {
            var allEmployees = _employeeRepository.All().Where(i => i.SupervisorId == entity.Id);
            if (allEmployees.Any())
            {
                foreach (var item in allEmployees)
                {
                    item.SupervisorId = null;
                    item.Supervisor = null;
                }
                _employeeRepository.SaveChanges();
            }
        }
    }
}
