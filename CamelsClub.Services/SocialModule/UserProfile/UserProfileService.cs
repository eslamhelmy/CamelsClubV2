using CamelsClub.Data.Extentions;
using CamelsClub.Data.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
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
    public class UserProfileService : IUserProfileService
    {
        //test
        private readonly IUnitOfWork _unit;
        private readonly IUserProfileRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IProfileImageRepository _profileImageRepository;
        private readonly IProfileVideoRepository _profileVideoRepository;

        private readonly IUserConfirmationMessageRepository _userConfirmationMessagerepo;
        public UserProfileService(IUnitOfWork unit,
                                  IUserProfileRepository repo,
                                  IProfileImageRepository profileImageRepository,
                                  IProfileVideoRepository profileVideoRepository,
                                  IUserRepository userRepository,
                                  IUserConfirmationMessageRepository userConfirmationMessageRepository
                                )
        {
            _unit = unit;
            _repo = repo;
            _profileImageRepository = profileImageRepository;
            _profileVideoRepository = profileVideoRepository;
            _userRepository = userRepository;
            _userConfirmationMessagerepo = userConfirmationMessageRepository;



    }
        public PagingViewModel<UserProfileViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            //get parent comments only

            var query = _repo.GetAll()
                       .Where(post => !post.IsDeleted);
            
            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<UserProfileViewModel> result = new List<UserProfileViewModel>();

            var posts = query.Select(obj => new UserProfileViewModel
            {
                ID = obj.ID,
                BirthDate = obj.BirthDate,
                MainImagePath = protocol + "://" + hostName + "/uploads/UserProfile-Document/" + obj.MainImage,
                CoverImagePath = protocol + "://" + hostName + "/uploads/UserProfile-Document/" + obj.CoverImage,

                ProfileImages = obj.ProfileImages.Where(doc => !doc.IsDeleted).Select(doc => new ProfileImageViewModel
                {
                    Path = protocol + "://" + hostName + "/uploads/UserProfile-Document/" + doc.FileName,
                    
                }),
                ProfileVideos = obj.ProfileVideos.Where(doc => !doc.IsDeleted).Select(doc => new ProfileVideoViewModel
                {
                    Path = protocol + "://" + hostName + "/uploads/UserProfile-Document/" + doc.FileName,

                })
                
            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<UserProfileViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(UserProfileCreateViewModel viewModel)
        {

            var profile = _repo.Add(viewModel.ToModel());

            if (viewModel.ProfileImages != null && viewModel.ProfileImages.Count() > 0)
            {
                foreach (var file in viewModel.ProfileImages)
                {
                    _profileImageRepository.Add(new ProfileImage
                    {
                        UserProfileID=file.UserProfileID,
                        FileName = file.FileName
                        
                    });
                }
            }

            if (viewModel.ProfileVideos != null && viewModel.ProfileVideos.Count() > 0)
            {
                foreach (var file in viewModel.ProfileVideos)
                {
                    _profileVideoRepository.Add(new ProfileVideo
                    {
                        UserProfileID = file.UserProfileID,
                        FileName = file.FileName

                    });
                }
            }

        }
        public UserProfileCreateViewModel GetEditableByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var userProfileData = _repo.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Select(obj => new UserProfileCreateViewModel
                                {
                                    ID = obj.ID,
                                    BirthDate = obj.BirthDate,
                                    MainImage = obj.MainImage,
                                    ProfileImages= obj.ProfileImages.Where(doc => !doc.IsDeleted).Select(doc => new ProfileImageCreateViewModel
                                    {
                                        FileName =doc.FileName,

                                    }).ToList(),
                                    ProfileVideos = obj.ProfileVideos.Where(doc => !doc.IsDeleted).Select(doc => new ProfileVideoCreateViewModel
                                    {
                                        FileName = doc.FileName

                                    }).ToList()

                                }).FirstOrDefault();


            return userProfileData;

        }

        public UserProfileViewModel GetByID(int loggedUserID, int userId)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            var userProfileData = _userRepository.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.ID == userId)
                                .Select(obj => new UserProfileViewModel
                                {
                                    ID = obj.ID,
                                    BirthDate = obj.UserProfile.BirthDate,
                                    Name = obj.UserName,
                                    Address = obj.UserProfile.Address,
                                    Email = obj.Email,
                                    DisplayName = obj.DisplayName,
                                    CamelsCount = obj.Camels.Where(x=> !x.IsDeleted).Count(),
                                    GroupsCount = obj.Groups.Where(g=>!g.IsDeleted).Count(),
                                    NumberOfCompetitionsJoined = obj.CompetitionInvites
                                                            .Where(x => !x.IsDeleted && x.SubmitDateTime != null)
                                                            .Count(),
                                    NumberOfFriends = obj.Friends.Where(x => !x.IsDeleted && !x.IsBlocked).Count() +
                                                      obj.FriendUsers.Where(x => !x.IsDeleted && !x.IsBlocked).Count(),
                                    HasSentFriendReuestToMine = obj.FromFriendRequests
                                                     .Where(r => !r.IsDeleted)
                                                    .Where(r => r.ToUserID == loggedUserID)
                                                    .Where(r => r.Status == (int)FriendRequestStatus.Pending )
                                                    .Any(),
                                    HasReceivedFriendRequestFromMine = obj.ToFriendRequests
                                                        .Where(r => !r.IsDeleted)
                                                        .Where(r => r.FromUserID == loggedUserID)
                                                       .Where(r => r.Status == (int)FriendRequestStatus.Pending)
                                                        .Any(),
                                    IsFriend = obj.Friends.Where(f => !f.IsDeleted).Where(f => f.FriendUserID == loggedUserID).Any() || obj.FriendUsers.Where(f => !f.IsDeleted).Where(f => f.UserID == loggedUserID).Any(),
                                    IsBlocked = obj.FriendsBlockedMe.Where(f => !f.IsDeleted).Where(f => f.UserID == loggedUserID).Any() 
                                                        || obj.BlockedFriends.Where(f => !f.IsDeleted).Where(f=> f.BlockedFriendID == loggedUserID).Any(),//kindly note you should check for block users 

                                    NumberOfRefereesJoined = obj.CompetitionReferees
                                    .Where(x => !x.IsDeleted)
                                    .Where(x => x.JoinDateTime != null)
                                    .Count(),
                                    UserName = obj.UserName,
                                    PhoneNumber = obj.Phone,
                                    Gender = obj.UserProfile.GenderID,
                                    MainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.UserProfile.MainImage,
                                    CoverImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.UserProfile.CoverImage,

                                    ProfileImages = obj.Posts.Where(post => !post.IsDeleted).SelectMany(post => post.PostDocuments).Where(doc => !doc.IsDeleted && (doc.Type.Contains("Image") || doc.Type.Contains("image"))).OrderByDescending(doc=>doc.ID).Select(doc => new ProfileImageViewModel
                                    {
                                        Path = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,

                                    }),
                                    ProfileVideos = obj.Posts.Where(post => !post.IsDeleted).SelectMany(post => post.PostDocuments).Where(doc => !doc.IsDeleted && (doc.Type.Contains("video") || doc.Type.Contains("Video"))).OrderByDescending(doc => doc.ID).Select(doc => new ProfileVideoViewModel
                                    {
                                        Path = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,

                                    }),
                                    Camels = obj.Camels.Select(x => new CamelViewModel
                                    {
                                        ID = x.ID,
                                        BirthDate = x.BirthDate,
                                        CategoryArabicName = x.Category.NameArabic,
                                        CategoryEnglishName = x.Category.NameEnglish,
                                        CategoryID = x.CategoryID,
                                        Details = x.Details,
                                        FatherName = x.FatherName,
                                        Location = x.Location,
                                        MotherName = x.MotherName,
                                        Name = x.Name,
                                        Code = x.Code,
                                        GenderID = x.GenderConfigDetailID,
                                        GenderName = x.GenderConfigDetail.NameArabic,
                                        camelDocuments = x.CamelDocuments.Where(doc => !doc.IsDeleted).OrderByDescending(doc => doc.ID).Select(doc => new CamelDocumentViewModel
                                        {
                                            FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                                            UploadedDate = doc.CreatedDate,
                                            FileType = doc.Type
                                        }).ToList()
                                    }).OrderByDescending(x=>x.ID).ToList(),
                                        Posts = obj.Posts.Where(p=>!p.IsDeleted).Select(p=> new PostDetailsViewModel
                                    {
                                        ID = p.ID,
                                        Text = p.Text,
                                        CreatedDate = p.CreatedDate,
                                        UserName = p.User.UserName,
                                        Notes = p.Notes,
                                        PostType = (PostType)p.PostType,
                                        NumberOfLike = p.PostUserActions
                                                .Where(x => x.ActionID == (int)Actions.Like)
                                                    .Where(x => !x.IsDeleted)
                                                        .Where(x => x.IsActive)
                                                            .Count(),
                                        NumberOfComments = p.Comments
                                               .Where(x => x.PostID == obj.ID)
                                                    .Where(x => !x.IsDeleted)
                                                        .Count(),
                                        PostStatus = (PostStatus)p.PostStatus,

                                        Comments = p.Comments.Where(com => !com.IsDeleted).Select(com => new CommentViewModel
                                        {

                                            ID = com.ID,
                                            Text = com.Text,
                                            CreatedDate = com.CreatedDate,
                                            UserName = com.User.UserName
                                        }),
                                        Documents = p.PostDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                                        {
                                            FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,
                                            UploadedDate = doc.CreatedDate

                                        })


                                    }).OrderByDescending(x=>x.ID).ToList()

                                }).FirstOrDefault();

            userProfileData.Email =
                !string.IsNullOrWhiteSpace(userProfileData.Email) ?
                                SecurityHelper.Decrypt(userProfileData.Email) : "";  //email is optional
            userProfileData.PhoneNumber =
                                SecurityHelper.Decrypt(userProfileData.PhoneNumber);
            return userProfileData;

        }
        //for admin only
        public UserProfileViewModel GetProfileByID(int userId)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            var userProfileData = _userRepository.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.ID == userId)
                                .Select(obj => new UserProfileViewModel
                                {
                                    ID = obj.ID,
                                    BirthDate = obj.UserProfile.BirthDate,
                                    Name = obj.UserName,
                                    Address = obj.UserProfile.Address,
                                    Email = obj.Email,
                                    DisplayName = obj.DisplayName,
                                    CamelsCount = obj.Camels.Where(x => !x.IsDeleted).Count(),
                                    GroupsCount = obj.Groups.Where(g => !g.IsDeleted).Count(),
                                    NumberOfCompetitionsJoined = obj.CompetitionInvites
                                                            .Where(x => !x.IsDeleted && x.SubmitDateTime != null)
                                                            .Count(),
                                    NumberOfFriends = obj.Friends.Where(x => !x.IsDeleted && !x.IsBlocked).Count() +
                                                      obj.FriendUsers.Where(x => !x.IsDeleted && !x.IsBlocked).Count(),
                                  
                                    NumberOfRefereesJoined = obj.CompetitionReferees
                                    .Where(x => !x.IsDeleted)
                                    .Where(x => x.JoinDateTime != null)
                                    .Count(),
                                    UserName = obj.UserName,
                                    PhoneNumber = obj.Phone,
                                    Gender = obj.UserProfile.GenderID,
                                    MainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.UserProfile.MainImage,
                                    CoverImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + obj.UserProfile.CoverImage,

                                    ProfileImages = obj.Posts.Where(post => !post.IsDeleted).SelectMany(post => post.PostDocuments).Where(doc => !doc.IsDeleted && (doc.Type.Contains("Image") || doc.Type.Contains("image"))).OrderByDescending(doc => doc.ID).Select(doc => new ProfileImageViewModel
                                    {
                                        Path = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,

                                    }),
                                    ProfileVideos = obj.Posts.Where(post => !post.IsDeleted).SelectMany(post => post.PostDocuments).Where(doc => !doc.IsDeleted && (doc.Type.Contains("video") || doc.Type.Contains("Video"))).OrderByDescending(doc => doc.ID).Select(doc => new ProfileVideoViewModel
                                    {
                                        Path = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,

                                    }),
                                    Camels = obj.Camels.Select(x => new CamelViewModel
                                    {
                                        ID = x.ID,
                                        BirthDate = x.BirthDate,
                                        CategoryArabicName = x.Category.NameArabic,
                                        CategoryEnglishName = x.Category.NameEnglish,
                                        CategoryID = x.CategoryID,
                                        Details = x.Details,
                                        FatherName = x.FatherName,
                                        Location = x.Location,
                                        MotherName = x.MotherName,
                                        Name = x.Name,
                                        Code = x.Code,
                                        GenderID = x.GenderConfigDetailID,
                                        GenderName = x.GenderConfigDetail.NameArabic,
                                        camelDocuments = x.CamelDocuments.Where(doc => !doc.IsDeleted).OrderByDescending(doc => doc.ID).Select(doc => new CamelDocumentViewModel
                                        {
                                            FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                                            UploadedDate = doc.CreatedDate,
                                            FileType = doc.Type
                                        }).ToList()
                                    }).OrderByDescending(x => x.ID).ToList(),
                                    Posts = obj.Posts.Where(p => !p.IsDeleted).Select(p => new PostDetailsViewModel
                                    {
                                        ID = p.ID,
                                        Text = p.Text,
                                        CreatedDate = p.CreatedDate,
                                        UserName = p.User.UserName,
                                        Notes = p.Notes,
                                        PostType = (PostType)p.PostType,
                                        NumberOfLike = p.PostUserActions
                                              .Where(x => x.ActionID == (int)Actions.Like)
                                                  .Where(x => !x.IsDeleted)
                                                      .Where(x => x.IsActive)
                                                          .Count(),
                                        NumberOfComments = p.Comments
                                             .Where(x => x.PostID == obj.ID)
                                                  .Where(x => !x.IsDeleted)
                                                      .Count(),
                                        PostStatus = (PostStatus)p.PostStatus,

                                        Comments = p.Comments.Where(com => !com.IsDeleted).Select(com => new CommentViewModel
                                        {

                                            ID = com.ID,
                                            Text = com.Text,
                                            CreatedDate = com.CreatedDate,
                                            UserName = com.User.UserName
                                        }),
                                        Documents = p.PostDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                                        {
                                            FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,
                                            UploadedDate = doc.CreatedDate

                                        })


                                    }).OrderByDescending(x => x.ID).ToList()

                                }).FirstOrDefault();

            userProfileData.Email =
                !string.IsNullOrWhiteSpace(userProfileData.Email) ?
                                SecurityHelper.Decrypt(userProfileData.Email) : "";  //email is optional
            userProfileData.PhoneNumber =
                                SecurityHelper.Decrypt(userProfileData.PhoneNumber);
            return userProfileData;

        }

        public UserProfileViewModel GetMyProfile(int userId)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            var userProfileData = _userRepository.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.ID == userId)
                                .Select(obj => new UserProfileViewModel
                                {
                                    ID = obj.ID,
                                    BirthDate = obj.UserProfile.BirthDate,
                                    Name = obj.UserName,
                                    UserName = obj.UserName,
                                    DisplayName = obj.DisplayName,
                                    CamelsCount = obj.Camels.Where(x => !x.IsDeleted).Count(),
                                    GroupsCount = obj.Groups.Where(g => !g.IsDeleted).Count(),
                                    NumberOfCompetitionsJoined = obj.CompetitionInvites
                                                            .Where(x => !x.IsDeleted && x.SubmitDateTime != null)
                                                            .Count(),
                                    NumberOfFriends = obj.Friends.Where(x=>!x.IsDeleted && !x.IsBlocked).Count() +
                                                      obj.FriendUsers.Where(x => !x.IsDeleted && !x.IsBlocked).Count(),
                                    NumberOfRefereesJoined = obj.CompetitionReferees
                                    .Where(x=>!x.IsDeleted)
                                    .Count(),
                                    Address = obj.UserProfile.Address,
                                    Email=obj.Email,
                                    PhoneNumber=obj.Phone,
                                    Gender = obj.UserProfile.GenderID ,
                                    MainImagePath = obj.UserProfile.MainImage != null ? protocol + "://" + hostName + "/uploads/User-Document/" + obj.UserProfile.MainImage : "",
                                    CoverImagePath = obj.UserProfile.CoverImage != null ? protocol + "://" + hostName + "/uploads/User-Document/" + obj.UserProfile.CoverImage : "",

                                    ProfileImages = obj.Posts.Where(post => !post.IsDeleted).SelectMany(post=>post.PostDocuments).Where(doc=>!doc.IsDeleted && (doc.Type.Contains("Image") || doc.Type.Contains("image"))).OrderByDescending(doc => doc.ID).Select(doc => new ProfileImageViewModel
                                    {
                                        Path = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,

                                    }),
                                    Posts = obj.Posts.Where(p => !p.IsDeleted).Select(p => new PostDetailsViewModel
                                    {
                                        ID = p.ID,
                                        Text = p.Text,
                                        CreatedDate = p.CreatedDate,
                                        UserName = p.User.UserName,
                                        DisplayName = p.User.DisplayName,
                                        Notes = p.Notes,
                                        PostType = (PostType)p.PostType,
                                        NumberOfLike = p.PostUserActions
                                                  .Where(x => x.ActionID == (int)Actions.Like)
                                                      .Where(x => !x.IsDeleted)
                                                          .Where(x => x.IsActive)
                                                              .Count(),
                                        NumberOfComments = p.Comments
                                                 .Where(x => x.PostID == obj.ID)
                                                      .Where(x => !x.IsDeleted)
                                                          .Count(),
                                        PostStatus = (PostStatus)p.PostStatus,

                                        Comments = p.Comments.Where(com => !com.IsDeleted).Select(com => new CommentViewModel
                                        {

                                            ID = com.ID,
                                            Text = com.Text,
                                            CreatedDate = com.CreatedDate,
                                            UserName = com.User.UserName
                                        }),
                                        Documents = p.PostDocuments.Where(doc => !doc.IsDeleted).Select(doc => new DocumentViewModel
                                        {
                                            FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,
                                            UploadedDate = doc.CreatedDate

                                        })


                                    }).OrderByDescending(x=>x.ID).ToList(),
                                    Camels = obj.Camels.Select(x => new CamelViewModel
                                    {
                                        ID = x.ID,
                                        BirthDate = x.BirthDate,
                                        CategoryArabicName = x.Category.NameArabic,
                                        CategoryEnglishName = x.Category.NameEnglish,
                                        CategoryID = x.CategoryID,
                                        Details = x.Details,
                                        FatherName = x.FatherName,
                                        Location = x.Location,
                                        MotherName = x.MotherName,
                                        Name = x.Name,
                                        Code = x.Code,
                                        GenderID = x.GenderConfigDetailID,
                                        GenderName = x.GenderConfigDetail.NameArabic,
                                        camelDocuments = x.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
                                        {
                                            FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                                            UploadedDate = doc.CreatedDate,
                                            FileType = doc.Type
                                        }).ToList()

                                    }).OrderByDescending(x => x.ID).ToList(),
                                ProfileVideos = obj.Posts.Where(post => !post.IsDeleted).SelectMany(post => post.PostDocuments).Where(doc => !doc.IsDeleted && (doc.Type.Contains("video") || doc.Type.Contains("Video"))).OrderByDescending(doc => doc.ID).Select(doc => new ProfileVideoViewModel
                                    {
                                        Path = protocol + "://" + hostName + "/uploads/Post-Document/" + doc.FileName,

                                    })
                                 
                                }).FirstOrDefault();

            userProfileData.Email =
                !string.IsNullOrWhiteSpace(userProfileData.Email) ? 
                                SecurityHelper.Decrypt(userProfileData.Email):"";  //email is optional
            userProfileData.PhoneNumber = 
                                SecurityHelper.Decrypt(userProfileData.PhoneNumber);
            return userProfileData;

        }

        public List<DocumentViewModel> GetMyProfileImages(int userId)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            var userProfileData = _userRepository.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.ID == userId)
                                .SelectMany(x => x.Posts)
                                .SelectMany(x => x.PostDocuments)
                                .Where(x => x.Type == "Image" || x.Type == "image")
                                .Select(x => new DocumentViewModel
                                {
                                    
                                    FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + x.FileName,
                                }).ToList();
            return userProfileData;

        }
        public List<DocumentViewModel> GetMyProfileVideos(int userId)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            var userProfileData = _userRepository.GetAll()
                                .Where(x => !x.IsDeleted)
                                .Where(x => x.ID == userId)
                                .SelectMany(x => x.Posts)
                                .SelectMany(x => x.PostDocuments)
                                .Where(x => x.Type == "Video" || x.Type =="video")
                                .Select(x => new DocumentViewModel
                                {

                                    FilePath = protocol + "://" + hostName + "/uploads/Post-Document/" + x.FileName,
                                }).ToList();
            return userProfileData;

        }

        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id && !x.IsDeleted).Any();
        }


        public string Edit(UserProfileCreateViewModel viewModel)
        {
            string Code = "";
            if(string.IsNullOrWhiteSpace( viewModel.PhoneNumber))
            viewModel.PhoneNumber = SecurityHelper.Encrypt(viewModel.PhoneNumber);
            if (!string.IsNullOrWhiteSpace(viewModel.Email))
            {
                viewModel.Email = SecurityHelper.Encrypt(viewModel.Email);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.Email))
            {
                if (_userRepository.IsEmailNoteUnique(viewModel.Email,viewModel.ID))
                    throw new Exception(Resource.EmailAreadyExist);
            }
            viewModel.PhoneNumber = SecurityHelper.Encrypt(viewModel.PhoneNumber);

            if (_userRepository.IsPhoneNoteUnique(viewModel.PhoneNumber, viewModel.ID))
                throw new Exception(Resource.PhoneNumberAreadyExist);

            if (!_userRepository.HasSamePhone(viewModel.PhoneNumber))
            {
                var verificationCode = SecurityHelper.GetRandomNumber();
                while (!_userConfirmationMessagerepo.IsValidCode(verificationCode.ToString(),viewModel.ID))
                {
                    verificationCode = SecurityHelper.GetRandomNumber();
                }

                _userConfirmationMessagerepo
                        .Add(new UserConfirmationMessage
                        {
                            UserID = viewModel.ID,
                            Code = verificationCode.ToString(),
                            CreatedBy = viewModel.ID.ToString(),
                            CreatedDate = DateTime.UtcNow
                        });

                Code = verificationCode.ToString();
            }

            if(_repo.GetAll().Where(x=>x.ID == viewModel.ID && !x.IsDeleted).FirstOrDefault() == null)
            {
                _repo.Add(viewModel.ToModel());
            }
            else
            {
                var UserProfileData = _repo.Edit(viewModel.ToModel());

            }
            var user = _userRepository.SaveExcluded(viewModel.ToUserModel(),"NID","UserName");

            return Code;
        }

        public void Delete(int id)
        {
            _repo.Remove(id);
        }
    }
}

