using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IFriendRequestService
    {
        PagingViewModel<FriendRequestViewModel> Search(int userID=0 , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(FriendRequestCreateViewModel view);
        void Edit(FriendRequestCreateViewModel viewModel);

        FriendRequestViewModel GetByID(int id);
        bool IsExists(int id);
        PagingViewModel<ReceivedFriendRequestViewModel> GetReceivedFriendRequests(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        PagingViewModel<SentFriendRequestViewModel> GetSentFriendRequests(int userID = 0, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);

        bool ApproveFriendRequest(FriendCreateViewModel viewModel);
        bool IgnoreFriendRequest(FriendCreateViewModel viewModel);

    }
}
