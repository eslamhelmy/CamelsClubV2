using CamelsClub.API.Filters;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Comment
{
    public class GroupController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IGroupService _groupService;

        public GroupController(IUnitOfWork unit, IGroupService categoryService)
        {
            _unit = unit;
            _groupService = categoryService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Group/GetList")]
        public ResultViewModel<PagingViewModel<GroupViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<GroupViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<GroupViewModel>>();
                //get user specified groups
                int userID = int.Parse(UserID);
                resultViewModel.Data = _groupService.Search(userID,orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

          }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Group/GetUserGroups")]
        public ResultViewModel<List<GroupViewModel>> GetUserGroups(int userID)
        {
            try
            {
                ResultViewModel<List<GroupViewModel>> resultViewModel = new ResultViewModel<List<GroupViewModel>>();
                resultViewModel.Data = _groupService.GetUserGroups(userID);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<GroupViewModel>>(ex.Message);
            }
           
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Group/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(GroupCreateViewModel viewModel)
        {
            try
            {
                ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                viewModel.UserID = int.Parse(UserID);
                var res = _groupService.Add(viewModel);
                if (!string.IsNullOrEmpty(viewModel.Image))
                {
                    FileHelper.MoveFileFromTempPathToAnotherFolder(viewModel.Image, "Camel-Document");
                }
                _unit.Save();

                resultViewModel.Success = true;
                resultViewModel.Data = true;
                resultViewModel.Message = Resource.AddedSuccessfully;

                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
         
          }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Group/RemoveCamel")]
        [ValidateViewModel]
        public ResultViewModel<bool> RemoveCamel(int camelID , int groupID)
        {
            try
            {
                ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
               var userID = int.Parse(UserID);
                bool res = _groupService.RemoveCamel(userID, camelID,groupID);
                
                resultViewModel.Success = true;
                resultViewModel.Data = true;
                resultViewModel.Message = Resource.AddedSuccessfully;

                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }

        }

        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Group/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(GroupCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                viewModel.UserID = int.Parse(UserID);
                var res = _groupService.Edit(viewModel);
                if (viewModel.Image != null)
                {
                    try
                    {
                        FileHelper.MoveFileFromTempPathToAnotherFolder(viewModel.Image, "Camel-Document");

                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }

                }
                _unit.Save();

                resultViewModel.Success = true;
                resultViewModel.Data = true;

                resultViewModel.Message = Resource.AddedSuccessfully;

                return resultViewModel;

            
           }
        [HttpGet]
        [Route("api/Group/GetByID")]
        public ResultViewModel<GroupViewModel> GetByID(int id)
        {
            ResultViewModel<GroupViewModel> resultViewModel = new ResultViewModel<GroupViewModel>();
                var res = _groupService.GetByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

        }
        [HttpGet]
        [Route("api/Group/GetEditableByID")]
        public ResultViewModel<GroupCreateViewModel> GetEditableByID(int id)
        {
            ResultViewModel<GroupCreateViewModel> resultViewModel = new ResultViewModel<GroupCreateViewModel>();
                var res = _groupService.GetEditableByID(id);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

        }


        [HttpDelete]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Group/Delete")]
        public ResultViewModel<bool> Delete(int id)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                _groupService.Delete(id);
                _unit.Save();
                resultViewModel.Success = true;
                return resultViewModel;

           
        }
    }


}
