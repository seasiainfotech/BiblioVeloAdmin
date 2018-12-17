using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Repository
{
    public interface ISecurityRepository
    {
        IEnumerable<SecurityDetail> GetSecurityClaims();

        string CaptureSecuirtyById(long id);

        string ReleaseSecuirtyById(long id);
    }
}
