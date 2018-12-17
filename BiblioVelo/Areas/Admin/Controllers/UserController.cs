using BiblioVelo.BusinessLogic.Services;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BiblioVelo.Areas.Admin.Controllers
{
    [SessionAuthorize]
    public class UserController : Controller
    {
        IUserDetailService _userServices;

        // GET: Admin/User
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ManageUser(int page = 1, int PageSize = 5, string searchString = "", string sort = "", string sortdir = "ASC", string message = "")
        {
            _userServices = new UserDetailServices();
            ViewBag.sortdir = "ASC" == sortdir ? "DESC" : "ASC";
            sort = String.IsNullOrEmpty(sort) ? "CreatedDate" : sort;
            var userDetail = _userServices.GetAllUsers();
            if (userDetail != null)
            {
                switch (sort)
                {
                    case "Name":
                        if (sortdir == "DESC")
                            userDetail = userDetail.OrderByDescending(tes => tes.Name);
                        else
                            userDetail = userDetail.OrderBy(tes => tes.Name);
                        break;
                    case "PhoneNumber":
                        if (sortdir == "DESC")
                            userDetail = userDetail.OrderByDescending(tes => tes.PhoneNumber);
                        else
                            userDetail = userDetail.OrderBy(tes => tes.PhoneNumber);
                        break;
                    case "Address":
                        if (sortdir == "DESC")
                            userDetail = userDetail.OrderByDescending(tes => tes.Address);
                        else
                            userDetail = userDetail.OrderBy(tes => tes.Address);
                        break;
                    case "Email":
                        if (sortdir == "DESC")
                            userDetail = userDetail.OrderByDescending(tes => tes.Email);
                        else
                            userDetail = userDetail.OrderBy(tes => tes.Email);
                        break;
                    default:
                        userDetail = userDetail.OrderByDescending(tes => tes.Name);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    userDetail = userDetail.Where(p => p.Address != null && p.Name != null && p.PhoneNumber != null).ToList();
                    userDetail = userDetail.Where(s => (s.Name.ToUpper().Contains(searchString.ToUpper()) || s.PhoneNumber.ToUpper().Contains(searchString.ToUpper()) || s.Address.ToUpper().Contains(searchString.ToUpper())));
                }
                UserDetailEntity model = new UserDetailEntity();
                var userlist = userDetail.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                if (userlist.Count > 0)
                {
                    model.lstUser = userlist;
                }
                else
                {
                    model.lstUser = null;
                }
                ViewBag.Message = message;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = Math.Ceiling((double)userDetail.Count() / PageSize);
                model.Total = userDetail.Count();
                return View(model);
            }
            return View();
        }

        /// <summary>
        /// active deactive an existing user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public ActionResult Active(int pagesize, int UserId, int Active) 
        {
            _userServices = new UserDetailServices();
            _userServices.UserStatus(UserId, Active);
            if (Active == 1)
            {
                ViewBag.Message = "ActiveSuccess";
                ViewBag.PageSize = pagesize;
            }
            else
            {
                ViewBag.Message = "DeactiveSuccess";
                ViewBag.PageSize = pagesize;
            }
            return RedirectToAction("ManageUser", new { PageSize = pagesize, message = ViewBag.Message });
           
        }


    }
}