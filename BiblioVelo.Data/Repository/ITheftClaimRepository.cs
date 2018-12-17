using AutoMapper;
using AutoMapper.Configuration;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Winnovative.WnvHtmlConvert;

namespace BiblioVelo.Data.Repository
{
    public class TheftClaimRepository : ITheftClaimRepository
    {
        private static readonly Random Random = new Random();
        private BiblioveloEntities1 _dbEntity = new BiblioveloEntities1();

        public string ApproveTheftReportById(long id)
        {
            try
            {
                string str1 = HostingEnvironment.MapPath("~/PdfTemplates/TheftClaimForms/");
              
                tblTheftClaim tblTheftReports = ((IQueryable<tblTheftClaim>)this._dbEntity.tblTheftClaims).FirstOrDefault<tblTheftClaim>((Expression<Func<tblTheftClaim, bool>>)(x => x.Id == id));
                if (tblTheftReports == null)
                    return string.Empty;
                string host = ConfigurationManager.AppSettings["Host"];
                string from = ConfigurationManager.AppSettings["FromEmail"];
                string insurerEmail = ConfigurationManager.AppSettings["InsurerEmail"];
                string password = ConfigurationManager.AppSettings["Password"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                string str2 = str1 + tblTheftReports.PDFName;
               
                if (System.IO.File.Exists(str2))
                {
                    MailMessage message = new MailMessage();
                    string str3 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("Hi", "Dear").Replace("{UserName}", "Sir/Mam,").Replace("{Message}", "We have sent you a theft report form .<br/>Please find attached PDF").Replace("{Link}", string.Empty);
                    message.Attachments.Add(new Attachment(str2)
                    {
                        ContentDisposition = {
              Inline = true
            }
                    });
                    message.From = new MailAddress(from, "BiblioVélo Support");
                    message.To.Add(insurerEmail);
                    message.Subject = "BiblioVélo::Theft Report";
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

                MailMessage messageToUser = new MailMessage();
                string str4 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("{UserName}", tblTheftReports.FullName + " ,").Replace("{Message}", "Your Theft report form regarding Booking Ref :" + tblTheftReports.BookingId + " has been approved by the Admin and send it to the insurer for further inquiry.<br/>").Replace("{Link}", string.Empty);
                string email = tblTheftReports.Email;
                string str5 = "BiblioVélo:: Theft Report Approved";
                messageToUser.From = new MailAddress(from, "BiblioVélo Support");
                messageToUser.To.Add(email);
                messageToUser.Subject = str5;
                messageToUser.IsBodyHtml = true;
                messageToUser.Body = str4;
                SmtpClient smtpClient1 = new SmtpClient(host, port);

                smtpClient1.EnableSsl = true;
                smtpClient1.Credentials = (ICredentialsByHost)new NetworkCredential(from, password);
                try
                {
                    smtpClient1.Send(messageToUser);
                }
                catch (SmtpException ex1)
                {
                    Common.ExcepLog(ex1);

                }
                tblTheftReports.IsApproved = new bool?(true);
                this._dbEntity.SaveChanges();
                return "deleted";
            }
         
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }

        public IEnumerable<TheftClaimModel> GetTheftReports()
        {
            try
            {
                List<tblTheftClaim> list = ((IQueryable<tblTheftClaim>)this._dbEntity.tblTheftClaims).Where<tblTheftClaim>((Expression<Func<tblTheftClaim, bool>>)(x => x.IsApproved != (bool?)true)).ToList<tblTheftClaim>();
                List<TheftClaimModel> theftClaimModelList = new List<TheftClaimModel>();
                string appSetting = ConfigurationManager.AppSettings["TheftClaimPdf"];
                foreach (tblTheftClaim tblTheftClaim in list)
                    theftClaimModelList.Add(new TheftClaimModel()
                    {
                        Id = tblTheftClaim.Id,
                        FullName = tblTheftClaim.FullName,
                        Address = tblTheftClaim.Address,
                        BookingId = tblTheftClaim.BookingId.ToString(),
                        CreatedDate = Convert.ToDateTime((object)tblTheftClaim.CreatedDate).ToString("MM-dd-yyyy  hh:mm tt"),
                        Email = tblTheftClaim.Email,
                        PDFName = appSetting + tblTheftClaim.PDFName
                    });
                return (IEnumerable<TheftClaimModel>)theftClaimModelList;
            }
            catch (Exception ex)
            {
               
                Common.ExcepLog((Exception)ex);
                throw;
            }
           
        }

        public string RejectTheftReportById(long id)
        {
            try
            {
                tblTheftClaim tblTheftClaim = _dbEntity.tblTheftClaims.FirstOrDefault(x => x.Id == id);
                if (tblTheftClaim == null)
                    return string.Empty;
                string host = ConfigurationManager.AppSettings["Host"];
                string from = ConfigurationManager.AppSettings["FromEmail"];
                string password = ConfigurationManager.AppSettings["Password"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                MailMessage messageToUser = new MailMessage();
                string str1 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("{UserName}", tblTheftClaim.FullName + " ,").Replace("{Message}", "Your Theft report form regarding Booking Ref :" + tblTheftClaim.BookingId + " has been rejected by the Admin<br/>").Replace("{Link}", string.Empty);
                string email = tblTheftClaim.Email;
                string str2 = "BiblioVélo:: Theft Report Rejected";
                messageToUser.From = new MailAddress(from, "BiblioVélo Support");
                messageToUser.To.Add(email);
                messageToUser.Subject = str2;
                messageToUser.IsBodyHtml = true;
                messageToUser.Body = str1;

                SmtpClient smtpClient1 = new SmtpClient(host, port);

                smtpClient1.EnableSsl = true;
                smtpClient1.Credentials = (ICredentialsByHost)new NetworkCredential(from, password);
                try
                {
                    smtpClient1.Send(messageToUser);
                }
                catch (SmtpException ex1)
                {
                    Common.ExcepLog(ex1);

                }
                this._dbEntity.tblTheftClaims.Remove(tblTheftClaim);
                this._dbEntity.SaveChanges();
                return "deleted";
            }
  
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }

        public string SaveTheftClaim(TheftClaimModel model)
        {
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/PdfTemplates"), "TheftClaimPDF.html");
            model.FullName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.FullName.ToLower());
            int bookingid;
            try
            {
                bookingid = Convert.ToInt32(model.BookingId);
            }
            catch (Exception ex)
            {
                bookingid = 0;
            }
            tblBooking tblBooking = ((IQueryable<tblBooking>)this._dbEntity.tblBookings).FirstOrDefault<tblBooking>((Expression<Func<tblBooking, bool>>)(x => x.BookingId == (long)bookingid));
            if (tblBooking != null)
            {
                tblBooking.IsTheftClaimRaised = new bool?(true);
                tblBooking.IsRenterRaisedDispute = new bool?(true);
                this._dbEntity.SaveChanges();
            }
            string str1 = System.IO.File.ReadAllText(path).Replace("{fullname}", model.FullName).Replace("{email}", model.Email);
            string oldValue1 = "{dob}";
            DateTime dateTime = model.DOB.Date;
            string newValue1 = dateTime.ToString("MM-dd-yyyy");
            string str2 = str1.Replace(oldValue1, newValue1).Replace("{booking}", model.BookingId).Replace("{address}", model.Address).Replace("{postcode}", model.PostCode).Replace("{mobilenumber}", model.MobileNumber).Replace("{circumstances}", model.Circumstances).Replace("{stolenlist}", model.ListOfStolenItems).Replace("{approxvalue}", "£ " + model.ApproxValue).Replace("{responsible}", model.WhoWasResponsible).Replace("{unattended}", model.HowLongUnattended);
            string oldValue2 = "{lastseen}";
            dateTime = model.TimeAndDateLastSeen;
            string newValue2 = dateTime.ToString();
            string str3 = str2.Replace(oldValue2, newValue2);
            string oldValue3 = "{discovered}";
            dateTime = model.TimeAndDateTheftDiscovered;
            string newValue3 = dateTime.ToString();
            string str4 = str3.Replace(oldValue3, newValue3);
            bool? nullable = model.IsAnyWitness;
            bool flag1 = true;
            string str5 = ((nullable.GetValueOrDefault() == flag1 ? (nullable.HasValue ? 1 : 0) : 0) == 0 ? str4.Replace("{anywitness}", "No").Replace("{witnessdetails}", string.Empty) : str4.Replace("{anywitness}", "Yes").Replace("{witnessdetails}", model.WitnessDetails)).Replace("{policecontact}", model.PoliceContactNumber).Replace("{crimeincident}", model.CrimeIncidentNumber);
            string str6 = !model.IsPoliceAttend ? str5.Replace("{policeattend}", "No") : str5.Replace("{policeattend}", "Yes");
            string str7 = (!model.IsReportedToPoliceImmediately ? str6.Replace("{policeimmediately}", "No").Replace("{reasonforpolice}", model.ReasonForNotReportedImmediately) : str6.Replace("{policeimmediately}", "Yes").Replace("{reasonforpolice}", string.Empty)).Replace("{howsecured}", model.HowBicycleWasSecured).Replace("{locksecured}", model.DetailOfSecuredLock).Replace("{howaccess}", model.HowAccessGained).Replace("{alternatesecurity}", model.AlternativeSecurityMethod);
            nullable = model.IsAnotherInsurer;
            bool flag2 = true;
            string str8 = ((nullable.GetValueOrDefault() == flag2 ? (nullable.HasValue ? 1 : 0) : 0) == 0 ? str7.Replace("{isanotherinsurer}", "No") : str7.Replace("{isanotherinsurer}", "Yes")).Replace("{previousinsurer}", model.PreviousInsurerName).Replace("{expirydate}", model.ExpiryDate).Replace("{anybicycleclaim}", model.PastClaimDetails);
            nullable = model.IsAnyCriminalConviction;
            bool flag3 = true;
            string str9 = (nullable.GetValueOrDefault() == flag3 ? (nullable.HasValue ? 1 : 0) : 0) == 0 ? str8.Replace("{iscriminal}", "No") : str8.Replace("{iscriminal}", "Yes");
            nullable = model.IsPolicyCancelled;
            bool flag4 = true;
            string str10 = (nullable.GetValueOrDefault() == flag4 ? (nullable.HasValue ? 1 : 0) : 0) == 0 ? str9.Replace("{ispolicycancelled}", "No") : str9.Replace("{ispolicycancelled}", "Yes");
            nullable = model.IsRefusedRenewal;
            bool flag5 = true;
            string str11 = ((nullable.GetValueOrDefault() == flag5 ? (nullable.HasValue ? 1 : 0) : 0) == 0 ? str10.Replace("{isrefusedrenewal}", "No") : str10.Replace("{isrefusedrenewal}", "Yes")).Replace("{provideddetails}", model.ProvidedDetails);
            StreamWriter streamWriter = new StreamWriter(HostingEnvironment.MapPath("~/Templates/dfTemplate.htm"));
            streamWriter.Write(str11);
            streamWriter.Close();
            streamWriter.Dispose();
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.LicenseKey = "elFLWktaTE9IWk9USlpJS1RLSFRDQ0ND";
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.NoCompression;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Landscape;
            pdfConverter.PdfDocumentOptions.BottomMargin = 20;
            pdfConverter.PdfDocumentOptions.TopMargin = 20;
            pdfConverter.PdfDocumentOptions.LeftMargin = 20;
            pdfConverter.PdfDocumentOptions.RightMargin = 20;
            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            pdfConverter.PdfDocumentOptions.ShowFooter = false;
            pdfConverter.PdfDocumentOptions.AutoSizePdfPage = false;
            pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
            pdfConverter.PdfDocumentOptions.SinglePage = true;
            string str12 = RandomString(7) + ".pdf";
            byte[] pdfBytesFromUrl = pdfConverter.GetPdfBytesFromUrl(HostingEnvironment.MapPath("~/Templates/dfTemplate.htm"));
            System.IO.File.WriteAllBytes(HostingEnvironment.MapPath("~/PdfTemplates/TheftClaimForms/" + str12 ?? ""), pdfBytesFromUrl);
            model.PDFName = str12;
            Mapper.Initialize(new MapperConfigurationExpression() { AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true });
          
            tblTheftClaim tblTheftClaim = Mapper.Instance.Map<tblTheftClaim>(model);
            tblTheftClaim.IsApproved = new bool?(false);
            tblTheftClaim.CreatedDate = new DateTime?(DateTime.Now);
            this._dbEntity.tblTheftClaims.Add(tblTheftClaim);
            this._dbEntity.SaveChanges();
            return "Success";
        }

        public bool CheckBookingId(int bookingId)
        {
            try
            {
                return ((IQueryable<tblBooking>)this._dbEntity.tblBookings).FirstOrDefault<tblBooking>((Expression<Func<tblBooking, bool>>)(x => x.BookingId == (long)bookingId && x.IsBookingEnded != (bool?)true && x.IsRenterRaisedDispute != (bool?)true)) != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
