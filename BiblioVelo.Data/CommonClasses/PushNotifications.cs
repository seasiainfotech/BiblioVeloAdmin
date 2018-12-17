using System;
using System.Collections.Generic;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;
using BiblioVelo.Services.Models;
using System.Configuration;
using System.Web.Hosting;

namespace BiblioVelo.Data.CommonClasses

{
    class PushNotifications
    {
        public static string Result = "";
        /// <summary>
        /// PushToIphone
        /// </summary>
        /// <param name="devicelist"></param>
        /// <returns></returns>
        public static bool PushToIphone(List<PushNotificationModel> devicelist)
        {
            try
            {
                string certificatePath = HostingEnvironment.MapPath("~/Certificate/Certificates_bibliovelo_push_production.p12");
                //string certificatepath = "D:/Projects/BiblioVeloApi/BiblioVeloApi/Certificate/APNSDevBibliovelo";
                string certificatepassword = ConfigurationManager.AppSettings["CertificatePassword"].ToString();
                // string certificatePath = ConfigurationManager.AppSettings["CertificatePath"].ToString();
                // Configuration (NOTE: .pfx can also be used here)
                var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
                    certificatePath, string.Empty);

                // Create a new broker
                var apnsBroker = new ApnsServiceBroker(config);

                // Wire up events
                apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                {

                    aggregateEx.Handle(ex =>
                    {

                        // See what kind of exception it was to further diagnose
                        if (ex is ApnsNotificationException)
                        {
                            var notificationException = (ApnsNotificationException)ex;

                            // Deal with the failed notification
                            var apnsNotification = notificationException.Notification;
                            var statusCode = notificationException.ErrorStatusCode;

                            Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                        }
                        else
                        {
                            // Inner exception might hold more useful information like an ApnsConnectionException			
                            Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                        }

                        // Mark it as handled
                        return true;
                    });
                };

                apnsBroker.OnNotificationSucceeded += (notification) =>
                {
                    Console.WriteLine("Apple Notification Sent!");
                };

                // Start the broker
                apnsBroker.Start();

                foreach (var deviceToken in devicelist)
                {
                    // Queue a notification to send
                    apnsBroker.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = deviceToken.DeviceToken,
                        Payload = JObject.Parse("{\"aps\":{\"alert\":\"" + deviceToken.Message + "\",\"badge\":1,\"sound\":\"default\"}}")
                        //Payload = JObject.Parse("{\"aps\":{\"alert\":\"" + deviceToken.Message + "\",\"badge\":1,\"sound\":\"default\"}\"BookingId\":{\"" + deviceToken.BookingId + "\"}}")

                    });
                }

                // Stop the broker, wait for it to finish   
                // This isn't done after every message, but after you're
                // done with the broker
                apnsBroker.Stop();

                return true;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;

            }

        }

    }
}
