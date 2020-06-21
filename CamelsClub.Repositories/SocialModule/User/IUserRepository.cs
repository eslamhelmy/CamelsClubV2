using CamelsClub.Models;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        bool IsEmailAlreadyExists(string email);
        bool IsPhoneAlreadyExists(string mobileNumber);
        User GetUserByPhone(string phone);
        List<int> GetUsersIDs();
        bool HasSamePhone(string newphone);
        bool IsPhoneNoteUnique(string phone, int userId);
        bool IsEmailNoteUnique(string email, int userId);
        bool IsUserNameAlreadyExists(string userName);

        List<ListViewModel> GetUserList(List<int> UsersIDs);
    }
}
