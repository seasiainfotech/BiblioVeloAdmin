using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.UserDetailEntity;

namespace BiblioVelo.Services.Repository
{
    public interface IUserDetailRepo
    {
        IEnumerable<UserDetail> GetAllUsers();
        void UserStatus(int UserId, int Active);
    }
}
