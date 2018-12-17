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
    public class BookingController : Controller
    {
        IBookingDetailServices _bookingServices;
        // GET: Admin/Booking
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ManageBooking(int page = 1, int PageSize = 5, string searchString = "", string sort = "", string sortdir = "DESC", string message = "")
        {
            _bookingServices = new BookingDetailServices();
            ViewBag.sortdir = "ASC" == sortdir ? "DESC" : "ASC";
            sort = String.IsNullOrEmpty(sort) ? "BookingId" : sort;
            var bookingDetail = _bookingServices.GetAllBookings();
            if (bookingDetail != null)
            {
                switch (sort)
                {
                    case "Owner":
                        if (sortdir == "DESC")
                            bookingDetail = bookingDetail.OrderByDescending(tes => tes.Owner);
                        else
                            bookingDetail = bookingDetail.OrderBy(tes => tes.Owner);
                        break;
                    case "Renter":
                        if (sortdir == "DESC")
                            bookingDetail = bookingDetail.OrderByDescending(tes => tes.Renter);
                        else
                            bookingDetail = bookingDetail.OrderBy(tes => tes.Renter);
                        break;
                    case "PickupTime":
                        if (sortdir == "DESC")
                            bookingDetail = bookingDetail.OrderByDescending(tes => Convert.ToDateTime(tes.PickUpTime));
                        else
                            bookingDetail = bookingDetail.OrderBy(tes => Convert.ToDateTime(tes.PickUpTime));
                        break;
                    case "DropTime":
                        if (sortdir == "DESC")
                            bookingDetail = bookingDetail.OrderByDescending(tes => Convert.ToDateTime(tes.DropTime));
                        else
                            bookingDetail = bookingDetail.OrderBy(tes => Convert.ToDateTime(tes.DropTime));
                        break;
                    case "BookingId":
                        if (sortdir == "DESC")
                            bookingDetail = bookingDetail.OrderByDescending(tes => tes.BookingId);
                        else
                            bookingDetail = bookingDetail.OrderBy(tes => tes.BookingId);
                        break;

                    case "NoOfDays":
                        if (sortdir == "DESC")
                            bookingDetail = bookingDetail.OrderByDescending(tes => tes.NoOfDays);
                        else
                            bookingDetail = bookingDetail.OrderBy(tes => tes.NoOfDays);
                        break;
                   
                    default:
                        bookingDetail = bookingDetail.OrderByDescending(tes => tes.BookingId);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    bookingDetail = bookingDetail.Where(p => p.BookingId != null && p.Owner != null && p.Renter != null && p.PickUpTime != null && p.DropTime != null).ToList();
                    bookingDetail = bookingDetail.Where(s => (s.BookingId.ToString().ToUpper().Contains(searchString.ToUpper()) || (s.Owner.ToUpper().Contains(searchString.ToUpper()) || s.Renter.ToUpper().Contains(searchString.ToUpper()))));
                }
                BookingDetailModel model = new BookingDetailModel();
                var userlist = bookingDetail.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (userlist.Count > 0)
                {
                    model.lstBooking = userlist;
                }
                else
                {
                    model.lstBooking = null;
                }
                ViewBag.Message = message;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double)bookingDetail.Count() / PageSize);
                model.Total = bookingDetail.Count();
                return View(model);
            }
            return View();
        }
    }
}