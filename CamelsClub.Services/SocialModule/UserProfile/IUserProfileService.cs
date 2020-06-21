using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IUserProfileService
    {
        //test
        PagingViewModel<UserProfileViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(UserProfileCreateViewModel view);
        UserProfileViewModel GetByID(int loggedUserID, int userId);
        UserProfileViewModel GetProfileByID(int userId);

        UserProfileCreateViewModel GetEditableByID(int id);

        bool IsExists(int CoomentId);
        string Edit(UserProfileCreateViewModel viewModel);
        void Delete(int id);
        UserProfileViewModel GetMyProfile(int userId);
        List<DocumentViewModel> GetMyProfileImages(int userId);
        List<DocumentViewModel> GetMyProfileVideos(int userId);


    }
}
