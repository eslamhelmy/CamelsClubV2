using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Models.Enums;
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
using CamelsClub.Localization.Shared;

namespace CamelsClub.Services
{

    public class PostService: IPostService
    {

        private readonly IUnitOfWork _unit;
        private readonly IPostRepository _repo;
        private readonly IFriendRepository _friendRepository;
        private readonly IPostDocumentRepository _postDocumentrepo;
        private readonly ICommentRepository _commentRepository;
        private readonly INotificationService _notificationService;
        public PostService(IUnitOfWork unit, IPostRepository repo, IPostDocumentRepository postDocumentrepo, ICommentRepository commentRepository, IFriendRepository friendRepository, INotificationService notificationService)
        {
            _unit = unit;
            _repo = repo;
            _postDocumentrepo = postDocumentrepo;
            _commentRepository = commentRepository;
            _friendRepository = friendRepository;
            _notificationService = notificationService;
        }

        public PagingViewModel<PostDetailsViewModel> GetHomePosts(int userID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

           
            var query = _repo.GetAll()
                            .Where(post =>post.UserID == userID)
                                          .Where(post=>!post.IsDeleted);
            
            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<PostDetailsViewModel> result = new List<PostDetailsViewModel>();

            var posts = query.Select(obj => new PostDetailsViewModel
            {
                ID = obj.ID,
                Text = obj.Text,
                CreatedDate = obj.CreatedDate,
                UserID = obj.UserID,
                IsLiked = obj.PostUserActions.Where(x => x.UserID == userID
                                                    && x.ActionID == (int)Actions.Like
                                                    && x.IsActive && !x.IsDeleted).Any(),
                UserName = obj.User.UserName,
                UserImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage,
                Notes = obj.Notes,
                NumberOfLike = obj.PostUserActions
                                     .Where(x=>x.ActionID == (int) Actions.Like)
                                     .Where(x=>!x.IsDeleted)
                                     .Where(x=>x.IsActive)
                                     .Count(),
                NumberOfComments = obj.Comments
                                     .Where(x => x.PostID == obj.ID)
                                     .Where(x => !x.IsDeleted)
                                     .Count(),
                NumberOfShare = obj.PostUserActions
                                     .Where(x => x.ActionID == (int)Actions.Share)
                                     .Where(x => !x.IsDeleted)
                                     .Count(),

                PostStatus = (PostStatus)obj.PostStatus,
                PostType = (PostType)obj.PostType,
                Documents = obj.PostDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                {
                    FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,
                    UploadedDate = doc.CreatedDate,
                    FileType = doc.Type

                })




            }).OrderByPropertyName(orderBy, isAscending);
            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<PostDetailsViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        public PagingViewModel<PostDetailsViewModel> Search(int userID=0 , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll()
                .Where(post => !post.IsDeleted)
                //post has not reported by this user
                .Where(p => !p.IssueReports.Where(i => i.UserID == userID).Any())
                //user created the post must not be blocked by logged user
                .Where(x => !x.User.FriendsBlockedMe.Any(b => b.UserID == userID));
                // get posts which owners put notification setting to public
              //  .Where(x => x.User.UserNotificationSettings.Where(n => n.NotificationSettingID == (int)NotificationSettingsKey.ViewPosts && n.NotificationSettingValueID == 7).Any());
                

            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;
          
            List<PostDetailsViewModel> result = new List<PostDetailsViewModel>();


            var posts = query.Select(obj => new PostDetailsViewModel
            {
                ID = obj.ID,
                Text = obj.Text,
                UserID = obj.UserID,
                IsLiked = obj.PostUserActions.Where(x=>x.UserID == userID 
                                                    && x.ActionID==(int)Actions.Like
                                                    &&x.IsActive && !x.IsDeleted).Any(), 
                CreatedDate = obj.CreatedDate,
                UserName = obj.User.UserName,
                DisplayName = obj.User.DisplayName,
                UserImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage,
                Notes = obj.Notes,
                LastComment = obj.Comments.OrderByDescending(c=>c.ID).Select(c=>new CommentViewModel {
                    ID = c.ID,
                    Text = c.Text,
                    UserName = c.User.UserName,
                    UserProfileImagePath = c.User.UserProfile != null ? protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage : "",
                    
                }).FirstOrDefault(),
                Comments = obj.Comments.OrderByDescending(c => c.ID).Select(c => new CommentViewModel
                {
                    ID = c.ID,
                    Text = c.Text,
                    UserName = c.User.UserName,
                    UserProfileImagePath = c.User.UserProfile != null ? protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage : "",

                }).ToList(),
                NumberOfLike = obj.PostUserActions
                                     .Where(x => x.ActionID == (int)Actions.Like)
                                     .Where(x => !x.IsDeleted)
                                     .Where(x => x.IsActive)
                                     .Count(),
                NumberOfComments = obj.Comments
                                     .Where(x => x.PostID == obj.ID)
                                     .Where(x => !x.IsDeleted)
                                     .Count(),
                NumberOfShare = obj.PostUserActions
                                     .Where(x => x.ActionID == (int)Actions.Share)
                                     .Where(x => !x.IsDeleted)
                                     .Count(),

                PostStatus = (PostStatus)obj.PostStatus,
                PostType = (PostType)obj.PostType,
                Documents = obj.PostDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                {
                    FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,
                    UploadedDate = doc.CreatedDate,
                    FileType = doc.Type
                })
                
            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<PostDetailsViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }

        //public List<CommentViewModel> GetComments(int postId)
        //{
        //    string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
        //    string hostName = HttpContext.Current.Request.Url.Authority.ToString();

        //    var post =  _repo.GetAll().FirstOrDefault(x => x.ID == postId);
        //    List<CommentViewModel> list =
        //             new List<CommentViewModel>();
        //    foreach (var item in post.Comments)
        //    {
        //        list.Add(new CommentViewModel
        //        {
        //            ID = item.ID,
        //            Text = item.Text,
        //            CreatedDate = item.CreatedDate,
        //            Documents = item.CommentDocuments.Select(d => new DocumentViewModel
        //            {
        //                FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + d.FileName,
        //                UploadedDate = d.CreatedDate

        //            }),
        //            Replies = _commentRepository.GetAll()
        //                        .Where(x=>x.ParentCommentID == item.ID)
        //                        .Select( x => new CommentViewModel
        //                        {
        //                            ID = x.ID,
        //                            Text = x.Text,
        //                            CreatedDate = x.CreatedDate,
        //                            Documents = x.CommentDocuments.Select(d => new DocumentViewModel
        //                            {
        //                                FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + d.FileName,
        //                                UploadedDate = d.CreatedDate

        //                            }),
        //                        })
        //        });
        //    }
        //    return list;
        //}
        public CreatePostViewModel CreatePost (CreatePostViewModel view)
        {
            
            var insertedPost = _repo.Add(view.ToPostModel());
            
            if (view.Files != null && view.Files.Count>0)
            {
                foreach (var file in view.Files)
                {
                    _postDocumentrepo.Add(new PostDocument {

                        FileName = file.FileName,
                        Type=view.PostType == PostType.Image? "Image":"Video",
                        PostID=insertedPost.ID

                    }); 
                }
            }

            var usersIDs = _friendRepository.FriendUsersIDs(view.UserID);
            _unit.Save();

            var user = _repo.GetAll().Where(x => x.ID == insertedPost.ID).Select(p => p.User).FirstOrDefault();
            var notifcation =  new NotificationCreateViewModel
            {
                ContentArabic = $"{NotificationArabicKeys.NewPost} {user.UserName}",
                ContentEnglish = $"{NotificationEnglishKeys.NewPost} {user.UserName}",
                NotificationTypeID = 2,
                SourceID = view.UserID,
                PostID= insertedPost.ID,
               
            };

            _notificationService.SendNotifictionForFriends(notifcation, usersIDs);




        
            return new CreatePostViewModel {
                ID = insertedPost.ID
            };

        }

        public PostDetailsViewModel GetByID(int loggedUserID , int postId)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var PostData = _repo.GetAll().Where(post => post.ID == postId).Select(obj => new PostDetailsViewModel
            {
                ID = obj.ID,
                Text = obj.Text,
                UserID = obj.UserID,
                IsLiked = obj.PostUserActions.Where(x => x.UserID == loggedUserID
                                                    && x.ActionID == (int)Actions.Like
                                                    && x.IsActive && !x.IsDeleted).Any(),
                CreatedDate = obj.CreatedDate,
                UserName = obj.User.UserName,
                UserImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage,
                Notes = obj.Notes,
                LastComment = obj.Comments.OrderByDescending(c => c.ID).Select(c => new CommentViewModel
                {
                    ID = c.ID,
                    Text = c.Text,
                    UserName = c.User.UserName,
                    UserProfileImagePath = c.User.UserProfile != null ? protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage : "",

                }).FirstOrDefault(),
                Comments = obj.Comments.OrderByDescending(c => c.ID).Select(c => new CommentViewModel
                {
                    ID = c.ID,
                    Text = c.Text,
                    UserName = c.User.UserName,
                    UserProfileImagePath = c.User.UserProfile != null ? protocol + "://" + hostName + "/uploads/User-Document/" + c.User.UserProfile.MainImage : "",

                }).ToList(),
                NumberOfLike = obj.PostUserActions
                                     .Where(x => x.ActionID == (int)Actions.Like)
                                     .Where(x => !x.IsDeleted)
                                     .Where(x => x.IsActive)
                                     .Count(),
                NumberOfComments = obj.Comments
                                     .Where(x => x.PostID == obj.ID)
                                     .Where(x => !x.IsDeleted)
                                     .Count(),
                NumberOfShare = obj.PostUserActions
                                     .Where(x => x.ActionID == (int)Actions.Share)
                                     .Where(x => !x.IsDeleted)
                                     .Count(),

                PostStatus = (PostStatus)obj.PostStatus,
                PostType = (PostType)obj.PostType,
                Documents = obj.PostDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                {
                    FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,
                    UploadedDate = doc.CreatedDate,
                    FileType = doc.Type
                })

            }).FirstOrDefault();


