using BiblioVelo.Data.CommonClasses;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BiblioVelo.Data.Repository
{
    public class SecurityRepository : ISecurityRepository
    {
        private BiblioveloEntities1 _dbEntity = new BiblioveloEntities1();

        public string CaptureSecuirtyById(long id)
        {
            try
            {
                string host = ConfigurationManager.AppSettings["Host"];
                string from = ConfigurationManager.AppSettings["FromEmail"];
                string insurerEmail = ConfigurationManager.AppSettings["InsurerEmail"];
                string password = ConfigurationManager.AppSettings["Password"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                tblBooking bookingDetails = ((IQueryable<tblBooking>)this._dbEntity.tblBookings).FirstOrDefault<tblBooking>((Expression<Func<tblBooking, bool>>)(x => x.BookingId == id));
                if (bookingDetails == null)
                {
                    return string.Empty;
                }


                string str1 = string.Empty;
                if (bookingDetails.IsSecurityByPaypal == true)
                {
                    if (!string.IsNullOrEmpty(bookingDetails.SecurityId))
                    {
                        try
                        {
                            bool captureId = PayPalImplement.CaptureAuthorization(bookingDetails.SecurityId);
                            str1 = "ok";
                            bookingDetails.IsSecurityCaptureOrRefund = new bool?(true);
                            this._dbEntity.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            str1 = string.Empty;

                        }
                    }
                }
                else
                {
                    str1 = "ok";
                }
                
                bookingDetails.IsSecurityCaptureOrRefund = new bool?(true);
                this._dbEntity.SaveChanges();
             
                if (!string.IsNullOrEmpty(str1))
                {
                    List<tblUserDeviceMapping> list = ((IQueryable<tblUserDeviceMapping>)this._dbEntity.tblUserDeviceMappings).Where<tblUserDeviceMapping>((Expression<Func<tblUserDeviceMapping, bool>>)(x => x.UserId == bookingDetails.UserId)).ToList<tblUserDeviceMapping>();
                    if (list != null && list.Count > 0)
                    {
                        List<PushNotificationModel> devicelist = new List<PushNotificationModel>();
                        foreach (tblUserDeviceMapping userDeviceMapping in list)
                            devicelist.Add(new PushNotificationModel()
                            {
                                DeviceToken = userDeviceMapping.DeviceToken,
                                Message = "Security amount has been captured by BiblioVélo for your booking with Booking Id: " + (object)bookingDetails.BookingId
                            });
                        PushNotifications.PushToIphone(devicelist);
                    }
                    tblUser userDetails = ((IQueryable<tblUser>)this._dbEntity.tblUsers).FirstOrDefault<tblUser>((Expression<Func<tblUser, bool>>)(x => (long?)x.Id == bookingDetails.UserId));
                    tblLogin tblLogin = ((IQueryable<tblLogin>)this._dbEntity.tblLogins).FirstOrDefault<tblLogin>((Expression<Func<tblLogin, bool>>)(x => (long?)x.Id == userDetails.LoginId));
                    MailMessage message = new MailMessage();
                    string str2 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("{UserName}", userDetails.FirstName + ",");
                    string empty = string.Empty;
                    string newValue = "Security amount has been captured/hold by BiblioVélo for your booking with Booking Id:" + (object)bookingDetails.BookingId;
                    string str3 = str2.Replace("{Message}", newValue);
                    string email = tblLogin.Email;
                    string str4 = "Security Amount Captured By BiblioVélo";
                
                    message.From = new MailAddress(from, "BiblioVélo Support");
                    message.To.Add(email);
                    message.Subject = str4;
                    message.IsBodyHtml = true;
                    message.Body = str3;
                    SmtpClient smtpClient = new SmtpClient(host, port);

                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(from, password);
                    try
                    {
                        smtpClient.Send(message);
                    }
                    catch (SmtpException ex1)
                    {
                        Common.ExcepLog(ex1);

                    }
                }
                return str1;
            }
            
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }

        public IEnumerable<SecurityDetail> GetSecurityClaims()
        {
            try
            {
                List<tblBooking> list = _dbEntity.tblBookings.Where(x => (x.IsAccidentalClaimRaised == true || x.IsTheftClaimRaised == true) && x.IsSecurityAmountPaid == true && !string.IsNullOrEmpty(x.SecurityId) && x.IsSecurityCaptureOrRefund == false).ToList();
                List<SecurityDetail> securityDetailList = new List<SecurityDetail>();
                foreach (tblBooking tblBooking in list)
                {
                    SecurityDetail securityDetail1 = new SecurityDetail();
                    securityDetail1.Id = tblBooking.BookingId;
                    string bookingId = tblBooking.BookingId.ToString();
                    securityDetail1.BookingId = bookingId;
                    string empty = string.Empty;
                 
                    DateTime dateTime;
                    if (tblBooking.IsTheftClaimRaised == true) 
                    {
                        securityDetail1.TypeOfClaim = "Theft Report";
                        string appSetting = ConfigurationManager.AppSettings["TheftClaimPdf"];
                        tblTheftClaim tblTheftClaim = _dbEntity.tblTheftClaims.Where(x => x.BookingId == bookingId).FirstOrDefault();
                        if (tblTheftClaim != null)
                        {
                            securityDetail1.PDF = appSetting + tblTheftClaim.PDFName;
                            SecurityDetail securityDetail2 = securityDetail1;
                            dateTime = Convert.ToDateTime((object)tblTheftClaim.CreatedDate);
                            string str = dateTime.ToString("MM-dd-yyyy  hh:mm tt");
                            securityDetail2.CreatedDate = str;
                        }
                    }
                    else if (tblBooking.IsAccidentalClaimRaised == true)
                    {
                        securityDetail1.TypeOfClaim = "Accidental Claim";
                        string appSetting = ConfigurationManager.AppSettings["AccidentalClaimPdf"];
                        tblAccidentalClaim tblAccidentalClaim = _dbEntity.tblAccidentalClaims
                            .Where(x => x.BookingId == bookingId).FirstOrDefault();
                        if (tblAccidentalClaim != null)
                        {
                            securityDetail1.PDF = appSetting + tblAccidentalClaim.PDFName;
                            SecurityDetail securityDetail2 = securityDetail1;
                            dateTime = Convert.ToDateTime(tblAccidentalClaim.CreatedDate);
                            string str = dateTime.ToString("MM-dd-yyyy  hh:mm tt");
                            securityDetail2.CreatedDate = str;
                        }
                    }

                    securityDetailList.Add(securityDetail1);
                }
                return securityDetailList;
            }
            
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;
            }
        }

        public string ReleaseSecuirtyById(long id)
        {
            try
            {
                string host = ConfigurationManager.AppSettings["Host"];
                string from = ConfigurationManager.AppSettings["FromEmail"];
                string insurerEmail = ConfigurationManager.AppSettings["InsurerEmail"];
                string password = ConfigurationManager.AppSettings["Password"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                tblBooking bookingDetails = ((IQueryable<tblBooking>)this._dbEntity.tblBookings).FirstOrDefault<tblBooking>((Expression<Func<tblBooking, bool>>)(x => x.BookingId == id));
                if (bookingDetails == null)
                {
                    return string.Empty;
                }


                string str1 = string.Empty;
                if (bookingDetails.IsSecurityByPaypal == true) 
                {
                    if (!string.IsNullOrEmpty(bookingDetails.SecurityId))
                    {
                        try
                        {
                            string VoidId = PayPalImplement.VoidAuthorization(bookingDetails.SecurityId);
                             str1 = "ok";
                            bookingDetails.IsSecurityCaptureOrRefund = new bool?(true);
                            this._dbEntity.SaveChanges();
                            
                        }
                        catch (Exception ex)
                        {
                            str1 = string.Empty;
                          
                        }
                    }
                }
                else
                {
                    string RefundId = PaymentGateway.RefundToUser(bookingDetails.SecurityId, true, 0);

                    str1 = "ok";
                    bookingDetails.IsSecurityCaptureOrRefund = new bool?(true);
                    this._dbEntity.SaveChanges();
                }
              
               
                if (!string.IsNullOrEmpty(str1))
                {
                    List<tblUserDeviceMapping> list = ((IQueryable<tblUserDeviceMapping>)this._dbEntity.tblUserDeviceMappings).Where<tblUserDeviceMapping>((Expression<Func<tblUserDeviceMapping, bool>>)(x => x.UserId == bookingDetails.UserId)).ToList<tblUserDeviceMapping>();
                    if (list != null && list.Count > 0)
                    {
                        List<PushNotificationModel> devicelist = new List<PushNotificationModel>();
                        foreach (tblUserDeviceMapping userDeviceMapping in list)
                            devicelist.Add(new PushNotificationModel()
                            {
                                DeviceToken = userDeviceMapping.DeviceToken,
                                Message = "Security amount has been refunded by BiblioVélo for your booking with Booking Id: " + (object)bookingDetails.BookingId
                            });
                        PushNotifications.PushToIphone(devicelist);
                    }
                    tblUser userDetails = ((IQueryable<tblUser>)_dbEntity.tblUsers).FirstOrDefault(x => x.Id == bookingDetails.UserId);
                    tblLogin tblLogin = ((IQueryable<tblLogin>)_dbEntity.tblLogins).FirstOrDefault(x => (long?)x.Id == userDetails.LoginId);
                    MailMessage message = new MailMessage();
                    string str2 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("{UserName}", userDetails.FirstName + ",");
                    string empty = string.Empty;
                    string newValue = "Security amount has been refunded by BiblioVélo for your booking with Booking Id:" + (object)bookingDetails.BookingId;
                    string str3 = str2.Replace("{Message}", newValue);
                    string email = tblLogin.Email;
                    string str4 = "Security Amount Refunded By BiblioVélo";
                    message.From = new MailAddress(from, "BiblioVélo Support");
                    message.To.Add(email);
                    message.Subject = str4;
                    message.IsBodyHtml = true;
                    message.Body = str3;
                    SmtpClient smtpClient = new SmtpClient(host, port);

                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(from, password);
                    try
                    {
                        smtpClient.Send(message);
                    }
                    catch (SmtpException ex1)
                    {
                        Common.ExcepLog(ex1);

                    }
                }
                return str1;
            }
           
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }
    }
}
