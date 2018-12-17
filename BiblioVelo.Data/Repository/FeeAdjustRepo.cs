using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Data.Repository
{
    public class FeeAdjustRepo : IFeeAdjustRepo
    {
        private BiblioveloEntities1 _dbEntity = new BiblioveloEntities1();

        public string EditFeeConfirmed(FeeAdjustModel feeModel)
        {
            try
            {
                tblAdminFee tblAdminFee = ((IQueryable<tblAdminFee>)this._dbEntity.tblAdminFees).FirstOrDefault<tblAdminFee>(x => x.Id == feeModel.Id);
                tblAdminFee.InsuranceFor1Day = new double?(feeModel.InsuranceFor1Day);
                tblAdminFee.InsuranceFor2to3Days = new double?(feeModel.InsuranceFor2to3Days);
                tblAdminFee.InsuranceFor4to6Days = new double?(feeModel.InsuranceFor4to6Days);
                tblAdminFee.InsuranceFor7to10Days = new double?(feeModel.InsuranceFor7to10Days);
                tblAdminFee.InsuranceForMoreThan10Days = new double?(feeModel.InsuranceForMoreThan10Days);
                tblAdminFee.BookingFeeRenter = new double?(feeModel.BookingFeeRenter);
                tblAdminFee.BookingFeeOwner = new double?(feeModel.BookingFeeOwner);
                tblAdminFee.ModifiedDate = new DateTime?(DateTime.Now);
                this._dbEntity.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return "Exception";
            }
        }

        public FeeAdjustModel GetFeePercentage()
        {
            try
            {
                tblAdminFee tblAdminFee = ((IQueryable<tblAdminFee>)this._dbEntity.tblAdminFees).FirstOrDefault();
                FeeAdjustModel feeAdjustModel1 = new FeeAdjustModel();
                if (tblAdminFee != null)
                {
                    feeAdjustModel1.Id = tblAdminFee.Id;
                    FeeAdjustModel feeAdjustModel2 = feeAdjustModel1;
                    double? nullable = tblAdminFee.InsuranceFor1Day;
                    double num1 = nullable.Value;
                    feeAdjustModel2.InsuranceFor1Day = num1;
                    FeeAdjustModel feeAdjustModel3 = feeAdjustModel1;
                    nullable = tblAdminFee.InsuranceFor2to3Days;
                    double num2 = nullable.Value;
                    feeAdjustModel3.InsuranceFor2to3Days = num2;
                    FeeAdjustModel feeAdjustModel4 = feeAdjustModel1;
                    nullable = tblAdminFee.InsuranceFor4to6Days;
                    double num3 = nullable.Value;
                    feeAdjustModel4.InsuranceFor4to6Days = num3;
                    FeeAdjustModel feeAdjustModel5 = feeAdjustModel1;
                    nullable = tblAdminFee.InsuranceFor7to10Days;
                    double num4 = nullable.Value;
                    feeAdjustModel5.InsuranceFor7to10Days = num4;
                    FeeAdjustModel feeAdjustModel6 = feeAdjustModel1;
                    nullable = tblAdminFee.InsuranceForMoreThan10Days;
                    double num5 = nullable.Value;
                    feeAdjustModel6.InsuranceForMoreThan10Days = num5;
                    FeeAdjustModel feeAdjustModel7 = feeAdjustModel1;
                    nullable = tblAdminFee.BookingFeeOwner;
                    double num6 = nullable.Value;
                    feeAdjustModel7.BookingFeeOwner = num6;
                    FeeAdjustModel feeAdjustModel8 = feeAdjustModel1;
                    nullable = tblAdminFee.BookingFeeRenter;
                    double num7 = nullable.Value;
                    feeAdjustModel8.BookingFeeRenter = num7;
                    feeAdjustModel1.ModifiedDate = Convert.ToDateTime((object)tblAdminFee.ModifiedDate).ToString("dd MMM yyyy @ HH:mm");
                }
                return feeAdjustModel1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
