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
    public class CompetitionInviteService : ICompetitionInviteService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICompetitionInviteRepository _repo;
        private readonly INotificationService _notificationService;


        public CompetitionInviteService(IUnitOfWork unit, ICompetitionInviteRepository repo,INotificationService notificationService)
        {
            _unit = unit;
            _repo = repo;
            _notificationService = notificationService;

        }
        //get associated competitionInvites with that user
        public PagingViewModel<CompetitionInviteViewModel> Search(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(comp => !comp.IsDeleted)
                .Where(x => x.Competition.UserID == userID);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CompetitionInviteViewModel> result = new List<CompetitionInviteViewModel>();

            var CompetitionInvites = query.Select(obj => new CompetitionInviteViewModel
            {
                ID = obj.ID,
            //    CompetitionID = obj.CompetitionID,
                UserName = obj.User.UserName

            }).OrderByPropertyName(orderBy, isAscending);

            result = CompetitionInvites.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CompetitionInviteViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(CompetitionInviteCreateViewModel viewModel)
        {
           
            var insertedCompetitionInvite = _repo.Add(viewModel.ToModel());
            _unit.Save();
            var CompetitionInvite = _repo.GetAll().Where(ch => ch.ID == insertedCompetitionInvite.ID).FirstOrDefault();
            var notifcation = new NotificationCreateViewModel
            {
                ContentArabic = $"{CompetitionInvite.Competition.NameArabic} تم دعوتك للاشتراك بالمسابقة ",
                ContentEnglish = $"You have been joined to competition {CompetitionInvite.Competition.NameArabic}",
                NotificationTypeID = 6,
                SourceID = CompetitionInvite.Competition.UserID,
                DestinationID = viewModel.UserID,
                CompetitionID = viewModel.CompetitionID

            };

             _notificationService.SendNotifictionForUser(notifcation);

        }

        public void Edit(CompetitionInviteEditViewModel viewModel)
        {
          
            _repo.Edit(viewModel.ToModel());

        }

        public CompetitionInviteViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var CompetitionInvite = _repo.GetAll().Where(comp => comp.ID == id)
                .Select(obj => new CompetitionInviteViewModel
                {
                    ID = obj.ID,
              //      CompetitionID = obj.CompetitionID,
                    UserName = obj.User.UserName
                }).FirstOrDefault();

            return CompetitionInvite;
        }

        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }
        public void Delete(int id)
        {
                _repo.Remove(id);
        }
        public bool RejectCompetition(int competitionID, int userID)
        {
            var checker = _repo.GetAll()
                    .Where(x => x.CompetitionID == competitionID && x.UserID == userID)
                    .FirstOrDefault();
            if (checker.JoinDateTime.HasValue)
            {
                throw new Exception("You joined , you can not reject ");
            }
            else
            {
                //do nothing
                checker.RejectDateTime = DateTime.UtcNow;
                _unit.Save();
            }
            return true;
        }
        public bool JoinCompetition(CheckerJoinCompetitionCreateViewModel viewModel)
        {
            var competitor =
               _repo.GetAll()
               .Where(x => x.CompetitionID == viewModel.CompetitionID && !x.IsDeleted)
               .Where(x => x.UserID == viewModel.UserID)
               .FirstOrDefault();

            if (competitor == null)
            {
                throw new Exception("this user is not a checker, your you don't have permission to do that");
            }

            competitor.JoinDateTime = DateTime.UtcNow;
            _unit.Save();

            return true;
        }

    }




}

