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
    public class CompetitionRewardController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionRewardService _CompetitionRewardService;

        public CompetitionRewardController(IUnitOfWork unit, ICompetitionRewardService CompetitionRewardService)
        {
            _unit = unit;
            _CompetitionRewardService = CompetitionRewardService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User") ]
        [Route("api/CompetitionReward/GetList")]
        public ResultViewModel<PagingViewModel<CompetitionRewardViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<CompetitionRewardViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<CompetitionRewardViewModel>>();
            var userID = int.Parse(UserID);
            resultViewModel.Data = _CompetitionRewardService.Search(userID ,orderBy, isAscending, pageIndex, pageSize, Language);
            resultViewModel.Success = true;
            resultViewModel.Message = Resource.DataLoaded;
            return resultViewModel;
        }

       
        [HttpPost]
        [AuthorizeUserFilter(Role = "User") ]
        [Route("api/CompetitionReward/Add")]
        [ValidateViewModel]
        public ResultViewModel<bool> Add(CompetitionRewardCreateViewModel viewModel)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

                _CompetitionRewardService.Add(viewModel);
               
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data = true;
                resultViewModel.Message = Resource.AddedSuccessfully;
            
    
            return resultViewModel;
        }
       

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionReward/GetByID")]
        public ResultViewModel<CompetitionRewardViewModel> GetByID(int Id)
        {
            ResultViewModel<CompetitionRewardViewModel> resultViewModel = new ResultViewModel<CompetitionRewardViewModel>();

            try
            {
                if(_CompetitionRewardService.IsExists(Id))
                {
                    var res = _CompetitionRewardService.GetByID(Id);
                    resultViewModel.Success = true;
                    resultViewModel.Data = res;
                    resultViewModel.Message = Resource.DataLoaded;
                }
                else
                {
                    resultViewModel.Success = false;
                    resultViewModel.Message = Resource.NotFound;
                }
              

                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<CompetitionRewardViewModel>(ex.Message);
            }
               }


        [HttpPut]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionReward/Edit")]
        [ValidateViewModel]
        public ResultViewModel<bool> Edit(CompetitionRewardEditViewModel viewModel)
        {
            try
            {
                _CompetitionRewardService.Edit(viewModel);
                _unit.Save();
                return new ResultViewModel<bool>(true, Resource.UpdatedSuccessfully, true);

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
               }

        [HttpDelete]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/CompetitionReward/Delete")]
        public ResultViewModel<bool> Delete(int id)
      {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
            try
            {    
                if(_CompetitionRewardService.IsExists(id))
                {
                    _CompetitionRewardService.Delete(id);
                    _unit.Save();
                    resultViewModel.Data = true;
                    resultViewModel.Success = true;
                    resultViewModel.Message = Resource.DataDeleted;
                }
                else
                {
                    resultViewModel.Success = false;
                    resultViewModel.Message = Resource.NotFound;
                }
             
                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<bool>(ex.Message);
            }
            
        }
    }
}
