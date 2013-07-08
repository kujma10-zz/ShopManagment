using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopManagment
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public enum AdminRole
        {
            StorageOperator,
            ShopOperator,
            ShopManager
        }

        public AdminRole Role { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool isAuthorised = false;

            IPrincipal user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                //if(user.IsInRole("admin"){
                // aq adminis rolebi unda daiweros mogvianebit
                isAuthorised = true;

            }

            if (!isAuthorised)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {{"action", "Index"}, {"controller", "Home"}});
            }
        }
    }
}