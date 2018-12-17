using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class TheftClaimModelEntity
    {
        public int Total { get; set; }

        public int PageSize { get; set; }

        public List<TheftClaimModel> lstTheftReports { get; set; }
    }
}
