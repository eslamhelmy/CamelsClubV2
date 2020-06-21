//using CamelsClub.API.Filters;
//using CamelsClub.Data.UnitOfWork;
//using CamelsClub.Localization.Shared;
//using CamelsClub.Services;
//using CamelsClub.ViewModels;
//using System;
//using System.Web.Http;

//namespace CamelsClub.API.Controllers
//{
//    public class SubCheckerApproveController : BaseController
//    {
//        private readonly IUnitOfWork _unit;
//        private readonly ISubCheckerApproveService _subCheckerApproveService;
//        private readonly ICheckerBossApproveService _bossCheckerApproveService;

//        public SubCheckerApproveController(IUnitOfWork unit,
//                                    ISubCheckerApproveService subCheckerApproveService,
//                                    ICheckerBossApproveService bossCheckerApproveService)
//        {
//            _unit = unit;
//            _subCheckerApproveService = subCheckerApproveService;
//            _bossCheckerApproveService = bossCheckerApproveService;

//        }

//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User") ]
//        [Route("api/TeamCheckerApprove/GetMyApprovals")]
//        public ResultViewModel GetMyApprovals(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            resultViewModel.Data = _subCheckerApproveService.Search(userID ,orderBy, isAscending, pageIndex, pageSize, Language);
//            resultViewModel.Success = true;
//            resultViewModel.Message = Resource.DataLoaded;
//            return resultViewModel;
//        }
//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetUpToApprovalRequestsForBoss")]
//        public ResultViewModel GetUpToApprovalRequests(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            resultViewModel.Data = _bossCheckerApproveService.GetUpToApprovalRequests(orderBy, isAscending, pageIndex, pageSize, Language);
//            resultViewModel.Success = true;
//            resultViewModel.Message = Resource.DataLoaded;
//            return resultViewModel;
//        }

//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetReplacedCamelsRequests")]
//        public ResultViewModel GetReplacedCamelsRequests(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            resultViewModel.Data = _bossCheckerApproveService.GetReplacedCamelsRequests(orderBy, isAscending, pageIndex, pageSize, Language);
//            resultViewModel.Success = true;
//            resultViewModel.Message = Resource.DataLoaded;
//            return resultViewModel;
//        }
//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetUserRejectedCamels")]
//        public ResultViewModel GetUserRejectedCamels()
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            resultViewModel.Data = _bossCheckerApproveService.GetUserRejectedCamels(userID, Language);
//            resultViewModel.Success = true;
//            resultViewModel.Message = Resource.DataLoaded;
//            return resultViewModel;
//        }

//        [HttpPost]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/EditRejectedCamel")]
//        [ValidateViewModel]
//        public ResultViewModel EditRejectedCamel(EditRejectedCamelCreateViewModel viewModel)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);

//            _bossCheckerApproveService.EditingRejectedCamel(viewModel);
//            _unit.Save();
//            resultViewModel.Success = true;
//            resultViewModel.Data = "Done";
//            resultViewModel.Message = Resource.AddedSuccessfully;

//            return resultViewModel;
//        }
//        [HttpPost]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/ReplaceRejectedCamel")]
//        [ValidateViewModel]
//        public ResultViewModel ReplaceRejectedCamel(ReplaceRejectedCamelCreateViewModel viewModel)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            viewModel.LoggedUserID = int.Parse(UserID);
           
//            _bossCheckerApproveService.ReplaceRejectedCamel(viewModel);
//            _unit.Save();
//            resultViewModel.Success = true;
//            resultViewModel.Data = "Done";
//            resultViewModel.Message = Resource.AddedSuccessfully;

//            return resultViewModel;
//        }
//        [HttpPost]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/ApproveCamel")]
//        [ValidateViewModel]
//        public ResultViewModel ApproveCamel(CheckerBossApprovalCreateViewModel viewModel)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            viewModel.UserID = userID;
//            _bossCheckerApproveService.ApproveCamel(viewModel);
//            _unit.Save();
//            resultViewModel.Success = true;
//            resultViewModel.Data = "Done";
//            resultViewModel.Message = Resource.AddedSuccessfully;

//            return resultViewModel;
//        }

//        [HttpPost]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/RejectCamel")]
//        [ValidateViewModel]
//        public ResultViewModel RejectCamel(CheckerBossApprovalCreateViewModel viewModel)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            viewModel.UserID = userID;
//            _bossCheckerApproveService.RejectCamel(viewModel);
//            _unit.Save();
//            resultViewModel.Success = true;
//            resultViewModel.Data = "Done";
//            resultViewModel.Message = Resource.AddedSuccessfully;

//            return resultViewModel;
//        }

//        [HttpPost]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/TerminateCamel")]
//        [ValidateViewModel]
//        public ResultViewModel TerminateCamel(CheckerBossApprovalCreateViewModel viewModel)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            viewModel.UserID = userID;
//            _bossCheckerApproveService.TerminateCamel(viewModel);
//            _unit.Save();
//            resultViewModel.Success = true;
//            resultViewModel.Data = "Done";
//            resultViewModel.Message = Resource.AddedSuccessfully;

