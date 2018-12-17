using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class SecurityEntityDetail
    {
        public int Total { get; set; }

        public int PageSize { get; set; }

        public List<SecurityDetail> lstSecurityClaims { get; set; }
    }
    public class SecurityDetail
    {
        public long Id { get; set; }

        public string BookingId { get; set; }

        public string TypeOfClaim { get; set; }

        public string PDF { get; set; }

        public string CreatedDate { get; set; }
    }
}
