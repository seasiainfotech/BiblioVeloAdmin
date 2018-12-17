using BiblioVelo.Data.Repository;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using BiblioVelo.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.BusinessLogic.Services
{
    public class TheftClaimServices : ITheftClaimServices
    {
        private ITheftClaimRepository _theftRepo = (ITheftClaimRepository)new TheftClaimRepository();

        public string ApproveTheftReportById(long id)
        {
            return _theftRepo.ApproveTheftReportById(id);
        }

        public IEnumerable<TheftClaimModel> GetTheftReports()
        {
            return this._theftRepo.GetTheftReports();
        }

        public string RejectTheftReportById(long id)
        {
            return this._theftRepo.RejectTheftReportById(id);
        }

        public string SaveTheftClaim(TheftClaimModel model)
        {
            return this._theftRepo.SaveTheftClaim(model);
        }
    }
}
