using BiblioVelo.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioVelo.Services.Models;
using Winnovative.WnvHtmlConvert;
using System.Web.Hosting;
using System.Web;
using System.IO;
using AutoMapper;
using AutoMapper.Configuration;
using System.Configuration;
using System.Net.Mail;
using System.Net.Http;
using System.Net;
using System.Linq.Expressions;

namespace BiblioVelo.Data.Repository
{
    public class AccidentalRepo : IAccidentalRepo
    {
        private static readonly Random Random = new Random();
        BiblioveloEntities1 db = new BiblioveloEntities1();
        public long AddAccidentalClaim(AccidentalClaimModel accidentalModel)
        {
            try
            {
                tblBooking tblBooking = db.tblBookings.FirstOrDefault(x => x.BookingId == Convert.ToInt32(accidentalModel.BookingId));
                if (tblBooking != null)
                {
                    tblBooking.IsAccidentalClaimRaised = new bool?(true);
                    tblBooking.IsRenterRaisedDispute = new bool?(true);
                    db.SaveChanges();
                }
            }
            catch (Exception Ex)
            {

            }
           

            string tempPath = System.IO.Path.Combine(HostingEnvironment.MapPath("~/PdfTemplates"), "AccidentalClaimPDF.html");
            string _pdfContent = System.IO.File.ReadAllText(tempPath);
            _pdfContent = _pdfContent.Replace("{fullname}", accidentalModel.FullName);
            _pdfContent = _pdfContent.Replace("{email}", accidentalModel.Email);
            _pdfContent = _pdfContent.Replace("{dob}", accidentalModel.DOB);
            _pdfContent = _pdfContent.Replace("{booking}", accidentalModel.BookingId);
            _pdfContent = _pdfContent.Replace("{address}", accidentalModel.Address);
            _pdfContent = _pdfContent.Replace("{postcode}", accidentalModel.PostCode);
            _pdfContent = _pdfContent.Replace("{mobilenumber}", accidentalModel.MobileNumber);
            _pdfContent = _pdfContent.Replace("{dateofincident}", accidentalModel.DateOfIncident);
            _pdfContent = _pdfContent.Replace("{timeofincident}", accidentalModel.TimeOfIncident);
            _pdfContent = _pdfContent.Replace("{incharge}", accidentalModel.InCharge);
            _pdfContent = _pdfContent.Replace("{incidentdetails}", accidentalModel.IncidentDescription);
            _pdfContent = _pdfContent.Replace("{witnessdetails}", accidentalModel.WitnessDetails);
            _pdfContent = _pdfContent.Replace("{thirdpartydetails}", accidentalModel.ThirdPartyDetails);
            _pdfContent = _pdfContent.Replace("{reportedtopolice}", accidentalModel.IsReportedToPolice == true ? "Yes" : "No");
            _pdfContent = _pdfContent.Replace("{anotherinsurer}", accidentalModel.IsAnotherInsurer == true ? "Yes" : "No");
            _pdfContent = _pdfContent.Replace("{refused}", accidentalModel.IsRefusedRenewal == true ? "Yes" : "No");
            _pdfContent = _pdfContent.Replace("{itemclaims}", accidentalModel.ItemsClaimedFor);
            _pdfContent = _pdfContent.Replace("{circumstances}", accidentalModel.Circumstances);
            
            _pdfContent = _pdfContent.Replace("{previousinsurer}", accidentalModel.PreviousInsurerName);
            _pdfContent = _pdfContent.Replace("{expirydate}", accidentalModel.ExpiryDate);
            _pdfContent = _pdfContent.Replace("{pastclaims}", accidentalModel.PastClaimDetails);
            _pdfContent = _pdfContent.Replace("{criminalconvictions}", accidentalModel.IsAnyCriminalConviction == true ? "Yes" : "No");
            _pdfContent = _pdfContent.Replace("{policycancelled}", accidentalModel.IsPolicyCancelled == true ? "Yes" : "No");
            _pdfContent = _pdfContent.Replace("{providedetails}", accidentalModel.ProvidedDetails);
        



            TextWriter _fWriter = new StreamWriter(HostingEnvironment.MapPath("~/Templates/dfTemplate.htm"));
            _fWriter.Write(_pdfContent);
            _fWriter.Close();
            _fWriter.Dispose();

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
            byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(HostingEnvironment.MapPath("~/Templates/dfTemplate.htm"));
            string pdfName = RandomString(7) + ".pdf";
            accidentalModel.PDFName = pdfName;
            System.IO.File.WriteAllBytes(HostingEnvironment.MapPath("~/PdfTemplates/AccidentalClaimForms/" + pdfName), pdfBytes);
            Mapper.Initialize(new MapperConfigurationExpression() { AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true });
            tblAccidentalClaim accidental = Mapper.Instance.Map<tblAccidentalClaim>(accidentalModel);
            accidental.IsApproved = false;
            accidental.CreatedDate = DateTime.Now;
            db.tblAccidentalClaims.Add(accidental);
            db.SaveChanges();
            return accidental.Id;

        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public string ApproveAccidentalClaimById(long id)
        {
            try
            {
                string str1 = HostingEnvironment.MapPath("~/PdfTemplates/AccidentalClaimForms/");
                string appSetting = ConfigurationManager.AppSettings["AccidentalClaimBackupPath"];
                tblAccidentalClaim tblAccidentalClaim = db.tblAccidentalClaims.FirstOrDefault(x => x.Id == id);
                if (tblAccidentalClaim == null)
                    return string.Empty;
                string host = ConfigurationManager.AppSettings["Host"];
                string from = ConfigurationManager.AppSettings["FromEmail"];
                string insurerEmail = ConfigurationManager.AppSettings["InsurerEmail"];
                string password = ConfigurationManager.AppSettings["Password"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                string str2 = str1 + tblAccidentalClaim.PDFName;
                string requestUri = appSetting + tblAccidentalClaim.PDFName;
                if (System.IO.File.Exists(str2))
                {
                    MailMessage message = new MailMessage();
                    string str3 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("Hi", "Dear").Replace("{UserName}", "Sir/Mam,").Replace("{Message}", "We have sent you an accidental claim form .<br/>Please find attached PDF").Replace("{Link}", string.Empty);
                    message.Attachments.Add(new Attachment(str2)
                    {
                        ContentDisposition = {
              Inline = true
            }
                    });
                    message.From = new MailAddress(from, "BiblioVélo Support");
                    message.To.Add(insurerEmail);
                    message.Subject = "BiblioVélo::Accidental Claim Form";
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
                        Common.ExcepLog((Exception)ex1);
                       
                    }
                }
       
                MailMessage messageToUser = new MailMessage();
                string str4 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("{UserName}", tblAccidentalClaim.FullName + " ,").Replace("{Message}", "Your Accidental claim form regarding Booking Ref :" + tblAccidentalClaim.BookingId + " has been approved by the Admin and send it to the insurer for further inquiry.<br/>").Replace("{Link}", string.Empty);
                string email = tblAccidentalClaim.Email;
                string str5 = "BiblioVélo:: Accidental Claim Approved";
                messageToUser.From = new MailAddress(from, "BiblioVélo Support");
                messageToUser.To.Add(email);
                messageToUser.Subject = str5;
                messageToUser.IsBodyHtml = true;
                messageToUser.Body = str4;
                SmtpClient smtpClient1 = new SmtpClient(host, port);
                smtpClient1.EnableSsl = true;
                smtpClient1.Credentials = new NetworkCredential(from, password);
                try
                {
                    smtpClient1.Send(messageToUser);
                }
                catch (SmtpException ex1)
                {
                    Common.ExcepLog(ex1);

                }
                tblAccidentalClaim.IsApproved = new bool?(true);
                this.db.SaveChanges();
                return "deleted";
            }
           
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }

        public IEnumerable<AccidentalClaimModel> GetAccidentalClaims()
        {
            try
            {
                List<tblAccidentalClaim> list = db.tblAccidentalClaims.Where(x => x.IsApproved != true).ToList();
                List<AccidentalClaimModel> accidentalClaimModelList = new List<AccidentalClaimModel>();
                string appSetting = ConfigurationManager.AppSettings["AccidentalClaimPdf"];
                foreach (tblAccidentalClaim tblAccidentalClaim in list)
                    accidentalClaimModelList.Add(new AccidentalClaimModel()
                    {
                        Id = tblAccidentalClaim.Id,
                        FullName = tblAccidentalClaim.FullName,
                        Address = tblAccidentalClaim.Address,
                        BookingId = tblAccidentalClaim.BookingId.ToString(),
                        CreatedDate = Convert.ToDateTime(tblAccidentalClaim.CreatedDate).ToString("MM-dd-yyyy  hh:mm tt"),
                        Email = tblAccidentalClaim.Email,
                        PDFName = appSetting + tblAccidentalClaim.PDFName
                    });
                return (IEnumerable<AccidentalClaimModel>)accidentalClaimModelList;
            }
      
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;
            }
        }

