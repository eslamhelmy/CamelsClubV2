using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IFriendService
    {
        PagingViewModel<ClearedFriendViewModel> Search(int userID=0 , string search = "", string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        FriendViewModel GetByID(int id);
        bool Unfollow(BlockedFriendCreateViewModel viewModel);
        bool IsAlreadyBlocked(int userID, int blockedFriendID);
    }
}
