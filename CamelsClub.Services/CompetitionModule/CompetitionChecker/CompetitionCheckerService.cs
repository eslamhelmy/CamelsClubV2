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
    public class CompetitionCheckerService : ICompetitionCheckerService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionCheckerRepository _repo;
        private readonly ICompetitionInviteRepository _competitionInviteRepository;
        private readonly ICamelCompetitionRepository _camelCompetitionRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICheckerApproveRepository _checkerApproveRepository;
        private readonly ICompetitionAllocateRepository _competitionAllocateRepository;
        private readonly IReviewApproveRepository _reviewApproveRepository;
        private readonly IApprovedGroupRepository _approvedGroupRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly INotificationService _notificationService;


        public CompetitionCheckerService(IUnitOfWork unit, 
                                         ICompetitionCheckerRepository repo,
                                         IGroupRepository groupRepository,
                                         IReviewApproveRepository reviewApproveRepository,
                                         ICompetitionRepository competitionRepository,
                                         IApprovedGroupRepository approvedGroupRepository,
                                         ICamelCompetitionRepository camelCompetitionRepository,
                                         ICheckerApproveRepository checkerApproveRepository,
                                         ICompetitionAllocateRepository competitionAllocateRepository,
                                         INotificationService notificationService,
                                         ICompetitionInviteRepository competitionInviteRepository)
                                         
        {
            _unit = unit;
            _repo = repo;
            _reviewApproveRepository = reviewApproveRepository;
            _competitionInviteRepository = competitionInviteRepository;
            _approvedGroupRepository = approvedGroupRepository;
            _camelCompetitionRepository = camelCompetitionRepository;
            _competitionAllocateRepository = competitionAllocateRepository;
            _checkerApproveRepository = checkerApproveRepository;
            _competitionRepository = competitionRepository;
            _notificationService = notificationService;
            _groupRepository = groupRepository;

        }
        //get associated competitionCheckers with that user
        public PagingViewModel<CompetitionCheckerViewModel> Search(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
                .Where(x => x.Competition.UserID == userID);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionCheckerViewModel> result = new List<CompetitionCheckerViewModel>();

            var CompetitionCheckers = query.Select(obj => new CompetitionCheckerViewModel
            {
                ID = obj.ID,
            //    CompetitionID = obj.CompetitionID,
                UserName = obj.User.UserName

            }).OrderByPropertyName(orderBy, isAscending);

            result = CompetitionCheckers.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CompetitionCheckerViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(CompetitionCheckerCreateViewModel viewModel)
        {
           
            var insertedCompetitionChecker = _repo.Add(viewModel.ToModel());
            _unit.Save();

            var checker = _repo.GetAll().Where(ch => ch.ID == insertedCompetitionChecker.ID).FirstOrDefault();
            var notifcation = new NotificationCreateViewModel
            {
                ContentArabic = $"{checker.Competition.NameArabic} تم دعوتك للاشتراك بالمسابقة ",
                ContentEnglish = $"You have been joined to competition {checker.Competition.NameArabic}",
                NotificationTypeID = 6,
                SourceID = checker.Competition.UserID,
                DestinationID = viewModel.UserID,
                CompetitionID = viewModel.CompetitionID

            };

           _notificationService.SendNotifictionForUser(notifcation);

        }

        public void Edit(CompetitionCheckerCreateViewModel viewModel)
        {
          
            _repo.Edit(viewModel.ToModel());

        }

        public CompetitionCheckerViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var CompetitionChecker = _repo.GetAll().Where(comp => comp.ID == id)
                .Select(obj => new CompetitionCheckerViewModel
                {
                    ID = obj.ID,
              //      CompetitionID = obj.CompetitionID,
                    UserName = obj.User.UserName
                }).FirstOrDefault();

            return CompetitionChecker;
        }
        public bool RejectCompetition(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            CompetitionChecker checker = _repo.GetAll()
                    .Where(x => x.CompetitionID == viewModel.CompetitionID && x.UserID == viewModel.UserID)
                    .FirstOrDefault();
            if (checker.JoinDateTime.HasValue)
            {
                throw new Exception("You joined , you can not reject ");
            }
            else
            {
                checker.RejectDateTime = DateTime.UtcNow;
                _unit.Save();
            }
            return true;
        }
        public bool HasJoinedCompetition(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            return _repo.GetAll().Where(x => x.CompetitionID == viewModel.CompetitionID && x.UserID == viewModel.UserID && x.JoinDateTime.HasValue)
                     .Any();
        }
        public bool JoinCompetition(CheckerJoinCompetitionCreateViewModel viewModel)
        {
                var checker =
                   _repo.GetAll()
                   .Where(x => x.CompetitionID == viewModel.CompetitionID && !x.IsDeleted)
                   .Where(x => x.UserID == viewModel.UserID)
                   .FirstOrDefault();

                if (!checker.IsBoss && checker.PickupDateTime == null)
                {
                    throw new Exception("You must picked up by boss first");
                }

                if (checker == null)
                {
                    throw new Exception("this user is not a checker, your you don't have permission to do that");
                }

                checker.JoinDateTime = DateTime.UtcNow;
                _unit.Save();
            
                return true;
        }
       
        public bool PickupTeam(List<CompetitionCheckerPickupViewModel> viewModels,int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var IDs = viewModels.Select(x => x.ID).ToList();
           
            var data =
                    _repo.GetAll().Where(x => IDs.Contains(x.ID)).Select(x => new { 
                        Checker = x,
                        Competition = x.Competition
                    }).ToList();
            if(IDs.Count != data.Select(x => x.Competition).FirstOrDefault().MinimumCheckersCount)
            {
                return false;
            }
            data.Select(x=>x.Checker).ToList().ForEach(x => x.PickupDateTime = DateTime.UtcNow);
            _competitionRepository.SaveIncluded(new Competition { ID = data.FirstOrDefault().Competition.ID, CheckerPickupTeamDateTime = DateTime.UtcNow }, "CheckerPickupTeamDateTime");
            _unit.Save();
            //send notification to picked checkers
            var notification = new NotificationCreateViewModel
            {
                ContentArabic = $"{NotificationArabicKeys.NewCompetitionAnnounceToChecker} {data.FirstOrDefault().Competition.NameArabic}",
                ContentEnglish = $"{NotificationEnglishKeys.NewCompetitionAnnounceToReferee} {data.FirstOrDefault().Competition.NamEnglish}",
                NotificationTypeID = (int)TypeOfNotification.CheckerRequestForJoinCompetition,
                ArbNotificationType = "الانضمام الي لجنة تمييز المسايقة",
                EngNotificationType = "Join the competition as checker",
                SourceID = loggedUserID,
                //   SourceName = insertedCompetition.User.Name,
                CompetitionID = data.FirstOrDefault().Competition.ID,
                CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.FirstOrDefault().Competition.Image,
              
            };
            Task.Run(() => {
                _notificationService.SendNotifictionForCheckers(notification, data.Select(x => x.Checker).Select(c => new CompetitionCheckerCreateViewModel { UserID = c.UserID }).ToList());
            });
            return true;
        }
        public CheckersReportViewModel GetPickedCheckers(int competitionID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            if (!_groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.Allocates)
                        .Where(a => a.CompetitionChecker.CompetitionID == competitionID)
                        .Any())
            {
                var data = _competitionRepository.GetAll()
                   .Where(x => !x.IsDeleted)
                   .Where(x => x.ID == competitionID)
                   .Select(x => new
                   {
                       CamelsCount = x.CamelCompetitions.Where(c => c.CompetitionID == competitionID)
                                    .Where(c => c.CompetitionInvite.SubmitDateTime != null)
                                    .Count(),
                       Checkers = x.CompetitionCheckers.Where(c => c.PickupDateTime != null).Where(c => !c.IsBoss).Select(c => new CompetitionCheckerViewModel
                       {
                           ID = c.ID,
                           UserName = c.User.UserName,
                           DisplayName = c.User.DisplayName,
                           UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage,
                           IsBoss = c.IsBoss,
                           HasJoined = c.JoinDateTime.HasValue,
                           HasPicked = c.PickupDateTime.HasValue,
                           CamelsEvaluatedCount = c.Competition.CamelCompetitions.Where(cc => cc.CheckerApprovers.Where(a => a.CompetitionCheckerID == c.ID).Any())
                                                 .Count(),
                           AssignedCamels = c.Allocates.Select(a => a.Group).Select(g => g.CamelGroups).Count()
                       }).ToList(),
                       CheckerBoss = x.CompetitionCheckers
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
                var res = new CheckersReportViewModel
                {
                    TotalNumberOfAllCamels = data.CamelsCount,
                    TotalNumberOfEvaluatedCamels = 0,
                    CheckingCompletitionRatio = 0,
                    Checkers = data.Checkers.ToList()
                };


                return res;
            }


            var checkersInCompetition =
                _groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.Allocates)
                        .Where(a => a.CompetitionChecker.CompetitionID == competitionID)
                        .Where(a => !a.IsReplaced)
                        .GroupBy(x => x.CompetitionChecker)
                        .Select(g => new CompetitionCheckerViewModel
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
                                                   .Where(cc => cc.CheckerApprovers.Any(p => p.CompetitionCheckerID == g.Key.ID))
                                                    .Count(),
                            AssignedGroups = g.Select(a => a.Group).Count()
                        })
                        .ToList();

            var totalNumberOfAllCamels = checkersInCompetition.Select(x => x.TotalCamelsCount).ToList().Sum();
            var totalNumberOfEvaluatedCamels = checkersInCompetition.Select(x => x.CamelsEvaluatedCount).ToList().Sum();
            var checkingCompletitionRatio = (totalNumberOfEvaluatedCamels * 100) / totalNumberOfAllCamels;

            var result = new CheckersReportViewModel
            {
                TotalNumberOfAllCamels = totalNumberOfAllCamels,
                TotalNumberOfEvaluatedCamels = totalNumberOfEvaluatedCamels,
                CheckingCompletitionRatio = checkingCompletitionRatio,
                Checkers = checkersInCompetition
            };

            //check if the loggeduser is boss
            if (!_repo.GetAll().Where(x => x.UserID == loggedUserID && x.CompetitionID == competitionID && x.IsBoss && !x.IsDeleted).Any())
            {
                throw new Exception("the logged user is not boss checker for this competition");
            }
            checkersInCompetition.ForEach(c => {
                c.CompletionRatio = ((c.CamelsEvaluatedCount / c.TotalCamelsCount) * 100);
            });
            return result;

        }

        public CheckersReportViewModel GetTeam(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            if(!_groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.Allocates)
                        .Where(a => a.CompetitionChecker.CompetitionID == viewModel.CompetitionID)
                        .Any())
            {
                var data = _competitionRepository.GetAll()
                   .Where(x => !x.IsDeleted)
                   .Where(x => x.ID == viewModel.CompetitionID)
                   .Select(x => new
                   {
                       CamelsCount = x.CamelCompetitions.Where(c=>c.CompetitionID == viewModel.CompetitionID)
                                    .Where(c=>c.CompetitionInvite.SubmitDateTime != null)
                                    .Count(),
                       Checkers = x.CompetitionCheckers.Where(c => !c.IsBoss).Select(c => new CompetitionCheckerViewModel
                       {
                           ID = c.ID,
                           UserName = c.User.UserName,
                           DisplayName = c.User.DisplayName,
                           UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage,
                           IsBoss = c.IsBoss,
                           HasJoined = c.JoinDateTime.HasValue,
                           HasPicked = c.PickupDateTime.HasValue,
                           CamelsEvaluatedCount = c.Competition.CamelCompetitions.Where(cc => cc.CheckerApprovers.Where(a => a.CompetitionCheckerID == c.ID).Any())
                                                 .Count(),
                           AssignedCamels = c.Allocates.Select(a => a.Group).Select(g => g.CamelGroups).Count()
                       }).ToList(),
                       CheckerBoss = x.CompetitionCheckers
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
                var res = new CheckersReportViewModel
                {
                    TotalNumberOfAllCamels = data.CamelsCount,
                    TotalNumberOfEvaluatedCamels = 0,
                    CheckingCompletitionRatio = 0,
                    Checkers = data.Checkers.ToList()
                };


                return res;
            }


            var checkersInCompetition = 
                _groupRepository.GetAll()
                        .Where(x => !x.IsDeleted)
                        .SelectMany(g => g.Allocates)
                        .Where(a=> a.CompetitionChecker.CompetitionID == viewModel.CompetitionID)
                        .Where(a=> !a.IsReplaced)
                        .GroupBy(x => x.CompetitionChecker)
                        .Select(g => new CompetitionCheckerViewModel
                        {
                            ID = g.Key.ID,
                            UserName = g.Key.User.UserName,
                            DisplayName = g.Key.User.DisplayName,
                            UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + g.Key.User.UserProfile.MainImage,
                            IsBoss = g.Key.IsBoss,
                            HasJoined = g.Key.JoinDateTime.HasValue,
                            HasPicked = g.Key.PickupDateTime.HasValue,
                            TotalCamelsCount = g.Select(a => a.Group).SelectMany(a => a.CamelCompetitions).Where(cc=>cc.CompetitionID == viewModel.CompetitionID).Count(),
                            CamelsEvaluatedCount = g.Select(a => a.Group).SelectMany(a => a.CamelCompetitions)
                                                    .Where(a => a.CompetitionID == viewModel.CompetitionID)
                                                   .Where(cc => cc.CheckerApprovers.Any(p => p.CompetitionCheckerID == g.Key.ID))
                                                    .Count(),
                            AssignedGroups = g.Select(a => a.Group).Count()
                        })
                        .ToList();

            var totalNumberOfAllCamels = checkersInCompetition.Select(x => x.TotalCamelsCount).ToList().Sum();
            var totalNumberOfEvaluatedCamels = checkersInCompetition.Select(x => x.CamelsEvaluatedCount).ToList().Sum();
            var checkingCompletitionRatio = (totalNumberOfEvaluatedCamels * 100) / totalNumberOfAllCamels;

            var result = new CheckersReportViewModel
            {
                TotalNumberOfAllCamels = totalNumberOfAllCamels,
                TotalNumberOfEvaluatedCamels = totalNumberOfEvaluatedCamels,
                CheckingCompletitionRatio = checkingCompletitionRatio,
                Checkers = checkersInCompetition
            };

            //check if the loggeduser is boss
            if (!_repo.GetAll().Where(x=>x.UserID == viewModel.UserID && x.CompetitionID == viewModel.CompetitionID && x.IsBoss && !x.IsDeleted).Any() )
            {
                throw new Exception("the logged user is not boss checker for this competition");
            }
            checkersInCompetition.ForEach(c => {
                    c.CompletionRatio = ((c.CamelsEvaluatedCount / c.TotalCamelsCount) * 100);
            });
            return result;
         //   return data.Checkers.ToList();
        }

        //public bool AutoAllocate(CheckerJoinCompetitionCreateViewModel viewModel)
        //{
        //    //get all groups of all competitors
        //    var data = _competitionRepository.GetAll()
        //                            .Where(x => x.ID == viewModel.CompetitionID)
        //                            .Where(x => !x.IsDeleted)
        //                            .Select(x => new
        //                            {
        //                                StartedDate = x.StartedDate,
        //                                JoinedGroupIDs = x.CamelCompetitions
        //                                    .Where(c => c.CompetitionInvite.SubmitDateTime.HasValue)
        //                                    .Where(c => !c.IsDeleted)
        //                                    .Select(c => c.GroupID)
        //                                    .Distinct()
        //                                    .ToList(),
        //                                JoinedCheckerIDs = x.CompetitionCheckers
        //                                                    .Where(c=> !c.IsDeleted)
        //                                                    .Where(c=>c.JoinDateTime.HasValue)
        //                                                    .Where(c=>c.PickupDateTime.HasValue)
        //                                                    .Select(c=>c.ID)
        //                                                    .ToList()

        //                            }).FirstOrDefault();
        //    if (!data.StartedDate.HasValue)
        //    {
        //        throw new Exception("competition not started yet");
        //    }               
        //    //add list to store all assigning
        //    List<CompetitionAllocate> Allocates = new List<CompetitionAllocate>();
        //    //assign each group to a checker
        //    //init values 
        //    var checkersCount = data.JoinedCheckerIDs.Count;
        //    int limit = 0;
        //    if(checkersCount%2 == 0)
        //    {
        //        limit = 2;
        //    }
        //    else
        //    {
        //        limit = 3;
        //    }
        //    int i = 0;
        //    int z = 0;
        //    foreach (var groupID in data.JoinedGroupIDs)
        //    {
        //        while (i <= data.JoinedCheckerIDs.Count-1 && z< limit)
        //        {
        //            Allocates.Add(new CompetitionAllocate
        //            {
        //                GroupID = groupID,
        //                CompetitionCheckerID = data.JoinedCheckerIDs[i]
        //            });
        //            z++;
        //            i++;

        //            if (i == (checkersCount - 1))
        //            {
        //                i = 0;
        //            }

        //        }
        //        z = 0;
               


        //    }
        //    //save them in DB
        //    _competitionAllocateRepository.AddRange(Allocates);
        //    _unit.Save();
        //    return true;
        //}
        public bool AutoAllocate(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            //get all groups of all competitors
            var data = _competitionRepository.GetAll()
                                    .Where(x => x.ID == viewModel.CompetitionID)
                                    .Where(x => !x.IsDeleted)
                                    .Select(x => new
                                    {
                                        CheckingAllocationDate = x.CheckersAllocatedDate,
                                        Competition = x,
                                        StartedDate = x.StartedDate,
                                        JoinedGroupIDs = x.CamelCompetitions
                                            .Where(c => c.CompetitionInvite.SubmitDateTime.HasValue)
                                            .Where(c => !c.IsDeleted)
                                            .Select(c => c.GroupID)
                                            .Distinct()
                                            .ToList(),
                                        JoinedCheckerIDs = x.CompetitionCheckers
                                                            .Where(c => !c.IsDeleted)
                                                            .Where(c => c.JoinDateTime.HasValue)
                                                            .Where(c => c.PickupDateTime.HasValue)
                                                            .Select(c => c.ID)
                                                            .ToList()

                                    }).FirstOrDefault();
            
            if (data.CheckingAllocationDate != null)
            {
                throw new Exception("Competition is already allocated");
            }
            
            if (data.Competition.CheckersAllocatedDate.HasValue)
            {
                throw new Exception("competition is already allocated");
            }
            if (!data.StartedDate.HasValue)
            {
                throw new Exception("competition not started yet");
            }
            //add list to store all assigning
            List<CompetitionAllocate> Allocates = new List<CompetitionAllocate>();
            //assign each group to a checker
            //init values 
            var checkersCount = data.JoinedCheckerIDs.Count;
            int i = 0;
            foreach (var groupID in data.JoinedGroupIDs)
            {
               
                    Allocates.Add(new CompetitionAllocate
                    {
                        GroupID = groupID,
                        CompetitionCheckerID = data.JoinedCheckerIDs[i]
                    });
                    i++;
                    if (i == checkersCount )
                    {
                        i = 0;
                    }


            }
            //save them in DB
            _competitionAllocateRepository.AddRange(Allocates);
            //update competition 
            data.Competition.CheckersAllocatedDate = DateTime.UtcNow;
            _unit.Save();
            return true;
        }
        public bool ManualAllocate(List<ManualAllocateCreateViewModel> viewModels)
        {
            var checkerID = viewModels[0].CheckerID;
            var competition = _repo.GetAll().Where(x => x.ID == checkerID).Select(x => x.Competition).FirstOrDefault();
            if (competition.CheckersAllocatedDate.HasValue)
            {
                throw new Exception("competition is already allocated");
            }
            if (!competition.StartedDate.HasValue)
                throw new Exception("competition is not started yet");
            foreach (var item in viewModels)
            {
               var existedBefore = 
                    _competitionAllocateRepository.GetAll()
                         .Where(x => x.GroupID == item.GroupID && x.CompetitionCheckerID == item.CheckerID && !x.IsDeleted).Any();
                if (existedBefore)
                    throw new Exception($"you assigned this group {item.GroupID} with this checker {item.CheckerID} before");

            }
            _competitionAllocateRepository.AddRange(viewModels.Select(x => x.ToModel()).ToList());
            //update competition to be CheckersAllocated
            competition.CheckersAllocatedDate = DateTime.UtcNow;
            _unit.Save();
            return true;
        }

        //for logged checker
        public List<GroupViewModel> GetGroups(int competitionID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            //check if logged user is boss
            var boss = 
            _competitionRepository.GetAll()
                .Where(x => x.ID == competitionID)
                .SelectMany(x => x.CompetitionCheckers)
                .FirstOrDefault(x => x.IsBoss);
            if(boss.UserID == loggedUserID)
            {
                
                if(!_competitionAllocateRepository.GetAll()
                .Where(x => x.CompetitionChecker.CompetitionID == competitionID)
                .Any()
                )
                {
                    return _camelCompetitionRepository.GetAll()
                           .Where(x => x.CompetitionID == competitionID)
                           .GroupBy(x => x.Group)
                           .Select(x => new GroupViewModel
                           {
                               ID = x.Key.ID,
                               NameArabic = x.Key.NameArabic,
                               NameEnglish = x.Key.NameEnglish,
                               CamelsCountInGroup = x.Key.CamelGroups.Count(),
                               ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.Key.Image

                           }).ToList();
                }
                    var groups = 
                _competitionAllocateRepository.GetAll()
                .Where(x => x.CompetitionChecker.CompetitionID == competitionID)
                .Where(x => x.IsReplaced == false)
                .Select(x => new GroupViewModel
                {
                    ID = x.Group.ID,
                    NameArabic = x.Group.NameArabic,
                    NameEnglish = x.Group.NameEnglish,
                    CamelsCountInGroup = x.Group.CamelGroups.Count(),
                    ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.Group.Image,
                    IsGroupApproved = x.Group.CamelCompetitions.Where(cc=>cc.CompetitionID ==competitionID).All(c=>c.ApprovedByCheckerBossDateTime != null && !c.IsDeleted),
                    IsGroupRejected = x.Group.CamelCompetitions.Where(cc => cc.CompetitionID == competitionID).All(c=>c.RejectedByCheckerBossDateTime != null && !c.IsDeleted),
                    AssignedCheckers = x.Group.Allocates.Where(all=>all.CompetitionChecker.CompetitionID == competitionID)
                        .Select(a=>new CompetitionCheckerViewModel
                        {
                            ID = a.CompetitionCheckerID,
                            CamelsIApproved = a.CompetitionChecker.CamelsIApproved//.Select(a=).ToList()
                                                    .Where(ca => ca.CamelCompetition.GroupID == x.GroupID)
                                                    .Where(ca=> ca.CompetitionChecker.CompetitionID == competitionID)
                                                    .Count(),

                            UserImage = protocol + "://" + hostName + "/uploads/User-Document/"+ a.CompetitionChecker.User.UserProfile.MainImage,
                            UserName = a.CompetitionChecker.User.UserName,
                        }).ToList()
                }).ToList().Distinct(new GroupComparer()).ToList();

                groups.ForEach(x => x.AssignedCheckers.ForEach(c =>
                {
                    if (c.CamelsIApproved == x.CamelsCountInGroup)
                    {
                        c.HasFinisedRating = true;
                    }
                }));
                return groups;

            }
            return
            _competitionAllocateRepository.GetAll()
                .Where(x => x.CompetitionChecker.UserID == loggedUserID)
                .Where(x => x.CompetitionChecker.CompetitionID == competitionID)
                .Where(x => x.IsReplaced == false)
                .Select(x => new GroupViewModel
                {
                    ID = x.Group.ID,
                    NameArabic = x.Group.NameArabic,
                    NameEnglish = x.Group.NameEnglish,
                    ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.Group.Image,
                    IsCheckerFinishedRating = x.Group.CamelCompetitions
                                                .Where(c => c.CompetitionID == competitionID && c.GroupID == x.GroupID)
                                                .All(c =>
                                                    c.CheckerApprovers
                                                    .Any(p => p.CompetitionChecker.UserID == loggedUserID 
                                                        && p.CompetitionChecker.CompetitionID == competitionID))
                }).ToList();

        }

        public List<CamelCompetitionViewModel> GetCamels(int competitionID,int groupID, int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            //check if logged user is boss
            var boss =
            _competitionRepository.GetAll()
                .Where(x => x.ID == competitionID)
                .SelectMany(x => x.CompetitionCheckers)
                .FirstOrDefault(x => x.IsBoss);
            if (boss.UserID == loggedUserID)
            {
                return
               _camelCompetitionRepository.GetAll()
               .Where(x => x.GroupID == groupID)
               .Where(x => x.CompetitionID == competitionID)
               .Select(x => new CamelCompetitionViewModel
               {
                   ID = x.ID,
                   CamelName = x.Camel.Name,
                   CheckerEvaluates = x.CheckerApprovers.Where(a=>a.CompetitionChecker.Allocates.Any(all=>all.GroupID == x.GroupID && !all.IsReplaced)).Select(a => new CheckerApproveViewModel
                   {
                       ID = a.CompetitionCheckerID,
                       SubCheckerName = a.CompetitionChecker.User.UserName,
                       CheckerImage = protocol + "://" + hostName + "/uploads/User-Document/" + a.CompetitionChecker.User.UserProfile.MainImage,
                       Notes = a.Notes,
                       Status = a.Status,
                       ReviewRequests = a.Reviews.Where(r=> !r.IsDeleted)
                                            .Where(r=> r.Status == (int) ReviewApproveStatus.Pending)
                                            .Select(r => new ReviewApproveViewModel
                                            {
                                                CheckerID = r.CheckerID,
                                                CheckerName = r.CompetitionChecker.User.UserName,
                                                NewNotes = r.Notes

                                            }).ToList(),
                       Reviews = a.Reviews.Where(r => !r.IsDeleted)
                                            .Where(r => r.Status == (int)ReviewApproveStatus.Reviewed)
                                            .Select(r => new ReviewApproveViewModel
                                            {
                                                CheckerID = r.CheckerID,
                                                CheckerName = r.CompetitionChecker.User.UserName,
                                                NewNotes = r.Notes
                                            }).ToList()
                   }).ToList(),
                   NotEvaluatedByCheckers = x.Group.Allocates.Where(a=> !a.IsReplaced && a.CompetitionChecker.CamelsIApproved.All(p=>p.CamelCompetitionID != x.ID && !p.IsDeleted))
                                        .Select(a=> new CompetitionCheckerViewModel
                                        {
                                            ID = a.CompetitionCheckerID,
                                            UserName = a.CompetitionChecker.User.UserName,
                                            CheckerImage = protocol + "://" + hostName + "/uploads/User-Document/" + a.CompetitionChecker.User.UserProfile.MainImage,

                                        }).ToList(),
                   CamelImages = x.Camel.CamelDocuments.Select(d => new CamelDocumentViewModel
                   {
                       FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + d.FileName
                   }).ToList()

               }).ToList();

            }
            else
            {

                return
                _camelCompetitionRepository.GetAll()
                .Where(x => x.GroupID == groupID)
                .Where(x => x.CompetitionID == competitionID)
                .Select(x => new CamelCompetitionViewModel
                {
                    ID = x.ID,
                    CamelName = x.Camel.Name,
                    IsEvaluated = x.CheckerApprovers.Any(c => c.CompetitionChecker.UserID == loggedUserID && !c.IsDeleted),
                    EvaluateStatus = x.CheckerApprovers.Where(c => c.CompetitionChecker.UserID == loggedUserID && !c.IsDeleted).Select(a=>a.Status).FirstOrDefault(),
                    CamelImages = x.Camel.CamelDocuments.Select(d => new CamelDocumentViewModel
                    {
                        FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + d.FileName
                    }).ToList()

                }).ToList();
            }
        }

        public bool EvaluateCamel(CheckerApproveCreateViewModel viewModel)
        {
            if (IsCamelEvaluated(viewModel.UserID, viewModel.ID))
            {
                throw new Exception("you already evaluated this camel before");
            }
            //get competition from camel competition
            var competitionID = _camelCompetitionRepository.GetAll().Where(x => x.ID == viewModel.ID).Select(x => x.CompetitionID).FirstOrDefault();
            //get competitionChecker and check if not boss
            var checker = _repo.GetAll().Where(x => x.UserID == viewModel.UserID && x.CompetitionID == competitionID)
                            .Where(x => !x.IsBoss && !x.IsDeleted)
                            .FirstOrDefault();

            viewModel.CompetitionCheckerID = checker.ID;

            _checkerApproveRepository.Add(viewModel.ToModel());
            _unit.Save();
            return true;
        }

        public bool ApproveGroup(ApproveGroupCreateViewModel viewModel)
        {
            //get competition
            var competition = _competitionRepository.GetById(viewModel.CompetitionID);
            //get number of approved groups and compare to maximum allowed number in case of public competition
            var data = _camelCompetitionRepository.GetAll()
                 .Where(x => x.CompetitionID == viewModel.CompetitionID)
                 .Where(x => x.ApprovedByCheckerBossDateTime != null)
                 .GroupBy(x => x.CompetitionInviteID)
                 .Select(g => new
                 {
                    ID =  g.Key,
                  //  CompetitionType = g.Select(cc=>cc.Competition).FirstOrDefault().CompetitionType,
                  //  AllowedCompetitors = g.Select(cc => cc.Competition).FirstOrDefault().MaximumCompetitorsCount,
                    Count = g.Count()
                 }).ToList();

            if(competition.CompetitionType == (int)CompetitionType.PublicForAll)
            {
                if(data != null && data.Count == competition.MaximumCompetitorsCount)
                {
                    throw new Exception("number of approved competitors match the Maximum Allowed Number, you are not allowed to approve ramaining groups");

                }
            }
            var camelsCompetitions = 
            _camelCompetitionRepository.GetAll()
                .Where(x => x.GroupID == viewModel.GroupID && x.CompetitionID == viewModel.CompetitionID)
                .ToList();
            var timeNow = DateTime.UtcNow;
            camelsCompetitions.ForEach(x => x.ApprovedByCheckerBossDateTime = timeNow);
            _unit.Save();
            //reject other groups
            var newCount = _camelCompetitionRepository.GetAll()
                                .Where(x => x.CompetitionID == viewModel.CompetitionID)
                                .Where(x => x.ApprovedByCheckerBossDateTime == null)
                                .GroupBy(x => x.GroupID)
                                .Select(g => new
                                {
                                    Total = g.Count()
                                })
                                .FirstOrDefault();
            
            if ( newCount.Total == competition.MaximumCompetitorsCount)
            {
                var notApprovedGroups =
              _camelCompetitionRepository.GetAll()
              .Where(x => x.GroupID == viewModel.GroupID && x.CompetitionID == viewModel.CompetitionID && x.ApprovedByCheckerBossDateTime == null)
              .ToList();
                notApprovedGroups.ForEach(x => x.RejectedByCheckerBossDateTime = DateTime.UtcNow);
                _unit.Save();

            }

            return true;


        }

        public bool ReviewApprove(ReviewApproveRequestCreateViewModel viewModel, int loggedUserID)
        {
            if(!_checkerApproveRepository.GetAll()
                .Where(x=> x.ID == viewModel.CheckerApproveID)
                .SelectMany(x=> x.CamelCompetition.Competition.CompetitionCheckers)
                .Where(x=>x.UserID == loggedUserID && x.IsBoss).Any())
            {
                throw new Exception("You can not review approve without being logged by boss");
            }
            _reviewApproveRepository.Add(new ReviewApprove
            {
                CheckerApproveID = viewModel.CheckerApproveID,
                CheckerID = viewModel.CheckerID,
                Status = (int) ReviewApproveStatus.Pending
            });
            _unit.Save();
            return true;
        }
        public bool RejectGroup(ApproveGroupCreateViewModel viewModel)
        {
            //var isExist = _approvedGroupRepository.GetAll()
            //       .Where(x => x.GroupID == viewModel.GroupID && x.CompetitionID == viewModel.CompetitionID && !x.IsDeleted && x.Status == (int)ApprovedGroupStatus.Rejected).Any();
            //if (isExist)
            //    return true;

            //_approvedGroupRepository.Add(new ApprovedGroup
            //{
            //    GroupID = viewModel.GroupID,
            //    CompetitionID = viewModel.CompetitionID,
            //    Status = (int)ApprovedGroupStatus.Rejected
            //});
            //_unit.Save();

            var camelsCompetitions =
         _camelCompetitionRepository.GetAll()
             .Where(x => x.GroupID == viewModel.GroupID && x.CompetitionID == viewModel.CompetitionID)
             .ToList();
            var timeNow = DateTime.UtcNow;
            camelsCompetitions.ForEach(x => x.RejectedByCheckerBossDateTime = timeNow);
            _unit.Save();
            return true;

        }

        public bool ChangeChecker(ChangeCheckerCreateViewModel viewModel)
        {
            var oldAllocate =
            _competitionAllocateRepository.GetAll()
                .Where(x => x.GroupID == viewModel.GroupID)
                .Where(x => x.CompetitionCheckerID == viewModel.OldCheckerID)
                .FirstOrDefault();
            oldAllocate.IsReplaced = true;

            _competitionAllocateRepository.Add(new CompetitionAllocate
            {
                GroupID = viewModel.GroupID,
                CompetitionCheckerID = viewModel.NewCheckerID
            });

            _unit.Save();
            return true;
        }

        public bool IsCamelEvaluated(int userID, int camelCompetitionID)
        {
            return
            _checkerApproveRepository.GetAll()
                .Any(x => x.CompetitionChecker.UserID == userID && x.CamelCompetitionID == camelCompetitionID && !x.IsDeleted);
            
        }
        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }
        public void Delete(int id)
        {
                _repo.Remove(id);
        }

        public List<ReviewApproveViewModel> GetReviewRequests(int competitionID, int loggedUserID)
        {
            return
            _reviewApproveRepository.GetAll()
                .Where(x => x.CompetitionChecker.UserID == loggedUserID)
                .Where(x => x.Status == (int)ReviewApproveStatus.Pending)
                .Select(x => new ReviewApproveViewModel
                {
                    CheckerID = x.CheckerID,
                    CheckerName = x.CompetitionChecker.User.UserName,
                    OldNotes = x.CheckerApprove.Notes,
                    CheckerApproveID = x.CheckerApproveID
                }).ToList();
        }

        public bool AddApproveReview(ReviewApproveCreateViewModel viewModel, int loggedUserID)
        {
            //get request 
           var request =  _reviewApproveRepository.GetById(viewModel.ID);
            if(request.CompetitionChecker?.UserID != loggedUserID)
            {
                throw new Exception("you can not add review with this checker");
            }
            request.Notes = viewModel.Notes;
            request.Status = (int) ReviewApproveStatus.Reviewed;
            _unit.Save();
            return true;
        }

    }
    public class GroupComparer : IEqualityComparer<GroupViewModel>
    {
        public bool Equals(GroupViewModel x, GroupViewModel y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(GroupViewModel obj)
        {
            return 0;
        }
    }
    public class CheckersReportViewModel
    {
        public int TotalNumberOfAllCamels { get; set; }
        public int TotalNumberOfEvaluatedCamels { get; set; }
        public int CheckingCompletitionRatio { get; set; }
        public List<CompetitionCheckerViewModel> Checkers { get; set; }
    }

    public enum ApprovedGroupStatus
    {
        Approved = 1,
        Rejected
    }
}

