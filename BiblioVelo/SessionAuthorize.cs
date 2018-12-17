using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BiblioVelo
{
    public class SessionAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private string redirectUrl = "";
        public SessionAuthorize() : base()
        {
        }

        public SessionAuthorize(string redirectUrl) : base()
        {
            this.redirectUrl = redirectUrl;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                return;
            }

            // Check for authorization

            bool logged = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (logged == false)
            {
                filterContext.Result = filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        // put whatever data you want which will be sent
                        // to the client
                        message = "sorry, but you were logged out"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }


            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                string authUrl = this.redirectUrl; //passed from attribute

                //if null, get it from config
                if (String.IsNullOrEmpty(authUrl))
                {
                    var url = new UrlHelper(filterContext.RequestContext);
                    authUrl = url.Content("~/Admin/Account/Login");
                }

                if (!String.IsNullOrEmpty(authUrl))
                {
                    filterContext.HttpContext.Response.Redirect(authUrl);
                }
            }

            bool logged = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (logged == false)
            {
                string authUrl = this.redirectUrl; //passed from attribute

                //if null, get it from config
                if (String.IsNullOrEmpty(authUrl))
                {
                    var url = new UrlHelper(filterContext.RequestContext);
                    authUrl = url.Content("~/Admin/Account/Login");
                }

                if (!String.IsNullOrEmpty(authUrl))
                {
                    filterContext.HttpContext.Response.Redirect(authUrl);
                }
            }

            //else do normal process
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}