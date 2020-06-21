//using CamelsClub.Data.Extentions;
//using CamelsClub.Data.UnitOfWork;
//using CamelsClub.Repositories;
//using CamelsClub.ViewModels;
//using CamelsClub.ViewModels.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace CamelsClub.Services
//{
//    public class SubCheckerApproveService : ISubCheckerApproveService
//    {
//        private readonly IUnitOfWork _unit;
//        private readonly ICheckerApproveRepository _repo;
//        private readonly ICamelCompetitionRepository _camelCompetitionRepository;
//        private readonly ICompetitionCheckerRepository _competitionCheckerRepository;
//        private readonly ICompetitionInviteRepository _competitionInviteRepository;
//        private readonly IUserRepository _userRepository;
//        private readonly INotificationService _notificationService;


//        public SubCheckerApproveService(IUnitOfWork unit,
//                                       ICamelCompetitionRepository camelCompetitionRepository , 
//                                       ICheckerApproveRepository repo,
//                                       ICompetitionCheckerRepository competitionCheckerRepository,
//                                       ICompetitionInviteRepository competitionInviteRepository,
//                                       IUserRepository userRepository,
//                                       INotificationService notificationService)
//        {
//            _unit = unit;
//            _repo = repo;
//            _userRepository = userRepository;
//            _competitionCheckerRepository = competitionCheckerRepository;
//            _camelCompetitionRepository = camelCompetitionRepository;
//            _competitionInviteRepository = competitionInviteRepository;
//            _notificationService = notificationService;
//        }

//        //search for sub-checkers Approval
//        public PagingViewModel Search(int userID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
//        {
//            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
//            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

//            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
//                .Where(x => (x.CompetitionChecker.UserID == userID))
//                .Where(x=>x.Status == (int)CheckerApprovalStatus.FinishedBySubChecker);
                

//            int records = query.Count();
//            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
//            int pages = (int)Math.Ceiling((double)records / pageSize);
//            int excludedRows = (pageIndex - 1) * pageSize;

//            List<CheckerApproveViewModel> result = new List<CheckerApproveViewModel>();

//            var competitions = query.Select(obj => new CheckerApproveViewModel
//            {
//                ID = obj.ID,
//                CamelCompetitionID = obj.CamelCompetitionID,
//                CompetitionName = (language == Languages.English) ? obj.CamelCompetition.Competition.NamEnglish : obj.CamelCompetition.Competition.NameArabic,
//                CamelName = obj.CamelCompetition.Camel.Name,
//                Notes = obj.Notes,
           
//            }).OrderByPropertyName(orderBy, isAscending);

//            result = competitions.Skip(excludedRows).Take(pageSize).ToList();
//            return new PagingViewModel() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
//        }
//        //for sub-checkers
//        public List<CompetitionInviteViewModel> GetMyInvitedUsers(int checkerUserID , int competitionID)
//        {
//            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
//            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

//            var data = 
//            _competitionInviteRepository.GetAll()
//                .Where(x => !x.IsDeleted)
//                .Where(x => x.CompetitionID == competitionID)
//                .Where(x => x.Checker.UserID == checkerUserID)
//                .Select(x => new CompetitionInviteViewModel
//                {
//                    ID = x.ID,
//                    UserID = x.UserID,
//                    UserName = x.User.Name,
//                    UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage,
//                    Approves = x.Checker.CamelsIApproved
//                                    .Where(p=>!p.IsDeleted)
//                                    .Where(p=>p.CamelCompetition.Camel.UserID == x.UserID)
//                                    .Select(p=> new CheckerApproveViewModel {
//                                        ID = p.ID , 
//                                        CamelCompetitionID = p.CamelCompetitionID,
//                                        Status = p.Status,
//                                        Notes = p.Notes
//                                    }).ToList(),
//                    JoinedCamelsCount = x.Competition.CamelCompetitions
//                                    .Where(c=>c.Camel.UserID == x.UserID)
//                                    .Where(c=>!c.IsDeleted)
//                                    .Count()
//                })
//                .ToList();

