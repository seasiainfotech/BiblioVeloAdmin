using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Services
{
    public interface IFeeAdjustService
    {
        FeeAdjustModel GetFeePercentage();

        string EditFeeConfirmed(FeeAdjustModel feeModel);
    }
}
