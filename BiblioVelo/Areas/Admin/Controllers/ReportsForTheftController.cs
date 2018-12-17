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
    public class ReportsForTheftController : Controller
    {
        // GET: Admin/ReportsForTheft
        private ITheftClaimServices theftClaim;

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(int page = 1, int PageSize = 5, string searchString = "", string sort = "",
            string sortdir = "DESC", string message = "")
        {
            theftClaim = new TheftClaimServices();
            ViewBag.sortdir = "ASC" == sortdir ? "DESC" : "ASC";
            sort = String.IsNullOrEmpty(sort) ? "CreatedDate" : sort;
            var theftClaimList = theftClaim.GetTheftReports();
            if (theftClaimList != null)
            {
                switch (sort)
                {
                    case "FullName":
                        if (sortdir == "DESC")
                            theftClaimList = theftClaimList.OrderByDescending(tes => tes.FullName);
                        else
                            theftClaimList = theftClaimList.OrderBy(tes => tes.FullName);
                        break;
                    case "BookingId":
                        if (sortdir == "DESC")
                            theftClaimList = theftClaimList.OrderByDescending(tes => tes.BookingId);
                        else
                            theftClaimList = theftClaimList.OrderBy(tes => tes.BookingId);
                        break;
                    case "Address":
                        if (sortdir == "DESC")
                            theftClaimList = theftClaimList.OrderByDescending(tes => tes.Address);
                        else
                            theftClaimList = theftClaimList.OrderBy(tes => tes.Address);
                        break;
                    case "Email":
                        if (sortdir == "DESC")
                            theftClaimList = theftClaimList.OrderByDescending(tes => tes.Email);
                        else
                            theftClaimList = theftClaimList.OrderBy(tes => tes.Email);
                        break;
                    case "CreatedDate":
                        if (sortdir == "DESC")
                            theftClaimList = theftClaimList.OrderByDescending(tes => tes.CreatedDate);
                        else
                            theftClaimList = theftClaimList.OrderBy(tes => tes.CreatedDate);
                        break;
                    default:
                        theftClaimList = theftClaimList.OrderByDescending(tes => tes.CreatedDate);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    theftClaimList = theftClaimList.Where(p => p.BookingId != null && p.FullName != null).ToList();
                    theftClaimList = theftClaimList.Where(s => (s.BookingId.ToUpper().Contains(searchString.ToUpper()) || (s.FullName.ToUpper().Contains(searchString.ToUpper()))));
                }
                TheftClaimModelEntity model = new TheftClaimModelEntity();
                var userlist = theftClaimList.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (userlist.Count > 0)
                {
                    model.lstTheftReports = userlist;
                }
                else
                {
                    model.lstTheftReports = null;
                }
                ViewBag.Message = message;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double)theftClaimList.Count() / PageSize);
                model.Total = theftClaimList.Count();
                return View(model);
            }
            return View();
        }

        public ActionResult Reject(long id, int pagesize, int page)
        {
            this.theftClaim = (ITheftClaimServices)new TheftClaimServices();
            if (!string.IsNullOrEmpty(this.theftClaim.RejectTheftReportById(id)))
            {
                ViewBag.Message = "Deleted";
            }
            else
            {
                ViewBag.Message = "Something went wrong";
            }

            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        public ActionResult Approve(long id, int pagesize, int page)
        {
            this.theftClaim = (ITheftClaimServices)new TheftClaimServices();
            string Message = string.Empty;
            if (!string.IsNullOrEmpty(this.theftClaim.ApproveTheftReportById(id)))
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