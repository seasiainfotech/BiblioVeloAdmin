using BiblioVelo.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using BiblioVelo.Data.Repository;

namespace BiblioVelo.BusinessLogic.Services
{
    public class AccidentalService : IAccidentalService
    {
        IAccidentalRepo _repo = new AccidentalRepo();
        public long AddAccidentalClaim(AccidentalClaimModel accidentalModel)
        {
            try
            {
               
             
                return _repo.AddAccidentalClaim(accidentalModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ApproveAccidentalClaimById(long id)
        {

            return this._repo.ApproveAccidentalClaimById(id);
        }

        public IEnumerable<AccidentalClaimModel> GetAccidentalClaims()
        {
            return this._repo.GetAccidentalClaims();
        }

        public string RejectAccidentalClaimById(long id)
        {
            return this._repo.RejectAccidentalClaimById(id);
        }
    }
}
