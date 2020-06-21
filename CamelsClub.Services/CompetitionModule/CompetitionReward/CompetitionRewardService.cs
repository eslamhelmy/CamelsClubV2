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
    public class CompetitionRewardService : ICompetitionRewardService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionRewardRepository _repo;


        public CompetitionRewardService(IUnitOfWork unit, ICompetitionRewardRepository repo)
        {
            _unit = unit;
            _repo = repo;

        }
        //get associated competitionRewards with that user
        public PagingViewModel<CompetitionRewardViewModel> Search(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
                .Where(x => x.Competition.UserID == userID);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionRewardViewModel> result = new List<CompetitionRewardViewModel>();

            var CompetitionRewards = query.Select(obj => new CompetitionRewardViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic,
                NamEnglish = obj.NameEnglish,
                CompetitionID = obj.CompetitionID,
                CompetitionNameArabic = obj.Competition.NameArabic,
                CompetitionNameEnglish = obj.Competition.NamEnglish
            }).OrderByPropertyName(orderBy, isAscending);

            result = CompetitionRewards.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CompetitionRewardViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(CompetitionRewardCreateViewModel viewModel)
        {
           
            var insertedCompetitionReward = _repo.Add(viewModel.ToModel());


        }

        public void Edit(CompetitionRewardEditViewModel viewModel)
        {
          
            _repo.Edit(viewModel.ToModel());

        }

        public CompetitionRewardViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var CompetitionReward = _repo.GetAll().Where(comp => comp.ID == id)
                .Select(obj => new CompetitionRewardViewModel
                {
                    ID = obj.ID,
                    NameArabic = obj.NameArabic,
                    NamEnglish = obj.NameEnglish,
                    CompetitionID = obj.CompetitionID,
                    CompetitionNameArabic = obj.Competition.NameArabic,
                    CompetitionNameEnglish = obj.Competition.NamEnglish

                }).FirstOrDefault();

            return CompetitionReward;
        }

        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }
        public void Delete(int id)
        {
                _repo.Remove(id);
        }
    }

 
      
    
}

