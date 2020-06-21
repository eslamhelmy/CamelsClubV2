using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
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
    public class CommentUserActionService : ICommentUserActionService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICommentUserActionRepository _repo;
        private readonly INotificationService _notificationService;
        private readonly IFriendRepository _friendRepository;
        public CommentUserActionService(IUnitOfWork unit, ICommentUserActionRepository repo, IFriendRepository friendRepository, INotificationService notificationService)
        {
            _unit = unit;
            _repo = repo;
            _notificationService = notificationService;
            _friendRepository = friendRepository;
        }
        public PagingViewModel<CommentUserActionViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll()
                        .Where(x => !x.IsDeleted)
                        .Where(x => x.IsActive);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CommentUserActionViewModel> result = new List<CommentUserActionViewModel>();

            var posts = query.Select(obj => new CommentUserActionViewModel
            {
                ID = obj.ID,
                ActionID = obj.ActionID,
                CommentID = obj.CommentID ,
                UserID = obj.UserID
            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CommentUserActionViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public CommentUserActionCreateViewModel Add(CommentUserActionCreateViewModel viewModel)
        {
            CommentUserAction res;
            bool IsLiked = false;
            var commentUserAction =
               _repo.GetAll().FirstOrDefault(x => !x.IsDeleted &&
                                   x.CommentID == viewModel.CommentID &&
                                   x.UserID == viewModel.UserID &&
                                   x.ActionID == viewModel.ActionID
                                   );
            if (commentUserAction != null)
            {
                res = _repo.SaveIncluded(new CommentUserAction
                {
                    ID = viewModel.ID,
                    IsActive = viewModel.IsActive
                }, "IsActive");
            }
            else
            {
                res = _repo.Add(viewModel.ToModel());

                _unit.Save();

                //var com = _repo.GetAll().Where(x=>x.CommentID==res.CommentID).Select(x=>x.Comment).FirstOrDefault();

                var usersIDs = _friendRepository.FriendUsersIDs(viewModel.UserID);
                var notifcation = new NotificationCreateViewModel
                {
                    ContentArabic = "تم الاعجاب بالتعليق  ",
                    ContentEnglish = "Comment has been liked",
                    NotificationTypeID = 8,
                    SourceID = viewModel.UserID,
                    //DestinationID = res.Comment.UserID,
                    ActionID = viewModel.ActionID,
                    CommentID=viewModel.CommentID

                };


                //_notificationService.SendNotifictionForUser(notifcation);
                _notificationService.SendNotifictionForFriends(notifcation, usersIDs);
            }
            return new CommentUserActionCreateViewModel
            {
                ID = res.ID,
                IsActive = res.IsActive,
                ActionID = viewModel.ActionID,
                CommentID = viewModel.CommentID
            };
        }
        public void Edit(CommentUserActionCreateViewModel viewModel)
        {
            _repo.SaveIncluded(viewModel.ToModel(), "ActionID");
        }
        public CommentUserActionViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var postUserAction = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new CommentUserActionViewModel
            {
                ID = obj.ID,
                UserID = obj.UserID,
                CommentID = obj.CommentID,
                ActionID = obj.ActionID
            }).FirstOrDefault();
            
            return postUserAction;

        }


        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }


      }
}

