using BiblioVelo.BusinessLogic.Services;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Services;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiblioVelo.Areas.Admin.Controllers
{
    [SessionAuthorize]
    public class FeeAdjustController : Controller
    {
        private IFeeAdjustService feeAdjust;

        public ActionResult Index(string message="")
        {
            this.feeAdjust = new FeeAdjustService();
            FeeAdjustModel feePercentage = this.feeAdjust.GetFeePercentage();
            ViewBag.Message = message;
            return View(feePercentage);
        }

        public ActionResult EditFee()
        {
            feeAdjust = new FeeAdjustService();
            return View(feeAdjust.GetFeePercentage());
        }

        [HttpPost]
        [ActionName("EditFee")]
        [ValidateAntiForgeryToken]
        public ActionResult EditFeeConfirmed([Bind(Include = "Id,InsuranceFor1Day,InsuranceFor2to3Days,InsuranceFor4to6Days,InsuranceFor7to10Days,InsuranceForMoreThan10Days,BookingFeeRenter,BookingFeeOwner")] FeeAdjustModel feeModel)
        {
            this.feeAdjust = new FeeAdjustService();
            string str = feeAdjust.EditFeeConfirmed(feeModel);
            if (string.IsNullOrEmpty(str))
                return View();
            if (str == "Success")
            {
                ViewBag.Message = "Success";
            }
            else
            {
                ViewBag.Message = "Exception";
            }


            return RedirectToAction("Index", new { message = ViewBag.Message });
        }
    }
}