namespace Interview.BusinessLayer
{
    using Data.Repositories;
    using Interview.Data.Models;
    public interface IEmployeeService:IService, IRepository<Employee>
    {
        /// <summary>
        /// Checks if an employee could become an supervisor to another one
        /// </summary>
        /// <param name="emp1"></param>
        /// <param name="emp2"></param>
        /// <returns>bool</returns>
        bool checkIfCanBeSupervisor(Employee emp1, Employee emp2);
        /// <summary>
        /// Change Supervisor status to all entities that relate that employee to null
        /// It happens on deleting Employee
        /// </summary>
        /// <param name="entity"></param>
        void ChangeSupervisorStatusToEntitiesThatRelateThatEmployee(Employee entity);

    }
}
