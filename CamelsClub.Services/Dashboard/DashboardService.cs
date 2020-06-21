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
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICamelRepository _camelRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
    

        public DashboardService(IUnitOfWork unit,
                                       IGroupRepository groupRepository,
                                       ICamelRepository camelRepository,
                                       ICompetitionRepository competitionRepository,
                                       IUserRepository userRepository
                               )
        {
            _unit = unit;
            _competitionRepository = competitionRepository;
            _camelRepository = camelRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
  
        }
        public DashboardViewModel GetDashboardData(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _competitionRepository.GetAll().Where(comp => !comp.IsDeleted);

            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionViewModel> latestCompetitionsResult = new List<CompetitionViewModel>();

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
                CreatedDate = obj.CreatedDate
            }).OrderByPropertyName("ID", false);

            latestCompetitionsResult = competitions.Skip(excludedRows).Take(pageSize).ToList();
            //latest users
             var usersQuery = _userRepository.GetAll().Where(comp => !comp.IsDeleted);

             records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            pages = (int)Math.Ceiling((double)records / pageSize);
            excludedRows = (pageIndex - 1) * pageSize;

            List<UserViewModel> latestUsersResult = new List<UserViewModel>();

            var users = usersQuery.Select(obj => new UserViewModel
            {
                ID = obj.ID,
                DisplayName = obj.DisplayName,
                UserName = obj.UserName,
                CreatedDate = obj.CreatedDate
            }).OrderByPropertyName("ID", false);

            latestUsersResult = users.Skip(excludedRows).Take(pageSize).ToList();
            // get user statictics
            var usersCount = _userRepository.GetAll().Where(x => !x.IsDeleted).Count();
            //get competitions statictics
            var waitingCompetitionsCount = _competitionRepository.GetAll()
                                            .Where(x => !x.IsDeleted)
                                            .Where(x => x.StartedDate == null)
                                            .Count();
            var currentCompetitionsCount = _competitionRepository.GetAll()
                                            .Where(x => !x.IsDeleted)
                                            .Where(x => x.StartedDate != null && x.Published == null)
                                            .Count();
            var finishedCompetitionsCount = _competitionRepository.GetAll()
                                            .Where(x => !x.IsDeleted)
                                            .Where(x => x.StartedDate != null && x.Published != null)
                                            .Count();
            //get camels statictics
            var camels = _camelRepository.GetAll()
                            .GroupBy(c => c.Category)
                            .Select(g => new CamelsCountCategoryViewModel
                            {
                                CategoryNameArabic = g.Key.NameArabic,
                                CategoryNameEnglish = g.Key.NameEnglish,
                                Count = g.Count()
                            }).ToList();
            var camelsCount = _camelRepository.GetAll().Where(x => !x.IsDeleted).Count();
            var finalResult = new DashboardViewModel
            {
                CamelsCount = camelsCount,
                Camels = camels,
                UsersCount = usersCount,
                Competitions = new CompetitionsDashboardViewModel
                {
                    UpToStart = waitingCompetitionsCount,
                    Current = currentCompetitionsCount,
                    Finished = finishedCompetitionsCount
                },
                NewCompetitions = latestCompetitionsResult,
                NewUsers = latestUsersResult
            };
            return finalResult;
 
          }
        
        

    }

    public class DashboardViewModel
    {
        public int CamelsCount { get; set; }
        public List<CamelsCountCategoryViewModel> Camels { get; set; }
        public int UsersCount { get; set; }
        public CompetitionsDashboardViewModel Competitions { get; set; }
        public List<CompetitionViewModel> NewCompetitions { get; set; }
        public List<UserViewModel> NewUsers { get; set; }
    }

    public class CompetitionsDashboardViewModel
    {
        public int UpToStart { get; set; }
        public int Current { get; set; }
        public int Finished { get; set; }
    }

    public class CamelsCountCategoryViewModel
    {
        public string CategoryNameArabic { get; set; }
        public string CategoryNameEnglish { get; set; }
        public int Count { get; set; }
    }
}

