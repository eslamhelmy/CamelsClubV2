using CamelsClub.Models;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface IUserRoleService 
    {
        void InsertUserRole(int userID, Roles roleID);
        bool HasRole(int userId, string role);
    }
}