        public string RejectAccidentalClaimById(long id)
        {
            try
            {
                tblAccidentalClaim tblAccidentalClaim = db.tblAccidentalClaims.FirstOrDefault(x => x.Id == id);
                if (tblAccidentalClaim == null)
                    return string.Empty;
                string host = ConfigurationManager.AppSettings["Host"];
                string from = ConfigurationManager.AppSettings["FromEmail"];
                string password = ConfigurationManager.AppSettings["Password"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                MailMessage messageToUser = new MailMessage();
                string str1 = System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplate"), "MailTemplate.html")).Replace("{UserName}", tblAccidentalClaim.FullName + " ,").Replace("{Message}", "Your Accidental claim form regarding Booking Ref :" + tblAccidentalClaim.BookingId + " has been rejected by the Admin<br/>").Replace("{Link}", string.Empty);
                string email = tblAccidentalClaim.Email;
                string str2 = "BiblioVélo:: Accidental Claim Rejected";
                messageToUser.From = new MailAddress(from, "BiblioVélo Support");
                messageToUser.To.Add(email);
                messageToUser.Subject = str2;
                messageToUser.IsBodyHtml = true;
                messageToUser.Body = str1;
             
                SmtpClient smtpClient1 = new SmtpClient(host, port);
                smtpClient1.EnableSsl = true;
                smtpClient1.Credentials = new NetworkCredential(from, password);
                try
                {
                    smtpClient1.Send(messageToUser);
                }
                catch (SmtpException ex1)
                {
                    Common.ExcepLog(ex1);

                }
                this.db.tblAccidentalClaims.Remove(tblAccidentalClaim);
                this.db.SaveChanges();
                return "deleted";
            }
          
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                return string.Empty;
            }
        }

    }
}
