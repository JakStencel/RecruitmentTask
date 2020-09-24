using Newtonsoft.Json.Linq;
using RecruitmentTask.Services;
using System;
using System.Web.Mvc;

namespace RecruitmentTask.Controllers
{
    public class SsoController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ISSOServcie _ssoService;

        public SsoController(IAuthService authService,
                             ISSOServcie ssoService)
        {
            _authService = authService;
            _ssoService = ssoService;
        }

        public ActionResult Commento(string token, string hmac)
        {
            try
            {
                _ssoService.AreHmacWithEncodedTokenEqual(token, hmac); //return value should be taken under consideration to determine behavior, but method is not working properly

                var loggedUser = _authService.GetUserById(User.Identity.Name); //just taking logged user from db

                var payloadJson = JObject.FromObject(new
                {
                    token = token,
                    email = loggedUser.Email,
                    name = loggedUser.UserName,
                });

                var hashedPayload = _ssoService.GetHashedPayloadString(payloadJson);

                return Redirect(_ssoService.GetCommentoSSOPath(hashedPayload, hmac));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}