using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Data.CommonClasses
{
    /// <summary>
    /// PayPalImplementation
    /// </summary>
    public class PayPalImplement
    {

      

        /// <summary>
        /// VoidAuthorization
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public static string VoidAuthorization(string paymentId)
        {
            try
            {
                var config = ConfigManager.Instance.GetProperties();
                var accessToken = new OAuthTokenCredential(config).GetAccessToken();
                var apiContext = new APIContext(accessToken);
                var getPayment = Payment.Get(apiContext, paymentId);
                var authorization = getPayment.transactions[0].related_resources[0].authorization;
                var Void = authorization.Void(apiContext);


                return Void.id;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }

        }
        public static bool CaptureAuthorization(string paymentId)
        {
            try
            {
                var config = ConfigManager.Instance.GetProperties();
                var accessToken = new OAuthTokenCredential(config).GetAccessToken();
                var apiContext = new APIContext(accessToken);


                var getPayment = Payment.Get(apiContext, paymentId);
                var authorization = getPayment.transactions[0].related_resources[0].authorization;
                var capture = new Capture()
                {
                    amount = new Amount()
                    {
                        currency = "GBP",
                        total = "160"
                    },
                    is_final_capture = true
                };
                var Capture = authorization.Capture(apiContext, capture);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
