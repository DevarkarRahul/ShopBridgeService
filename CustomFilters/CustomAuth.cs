using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Configuration;
using System.Security.Principal;

namespace ShopBridgeAPI.CustomFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAuth : AuthorizationFilterAttribute,IAuthorizationFilter
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {

                var token = actionContext.Request.Headers.Authorization.Parameter;
                var decodedString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));

                string[] creds = decodedString.Split(':');
                string userName = Convert.ToString(creds[0]);
                string password = Convert.ToString(creds[1]);

                if (IsAuthorizedUser(userName, password))
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                else
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        bool IsAuthorizedUser(string Username, string Password)
        {
            string defaultUserName = ConfigurationManager.AppSettings["apiUserName"];
            string defaultPassword = ConfigurationManager.AppSettings["apiPassword"];
            return Username.Equals(defaultUserName) && Password.Equals(defaultPassword);
        }

    }
}