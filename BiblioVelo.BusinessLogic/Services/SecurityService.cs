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
    public class SecurityService : ISecurityService
    {
        private ISecurityRepository _securityRepo = (ISecurityRepository)new SecurityRepository();

        public string CaptureSecuirtyById(long id)
        {
            return this._securityRepo.CaptureSecuirtyById(id);
        }

        public IEnumerable<SecurityDetail> GetSecurityClaims()
        {
            return this._securityRepo.GetSecurityClaims();
        }

        public string ReleaseSecuirtyById(long id)
        {
            return this._securityRepo.ReleaseSecuirtyById(id);
        }
    }
}
