using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class UserDetailEntity
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<UserDetail> lstUser { get; set; }

        public class UserDetail
        {
            public Int64 Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public int? IsActive { get; set; }
        }
    }
}
