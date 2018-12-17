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
    public class ClaimsForAccidentController : Controller
    {
        private IAccidentalService accidentalClaim;

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(int page = 1, int PageSize = 5, string searchString = "", string sort = "",
            string sortdir = "DESC", string message = "")
        {
            accidentalClaim = new AccidentalService();
            ViewBag.sortdir = "ASC" == sortdir ? "DESC" : "ASC";
            sort = String.IsNullOrEmpty(sort) ? "CreatedDate" : sort;
            var accidentalClaimList = accidentalClaim.GetAccidentalClaims();
            if (accidentalClaimList != null)
            {
                switch (sort)
                {
                    case "FullName":
                        if (sortdir == "DESC")
                            accidentalClaimList = accidentalClaimList.OrderByDescending(tes => tes.FullName);
                        else
                            accidentalClaimList = accidentalClaimList.OrderBy(tes => tes.FullName);
                        break;
                    case "BookingId":
                        if (sortdir == "DESC")
                            accidentalClaimList = accidentalClaimList.OrderByDescending(tes => tes.BookingId);
                        else
                            accidentalClaimList = accidentalClaimList.OrderBy(tes => tes.BookingId);
                        break;
                    case "Address":
                        if (sortdir == "DESC")
                            accidentalClaimList = accidentalClaimList.OrderByDescending(tes => tes.Address);
                        else
                            accidentalClaimList = accidentalClaimList.OrderBy(tes => tes.Address);
                        break;
                    case "Email":
                        if (sortdir == "DESC")
                            accidentalClaimList = accidentalClaimList.OrderByDescending(tes => tes.Email);
                        else
                            accidentalClaimList = accidentalClaimList.OrderBy(tes => tes.Email);
                        break;
                    case "CreatedDate":
                        if (sortdir == "DESC")
                            accidentalClaimList = accidentalClaimList.OrderByDescending(tes => tes.CreatedDate);
                        else
                            accidentalClaimList = accidentalClaimList.OrderBy(tes => tes.CreatedDate);
                        break;
                    default:
                        accidentalClaimList = accidentalClaimList.OrderByDescending(tes => tes.CreatedDate);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    accidentalClaimList = accidentalClaimList.Where(p => p.BookingId != null && p.FullName != null ).ToList();
                    accidentalClaimList = accidentalClaimList.Where(s => (s.BookingId.ToUpper().Contains(searchString.ToUpper()) || (s.FullName.ToUpper().Contains(searchString.ToUpper()) )));
                }
                AccidentalClaimModelEntity model = new AccidentalClaimModelEntity();
                var userlist = accidentalClaimList.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (userlist.Count > 0)
                {
                    model.lstAccidentalClaims = userlist;
                }
                else
                {
                    model.lstAccidentalClaims = null;
                }
                ViewBag.Message = message;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double)accidentalClaimList.Count() / PageSize);
                model.Total = accidentalClaimList.Count();
                return View(model);
            }
            return View();
        }

        public ActionResult Reject(long id, int pagesize, int page)
        {
            this.accidentalClaim = (IAccidentalService) new AccidentalService();
            if (!string.IsNullOrEmpty(this.accidentalClaim.RejectAccidentalClaimById(id)))
            {
                ViewBag.Message = "Deleted";
            }
            else
            {
                ViewBag.Message = "Something went wrong";
            }

            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        public  ActionResult Approve(long id, int pagesize, int page)
        {
            this.accidentalClaim = (IAccidentalService) new AccidentalService();
            string Message = string.Empty;
            if (!string.IsNullOrEmpty(this.accidentalClaim.ApproveAccidentalClaimById(id)))
            {
                ViewBag.Message = "Approved";
            }
            else
            {
                ViewBag.Message = "Something went wrong";
            }
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }
    }
}