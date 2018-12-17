using BiblioVelo.BusinessLogic.Services;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BiblioVelo.Areas.Admin.Controllers
{
    [SessionAuthorize]
    public class ReportsController : Controller
    {
        // GET: Admin/Reports

        IReportsService _reports;
        /// <summary>
        /// ManageReviews
        /// </summary>
        /// <param name="page"></param>
        /// <param name="PageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="sort"></param>
        /// <param name="sortdir"></param>
        /// <returns></returns>
        public ActionResult ManageReviews(int page = 1, int PageSize = 5, string searchString = "", string sort = "", string sortdir = "ASC")
        {


            _reports = new ReportsService();
            var listReports = _reports.GetReports();
            if (listReports != null)
            {
                switch (sort)
                {
                    case "RatedBy":
                        if (sortdir == "DESC")
                            listReports = listReports.OrderByDescending(tes => tes.RatedBy);
                        else
                            listReports = listReports.OrderBy(tes => tes.RatedBy);
                        break;
                    case "RatedTo":
                        if (sortdir == "DESC")
                            listReports = listReports.OrderByDescending(tes => tes.RatedTo);
                        else
                            listReports = listReports.OrderBy(tes => tes.RatedTo);
                        break;
                    case "Review":
                        if (sortdir == "DESC")
                            listReports = listReports.OrderByDescending(tes => tes.Review);
                        else
                            listReports = listReports.OrderBy(tes => tes.Review);
                        break;
                    case "ReportedBy":
                        if (sortdir == "DESC")
                            listReports = listReports.OrderByDescending(tes => tes.ReportedBy);
                        else
                            listReports = listReports.OrderBy(tes => tes.ReportedBy);
                        break;
                    case "Reason":
                        if (sortdir == "DESC")
                            listReports = listReports.OrderByDescending(tes => tes.Reason);
                        else
                            listReports = listReports.OrderBy(tes => tes.Reason);
                        break;
                    default:
                        listReports = listReports.OrderByDescending(tes => tes.Id);
                        break;
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    listReports = listReports.Where(p => p.RatedBy != null && p.RatedTo != null && p.ReportedBy != null && p.Reason != null && p.Review != null).ToList();
                    listReports = listReports.Where(s => (s.RatedBy.ToUpper().Contains(searchString.ToUpper()) || s.RatedTo.ToUpper().Contains(searchString.ToUpper()) || s.ReportedBy.ToUpper().Contains(searchString.ToUpper()) || s.Reason.ToUpper().Contains(searchString.ToUpper()) || s.Review.ToUpper().Contains(searchString.ToUpper())));
                }
                ReportedReviewsEntity model = new ReportedReviewsEntity();
                var reportlist = listReports.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (reportlist.Count > 0)
                {
                    model.lstReviews = reportlist;
                }
                else
                {
                    model.lstReviews = null;
                }
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double)listReports.Count() / PageSize);
                model.Total = listReports.Count();
                return View(model);
            }
            return View();

        }
        /// <summary>
        /// DeleteConfirmed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteConfirmed(long id)
        {

            _reports = new ReportsService();
            var delete = _reports.DeleteReviewById(id);
            if (delete == null)
            {

                ViewBag.Message = "Something went wrong";
            }
            else
            {
                ViewBag.Message = "Deleted";
            }
            return View("ManageReviews");
        }
    }
}