//            foreach (var user in data)
//            {
//                if(user.Approves != null && user.Approves.Count == 0)
//                {
//                    user.UserStatus = (int)InvitedUserStatus.AllCamelsNotCheckedYet;
//                }
//                else if(user.Approves.Count == user.JoinedCamelsCount && user.Approves.All(x=>x.Status == (int)CheckerApprovalStatus.FinishedBySubChecker))
//                {
//                    user.UserStatus = (int) InvitedUserStatus.ApprovedBySubChecker;
//                }
//                else if (user.Approves.Count == user.JoinedCamelsCount && user.Approves.Any(x => x.Status == (int)CheckerApprovalStatus.RejectedByBoss))
//                {
//                    user.UserStatus = (int)InvitedUserStatus.RejectedTillUpdateFromUser;
//                }
//                else if (user.Approves.Count == user.JoinedCamelsCount && 
//                         user.Approves.All(x => x.Status != (int)CheckerApprovalStatus.RejectedByBoss) &&
//                         user.Approves.Any(x => x.Status == (int)CheckerApprovalStatus.RejectedByBoss))
//                {
//                    user.UserStatus = (int)InvitedUserStatus.UpdatedByUserAndWaitingForApprovalAgain;
//                }
//                else if(user.Approves.Count < user.JoinedCamelsCount)
//                {
//                    user.UserStatus = (int)InvitedUserStatus.PartialApprovalBySubChecker;
//                }
//            }
//            data = data
//                    .Where(u => u.Approves.All(x => x.Status != (int)CheckerApprovalStatus.Terminate)).ToList();
//            return data;
//        }

//        //for boss-checker
//        public List<CompetitionInviteViewModel> GetBossInvitedUsers(int checkerUserID, int competitionID)
//        {
//            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
//            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

//            var data =
//            _competitionInviteRepository.GetAll()
//                .Where(x => !x.IsDeleted)
//                .Where(x => x.CompetitionID == competitionID)
//               .Where(x=>x.CamelCompetitions.All(cc=>cc.CheckerApprovers.Any()))
//                .Select(x => new CompetitionInviteViewModel
//                {
//                    ID = x.ID,
//                    UserID = x.UserID,
//                    UserName = x.User.Name,
//                    UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage,
//                    Approves = x.Checker.CamelsIApproved
//                                    .Where(p => !p.IsDeleted)
//                                    .Where(p => p.CamelCompetition.Camel.UserID == x.UserID)
//                                    .Select(p => new CheckerApproveViewModel
//                                    {
//                                        ID = p.ID,
//                                        CamelCompetitionID = p.CamelCompetitionID,
//                                        Status = p.Status,
//                                        Notes = p.Notes
//                                    }).ToList(),
//                    JoinedCamelsCount = x.Competition.CamelCompetitions
//                                    .Where(c => c.Camel.UserID == x.UserID)
//                                    .Where(c => !c.IsDeleted)
//                                    .Count()
//                })
//                .ToList();

//            foreach (var user in data)
//            {
//                if (user.Approves != null && user.Approves.Count == 0)
//                {
//                    user.UserStatus = (int)InvitedUserStatus.AllCamelsNotCheckedYet;
//                }
//                else if (user.Approves.Count == user.JoinedCamelsCount && user.Approves.All(x => x.Status == (int)CheckerApprovalStatus.FinishedBySubChecker))
//                {
//                    user.UserStatus = (int)InvitedUserStatus.ApprovedBySubChecker;
//                }
//                else if (user.Approves.Count == user.JoinedCamelsCount && user.Approves.Any(x => x.Status == (int)CheckerApprovalStatus.RejectedByBoss))
//                {
//                    user.UserStatus = (int)InvitedUserStatus.RejectedTillUpdateFromUser;
//                }
//                else if (user.Approves.Count == user.JoinedCamelsCount &&
//                         user.Approves.All(x => x.Status != (int)CheckerApprovalStatus.RejectedByBoss) &&
//                         user.Approves.Any(x => x.Status == (int)CheckerApprovalStatus.RejectedByBoss))
//                {
//                    user.UserStatus = (int)InvitedUserStatus.UpdatedByUserAndWaitingForApprovalAgain;
//                }
//                else if (user.Approves.Count < user.JoinedCamelsCount)
//                {
//                    user.UserStatus = (int)InvitedUserStatus.PartialApprovalBySubChecker;
//                }
//            }
//            data = data
//                    .Where(u => u.Approves.All(x => x.Status != (int)CheckerApprovalStatus.Terminate)).ToList();
//            return data;
//        }
//        //for sub chekers 
//        public InvitedUserCamelsViewModel GetInvitedUserCamels(int userID, int competitionID)
//        {
//            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
//            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

