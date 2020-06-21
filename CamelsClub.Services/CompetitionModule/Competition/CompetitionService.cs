using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CamelsClub.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionRepository _repo;
        private readonly ICamelCompetitionRepository _competitonCamelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICompetitionInviteRepository _competitonInviteRepository;
        private readonly ICompetitionRefereeRepository _competitonRefereeRepository;
        private readonly ICompetitionRewardRepository _competitonRewardRepository;
        private readonly ICompetitionTeamRewardRepository _competitonTeamRewardRepository;
        private readonly ICompetitionConditionRepository _competitonConditionRepository;
        private readonly ICompetitionCheckerRepository _competitonCheckerRepository;
        private readonly ICompetitionSpecificationRepository _competitonSpecificationRepository;

        private readonly INotificationService _notificationService;
        public CompetitionService(IUnitOfWork unit,
                                  ICompetitionRepository repo ,
                                  IPostRepository postRepository,
                                  IUserRepository userRepository ,
                                  ICamelCompetitionRepository competitonCamelRepository,
                                  ICompetitionTeamRewardRepository competitonTeamRewardRepository,
                                  ICompetitionSpecificationRepository competitonSpecificationRepository,
                                  ICompetitionInviteRepository competitonInviteRepository,
                                  ICompetitionRefereeRepository competitonRefereeRepository,
                                  ICompetitionRewardRepository competitonRewardRepository,
                                  INotificationService notificationService,
                                  ICompetitionConditionRepository competitonConditionRepository,
                                  ICompetitionCheckerRepository competitonCheckerRepository
                                 )
        {
            _unit = unit;
            _repo = repo;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _competitonCamelRepository = competitonCamelRepository;
            _competitonSpecificationRepository = competitonSpecificationRepository;
            _competitonInviteRepository = competitonInviteRepository;
            _competitonRefereeRepository = competitonRefereeRepository;
            _competitonRewardRepository = competitonRewardRepository;
            _competitonTeamRewardRepository = competitonTeamRewardRepository;
            _competitonConditionRepository = competitonConditionRepository;
            _competitonCheckerRepository = competitonCheckerRepository;
            _notificationService = notificationService;
        }
        public PagingViewModel<CompetitionViewModel> Search(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
                .Where(x => userID == 0 || x.UserID == userID);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionViewModel> result = new List<CompetitionViewModel>();

            var competitions = query.Select(obj => new CompetitionViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic,
                NamEnglish = obj.NamEnglish,
                Address = obj.Address,
                CamelsCount = obj.CamelsCount,
                From = obj.From,
                To = obj.To,
                CategoryArabicName = obj.Category.NameArabic,
                CategoryEnglishName = obj.Category.NameEnglish,
                CategoryID = obj.CategoryID,
                UserName = obj.User.UserName,
                ImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + obj.Image,
                CompetitionType = obj.CompetitionType,
                //Camels = obj.CamelCompetitions.Where(c => !c.IsDeleted).Select(CamelComp => new CamelViewModel
                //{
                //    ID = CamelComp.CamelID,
                //    BirthDate = CamelComp.Camel.BirthDate,
                //    CategoryArabicName = CamelComp.Camel.Category.NameArabic,
                //    CategoryEnglishName = CamelComp.Camel.Category.NameEnglish,
                //    CategoryID = CamelComp.Camel.CategoryID,
                //    UserName = CamelComp.Camel.User.Name,
                //    Details = CamelComp.Camel.Details,
                //    FatherName = CamelComp.Camel.FatherName,
                //    Location = CamelComp.Camel.Location,
                //    MotherName = CamelComp.Camel.MotherName,
                //    Name = CamelComp.Camel.Name,
                //    GenderID = CamelComp.Camel.GenderConfigDetailID,
                //    GenderName = CamelComp.Camel.GenderConfigDetail.NameArabic,
                //    camelDocuments = CamelComp.Camel.CamelDocuments.Select(x=>new CamelDocumentViewModel
                //    {
                //        FilePath = 
                //           protocol + "://" + hostName + "/uploads/Camel-Document/" + obj.Image,
                //        FileType = x.Type
                        
                //    }).ToList()
                //}),
                Rewards = obj.CompetitionRewards.Where(x=>!x.IsDeleted).Select(x=>new CompetitionRewardViewModel
                {
                    ID = x.ID,
                    NameArabic = x.NameArabic,
                    NamEnglish = x.NameEnglish,
                    SponsorText = x.SponsorText
                }),
                
                Invites = obj.CompetitionInvites.Where(x => !x.IsDeleted).Select(x => new CompetitionInviteViewModel
                {
                    ID = x.ID,
                    UserName = x.User.UserName,
                    UserImage = obj.User.UserProfile.MainImage != null ?
                       protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage : "",

                }),
                Referees = obj.CompetitionReferees.Where(x => !x.IsDeleted).Select(x => new CompetitionRefereeViewModel
                {
                    ID = x.ID,
                    UserName = x.User.UserName,
                    UserImage = obj.User.UserProfile.MainImage != null ?
                       protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage : "",
                    IsBoss = x.IsBoss
                }),
                Checkers = obj.CompetitionCheckers.Where(x => !x.IsDeleted).Select(x => new CompetitionCheckerViewModel
                {
                    ID = x.ID,
                    UserName = x.User.UserName,
                    UserImage = obj.User.UserProfile.MainImage != null ?
                       protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage : "",
                    IsBoss = x.IsBoss
                }),
                Conditions = obj.CompetitionConditions.Where(x => !x.IsDeleted).Select(x => new CompetitionConditionViewModel
                {
                    ID = x.ID,
                    TextArabic = x.TextArabic,
                    TextEnglish = x.TextEnglish
                })

            }).OrderByPropertyName(orderBy, isAscending);

            result = competitions.Skip(excludedRows).Take(pageSize).ToList();
            var res=  new PagingViewModel<CompetitionViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
            return res;
        }

        public PagingViewModel<CompetitionViewModel> GetMyCompetitons(int userId, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
                .Where(x => x.UserID == userId);
              


            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionViewModel> result = new List<CompetitionViewModel>();

            var competitions = query.Select(obj => new CompetitionViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic,
                NamEnglish = obj.NamEnglish,
                Address = obj.Address,
                CamelsCount = obj.CamelsCount,
                From = obj.From,
                To = obj.To,
                CategoryArabicName = obj.Category.NameArabic,
                CategoryEnglishName = obj.Category.NameEnglish,
                CategoryID = obj.CategoryID,
                UserName = obj.User.UserName,
                ImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + obj.Image,
                CompetitionType = obj.CompetitionType,
                Rewards = obj.CompetitionRewards.Where(x => !x.IsDeleted).Select(x => new CompetitionRewardViewModel
                {
                    ID = x.ID,
                    NameArabic = x.NameArabic,
                    NamEnglish = x.NameEnglish,
                    SponsorText = x.SponsorText
                }),

                Invites = obj.CompetitionInvites.Where(x => !x.IsDeleted).Select(x => new CompetitionInviteViewModel
                {
                    ID = x.ID,
                    UserName = x.User.UserName,
                    UserImage = x.User.UserProfile.MainImage != null ?
                       protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",

                }),
                Referees = obj.CompetitionReferees.Where(x => !x.IsDeleted).Select(x => new CompetitionRefereeViewModel
                {
                    ID = x.ID,
                    UserName = x.User.UserName,
                    UserImage = x.User.UserProfile.MainImage != null ?
                       protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                    IsBoss = x.IsBoss
                }),
                Checkers = obj.CompetitionCheckers.Where(x => !x.IsDeleted).Select(x => new CompetitionCheckerViewModel
                {
                    ID = x.ID,
                    UserName = x.User.UserName,
                    UserImage = x.User.UserProfile.MainImage != null ?
                       protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                    IsBoss = x.IsBoss
                }),
                Conditions = obj.CompetitionConditions.Where(x => !x.IsDeleted).Select(x => new CompetitionConditionViewModel
                {
                    ID = x.ID,
                    TextArabic = x.TextArabic,
                    TextEnglish = x.TextEnglish
                })

            }).OrderByPropertyName(orderBy, isAscending);

            result = competitions.Skip(excludedRows).Take(pageSize).ToList();
            var res =  new PagingViewModel<CompetitionViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
            return res;
        }

        public bool  Add(CompetitionCreateViewModel viewModel)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var currentDate = DateTime.Now;

            var refereeUserIDs = viewModel.Referees.Select(x => x.UserID).ToList();
            var checkerUserIDs = viewModel.Checkers.Select(x => x.UserID).ToList();
            var competitorUserIDs = viewModel.Invites.Select(x => x.UserID).ToList();
            if (refereeUserIDs.Any(r=>checkerUserIDs.Contains(r)))
            {
                throw new Exception($"can not assign same user to be both checker and referee");
            }
            if (competitorUserIDs.Any(r => checkerUserIDs.Contains(r)))
            {
                throw new Exception($"can not assign same user to be both checker and competitor");
            }

            if (viewModel.Referees.Where(x=>x.IsBoss == true).Count() > 1 || viewModel.Referees.Where(x => x.IsBoss == true).Count() == 0)
            {
                throw new Exception(Resource.ChooseOneReferee);
            }
            if (viewModel.Checkers.Where(x => x.IsBoss == true).Count() > 1 || viewModel.Checkers.Where(x => x.IsBoss == true).Count() == 0)
            {
                throw new Exception(Resource.ChooseOneBossChecker);
            }
            if (viewModel.Checkers.Count() != viewModel.MaximumCheckersCount || viewModel.Referees.Count() != viewModel.MaximumRefereesCount)
            {
                throw new Exception("check number of assigned checkers and referees, they don't match the maximum number for both");
            }
            if (viewModel.From.Date< currentDate.Date)
            {
                throw new Exception(Resource.InvalidCompetitionStartDate);
            }
            if(viewModel.From > viewModel.To)
            {
                throw new Exception(Resource.InvalidCompetitionDates);
            }
            if (viewModel.CompetitorsEndJoinDate > viewModel.To || viewModel.CompetitorsEndJoinDate < viewModel.From)
            {
                throw new Exception(Resource.InvalidCompetitionDates);
            }
            if (viewModel.RefereesVotePercentage + viewModel.PeopleVotePercentage != 100)
            {
                throw new Exception(Resource.InvalidVotePercentage);

            }
            if(viewModel.Rewards.Distinct(new RewardsComparer()).Count() != viewModel.Rewards.Count())
            {
                throw new Exception("Duplicate Ranks in Rewards");
            }
            if (viewModel.TeamRewards.Count() > 0)
            {
                foreach (var item in viewModel.TeamRewards)
                {
                    if(string.IsNullOrWhiteSpace(item.TextArabic) || item.AssignedTo > 4 || item.AssignedTo < 1)
                    {
                        throw new Exception("empty values in Team rewards");
                    }
                }
            }

            var insertedCompetition = _repo.Add(viewModel.ToModel());

            if(viewModel.CompetitionType == CompetitionType.PublicForAll)
            {
                //viewModel.Invites = _userRepository.GetAll()
                //                .Where(x => x.ID == viewModel.UserID)
                //                .SelectMany(x => x.Friends)
                //                .Where(x => !x.IsDeleted)
                //                .Select(x => new CompetitionInviteCreateViewModel
                //                {
                //                    UserID = x.ID
                //                })
                //                .ToList();
                viewModel.Invites = _userRepository.GetAll()
                                .Where(x => x.ID != viewModel.UserID)
                                .Where(x => !x.IsDeleted)
                                .Where(x=> !refereeUserIDs.Contains(x.ID))
                                .Where(x=> !checkerUserIDs.Contains(x.ID))
                                .Select(x => new CompetitionInviteCreateViewModel
                                {
                                    UserID = x.ID
                                })
                                .ToList();
                viewModel.Invites.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                _competitonInviteRepository.AddRange(viewModel.Invites.Select(x => x.ToModel()));

            }
            else if(viewModel.CompetitionType == CompetitionType.PrivateForInvites)
            {
                if (viewModel.Invites != null)
                {
                    viewModel.Invites.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                    _competitonInviteRepository.AddRange(viewModel.Invites.Select(x => x.ToModel()));
                }
                else
                {
                    throw new Exception("You must add inviters for the competition");

                }

            }
            else
            {
                throw new Exception("You have to pick up one type for the competition");
            }

            if (viewModel.Referees != null)
            {
                viewModel.Referees.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                _competitonRefereeRepository.AddRange(viewModel.Referees.Select(x => x.ToModel()));
            }
            if(viewModel.Rewards !=null)
            {
                viewModel.Rewards.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                _competitonRewardRepository.AddRange(viewModel.Rewards.Select(x => x.ToModel()));

            }

            if (viewModel.Conditions != null)
            {
                viewModel.Conditions.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                _competitonConditionRepository.AddRange(viewModel.Conditions.Select(x => x.ToModel()));
            }

            if (viewModel.Checkers !=null)
            {
                viewModel.Checkers.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                _competitonCheckerRepository.AddRange(viewModel.Checkers.Select(x => x.ToModel()));
            }
            if (viewModel.Specifications != null)
            {
                if( viewModel.Specifications.Select(x=>x.MaxAllowedValue).Sum() != 100)
                {
                    throw new Exception("specifcations not equal to 100");
                }
                viewModel.Specifications.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                _competitonSpecificationRepository.AddRange(viewModel.Specifications.Select(x => x.ToModel()));
            }

            if (viewModel.TeamRewards != null)
            {
                viewModel.TeamRewards.ForEach(x => x.CompetitionID = insertedCompetition.ID);
                _competitonTeamRewardRepository.AddRange(viewModel.TeamRewards.Select(x => x.ToModel()));
            }

            _unit.Save();


            var comp = _repo.GetAll().Where(com => com.ID == insertedCompetition.ID).FirstOrDefault();
            var comImg = protocol + "://" + hostName + "/uploads/Competition-Document/" + insertedCompetition.Image;
            if (viewModel.Invites != null && viewModel.Invites.Count>0)

            {
                var notifcation = new NotificationCreateViewModel
                {
                    ID = insertedCompetition.ID,
                    ContentArabic = $"{NotificationArabicKeys.NewCompetitionAnnounceToCompetitor}{comp.NameArabic}",
                    ContentEnglish = $"{NotificationEnglishKeys.NewCompetitionAnnounceToCompetitor}{comp.NamEnglish}",
                    NotificationTypeID = 1,
                    EngNotificationType = "Join the competition",
                    ArbNotificationType = "الانضمام الي المسايقة",
                    SourceID = viewModel.UserID,
                  //  SourceName = insertedCompetition.User.Name,
                    CompetitionImagePath = comImg,
                    CompetitionID = insertedCompetition.ID,
            
                };
                
               _notificationService.SendNotifictionForInvites(notifcation, viewModel.Invites);
            }

            //send for checker boss
            if (viewModel.Checkers != null &&  viewModel.Checkers.Count > 0)

            {
                var notifcation = new NotificationCreateViewModel
                {
                    ID = insertedCompetition.ID,
                    ContentArabic = $"{NotificationArabicKeys.NewCompetitionAnnounceToChecker}{comp.NameArabic}",
                    ContentEnglish = $"{NotificationEnglishKeys.NewCompetitionAnnounceToChecker}{comp.NamEnglish}",
                    NotificationTypeID = (int)TypeOfNotification.CheckerRequestForJoinCompetition,
                    ArbNotificationType= "الانضمام الي لجنة التمييز",
                    EngNotificationType= "Join the competition as Checker boss",
                    SourceID = viewModel.UserID,
                    DestinationID = viewModel.Checkers.Where(x=>x.IsBoss).FirstOrDefault().UserID,
                //    SourceName = insertedCompetition.User.Name,
                    CompetitionImagePath = comImg,
                    CompetitionID = insertedCompetition.ID,

                };

                _notificationService.SendNotifictionForUser(notifcation);
            }

            //only send to boss checker
            if (viewModel.Referees != null && viewModel.Referees.Count > 0)

            {
                var notifcation = new NotificationCreateViewModel
                {
                    ID = insertedCompetition.ID,
                    ContentArabic = $"{NotificationArabicKeys.NewCompetitionAnnounceToReferee} {comp.NameArabic}",
                    ContentEnglish = $"{NotificationEnglishKeys.NewCompetitionAnnounceToReferee} {comp.NamEnglish}",
                    NotificationTypeID = (int)TypeOfNotification.RefereeRequestForJoinCompetition,
                    ArbNotificationType = "الانضمام الي لجنة تحكيم المسايقة",
                    EngNotificationType = "Join the competition as Referee boss",
                    SourceID = viewModel.UserID,
                    DestinationID = viewModel.Referees.Where(x=>x.IsBoss).FirstOrDefault().UserID,
                 //   SourceName = insertedCompetition.User.Name,
                    CompetitionID = insertedCompetition.ID,
                    CompetitionImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + insertedCompetition?.Image,
                    UserImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + insertedCompetition.User?.UserProfile?.MainImage,
             
                };

                _notificationService.SendNotifictionForUser(notifcation);
            }
            return true;
        }

        public void Edit(CompetitionEditViewModel viewModel)
        {
            var currentDate = DateTime.Now;
            if (viewModel.From.Date < currentDate.Date)
            {
                throw new Exception(Resource.InvalidCompetitionStartDate);
            }
            if (viewModel.From > viewModel.To)
            {
                throw new Exception(Resource.InvalidCompetitionDates);
            }
            _repo.Edit(viewModel.ToModel());
            //edit Conditions
            var oldCompetitionConditionIds =
                _competitonConditionRepository
                .GetAll()
                .Where(x => x.CompetitionID == viewModel.ID)
                .Where(x => !x.IsDeleted)
                .Select(x => x.ID)
                .ToList();

            var newCompetitionConditions =
                viewModel.Conditions.Where(x => x.ID == 0).ToList();
            newCompetitionConditions.ForEach(x => x.CompetitionID = viewModel.ID);

            var editedCompetitionConditions =
                viewModel.Conditions
                .Where(x => x.ID != 0 && oldCompetitionConditionIds.Contains(x.ID))
                .ToList();

            var deletedCompetitionConditionIds =
                oldCompetitionConditionIds.
                Where(x => !editedCompetitionConditions.Select(i => i.ID).Contains(x))
                .ToList();

            _competitonConditionRepository.AddRange(newCompetitionConditions.Select(x => x.ToModel()));
            _competitonConditionRepository.RemoveRange(deletedCompetitionConditionIds);
            _competitonConditionRepository.EditRange(editedCompetitionConditions.Select(x => x.ToModel()));

            //edit invitees
            var oldCompetitionInviteIds =
                _competitonInviteRepository
                .GetAll()
                .Where(x => x.CompetitionID == viewModel.ID)
                .Where(x => !x.IsDeleted)
                .Select(x=>x.ID)
                .ToList();

            var newCompetitionInvites =
                viewModel.Invites.Where(x => x.ID == 0).ToList();
            newCompetitionInvites.ForEach(x => x.CompetitionID = viewModel.ID);

            var editedCompetitionInvites =
                viewModel.Invites
                .Where(x => x.ID != 0 && oldCompetitionInviteIds.Contains(x.ID))
                .ToList();

            var deletedCompetitionInviteIds =
                oldCompetitionInviteIds.
                Where(x => !editedCompetitionInvites.Select(i => i.ID).Contains(x))
                .ToList();

            _competitonInviteRepository.AddRange(newCompetitionInvites.Select(x => x.ToModel()));
            _competitonInviteRepository.RemoveRange(deletedCompetitionInviteIds);
            _competitonInviteRepository.EditRange(editedCompetitionInvites.Select(x => x.ToModel()));
            //edit Checkeres
            var oldCompetitionCheckerIds =
                _competitonCheckerRepository
                .GetAll()
                .Where(x => x.CompetitionID == viewModel.ID)
                .Where(x => !x.IsDeleted)
                .Select(x => x.ID)
                .ToList();

            var newCompetitionCheckers =
                viewModel.Checkers.Where(x => x.ID == 0).ToList();
            newCompetitionCheckers.ForEach(x => x.CompetitionID = viewModel.ID);

            var editedCompetitionCheckers =
                viewModel.Checkers
                .Where(x => x.ID != 0 && oldCompetitionCheckerIds.Contains(x.ID))
                .ToList();

            var deletedCompetitionCheckerIds =
                oldCompetitionCheckerIds.
                Where(x => !editedCompetitionCheckers.Select(i => i.ID).Contains(x))
                .ToList();

            _competitonCheckerRepository.AddRange(newCompetitionCheckers.Select(x => x.ToModel()));
            _competitonCheckerRepository.RemoveRange(deletedCompetitionCheckerIds);
            _competitonCheckerRepository.EditRange(editedCompetitionCheckers.Select(x => x.ToModel()));

            //edit referees
            var oldCompetitionRefereeIds =
                _competitonRefereeRepository
                .GetAll()
                .Where(x => x.CompetitionID == viewModel.ID)
                .Where(x => !x.IsDeleted)
                .Select(x => x.ID)
                .ToList();

            var newCompetitionReferees =
                viewModel.Referees.Where(x => x.ID == 0).ToList();
            newCompetitionReferees.ForEach(x => x.CompetitionID = viewModel.ID);

            var editedCompetitionReferees =
                viewModel.Referees
                .Where(x => x.ID != 0 && oldCompetitionRefereeIds.Contains(x.ID))
                .ToList();

            var deletedCompetitionRefereeIds =
                oldCompetitionRefereeIds.
                Where(x => !editedCompetitionReferees.Select(i => i.ID).Contains(x))
                .ToList();

            _competitonRefereeRepository.AddRange(newCompetitionReferees.Select(x => x.ToModel()));
            _competitonRefereeRepository.RemoveRange(deletedCompetitionRefereeIds);
            _competitonRefereeRepository.EditRange(editedCompetitionReferees.Select(x => x.ToModel()));

           
            _unit.Save();
        }
      
        public int GetSuspendCompetitionsCount(int userID)
        {
            var dateToday = DateTime.UtcNow;
            var count = _repo.GetAll()
                // .Where(c => c.To <= dateToday)
                .Where(c => c.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.JoinDateTime == null && i.RejectDateTime == null) ||
                            c.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null && i.JoinDateTime == null && i.RejectDateTime == null) ||
                           c.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.JoinDateTime == null && i.RejectDateTime == null) ||
                            c.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null && i.JoinDateTime == null && i.RejectDateTime == null) ||
                            c.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.JoinDateTime == null && i.RejectDateTime == null))
                .Where(c => c.CompetitorsEndJoinDate >= dateToday)
                .Count();
           
            return count;
        }
        public CompetitionViewModel GetByID(int userID,int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

        //     public bool ShowReferees { get; set; }
        //public bool ShowCheckers { get; set; }
        //public bool ShowCompetitors { get; set; }

        var competition = _repo.GetAll().Where(comp => comp.ID == id)
                .Select(obj => new CompetitionViewModel
                {
                    ID = obj.ID,
                    NameArabic = obj.NameArabic,
                    NamEnglish = obj.NamEnglish,
                    Address = obj.Address,
                    CamelsCount = obj.CamelsCount,
                    From = obj.From,
                    To = obj.To,
                    Published = obj.Published,
                    ShowReferees = obj.ShowReferees,
                    ShowCheckers = obj.ShowCheckers,
                    ShowCompetitors = obj.ShowCompetitors,
                    CategoryArabicName = obj.Category.NameArabic,
                    CategoryEnglishName = obj.Category.NameEnglish,
                    IsCheckerPickupTeam = obj.CheckerPickupTeamDateTime != null,
                    IsRefereePickupTeam = obj.RefereePickupTeamDateTime != null,
                    CategoryID = obj.CategoryID,
                    UserName = obj.User.UserName,
                    ImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + obj.Image,
                    CompetitionType = obj.CompetitionType,
                    IsCheckersAllocated = obj.CheckersAllocatedDate != null,
                    IsRefereesAllocated = obj.RefereesAllocatedDate != null,
                    MinimumCheckersCount =  obj.MinimumCheckersCount,
                    MinimumRefereesCount = obj.MinimumRefereesCount,
                    MinimumCompetitorsCount = obj.MinimumCompetitorsCount,
                    Completed = obj.Completed,
                    StartedDate = obj.StartedDate,
                    UserID = obj.UserID,
                    ConditionText = obj.ConditionText,
                    IsChecker = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss),
                    IsCheckerBoss = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.ChangeByOwnerDateTime == null),
                    IsReferee = obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss),
                    IsBossReferee = obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.ChangeByOwnerDateTime == null),
                    IsCompetitor = obj.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID),
                    HasPicked = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null)
                                    || obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null)
                                    ,
                    HasJoined = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss != true && i.PickupDateTime != null && i.JoinDateTime != null) ||
                                   obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == true && i.JoinDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == false && i.PickupDateTime != null && i.JoinDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == true && i.JoinDateTime != null) ||
                                     obj.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.JoinDateTime != null),
                    HasRejected = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.PickupDateTime != null && i.RejectDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == false && i.PickupDateTime != null && i.RejectDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == true && i.RejectDateTime != null) ||

                                    obj.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.RejectDateTime != null),

                    Rewards = obj.CompetitionRewards.Where(x => !x.IsDeleted).Select(x => new CompetitionRewardViewModel
                    {
                        ID = x.ID,
                        NameArabic = x.NameArabic,
                        NamEnglish = x.NameEnglish,
                        SponsorText = x.SponsorText,
                        Rank = (Rank) x.Rank
                        
                    }),
                    TeamRewards = obj.CompetitionTeamRewards.Where(x => !x.IsDeleted).Select(x => new CompetitionTeamRewardViewModel
                    {
                        ID = x.ID,
                        AssignedTo = x.AssignedTo,
                        TextArabic = x.TextArabic,
                        TextEnglish = x.TextEnglish
                    }).ToList(),
                    Specifications = obj.CompetitionSpecifications.Where(x => !x.IsDeleted).Select(x => new CompetitionSpecificationViewModel
                    {
                        ID = x.ID,
                        CamelSpecificationID = x.CamelSpecificationID,
                        MaxAllowedValue = x.MaxAllowedValue
                        
                    }).ToList(),
                    Invites = obj.CompetitionInvites.Where(x => !x.IsDeleted).Where(x=>x.JoinDateTime != null && x.SubmitDateTime != null).Select(x => new CompetitionInviteViewModel
                    {
                        ID = x.ID,
                        UserName = x.User.UserName,
                        DisplayName = x.User.DisplayName,
                        UserImage = x.User.UserProfile.MainImage != null ?
                           protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                        HasJoined = x.JoinDateTime != null && x.SubmitDateTime != null,
                        FinalScore = x.FinalScore,
                        
                    }),
                    CamelCompetitions = obj.CamelCompetitions
                                .Where(x=>x.ApprovedByRefereeBossDateTime != null)
                                .Select(x => new CamelCompetitionViewModel
                                {
                                    ID = x.ID,
                                    CamelName = x.Camel.Name,
                                    UserName = x.CompetitionInvite.User.UserName,
                                    ApprovedDateTime = x.ApprovedByRefereeBossDateTime,
                                    GroupID = x.GroupID,
                                    GroupName = x.Group.NameArabic,
                                    GroupImage = protocol + "://" + hostName + "/uploads/Camel-Document/" + x.Group.Image,
                                    CompetitorID = x.CompetitionInviteID,
                                    Values = x.RefereeReviews.Where(r => !r.IsDeleted)
                                         .Select(w => new CamelSpecificationViewModel
                                         {
                                             CamelSpecificationID = w.CamelSpecificationID,
                                             SpecificationValue = w.ActualPercentageValue,
                                             SpecificationNameArabic = w.CamelSpecification.SpecificationArabic,
                                             SpecificationNameEnglish = w.CamelSpecification.SpecificationEnglish,

                                         }).ToList()
                                }).ToList()
                      ,
                    Referees = obj.CompetitionReferees.Where(x => !x.IsDeleted).Select(x => new CompetitionRefereeViewModel
                    {
                        ID = x.ID,
                        UserName = x.User.UserName,
                        DisplayName = x.User.DisplayName,
                        UserImage = x.User.UserProfile.MainImage != null ?
                           protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                        IsBoss = x.IsBoss,
                        HasJoined = x.PickupDateTime != null && x.JoinDateTime != null,
                        HasPicked = x.PickupDateTime != null
                    }),
                    Checkers = obj.CompetitionCheckers.Where(x => !x.IsDeleted).Select(x => new CompetitionCheckerViewModel
                    {
                        ID = x.ID,
                        UserName = x.User.UserName,
                        DisplayName = x.User.DisplayName,
                        UserImage = x.User.UserProfile.MainImage != null ?
                           protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                        IsBoss = x.IsBoss,
                        HasJoined = x.PickupDateTime != null && x.JoinDateTime != null,
                        HasPicked = x.PickupDateTime != null
                    }),
                    Conditions = obj.CompetitionConditions.Where(x => !x.IsDeleted).Select(x => new CompetitionConditionViewModel
                    {
                        ID = x.ID,
                        TextArabic = x.TextArabic,
                        TextEnglish = x.TextEnglish
                    })
                }).FirstOrDefault();
          
            if(competition.StartedDate != null)
            {
                //checking
                var numberOfRegisteredCamels = _competitonCamelRepository.GetAll()
                                                        .Where(x => x.CompetitionID == id).Count();
                var numberOfFinishedCamelsByCheckerBoss = _competitonCamelRepository.GetAll()
                                                        .Where(x => x.CompetitionID == id)
                                                        .Where(x => x.ApprovedByCheckerBossDateTime != null || x.RejectedByCheckerBossDateTime != null)
                                                        .Count();
                if(numberOfRegisteredCamels != 0)
                    competition.CheckingCompletionPercentage = (numberOfFinishedCamelsByCheckerBoss * 100) / numberOfRegisteredCamels;

                //referee
                var numberOfApprovedCamelsByChecker = _competitonCamelRepository.GetAll()
                                                        .Where(x => x.CompetitionID == id)
                                                        .Where(x => x.ApprovedByCheckerBossDateTime != null)
                                                        .Count();
                var numberOfFinishedCamelsByRefereeBoss = _competitonCamelRepository.GetAll()
                                                        .Where(x => x.CompetitionID == id)
                                                        .Where(x => x.ApprovedByRefereeBossDateTime != null)
                                                        .Count();
                if(numberOfApprovedCamelsByChecker!= 0)
                    competition.RefereeingCompletionPercentage = (numberOfFinishedCamelsByRefereeBoss * 100) / numberOfApprovedCamelsByChecker;

            }
            //dashboard
            competition.ModuleCompletion = new ModuleComplateViewModel();
            if (competition.UserID == userID)
            {
                competition.ModuleCompletion = new ModuleComplateViewModel
                {
                    CheckingModuleDone = competition.Checkers.Where(x => x.HasJoined).Count() >= competition.MinimumCheckersCount,
                    RefereeModuleDone = competition.Referees.Where(x => x.HasJoined).Count() >= competition.MinimumRefereesCount,
                    InviteCompetitorsModuleDone = competition.StartedDate != null,
                    RatingModuleDone = competition.Completed != null,
                    PublishModuleDone = competition.Published != null

                };
      
            }
            if (competition.Completed != null && competition.UserID == userID)
            {

                competition.Winners =
                     competition.Invites
                     .OrderByDescending(cc => cc.FinalScore).Select(cc => new CompetitionWinnerViewModel
                     {
                         Percentage = cc.FinalScore,
                         UserName = cc.UserName,
                         DisplayName = cc.DisplayName,
                         UserImage = cc.UserImage,
                         GroupName = competition.CamelCompetitions.Where(c=>c.CompetitorID == cc.ID).FirstOrDefault().GroupName,
                         GroupImage = competition.CamelCompetitions.Where(c=>c.CompetitorID == cc.ID).FirstOrDefault().GroupImage,
                        
                     }).Skip(0).Take(5).ToList();
                for (int i = 0; i < competition.Winners.Count(); i++)
                {
                    competition.Winners.ElementAt(i).Rank = (Rank)(i + 1);
                    var reward = competition.Rewards.Where(x => x.Rank == (Rank)(i + 1)).FirstOrDefault();
                    if(reward != null)
                    {
                        competition.Winners.ElementAt(i).RewardTextArabic = reward.NameArabic;
                        competition.Winners.ElementAt(i).RewardTextEnglish = reward.NamEnglish;

                    }
                }
            }
            competition.Created = competition.UserID == userID;
            return competition;
        }
        public PagingViewModel<CompetitionViewModel> GetCurrentInvolvedCompetitions(int userID, string orderBy, bool isAscending, int pageIndex, int pageSize, Languages language)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var dateToday = DateTime.Now;
            var query = _repo.GetAll()
               // .Where(c => c.To <= dateToday)
                .Where(c => c.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.RejectDateTime == null)  ||
                            c.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null && i.RejectDateTime ==null) ||
                           c.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.RejectDateTime == null) ||
                            c.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null && i.RejectDateTime == null) ||
                            c.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.RejectDateTime == null));
            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionViewModel> result = new List<CompetitionViewModel>();

            var data =
                query
                .Select(obj => new CompetitionViewModel
                {
                    ID = obj.ID,
                    NameArabic = obj.NameArabic,
                    NamEnglish = obj.NamEnglish,
                    Address = obj.Address,
                    CamelsCount = obj.CamelsCount,
                    From = obj.From,
                    To = obj.To,
                    ShowReferees = obj.ShowReferees,
                    ShowCheckers = obj.ShowCheckers,
                    ShowCompetitors = obj.ShowCompetitors,
                    CategoryArabicName = obj.Category.NameArabic,
                    CategoryEnglishName = obj.Category.NameEnglish,
                    CategoryID = obj.CategoryID,
                    UserName = obj.User.UserName,
                    ImagePath = protocol + "://" + hostName + "/uploads/Competition-Document/" + obj.Image,
                    CompetitionType = obj.CompetitionType,
                    IsCheckersAllocated = obj.CheckersAllocatedDate != null,
                    IsRefereesAllocated = obj.RefereesAllocatedDate != null,
                    IsChecker = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss),
                    IsCheckerBoss = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.ChangeByOwnerDateTime == null),
                    IsReferee = obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss),
                    IsBossReferee = obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss && i.ChangeByOwnerDateTime == null),
                    IsCompetitor = obj.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID),
                    HasPicked =    obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null)
                                    || obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && !i.IsBoss && i.PickupDateTime != null) 
                                    ,
                    HasJoined =    obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss != true && i.PickupDateTime != null && i.JoinDateTime != null) ||
                                   obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == true && i.JoinDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID &&i.IsBoss ==false&& i.PickupDateTime != null && i.JoinDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == true && i.JoinDateTime != null)||
                                     obj.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.JoinDateTime != null),
                    HasRejected = obj.CompetitionCheckers
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.PickupDateTime != null && i.RejectDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.IsBoss == false && i.PickupDateTime != null && i.RejectDateTime != null) ||
                                    obj.CompetitionReferees
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID  && i.IsBoss == true  && i.RejectDateTime != null) ||

                                    obj.CompetitionInvites
                                    .Where(i => !i.IsDeleted)
                                    .Any(i => i.UserID == userID && i.RejectDateTime != null),
           
                    Rewards = obj.CompetitionRewards.Where(x => !x.IsDeleted).Select(x => new CompetitionRewardViewModel
                    {
                        ID = x.ID,
                        NameArabic = x.NameArabic,
                        NamEnglish = x.NameEnglish,
                        SponsorText = x.SponsorText
                    }),
                    TeamRewards = obj.CompetitionTeamRewards.Where(x => !x.IsDeleted).Select(x => new CompetitionTeamRewardViewModel
                    {
                        ID = x.ID,
                        AssignedTo = x.AssignedTo,
                        TextArabic = x.TextArabic,
                        TextEnglish = x.TextEnglish
                    }).ToList(),
                    Specifications = obj.CompetitionSpecifications.Where(x => !x.IsDeleted).Select(x => new CompetitionSpecificationViewModel
                    {
                        ID = x.ID,
                        CamelSpecificationID = x.CamelSpecificationID,
                        MaxAllowedValue = x.MaxAllowedValue

                    }).ToList(),

                    Invites = obj.CompetitionInvites.Where(x => !x.IsDeleted).Select(x => new CompetitionInviteViewModel
                    {
                        ID = x.ID,
                        UserName = x.User.UserName,
                        UserImage = x.User.UserProfile.MainImage != null ?
                           protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",

                    }),
                    Referees = obj.CompetitionReferees.Where(x => !x.IsDeleted).Select(x => new CompetitionRefereeViewModel
                    {
                        ID = x.ID,
                        UserName = x.User.UserName,
                        UserImage = x.User.UserProfile.MainImage != null ?
                           protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                        IsBoss = x.IsBoss
                    }),
                    Checkers = obj.CompetitionCheckers.Where(x => !x.IsDeleted).Select(x => new CompetitionCheckerViewModel
                    {
                        ID = x.ID,
                        UserName = x.User.UserName,
                        UserImage = x.User.UserProfile.MainImage != null ?
                           protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage : "",
                        IsBoss = x.IsBoss
                    }),
                    Conditions = obj.CompetitionConditions.Where(x => !x.IsDeleted).Select(x => new CompetitionConditionViewModel
                    {
                        ID = x.ID,
                        TextArabic = x.TextArabic,
                        TextEnglish = x.TextEnglish
                    })
                }).OrderByPropertyName(orderBy, isAscending);
            result = data.Skip(excludedRows).Take(pageSize).ToList();
            var res=  new PagingViewModel<CompetitionViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
            return res;
        }
        public bool PublishCompetition(int userID, int competitionID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var data = _repo.GetAll().Where(x => x.ID == competitionID && x.UserID == userID && x.Completed != null).Select(x=>new { 
            Competition = x,
            CompetitionUser = x.User,
            Competiters = x.CompetitionInvites
          }).FirstOrDefault();
            var comImg = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.Competition.Image;

            if (data.Competition != null)
            {
                if (data.Competiters != null && data.Competiters.Count > 0)

                {
                    var notifcation = new NotificationCreateViewModel
                    {
                        ContentArabic = $"{NotificationArabicKeys.PublishCompetition} {data.Competition.NameArabic}",
                        ContentEnglish = $"{NotificationEnglishKeys.PublishCompetition}{data.Competition.NamEnglish}",
                        NotificationTypeID = 17,
                        EngNotificationType = "competition is published",
                        ArbNotificationType = "تم نشر المسايقة",
                        SourceID = data.Competition.UserID,
                         SourceName = data.CompetitionUser.UserName,
                        CompetitionImagePath = comImg,
                        CompetitionID = data.Competition.ID,

                    };

                    _notificationService.SendNotifictionForInvites(notifcation, data.Competiters.Select(x=> new CompetitionInviteCreateViewModel { 
                     UserID = x.UserID,
                     CompetitionID = x.CompetitionID
                    }).ToList());

                    _repo.SaveIncluded(new Competition { ID = competitionID, Published = DateTime.Now }, "Published");
                    _unit.Save();
                }
                //make a post
                var newPost = _postRepository.Add(new Post
                {
                    UserID = userID,
                    PostStatus = (int)PostStatus.SharedWithPublic,
                    PostType = (int)PostType.Image,
                    PostDocuments = new List<PostDocument>()
                    {
                        new PostDocument
                        {
                             FileName = data.Competition.Image,
                             Type = "Image" 
                        }

                    },
                    Text = $"{data.Competition.NameArabic} تم نشر مسابقة " 
                });
                FileHelper.MoveFileFromTempPathToAnotherFolder(data.Competition.Image, "Post-Document");
                _unit.Save();

                return true;
            }
            return false;
        }

        public bool ChangeRefereeBoss(ChangeRefereeBossCreateViewModel viewModel)
        {
            var referees = 
                _competitonRefereeRepository.GetAll()
                    .Where(x => x.CompetitionID == viewModel.CompetitionID)
                    .Where(x => x.ID == viewModel.OldRefereeID || x.ID == viewModel.NewRefereeID)
                    .ToList();
            var oldReferee = referees.Where(x => x.ID == viewModel.OldRefereeID).FirstOrDefault();
            if (!oldReferee.IsBoss)
            {
                throw new Exception("this referee is not boss");
            }
            oldReferee.ChangeByOwnerDateTime = DateTime.UtcNow;
            referees.Where(x=> x.ID == viewModel.NewRefereeID).FirstOrDefault().IsBoss = true;

            _unit.Save();

            return true;
        }

        public bool ChangeCheckerBoss(ChangeCheckerBossCreateViewModel viewModel)
        {
            var checkers =
                _competitonCheckerRepository.GetAll()
                    .Where(x => x.CompetitionID == viewModel.CompetitionID)
                    .Where(x => x.ID == viewModel.OldCheckerID || x.ID == viewModel.NewCheckerID)
                    .ToList();
            var oldChecker = checkers.Where(x => x.ID == viewModel.OldCheckerID).FirstOrDefault();
            if (!oldChecker.IsBoss)
            {
                throw new Exception("this referee is not boss");
            }
           
            oldChecker.ChangeByOwnerDateTime = DateTime.UtcNow;
            checkers.Where(x => x.ID == viewModel.NewCheckerID).FirstOrDefault().IsBoss = true;

            _unit.Save();

            return true;
        }
        public bool InviteCompetitors(int userID, int competitionID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var data = _repo.GetAll().Where(x => x.ID == competitionID)
                        .Select(x => new
                        {
                            Competition = x,
                            Checkers = x.CompetitionCheckers,
                            Referees = x.CompetitionReferees,
                            Competitors = x.CompetitionInvites
                        }).FirstOrDefault();
            if (data.Competition.UserID != userID)
            {
                throw new Exception("this competition does not belong to you");
            }
            if (data.Competition.MaximumCheckersCount <
                data.Checkers.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count() ||
               data.Competition.MinimumCheckersCount >
                data.Checkers.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count())
            {
                throw new Exception("number of checkers is much than allowed checkers");
            }
            if (data.Referees.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count() % 2 == 0)
            {
                throw new Exception("number of referees is not individual");
            }
            if (data.Referees.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count() == 1)
            {
                throw new Exception("you have only one referee");
            }


            var comImg = protocol + "://" + hostName + "/uploads/Competition-Document/" + data.Competition.Image;
            if (data.Competitors != null && data.Competitors.Count > 0)

            {
                var notifcation = new NotificationCreateViewModel
                {
                    ID = data.Competition.ID,
                    ContentArabic = $"{NotificationArabicKeys.NewCompetitionAnnounceToCompetitor}{data.Competition.NameArabic}",
                    ContentEnglish = $"{NotificationEnglishKeys.NewCompetitionAnnounceToCompetitor}{data.Competition.NamEnglish}",
                    NotificationTypeID = 1,
                    EngNotificationType = "Join the competition",
                    ArbNotificationType = "الانضمام الي المسايقة",
                    SourceID = userID,
                    //  SourceName = insertedCompetition.User.Name,
                    CompetitionImagePath = comImg,
                    CompetitionID = data.Competition.ID,

                };

                _notificationService.SendNotifictionForInvites(notifcation, 
                    data.Competitors.Select(x => new CompetitionInviteCreateViewModel { UserID = x.UserID}).ToList());
            }
            //mark competition as invited
            // data.Competition.CompetitorsInvitedDate = DateTime.Now;
            return true;
        }

        public bool StartCompetition(int userID, int competitionID)
        {
            var data = _repo.GetAll().Where(x => x.ID == competitionID)
                        .Select(x => new
                        {
                            Competition = x,
                            Checkers = x.CompetitionCheckers,
                            Referees = x.CompetitionReferees,
                            Competitors = x.CompetitionInvites
                        }).FirstOrDefault();
            if(data.Competition.UserID != userID)
            {
                throw new Exception("this competition does not belong to you");
            }
            if(data.Competition.MaximumCheckersCount < 
                data.Checkers.Where(c=>c.JoinDateTime != null && c.PickupDateTime != null).Count() ||
               data.Competition.MinimumCheckersCount >
                data.Checkers.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count())
            {
                throw new Exception("number of checkers is much than allowed checkers");
            }
            if (data.Competition.MaximumRefereesCount <
                data.Referees.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count() ||
               data.Competition.MinimumRefereesCount >
                data.Referees.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count())
                
            {
                throw new Exception("number of referees is much than allowed referees");
            }
            if (data.Referees.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count() % 2 == 0)
            {
                throw new Exception("number of referees is not individual");

            }
            if (data.Referees.Where(c => c.JoinDateTime != null && c.PickupDateTime != null).Count()  == 1)
            {
                throw new Exception("you have only one referee");

            }
            //if competition is public , we will let competitors to join without limit and then filter in checking phase
            if (data.Competition.MaximumCompetitorsCount <
                data.Competitors.Where(c => c.JoinDateTime != null && c.SubmitDateTime != null).Count() &&
                data.Competition.CompetitionType == (int) CompetitionType.PrivateForInvites)
            {
                throw new Exception("number of competitors is much than allowed competitors");
            }

            if(data.Competition.MinimumCompetitorsCount >
                data.Competitors.Where(c => c.JoinDateTime != null && c.SubmitDateTime != null).Count() &&
                data.Competition.CompetitionType == (int)CompetitionType.PrivateForInvites)
            {
                throw new Exception("number of competitors is much than allowed competitors");
            }
            if(data.Competition.CompetitionType == (int)CompetitionType.PublicForAll &&
                 data.Competition.MinimumCompetitorsCount >
                data.Competitors.Where(c => c.JoinDateTime != null && c.SubmitDateTime != null).Count())
            {
                throw new Exception("competition is public but number of competitors is less than allowed competitors");
            }

            data.Competition.StartedDate = DateTime.UtcNow;
            _unit.Save();
            return  true;
        }

        public CompetitionEditViewModel GetEditableByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var competition = _repo.GetAll().Where(comp => comp.ID == id)
                .Select(obj => new CompetitionEditViewModel
                {
                    ID = obj.ID,
                    NameArabic = obj.NameArabic,
                    NameEnglish = obj.NamEnglish,
                    Address = obj.Address,
                    CamelsCount = obj.CamelsCount,
                    From = obj.From,
                    To = obj.To,
                     CategoryID = obj.CategoryID,
                     CompetitionType = (CompetitionType)obj.CompetitionType,
                     Image = obj.Image,
                     MaximumCompetitorsCount = obj.MaximumCompetitorsCount,
                     PeopleVotePercentage = obj.PeopleVotePercentage,
                     MaximumRefereesCount = obj.MaximumRefereesCount,
                     RefereesVotePercentage = obj.RefereesVotePercentage,
                    Invites = obj.CompetitionInvites.Select(x=> new CompetitionInviteEditViewModel
                    {
                        ID = x.ID,
                        CompetitionID = x.CompetitionID,
                        UserID = x.UserID
                    }).ToList(),
                    Conditions = obj.CompetitionConditions.Select(x => new CompetitionConditionEditViewModel
                    {
                        ID = x.ID,
                        CompetitionID = x.CompetitionID,
                        TextArabic = x.TextArabic,
                        TextEnglish = x.TextEnglish
                    }).ToList(),
                    Rewards = obj.CompetitionRewards.Select(x => new CompetitionRewardEditViewModel
                    {
                        ID = x.ID,
                        CompetitionID = x.CompetitionID,
                        NameArabic = x.NameArabic,
                        SponsorID = x.SponsorID,
                        NamEnglish = x.NameEnglish,
                        SponsorText = x.SponsorText
                    }).ToList(),
                    Referees = obj.CompetitionReferees.Select(x => new CompetitionRefereeEditViewModel
                    {
                        ID = x.ID,
                        CompetitionID = x.CompetitionID,
                        UserID = x.UserID
                    }).ToList(),
                    Checkers = obj.CompetitionCheckers.Select(x => new CompetitionCheckerEditViewModel
                    {
                        ID = x.ID,
                        CompetitionID = x.CompetitionID,
                        UserID = x.UserID
                    }).ToList(),
                    
                }).FirstOrDefault();

            return competition; 
        }

        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }
        public bool IsAllowedToEdit(int id)
        {
            var notAllowed = 
            _competitonCamelRepository.GetAll()
                .Any(x => x.CompetitionID == id && !x.IsDeleted);

            return !notAllowed;
        }

        public void Delete(int id)
        {
            var competition = _repo.GetAll().Where(comp => comp.ID == id).FirstOrDefault();
          
                _repo.RemoveByIncluded(competition);
            
        }
        //private void AssignInvitesToCheckers(int competitionID)
        //{
        //    //get number of submitted checkers
        //    var submittedCheckers =
        //            _competitonCheckerRepository.GetAll()
        //                .Where(x => x.CompetitionID == competitionID && !x.IsBoss && x.SubmitDateTime.HasValue)
        //                .ToList();
        //    var submittedCheckersCount = submittedCheckers.Count;

        //    //get number of submitted invites
        //    var submittedInvites = _competitonInviteRepository.GetAll()
        //                                .Where(x => x.CompetitionID == competitionID && x.SubmitDateTime.HasValue)
        //                                .ToList();
        //    var submittedInvitesCount = submittedInvites.Count;
        //    //start to assign
        //    var numberOfInvitesPerChecker = submittedInvitesCount / submittedCheckersCount;
        //    foreach (var item in submittedCheckers)
        //    {
        //        int i = 0;
        //        while (i < numberOfInvitesPerChecker)
        //        {
        //            submittedInvites[i++].CheckerID = item.ID;
        //        }
        //    }
        //    submittedInvites
        //        .Where(x => x.CheckerID == null)
        //        .ToList()
        //        .ForEach(x => x.CheckerID = submittedCheckers[submittedCheckers.Count - 1].ID);

        //}

    }

   
    public class RewardsComparer : IEqualityComparer<CompetitionRewardCreateViewModel>
    {
        public bool Equals(CompetitionRewardCreateViewModel x, CompetitionRewardCreateViewModel y)
        {
            return x.Rank == y.Rank;
        }

        public int GetHashCode(CompetitionRewardCreateViewModel obj)
        {
            return 0;
        }
    }
}

