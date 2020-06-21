using CamelsClub.Data.Context;
using CamelsClub.Models;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public class UserRepository : GenericRepository<User> , IUserRepository
    {
        public UserRepository(CamelsClubContext context) : base(context)
        {
        }

        public User GetUserByPhone(string phone)
        {
            return GetAll()
                   .Where(u => u.Phone == phone)
                   .FirstOrDefault();
        }

        public bool IsEmailAlreadyExists(string email)
        {
            return GetAll()
                      .Where(u => u.Email == email)
                      .Any();

        }

        public bool IsPhoneAlreadyExists(string phone)
        {

            return GetAll()
                    .Where(u => u.Phone == phone)
                    .Any();

        }

        public bool IsUserNameAlreadyExists(string userName)
        {

            return GetAll()
                    .Where(u => u.UserName == userName)
                    .Any();

        }

        public bool IsPhoneNoteUnique(string phone,int userId)
        {

            return GetAll()
                    .Where(u => u.Phone == phone && u.ID != userId)
                    .Any();

        }
        public bool IsEmailNoteUnique(string email, int userId)
        {

            return GetAll()
                    .Where(u => u.Email == email && u.ID != userId)
                    .Any();

        }


        public List<int> GetUsersIDs()
        {

            return GetAll().Select(user => user.ID).ToList();
                   

        }


        public bool HasSamePhone (string newphone)
        {
            return GetAll()
                     .Where(u => u.Phone == newphone)
                     .Any();
        }


        public List<ListViewModel> GetUserList(List<int> UsersIDs)
        {
            return GetAll()
                     .Where(u => UsersIDs.Contains(u.ID)).Select(u=>new ListViewModel
                     {
                         ID=u.ID,
                         Name = u.UserName
                     }).ToList();
                     
        }
     
    }
}
