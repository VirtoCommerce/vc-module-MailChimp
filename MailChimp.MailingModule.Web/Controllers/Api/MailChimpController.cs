﻿using MailChimp.Helper;
using MailChimp.MailingModule.Web.Services;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Web.Security;

namespace MailChimp.MailingModule.Web.Controllers.Api
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix("api/mc")]
    [CheckPermission(Permission = PredefinedPermissions.Manage)]
    public class MailChimpController : ApiController
    {
        private readonly IMailingSettings _mailingSettings;

        public MailChimpController(IMailingSettings mailingSettings)
        {
            _mailingSettings = mailingSettings;
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("subscribe/{subscribeEmail}")]
        public IHttpActionResult Subscribe(string subscribeEmail)
        {
            EmailParameter result;
            if (!string.IsNullOrEmpty(_mailingSettings.AccessToken) && !string.IsNullOrEmpty(_mailingSettings.AccessToken) && !string.IsNullOrEmpty(_mailingSettings.SubscribersListId) &&
                subscribeEmail.Contains("@") && subscribeEmail.Contains("."))
            {
                var mc = new MailChimpManager(_mailingSettings.AccessToken, _mailingSettings.DataCenter);

                //  Create the email parameter
                var email = new EmailParameter()
                {
                    Email = subscribeEmail,
                };

                result = mc.Subscribe(_mailingSettings.SubscribersListId, email);
                if (string.IsNullOrEmpty(result.EUId))
                    return BadRequest();
            }
            else
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