//            return resultViewModel;
//        }

//        [HttpPost]
//        [AuthorizeUserFilter(Role = "User") ]
//        [Route("api/TeamCheckerApprove/Add")]
//        [ValidateViewModel]
//        public ResultViewModel Add(CheckerApproveCreateViewModel viewModel)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();
//            var userID = int.Parse(UserID);
//            if(!_subCheckerApproveService.IsExists(viewModel.CamelCompetitionID, userID))
//            {
//                viewModel.UserID = userID;
//                _subCheckerApproveService.Add(viewModel);
//                _unit.Save();
//            }
//            resultViewModel.Success = true;
//            resultViewModel.Data = "Done";
//            resultViewModel.Message = Resource.AddedSuccessfully;

//            return resultViewModel;
//        }
       
//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetByID")]
//        public ResultViewModel GetByID(int Id)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();

//            try
//            {
               
//                    var res = _subCheckerApproveService.GetByID(Id);
//                    resultViewModel.Success = true;
//                    resultViewModel.Data = res;
//                    resultViewModel.Message = Resource.DataLoaded;
              
//                return resultViewModel;

//            }
//            catch (Exception ex)
//            {
//                return new ResultViewModel(null, ex.Message, false);
//            }
//        }

//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetInvitedUserCamels")]
//        public ResultViewModel GetInvitedUserCamels(int competitionID , int userID)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();

//            try
//            {

//                var res = _subCheckerApproveService.GetInvitedUserCamels(userID , competitionID);
//                resultViewModel.Success = true;
//                resultViewModel.Data = res;
//                resultViewModel.Message = Resource.DataLoaded;

//                return resultViewModel;

//            }
//            catch (Exception ex)
//            {
//                return new ResultViewModel(null, ex.Message, false);
//            }
//        }


//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetBossInvitedUserCamels")]
//        public ResultViewModel GetBossInvitedUserCamels(int competitionID, int userID)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();

//            try
//            {

//                var res = _subCheckerApproveService.GetBossInvitedUserCamels(userID, competitionID);
//                resultViewModel.Success = true;
//                resultViewModel.Data = res;
//                resultViewModel.Message = Resource.DataLoaded;

//                return resultViewModel;

//            }
//            catch (Exception ex)
//            {
//                return new ResultViewModel(null, ex.Message, false);
//            }
//        }

//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetBossInvitedUsers")]
//        public ResultViewModel GetBossInvitedUsers(int competitionID)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();

//            try
//            {
//                var res = _subCheckerApproveService.GetBossInvitedUsers(int.Parse(UserID), competitionID);
//                resultViewModel.Success = true;
//                resultViewModel.Data = res;
//                resultViewModel.Message = Resource.DataLoaded;

//                return resultViewModel;

//            }
//            catch (Exception ex)
//            {
//                return new ResultViewModel(null, ex.Message, false);
//            }
//        }
//        [HttpGet]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/GetMyInvitedUsers")]
//        public ResultViewModel GetMyInvitedUsers(int competitionID)
//        {
//            ResultViewModel resultViewModel = new ResultViewModel();

//            try
//            {
//                var res = _subCheckerApproveService.GetMyInvitedUsers(int.Parse(UserID), competitionID);
//                resultViewModel.Success = true;
//                resultViewModel.Data = res;
//                resultViewModel.Message = Resource.DataLoaded;

//                return resultViewModel;

//            }
//            catch (Exception ex)
//            {
//                return new ResultViewModel(null, ex.Message, false);
//            }
//        }

//        [HttpPut]
//        [AuthorizeUserFilter(Role = "User")]
//        [Route("api/TeamCheckerApprove/Edit")]
//        [ValidateViewModel]
//        public ResultViewModel Edit(CheckerApproveCreateViewModel viewModel)
//        {
//            try
//            {
//                viewModel.UserID = int.Parse(UserID);
//                _subCheckerApproveService.Edit(viewModel);
//                _unit.Save();
//                return new ResultViewModel(viewModel, Resource.UpdatedSuccessfully, true);

//            }
//            catch (Exception ex)
//            {
//                return new ResultViewModel(null, ex.Message, false);
//            }
//               }

//      //  [HttpDelete]
//      //  [AuthorizeUserFilter(Role = "User")]
//      //  [Route("api/CheckerApprove/Delete")]
//      //  public ResultViewModel Delete(int id)
//      //{
//      //      ResultViewModel resultViewModel = new ResultViewModel();
//      //      try
//      //      {    
//      //          if(_CheckerApproveService.IsExists(id))
//      //          {
//      //              _CheckerApproveService.Delete(id);
//      //              _unit.Save();
//      //              resultViewModel.Success = true;
//      //              resultViewModel.Message = Resource.DataDeleted;
//      //          }
//      //          else
//      //          {
//      //              resultViewModel.Success = false;
//      //              resultViewModel.Message = Resource.NotFound;
//      //          }
             
//      //          return resultViewModel;

//      //      }
//      //      catch (Exception ex)
//      //      {
//      //          return new ResultViewModel(null, ex.Message, false);
//      //      }
            
//      //  }
//    }
//}