//            var data =
//            _camelCompetitionRepository.GetAll()
//                .Where(x => !x.IsDeleted)
//                .Where(x => x.CompetitionID == competitionID)
//                .Where(x => x.Camel.UserID == userID)
//                .Select(x => new CamelCompetitionViewModel
//                {
//                    ID = x.ID,
//                    UserName = x.Camel.User.Name,
//                    UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + x.Camel.User.UserProfile.MainImage,
//                    CamelID = x.CamelID,
//                    CheckerApproveID = x.CheckerApprovers.Select(c=>c.ID).FirstOrDefault(),
//                    CheckerNotes = x.CheckerApprovers.Select(c => c.Notes).FirstOrDefault(),
//                    IsRatedBySubChecker = x.CheckerApprovers
//                                    .Where(a=>a.Status == (int)CheckerApprovalStatus.FinishedBySubChecker)
//                                    .Where(a=> !a.IsDeleted).Any(),
//                    IsRejectedByBoss = x.CheckerApprovers
//                                    .Where(a => a.Status == (int)CheckerApprovalStatus.RejectedByBoss)
//                                    .Where(a => !a.IsDeleted).Any(),
//                    //IsTerminatedByBoss = x.CheckerApprovers
//                    //                .Where(a => a.Status == (int)CheckerApprovalStatus.Terminate)
//                    //                .Where(a => !a.IsDeleted).Any(),
//                    CompetitionID = x.CompetitionID,
//                    CamelName = x.Camel.Name,
//                    CategoryArabicName = x.Camel.Category.NameArabic,
//                    CategoryEnglishName = x.Camel.Category.NameEnglish,

//                    CamelImages = x.Camel.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
//                    {
//                        FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
//                        UploadedDate = doc.CreatedDate,
//                        FileType = doc.Type
//                    }).ToList()


//                })
//                .ToList();
//            var result = new InvitedUserCamelsViewModel
//            {
//                Camels = data,
//                NumberOfRatedCamels = data.Where(x => x.IsRatedBySubChecker).Count(),
//                NumberOfRejectedCamels = data.Where(x => x.IsRejectedByBoss).Count(),
//                UserName = data != null && data.Count > 0 ? data[0].UserName : "",
//                UserImage = data != null && data.Count > 0 ? data[0].UserImage : ""
//            };
          
//            return result;
//        }
//        //for admin
//        public InvitedUserCamelsForBossViewModel GetBossInvitedUserCamels(int userID, int competitionID)
//        {
//            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
//            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            

//            var data =
//            _competitionInviteRepository.GetAll()
//                .Where(x => !x.IsDeleted)
//                .Where(x => x.CompetitionID == competitionID)
//                .Where(x => x.UserID == userID)
//                .Select(x=> new InvitedUserCamelsForBossViewModel
//                {
//                    UserName = x.User.Name,
//                    UserImage = protocol + "://" + hostName + "/uploads/User-Document/" + x.User.UserProfile.MainImage,
//                    NumberOfApprovedCamels = x.CamelCompetitions
//                                                .Where(cc=>!cc.IsDeleted)
//                                                .SelectMany(cc=>cc.CheckerApprovers)
//                                                .Where(p=>!p.IsDeleted)
//                                                .Where(p=>p.Status == (int)CheckerApprovalStatus.ApprovedByBoss)
//                                                .Count(),
//                    NumberOfRejectedCamels = x.CamelCompetitions
//                                                .Where(cc => !cc.IsDeleted)
//                                                .SelectMany(cc => cc.CheckerApprovers)
//                                                .Where(p => !p.IsDeleted)
//                                                .Where(p => p.Status == (int)CheckerApprovalStatus.RejectedByBoss)
//                                                .Count(),
//                    Camels = x.CamelCompetitions
//                         .Where(c=>!c.IsDeleted)
//                         .Select(c => new CamelCompetitionForBossViewModel
//                         {
//                             ID = c.ID,
//                             CamelID = c.CamelID,
//                             CheckerApproveID = c.CheckerApprovers.Select(p => p.ID).FirstOrDefault(),
//                             CheckerNotes = c.CheckerApprovers.Select(p => p.Notes).FirstOrDefault(),
//                             IsApproved = c.CheckerApprovers
//                                    .Where(a => a.Status == (int)CheckerApprovalStatus.ApprovedByBoss)
//                                    .Where(a => !a.IsDeleted).Any(),
//                             IsRejectedByBoss = c.CheckerApprovers
//                                    .Where(a => a.Status == (int)CheckerApprovalStatus.RejectedByBoss)
//                                    .Where(a => !a.IsDeleted).Any(),
//                             CompetitionID = c.CompetitionID,
//                             CamelName = c.Camel.Name,
//                             CategoryArabicName = c.Camel.Category.NameArabic,
//                             CategoryEnglishName = c.Camel.Category.NameEnglish,
//                             CamelImages = c.Camel.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
//                             {
//                                 FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
//                                 UploadedDate = doc.CreatedDate,
//                                 FileType = doc.Type
//                             }).ToList()
//                         }).ToList()
                


//                })
//                .FirstOrDefault();
            
//            return data;
//        }

//        //for sub checkers
//        public void Edit(CheckerApproveCreateViewModel viewModel)
//        {
//            var existApproval = 
//            _repo.GetAll()
//                .Where(x => !x.IsDeleted)
//                .Where(x => x.ID == viewModel.ID)
//                .Where(x => x.Status == (int)CheckerApprovalStatus.FinishedBySubChecker)
//                .FirstOrDefault();
//            if (existApproval != null)
//            {
//                existApproval.Notes = viewModel.Notes;
//            }
//            _unit.Save();
//        }

