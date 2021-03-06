﻿using BiblioVelo.Data.Repository;
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
    public class FeeAdjustService : IFeeAdjustService
    {
        private IFeeAdjustRepo feeAdjust;

        public string EditFeeConfirmed(FeeAdjustModel feeModel)
        {
            feeAdjust = (IFeeAdjustRepo)new FeeAdjustRepo();
            return this.feeAdjust.EditFeeConfirmed(feeModel);
        }

        public FeeAdjustModel GetFeePercentage()
        {
            this.feeAdjust = (IFeeAdjustRepo)new FeeAdjustRepo();
            return this.feeAdjust.GetFeePercentage();
        }
    }
}
