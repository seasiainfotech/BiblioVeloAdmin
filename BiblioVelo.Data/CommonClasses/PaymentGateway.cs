using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Data.CommonClasses
{
    class PaymentGateway
    {
        public static string GetTokenId()
        {
            try
            {
                var stripekey = ConfigurationManager.AppSettings["AdminStripeApiKey"];
                StripeConfiguration.SetApiKey(stripekey);
                StripeTokenCreateOptions myToken = new StripeTokenCreateOptions();
                var card = new StripeCreditCardOptions()
                {
                    AddressCountry = "",
                    AddressLine1 = "",
                    AddressLine2 = "",
                    AddressCity = "",
                    AddressZip = "144507",
                    Cvc = 150.ToString(CultureInfo.CurrentCulture),
                    ExpirationMonth = Convert.ToInt32(12),
                    ExpirationYear = Convert.ToInt32(2022),
                    Name = "Inderjit",
                    Number = "4242424242424242"
                };
                myToken.Card = card;
                var tokenService = new StripeTokenService();
                var stripeToken = tokenService.Create(myToken);
                return stripeToken.Id;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;
            }

        }

        public static string ChargeCustomer(double amount, string currency, string description, string tokenId)
        {
            try
            {
                double totalAmount = amount * 100;
                totalAmount = Math.Round(totalAmount, MidpointRounding.AwayFromZero);
                int realAmount = Convert.ToInt32(totalAmount);
                string stripekey = ConfigurationManager.AppSettings["AdminStripeApiKey"];
                StripeConfiguration.SetApiKey(stripekey);
                var chargeOptions = new StripeChargeCreateOptions
                {
                    Amount = realAmount,
                    Currency = currency,
                    Description = description,
                    SourceTokenOrExistingSourceId = tokenId
                };
                var chargeService = new StripeChargeService();
                var stripeCharge = chargeService.Create(chargeOptions);
                return stripeCharge.Id;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }

        public static string CreateStripeAccount(string email, string country)
        {
            try
            {
                var stripekey = ConfigurationManager.AppSettings["AdminStripeApiKey"];
                StripeConfiguration.SetApiKey(stripekey);
                var accountOptions = new StripeAccountCreateOptions()
                {
                    Email = email,
                    Type = StripeAccountType.Standard,
                    Country = country
                };
                var accountService = new StripeAccountService();
                var account = accountService.Create(accountOptions);
                return account.Id;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }

        public static string TranferPayment(int amount, string currency, string destination)
        {
            try
            {
                amount = amount * 100;
                string stripekey = ConfigurationManager.AppSettings["AdminStripeApiKey"];
                StripeConfiguration.SetApiKey(stripekey);
                var transferOptions = new StripeTransferCreateOptions()
                {
                    Amount = amount,
                    Currency = currency,
                    Destination = destination
                };
                var transferService = new StripeTransferService();
                var stripeTransfer = transferService.Create(transferOptions);
                return stripeTransfer.Id;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;
            }

        }


        public static int AmountToOwner(double totalamount)
        {
            double amount = Convert.ToDouble(totalamount);
            double taxOnOwner = 4.00;
            double payableAmount = amount - ((taxOnOwner / 100) * amount);
            payableAmount = payableAmount * 100;
            payableAmount = Math.Round(payableAmount, MidpointRounding.AwayFromZero);
            int finalAmount = Convert.ToInt32(payableAmount);
            return finalAmount;

        }

        public static string RefundToUser(string chargeId,bool isFullRefund,int specificAmount)
        {
            try
            {
                string stripekey = ConfigurationManager.AppSettings["AdminStripeApiKey"];
                StripeConfiguration.SetApiKey(stripekey);
                StripeRefundCreateOptions refundOptions;
                if (isFullRefund == true)
                {
                    refundOptions = new StripeRefundCreateOptions()
                    {
                        Reason = StripeRefundReasons.RequestedByCustomer,
                        //RefundApplicationFee = true
                    };
                }
                else
                {
                    refundOptions = new StripeRefundCreateOptions()
                    {
                        Reason = StripeRefundReasons.RequestedByCustomer,
                        //RefundApplicationFee = true,
                        Amount = specificAmount * 100

                    };
                }               
                var refundService = new StripeRefundService();
                StripeRefund refund = refundService.Create(chargeId, refundOptions);
                return refund.Id;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return null;
            }
        }
    }
}
