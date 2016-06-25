namespace Interview.Web.Controllers
{
    using Interview.BusinessLayer;
    using Interview.BusinessLayer.Interfaces;
    using Interview.Data.Models;
    using Interview.Web.Models;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class EmployeeController : Controller
    {
        #region private fields
        private readonly IEmployeeService _employee;
        private readonly IBusinessPartnerService _businessPartner;
        #endregion
        #region Constructors
        public EmployeeController(IEmployeeService employee, IBusinessPartnerService businessPartner)
        {
            this._employee = employee;
            this._businessPartner = businessPartner;
        }
        #endregion
        /// <summary>
        /// Get Method | Route: /Employee | View of all employee and input for create a new one
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //Take all entities from db and project them into view model
            var employees = _employee.All().Select(e => new EmployeeDetailsViewModel()
            {
                BusinessPartnerId = e.BusinessPartnerId,
                Id = e.Id,
                Name = e.Name,
                Position = e.Position,
                SupervisorId = e.SupervisorId,
                SupervisorName = e.Supervisor?.Name,
                BusinessPartnerName = e.BusinessPartner?.Name,
            }).OrderByDescending(e => e.Id)
              .ToList();

            var addEmployeeFields = new EmplyeeBindingModel();

            var model = new EmployeesGlobalViewModel() { AddEmployee = addEmployeeFields, EmployeesDetails = employees };

            return View(model);
        }
        /// <summary>
        /// Post Method | Route: /Employee/Add | Create/Add a new entity in db
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(EmplyeeBindingModel emp)
        {
            //check if model is valid, if not -> redirect to Index with error message
            if (!ModelState.IsValid)
            {
                TempData["class"] = "alert-danger";
                TempData["Response"] = "Unable to add new Employee ";
                return RedirectToAction("Index");
            }

            //Check if an entity with current name is existing
            if (_employee.All().Any(i=>i.Name == emp.Name))
            {
                TempData["class"] = "alert-danger";
                TempData["Response"] = "Bad Request! Unvalid Input Username should be unique!";
                return RedirectToAction("Index");
            }

            var employee = new Employee()
            {
                Name = emp.Name,
                Position = emp.Position,
            };

            //check if business partner field is empty
            if (emp.BusinessPartner != null)
            {
                //take business partner from db by given name
                var empBusnessPartner = _businessPartner.All().FirstOrDefault(i => i.Name == emp.BusinessPartner);

                //check if exists
                if (empBusnessPartner == null)
                {
                    TempData["class"] = "alert-danger";
                    TempData["Response"] = "Bad Request! That Business Partner does not exist ! Tip : Check your input twice";
                    return RedirectToAction("Index");

                }
                employee.BusinessPartner = empBusnessPartner;
                employee.BusinessPartnerId = empBusnessPartner.Id;
            }
            //Check if supervisor field is empty
            if (emp.SupervisorName != null)
            {
                //Check if supervisor and Name is equal
                var supervisor = _employee.All().FirstOrDefault(i => i.Name == emp.SupervisorName);
                if (emp.SupervisorName == emp.Name)
                {
                    TempData["class"] = "alert-danger";
                    TempData["Response"] = "Bad Request! You can not select the same Supervisor as the current user";
                    return RedirectToAction("Index");
                }
                //Check if supervisor is an existing employee
                if (supervisor == null)
                {
                    TempData["class"] = "alert-danger";
                    TempData["Response"] = "Bad Request! Invalid Supervisor's name ! It should be an existing Employee";
                    return RedirectToAction("Index");
                }

                if (_employee.checkIfCanBeSupervisor(employee, supervisor))
                {
                    TempData["class"] = "alert-danger";
                    TempData["Response"] = "Unable to add new Employee ";
                    return RedirectToAction("Index");
                }

                employee.Supervisor = supervisor;
                employee.SupervisorId = supervisor.Id;

            }
            _employee.Add(employee);
            return RedirectToRoute("Employee");
        }

        /// <summary>
        /// Get Method | Route: /Employee/Add  | Retruns View with input fields
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            return View("_Add");
        }
        /// <summary>
        /// Get Method | Route : /Employee/Details/{id} Take detail view of employee by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details(int id)
        {
            //withdraw an entity from db
            var emp = _employee.Find(id);
            //check if exists
            if (emp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request ! Not such Employee exists ");
            }
            //project to view model
            var model = new EmployeeDetailsViewModel()
            {
                Name = emp.Name,
                Id = emp.Id,
                Position = emp.Position,
                SupervisorName = emp.Supervisor?.Name,
                SupervisorId = emp?.SupervisorId,
                BusinessPartnerId = emp?.BusinessPartnerId,
                BusinessPartnerName = emp.BusinessPartner?.Name,

            };
            return View(model);
        }

        /// <summary>
        /// Delete /Remove an employee by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var emp = _employee.Find(id);
            _employee.Remove(emp);
            return RedirectToAction("Index");

        }

        /// <summary>
        /// Get Method | Route: /Employee/Edit/{id} Returns a view with edit fields for entity withc current id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //withdraw entity from db
            var employee = _employee.Find(id);

            //if entity exists
            if (employee == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request. Invalid Employee");
            }
            //project the entity to view model
            var model = new EmployeeViewModel()
            {
                BusinessPartner = employee.BusinessPartner?.Name,
                Id = employee.Id,
                Name = employee.Name,
                Position = employee.Position,
                SupervisorName = employee.Supervisor?.Name,
            };
            return View(model);
        }

        /// <summary>
        /// Post Method | Route: /Employee/Edit/{model}  Edits an existing entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmplyeeBindingModel model) {
            //check if model state is valid -> all required field are correctly fulfilled 
            if (!this.ModelState.IsValid)
            {
                TempData["Response"] = "Unable to Save Changes. Try again !";
                TempData["class"] = "alert-danger";
                return RedirectToAction("Edit", new { id = model.Id });
            }
            
            var partner = new Employee()
            {
                Name = model.Name,
                Position = model.Position,
                Id = model.Id,
            };

            if (_employee.All().Any(i => i.Name == model.Name && i.Id!=model.Id))
            {
                TempData["class"] = "alert-danger";
                TempData["Response"] = "Bad Request! Unvalid Input Username should be unique!";
                return RedirectToAction("Edit", new { id = model.Id});
            }
            //check if Business Partner Field is empty
            if (!string.IsNullOrEmpty(model.BusinessPartner))
            {
                //check if Business Partner is existing
                var findBusinessPartner = _businessPartner.All().FirstOrDefault(b => b.Name == model.BusinessPartner);
                if (findBusinessPartner == null)
                {
                    TempData["Response"] = "Invalid business partner. Business partner is not existing.  !";
                    TempData["class"] = "alert-danger";
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                partner.BusinessPartner = findBusinessPartner;
                partner.BusinessPartnerId = findBusinessPartner.Id;
            }
            //check if Supervisor Name is not empty
            if (!string.IsNullOrEmpty(model.SupervisorName))
            {
                if (model.SupervisorName == model.Name)
                {
                    TempData["Response"] = "You can not select the same supervisor name as employee's name";
                    TempData["class"] = "alert-danger";
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                var supervisor = _employee.All().FirstOrDefault(e => e.Name == model.SupervisorName);
                //check if supervisor is an existing employee
                if (supervisor == null)
                {
                    TempData["Response"] = "Invalid supervisor name. Supervisor must be an existing Employee  !";
                    TempData["class"] = "alert-danger";
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                partner.Supervisor = supervisor;
                partner.SupervisorId = supervisor.Id;
            }
            _employee.Update(partner);
            TempData["class"] = "alert-success";
            TempData["Response"] = "Successfully updated that entity!";
            return RedirectToAction("Index", new { id = model.Id });

        }
        
    }
}
