using CamelsClub.API.Filters;
using CamelsClub.API.Helpers;
using CamelsClub.Data.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using static CamelsClub.Services.UserService;

namespace CamelsClub.API.Controllers.UserManagment
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserAccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unit;
        private readonly ITokenService _tokenService;

        public UserAccountController(IUnitOfWork unit, IUserService userService,ITokenService tokenService)
        {
            _unit = unit;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("api/User/Register")]
        [ValidateViewModel]
        public ResultViewModel<ConfirmationMessageViewModel> Register(CreateUserViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                var result = new ResultViewModel<ConfirmationMessageViewModel>
                {
                    Errors = APIHelper.ValidationMessages(ModelState),
                    Success = false,
                    Message = APIHelper.ValidationMessages(ModelState).ToList().FirstOrDefault().ToString()
                };

                return result;
            }

            var res = _userService.Register(viewModel);
                return new ResultViewModel<ConfirmationMessageViewModel>(res, Resource.SendVerificationCode, true);

        }
            catch (Exception ex)
            {
                return new ResultViewModel<ConfirmationMessageViewModel>(ex.Message);
            }

}

        [HttpGet]
        [Route("api/User/IsUserNameExists")]
        [ValidateViewModel]
        public ResultViewModel<bool> IsUserNameExists(string userName)
        {
            try
            {
                var res = _userService.IsUserNameExists(userName.ToLower());
                return new ResultViewModel<bool>(res, Resource.SendVerificationCode, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }

        }

        [HttpPost]
        [Route("api/User/AdminLogin")]
        public ResultViewModel<TokenResponseViewModel> AdminLogin(AdminLoginCreateViewModel viewModel)
        {
            try
            {
                ResultViewModel<TokenResponseViewModel> result = new ResultViewModel<TokenResponseViewModel>();
                var res = _userService.AdminLogin(viewModel);
                string token = SecurityHelper.GenerateToken(res.UserID);
                var tkn = _tokenService.AddTokenForMobile(token, null, Request?.Headers?.UserAgent?.ToString(), ((HttpContextBase)Request.Properties["MS_HttpContext"])?.Request?.UserHostAddress);
                _unit.Save();
                result.Data = new TokenResponseViewModel { Token = token, UserName = res.UserName, UserImage = res.ProfileImagePath };
                result.Message = "logged successfully";
                result.Success = true;

                return result;
             
            }
            catch (Exception ex)
            {
                return new ResultViewModel<TokenResponseViewModel>(ex.Message);
            }

        }

        [HttpPost]
        [Route("api/User/Login")]
        public ResultViewModel<ConfirmationMessageViewModel> Login(LoginCreateViewModel viewModel)
       {
            try
            {
                var res = _userService.Login(viewModel.Phone);
                _unit.Save();
                return new ResultViewModel<ConfirmationMessageViewModel>
                {
                    Message = Resource.SendVerificationCode,
                    Data = res,
                    Success = true
                };

            }
            catch (Exception ex)
            {
                return new ResultViewModel<ConfirmationMessageViewModel>(ex.Message);
            }
              
        }
        [HttpGet]
        [Route("api/User/LogOut")]
        [AuthorizeUserFilter(Role = "User")]
        public ResultViewModel<bool> LogOut()
        {
            try
            {
                var resultViewModel = new ResultViewModel<bool>();
                resultViewModel.Data = _userService.SignOut(int.Parse(UserID), AccessToken);
                resultViewModel.Message = Resource.UpdatedSuccessfully;
                resultViewModel.Success = true;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
          }

        [HttpGet]
        [Route("api/User/AdminLogOut")]
        [AuthorizeUserFilter(Role = "Admin")]
        public ResultViewModel<bool> AdminLogOut()
        {
            try
            {
                var resultViewModel = new ResultViewModel<bool>();
                resultViewModel.Data = _userService.SignOut(int.Parse(UserID), AccessToken);
                resultViewModel.Message = Resource.UpdatedSuccessfully;
                resultViewModel.Success = true;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
        }

        //[HttpPost]
        ////  [Anonymous]
        //public ResultViewModel LogOut(long userID)
        //{
        //    try
        //    {
        //        bool done = _userFacade.SignOut(userID);
        //        if (done)
        //            _hubContext.Clients.All.LogOut(userID);
        //        _resultViewModel = _resultViewModel.Create(true, SharedResource.SuccessfullyCreated, done);
        //    }
        //    catch (Exception ex)
        //    {
        //        _resultViewModel = _resultViewModel.Create(false, SharedResource.ErrorOccurred);
        //    }
        //    return _resultViewModel;
        //}

        //[HttpPost]
        ////  [Anonymous]
        //public ResultViewModel LogOutAll()
        //{
        //    try
        //    {
        //        bool done = _userFacade.SignOut();
        //        if (done)
        //            _hubContext.Clients.All.LogOut();
        //        _resultViewModel = _resultViewModel.Create(true, SharedResource.SuccessfullyCreated, done);
        //    }
        //    catch (Exception ex)
        //    {
        //        _resultViewModel = _resultViewModel.Create(false, SharedResource.ErrorOccurred);
        //    }
        //    return _resultViewModel;
        //}
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/User/GetList")]
        public ResultViewModel<PagingViewModel<UserSearchViewModel>> GetList(string text="" ,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<UserSearchViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<UserSearchViewModel>>();
            var userID = int.Parse(UserID);
            resultViewModel.Data = _userService.FindUsers(userID , text, orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "Admin")]
        [Route("api/User/GetAllUsers")]
        public ResultViewModel<PagingViewModel<UserViewModel>> GetAllUsers(string text = "", string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<UserViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<UserViewModel>>();
            resultViewModel.Data = _userService.Search(text, orderBy, isAscending, pageIndex, pageSize,Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "Admin")]
        [Route("api/User/GetAllAdminUsers")]
        public ResultViewModel<PagingViewModel<AdminViewModel>> GetAllAdminUsers(string text = "", string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<AdminViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<AdminViewModel>>();
            resultViewModel.Data = _userService.GetAllAdminUsers(text, orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        //[HttpPost]
        //[Route("api/User/SetUserName")]
        //public ResultViewModel<bool> SetUserName(List<ChangeUserNameViewModel> viewModels)
        //{
        //    bool res =  _userService.SetUserName(viewModels);
        //    return new ResultViewModel<bool>();
        //}

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/User/Search")]
        public ResultViewModel<PagingViewModel<UserViewModel>> Search(string text = "", string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<UserViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<UserViewModel>>();
            var userID = int.Parse(UserID);
            resultViewModel.Data = _userService.Search(text, orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }



    }
}

