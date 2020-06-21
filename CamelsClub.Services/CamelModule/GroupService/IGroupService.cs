using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IGroupService
    {
        PagingViewModel<GroupViewModel> Search(int userID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        List<GroupViewModel> GetUserGroups(int userID);


        GroupViewModel Add(GroupCreateViewModel view);
        GroupViewModel Edit(GroupCreateViewModel viewModel);
        void Delete(int id);
        GroupViewModel GetByID(int id);
        GroupCreateViewModel GetEditableByID(int id);
        bool IsExists(int id);
        bool RemoveCamel(int userID, int camelID, int groupID);
    }
}
