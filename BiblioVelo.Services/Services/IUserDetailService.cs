using System;
using System.Collections.Generic;
using static BiblioVelo.Services.Models.UserDetailEntity;

namespace BiblioVelo.Services.Services
{
    public interface IUserDetailService
    {
        IEnumerable<UserDetail> GetAllUsers();
        void UserStatus(int UserId, int Active);
    }
}
