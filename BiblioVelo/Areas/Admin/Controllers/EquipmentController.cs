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
    public class EquipmentController : Controller
    {

        IEquipmentService _equipmentServices;
        // GET: Admin/Equipment
        // GET: Admin/User
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ManageEquipment(int page = 1, int PageSize = 5, string searchString = "", string sort = "", string sortdir = "ASC",string message="")
        {
            _equipmentServices = new EquipmentService();
            ViewBag.sortdir = "ASC" == sortdir ? "DESC" : "ASC";
            sort = String.IsNullOrEmpty(sort) ? "CreatedDate" : sort;
            var equipmentDetail = _equipmentServices.GetAllEquipment();
            if (equipmentDetail != null)
            {
                switch (sort)
                {
                    case "Name":
                        if (sortdir == "DESC")
                            equipmentDetail = equipmentDetail.OrderByDescending(tes => tes.Name);
                        else
                            equipmentDetail = equipmentDetail.OrderBy(tes => tes.Name);
                        break;
                    case "Model":
                        if (sortdir == "DESC")
                            equipmentDetail = equipmentDetail.OrderByDescending(tes => tes.Model);
                        else
                            equipmentDetail = equipmentDetail.OrderBy(tes => tes.Model);
                        break;
                    case "Size":
                        if (sortdir == "DESC")
                            equipmentDetail = equipmentDetail.OrderByDescending(tes => tes.Size);
                        else
                            equipmentDetail = equipmentDetail.OrderBy(tes => tes.Size);
                        break;
                    case "Price":
                        if (sortdir == "DESC")
                            equipmentDetail = equipmentDetail.OrderByDescending(tes => tes.Price);
                        else
                            equipmentDetail = equipmentDetail.OrderBy(tes => tes.Price);
                        break;
                    default:
                        equipmentDetail = equipmentDetail.OrderByDescending(tes => tes.Name);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    equipmentDetail = equipmentDetail.Where(p => p.Name != null && p.Model != null && p.Size != null).ToList();
                    equipmentDetail = equipmentDetail.Where(s => (s.Name.ToUpper().Contains(searchString.ToUpper()) || s.Model.ToUpper().Contains(searchString.ToUpper()) || s.Size.ToUpper().Contains(searchString.ToUpper())));
                }
                EquipmentDetailEntity model = new EquipmentDetailEntity();
                var equipmentlist = equipmentDetail.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (equipmentlist.Count > 0)
                {
                    model.lstEquipment = equipmentlist;
                }
                else
                {
                    model.lstEquipment = null;
                }
                ViewBag.Message = message;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double)equipmentDetail.Count() / PageSize);
                model.Total = equipmentDetail.Count();
                return View(model);
            }
            return View();
        }

        /// <summary>
        /// block unblock Equipment
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult Block(int pagesize,int equipmentId, bool status)
        {
            _equipmentServices = new EquipmentService();
            _equipmentServices.EquipmentStatus(equipmentId, status);
            if (status == true)
            {
                ViewBag.Message = "BlockSuccess";
                ViewBag.PageSize = pagesize;
            }
            else
            {
                ViewBag.Message = "UnBlockSuccess";
                ViewBag.PageSize = pagesize;
            }

            return RedirectToAction("ManageEquipment", new { PageSize = pagesize, message = ViewBag.Message });
        }
    }
}