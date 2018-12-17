using BiblioVelo.Services.Models;
using BiblioVelo.Services.Services;
using BiblioVelo.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiblioVelo.Areas.Admin.Controllers
{
    public class AccidentalClaimController : Controller
    {
        IAccidentalService _accidentalServices;
        // GET: Admin/AccidentalClaim
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind()] AccidentalClaimModel accidentalModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _accidentalServices = new AccidentalService();

            string accidentalId = _accidentalServices.AddAccidentalClaim(accidentalModel).ToString();

                if (!string.IsNullOrEmpty(accidentalId))
                {
                    ViewBag.Message = "Success";

                }
                else 
                {
                    ViewBag.Message = "Something went wrong";

                }
                
                return RedirectToAction("Index", new { message = ViewBag.Message });
            
            

        }
    }
}