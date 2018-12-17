using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Services
{
    public interface IAccidentalService
    {
        long AddAccidentalClaim(AccidentalClaimModel accidentalModel);
        IEnumerable<AccidentalClaimModel> GetAccidentalClaims();

        string RejectAccidentalClaimById(long id);

        string ApproveAccidentalClaimById(long id);
    }
}
