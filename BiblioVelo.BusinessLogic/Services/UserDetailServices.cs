using BiblioVelo.Data.Repository;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using BiblioVelo.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.UserDetailEntity;

namespace BiblioVelo.BusinessLogic.Services
{
    public class UserDetailServices : IUserDetailService
    {
        IUserDetailRepo _repo;
        public IEnumerable<UserDetail> GetAllUsers()
        {
            try
            {
                UserDetail objOutPut = new UserDetail();
                _repo = new UserDetailRepo();
                return _repo.GetAllUsers();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// user status
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public void UserStatus(int UserId, int Active)
        {
            try
            {
                _repo = new UserDetailRepo();
                _repo.UserStatus(UserId, Active);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
