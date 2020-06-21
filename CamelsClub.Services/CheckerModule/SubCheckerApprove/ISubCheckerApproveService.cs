//using CamelsClub.ViewModels;
//using CamelsClub.ViewModels.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CamelsClub.Services
//{
//    public interface ISubCheckerApproveService
//    {
//        PagingViewModel Search(int userID, string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
//        void Add(CheckerApproveCreateViewModel viewModel);
//        void Edit(CheckerApproveCreateViewModel viewModel);
//        CheckerApproveViewModel GetByID(int id, Languages language = Languages.Arabic);
//        bool IsExists(int camelCompetitionID , int userID);
//        InvitedUserCamelsViewModel GetInvitedUserCamels(int userID, int competitionID);
//        List<CompetitionInviteViewModel> GetMyInvitedUsers(int checkerUserID, int competitionID);
//        List<CompetitionInviteViewModel> GetBossInvitedUsers(int checkerUserID, int competitionID);
//        InvitedUserCamelsForBossViewModel GetBossInvitedUserCamels(int userID, int competitionID);
//    }
//}