//        //for sub checkers
//        public void Add( CheckerApproveCreateViewModel viewModel )
//        {
//            lock (this)
//            {
//                //get camelcompetiton data
//                var camelCompetiton = _camelCompetitionRepository
//                                               .GetAll().Where(x => x.ID == viewModel.CamelCompetitionID)
//                                               .FirstOrDefault();
//                var InvitedUser = camelCompetiton.Camel.User;

//                //check if all camels of that user is approved by the checker
//                var userCamels =
//                    _repo.GetAll().Where(x => x.CamelCompetition.Camel.UserID == InvitedUser.ID)
//                    .Where(x => !x.IsDeleted)
//                    .Where(x => x.CamelCompetition.ID == viewModel.CamelCompetitionID)
//                    .Where(x => x.Status == (int)ViewModels.Enums.CheckerApprovalStatus.FinishedBySubChecker)
//                    .Select(x=>x.CamelCompetition.Camel)
//                    .ToList();

//                //allowedNumberOfCamelsInThatCompetition
//                var competitionCamelsCount = camelCompetiton.Competition.CamelsCount;
//                //check if it is the last one, send notification to Admin Checker
//                if((userCamels.Count - 1) == competitionCamelsCount)
//                {
//                    //save approve in DB
                   
//                    var insertedApprove = viewModel.ToModel();
//                    insertedApprove.CompetitionCheckerID = _competitionCheckerRepository.GetAll()
//                                                                .Where(x => x.UserID == viewModel.UserID)
//                                                                .Select(x=>x.ID).FirstOrDefault();
//                    insertedApprove.Status = (int)ViewModels.Enums.CheckerApprovalStatus.FinishedBySubChecker;
//                    _repo.Add(insertedApprove);
//                    //send Notification to Admin Checker

//                }
//                else
//                {
                    
//                    //just save
//                    var insertedApprove = viewModel.ToModel();
//                    insertedApprove.CompetitionCheckerID = _competitionCheckerRepository.GetAll()
//                                                                .Where(x => x.UserID == viewModel.UserID)
//                                                                .Select(x => x.ID).FirstOrDefault();

//                    insertedApprove.Status = (int)CheckerApprovalStatus.FinishedBySubChecker;
//                    _repo.Add(insertedApprove);
//                }
//                _unit.Save();
//            }

//        }

        
//        public CheckerApproveViewModel GetByID(int id, Languages language = Languages.Arabic)
//        {

//            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
//            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

//            var data = _repo.GetAll().Where(x => x.ID == id)
//                .Select(obj => new CheckerApproveViewModel
//                {
//                    ID = obj.ID,
//                    CamelCompetitionID = obj.CamelCompetitionID ,
//                    Notes = obj.Notes ,
//                    Status = obj.Status , 
//                    CompetitionName=(language == Languages.English)?obj.CamelCompetition.Competition.NamEnglish:obj.CamelCompetition.Competition.NameArabic,
//                    CamelName = obj.CamelCompetition.Camel.Name

//                }).FirstOrDefault();

//            return data;
//        }

//        public bool IsExists(int camelCompetitionID , int userID)
//        {
//            return _repo.GetAll().Where(x => x.CamelCompetitionID == camelCompetitionID && x.CompetitionChecker.UserID == userID && !x.IsDeleted).Any();
//        }
       
       
//    }

//    public class CamelCompetitionForBossViewModel 
//    {
//        public int ID { get; set; }
//        public int CamelID { get; set; }
//        public int CheckerApproveID { get; set; }
//        public string CheckerNotes { get; set; }
//        public bool IsApproved { get; set; }
//        public bool IsRejectedByBoss { get; set; }
//        public int CompetitionID { get; set; }
//        public string CamelName { get; set; }
//        public string CategoryArabicName { get; set; }
//        public string CategoryEnglishName { get; set; }
//        public List<CamelDocumentViewModel> CamelImages { get; set; }
//    }

//    public class InvitedUserCamelsViewModel
//    {
//        public string UserName { get; set; }
//        public string UserImage { get; set; }
//        public int NumberOfRatedCamels { get; set; }
//        public int NumberOfRejectedCamels { get; set; }
//        public List<CamelCompetitionViewModel> Camels { get; set; }
//    }
//    public class InvitedUserCamelsForBossViewModel
//    {
//        public string UserName { get; set; }
//        public string UserImage { get; set; }
//        public int NumberOfApprovedCamels { get; set; }
//        public int NumberOfRejectedCamels { get; set; }
//        public List<CamelCompetitionForBossViewModel> Camels { get; set; }
//    }
//}

