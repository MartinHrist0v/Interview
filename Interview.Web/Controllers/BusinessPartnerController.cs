namespace Interview.Web.Controllers
{
    using Interview.Data.Models;
    using Interview.Web.Models;
    using System.Linq;
    using System.Web.Mvc;
    using Interview.BusinessLayer;
    using Interview.BusinessLayer.Interfaces;
    using System.Net;


    public class BusinessPartnerController : Controller
    {
        #region private fields
        private readonly IBusinessPartnerService _bussinessPartner;
        private readonly IEmployeeService _employee;
        #endregion
        #region Constructors
        public BusinessPartnerController(IBusinessPartnerService bussinessPartner, IEmployeeService employee)
        {
            this._employee = employee;
            this._bussinessPartner = bussinessPartner;
        }
        #endregion
        #region Controller actions
        /// <summary>
        ///  Get Method | Route: /BusinessPartner | View of all business partner and input for create a new one
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Take all entities from db and project them into view model
            var allPartners = _bussinessPartner.All().Select(i => new BusinessPartnerDetailsView {
                Email = i.Email,
                Id = i.Id,
                Name = i.Name,
                PhoneNumber = i.PhoneNumber,
            }).OrderByDescending(i=>i.Id).ToList();
            var addbox = new BusinessPartnerBindingModel();
            var model = new BusinessPartnerGlobalView() { AddBusinessPartner= addbox,PartnerDetails =allPartners};
            return View(model);
        }

        /// <summary>
        /// Post Method | Route /BusinessPartner/Add  | Bind all inputs and create a new entity, 
        /// All TempData[] is for returning a messages 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(BusinessPartnerBindingModel model)
        {
            //check if model is valid, if not retrn to Index and dispay error message
            if (!this.ModelState.IsValid)
            {
                TempData["class"] = "alert-danger";
                TempData["Response-bad"] = "Unable to add new Business Partner ";
                return RedirectToAction("Index");
            }

            //check if entity with current name exists
            if (_bussinessPartner.All().Any(i => i.Name == model.Name))
            {
                TempData["class"] = "alert-danger";
                TempData["Response"] = "Bad Request! Unvalid Business Partner should be unique!";
                return RedirectToAction("Index");
            }
            //project to db model and create it
            var partner = new BusinessPartner()
            {
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
            };

            _bussinessPartner.Add(partner);

            return RedirectToRoute("BusinessPartner");
        }

        /// <summary>
        /// Get Method | Route /BusinessPartner/Add | Display inputs for creating a new business partner
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// Get Method | Route /BusinessPartner/Edit/{id} | Displays inputs for editing an entity by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = _bussinessPartner.Find(id);
            //check if entity exists in db
            if (entity==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }
            //project the entity in view model
            var model = new BusinessPartnerDetailsView()
            {
                Email = entity.Email,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                Id = entity.Id               
            };

            return View(model);
        }

        /// <summary>
        /// Post Method | Route /BusinessPartner/Edit/model 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (BusinessPartnerBindingModel model)
        {
            //check if model state is valid, if not redirect to Edit/{id} + returns a message
            if (!this.ModelState.IsValid)
            {
                TempData["Response"] = "Unable to Save Changes. Try again !";
                TempData["class"] = "alert-danger";
                return RedirectToAction("Edit", new { id = model.Id });
            }

            //check if an entity with a inputed name is existing 
            if (_bussinessPartner.All().Any(i=>i.Name==model.Name))
            {
                TempData["class"] = "alert-danger";
                TempData["Response"] = "Bad Request! Unvalid Input Username should be unique!";
                return RedirectToAction("Index");
            }

            // project a new entity with given information
            var partner = new BusinessPartner()
            {
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Id =model.Id
            };

            //update that entity in DB
            _bussinessPartner.Update(partner);
            TempData["class"] = "alert-success";
            TempData["Response"] = "Successfully updated that entity!";

            return RedirectToAction("Edit", new { id = model.Id });
        }
        /// <summary>
        /// Get Method | Route /BusinessPartner/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details (int id)
        {

            var entity =_bussinessPartner.Find(id);

            //check if entity exists
            if (entity ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }

            //project the entity in view model
            var model = new BusinessPartnerDetailsView()
            {
                Name = entity.Name,
                Email = entity.Email,
                Id = entity.Id,
                PhoneNumber = entity.PhoneNumber
            };
            return View(model);
        }

        /// <summary>
        /// Post Method | Delete an entity by given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePartner(int id)
        {
            var entity = _bussinessPartner.Find(id);
            _bussinessPartner.Remove(entity);
            return RedirectToAction("Index");
        }
        #endregion
    }
}