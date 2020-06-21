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
using static CamelsClub.Services.UserService;

namespace CamelsClub.Services
{
    public interface IUserService
    {
        ConfirmationMessageViewModel Register(CreateUserViewModel viewModel);
        ConfirmationMessageViewModel Login(string mobileNumber);
        bool IsExistUser(int userId);
        bool SignOut(int userId,string token);
        PagingViewModel<UserSearchViewModel> FindUsers(int userID , string text, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        PagingViewModel<AdminViewModel> GetAllAdminUsers(string text, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);


        PagingViewModel<UserViewModel> Search(string text, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        bool IsUserNameExists(string userName);
        ConfirmationMessageViewModel AdminLogin(AdminLoginCreateViewModel viewModel);
        //    bool SetUserName(List<ChangeUserNameViewModel> viewModels);
    }
}
