using CamelsClub.Data.Extentions;
using CamelsClub.Data.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unit;
        private readonly IUserRepository _repo;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserConfirmationMessageRepository _userConfirmationMessagerepo;
        private readonly ITokenRepository _tokenRepository;
        public UserService(IUnitOfWork unit , 
            IUserRepository repo,
            IUserRoleService userRoleService,
            ITokenRepository tokenRepository,
            IUserConfirmationMessageRepository userConfirmationMessagerepo
            )
        {
            _unit = unit;
            _userRoleService = userRoleService;
            _repo = repo;
            _tokenRepository = tokenRepository;
            _userConfirmationMessagerepo = userConfirmationMessagerepo;
        }

        public bool SignOut(int userID, string accessToken)
        {
            var encryptedToken = SecurityHelper.Encrypt(accessToken);
            var item = _tokenRepository.GetAll()
                .FirstOrDefault(i => !i.IsDeleted && i.UserID == userID && i.TokenGUID == encryptedToken);
            if (item != null)
            {
                item.Active = false;
                item.LoggedOutDate = DateTime.Now;
                _unit.Save();
                return true;
            }
            return false;
        }
        public ConfirmationMessageViewModel Register(CreateUserViewModel viewModel)
        {
            var notEncryptedPhone = viewModel.MobileNumber;
            if (!string.IsNullOrWhiteSpace(viewModel.Email))
            {
                viewModel.Email =
                SecurityHelper.Encrypt(viewModel.Email.ToLower().Trim());

                if (_repo.IsEmailAlreadyExists(viewModel.Email))
                     throw new Exception(Resource.EmailAreadyExist);

                
            }
            viewModel.MobileNumber =
             SecurityHelper.Encrypt(viewModel.MobileNumber.ToLower().Trim());
           
                if (_repo.IsPhoneAlreadyExists(viewModel.MobileNumber))
                    throw new Exception(Resource.PhoneNumberAreadyExist);

            viewModel.UserName = viewModel.UserName.ToLower();
            if (_repo.IsUserNameAlreadyExists(viewModel.UserName))
                throw new Exception(Resource.UserNameAlreadyExist);

            viewModel.NID =
                   SecurityHelper.Encrypt(viewModel.NID.ToLower().Trim());

                var insertedUser = _repo.Add(viewModel.ToUserModel());

                _userRoleService.InsertUserRole(insertedUser.ID, Roles.User);

                var verificationCode = SecurityHelper.GetRandomNumber().ToString();

            while (_userConfirmationMessagerepo.GetAll()
                .Any(x => x.Code == verificationCode && !x.IsDeleted && x.UserID == insertedUser.ID))
            {
                verificationCode = SecurityHelper.GetRandomNumber().ToString();
            }
            _userConfirmationMessagerepo
                .Add(new UserConfirmationMessage
                    {
                        UserID = insertedUser.ID,
                        Code = verificationCode.ToString(),
                        IsDeleted = false,
                        CreatedBy = insertedUser.ID.ToString(),
                        CreatedDate = DateTime.UtcNow,
                        
                    });
                _unit.Save();
            var userViewModel = new ConfirmationMessageViewModel { UserID = insertedUser.ID, Code = verificationCode , UserName = viewModel.UserName, Phone =  notEncryptedPhone};
                return userViewModel;

            }

        //public bool SetUserName(List<ChangeUserNameViewModel> viewModels)
        //{
        //    var decrypted = SecurityHelper.Decrypt("B0EaWq/xzU9wUnoc0mZwpA==");
        //    foreach (var item in viewModels)
        //    {
        //        var encryptedPhone = SecurityHelper.Encrypt(item.Phone);
        //        var user = _repo.GetAll().Where(x => x.Phone == encryptedPhone).FirstOrDefault();
        //        if(user != null)
        //        {
        //            user.UserName = item.UserName;
        //            user.DisplayName = item.DisplayName;

        //        }
        //        else
        //        {
        //            _repo.Add(new User
        //            {
        //                Phone = encryptedPhone,
        //                UserName = item.UserName,
        //                DisplayName = item.DisplayName,
        //                NID = SecurityHelper.Encrypt("123456789")
                       
        //            });
        //        }

        //    }
        //    _unit.Save();

        //    return true;

        //}
        //public class ChangeUserNameViewModel
        //{
        //    public string UserName { get; set; }
        //    public string DisplayName { get; set; }
        //    public string Phone { get; set; }
        //}
        public bool IsUserNameExists(string userName)
        {
            return _repo.IsUserNameAlreadyExists(userName);
        }
        public ConfirmationMessageViewModel Login(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new Exception(Resource.PhoneRequired);

            phone = SecurityHelper.Encrypt(phone.ToLower().Trim());

            if (!_repo.IsPhoneAlreadyExists(phone))
                throw new Exception(Resource.UserNotFound);

            var user = _repo.GetUserByPhone(phone);

            if (user == null)
                throw new Exception(Resource.UserNotFound);

            var VerificationCode = SecurityHelper.GetRandomNumber();

            _userConfirmationMessagerepo
                    .Add( new UserConfirmationMessage
                          {
                             UserID = user.ID,
                             Code = VerificationCode.ToString() ,
                             CreatedBy = user.ID.ToString() ,
                             CreatedDate = DateTime.UtcNow 
                    });

                return new ConfirmationMessageViewModel { UserID = user.ID, Code = VerificationCode.ToString() };
            
        }

        public ConfirmationMessageViewModel AdminLogin(AdminLoginCreateViewModel viewModel)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();
            viewModel.Email = SecurityHelper.Encrypt(viewModel.Email.ToLower());
            viewModel.Password = SecurityHelper.GetHashedString(viewModel.Password);
            var user = _repo.GetAll().Where(x => x.Email == viewModel.Email && x.Password == viewModel.Password)
                .Select(x =>
             new ConfirmationMessageViewModel
             {
                 UserID = x.ID,
                 UserName = x.UserName,
                 ProfileImagePath = x.UserProfile.MainImage != null? protocol + "://" + hostName + "/uploads/User-Document/" + x.UserProfile.MainImage:"",

             }).FirstOrDefault();
            
            if(user == null)
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }

        public PagingViewModel<UserViewModel> Search(string text, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x => x.DisplayName.Contains(text))
                .Where(x => x.UserName != "admin");



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;
            
            List<UserViewModel> result = new List<UserViewModel>();

            var users = query.OrderByPropertyName(orderBy, isAscending)
                      .Skip(excludedRows).Take(pageSize).ToList();
            foreach (var item in users)
            {
                result.Add(new UserViewModel
                {
                    ID = item.ID,
                    UserMainImagePath = item.UserProfile?.MainImage != null && item.UserProfile?.MainImage != "" ? protocol + "://" + hostName + "/uploads/User-Document/" + item?.UserProfile?.MainImage : "",
                    UserName = item.UserName,
                    DisplayName = item.DisplayName,
                    Email = string.IsNullOrWhiteSpace(item.Email)?item.Email : SecurityHelper.Decrypt(item.Email),
                    Phone = SecurityHelper.Decrypt(item.Phone)
                });

            }
            foreach (var item in result)
            {
                var nameParts = item.DisplayName.Split(' ');
                for (int i = 0; i < nameParts.Length; i++)
                {
                    if (nameParts[nameParts.Length-1-i].Contains(text))
                    {
                        item.SyllableContainsText = nameParts.Length - 1 - i;
                    }
                }
                
            }
            result = result.OrderBy(x => x.SyllableContainsText).ToList();
            return new PagingViewModel<UserViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };

        }

        public PagingViewModel<AdminViewModel> GetAllAdminUsers(string text, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x => x.DisplayName.Contains(text))
                .Where(x => x.UserRoles.Any(r=>r.RoleID == (int)Roles.Admin));



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<AdminViewModel> result = new List<AdminViewModel>();

            var users = query.OrderByPropertyName(orderBy, isAscending)
                      .Skip(excludedRows).Take(pageSize).ToList();
            foreach (var item in users)
            {
                result.Add(new AdminViewModel
                {
                    ID = item.ID,
                    UserMainImagePath = item.UserProfile?.MainImage != null && item.UserProfile?.MainImage != "" ? protocol + "://" + hostName + "/uploads/User-Document/" + item?.UserProfile?.MainImage : "",
                    UserName = item.UserName,
                    DisplayName = item.DisplayName,
                    Email = string.IsNullOrWhiteSpace(item.Email) ? item.Email : SecurityHelper.Decrypt(item.Email),
                    Phone = SecurityHelper.Decrypt(item.Phone),
                    Role = Roles.Admin.ToString()
                });

            }
            foreach (var item in result)
            {
                var nameParts = item.DisplayName.Split(' ');
                for (int i = 0; i < nameParts.Length; i++)
                {
                    if (nameParts[nameParts.Length - 1 - i].Contains(text))
                    {
                        item.SyllableContainsText = nameParts.Length - 1 - i;
                    }
                }

            }
            result = result.OrderBy(x => x.SyllableContainsText).ToList();
            return new PagingViewModel<AdminViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };

        }
        // get my friendRequests
        public PagingViewModel<UserSearchViewModel> FindUsers(int userID ,string text, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var lowerText = !string.IsNullOrWhiteSpace(text) ? text.ToLower() : null ;
            var query = _repo.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x => x.ID != userID);
              
            if(lowerText != null)
            {
                query = query.Where(x => x.DisplayName.Contains(text) || x.UserName.Contains(text) || x.DisplayName.Contains(lowerText) || x.UserName.Contains(lowerText));

            }


            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;


         
            List<UserSearchViewModel> result = new List<UserSearchViewModel>();

            var users =
            query.OrderByPropertyName(orderBy, isAscending)
                      .Skip(excludedRows).Take(pageSize).
                      Select(x => new UserSearchViewModel {
                          ID = x.ID,
                          UserMainImagePath = protocol + "://" + hostName + "/uploads/User-Document/" + x.UserProfile.MainImage,
                          UserName = x.UserName,
                          DisplayName = x.DisplayName,
                          HasSentFriendReuestToMine = x.FromFriendRequests
                                                     .Where(r=> !r.IsDeleted)
                                                    .Where(r => r.ToUserID == userID && r.Status == (int)FriendRequestStatus.Pending)
                                                  //  .Where(r => r.Status == (int)FriendRequestStatus.Pending )
                                                    .Any() ,
                          HasReceivedFriendRequestFromMine = x.ToFriendRequests
                                                        .Where(r => !r.IsDeleted)
                                                        .Where(r => r.FromUserID == userID)
                                                        .Where(r => r.Status == (int)FriendRequestStatus.Pending)
                                                        .Any() ,
                          IsFriend =  x.Friends.Where(f=>!f.IsDeleted && !f.IsBlocked).Where(f => f.FriendUserID == userID).Any() || x.FriendUsers.Where(f => !f.IsDeleted && !f.IsBlocked).Where(f => f.UserID == userID).Any(),
                          IsBlocked = x.Friends.Where(f => !f.IsDeleted && f.IsBlocked).Where(f => f.FriendUserID == userID).Any() || x.FriendUsers.Where(f => !f.IsDeleted && f.IsBlocked).Where(f => f.UserID == userID).Any(),

                      }).ToList();
            //result = users.Where(u => !u.IsBlocked).ToList();
            result = users.ToList();
            foreach (var item in result)
            {
                var nameParts = item.DisplayName.Split(' ');
                for (int i = 0; i < nameParts.Length; i++)
                {
                    if (nameParts[nameParts.Length - 1 - i].Contains(text))
                    {
                        item.SyllableContainsText = nameParts.Length - 1 - i;
                    }
                }

            }
            result = result.OrderBy(x => x.SyllableContainsText).ToList();

            foreach (UserSearchViewModel item in result)
            {
                var firstPart = item.DisplayName.Split(' ')[0].ToCharArray();
                for (int i = 0; i < firstPart.Length; i++)
                {
                    if (firstPart[firstPart.Length - 1 - i].ToString().Contains(text))
                    {
                        item.LetterNoInFirstWord = firstPart.Length - 1 - i;
                    }
                }
                if(item.DisplayName.Split(' ').Length > 1)
                {
                    var secondPart = item.DisplayName.Split(' ')[1].ToCharArray();
                    for (int i = 0; i < secondPart.Length; i++)
                    {
                        if (secondPart[secondPart.Length - 1 - i].ToString().Contains(text))
                        {
                            item.LetterNoInSecondWord = firstPart.Length - 1 - i;
                        }
                    }
                }
            }
            result = result.OrderBy(x => x.SyllableContainsText)
                .ThenBy(x => x.LetterNoInFirstWord)
                .ThenBy(x => x.LetterNoInSecondWord).ToList();
            return new PagingViewModel<UserSearchViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public bool IsExistUser(int userId)
        {       
            return _repo.GetAll()
                    .Where(u => u.ID == userId)
                    .Any();
        }

    }

    public class AdminViewModel
    {
        public int ID { get; set; }
        public string UserMainImagePath { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public int SyllableContainsText { get; internal set; }
    }

    public class UserViewModel 
    {
        public int ID { get; set; }
        public string UserMainImagePath { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int SyllableContainsText { get; set; }
        public string Email { get; internal set; }
        public string Phone { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
    }

    public class UserSearchViewModel
    {
        public int ID { get; set; }
        public string UserMainImagePath { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        [IgnoreDataMember]
        public int SyllableContainsText { get; set; }
        [IgnoreDataMember]
        public int LetterNoInFirstWord { get; set; } = 100;
        [IgnoreDataMember]
        public int LetterNoInSecondWord { get; set; } = 100;

        public bool HasSentFriendReuestToMine { get; set; }
        public bool HasReceivedFriendRequestFromMine { get; set; }
        // is friend to logged user
        public bool IsFriend { get; set; }
        // is blocked by logged user
        [IgnoreDataMember]
        public bool IsBlocked { get; set; }
    }
}
