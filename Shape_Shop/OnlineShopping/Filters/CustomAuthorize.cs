using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Filters
{
    /// <summary>
    /// Implement custom authorization for controllers and/or actions.
    /// </summary>
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authroized = base.AuthorizeCore(httpContext);
            if (!authroized)
            {
                // the user is not authenticated or the forms authentication
                // cookie has expired
                return false;
            }

            // Now check the session:
            if (httpContext.Session != null)
            {
                var myvar = httpContext.Session["UserId"];
                if (myvar == null)
                {
                    // the session has expired
                    return false;
                }
            }

            return true;
        }
    }
}