using CamelsClub.Data.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System;
using System.Web;
using System.Web.Http;

namespace CamelsClub.API.Controllers.UserManagment
{
    public class ConfirmationMessageController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly IUserConfirmationMessageService _userConfirmationMessageService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unit;
        public ConfirmationMessageController(IUnitOfWork unit, IUserConfirmationMessageService userConfirmationMessageService, ITokenService tokenService, IUserService userService)
        {
            _unit = unit;
            _userConfirmationMessageService = userConfirmationMessageService;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("api/ConfirmationMessage/VerifyCode")]
        public ResultViewModel<VerifyCodeResponseViewModel> VerifyCode(VerificationCodeCreateViewModel viewModel)
        {
            ResultViewModel<VerifyCodeResponseViewModel> resultViewModel = new ResultViewModel<VerifyCodeResponseViewModel>();
               
                    var confirmMessage = _userConfirmationMessageService
                                        .UpdateUserConfirmationMessage(viewModel.Code,viewModel.UserID);
                    string token = SecurityHelper.GenerateToken(confirmMessage.UserID);
                    var tkn = _tokenService.AddTokenForMobile(token, viewModel.DeviceID, Request?.Headers?.UserAgent?.ToString(), ((HttpContextBase)Request.Properties["MS_HttpContext"])?.Request?.UserHostAddress);
                    _unit.Save();
                    resultViewModel.Data = new VerifyCodeResponseViewModel{ Token = token, UserID = confirmMessage.UserID ,UserName = confirmMessage.UserName,Phone = confirmMessage.Phone };
                    resultViewModel.Message = Resource.VerfifySucccessfully;
                
                return resultViewModel;
           
        }


        [HttpPost]
        [Route("api/ConfirmationMessage/ResendCode")]
        public ResultViewModel<string> ResendCode(LoginCreateViewModel viewModel)
        {
            ResultViewModel<string> resultViewModel = new ResultViewModel<string>();
         
                var code = _userConfirmationMessageService
                                    .ResendCode(viewModel.Phone);
                
                resultViewModel.Data = code;
                resultViewModel.Message = Resource.VerfifySucccessfully;

                return resultViewModel;
            
        }
    }

    public class VerifyCodeResponseViewModel
    {
        public string Token { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string Phone { get; set; }
    }
}
