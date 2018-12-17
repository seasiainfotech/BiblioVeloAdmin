using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Services
{
    public interface ISecurityService
    {
        IEnumerable<SecurityDetail> GetSecurityClaims();

        string CaptureSecuirtyById(long id);

        string ReleaseSecuirtyById(long id);
    }
}
