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
    [SessionAuthorize]
    public class SecurityController : Controller
    {
        private ISecurityService securityServices;

        public ActionResult ManageSecurity(int page = 1, int PageSize = 5, string searchString = "", string sort = "",
            string sortdir = "DESC", string message = "")
        {
            securityServices = new SecurityService();
            ViewBag.sortdir = "ASC" == sortdir ? "DESC" : "ASC";
            sort = String.IsNullOrEmpty(sort) ? "BookingId" : sort;
            var securityDetail = securityServices.GetSecurityClaims();
            if (securityDetail != null)
            {
                switch (sort)
                {
                   
                    case "BookingId":
                        if (sortdir == "DESC")
                            securityDetail = securityDetail.OrderByDescending(tes => tes.BookingId);
                        else
                            securityDetail = securityDetail.OrderBy(tes => tes.BookingId);
                        break;

                    case "CreatedDate":
                        if (sortdir == "DESC")
                            securityDetail = securityDetail.OrderByDescending(tes => Convert.ToDateTime(tes.CreatedDate));
                        else
                            securityDetail = securityDetail.OrderBy(tes => Convert.ToDateTime(tes.CreatedDate));
                        break;

                    default:
                        securityDetail = securityDetail.OrderByDescending(tes => tes.BookingId);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    securityDetail = securityDetail.Where(p =>
                        p.BookingId != null).ToList();
                    securityDetail = securityDetail.Where(s =>
                        (s.BookingId.ToString().ToUpper().Contains(searchString.ToUpper())));
                }

                SecurityEntityDetail model = new SecurityEntityDetail();
                var securitylist = securityDetail.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (securitylist.Count > 0)
                {
                    model.lstSecurityClaims = securitylist;
                }
                else
                {
                    model.lstSecurityClaims = null;
                }

                ViewBag.Message = message;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double) securityDetail.Count() / PageSize);
                model.Total = securityDetail.Count();
                return View(model);
            }

            return View();
        }

        public ActionResult Capture(long id, int pagesize, int page)
        {
            this.securityServices = new SecurityService();
            if (!string.IsNullOrEmpty(securityServices.CaptureSecuirtyById(id)))
            {
                ViewBag.Message = "Capture";
            }
            else
            {
                ViewBag.Message = "Something went wrong";
            }
            return RedirectToAction("ManageSecurity", new { message = ViewBag.Message });
        }

        public ActionResult Release(long id, int pagesize, int page)
        {
            this.securityServices = new SecurityService();
            if (!string.IsNullOrEmpty(securityServices.ReleaseSecuirtyById(id)))
            {
                ViewBag.Message = "Release";
            }
            else
            {
                ViewBag.Message = "Something went wrong";
            }
            return RedirectToAction("ManageSecurity", new { message = ViewBag.Message });
        }
    }
}
        