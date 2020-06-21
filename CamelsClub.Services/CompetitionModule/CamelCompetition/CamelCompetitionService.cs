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
    public class CamelCompetitionService : ICamelCompetitionService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICamelCompetitionRepository _repo;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICamelGroupRepository _camelGroupRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICompetitionCheckerRepository _competitionCheckerRepository;
        private readonly ICompetitionInviteRepository _competitionInviteRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;


        public CamelCompetitionService(IUnitOfWork unit,
                                       ICamelCompetitionRepository repo,
                                       IGroupRepository groupRepository,
                                       ICamelGroupRepository camelGroupRepository,
                                       ICompetitionRepository competitionRepository,
                                       ICompetitionCheckerRepository competitionCheckerRepository,
                                       ICompetitionInviteRepository competitionInviteRepository,
                                       IUserRepository userRepository,
                                       INotificationService notificationService)
        {
            _unit = unit;
            _repo = repo;
            _competitionRepository = competitionRepository;
            _camelGroupRepository = camelGroupRepository;
            _groupRepository = groupRepository;
            _competitionCheckerRepository = competitionCheckerRepository;
            _competitionInviteRepository = competitionInviteRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;

        }
        public PagingViewModel<CamelCompetitionViewModel> Search(int competitionID = 0,int camelID=0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
                .Where(x => (x.CompetitionID == competitionID || competitionID == 0) &&
                (x.CamelID == camelID || camelID == 0));


            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CamelCompetitionViewModel> result = new List<CamelCompetitionViewModel>();

            var competitions = query.Select(obj => new CamelCompetitionViewModel
            {
                ID = obj.ID,
                CompetitionID = obj.CompetitionID,
                CamelID = obj.CamelID,
                CompetitionName = (language == Languages.English) ? obj.Competition.NamEnglish : obj.Competition.NameArabic,
                CamelName = obj.Camel.Name

            }).OrderByPropertyName(orderBy, isAscending);

            result = competitions.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CamelCompetitionViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }
        
        public void Add(int userID , List<CamelCompetitionCreateViewModel> camelCompetitions)
        {
            lock (this)
            {
                var competitionID = camelCompetitions[0].CompetitionID;

                //get competitionInviteID
                var invitedUserID =
                    _competitionInviteRepository.GetAll()
                    .Where(x => x.UserID == userID)
                    .Where(x=> x.CompetitionID == competitionID)
                    .Select(x=>x.ID).FirstOrDefault();
                camelCompetitions.ForEach(x => x.CompetitionInviteID = invitedUserID);
                //check number of participants and EndJoinDate is less than today
                var competitions =
                _competitionRepository.GetAll()
                    .Where(x => x.ID == competitionID)
                    .ToList();
                if (competitions != null && competitions.Count > 1)
                    throw new Exception("there is more than competition with same ID");
                Competition competition = competitions[0];
                if (competition.CompetitorsEndJoinDate < DateTime.Now)
                {
                    throw new Exception("sorry. you are late,Join Date is Passed");
                }
                //get actual joined users
                var usersJoined = _repo.GetAll()
                                    .Where(x => x.CompetitionID == competitionID)
                                    .Where(x=> !x.IsDeleted)
                                    //.Select(x => x.Camel)
                                   // .GroupBy(x => x.UserID)
                                    .GroupBy(x=>x.Camel.UserID)
                                    .Select(x => new
                                    {
                                        UserID = x.Key,
                                        CamelsCount = x.Count()
                                    }).ToList();

                if (competition.MaximumCompetitorsCount <= usersJoined.Count)
                {
                    throw new Exception("sorry. there is no place, the competition has enough number of users");
                }

                //make suure each camel belongs to the logged user
                //1-first get all user camels
                var userCamels = _userRepository.GetAll()
                                        .Where(x => x.ID == userID && !x.IsDeleted)
                                        .SelectMany(x => x.Camels)
                                        .Where(x => !x.IsDeleted)
                                        .ToList();
                foreach (var item in camelCompetitions)
                {
                    var camel = userCamels.Where(x => x.ID == item.CamelID).FirstOrDefault();
                    if(camel.CategoryID != competition.CategoryID)
                    {
                        throw new Exception($"the camel {camel.Name} does not belong to specified category of this competition");

                    }
                    if (!userCamels.Select(x => x.ID).Contains(item.CamelID))
                    {
                        throw new Exception("there are camels that do not belong to you");
                    }
                }
                //make sure number of camels user send is equal to count of competition
                if (camelCompetitions.Count != competition.CamelsCount)
                {
                    throw new Exception($"number of camels allowed is {competition.CamelsCount}");
                }

                //check if that user will be the last one
                //if ((competition.ParticipantsCount - 1) == usersJoined.Count)
                //{
                //    //check all checkers are submitted
                //    var checkers =
                //        _competitionCheckerRepository.GetAll().Where(x => x.CompetitionID == competitionID && !x.IsDeleted)
                //            .ToList();
                //    var submittedCheckersCount =
                //                       checkers.Where(x => x.SubmitDateTime.HasValue).Count();
                //    if (submittedCheckersCount == (checkers.Count))
                //    {
                //        //add to  db
                //        _repo.AddRange(camelCompetitions.Select(x => x.ToModel()));
                //        // submit thee invite user
                //        var invitedUsers =
                //        _competitionInviteRepository.GetAll()
                //            .Where(x => x.CompetitionID == competitionID)
                //            .Where(x => !x.IsDeleted)
                //            .ToList();
                //        var invitedUser =
                //        invitedUsers.Where(x => x.UserID == userID)
                //            .FirstOrDefault();
                //        invitedUser.SubmitDateTime = DateTime.Now;

                //        //assign
                //        AssignInvitesToCheckers(competitionID, checkers, invitedUsers);
                //    }
                //    else
                //    {
                //        AddCamelsInCompetition();
                //        //_repo.AddRange(camelCompetitions.Select(x => x.ToModel()));
                //        //var invitedUsers =
                //        //_competitionInviteRepository.GetAll()
                //        //    .Where(x => x.CompetitionID == competitionID)
                //        //    .Where(x => !x.IsDeleted)
                //        //    .ToList();
                //        //var invitedUser =
                //        //invitedUsers.Where(x => x.UserID == userID)
                //        //    .FirstOrDefault();
                //        //invitedUser.SubmitDateTime = DateTime.Now;
                //    }

                //}
                //else
                //{
                    AddCamelsInCompetition();
                    //_repo.AddRange(camelCompetitions.Select(x => x.ToModel()));
                    //var invitedUsers =
                    //_competitionInviteRepository.GetAll()
                    //    .Where(x => x.CompetitionID == competitionID)
                    //    .Where(x => !x.IsDeleted)
                    //    .ToList();
                    //var invitedUser =
                    //invitedUsers.Where(x => x.UserID == userID)
                    //    .FirstOrDefault();
                    //invitedUser.SubmitDateTime = DateTime.Now;
             //   }
                 void AddCamelsInCompetition()
                {
                    _repo.AddRange(camelCompetitions.Select(x => x.ToModel()));
                    var invitedUsers =
                    _competitionInviteRepository.GetAll()
                        .Where(x => x.CompetitionID == competitionID)
                        .Where(x => !x.IsDeleted)
                        .ToList();
                    var invitedUser =
                    invitedUsers.Where(x => x.UserID == userID)
                        .FirstOrDefault();
                    invitedUser.SubmitDateTime = DateTime.Now;
                }
            }
        }

        public bool IsGroupExistInCompetition(int competitionID, int groupID)
        {
            return
                _repo.GetAll()
                    .Where(x => x.CompetitionID == competitionID && x.GroupID == groupID && !x.IsDeleted)
                    .Any();
        }
        public bool AddGroup(JoinCompetitionCreateViewModel viewModel)
        {
            var data =
             _competitionInviteRepository.GetAll()
                 .Where(x => x.UserID == viewModel.UserID)
                 .Where(x => x.CompetitionID == viewModel.CompetitionID)
                 .Select(x => new
                 {
                     Competitor = new { 
                         ID = x.ID,
                         IsAllowedGroup = x.User.Groups.Where(g => g.ID == viewModel.GroupID && !g.IsDeleted).Any()
                     },
                     Group = new { 
                         CategoryID = x.User.Groups.Where(g => g.ID == viewModel.GroupID && !g.IsDeleted).Select(g=>g.CategoryID).FirstOrDefault(),
                         CamelsCount = x.User.Groups
                                        .Where(g => g.ID == viewModel.GroupID && !g.IsDeleted)
                                        .SelectMany(i=>i.CamelGroups).Select(cg => cg.Camel)
                                        .Where(c => !c.IsDeleted).Count(),
                         Camels = x.User.Groups
                                        .Where(g => g.ID == viewModel.GroupID && !g.IsDeleted)
                                        .SelectMany(i => i.CamelGroups).Select(cg => cg.Camel)
                                        .Where(c => !c.IsDeleted).ToList()
                     },                 
                     Competition = new
                     {
                         ID = x.CompetitionID,
                         EndJoinDate = x.Competition.CompetitorsEndJoinDate,
                         MaximumAllowedCompetitorsCount = x.Competition.MaximumCompetitorsCount,
                         AllowedCamelsCount = x.Competition.CamelsCount,
                         Type = x.Competition.CompetitionType,
                         Category = new
                         {
                             ID = x.Competition.CategoryID
                         }
                     }
                 }).FirstOrDefault();

            //check that group belong to logged user
            if (!data.Competitor.IsAllowedGroup)
            {
                throw new Exception("group does not belong to logged competitor");
            }
                //you should lock group editing

                //check number of camels in group
                //check count of camels in the group
                if(data.Group.CamelsCount != data.Competition.AllowedCamelsCount)
                {
                    throw new Exception("the number of camels in group must be equal to the allowed number of camels in the competition");
                }
                //check category of that group
                if (data.Group.CategoryID != data.Competition.Category.ID)
                {
                    throw new Exception("the group must be in same category as competition");
                }

                var camelsInCompetitionList = new List<CamelCompetitionCreateViewModel>();
                foreach (var item in data.Group.Camels)
                {
                    camelsInCompetitionList.Add(new CamelCompetitionCreateViewModel
                    {
                        CamelID = item.ID,
                        CompetitionID = data.Competition.ID,
                        CompetitionInviteID = data.Competitor.ID,
                    });
                }
                //check number of participants and EndJoinDate is less than today
                if (data.Competition.EndJoinDate < DateTime.Now)
                {
                    throw new Exception("sorry. you are late,Join Date is Passed");
                }
                //get actual joined users
                var competitorsJoinedCompetition = _competitionInviteRepository.GetAll()
                                                        .Where(x => x.CompetitionID == viewModel.CompetitionID)
                                                        .Where(x => !x.IsDeleted)
                                                        .Where(x => x.SubmitDateTime.HasValue)
                                                        .ToList();

                if (data.Competition.Type == (int) CompetitionType.PrivateForInvites && data.Competition.MaximumAllowedCompetitorsCount <= competitorsJoinedCompetition.Count)
                {
                    throw new Exception("sorry. there is no place, the competition has enough number of users");
                }
                    //add to  db
                    var camelsInCompetition = camelsInCompetitionList.Select(x => x.ToModel()).ToList();
                    camelsInCompetition.ForEach(x => x.GroupID = viewModel.GroupID);
                    _repo.AddRange(camelsInCompetition);
                _groupRepository.SaveIncluded(new Group { ID = viewModel.GroupID, IsLocked = true }, "IsLocked");
                _competitionInviteRepository.SaveIncluded(new CompetitionInvite { ID = data.Competitor.ID, SubmitDateTime = DateTime.UtcNow, JoinDateTime = DateTime.UtcNow }, "SubmitDateTime", "JoinDateTime");
                    _unit.Save();
            return true;
        }



        public CamelCompetitionViewModel GetByID(int id, Languages language = Languages.Arabic)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var data = _repo.GetAll().Where(calCom => calCom.ID == id)
                .Select(obj => new CamelCompetitionViewModel
                {
                    ID = obj.ID,
                    CompetitionID = obj.CompetitionID,
                    CamelID = obj.CamelID,
                    CompetitionName=(language == Languages.English)?obj.Competition.NamEnglish:obj.Competition.NameArabic,
                    CamelName = obj.Camel.Name

                }).FirstOrDefault();

            return data;
        }

        public bool IsExists(int cameID, int competitionID)
        {
            return _repo.GetAll().Where(x => x.CamelID == cameID && x.CompetitionID == competitionID).Any();
        }
        public void Delete(int id)
        {
            var camelCompetition = _repo.GetAll().Where(comp => comp.ID == id).FirstOrDefault();
            if (camelCompetition != null)
            {
                _repo.RemoveByIncluded(camelCompetition);
            }
        }
        //private void AssignInvitesToCheckers(int competitionID, List<CompetitionChecker> checkers, List<CompetitionInvite> invites)
        //{
        //    //get number of submitted checkers
        //    var submittedCheckers =
        //            checkers
        //                .Where(x => x.CompetitionID == competitionID && !x.IsBoss && x.SubmitDateTime.HasValue)
        //                .ToList();
        //    var submittedCheckersCount = submittedCheckers.Count;

        //    //get number of submitted invites
        //    var submittedInvites = invites
        //                                .Where(x => x.CompetitionID == competitionID && x.SubmitDateTime.HasValue)
        //                                .ToList();
        //    var submittedInvitesCount = submittedInvites.Count;
        //    var invitedUserTempCounter = submittedInvitesCount;
        //    //start to assign
        //    var numberOfInvitesPerChecker = submittedInvitesCount / submittedCheckersCount;
        //    foreach (var item in submittedCheckers)
        //    {
        //        int i = 0;
        //        while (i < numberOfInvitesPerChecker)
        //        {
        //            submittedInvites[--invitedUserTempCounter].CheckerID = item.ID;
        //            i++;
        //        }
        //    }
        //    submittedInvites
        //        .Where(x => x.CheckerID == null)
        //        .ToList()
        //        .ForEach(x => x.CheckerID = submittedCheckers[submittedCheckers.Count - 1].ID);

        //}


    }




}

