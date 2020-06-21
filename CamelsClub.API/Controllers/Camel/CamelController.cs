using CamelsClub.API.Filters;
using CamelsClub.API.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CamelsClub.API.Controllers
{
    public class CamelController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICamelService _camelService;

        public CamelController(IUnitOfWork unit, ICamelService camelService)
        {
            _unit = unit;
            _camelService = camelService;

        }

       
        [HttpGet]
        [AuthorizeUserFilter(Role = "User") ]
        [Route("api/Camel/GetList")]
        public ResultViewModel<PagingViewModel<CamelViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<CamelViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CamelViewModel>>();
            var userID = int.Parse(UserID);
            resultViewModel.Data = _camelService.Search(userID ,orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "Admin")]
        [Route("api/Camel/GetAllCamels")]
        public ResultViewModel<PagingViewModel<CamelViewModel>> GetAllCamels(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                ResultViewModel<PagingViewModel<CamelViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CamelViewModel>>();
                resultViewModel.Data = _camelService.Search(userID: 0, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<PagingViewModel<CamelViewModel>>(ex.Message);
            }
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "Admin")]
        [Route("api/Camel/GetAllUserCamels")]
        public ResultViewModel<PagingViewModel<CamelViewModel>> GetAllUserCamels(int userID, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                ResultViewModel<PagingViewModel<CamelViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CamelViewModel>>();
                resultViewModel.Data = _camelService.Search(userID: userID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<PagingViewModel<CamelViewModel>>(ex.Message);
            }
        }



        [HttpPost]
        [AuthorizeUserFilter(Role = "User") ]
        [Route("api/Camel/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(CamelCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

                viewModel.UserID = int.Parse(UserID);
                _camelService.Add(viewModel);
                if (viewModel.Files != null && viewModel.Files.Count > 0)
                {
                    foreach (var file in viewModel.Files)
                    {
                        FileHelper.MoveFileFromTempPathToAnotherFolder(file.FileName, "Camel-Document");
                    }

                }
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;
                resultViewModel.Message = Resource.AddedSuccessfully;
            
    
            return resultViewModel;
        }
        [HttpGet]
        [Route("api/Camel/GetGenderList")]
        public ResultViewModel<IEnumerable<GenderConfigDetailViewModel>> GetGenderList(DateTime birthDate)
        {
            ResultViewModel<IEnumerable<GenderConfigDetailViewModel>> resultViewModel = new ResultViewModel<IEnumerable<GenderConfigDetailViewModel>>();
                var res = _camelService.GetAllowdCamelGenderTypes(birthDate);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;

                return resultViewModel;
        }


        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Camel/GetByID")]
        public ResultViewModel<CamelViewModel> GetByID(int CamelId)
        {
            ResultViewModel<CamelViewModel> resultViewModel = new ResultViewModel<CamelViewModel>();
                var res = _camelService.GetByID(CamelId);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;

                return resultViewModel;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "Admin")]
        [Route("api/Camel/GetCamelByID")]
        public ResultViewModel<CamelViewModel> GetCamelByID(int CamelId)
        {
            ResultViewModel<CamelViewModel> resultViewModel = new ResultViewModel<CamelViewModel>();
            var res = _camelService.GetByID(CamelId);
            resultViewModel.Success = true;
            resultViewModel.Data = res;
            resultViewModel.Message = Resource.DataLoaded;

            return resultViewModel;

        }


        [HttpPut]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Camel/Edit")]
        [ValidateViewModel]
        public ResultViewModel<CamelCreateViewModel> Edit(CamelCreateViewModel viewModel)
        {
                viewModel.UserID = int.Parse(UserID);

                _camelService.Edit(viewModel);
                _unit.Save();
                if (viewModel.Files != null && viewModel.Files.Count > 0)
                {
                    try
                    {
                        foreach (var file in viewModel.Files)
                        {
                            FileHelper.MoveFileFromTempPathToAnotherFolder(file.FileName, "Camel-Document");
                        }

                    }
                    catch (Exception ex)
                    {
                        //do nothing , it means it has already the same image
                    }


                }
                return new ResultViewModel<CamelCreateViewModel>(viewModel, Resource.UpdatedSuccessfully, true);

               }

        [HttpDelete]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/Camel/Delete")]
        public ResultViewModel<bool> DeleteCamel(int id)
      {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                _camelService.Delete(id);
                _unit.Save();
                resultViewModel.Success = true;
                return resultViewModel;

        }
    }
}
