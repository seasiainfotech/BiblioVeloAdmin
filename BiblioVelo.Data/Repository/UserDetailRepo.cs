using AutoMapper;
using AutoMapper.Configuration;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using static BiblioVelo.Services.Models.UserDetailEntity;

namespace BiblioVelo.Data.Repository
{
    public class UserDetailRepo : IUserDetailRepo
    {
        BiblioveloEntities1 db = new BiblioveloEntities1();
        public IEnumerable<UserDetail> GetAllUsers()
        {
            try
            {
                var userDetail = (from x in db.tblUsers
                                  join y in db.tblLogins on x.LoginId equals y.Id
                                  where x.IsDelete == 0
                                  select new UserDetail()
                                  {
                                      Id = x.Id,
                                      FirstName = x.FirstName,
                                      LastName = x.LastName,
                                      Address = x.Address,
                                      PhoneNumber = x.PhoneNumber,
                                      Email = y.Email,
                                      Name = x.FirstName + " " + x.LastName,
                                      IsActive = x.IsActive
                                  }).ToList();

                if (userDetail.Any())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblUser, UserDetail>();
                    });
                    Mapper.Initialize(new MapperConfigurationExpression() { AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true });
                    var userModel = Mapper.Instance.Map<List<UserDetail>>(userDetail); 
                    return userModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public void UserStatus(int UserId, int Active)
        {
            try
            {
                var userDetails = db.tblUsers.FirstOrDefault(x => x.Id == UserId);
                userDetails.IsActive = Active;
                db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
;