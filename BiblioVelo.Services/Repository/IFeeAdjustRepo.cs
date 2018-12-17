using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Repository
{
    public interface IFeeAdjustRepo
    {
        FeeAdjustModel GetFeePercentage();

        string EditFeeConfirmed(FeeAdjustModel feeModel);
    }
}
