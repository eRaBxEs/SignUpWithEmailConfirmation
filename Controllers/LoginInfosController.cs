using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignupWithMailConfirmation.Common;
using SignupWithMailConfirmation.IServices;
using SignupWithMailConfirmation.Models;

namespace SignupWithMailConfirmation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginInfosController : ControllerBase
    {
        private readonly ILoginInfoService _loginInfoService;
        private readonly IMailService _mailService;
        public LoginInfosController(ILoginInfoService loginInfoService, IMailService mailService)
        {
            _mailService = mailService;
            _loginInfoService = loginInfoService;

        }
        // POST : api/LoginInfos
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] LoginInfo oLoginInfo) 
        {
            string sMessage = "";
            var user = await _loginInfoService.SignUp(oLoginInfo);
            if (user == null) return BadRequest(new {message = Message.ErrorFound});
            if (user.Message == Message.VerifyEmail) 
            {
                MailClass oMailClass = this.GetMailObject(user);
                await _mailService.SendMail(oMailClass);
                return BadRequest(new {message = Message.VerifyEmail});
            }

            #region Send Confirmation Mail
            if (user.Message == Message.Success) 
            {
                MailClass oMailClass = this.GetMailObject(user);
                sMessage = await _mailService.SendMail(oMailClass);
            }
            if (sMessage != Message.MailSent) return BadRequest(new {message = sMessage});
            else return Ok(new {message = Message.UserCreatedVerifyEmail});

            #endregion
            

            
        }

        [AllowAnonymous]
        [HttpPost("ConfirmMail")]
        public async Task<IActionResult> ConfirmMail (string username) 
        {
            string sMessage = await _loginInfoService.ConfirmMail(username);
            return Ok(new {message = sMessage});
        }

        public MailClass GetMailObject(LoginInfo user) 
        {
            MailClass oMailClass = new MailClass();
            oMailClass.Subject = "Mail Confirmation";
            oMailClass.Body = _mailService.GetMailBody(user);
            oMailClass.ToMailIds = new List<string> 
            {
                user.EmailId
            };

            return oMailClass;
        }

    }
}