            return PostData;

        }


        public bool IsPostExists(int postId)
        {
            return _repo.GetAll().Where(post => post.ID == postId).Any();
        }


        public CreatePostViewModel UpdatePost (CreatePostViewModel viewModel)
        {
            var edited =_repo.Edit(viewModel.ToPostModel());

                if (viewModel.Files!= null && viewModel.Files.Count>0)
                {
                    _postDocumentrepo.RemoveMany(doc => doc.PostID == viewModel.ID);
                    foreach (var f in viewModel.Files)
                    {
                        _postDocumentrepo.Add(new PostDocument
                        {
                            FileName = f.FileName,
                             Type = viewModel.PostType == PostType.Image ? "Image" : "Video",

                        });
                    }
                }
            _unit.Save();
            return new CreatePostViewModel
            {
                ID = edited.ID
            };
            
        }

        public void DeletePost (int postId)
        {
            var postData = _repo.GetAll().Where(post => post.ID == postId).FirstOrDefault();
            if (postData != null)
            {
                var PostDocuments = _postDocumentrepo.GetAll().Where(doc => doc.PostID == postId);
                if (PostDocuments.Count()>0)
                {
                    _postDocumentrepo.RemoveMany(doc => doc.PostID == postId);
                }

                var PostComments=_commentRepository.GetAll().Where(doc => doc.PostID == postId);
                if(PostComments.Count()>0)
                {
                    _commentRepository.RemoveMany(doc => doc.PostID == postId);
                }

                _repo.RemoveByIncluded(postData);
            }
        }
    }
}
