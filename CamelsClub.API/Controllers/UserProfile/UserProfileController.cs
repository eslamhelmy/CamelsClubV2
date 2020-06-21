using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class UserProfileController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUnitOfWork unit, IUserProfileService categoryService)
        {
            _unit = unit;
            _userProfileService = categoryService;

        }

        [HttpGet]
      //  [AuthorizeUserFilterTest(Role ="User")]

        [Route("api/UserProfile/GetList")]
        public ResultViewModel<PagingViewModel<UserProfileViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<UserProfileViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<UserProfileViewModel>>();
            try
            {
                resultViewModel.Data = _userProfileService.Search(orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<PagingViewModel<UserProfileViewModel>>(ex.Message);
            }
           
        }
      
        [HttpPost]
       [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserProfile/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(UserProfileCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            var userID = int.Parse(UserID);
            if (userID != 0)
            {
                viewModel.ID = userID;
                _userProfileService.Add(viewModel);
                _unit.Save();
                if (!string.IsNullOrWhiteSpace(viewModel.MainImage))
                {
                    FileHelper.MoveFileFromTempPathToAnotherFolder(viewModel.MainImage, "User-Document");
                }
                if (!string.IsNullOrWhiteSpace(viewModel.CoverImage))
                {
                    FileHelper.MoveFileFromTempPathToAnotherFolder(viewModel.CoverImage, "User-Document");
                }
                if (viewModel.ProfileImages != null && viewModel.ProfileImages.Count > 0)
                {
                    foreach (var file in viewModel.ProfileImages)
                    {
                        FileHelper.MoveFileFromTempPathToAnotherFolder(file.FileName, "User-Document");
                    }

                }
                if (viewModel.ProfileVideos != null && viewModel.ProfileVideos.Count > 0)
                {
                    foreach (var file in viewModel.ProfileVideos)
                    {
                        FileHelper.MoveFileFromTempPathToAnotherFolder(file.FileName, "User-Document");
                    }

                }
                resultViewModel.Success = true;
                resultViewModel.Data = true;
            resultViewModel.Message = Resource.AddedSuccessfully;
            }
            else
            {
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.UserNotFound;
            }

            return resultViewModel;
        }

        [HttpPut]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserProfile/Edit")]
        [ValidateViewModel]
        public ResultViewModel<string> Edit(UserProfileCreateViewModel viewModel)
        {
            ResultViewModel<string> resultViewModel = new ResultViewModel<string>();

            var userID = int.Parse(UserID);
            if (userID != 0)
            {
                viewModel.ID = userID;
              //  viewModel.UserID = userID;
                var code= _userProfileService.Edit(viewModel);
                _unit.Save();
                if (!string.IsNullOrWhiteSpace(viewModel.MainImage))
                {
                    FileHelper.MoveFileFromTempPathToAnotherFolder(viewModel.MainImage, "User-Document");
                }
                if (!string.IsNullOrWhiteSpace(viewModel.CoverImage))
                {
                    FileHelper.MoveFileFromTempPathToAnotherFolder(viewModel.CoverImage, "User-Document");
                }

                //if (viewModel.ProfileImages != null && viewModel.ProfileImages.Count > 0)
                //{
                //    foreach (var file in viewModel.ProfileImages)
                //    {
                //        FileHelper.MoveFileFromTempPathToAnotherFolder(file.FileName, "User-Document");
                //    }

                //}
                //if (viewModel.ProfileVideos != null && viewModel.ProfileVideos.Count > 0)
                //{
                //    foreach (var file in viewModel.ProfileVideos)
                //    {
                //        FileHelper.MoveFileFromTempPathToAnotherFolder(file.FileName, "User-Document");
                //    }

                //}

                if(code !="")
                {
                    resultViewModel.Data = code;

                    resultViewModel.Message = Resource.SendVerificationCode;

                }
                else
                {
                    resultViewModel.Data ="true";

                    resultViewModel.Message = Resource.UpdatedSuccessfully;
                }
                resultViewModel.Success = true;
               
            }
            else
            {
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.UserNotFound;
            }

            return resultViewModel;
        }
        [HttpGet]
        [AuthorizeUserFilter(Role = "Admin")]
        [Route("api/UserProfile/GetProfileByID")]
        public ResultViewModel<UserProfileViewModel> GetProfileByID(int id)
        {
            ResultViewModel<UserProfileViewModel> resultViewModel = new ResultViewModel<UserProfileViewModel>();
            try
            {
                
                var res = _userProfileService.GetProfileByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserProfileViewModel>(ex.Message);
            }
        }


        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserProfile/GetByID")]
        public ResultViewModel<UserProfileViewModel> GetByID(int id)
        {
            ResultViewModel<UserProfileViewModel> resultViewModel = new ResultViewModel<UserProfileViewModel>();
            try
            {
                var loggedUserID = int.Parse(UserID);
                var res = _userProfileService.GetByID(loggedUserID,id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserProfileViewModel>(ex.Message);
            }
                }
        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserProfile/GetMyProfile")]
        public ResultViewModel<UserProfileViewModel> GetMyProfile()
        {
            ResultViewModel<UserProfileViewModel> resultViewModel = new ResultViewModel<UserProfileViewModel>();
            try
            {

                var res = _userProfileService.GetMyProfile(int.Parse(UserID));
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;


                return resultViewModel;

            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<UserProfileViewModel>(ex.Message);
            }
                }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserProfile/GetMyProfileImages")]
        public ResultViewModel<List<DocumentViewModel>> GetMyProfileImages()
        {
            ResultViewModel<List<DocumentViewModel>> resultViewModel = new ResultViewModel<List<DocumentViewModel>>();
            try
            {

                var res = _userProfileService.GetMyProfileImages(int.Parse(UserID));
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;


                return resultViewModel;

            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<List<DocumentViewModel>>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserProfile/GetMyProfileVideos")]
        public ResultViewModel<List<DocumentViewModel>> GetMyProfileVideos()
        {
            ResultViewModel<List<DocumentViewModel>> resultViewModel = new ResultViewModel<List<DocumentViewModel>>();
            try
            {

                var res = _userProfileService.GetMyProfileVideos(int.Parse(UserID));
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;


                return resultViewModel;

            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<List<DocumentViewModel>>(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/UserProfile/GetEditableByID")]
        public ResultViewModel<UserProfileCreateViewModel> GetEditableByID(int id)
        {
            ResultViewModel<UserProfileCreateViewModel> resultViewModel = new ResultViewModel<UserProfileCreateViewModel>();
            try
            {
                var res = _userProfileService.GetEditableByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<UserProfileCreateViewModel>(ex.Message);
            }

        }


        [HttpDelete]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Group/Delete")]
        public ResultViewModel<bool> Delete(int id)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
            try
            {
                _userProfileService.Delete(id);
                _unit.Save();
                resultViewModel.Success = true;
                return resultViewModel;

            }
            catch (System.Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }

        }

    }


}
