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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services
{
    public class CompetitionRefereeService : ICompetitionRefereeService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionRefereeRepository _repo;
        private readonly INotificationService _notificationService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICompetitionInviteRepository _competitionInviteRepository;
        private readonly ICompetitionRefereeAllocateRepository _competitionRefereeAllocateRepository;
        private readonly ICamelCompetitionRepository _camelCompetitionRepository;
        private readonly IRefreeCamelReviewRepository _camelReviewRepository;
        private readonly IGroupRepository _groupRepository;


        public CompetitionRefereeService(IUnitOfWork unit,
                                ICompetitionRefereeRepository repo,
                                ICompetitionInviteRepository competitionInviteRepository,
                                IRefreeCamelReviewRepository camelReviewRepository,
                                IGroupRepository groupRepository,
                                ICamelCompetitionRepository camelCompetitionRepository,
                                INotificationService notificationService,
                                ICompetitionRefereeAllocateRepository competitionRefereeAllocateRepository,
                                ICompetitionRepository competitionRepository)
        {
            _unit = unit;
            _repo = repo;
            _camelCompetitionRepository = camelCompetitionRepository;
            _competitionInviteRepository = competitionInviteRepository;
            _camelReviewRepository = camelReviewRepository;
            _notificationService = notificationService;
            _competitionRepository = competitionRepository;
            _competitionRefereeAllocateRepository = competitionRefereeAllocateRepository;
            _groupRepository = groupRepository;

        }
        //get associated competitionReferees with that user
        public PagingViewModel<CompetitionRefereeViewModel> Search(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
                .Where(x => x.Competition.UserID == userID);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionRefereeViewModel> result = new List<CompetitionRefereeViewModel>();

            var CompetitionReferees = query.Select(obj => new CompetitionRefereeViewModel
            {
                ID = obj.ID,
            //    CompetitionID = obj.CompetitionID,
                UserName = obj.User.UserName

            }).OrderByPropertyName(orderBy, isAscending);

            result = CompetitionReferees.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CompetitionRefereeViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(CompetitionRefereeCreateViewModel viewModel)
        {

            var insertedCompetitionReferee = _repo.Add(viewModel.ToModel());


        }

        public void Edit(CompetitionRefereeCreateViewModel viewModel)
        {

            _repo.Edit(viewModel.ToModel());

        }

        public CompetitionRefereeViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var CompetitionReferee = _repo.GetAll().Where(comp => comp.ID == id)
                .Select(obj => new CompetitionRefereeViewModel
                {
                    ID = obj.ID,
                //    CompetitionID = obj.CompetitionID,
                    UserName = obj.User.UserName
                }).FirstOrDefault();

            return CompetitionReferee;
        }

        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }
        public void Delete(int id)
        {
            _repo.Remove(id);
        }
        public List<CompetitionSpecificationViewModel> GetCompetitionSpecifications(int competitionID)
        {
            return
            _competitionRepository.GetAll()
                .Where(x => x.ID == competitionID)
                .SelectMany(x => x.CompetitionSpecifications)
                .Where(x=> !x.IsDeleted)
                .Select(x => new CompetitionSpecificationViewModel
                {
                    CamelSpecificationID = x.CamelSpecificationID,
                    MaxAllowedValue = x.MaxAllowedValue,
                    SpecificationNameArabic = x.CamelSpecification.SpecificationArabic,
                    SpecificationNameEnglish = x.CamelSpecification.SpecificationEnglish
                    
                }).ToList();
        }

        public bool HasJoinedCompetition(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            return _repo.GetAll().Where(x => x.CompetitionID == viewModel.CompetitionID && x.UserID == viewModel.UserID && x.JoinDateTime.HasValue)
                     .Any();
        }
        public bool ManualAllocate(List<RefereeAllocateCreateViewModel> viewModels)
        {
            var competitionId = _repo.GetById(viewModels[0].RefereeID).ID;
            //get all approved groups of all competitors
            var data = _competitionRepository.GetAll()
                                    .Where(x => x.ID == competitionId)
                                    .Where(x => !x.IsDeleted)
                                    .Select(x => new
                                    {
                                        AllGroupIDs = x.CamelCompetitions
                                            .Where(c => c.CompetitionInvite.SubmitDateTime.HasValue)
                                            .Where(c => !c.IsDeleted)
                                            .Select(c => c.GroupID)
                                            .Distinct()
                                            .ToList(),
                                        RejectedGroupIDs = x.ApprovedGroups
                                                .Where(c => c.Status == (int)ApprovedGroupStatus.Rejected)
                                                .Where(c => !c.IsDeleted)
                                                .Select(c => c.GroupID)
                                                .ToList(),
                                        ApprovedGroupIDs = x.ApprovedGroups
                                            .Where(c => c.Status == (int)ApprovedGroupStatus.Approved)
                                            .Where(c => !c.IsDeleted)
                                            .Select(c => c.GroupID)
                                            .ToList(),
                                        JoinedRefereeIDs = x.CompetitionReferees
                                                            .Where(c => c.IsDeleted)
                                                            .Where(c => c.JoinDateTime.HasValue)
                                                            .Where(c => c.PickupDateTime.HasValue)
                                                            .Select(c => c.ID)
                                                            .ToList()

                                    }).FirstOrDefault();

            if (data.AllGroupIDs.Count != data.ApprovedGroupIDs.Count + data.RejectedGroupIDs.Count)
            {
                throw new Exception("Checking is not finished yet");
            }
            _competitionRefereeAllocateRepository.AddRange(viewModels.Select(x => x.ToModel()).ToList());
            _unit.Save();
            return true;
        }

        public bool AutoAllocate(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            //get all approved groups of all competitors
            var data = _competitionRepository.GetAll()
                                    .Where(x => x.ID == viewModel.CompetitionID)
                                    .Where(x => !x.IsDeleted)
                                    .Select(x => new
                                    {
                                        RefereeAllocationDate = x.RefereesAllocatedDate,
                                        AllGroupIDs = x.CamelCompetitions
                                            .Where(c => c.CompetitionInvite.SubmitDateTime.HasValue)
                                            .Where(c => !c.IsDeleted)
                                            .Select(c => c.GroupID)
                                            .Distinct()
                                            .ToList(),
                                        JoinedRefereeIDs = x.CompetitionReferees
                                                            .Where(c => !c.IsDeleted)
                                                            .Where(c => c.JoinDateTime.HasValue)
                                                            .Where(c => c.PickupDateTime.HasValue)
                                                            .Select(c => c.ID)
                                                            .ToList()

                                    }).FirstOrDefault();
            if(data.RefereeAllocationDate != null)
            {
                throw new Exception("Competition is already allocated");
            }
            //check if competition finished checking
            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == viewModel.CompetitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }
            var approvedGroupsIDs = _camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == viewModel.CompetitionID)
                .Where(x => x.ApprovedByCheckerBossDateTime != null).Select(x=>x.GroupID).ToList().Distinct().ToList();
           
            //add list to store all assigning
            List<CompetitionRefereeAllocate> Allocates = new List<CompetitionRefereeAllocate>();
            //assign each group to a referee
            //init values 
            var checkersCount = data.JoinedRefereeIDs.Count;
            int i = 0;
            foreach (var groupID in approvedGroupsIDs)
            {
                foreach (var refereeId in data.JoinedRefereeIDs)
                {
                    Allocates.Add(new CompetitionRefereeAllocate
                    {
                        GroupID = groupID,
                        CompetitionRefereeID = refereeId
                    });

                }
               
            }
            //save them in DB
            _competitionRefereeAllocateRepository.AddRange(Allocates);
            _competitionRepository.SaveIncluded(new Competition { ID = viewModel.CompetitionID, RefereesAllocatedDate = DateTime.UtcNow }, "RefereesAllocatedDate");
            _unit.Save();
            return true;
        }


        public bool JoinCompetition(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            CompetitionReferee referee =
               _repo.GetAll()
               .Where(x => x.CompetitionID == viewModel.CompetitionID && !x.IsDeleted)
               .Where(x => x.UserID == viewModel.UserID)
               .FirstOrDefault();

            if (referee == null)
            {
                throw new Exception("this user is not a referee, your you don't have permission to do that");
            }

            referee.JoinDateTime = DateTime.UtcNow;
            _unit.Save();

            return true;
        }
        public bool PickupTeam(List<CompetitionCheckerPickupViewModel> viewModels, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var IDs = viewModels.Select(x => x.ID).ToList();

            var data =
                    _repo.GetAll().Where(x => IDs.Contains(x.ID)).Select(x => new {
                        Referee = x,
                        Competition = x.Competition
                    }).ToList();
            if (IDs.Count != data.Select(x => x.Competition).FirstOrDefault().MinimumRefereesCount)
            {
                return false;
            }
            data.Select(x => x.Referee).ToList().ForEach(x => x.PickupDateTime = DateTime.UtcNow);
           _competitionRepository.SaveIncluded(new Competition { ID = data.FirstOrDefault().Competition.ID, RefereePickupTeamDateTime = DateTime.UtcNow }, "RefereePickupTeamDateTime");

            _unit.Save();
            //send notification to picked checkers
            var notification = new NotificationCreateViewModel
            {
                ContentArabic = $"{NotificationArabicKeys.NewCompetitionAnnounceToChecker} {data.FirstOrDefault().Competition.NameArabic}",
                ContentEnglish = $"{NotificationEnglishKeys.NewCompetitionAnnounceToReferee} {data.FirstOrDefault().Competition.NamEnglish}",
                NotificationTypeID = (int)TypeOfNotification.RefereeRequestForJoinCompetition,
                ArbNotificationType = "الانضمام الي لجنة تحكيم المسايقة",
                EngNotificationType = "Join the competition as Referee",
                SourceID = loggedUserID,
                //   SourceName = insertedCompetition.User.Name,
                CompetitionID = data.FirstOrDefault().Competition.ID,
                CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.FirstOrDefault().Competition.Image,

            };
            Task.Run(() => {
                _notificationService.SendNotifictionForReferees(notification, data.Select(x => x.Referee).Select(c => new CompetitionRefereeCreateViewModel { UserID = c.UserID }).ToList());
            });
            return true;
        }

        public List<GroupViewModel> GetGroups(int competitionID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            
            //check if competition finished checking
            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == competitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }
            
            //check if logged user is boss
            var boss =
            _competitionRepository.GetAll()
                .Where(x => x.ID == competitionID)
                .SelectMany(x => x.CompetitionReferees)
                .FirstOrDefault(x => x.IsBoss);
            if (boss.UserID == loggedUserID)
            {
                if(!_competitionRefereeAllocateRepository.GetAll()
                .Where(x => x.CompetitionReferee.CompetitionID == competitionID)
                .Where(x => x.IsReplaced == false)
                .Any())
                {
                   return
                    _camelCompetitionRepository.GetAll()
                        .Where(x => x.CompetitionID == competitionID)
                        .GroupBy(x=> x.Group)
                        .Select(x => new GroupViewModel
                        {
                            ID = x.Key.ID,
                            NameArabic = x.Key.NameArabic,
                            NameEnglish = x.Key.NameEnglish,
                            CamelsCountInGroup = x.Key.CamelGroups.Count(),
                            ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.Key.Image,
                        })
                        .ToList();

                }
                var groups =
                _competitionRefereeAllocateRepository.GetAll()
                .Where(x => x.CompetitionReferee.CompetitionID == competitionID)
                .Where(x => x.IsReplaced == false)
                .GroupBy(x=>x.Group)
                .Select(x => new GroupViewModel
                {
                    ID = x.Key.ID,
                    NameArabic = x.Key.NameArabic,
                    NameEnglish = x.Key.NameEnglish,
                    CamelsCountInGroup = x.Key.CamelGroups.Count(),
                    ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.Key.Image,
                    IsGroupApproved = x.Key.CamelCompetitions.Where(cc=>cc.CompetitionID == competitionID).All(cc=>cc.ApprovedByRefereeBossDateTime != null),
                    AssignedReferees = x.Key.RefereeAllocates.Where(all=>all.CompetitionReferee.CompetitionID == competitionID).Select(a => new CompetitionRefereeViewModel
                    {
                        ID = a.CompetitionRefereeID,
                        UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + a.CompetitionReferee.User.UserProfile.MainImage,
                        UserName = a.CompetitionReferee.User.UserName,
                        HasEvaluated = a.CompetitionReferee.MyReviews.Any(r=> r.CamelCompetition.GroupID == x.Key.ID && !r.IsDeleted)
                    }).ToList()
                }).ToList();

                groups.ForEach(x =>
                {
                    x.RefereeCompletionPercentage = (x.AssignedReferees.Where(a => a.HasEvaluated).Count() / x.AssignedReferees.Count()) * 100;
                });
                return groups;

            }
            return
            _competitionRefereeAllocateRepository.GetAll()
                .Where(x => x.CompetitionReferee.UserID == loggedUserID)
                .Where(x => x.CompetitionReferee.CompetitionID == competitionID)
                .Where(x => x.IsReplaced == false)
                .Select(x => new GroupViewModel
                {
                    ID = x.Group.ID,
                    NameArabic = x.Group.NameArabic,
                    NameEnglish = x.Group.NameEnglish,
                    ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.Group.Image,
                    IsRefereeFinishedRating = x.Group.CamelCompetitions.Where(cc => cc.CompetitionID == competitionID && cc.GroupID == x.GroupID)
                                                .All(cc => cc.RefereeReviews.Where(v=>v.CompetitionReferee.UserID == loggedUserID && v.CompetitionReferee.CompetitionID == competitionID).Any())
                }).ToList();

        }
        public bool ApproveCamel(ApproveCamelCreateViewModel viewModel)
        {
            var competitionID = _camelCompetitionRepository.GetById(viewModel.CamelCompetitionID).CompetitionID;
            if (!_camelCompetitionRepository.GetAll()
               .Where(x => x.CompetitionID == competitionID)
               .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }

            // approve only if all referees give review
            var numberOfEvaluatedReferees =
           _camelCompetitionRepository.GetAll()
               .Where(x => x.ID == viewModel.CamelCompetitionID)
               .SelectMany(c => c.RefereeReviews)
               .GroupBy(r => r.CompetitionRefereeID)
               .Count();

            var group =
                _camelCompetitionRepository.GetAll()
               .Where(x => x.ID == viewModel.CamelCompetitionID)
               .Select(c => c.Group)
               .FirstOrDefault();

            var numberOfSupposedEvaluatedReferees =
                _groupRepository.GetAll()
                    .Where(x => x.ID == group.ID)
                    .Select(x => x.RefereeAllocates)
                    .Count();


            if(numberOfSupposedEvaluatedReferees != numberOfEvaluatedReferees)
            {
                throw new Exception("can not approve because not all referees evaluated this camel");
            }
               


            var camelCompetition =
            _camelCompetitionRepository.GetAll()
                .Where(x => x.ID == viewModel.CamelCompetitionID )
                .FirstOrDefault();
            var timeNow = DateTime.UtcNow;
            camelCompetition.ApprovedByRefereeBossDateTime = timeNow;
            _unit.Save();
            //get group and check if all camels in this group are approved
            var groupID = camelCompetition.GroupID;
            if(_camelCompetitionRepository.GetAll()
                    .Where(x=>x.GroupID == groupID)
                    .All(x=>x.ApprovedByRefereeBossDateTime != null))
            {
                //calculate value for group
              var camelCompetitions =
             _camelCompetitionRepository.GetAll()
             .Where(x => x.GroupID == groupID)
             // .Where(x => x.ApprovedByCheckerBossDateTime != null)
               .Where(x => x.CompetitionID == camelCompetition.CompetitionID)
             .Select(x => new CamelCompetitionSpecificationBossViewModel
             {
                 ID = x.ID,
                 CamelName = x.Camel.Name,
                 ApprovedDateTime = x.ApprovedByRefereeBossDateTime,
                 RefereeEvaluates = x.RefereeReviews
                                      .Where(r => !r.IsDeleted)
                                      .GroupBy(r => r.CompetitionReferee)
                                      .Select(r => new CompetitionRefereeViewModel
                                      {
                                          ID = r.Key.ID,
                                          UserName = r.Key.User.UserName,
                                          Vaules = r
                                                            .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.Key.ID && w.CamelCompetitionID == x.ID)
                                                            //  .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                                            .Select(w => new CamelSpecificationViewModel
                                                            {
                                                                CamelSpecificationID = w.CamelSpecificationID,
                                                                SpecificationValue = w.ActualPercentageValue,
                                                                SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                                                SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                                            }).ToList()
                                      }),

                 NotEvaluatedByReferees = x.Group.RefereeAllocates.Where(a => a.CompetitionReferee.MyReviews.All(r => r.CamelCompetitionID != x.ID))
                                      .Select(r => new CompetitionRefereeViewModel
                                      {
                                          ID = r.CompetitionRefereeID,
                                          UserName = r.CompetitionReferee.User.UserName,
                                            //we put values only for linq compatiablity
                                            Vaules = x.RefereeReviews
                                                  // .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.Key.ID)
                                                  .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                                  .Select(w => new CamelSpecificationViewModel
                                                  {
                                                      CamelSpecificationID = w.CamelSpecificationID,
                                                      SpecificationValue = w.ActualPercentageValue,
                                                      SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                                      SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                                  }).ToList()
                                      }).ToList(),
                 
             }).ToList();


                camelCompetitions.ForEach(x => 
                {
                    x.AverageSpecification =
                             x.RefereeEvaluates.SelectMany(e => e.Vaules)
                             .GroupBy(v => new { v.CamelSpecificationID, v.SpecificationNameArabic, v.SpecificationNameEnglish })
                             .Select(g => new CamelSpecificationViewModel
                             {
                                 CamelSpecificationID = g.Key.CamelSpecificationID,
                                 SpecificationNameArabic = g.Key.SpecificationNameArabic,
                                 SpecificationNameEnglish = g.Key.SpecificationNameEnglish,
                                 SpecificationValue = g.Select(v => v.SpecificationValue).Sum() / g.Select(v => v.SpecificationValue).Count()
                             }).ToList();
                    x.TotalRefereeValue = x.AverageSpecification.Sum(s => s.SpecificationValue);
                });


                var groupFinalScore = camelCompetitions.Sum(x => x.TotalRefereeValue) / camelCompetitions.Count();
                //update competitor 
                var competitor =  _camelCompetitionRepository.GetAll()
                    .Where(x => x.ID == viewModel.CamelCompetitionID)
                    .Select(x => x.CompetitionInvite).FirstOrDefault();
                competitor.FinalScore = groupFinalScore;
                _unit.Save();
            }
            var camelsApprovedByBossChecker = _camelCompetitionRepository.GetAll()
                                                .Where(x => x.ApprovedByCheckerBossDateTime != null)
                                                .Count();
            var camelsSubmittedByBossReferee = _camelCompetitionRepository.GetAll()
                                                .Where(x => x.ApprovedByRefereeBossDateTime != null)
                                                .Count();
            if(camelsApprovedByBossChecker == camelsSubmittedByBossReferee)
            {
                //update competition to be finished
                Competition competition =
                    _camelCompetitionRepository.GetAll()
                    .Where(x => x.ID == viewModel.CamelCompetitionID)
                    .Select(x => x.Competition)
                    .FirstOrDefault();
                competition.Completed = DateTime.UtcNow;
                _unit.Save();

            }
            return true;


        }

        public bool RejectCamel(ApproveCamelCreateViewModel viewModel)
        {
    
            var camelsCompetitions =
         _camelCompetitionRepository.GetAll()
             .Where(x => x.ID == viewModel.CamelCompetitionID)
             .ToList();
            var timeNow = DateTime.UtcNow;
            camelsCompetitions.ForEach(x => x.RejectedByRefereeBossDateTime = timeNow);
            _unit.Save();
            return true;

        }

        public bool Evaluate(RefreeCamelReviewCreateViewModel viewModel)
        {
            //get competitonId 
            var competitionID = _camelCompetitionRepository.GetById(viewModel.CamelCompetitionID).CompetitionID;
            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == competitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }
            var camel =  _camelCompetitionRepository.GetAll()
                .Where(x => x.ID == viewModel.CamelCompetitionID)
                .FirstOrDefault();
            if(camel.ApprovedByRefereeBossDateTime != null)
            {
                throw new Exception("Can not evaluate this camel cause its result is submitted by boss");
            }
            //get competition specifications
            var data =

                _camelCompetitionRepository
                                        .GetAll()
                                        .Where(x=>x.ID == viewModel.CamelCompetitionID)
                                        .Select(x => x.Competition)
                                        .Select(x => new
                                        {
                                            Specifications = x.CompetitionSpecifications.ToList(),
                                            RefereeID = x.CompetitionReferees.Where(r => r.UserID == viewModel.UserID).FirstOrDefault().ID
                                        }).FirstOrDefault();
      
            var camelSpecificationIDs = data.Specifications.Select(s => s.CamelSpecificationID).ToList();
            
            //check if all in competition specification are included in view model 
            var AllIncluded =  viewModel.CamelsSpecificationValues.Select(x => x.CamelSpecificationID).All(id => camelSpecificationIDs.Contains(id));
            if (!AllIncluded)
            {
                throw new Exception("you put specification not included in competition specifications");
            }
            //check if this referee has specifcations on this camel, return false
            var exist = _camelReviewRepository.GetAll().Any(x => x.CompetitionRefereeID == data.RefereeID &&x.CamelCompetitionID == viewModel.CamelCompetitionID);
            if (exist)
            {
                throw new Exception("you already reviewed this camel");
            }
            //check maximum limit for each specification in view model
            viewModel.CamelsSpecificationValues.ForEach(x =>
            {
                //get maximum limit
                var maximumLimitSetByOwner = data.Specifications.Where(s => s.CamelSpecificationID == x.CamelSpecificationID).FirstOrDefault().MaxAllowedValue;
                if (x.SpecificationValue > Convert.ToDouble(maximumLimitSetByOwner))
                    throw new Exception($"you exceeded maximum value for specification {x.SpecificationNameArabic}");
            });
            //save in db
            _camelReviewRepository.AddRange(viewModel.CamelsSpecificationValues.Select(x => new RefereeCamelSpecificationReview
            {
                CamelCompetitionID = viewModel.CamelCompetitionID,
                ActualPercentageValue = x.SpecificationValue,
                CamelSpecificationID = x.CamelSpecificationID,
                CompetitionRefereeID = data.RefereeID

            }));
            _unit.Save();
            return true;
        }

        public List<CamelCompetitionRefereeViewModel> GetCamels(int competitionID, int groupID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            //check if logged user is boss
            var boss =
            _competitionRepository.GetAll()
                .Where(x => x.ID == competitionID)
                .SelectMany(x => x.CompetitionReferees)
                .FirstOrDefault(x => x.IsBoss && x.ChangeByOwnerDateTime== null);
            if (boss.UserID == loggedUserID)
            {
                List<CamelCompetitionRefereeViewModel> camels = 
               _camelCompetitionRepository.GetAll()
               .Where(x => x.GroupID == groupID)
               .Where(x=> x.ApprovedByCheckerBossDateTime != null)
               .Where(x => x.CompetitionID == competitionID)
               .Select(x => new CamelCompetitionRefereeViewModel
               {
                   ID = x.ID,
                   CamelName = x.Camel.Name,
                   ApprovedDateTime = x.ApprovedByRefereeBossDateTime ,
                   RejectedDateTime = x.RejectedByRefereeBossDateTime ,
                   RefereeEvaluates = x.RefereeReviews.Where(r=> !r.IsDeleted).GroupBy(r=> r.CompetitionReferee).Select(r=> new CompetitionRefereeViewModel
                   {
                       ID = r.Key.ID,
                       UserName = r.Key.User.UserName,
                       UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + r.Key.User.UserProfile.MainImage,
                       Vaules = x.RefereeReviews
                                    .Where(w=>!w.IsDeleted && w.CompetitionRefereeID == r.Key.ID)
                                    .Where(w=>w.CompetitionReferee.Allocates.Any(a=>a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                    .Select(w=> new CamelSpecificationViewModel
                                    {
                                        CamelSpecificationID = w.CamelSpecificationID,
                                        SpecificationValue = w.ActualPercentageValue,
                                        SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                        SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                    }).ToList()
                   }),

                   NotEvaluatedByReferees = x.Group.RefereeAllocates.Where(a=>a.CompetitionReferee.MyReviews.All(r=>r.CamelCompetitionID != x.ID))
                                        .Select(r=> new CompetitionRefereeViewModel
                                        {
                                            ID = r.CompetitionRefereeID,
                                            UserName = r.CompetitionReferee.User.UserName,
                                            UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + r.CompetitionReferee.User.UserProfile.MainImage,
                                           //we put values only for linq compatiablity
                                            Vaules = x.RefereeReviews
                                                   // .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.Key.ID)
                                                    .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                                    .Select(w => new CamelSpecificationViewModel
                                                    {
                                                        CamelSpecificationID = w.CamelSpecificationID,
                                                        SpecificationValue = w.ActualPercentageValue,
                                                        SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                                        SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                                    }).ToList()
                                        }).ToList(),
                   CamelImages = x.Camel.CamelDocuments.Select(d => new CamelDocumentViewModel
                   {
                       FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + d.FileName
                   }).ToList()

               }).ToList();

                camels.ForEach(c =>
                {
                    
                   c.AverageSpecification = 
                            c.RefereeEvaluates.SelectMany(e => e.Vaules)
                            .GroupBy(v => new { v.CamelSpecificationID , v.SpecificationNameArabic, v.SpecificationNameEnglish})
                            .Select(g => new CamelSpecificationViewModel 
                                    {
                                        CamelSpecificationID = g.Key.CamelSpecificationID,
                                        SpecificationNameArabic = g.Key.SpecificationNameArabic,
                                        SpecificationNameEnglish = g.Key.SpecificationNameEnglish,
                                        SpecificationValue = g.Select(x=>x.SpecificationValue).Sum()/ g.Select(x => x.SpecificationValue).Count()
                            }).ToList();
                
                    c.CompletionPercentage = (c.RefereeEvaluates.Count() / (c.RefereeEvaluates.Count() + c.NotEvaluatedByReferees.Count())) * 100;

                });
                //calculate percentage for entire group
                camels.ForEach(x =>
                {
                  x.TotalValue =  x.AverageSpecification.Sum(avg => avg.SpecificationValue);
                });
                var groupTotalValue = camels.Sum(x => x.TotalValue) / camels.Count();
                return camels;
            }
            else
            {

                return
                _camelCompetitionRepository.GetAll()
                .Where(x => x.GroupID == groupID)
                .Where(x=> x.ApprovedByCheckerBossDateTime != null)
                .Where(x => x.CompetitionID == competitionID)
                .Select(x => new CamelCompetitionRefereeViewModel
                {
                    ID = x.ID,
                    CamelName = x.Camel.Name,
                    IsEvaluated = x.RefereeReviews.Any(c => c.CompetitionReferee.UserID == loggedUserID && !c.IsDeleted),
                    CamelImages = x.Camel.CamelDocuments.Select(d => new CamelDocumentViewModel
                    {
                        FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + d.FileName
                    }).ToList()

                }).ToList();
            }
        }

        public bool ApproveGroup(ApproveGroupCreateViewModel viewModel)
        {
            if (!_camelCompetitionRepository.GetAll()
               .Where(x => x.CompetitionID == viewModel.CompetitionID)
               .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }

            if (_camelCompetitionRepository.GetAll()
                    .Where(x => x.GroupID == viewModel.GroupID && x.CompetitionID == viewModel.CompetitionID)
                    .All(x => x.ApprovedByRefereeBossDateTime != null))
            {
                throw new Exception("You approved this group before");
            }
            //check All Camels in group are evaluated by all referees
            var res = GetBossCamelsInfo(viewModel.CompetitionID, viewModel.GroupID, viewModel.UserID);
            if(res.GroupFinalScore == 0)
            {
                throw new Exception("not all camels are rated");
            }
            //approve all camels
            var camels = _camelCompetitionRepository.GetAll()
                             .Where(x => x.GroupID == viewModel.GroupID && x.CompetitionID == viewModel.CompetitionID).ToList();

            camels.ForEach(x => x.ApprovedByRefereeBossDateTime = DateTime.UtcNow);
            
            _unit.Save();
           
            //update CompetitionInvite with final score
            var competitor = 
                _camelCompetitionRepository.GetAll()
                .Where(x => x.GroupID == viewModel.GroupID && x.CompetitionID == viewModel.CompetitionID)
                .Select(x => x.CompetitionInvite)
                .FirstOrDefault();
            competitor.FinalScore = res.GroupFinalScore;
            _unit.Save();


            return true;


            }

        public CamelCompetitionSpecificationBossViewModel GetBossCamelsSpecifications(int ID , int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            //get ompetition
            var camelCompetition = _camelCompetitionRepository.GetById(ID);
            //check if logged user is boss
            var boss =
            _competitionRepository.GetAll()
                .Where(x => x.ID == camelCompetition.CompetitionID)
                .SelectMany(x => x.CompetitionReferees)
                .FirstOrDefault(x => x.IsBoss && x.ChangeByOwnerDateTime == null);
            if (boss.UserID == loggedUserID)
            {
                CamelCompetitionSpecificationBossViewModel camel =
               _camelCompetitionRepository.GetAll()
               .Where(x => x.ID == ID)
              // .Where(x => x.ApprovedByCheckerBossDateTime != null)
             //  .Where(x => x.CompetitionID == camelCompetition.CompetitionID)
               .Select(x => new CamelCompetitionSpecificationBossViewModel
               {
                   ID = x.ID,
                   CamelName = x.Camel.Name,
                   ApprovedDateTime = x.ApprovedByRefereeBossDateTime,
                   RefereeEvaluates =
                                        //x.RefereeReviews
                                        //.Where(r => !r.IsDeleted)
                                        //.Where(r => r.CompetitionReferee.CompetitionID == x.CompetitionID)
                                        //.GroupBy(r => r.CompetitionReferee)
                                         //.Select(r => new CompetitionRefereeViewModel
                                         //{
                                         //    ID = r.Key.ID,
                                         //    UserName = r.Key.User.UserName,
                                         //    UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + r.Key.User.UserProfile.MainImage,
                                         //    Vaules = r
                                         //                     .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.Key.ID && w.CamelCompetitionID == x.ID)
                                         //                     //  .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                         //                     .Select(w => new CamelSpecificationViewModel
                                         //                     {
                                         //                         CamelSpecificationID = w.CamelSpecificationID,
                                         //                         SpecificationValue = w.ActualPercentageValue,
                                         //                         SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                         //                         SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                         //                     }).ToList()
                                         //}),

                   x.Group.RefereeAllocates.Where(c=>!c.IsReplaced)
                        .Where(c=> c.CompetitionReferee.CompetitionID == camelCompetition.CompetitionID)
                        .Select(all=>all.CompetitionReferee)
                        //.Where(r=>r.MyReviews.Select(v=>v.CamelCompetitionID).Contains(x.ID)).Count(),


                                        .Select(r => new CompetitionRefereeViewModel
                                           {
                                               ID = r.ID,
                                               UserName = r.User.UserName,
                                               DisplayName = r.User.DisplayName,
                                               UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + r.User.UserProfile.MainImage,
                                               Vaules = r.MyReviews
                                                              .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.ID &&w.CamelCompetitionID ==x.ID)
                                                            //  .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                                              .Select(w => new CamelSpecificationViewModel
                                                              {
                                                                  CamelSpecificationID = w.CamelSpecificationID,
                                                                  SpecificationValue = w.ActualPercentageValue,
                                                                  SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                                                  SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                                              }).ToList()
                                           }),

                   NotEvaluatedByReferees =
                             x.Group.RefereeAllocates.Where(c=>!c.IsReplaced)
                             .Where(c=> c.CompetitionReferee.CompetitionID == camelCompetition.CompetitionID)
                             .Select(all=>all.CompetitionReferee)
                               .Where(r=>!r.MyReviews
                                            .Where(v=>v.CamelCompetition.CompetitionID == camelCompetition.CompetitionID)
                                            .Select(v=>v.CamelCompetitionID).Contains(x.ID))
                                .Select(r => new CompetitionRefereeViewModel
                                {
                                    ID = r.ID,
                                    UserName = r.User.UserName,
                                    DisplayName = r.User.DisplayName,
                                    UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + r.User.UserProfile.MainImage,
                                    //we put values only for linq compatiablity
                                    Vaules = r.MyReviews
                                                     .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.ID && w.CamelCompetitionID == ID)
                                                  //  .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                                    .Select(w => new CamelSpecificationViewModel
                                                    {
                                                        CamelSpecificationID = w.CamelSpecificationID,
                                                        SpecificationValue = w.ActualPercentageValue,
                                                        SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                                        SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                                    }).ToList()
                                }).ToList(),
                   //   x.Group.RefereeAllocates.Where(a => a.CompetitionReferee.CompetitionID == x.CompetitionID && a.CompetitionReferee.MyReviews.All(r => r.CamelCompetitionID != x.ID))
                   //.Select(r => new CompetitionRefereeViewModel
                   //{
                   //    ID = r.CompetitionRefereeID,
                   //    UserName = r.CompetitionReferee.User.UserName,
                   //    UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + r.CompetitionReferee.User.UserProfile.MainImage,
                   //    //we put values only for linq compatiablity
                   //    Vaules = x.RefereeReviews
                   //            // .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.Key.ID)
                   //            .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                   //            .Select(w => new CamelSpecificationViewModel
                   //            {
                   //                CamelSpecificationID = w.CamelSpecificationID,
                   //                SpecificationValue = w.ActualPercentageValue,
                   //                SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                   //                SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                   //            }).ToList()
                   //}).ToList(),
                   CamelImages = x.Camel.CamelDocuments.Select(d => new CamelDocumentViewModel
                   {
                       FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + d.FileName
                   }).ToList()

               }).FirstOrDefault();

                    camel.AverageSpecification =
                             camel.RefereeEvaluates.SelectMany(e => e.Vaules)
                             .GroupBy(v => new { v.CamelSpecificationID, v.SpecificationNameArabic, v.SpecificationNameEnglish })
                             .Select(g => new CamelSpecificationViewModel
                             {
                                 CamelSpecificationID = g.Key.CamelSpecificationID,
                                 SpecificationNameArabic = g.Key.SpecificationNameArabic,
                                 SpecificationNameEnglish = g.Key.SpecificationNameEnglish,
                                 SpecificationValue =Math.Round( g.Select(x => x.SpecificationValue).Sum() / g.Select(x => x.SpecificationValue).Count(),2)
                             }).ToList();

                if(camel.NotEvaluatedByReferees.Count == 0)
                {
                    camel.TotalRefereeValue = Math.Round(camel.AverageSpecification.Sum(x => x.SpecificationValue),2);
                }
                else
                {
                    camel.TotalRefereeValue = 0;
                }
                //    camel.CompletionPercentage = (camel.RefereeEvaluates.Count() / (camel.RefereeEvaluates.Count() + camel.NotEvaluatedByReferees.Count())) * 100;


                return camel;
            }
            else
            {
                throw new Exception("logged user is not boss");
            }
        }
        //for referee
        public List<CamelSpecificationViewModel> GetRefereeCamelSpecifications(int ID, int loggedUserID)
        {
             string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
           
            //get ompetition
            var camelCompetition = _camelCompetitionRepository.GetById(ID);
           
            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == camelCompetition.CompetitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }
            //check if logged user is boss
            var boss =
            _competitionRepository.GetAll()
                .Where(x => x.ID == camelCompetition.CompetitionID)
                .SelectMany(x => x.CompetitionReferees)
                .FirstOrDefault(x => x.IsBoss && x.ChangeByOwnerDateTime == null);
            if (boss.UserID == loggedUserID)
            {
                throw new Exception("logged user is boss");
            }
            else
            {
                return
                   _camelCompetitionRepository.GetAll()
               .Where(x => x.ID == ID)
               .SelectMany(x=> x.RefereeReviews)
               .Where(r=>r.CompetitionReferee.UserID == loggedUserID)
               .Select(g => new CamelSpecificationViewModel
               {
                   CamelSpecificationID = g.CamelSpecificationID,
                   SpecificationNameArabic = g.CamelSpecification.SpecificationArabic,
                   SpecificationNameEnglish = g.CamelSpecification.SpecificationEnglish,
                   SpecificationValue = g.ActualPercentageValue
               }).ToList();

            }

        }

        public OverallGroupRefereeViewModel GetRefereeCamelsInfo(int competitionID, int groupID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == competitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }
            //check if logged user is boss
            var boss =
            _competitionRepository.GetAll()
                .Where(x => x.ID == competitionID)
                .SelectMany(x => x.CompetitionReferees)
                .FirstOrDefault(x => x.IsBoss && x.ChangeByOwnerDateTime == null);
            if (boss.UserID == loggedUserID)
            {
                throw new Exception("logged user is boss, you must log with referee");
            }
            else
            {

                var camels = 
                _camelCompetitionRepository.GetAll()
                .Where(x => x.GroupID == groupID)
                .Where(x => x.ApprovedByCheckerBossDateTime != null)
                .Where(x => x.CompetitionID == competitionID)
                .Select(x => new CamelCompetitionInfoRefereeViewModel
                {
                    ID = x.ID,
                    CamelName = x.Camel.Name,
                    IsEvaluated = x.RefereeReviews.Any(c => c.CompetitionReferee.UserID == loggedUserID && !c.IsDeleted),
                    CamelImages = x.Camel.CamelDocuments.Select(d => new CamelDocumentViewModel
                    {
                        FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + d.FileName
                    }).ToList()

                }).ToList();
                var result = new OverallGroupRefereeViewModel
                {
                    CompletePercentage = (camels.Where(x => x.IsEvaluated).Count() * 100) / camels.Count(),
                    Camels = camels
                };
                return result;
            }
        }
        public OverallGroupViewModel GetBossCamelsInfo(int competitionID, int groupID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
           
            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == competitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }

            //check if logged user is boss
            var boss =
            _competitionRepository.GetAll()
                .Where(x => x.ID == competitionID)
                .SelectMany(x => x.CompetitionReferees)
                .FirstOrDefault(x => x.IsBoss && x.ChangeByOwnerDateTime == null);
            if (boss.UserID == loggedUserID)
            {
                var camels =
               _camelCompetitionRepository.GetAll()
               .Where(x => x.GroupID == groupID)
               .Where(x => x.ApprovedByCheckerBossDateTime != null)
               .Where(x => x.CompetitionID == competitionID)
               .Select(x => new CamelCompetitionBossRefereeViewModel
               {
                   ID = x.ID,
                   FinalScore = x.CompetitionInvite.FinalScore,
                   CamelName = x.Camel.Name,
                   ApprovedDateTime = x.ApprovedByRefereeBossDateTime,

                   RefereeEvaluatesCount =
                                            //x.RefereeReviews.Where(r => !r.IsDeleted)
                                            //.GroupBy(r => r.CompetitionRefereeID)
                                            //.Count(),
                   x.Group.RefereeAllocates
                   .Where(all=>all.CompetitionReferee.CompetitionID == competitionID)
                   .Where(c=>!c.IsReplaced).Select(all=>all.CompetitionReferee).Where(r=>r.MyReviews.Select(v=>v.CamelCompetitionID).Contains(x.ID)).Count(),
                   NotEvaluatedByRefereesCount = 
                                        //x.Group.RefereeAllocates.Where(all => !all.IsReplaced).Where(a => a.CompetitionReferee.MyReviews.All(r => r.CamelCompetitionID != x.ID))
                                        //.GroupBy(a => a.CompetitionRefereeID)
                                        //.Count(),
                    x.Group.RefereeAllocates
                    .Where(all=>all.CompetitionReferee.CompetitionID == competitionID)
                    .Where(c=>!c.IsReplaced)
                    .Select(all=>all.CompetitionReferee).Where(r=>!r.MyReviews.Select(v=>v.CamelCompetitionID).Contains(x.ID)).Count(),

                   CamelImages = x.Camel.CamelDocuments.Select(d => new CamelDocumentViewModel
                   {
                       FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + d.FileName
                   }).ToList()
                   
               }).ToList();

                camels.ForEach(x =>
                {
                    if (x.NotEvaluatedByRefereesCount > 0)
                        x.IsEvaluated = false;
                    else if(x.NotEvaluatedByRefereesCount == 0)
                        x.IsEvaluated = true;
                    else
                    {
                        x.IsEvaluated = false;
                    }
                });
                double groupFinalScore = 0;
                if (camels.All(c => c.IsEvaluated))
                {
                    //groupFinalScore = (int)camels.FirstOrDefault().FinalScore;
                    var camelCompetitions =
            _camelCompetitionRepository.GetAll()
            .Where(x => x.GroupID == groupID)
              // .Where(x => x.ApprovedByCheckerBossDateTime != null)
              .Where(x => x.CompetitionID == competitionID)
            .Select(x => new CamelCompetitionSpecificationBossViewModel
            {
                ID = x.ID,
                CamelName = x.Camel.Name,
                ApprovedDateTime = x.ApprovedByRefereeBossDateTime,
                RefereeEvaluates = x.RefereeReviews
                                     .Where(r => !r.IsDeleted)
                                     .GroupBy(r => r.CompetitionReferee)
                                     .Select(r => new CompetitionRefereeViewModel
                                     {
                                         ID = r.Key.ID,
                                         UserName = r.Key.User.UserName,
                                         Vaules = r
                                                           .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.Key.ID && w.CamelCompetitionID == x.ID)
                                                           //  .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                                           .Select(w => new CamelSpecificationViewModel
                                                           {
                                                               CamelSpecificationID = w.CamelSpecificationID,
                                                               SpecificationValue = w.ActualPercentageValue,
                                                               SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                                               SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                                           }).ToList()
                                     }),

                NotEvaluatedByReferees = x.Group.RefereeAllocates.Where(a => a.CompetitionReferee.MyReviews.All(r => r.CamelCompetitionID != x.ID))
                                     .Select(r => new CompetitionRefereeViewModel
                                     {
                                         ID = r.CompetitionRefereeID,
                                         UserName = r.CompetitionReferee.User.UserName,
                                          //we put values only for linq compatiablity
                                          Vaules = x.RefereeReviews
                                                 // .Where(w => !w.IsDeleted && w.CompetitionRefereeID == r.Key.ID)
                                                 .Where(w => w.CompetitionReferee.Allocates.Any(a => a.GroupID == x.GroupID && !a.IsReplaced && !a.IsDeleted))
                                                 .Select(w => new CamelSpecificationViewModel
                                                 {
                                                     CamelSpecificationID = w.CamelSpecificationID,
                                                     SpecificationValue = w.ActualPercentageValue,
                                                     SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                                     SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                                 }).ToList()
                                     }).ToList(),

                    }).ToList();


            camelCompetitions.ForEach(x =>
            {
                x.AverageSpecification =
                            x.RefereeEvaluates.SelectMany(e => e.Vaules)
                            .GroupBy(v => new { v.CamelSpecificationID, v.SpecificationNameArabic, v.SpecificationNameEnglish })
                            .Select(g => new CamelSpecificationViewModel
                            {
                                CamelSpecificationID = g.Key.CamelSpecificationID,
                                SpecificationNameArabic = g.Key.SpecificationNameArabic,
                                SpecificationNameEnglish = g.Key.SpecificationNameEnglish,
                                SpecificationValue = Math.Round(Convert.ToDouble(g.Select(v => v.SpecificationValue).Sum()) / g.Select(v => v.SpecificationValue).Count(),2)
                            }).ToList();
                x.TotalRefereeValue = Math.Round(Convert.ToDouble( x.AverageSpecification.Sum(s => s.SpecificationValue)));
            });


            groupFinalScore = Math.Round(Convert.ToDouble(camelCompetitions.Sum(x => x.TotalRefereeValue)) / camelCompetitions.Count(),2);

                }
                var result = new OverallGroupViewModel
                {
                    GroupFinalScore = groupFinalScore,
                    CompletePercentage = (camels.Where(x => x.IsEvaluated).Count() * 100) / camels.Count(),
                    Camels = camels
                };
                return result;
            }
            else
            {
                throw new Exception("logged user is not boss");
            }
        }

        public RefereesReportViewModel GetPickedTeam(int competitionID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            if (!_groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.RefereeAllocates)
                        .Where(a => a.CompetitionReferee.CompetitionID == competitionID)
                        .Any())
            {
                var data = _competitionRepository.GetAll()
                   .Where(x => !x.IsDeleted)
                   .Where(x => x.ID == competitionID)
                   .Select(x => new
                   {
                       CamelsCount = x.CamelCompetitions.Where(c => c.CompetitionID == competitionID)
                                    .Where(c => c.ApprovedByCheckerBossDateTime != null)
                                    .Count(),
                       Referees = x.CompetitionReferees.Where(r=> r.PickupDateTime != null).Where(c => !c.IsBoss).Select(c => new CompetitionRefereeViewModel
                       {
                           ID = c.ID,
                           UserName = c.User.UserName,
                           DisplayName = c.User.DisplayName,
                           UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage,
                           IsBoss = c.IsBoss,
                           HasJoined = c.JoinDateTime.HasValue,
                           HasPicked = c.PickupDateTime.HasValue,
                           CamelsEvaluatedCount = c.Competition.CamelCompetitions.Where(cc => cc.RefereeReviews.Where(a => a.CompetitionRefereeID == c.ID).Any())
                                                 .Count(),
                           AssignedCamels = c.Allocates.Select(a => a.Group).Select(g => g.CamelGroups).Count()
                       }).ToList(),
                       RefereeBoss = x.CompetitionReferees
                                            .Where(c => !c.IsDeleted)
                                            .Where(c => c.IsBoss)
                                            .Where(c => c.ChangeByOwnerDateTime == null)
                                            .Select(c => new { c.ID, c.UserID })
                                            .FirstOrDefault()


                   })
                   .FirstOrDefault();

                //check if the loggeduser is boss
                if (!_repo.GetAll().Where(x => x.UserID == loggedUserID && x.CompetitionID == competitionID && x.IsBoss && !x.IsDeleted).Any())
                {
                    throw new Exception("the logged user is not boss checker for this competition");
                }
                var res = new RefereesReportViewModel
                {
                    TotalNumberOfAllCamels = data.CamelsCount,
                    TotalNumberOfEvaluatedCamels = 0,
                    CheckingCompletitionRatio = 0,
                    Referees = data.Referees.ToList()
                };


                return res;
            }

            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == competitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }
            var refereesInCompetition =
                _groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.RefereeAllocates)
                        .Where(a => a.CompetitionReferee.CompetitionID == competitionID)
                        .GroupBy(x => x.CompetitionReferee)
                        .Select(g => new CompetitionRefereeViewModel
                        {
                            ID = g.Key.ID,
                            UserName = g.Key.User.UserName,
                            DisplayName = g.Key.User.DisplayName,
                            UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + g.Key.User.UserProfile.MainImage,
                            IsBoss = g.Key.IsBoss,
                            HasJoined = g.Key.JoinDateTime.HasValue,
                            HasPicked = g.Key.PickupDateTime.HasValue,
                            TotalCamelsCount = g.Select(a => a.Group).SelectMany(a => a.CamelCompetitions).Where(cc => cc.CompetitionID == competitionID).Count(),
                            CamelsEvaluatedCount = g.Select(a => a.Group).SelectMany(a => a.CamelCompetitions)
                                                    .Where(a => a.CompetitionID == competitionID)
                                                   .Where(cc => cc.RefereeReviews.Any(p => p.CompetitionRefereeID == g.Key.ID))
                                                    .Count(),
                            AssignedGroups = g.Select(a => a.Group).Count()
                        })
                        .ToList();

            var totalNumberOfAllCamels = refereesInCompetition.Select(x => x.TotalCamelsCount).ToList().Sum();
            var totalNumberOfEvaluatedCamels = refereesInCompetition.Select(x => x.CamelsEvaluatedCount).ToList().Sum();
            var refereeingCompletitionRatio = (totalNumberOfEvaluatedCamels * 100) / totalNumberOfAllCamels;

            var result = new RefereesReportViewModel
            {
                TotalNumberOfAllCamels = totalNumberOfAllCamels,
                TotalNumberOfEvaluatedCamels = totalNumberOfEvaluatedCamels,
                CheckingCompletitionRatio = refereeingCompletitionRatio,
                Referees = refereesInCompetition
            };

            //check if the loggeduser is boss
            if (!_repo.GetAll().Where(x => x.UserID == loggedUserID && x.CompetitionID == competitionID && x.IsBoss && !x.IsDeleted).Any())
            {
                throw new Exception("the logged user is not boss Referee for this competition");
            }
            refereesInCompetition.ForEach(c => {
                c.CompletionRatio = ((c.CamelsEvaluatedCount / c.TotalCamelsCount) * 100);
            });
            return result;
            //   return data.Checkers.ToList();
        }

        public RefereesReportViewModel GetTeam(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            if (!_groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.RefereeAllocates)
                        .Where(a => a.CompetitionReferee.CompetitionID == viewModel.CompetitionID)
                        .Any())
            {
                var data = _competitionRepository.GetAll()
                   .Where(x => !x.IsDeleted)
                   .Where(x => x.ID == viewModel.CompetitionID)
                   .Select(x => new
                   {
                       CamelsCount = x.CamelCompetitions.Where(c => c.CompetitionID == viewModel.CompetitionID)
                                    .Where(c => c.ApprovedByCheckerBossDateTime != null)
                                    .Count(),
                       Referees = x.CompetitionReferees.Where(c => !c.IsBoss).Select(c => new CompetitionRefereeViewModel
                       {
                           ID = c.ID,
                           UserName = c.User.UserName,
                           DisplayName = c.User.DisplayName,
                           UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage,
                           IsBoss = c.IsBoss,
                           HasJoined = c.JoinDateTime.HasValue,
                           HasPicked = c.PickupDateTime.HasValue,
                           CamelsEvaluatedCount = c.Competition.CamelCompetitions.Where(cc => cc.RefereeReviews.Where(a => a.CompetitionRefereeID == c.ID).Any())
                                                 .Count(),
                           AssignedCamels = c.Allocates.Select(a => a.Group).Select(g => g.CamelGroups).Count()
                       }).ToList(),
                       RefereeBoss = x.CompetitionReferees
                                            .Where(c => !c.IsDeleted)
                                            .Where(c => c.IsBoss)
                                            .Where(c => c.ChangeByOwnerDateTime == null)
                                            .Select(c => new { c.ID, c.UserID })
                                            .FirstOrDefault()


                   })
                   .FirstOrDefault();

                //check if the loggeduser is boss
                if (!_repo.GetAll().Where(x => x.UserID == viewModel.UserID && x.CompetitionID == viewModel.CompetitionID && x.IsBoss && !x.IsDeleted).Any())
                {
                    throw new Exception("the logged user is not boss checker for this competition");
                }
                var res = new RefereesReportViewModel
                {
                    TotalNumberOfAllCamels = data.CamelsCount,
                    TotalNumberOfEvaluatedCamels = 0,
                    CheckingCompletitionRatio = 0,
                    Referees = data.Referees.ToList()
                };


                return res;
            }

            if (!_camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == viewModel.CompetitionID)
                .All(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null))
            {
                throw new Exception("Checking is not completed");
            }
            var refereesInCompetition =
                _groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.RefereeAllocates)
                        .Where(a => a.CompetitionReferee.CompetitionID == viewModel.CompetitionID)
                        .GroupBy(x => x.CompetitionReferee)
                        .Select(g => new CompetitionRefereeViewModel
                        {
                            ID = g.Key.ID,
                            UserName = g.Key.User.UserName,
                            DisplayName = g.Key.User.DisplayName,
                            UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + g.Key.User.UserProfile.MainImage,
                            IsBoss = g.Key.IsBoss,
                            HasJoined = g.Key.JoinDateTime.HasValue,
                            HasPicked = g.Key.PickupDateTime.HasValue,
                            TotalCamelsCount = g.Select(a => a.Group).SelectMany(a => a.CamelCompetitions).Where(cc => cc.CompetitionID == viewModel.CompetitionID).Count(),
                            CamelsEvaluatedCount = g.Select(a => a.Group).SelectMany(a => a.CamelCompetitions)
                                                    .Where(a => a.CompetitionID == viewModel.CompetitionID)
                                                   .Where(cc => cc.RefereeReviews.Any(p => p.CompetitionRefereeID == g.Key.ID))
                                                    .Count(),
                            AssignedGroups = g.Select(a => a.Group).Count()
                        })
                        .ToList();

            var totalNumberOfAllCamels = refereesInCompetition.Select(x => x.TotalCamelsCount).ToList().Sum();
            var totalNumberOfEvaluatedCamels = refereesInCompetition.Select(x => x.CamelsEvaluatedCount).ToList().Sum();
            var refereeingCompletitionRatio = (totalNumberOfEvaluatedCamels * 100) / totalNumberOfAllCamels;

            var result = new RefereesReportViewModel
            {
                TotalNumberOfAllCamels = totalNumberOfAllCamels,
                TotalNumberOfEvaluatedCamels = totalNumberOfEvaluatedCamels,
                CheckingCompletitionRatio = refereeingCompletitionRatio,
                Referees = refereesInCompetition
            };

            //check if the loggeduser is boss
            if (!_repo.GetAll().Where(x => x.UserID == viewModel.UserID && x.CompetitionID == viewModel.CompetitionID && x.IsBoss && !x.IsDeleted).Any())
            {
                throw new Exception("the logged user is not boss Referee for this competition");
            }
            refereesInCompetition.ForEach(c => {
                c.CompletionRatio = ((c.CamelsEvaluatedCount / c.TotalCamelsCount) * 100);
            });
            return result;
            //   return data.Checkers.ToList();
        }

        //public List<CompetitionCheckerViewModel> GetTeam(CheckerJoinCompetitionCreateViewModel viewModel)
        //{
        //    string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
        //    string hostName = HttpContext.Current.Request.Url.Authority.ToString();

        //    var data = _competitionRepository.GetAll()
        //               .Where(x => !x.IsDeleted)
        //               .Where(x => x.ID == viewModel.CompetitionID)
        //               .Select(x => new
        //               {
        //                   Referees = x.CompetitionReferees.Where(c=>!c.IsBoss).Select(c => new CompetitionCheckerViewModel
        //                   {
        //                       ID = c.ID,
        //                       UserName = c.User.UserName,
        //                       UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage,
        //                       IsBoss = c.IsBoss,
        //                       HasJoined = c.JoinDateTime.HasValue,
        //                       HasPicked = c.PickupDateTime.HasValue
        //                   }).ToList(),
        //                   CheckerReferee = x.CompetitionReferees
        //                                        .Where(c => !c.IsDeleted)
        //                                        .Where(c => c.IsBoss)
        //                                        .Where(c => c.ChangeByOwnerDateTime == null)
        //                                        .Select(c => new { c.ID, c.UserID })
        //                                        .FirstOrDefault()


        //               })
        //               .FirstOrDefault();

        //    //check if the loggeduser is boss
        //    if (data.CheckerReferee.UserID != viewModel.UserID)
        //    {
        //        throw new Exception("the logged user is not boss referee for this competition");
        //    }
        //    return data.Referees.ToList();
        //}
        public bool RejectCompetition(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            var referee = _repo.GetAll()
                    .Where(x => x.CompetitionID == viewModel.CompetitionID && x.UserID == viewModel.UserID)
                    .FirstOrDefault();
            if (referee.JoinDateTime.HasValue)
            {
                throw new Exception("You joined , you can not reject ");
            }
            else
            {
                referee.RejectDateTime = DateTime.UtcNow;
                _unit.Save();
            }
            return true;
        }

        public bool ChangeReferee(ChangeRefereeCreateViewModel viewModel)
        {
            var oldAllocate =
            _competitionRefereeAllocateRepository.GetAll()
                .Where(x => x.GroupID == viewModel.GroupID)
                .Where(x => x.CompetitionRefereeID == viewModel.OldRefereeID)
                .FirstOrDefault();
            oldAllocate.IsReplaced = true;

            _competitionRefereeAllocateRepository.Add(new CompetitionRefereeAllocate
            {
                GroupID = viewModel.GroupID,
                CompetitionRefereeID = viewModel.NewRefereeID
            });

            _unit.Save();
            return true;
        }

        public bool SendCompetitionResult(SendCompetitionResultCreateViewModel viewModel)
        {
            //check all groups are Approved
            var approvedGroupsFromChecking = 
                _camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == viewModel.CompetitionID)
                .Where(x => x.ApprovedByCheckerBossDateTime != null)
                .Count();
           
            var approvedGroupsFromRefereeing =
                _camelCompetitionRepository.GetAll()
                .Where(x => x.CompetitionID == viewModel.CompetitionID)
                .Where(x => x.ApprovedByRefereeBossDateTime != null)
                .Count();
            if(approvedGroupsFromChecking != approvedGroupsFromRefereeing)
            {
                throw new Exception("not all groups are approved");
            }
            //mark competition as Completed
            var competition = _competitionRepository.GetById(viewModel.CompetitionID);
            competition.Completed = DateTime.UtcNow;
            _unit.Save();
            return true;
        }

    }

    public class OverallGroupRefereeViewModel
    {
        public int CompletePercentage { get; set; }
        public List<CamelCompetitionInfoRefereeViewModel> Camels { get; set; }
    }

    public class OverallGroupViewModel
    {
        public int CompletePercentage { get; set; }
        public List<CamelCompetitionBossRefereeViewModel> Camels { get; set; }
        public double GroupFinalScore { get; internal set; }
    }

    public class CamelCompetitionSpecificationBossViewModel
    {
        public int ID { get; set; }
        public string CamelName { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public IEnumerable<CompetitionRefereeViewModel> RefereeEvaluates { get; set; }
        public List<CompetitionRefereeViewModel> NotEvaluatedByReferees { get; set; }
        public List<CamelDocumentViewModel> CamelImages { get; set; }
        public List<CamelSpecificationViewModel> AverageSpecification { get; internal set; }
        public double TotalRefereeValue { get; set; }
    }

    public class CamelCompetitionInfoRefereeViewModel
    {
        public int ID { get; set; }
        public string CamelName { get; set; }
        public bool IsEvaluated { get; set; }
        public List<CamelDocumentViewModel> CamelImages { get; set; }
    }

    public class CamelCompetitionBossRefereeViewModel
    {
        public int ID { get; set; }
        public string CamelName { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        [IgnoreDataMember]
        public int RefereeEvaluatesCount { get; set; }
        [IgnoreDataMember]
        public int NotEvaluatedByRefereesCount { get; set; }
        public List<CamelDocumentViewModel> CamelImages { get; set; }
        public bool IsEvaluated { get; internal set; }
        [IgnoreDataMember]
        public double? FinalScore { get; internal set; }
    }

    public class CamelCompetitionRefereeViewModel
    {
        public int ID { get; set; }
        public string CamelName { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public DateTime? RejectedDateTime { get; set; }
        public IEnumerable<CompetitionRefereeViewModel> RefereeEvaluates { get; set; }
        public List<CompetitionRefereeViewModel> NotEvaluatedByReferees { get; set; }
        public List<CamelDocumentViewModel> CamelImages { get; set; }
        public List<CamelSpecificationViewModel> AverageSpecification { get; internal set; }
        public int CompletionPercentage { get; internal set; }
        public bool IsEvaluated { get; internal set; }
        [IgnoreDataMember]
        public double TotalValue { get; set; }
    }
   
}



