using BiblioVelo.BusinessLogic.Services;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Services;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BiblioVelo.Areas.Admin.Controllers
{
    [SessionAuthorize]
    public class DisputesController : Controller
    {
        public DisputesController()
        {
        }
        public static long staticId;
        public static long staticDisputeId;
        IDisputesService _disputes;

        // GET: Disputes
        /// <summary>
        /// Index
        /// </summary>
        /// <param name="page"></param>
        /// <param name="PageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="sort"></param>
        /// <param name="sortdir"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(int page = 1, int PageSize = 5, string searchString = "", string sort = "", string sortdir = "ASC",string message="")
        {


            _disputes = new DisputesService();
            var listdisputes = _disputes.GetDisputes();
            if (listdisputes != null)
            {
                switch (sort)
                {
                    case "RaisedBy":
                        if (sortdir == "DESC")
                            listdisputes = listdisputes.OrderByDescending(tes => tes.RaisedBy);
                        else
                            listdisputes = listdisputes.OrderBy(tes => tes.RaisedBy);
                        break;
                    case "RaisedFor":
                        if (sortdir == "DESC")
                            listdisputes = listdisputes.OrderByDescending(tes => tes.RaisedFor);
                        else
                            listdisputes = listdisputes.OrderBy(tes => tes.RaisedFor);
                        break;
                    case "CreatedDate":
                        if (sortdir == "DESC")
                            listdisputes = listdisputes.OrderByDescending(tes => tes.CreatedDate);
                        else
                            listdisputes = listdisputes.OrderBy(tes => tes.CreatedDate);
                        break;
                    case "BookingId":
                        if (sortdir == "DESC")
                            listdisputes = listdisputes.OrderByDescending(tes => tes.BookingId);
                        else
                            listdisputes = listdisputes.OrderBy(tes => tes.BookingId);
                        break;
                    case "Reason":
                        if (sortdir == "DESC")
                            listdisputes = listdisputes.OrderByDescending(tes => tes.Reason);
                        else
                            listdisputes = listdisputes.OrderBy(tes => tes.Reason);
                        break;
                    default:
                        listdisputes = listdisputes.OrderByDescending(tes => tes.CreatedDate);
                        break;
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    listdisputes = listdisputes.Where(p => p.RaisedBy != null && p.RaisedFor != null && p.CreatedDate != null && p.Reason != null && p.BookingId != null).ToList();
                    listdisputes = listdisputes.Where(s => (s.RaisedBy.ToUpper().Contains(searchString.ToUpper()) || s.RaisedFor.ToUpper().Contains(searchString.ToUpper()) || s.CreatedDate.ToUpper().Contains(searchString.ToUpper()) || s.BookingId.ToUpper().Contains(searchString.ToUpper()) || s.Reason.ToUpper().Contains(searchString.ToUpper())));
                }
                DisputeModelEntity model = new DisputeModelEntity();
                var userlist = listdisputes.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (userlist.Count > 0)
                {
                    model.lstDisputes = userlist;
                }
                else
                {
                    model.lstDisputes = null;
                }
                ViewBag.Message = message;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double)listdisputes.Count() / PageSize);
                model.Total = listdisputes.Count();
                return View(model);
            }
            return View();

        }

        /// <summary>
        /// DeleteConfirmed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteConfirmed(long id, int pagesize, int page) 
        {

            _disputes = new DisputesService();
            var delete = _disputes.DeleteDisputeById(id);
            if (delete == null)
            {

                ViewBag.Message = "Something went wrong";
                
            }
            else
            {
                ViewBag.Message = "Deleted";
            
            }
            return RedirectToAction("Index", new { message = ViewBag.Message, PageSize = pagesize, page=page });
        }

        /// <summary>
        /// Refund
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Refund(long? id, long? disputeId, int? pagesize, int? page,string message="")
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _disputes = new DisputesService();
          
            var booking = _disputes.GetRefundDetails((long)id,(long)disputeId);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pagesize;
            ViewBag.Message = message;
            return View(booking);
        }

        /// <summary>
        /// RefundConfirmed
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        // POST: Disputes/Refund/228
        [HttpPost, ActionName("Refund")]
        [ValidateAntiForgeryToken]
        public ActionResult RefundConfirmed([Bind(Include = "OwnerId,UserId,BookingId,AmountToOwner,ChargeId,DisputeId,RefundType,SpecificAmount")] BookingModel booking)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (booking.SpecificAmount != null) { booking.SpecificAmount = booking.SpecificAmount.Replace(" ", string.Empty); }
            if (booking.AmountToOwner != null) { booking.AmountToOwner = booking.AmountToOwner.Replace(" ", string.Empty); }
            _disputes = new DisputesService();

            if (booking.RefundType == "Specific" && (booking.SpecificAmount == null || booking.SpecificAmount == string.Empty))
            {
                ViewBag.Message = "SpecificCantBeZero";
                //RedirectToAction("Index", new { message = ViewBag.Message });
                return RedirectToAction("Refund", new {id=booking.BookingId, disputeId=booking.DisputeId, message = ViewBag.Message });
                //return Refund(booking.BookingId, booking.DisputeId,null,null);

            }
            else if ((booking.RefundType == "NoRefund" || booking.RefundType == "No Refund") && (booking.AmountToOwner == null || booking.AmountToOwner == string.Empty))
            {
                ViewBag.Message = "AmountCantBeZero";
                //return Refund(booking.BookingId, booking.DisputeId,null,null);
                return RedirectToAction("Refund", new { id = booking.BookingId, disputeId = booking.DisputeId, message = ViewBag.Message });
            }
            else
            {
                string refundid = _disputes.RefundToUser(booking);

                if (refundid == string.Empty)
                {
                    ViewBag.Message = "AmountCredit";

                }
                else if (refundid == null)
                {
                    ViewBag.Message = "Something went wrong";

                }
                else
                {
                    ViewBag.Message = "Refund Sucess";

                }
                return RedirectToAction("Index", new { message = ViewBag.Message });
              
            }
            
        }
        /// <summary>
        /// Fine
        /// </summary>
        /// <param name="id"></param>
        /// <param name="disputeId"></param>
        /// <returns></returns>
        public ActionResult Fine(long? id, long? disputeId, int? pagesize, int? page,string message="")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _disputes = new DisputesService();
            var booking = _disputes.GetRefundDetails((long)id, (long)disputeId);
            if (booking == null)
            {
                return HttpNotFound();
            }

            ViewBag.Message = message;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pagesize;
            return View(booking);
        }
        /// <summary>
        /// FineConfirmed
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Fine")]
        [ValidateAntiForgeryToken]
        public ActionResult FineConfirmed([Bind(Include = "OwnerId,UserId,BookingId,FineToOwner,FineToRenter")] BookingModel booking)
        {
            {
                if (booking.FineToOwner != null) { booking.FineToOwner = booking.FineToOwner.Replace(" ", string.Empty); }
                if (booking.FineToRenter != null) { booking.FineToRenter = booking.FineToRenter.Replace(" ", string.Empty); }
                _disputes = new DisputesService();
                if ((booking.FineToRenter == null || booking.FineToRenter == string.Empty) && (booking.FineToOwner == null || booking.FineToOwner == string.Empty)) 
                {
                    ViewBag.Message = "FineCantZero";
                    return RedirectToAction("Fine", new { id = booking.BookingId, disputeId = booking.DisputeId, message = ViewBag.Message });
                }

                else
                {
                    string fine = _disputes.FineToUser(booking);

                    if (fine == null)
                    {
                        ViewBag.Message = "Something went wrong";
                    }
                    else
                    {
                        ViewBag.Message = "FineSucess";
                    }
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
              
            }
        }
        /// <summary>
        /// BookingDetails
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public ActionResult BookingDetails(long? id, int page = 1, int PageSize = 5)
        {
            
            _disputes = new DisputesService();

            var booking = _disputes.GetBookingDetails((long)id);
            BookingModelEntity model = new BookingModelEntity();
            var bookinglist = booking.ToList();
            if (bookinglist.Count > 0)
            {
                model.lstBookings = bookinglist;
            }
            else
            {
                model.lstBookings = null;
            }
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = PageSize;
            ViewBag.TotalPages = Math.Ceiling((double)booking.Count() / PageSize);
            model.Total = booking.Count();
            return View(model);
        }    
    }
}