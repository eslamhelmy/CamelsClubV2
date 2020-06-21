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
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services
{
    public class RefereeCamelReviewService: IRefereeCamelReviewService
    {
        private readonly IUnitOfWork _unit;
        private readonly IRefreeCamelReviewRepository _repo;
        private readonly ICompetitionInviteRepository _competionInviteRepo;
        private readonly ICompetitionRepository _competitionRepo;
        private readonly ICamelCompetitionRepository _camelcompetitionrepo;
        private readonly ICheckerApproveRepository _checkerApproveRepository;
        private readonly ICamelSpecificationRepository _camelSpecificationRepository;
        private readonly ICompetitionRefereeRepository _competitionRefereeRepository;
        private readonly INotificationService _notificationService;
        public RefereeCamelReviewService(IUnitOfWork unitOfWork,
            IRefreeCamelReviewRepository refreeCamelReviewRepository,
            ICompetitionInviteRepository competitionInviteRepository,
            ICamelCompetitionRepository camelCompetitionRepository,
            ICheckerApproveRepository checkerApproveRepository,
            INotificationService notificationService,
            ICamelSpecificationRepository camelSpecificationRepository,
            ICompetitionRefereeRepository CompetitionRefereeRepository,
            ICompetitionRepository competitionRepo
            )
        
        {

            _unit = unitOfWork;
            _competitionRepo = competitionRepo;
            _repo = refreeCamelReviewRepository;
            _competionInviteRepo = competitionInviteRepository;
            _camelcompetitionrepo = camelCompetitionRepository;
            _checkerApproveRepository = checkerApproveRepository;
            _camelSpecificationRepository = camelSpecificationRepository;
            _competitionRefereeRepository = CompetitionRefereeRepository;
            _notificationService = notificationService;
        }

        public PagingViewModel<RefereeCompetitionInviteViewModel> GetUserListForBossReferee(int CompetitionID, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _competionInviteRepo.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.CompetitionID == CompetitionID)
                                .Where(x => x.SubmitDateTime.HasValue)
                                .Where(x => x.CamelCompetitions
                                                .Where(c => !c.IsDeleted)
                                                .All(c => c.RefereeReviews
                                                            .Where(a => !a.IsDeleted)
                                                         //   .Where(a => !a.Confirmed)
                                                            .Any()
                                                    )
                                      )
                                .Select(x => new RefereeCompetitionInviteViewModel
                                {
                                    ID = x.ID,
                                    FinishedByBoss = x.CamelCompetitions
                                    .Where(cc => !cc.IsDeleted)
                                    .Where(cc => cc.CompetitionID == CompetitionID)
                                    .All(cc => cc.RefereeReviews.Where(rv => !rv.IsDeleted && rv.Confirmed).Any()),
                                    Percentage = x.CamelCompetitions
                                    .Where(cc => !cc.IsDeleted)
                                    .Where(cc => cc.CompetitionID == CompetitionID)
                                    .All(cc => cc.RefereeReviews.Where(rv => !rv.IsDeleted && rv.Confirmed).Any())?
                                             x.CamelCompetitions
                                             .Where(cc => cc.CompetitionID == CompetitionID)
                                             .SelectMany(c => c.RefereeReviews).Where(rv => !rv.IsDeleted && rv.Confirmed).Sum(r => r.ActualPercentageValue) / x.CamelCompetitions.Where(cc => cc.CompetitionID == CompetitionID).Where(cc => !cc.IsDeleted).Count() : 0.0,
                                    UserID = x.User.ID,
                                    UserName = x.User.UserName,
                                    UserImage = (x.User.UserProfile != null) ? protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                                    UserStatus = x.CamelCompetitions
                                                .Where(c => !c.IsDeleted)
                                                .All(c => c.RefereeReviews
                                                            .Where(r => !r.IsDeleted)
                                                            .Any()) ? RefereeUserStatus.IsJudged : RefereeUserStatus.NotJudged

                                });
            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<RefereeCompetitionInviteViewModel> result = new List<RefereeCompetitionInviteViewModel>();

            result = query.OrderByPropertyName(orderBy, isAscending).Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<RefereeCompetitionInviteViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        public PagingViewModel<RefereeCompetitionInviteViewModel> GetUserListForReferee(int CompetitionID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _competionInviteRepo.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Where(x=> x.CompetitionID == CompetitionID)
                                .Where(x=>x.SubmitDateTime.HasValue)
                                .Where(x => x.CamelCompetitions
                                                .Where(c => !c.IsDeleted)
                                                .All(c => c.CheckerApprovers
                                                            .Where(a => !a.IsDeleted)
                                                            .All(a => a.Status == (int)ViewModels.Enums.CheckerApprovalStatus.ApprovedByBoss)
                                                    )
                                      )
                                .Select(x => new RefereeCompetitionInviteViewModel
                                {
                                    ID = x.ID,
                                    UserID = x.User.ID,
                                    UserName = x.User.UserName,
                                    UserImage = (x.User.UserProfile != null) ? protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                                    UserStatus = x.CamelCompetitions
                                                .Where(c => !c.IsDeleted)
                                                .All(c => c.RefereeReviews
                                                            .Where(r=> !r.IsDeleted)
                                                            .Any())? RefereeUserStatus.IsJudged:RefereeUserStatus.NotJudged

                                });
            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<RefereeCompetitionInviteViewModel> result = new List<RefereeCompetitionInviteViewModel>();

            result = query.OrderByPropertyName(orderBy, isAscending).Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<RefereeCompetitionInviteViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }
        
        public RefereeInvitedUserCamelsViewModel GetInvitedUserApprovedCamelsForBoss(int CompetitionID, int InviteID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            //new code
            var data = _competionInviteRepo.GetAll()
                                .Where(x => x.CompetitionID == CompetitionID)
                                .Where(x => x.ID == InviteID)
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.CamelCompetitions
                                                .Where(c => !c.IsDeleted)
                                                .All(c => c.RefereeReviews
                                                            .Where(a => !a.IsDeleted)
                                                         //   .All(a => !a.Confirmed)
                                                             .Any()
                                                )
                                      )
                                .Select(x => new RefereeInvitedUserCamelsViewModel
                                {
                                    UserName = x.User.UserName,
                                    UserImage = (x.User.UserProfile != null) ? protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                                    Camels = x.CamelCompetitions
                                         .Where(c => !c.IsDeleted)
                                         .Select(c => new RefereeCamelCompetitionViewModel
                                         {
                                             ID = c.ID,
                                             CamelID = c.CamelID,
                                             IsJudged = c.RefereeReviews
                                                    .Where(a => !a.IsDeleted).Any(),
                                             IsConfirmedByBoss = c.RefereeReviews
                                                             .Where(a=>!a.IsDeleted)
                                                             .Where(a=>a.Confirmed).Any(),
                                             CompetitionID = c.CompetitionID,
                                             CamelName = c.Camel.Name,
                                             CategoryArabicName = c.Camel.Category.NameArabic,
                                             CategoryEnglishName = c.Camel.Category.NameEnglish,
                                             CamelImages = c.Camel.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
                                             {
                                                 FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                                                 UploadedDate = doc.CreatedDate,
                                                 FileType = doc.Type
                                             }).ToList()
                                         }).ToList()

                                }).FirstOrDefault();
            data.NumberOfConfirmedCamels = data.Camels.Where(x => x.IsConfirmedByBoss).Count();
            data.NumberOfJudgedCamels = data.Camels.Where(x => x.IsJudged).Count();
            return data;

        }
        public RefereeInvitedUserCamelsViewModel GetInvitedUserApprovedCamels(int CompetitionID, int InviteID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            //new code
            var data = _competionInviteRepo.GetAll()
                                .Where(x => x.CompetitionID == CompetitionID)
                                .Where(x => x.ID == InviteID)
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.CamelCompetitions
                                                .Where(c => !c.IsDeleted)
                                                .All(c => c.CheckerApprovers
                                                            .Where(a => !a.IsDeleted)
                                                            .All(a => a.Status == (int)ViewModels.Enums.CheckerApprovalStatus.ApprovedByBoss)
                                                    )
                                      )
                                .Select(x => new RefereeInvitedUserCamelsViewModel
                                {
                                    UserName = x.User.UserName,
                                    UserImage = (x.User.UserProfile != null) ? protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                                    Camels = x.CamelCompetitions
                                         .Where(c => !c.IsDeleted)
                                         .Select(c => new RefereeCamelCompetitionViewModel
                                         {
                                             ID = c.ID,
                                             CamelID = c.CamelID,
                                             IsJudged = c.RefereeReviews
                                                    .Where(a => !a.IsDeleted).Any(),
                                             CompetitionID = c.CompetitionID,
                                             CamelName = c.Camel.Name,
                                             CategoryArabicName = c.Camel.Category.NameArabic,
                                             CategoryEnglishName = c.Camel.Category.NameEnglish,
                                             CamelImages = c.Camel.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
                                             {
                                                 FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                                                 UploadedDate = doc.CreatedDate,
                                                 FileType = doc.Type
                                             }).ToList()
                                         }).ToList()

                                }).FirstOrDefault();
            data.NumberOfJudgedCamels = data.Camels.Where(x => x.IsJudged).Count();
            return data;

        }


        public List<CamelReviewEditViewModel> GetApprovedCamelDetails(int camelCompetitionID, Languages language = Languages.Arabic)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var data = _repo.GetAll()
                            .Where(x => !x.IsDeleted)
                            .Where(x => x.CamelCompetitionID == camelCompetitionID)
                            .Select(x => new CamelReviewEditViewModel
                            {
                                ID = x.ID,
                                CamelCompetitionID = x.CamelCompetitionID,
                                ActualPercentageValue = x.ActualPercentageValue,
                                CamelSpecificationID = x.CamelSpecificationID,
                                CompetitionRefereeID = x.CompetitionRefereeID
                            }).ToList();

            return data;
        }


        public IEnumerable<ListViewModel> GetCamelSpecifications(Languages language = Languages.Arabic)
        {
            var query = _camelSpecificationRepository.GetAll().Select(spec => new ListViewModel
            {
                ID = spec.ID,
                Name = (language == Languages.Arabic)?spec.SpecificationArabic : spec.SpecificationEnglish
            });


            return query.ToList();
        }

        public int GetRefereeIDForThisCompetition(int userID ,int camelCompetitionID)
        {

            var com = _camelcompetitionrepo.GetAll().Where(camelcom => camelcom.ID == camelCompetitionID).Select(camelcom => camelcom.Competition).FirstOrDefault();
            if(com !=null)
            {
                var exist= _competitionRefereeRepository.GetAll().Where(comRef => comRef.CompetitionID == com.ID && comRef.UserID == userID).FirstOrDefault();
                if(exist != null)
                {
                    return exist.ID;
                   // return com.UserID;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool IsExist(int camelCompetitionID)
        {
            return _checkerApproveRepository.GetAll().Where(checkAPPR => checkAPPR.CamelCompetitionID == camelCompetitionID && checkAPPR.Status==2).Any();
        }
        
        public void RefereeCamelReviewBossSubmit(List<CamelReviewEditViewModel> viewModels)
        {
            _repo.EditRange(viewModels.Select(x => new RefereeCamelSpecificationReview {
                CompetitionRefereeID = x.CompetitionRefereeID,
                ActualPercentageValue = x.ActualPercentageValue,
                CamelCompetitionID = x.CamelCompetitionID,
                ID = x.ID,
                CamelSpecificationID = x.CamelSpecificationID,
                Confirmed = true
                
            }));
            _unit.Save();
         //   NotifyCamelsOwner(viewModels[0].CamelCompetitionID, viewModels[0].CompetitionRefereeID);
            CheckIsLastCamelForUser(viewModels[0].CamelCompetitionID, viewModels[0].CompetitionRefereeID);

        }
        public void SubmitRefreeCamelReview(RefreeCamelReviewCreateViewModel viewmodel)
        {
             viewmodel.RefereeID = GetRefereeIDForThisCompetition(viewmodel.UserID, viewmodel.CamelCompetitionID);

            foreach (var camSpec in viewmodel.CamelsSpecificationValues)
            {
                
                _repo.Add(
                     new RefereeCamelSpecificationReview
                     {
                         CamelCompetitionID = viewmodel.CamelCompetitionID,
                         CompetitionRefereeID = viewmodel.RefereeID,
                         CamelSpecificationID = camSpec.CamelSpecificationID,
                         ActualPercentageValue = camSpec.SpecificationValue

                     });
            }

            _unit.Save();

            //NotifyCamelsOwner(viewmodel.CamelCompetitionID, viewmodel.RefereeID);
            //CheckIsLastCamelForUser(viewmodel.CamelCompetitionID, viewmodel.RefereeID);


        }

        private void NotifyCamelsOwner (int camelCompetitionID,int RefreeID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            var camelCom = _repo.GetAll().Where(review => review.CamelCompetitionID == camelCompetitionID).Select(obj=>obj.CamelCompetition).FirstOrDefault();

            if(camelCom != null)
            {
                var notifcation = new NotificationCreateViewModel
                {

                    ContentArabic = $" تم تقييم الجمل  الخاص بك في مسابقة   {camelCom.Competition.NameArabic}",
                    ContentEnglish = $"Your Camel has been Reviewed in  {camelCom.CompetitionID} Competition",
                    NotificationTypeID = 11,
                    ArbNotificationType = " تقييم الجمل بواسطة لجنة التحكيم",
                    EngNotificationType = "Camel Review by Referee",
                    SourceID = RefreeID,
                    DestinationName= camelCom.Camel.User.UserName,
                    DestinationID= camelCom.Camel.User.ID,
                    CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + camelCom.Competition.Image,
                    CompetitionID = camelCom.CompetitionID,

                };

                _notificationService.SendNotifictionForUser(notifcation);
                
            }
 
        }


        public CamelsApprovmentStatisticViewModel GetCamelsApprovmentStatistics (int CompetitionID, int InviteID)
        {

            var camelscomIDs = _camelcompetitionrepo.GetAll().Where(camelCom => camelCom.CompetitionID == CompetitionID && camelCom.Camel.UserID == InviteID && !camelCom.Camel.IsDeleted).Select(obj => obj.ID).ToList(); // all camelcompetitionIDs of Specific Invite User
            var totalapprovedcamels = _checkerApproveRepository.GetAll()
                                                                   .Where(camAppr => camelscomIDs.Contains(camAppr.CamelCompetitionID) && camAppr.Status == 2)
                                                                   .Select(obj => obj.CamelCompetitionID).Distinct().Count();
            var ReviewedCamelsNum = _repo.GetAll().Where(camelreview => camelscomIDs.Contains(camelreview.CamelCompetitionID)).Select(data=>data.CamelCompetitionID).Distinct().Count();
            var notreviewedcamelsNum = totalapprovedcamels - ReviewedCamelsNum;

            return new CamelsApprovmentStatisticViewModel { NumberofReviewedCamels = ReviewedCamelsNum, NumberofWatingCamels = notreviewedcamelsNum };
        }


        public bool CheckCamelSpecification(List<CamelSpecificationViewModel> CamelsSpecificationValues)
        {
            var TotalCamelSpecificationCount = _camelSpecificationRepository.GetAll().Count();
             var actualCamelSpecification = CamelsSpecificationValues.Select(camSpec => camSpec.CamelSpecificationID).Distinct().Count();
            return actualCamelSpecification != TotalCamelSpecificationCount ? false : true;

        }
      
      

        public bool CheckCamelReviewdBefore(int CamelCompetitionID)
        {
            return _repo.GetAll().Any(obj => obj.CamelCompetitionID == CamelCompetitionID);
            
        }
           
        public bool AllInvitersAreReviewed(int competitionID)
        {
            var reviewedInvitersCount = _competionInviteRepo.GetAll()
                         .Where(x => x.CompetitionID == competitionID)
                        .Where(x => !x.IsDeleted)
                         .Where(x => x.SubmitDateTime.HasValue)
                        .Where(x => x.CamelCompetitions
                                    .Where(cc => !cc.IsDeleted)
                                    .All(cc => cc.RefereeReviews.Where(rv => !rv.IsDeleted).Any()))
                        .Count();
            var submittedInvitersCount = _competionInviteRepo.GetAll()
                         .Where(x => x.CompetitionID == competitionID)
                         .Where(x => x.SubmitDateTime.HasValue)
                        .Where(x => !x.IsDeleted)
                        .Count();
            return reviewedInvitersCount < submittedInvitersCount ? false : true;
        }
        public List<ReviewedCompetitorViewModel> GetAllReviewedCompetitors(int competitionID)
        {
            var res = _competionInviteRepo.GetAll()
                         .Where(x => x.CompetitionID == competitionID)
                        .Where(x => !x.IsDeleted)
                        .Where(x => x.CamelCompetitions
                                    .Where(cc => !cc.IsDeleted)
                                    .All(cc => cc.RefereeReviews.Where(rv => !rv.IsDeleted && rv.Confirmed).Any()))
                        .Select(x => new ReviewedCompetitorViewModel
                        {
                            CompetitorID = x.ID,
                            CompetitorName = x.User.UserName,
                            CompetitorImagePath = x.User.UserProfile.MainImage,
                            RefereePercentage = x.CamelCompetitions.SelectMany(c => c.RefereeReviews).Sum(r => r.ActualPercentageValue) / x.CamelCompetitions.SelectMany(c => c.RefereeReviews).Count()
                        }).ToList();
            return res;
        }

        public List<ReviewedCamelViewModel> GetAllReviewedCamels(int competitionID, int competitorID)
        {
            var res = _competionInviteRepo.GetAll()
                         .Where(x => x.CompetitionID == competitionID)
                         .Where(x => x.ID == competitorID)
                        .Where(x => !x.IsDeleted)
                        .Where(x => x.CamelCompetitions
                                    .Where(cc => !cc.IsDeleted)
                                    .All(cc => cc.RefereeReviews.Where(rv => !rv.IsDeleted).Any()))
                        .SelectMany(x => x.CamelCompetitions)
                        .Where(x => !x.IsDeleted)
                        .Select(x => new ReviewedCamelViewModel
                        {
                            ID = x.ID,
                            CamelName = x.Camel.Name,
                            CamelImagePath = "",
                            RefereePercentage = x.RefereeReviews.Where(r => !r.IsDeleted).Sum(r => r.ActualPercentageValue)
                        }).ToList();
            return res;
        }

        public bool NotifyOwner(int competitionID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            _competitionRepo.SaveIncluded(new Competition { ID = competitionID, Completed = DateTime.Now }, "Completed");
            
            _unit.Save();
            var data = 
            _competitionRepo.GetAll().Where(x => x.ID == competitionID).Select(x => new
            {
                Competition = x,
                CompetitionUser = x.User,
                CompetitionReferee = x.CompetitionReferees.Where(r => r.IsBoss && !r.IsDeleted).FirstOrDefault(),
                RefereeUser = x.CompetitionReferees.Where(r => r.IsBoss && !r.IsDeleted).Select(r=>r.User).FirstOrDefault()
            }).FirstOrDefault();
            //send notification to owner
            var notifcation = new NotificationCreateViewModel
            {

                ContentArabic = $" تم الانتهاء من  تقييم جميع المتسابقين    {data.Competition.NameArabic}",
                ContentEnglish = $"The Review of all the competitors in  {data.Competition.NameArabic} has been completed",
                NotificationTypeID = 18,
                ArbNotificationType = "اكتمال تقييم المتسابقين  ",
                EngNotificationType = "competitors review is done",
                SourceID = data.CompetitionReferee.UserID,
                SourceName = data.RefereeUser.UserName,
                DestinationName = data.CompetitionUser.UserName,
                DestinationID = data.CompetitionUser.ID,
                CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.Competition.Image,
                CompetitionID = data.Competition.ID,

            };

            _notificationService.SendNotifictionForUser(notifcation);

            return true;
        }

        private void CheckIsLastCamelForUser(int CamelCompetitionId,int refereeID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();


            // is User of Competition referee i named it for simplicity//
            var referee = _repo.GetAll().Where(camelrev => camelrev.CompetitionRefereeID == refereeID).Select(camelrev => camelrev.CompetitionReferee.User).FirstOrDefault();
            var data = _camelcompetitionrepo.GetAll().Where(camelCom => camelCom.ID == CamelCompetitionId).Select(camelCom => new { camelCom.Camel.User, camelCom.Competition }).FirstOrDefault();
           
            if(data !=null && data.Competition !=null && data.User !=null)
            {
                var camelscomIDs = _camelcompetitionrepo.GetAll().Where(camelCom => camelCom.CompetitionID == data.Competition.ID && camelCom.Camel.UserID == data.User.ID && !camelCom.Camel.IsDeleted).Select(obj => obj.ID).ToList(); // all camelcompetitionIDs of Specific Invite User

                var totalapprovedcamels = _checkerApproveRepository.GetAll()
                                                                       .Where(camAppr => camelscomIDs.Contains(camAppr.CamelCompetitionID) && camAppr.Status == 2)
                                                                       .Select(obj => obj.CamelCompetitionID).Distinct().Count();

                var ReviewedCamelsNum = _repo.GetAll().Where(camelreview => camelscomIDs.Contains(camelreview.CamelCompetitionID)).Select(obj => obj.CamelCompetitionID).Distinct().Count();
                var notreviewedcamelsNum = totalapprovedcamels - ReviewedCamelsNum;

                if(notreviewedcamelsNum==0)
                {
                   var average= CalculateCamelsSpecificationsAverage(camelscomIDs);
                    //set average in Competitor 
                   CompetitionInvite competitionIvited =
                        _camelcompetitionrepo.GetAll().Where(x => x.ID == CamelCompetitionId)
                        .Select(x => x.CompetitionInvite).FirstOrDefault();
                    competitionIvited.FinalScore = average;
                    _unit.Save();
                    //var allCompetitiors = _competionInviteRepo.GetAll().Where(x => x.CompetitionID == data.Competition.ID).Where(x => x.SubmitDateTime != null).ToList();
                    //if(allCompetitiors.Count() == allCompetitiors.Where(x=>x.FinalScore != null).Count())
                    //{
                    //    var notifcation = new NotificationCreateViewModel
                    //    {

                    //        ContentArabic = $" تم الانتهاء من  تقييم جميع المتسابقين    {data.Competition.NameArabic}",
                    //        ContentEnglish = $"The Review of all the competitors in  {data.Competition.NameArabic} has been completed",
                    //        NotificationTypeID = 12,
                    //        ArbNotificationType = "اكتمال تقييم المتسابقين  ",
                    //        EngNotificationType = "competitors review is done",
                    //        SourceID = referee.ID,
                    //        SourceName = referee.Name,
                    //        DestinationName = data.Competition.User.Name,
                    //        DestinationID = data.Competition.User.ID,
                    //        CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.Competition?.Image,
                    //        CompetitionID = data.Competition.ID,

                    //    };

                    //    _notificationService.SendNotifictionForUser(notifcation);

                    //}
                }
            }
           
        }


        private double CalculateCamelsSpecificationsAverage(List<int> CamelCompetitionIDs)
        {
            var camelsspecifications = _repo.GetAll().
                                          Where(camelreview => CamelCompetitionIDs.Contains(camelreview.CamelCompetitionID))
                                         .Sum(camelreview => camelreview.ActualPercentageValue);

            return camelsspecifications / CamelCompetitionIDs.Count;

          ;
        }
    
    
    }

    public class RefereeInvitedUserCamelsViewModel
    {
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public int NumberOfJudgedCamels { get; set; }
        public List<RefereeCamelCompetitionViewModel> Camels { get; set; }
        public int NumberOfConfirmedCamels { get; internal set; }
    }

    public class RefereeCamelCompetitionViewModel
    {
        public int ID { get; set; }
        public int CamelID { get; set; }
        public bool IsJudged { get; set; }
        public int CompetitionID { get; set; }
        public string CamelName { get; set; }
        public string CategoryArabicName { get; set; }
        public string CategoryEnglishName { get; set; }
        public List<CamelDocumentViewModel> CamelImages { get; set; }
        public bool IsConfirmedByBoss { get; internal set; }
    }

    public class RefereeCompetitionInviteViewModel
    {
        public int ID { get; set; }
        public bool FinishedByBoss { get; set; }
        public double Percentage { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public RefereeUserStatus UserStatus { get; set; }
    }
    public enum RefereeUserStatus
    {
        IsJudged,
        NotJudged
    }
}

