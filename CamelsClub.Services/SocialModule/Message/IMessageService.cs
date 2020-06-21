using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IMessageService
    {
        //passed user in token is LoggedUserID and FromUserID is the passed parameter
        PagingViewModel<MessageViewModel> GetReceivedMessage(int FromUserID , int LoggedUserID, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        bool Send(MessageCreateViewModel viewModel);
        int GetUnSeenMessagesCount(int loggedUserID);
        List<UserChatViewModel> GetUsersHasChatWithLoggedUser(int loggedUserID);
    }
}
