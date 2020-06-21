using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Models;
using CamelsClub.Models.Enums;
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
    public class CommentService : ICommentService
    {
        //test
        private readonly IUnitOfWork _unit;
        private readonly ICommentRepository _repo;
        private readonly ICommentDocumentRepository _commentDocumentRepository;
        private readonly INotificationService _notificationService;
        private readonly IFriendRepository _friendRepository;
        public CommentService(IUnitOfWork unit, ICommentRepository repo, IFriendRepository friendRepository,ICommentDocumentRepository commentDocumentRepository, INotificationService notificationService)
        {
            _unit = unit;
            _repo = repo;
            _commentDocumentRepository = commentDocumentRepository;
            _notificationService = notificationService;
            _friendRepository = friendRepository;
        }
        public PagingViewModel<CommentViewModel> Search(int postID=0 ,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            
            //get parent comments only

            var query = _repo.GetAll()
                       .Where(post => !post.IsDeleted)
                       .Where(c=>postID == 0 || c.PostID == postID)
                       .Where(c=>!c.ParentCommentID.HasValue);

                       
             


            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CommentViewModel> result = new List<CommentViewModel>();

            var comments = query.Select(obj => new CommentViewModel
            {
                ID = obj.ID,
                Text = obj.Text,
                CreatedDate = obj.CreatedDate,
                UserName = obj.User.UserName,
                UserProfileImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage,
                Documents = obj.CommentDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                {
                    FilePath = protocol + "://" + hostName + "/uploads/Comment-Document/" + doc.FileName,
                    UploadedDate = doc.CreatedDate ,
                    FileType = doc.Type
                    
                }),
                NumberOfLikes = obj.CommentUserActions
                                     .Where(x => x.ActionID == (int)Actions.Like)
                                     .Where(x => !x.IsDeleted)
                                     .Where(x => x.IsActive)
                                     .Count(),
                HasReplies = obj.ChildComments
                               .Where(x=>!x.IsDeleted).Any(),
                RepliesCount = obj.ChildComments
                               .Where(x => !x.IsDeleted).Count(),
                

            }).OrderByPropertyName(orderBy, isAscending);




            result = comments.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CommentViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(CommentCreateViewModel view)
        {

            var insertedcomment = _repo.Add(view.ToCommentModel());

            if (view.Files.Count > 0)
            {
                foreach (var file in view.Files)
                {
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        _commentDocumentRepository.Add(new CommentDocument
                        {
                            CommentID = file.CommentID,
                            FileName = file.FileName,
                            Type = file.FileType.ToString()

                        });
                    }
                    
                }
            }
            _unit.Save();
            var post=_repo.GetAll().Select(x => x.Post).Where(p => p.ID == view.PostID).FirstOrDefault();
            var usersIDs = _friendRepository.FriendUsersIDs(view.UserID);
            var notifcation = new NotificationCreateViewModel
            {
                ContentArabic = $"{NotificationArabicKeys.NewComment} {post.Text} ",
                ContentEnglish = $"{NotificationEnglishKeys.NewComment} {post.Text}",
                NotificationTypeID = 3,
                SourceID = view.UserID,
                DestinationID = post.UserID,
                CommentID = insertedcomment.ID,
                PostID = insertedcomment.PostID
            };
            _notificationService.SendNotifictionForFriends(notifcation, usersIDs);
            //if(notifcation.DestinationID !=notifcation.SourceID)
            //{
            //    _notificationService.SendNotifictionForUser(notifcation);
            //}


        }
        public IEnumerable<CommentViewModel> GetReplies(int commentId)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var replies = _repo.GetAll().Where(c => c.ParentCommentID == commentId).Select(obj => new CommentViewModel
            {
                ID = obj.ID,
                Text = obj.Text,
                CreatedDate = obj.CreatedDate,
                UserName = obj.User.UserName,
                UserProfileImagePath = protocol + "://" + hostName + "/uploads/Comment-Document/" + obj.User.UserProfile.MainImage,
                Documents = obj.CommentDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                {
                    FilePath = protocol + "://" + hostName + "/uploads/Comment-Document/" + doc.FileName,
                    UploadedDate = doc.CreatedDate,
                    FileType = doc.Type

                }),
                HasReplies = obj.ChildComments
                               .Where(x => !x.IsDeleted).Any(),
                RepliesCount = obj.ChildComments
                               .Where(x => !x.IsDeleted).Count(),





            }).ToList();


            return replies;

        }
        public CommentViewModel GetByID(int commentId)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var CommentData = _repo.GetAll().Where(post => post.ID == commentId).Select(obj => new CommentViewModel
            {
                ID = obj.ID,
                Text = obj.Text,
                CreatedDate = obj.CreatedDate,
                UserName = obj.User.UserName,
                
                Documents = obj.CommentDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                {
                    FilePath = protocol + "://" + hostName + "/uploads/Comment-Document/" + doc.FileName,
                    UploadedDate = doc.CreatedDate

                })




            }).FirstOrDefault();


            return CommentData;

        }


        public bool IsExists(int CoomentId)
        {
            return _repo.GetAll().Where(comm => comm.ID == CoomentId).Any();
        }


        public void Edit(CommentCreateViewModel viewModel)
        {
            var CommentData = _repo.GetById(viewModel.ID);
            if (CommentData != null)
            {

                var UpdatedComment = viewModel.ToCommentModel();
                UpdatedComment.ID = CommentData.ID;
                if (viewModel.Files.Count > 0)
                {
                    _commentDocumentRepository.RemoveMany(doc => doc.CommentID == viewModel.ID);
                    foreach (var f in viewModel.Files)
                    {
                        foreach (var file in viewModel.Files)
                        {
                            _commentDocumentRepository.Add(new CommentDocument
                            {
                                CommentID = file.CommentID,
                                FileName = file.FileName,
                                Type = file.FileType.ToString()

                            });
                        }
                    }
                }

                _repo.Edit(UpdatedComment);

            }
        }

        public void Delete(int commentId)
        {
            var CommentData = _repo.GetAll().Where(com => com.ID == commentId).FirstOrDefault();
            if (CommentData != null)
            {
                var CommentDocuments = _commentDocumentRepository.GetAll().Where(doc => doc.CommentID == commentId);
                if (CommentDocuments.Count() > 0)
                {
                    _commentDocumentRepository.RemoveMany(doc => doc.CommentID == commentId);
                }
                _repo.RemoveMany(com=>com.ParentCommentID== commentId);
                _repo.RemoveByIncluded(CommentData);
            }
        }
    }
}

