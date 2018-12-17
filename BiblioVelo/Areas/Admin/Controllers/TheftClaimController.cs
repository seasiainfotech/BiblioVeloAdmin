using BiblioVelo.BusinessLogic.Services;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiblioVelo.Areas.Admin.Controllers
{
    public class TheftClaimController : Controller
    {
        private ITheftClaimServices theftClaim;
        // GET: Admin/TheftClaim
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TheftClaimModel model)
        {
            if (!this.ModelState.IsValid)
                return (ActionResult)this.View();
            this.theftClaim = (ITheftClaimServices)new TheftClaimServices();
            return (ActionResult)this.RedirectToAction(nameof(Index), (object)new
            {
                message = this.theftClaim.SaveTheftClaim(model)
            });
        }
    }
}