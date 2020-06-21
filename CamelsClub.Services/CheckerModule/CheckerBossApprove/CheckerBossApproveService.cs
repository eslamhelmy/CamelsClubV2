using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CamelsClub.Services
{
    public class CheckerBossApproveService : ICheckerBossApproveService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICheckerApproveRepository _repo;
        private readonly ICamelCompetitionRepository _camelCompetitionRepository;
        private readonly ICamelRepository _camelRepository;
        private readonly ICompetitionCheckerRepository _competitionCheckerRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;


        public CheckerBossApproveService(IUnitOfWork unit,
                                        ICamelRepository camelRepository,
                                       ICamelCompetitionRepository camelCompetitionRepository , 
                                       ICheckerApproveRepository repo,
                                       ICompetitionCheckerRepository competitionCheckerRepository ,
                                       IUserRepository userRepository,
                                       INotificationService notificationService)
        {
            _unit = unit;
            _repo = repo;
            _camelRepository = camelRepository;
            _userRepository = userRepository;
            _camelCompetitionRepository = camelCompetitionRepository;
            _competitionCheckerRepository = competitionCheckerRepository;
            _notificationService = notificationService;
        }



        public PagingViewModel<CheckerApproveViewModel> GetUpToApprovalRequests( string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(x => !x.IsDeleted)
             
                .Where(x=>x.Status == (int)ViewModels.Enums.CheckerApprovalStatus.FinishedBySubChecker);
                

            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CheckerApproveViewModel> result = new List<CheckerApproveViewModel>();

            var competitions = query.Select(obj => new CheckerApproveViewModel
            {
                ID = obj.ID,
                CamelCompetitionID = obj.CamelCompetitionID ,
                CompetitionName = (language == Languages.English) ? obj.CamelCompetition.Competition.NamEnglish : obj.CamelCompetition.Competition.NameArabic,
                CamelName = obj.CamelCompetition.Camel.Name,
                SubCheckerName = obj.CompetitionChecker.User.UserName,
                Notes = obj.Notes,
           
            }).OrderByPropertyName(orderBy, isAscending);

            result = competitions.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CheckerApproveViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        
        public PagingViewModel<CheckerApproveViewModel> GetReplacedCamelsRequests( string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(x => !x.IsDeleted)

                .Where(x => x.Status == (int)ViewModels.Enums.CheckerApprovalStatus.ReplacedByUser);


            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CheckerApproveViewModel> result = new List<CheckerApproveViewModel>();

            var competitions = query.Select(obj => new CheckerApproveViewModel
            {
                ID = obj.ID,
                CamelCompetitionID = obj.CamelCompetitionID,
                CompetitionName = (language == Languages.English) ? obj.CamelCompetition.Competition.NamEnglish : obj.CamelCompetition.Competition.NameArabic,
                CamelName = obj.CamelCompetition.Camel.Name,
                SubCheckerName = obj.CompetitionChecker.User.UserName,
                Notes = obj.Notes,

            }).OrderByPropertyName(orderBy, isAscending);

            result = competitions.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CheckerApproveViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        public void ApproveCamel(CheckerBossApprovalCreateViewModel viewModel)
        {
           if( !_competitionCheckerRepository
                .GetAll()
                .Where(x=>x.UserID == viewModel.UserID && x.IsBoss)
                .Any())
            {
                throw new Exception("You can approve with admin only");
            }

            var request = _repo.GetAll()
                             .Where(x => x.ID == viewModel.CheckerApproveID)
                             .Where(x => x.Status != (int)ViewModels.Enums.CheckerApprovalStatus.ApprovedByBoss)
                             .FirstOrDefault();
            request.Status = (int)ViewModels.Enums.CheckerApprovalStatus.ApprovedByBoss;
            _unit.Save();
        }

        public void RejectCamel(CheckerBossApprovalCreateViewModel viewModel)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            if (!_competitionCheckerRepository
                .GetAll()
                .Where(x => x.UserID == viewModel.UserID && x.IsBoss)
                .Any())
            {
                throw new Exception("You can approve with admin only");
            }

            var request = _repo.GetAll()
                             .Where(x => x.ID == viewModel.CheckerApproveID)
                             .Where(x => x.Status != (int)ViewModels.Enums.CheckerApprovalStatus.ApprovedByBoss)
                             .FirstOrDefault();
            request.Status = (int)ViewModels.Enums.CheckerApprovalStatus.RejectedByBoss;
            var competitior = _camelCompetitionRepository.GetAll().Where(x => x.ID == request.CamelCompetitionID)
                                .Select(x => x.CompetitionInvite).FirstOrDefault();
            //send notification for user to change its camel or update its data
            var notifcation = new NotificationCreateViewModel
            {
                //$" رجاء استبدل هذا الجمل{request.CamelCompetition.Camel.Name}  {request.CamelCompetition.Competition.NameArabic}الخاص بك في مسابقة"
                ContentArabic =$"رجاء قم باستبدال هذا الجمل {request.CamelCompetition.Camel.Name} الموجود فى المسابقة{request.CamelCompetition.Competition.NameArabic}" ,
                ContentEnglish = $"Kindly Replpace the camel {request.CamelCompetition.Camel.Name} in this Competition {request.CamelCompetition.Competition.NamEnglish}",
                NotificationTypeID = 15,
                SourceID = request.CompetitionChecker.UserID,
                DestinationName = competitior.User.UserName,
                DestinationID = competitior.UserID,
                CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + request.CamelCompetition.Competition.Image,
                CompetitionID = request.CamelCompetition.CompetitionID,

            };

            _notificationService.SendNotifictionForUser(notifcation);


            _unit.Save();
        }
        public void TerminateCamel(CheckerBossApprovalCreateViewModel viewModel)
        {
            if (!_competitionCheckerRepository
                .GetAll()
                .Where(x => x.UserID == viewModel.UserID && x.IsBoss)
                .Any())
            {
                throw new Exception("You can approve with admin only");
            }

            var request = _repo.GetAll()
                             .Where(x => x.ID == viewModel.CheckerApproveID)
                             .Where(x => x.Status != (int)ViewModels.Enums.CheckerApprovalStatus.ApprovedByBoss)
                             .FirstOrDefault();
            request.Status = (int)ViewModels.Enums.CheckerApprovalStatus.Terminate;
            request.Notes = viewModel.Notes;
            //send notification for user to change its camel or update its data


            _unit.Save();
        }

        public List<CheckerApproveViewModel> GetUserRejectedCamels(int userID, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            
            var data =
                _repo.GetAll()
                    .Where(x => x.CamelCompetition.Camel.UserID == userID)
                    .Where(x => x.Status == (int)ViewModels.Enums.CheckerApprovalStatus.RejectedByBoss)
                     .Select(obj => new CheckerApproveViewModel
                     {
                         ID = obj.ID,
                         CamelCompetitionID = obj.CamelCompetitionID,
                         CamelID = obj.CamelCompetition.CamelID,
                         Notes = obj.Notes,
                         Status = obj.Status,
                         CompetitionName = (language == Languages.English) ? obj.CamelCompetition.Competition.NamEnglish : obj.CamelCompetition.Competition.NameArabic,
                         CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + obj.CamelCompetition.Competition.Image,
                         CamelName = obj.CamelCompetition.Camel.Name,
                         CamelImages = obj.CamelCompetition.Camel.CamelDocuments.Select(x => new CamelDocumentViewModel {
                           FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.FileName,
                           UploadedDate = x.CreatedDate,
                           FileType = x.Type
                         }).ToList()
                     })
                     .ToList();

            return data;
        }
        public void EditingRejectedCamel(EditRejectedCamelCreateViewModel viewModel)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var data = _repo.GetAll()
                             .Where(x => x.ID == viewModel.CheckerApproveID)
                             .Where(x => x.Status == (int)ViewModels.Enums.CheckerApprovalStatus.RejectedByBoss)
                             .Select(x => new
                             {
                                 Competition = x.CamelCompetition.Competition,
                                 CamelCompetitionID = x.CamelCompetitionID,
                                 OldCamel = x.CamelCompetition.Camel,
                                 CompetiterUser = x.CamelCompetition.CompetitionInvite.User,
                                 CheckerUser = x.CompetitionChecker.User
                             })
                             .FirstOrDefault();
            var bossCheckerData = _competitionCheckerRepository.GetAll().Where(x => x.CompetitionID == data.Competition.ID && !x.IsDeleted && x.IsBoss)
                                        .Select(x => new { 
                                          User = x.User
                                          
                                        }).FirstOrDefault();
            _repo.SaveIncluded(new CheckerApprove { ID = viewModel.CheckerApproveID, Status = (int)ViewModels.Enums.CheckerApprovalStatus.ReplacedByUser }, "Status");

            //var data = _camelCompetitionRepository.GetAll().Where(x => x.ID == request.CamelCompetitionID)
            //                     .Select(x => new { 
            //                      Competition = x.Competition,
            //                      Camel = x.Camel,
            //                      CompetiterUser = x.CompetitionInvite.User,
            //                      CheckerUser = x.CheckerApprovers.Select(c=>c.CompetitionChecker).Select(c=>c.User).FirstOrDefault()
            //                     }).FirstOrDefault();

            //send notification for checkers to notify them with neo data
            var notifcation = new NotificationCreateViewModel
            {
                //$" تم استبدل هذا الجمل{request.CamelCompetition.Camel.Name}  {request.CamelCompetition.Competition.NameArabic}  في مسابقة"
                ContentArabic = $"تم تعديل هذا الجمل {data.OldCamel.Name} الموجود فى مسابقة {data.Competition.NameArabic}",
                ContentEnglish = $" the camel {data.OldCamel.Name} is updated  in this Competition {data.Competition.NamEnglish}",
                NotificationTypeID = 16,
                SourceID = data.CompetiterUser.ID,
                DestinationName = bossCheckerData.User.UserName,
                DestinationID = bossCheckerData.User.ID,
                CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.Competition.Image,
                CompetitionID = data.Competition.ID,

            };

            _notificationService.SendNotifictionForUser(notifcation);

            _unit.Save();

        }

        public void ReplaceRejectedCamel(ReplaceRejectedCamelCreateViewModel viewModel)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var data = _repo.GetAll()
                             .Where(x => x.ID == viewModel.CheckerApproveID)
                             .Where(x => x.Status == (int)ViewModels.Enums.CheckerApprovalStatus.RejectedByBoss)
                             .Select(x => new
                             {
                                 Competition = x.CamelCompetition.Competition,
                                 CamelCompetitionID = x.CamelCompetitionID,
                                 OldCamel = x.CamelCompetition.Camel,
                                 CompetiterUser = x.CamelCompetition.CompetitionInvite.User,
                                 CheckerUser = x.CompetitionChecker.User
                             })
                             .FirstOrDefault();
            if(viewModel.CamelID == data.OldCamel.ID)
            {
                throw new Exception("you have to replace this camel");
            }
            //confirm that camel exist and belong to logged user
            var newCamel = _camelRepository.GetAll()
                                .Where(x => x.ID == viewModel.CamelID)
                                .Where(x => x.UserID == viewModel.LoggedUserID)
                                .Where(x => x.CategoryID == data.Competition.CategoryID)
                                .FirstOrDefault();
            if(newCamel == null)
            {
                throw new Exception("the camel category does not match the competition category");
            }
            //update camel competition with the new one
            _camelCompetitionRepository.SaveIncluded(new CamelCompetition { ID = data.CamelCompetitionID, CamelID = newCamel.ID }, "CamelID");

            _repo.SaveIncluded(new CheckerApprove { ID = viewModel.CheckerApproveID, Status = (int)ViewModels.Enums.CheckerApprovalStatus.ReplacedByUser }, "Status");
            var bossCheckerData = _competitionCheckerRepository.GetAll().Where(x => x.CompetitionID == data.Competition.ID && !x.IsDeleted && x.IsBoss)
                                         .Select(x => new {
                                             User = x.User

                                         }).FirstOrDefault();

            //var data = _camelCompetitionRepository.GetAll().Where(x => x.ID == request.CamelCompetitionID)
            //                     .Select(x => new { 
            //                      Competition = x.Competition,
            //                      Camel = x.Camel,
            //                      CompetiterUser = x.CompetitionInvite.User,
            //                      CheckerUser = x.CheckerApprovers.Select(c=>c.CompetitionChecker).Select(c=>c.User).FirstOrDefault()
            //                     }).FirstOrDefault();

            //send notification for checkers to notify them with neo data
            var notifcation = new NotificationCreateViewModel
            {
                //$" تم استبدل هذا الجمل{request.CamelCompetition.Camel.Name}  {request.CamelCompetition.Competition.NameArabic}  في مسابقة"
                ContentArabic = $"تم استبدال هذا الجمل {data.OldCamel.Name} الموجود فى مسابقة {data.Competition.NameArabic}",
                ContentEnglish = $" the camel {data.OldCamel.Name} is replaced  in this Competition {data.Competition.NamEnglish}",
                NotificationTypeID = 16,
                SourceID = data.CompetiterUser.ID,
                DestinationName = bossCheckerData.User.UserName,
                DestinationID = bossCheckerData.User.ID,
                CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.Competition.Image,
                CompetitionID = data.Competition.ID,

            };

            _notificationService.SendNotifictionForUser(notifcation);

            _unit.Save();

        }
        
    }
    
}

