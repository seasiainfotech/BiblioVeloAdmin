using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Services
{
    public interface ITheftClaimServices
    {
        string SaveTheftClaim(TheftClaimModel model);

        IEnumerable<TheftClaimModel> GetTheftReports();

        string RejectTheftReportById(long id);

        string ApproveTheftReportById(long id);
    }
}
