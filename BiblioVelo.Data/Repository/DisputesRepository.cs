using System;
using System.Collections.Generic;
using System.Linq;
using BiblioVelo.Services.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using BiblioVelo.Services.Repository;
using static BiblioVelo.Services.Models.DisputeModelEntity;
using static BiblioVelo.Services.Models.BookingModelEntity;
using AutoMapper;
using AutoMapper.Configuration;
using BiblioVelo.Data.CommonClasses;
using System.Web.Hosting;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Web;

namespace BiblioVelo.Data.Repository
{
    public class DisputesRepository : IDisputesRepository
    {
        BiblioveloEntities1 _dbEntity = new BiblioveloEntities1();

        /// <summary>
        /// GetDisputes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DisputeModel> GetDisputes()
        {
            try
            {
                var tblDisputes = _dbEntity.tblDisputes.Where(x => x.IsResolved == false || x.IsResolved == null).ToList();
                List<DisputeModel> listdisputes = new List<DisputeModel>();
                foreach (var item in tblDisputes)
                {
                    DisputeModel disputes = new DisputeModel();
                   
                    disputes.Id = (long)item.Id;
                    disputes.RaisedById = (long)item.RaisedBy;
                    disputes.RaisedForId = (long)item.RaisedFor;
                    long raisedByid = (long)item.RaisedBy;
                    var raisedbyUser = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == raisedByid);
                    long raisedForid = (long)item.RaisedFor;
                    var raisedforUser = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == raisedForid);                   
                    long bookingid = (long)item.BookingId;
                    var booking = _dbEntity.tblBookings.Find(bookingid);
                    if (booking.UserId == item.RaisedBy)
                    {
                        disputes.RaisedBy = raisedbyUser.FirstName + " " + raisedbyUser.LastName + " (Renter)";
                        disputes.RaisedFor = raisedforUser.FirstName + " " + raisedforUser.LastName + " (Owner)";
                    }
                    else if(booking.OwnerId== item.RaisedBy)
                    {
                        disputes.RaisedBy = raisedbyUser.FirstName + " " + raisedbyUser.LastName + " (Owner)";
                        disputes.RaisedFor = raisedforUser.FirstName + " " + raisedforUser.LastName + " (Renter)";
                    }
                    disputes.BookingId = item.BookingId.ToString();
                    disputes.CreatedDate = item.CreatedDate.ToString();
                    disputes.Reason = item.Reason;
                    listdisputes.Add(disputes);
                }
                return listdisputes;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                Common.ExcepLog(dbEx);
                throw;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;
            }
        }

        /// <summary>
        /// GetRefundDetails
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="disputeId"></param>
        /// <returns></returns>
        public BookingModel GetRefundDetails(long Id,long disputeId)
        {
            try
            {
                var equipDetail = _dbEntity.tblBookings.Where(x => x.BookingId == Id).FirstOrDefault(); ;
                long ownerId = (long)equipDetail.OwnerId;
                long renterId = (long)equipDetail.UserId;
                var ownerDetails = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == ownerId);
                var renterDetails = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == renterId);
                if (equipDetail != null)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblBooking, BookingModel>();
                    });
                    Mapper.Initialize(new MapperConfigurationExpression() { AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true });
                    var bookingModel = Mapper.Instance.Map<BookingModel>(equipDetail);
                    bookingModel.DisputeId = disputeId;
                    bookingModel.RenterName = renterDetails.FirstName + " " + renterDetails.LastName;
                    bookingModel.OwnerName = ownerDetails.FirstName + " " + ownerDetails.LastName;
                    double total = Convert.ToDouble(bookingModel.TotalPrice);
                    total = Math.Round(total, 2);
                    bookingModel.TotalPrice= "£" + total;
                    return bookingModel;
                }
                    return null;               
            }


            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                Common.ExcepLog(dbEx);
                throw;

            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;
            }

        }
        /// <summary>
        /// GetBookingDetails
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<BookingModel> GetBookingDetails(long Id)
        {

            var equipDetail = _dbEntity.tblBookings.Where(x => x.BookingId == Id).ToList(); ;
            long ownerId=(long) equipDetail[0].OwnerId;
            long renterId = (long)equipDetail[0].UserId;
            var ownerDetails = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == ownerId);
            var renterDetails = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == renterId);
            if (equipDetail != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<tblBooking, BookingModel>(); });
                Mapper.Initialize(new MapperConfigurationExpression()
                    {AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true});
                var bookingModel = Mapper.Instance.Map<List<BookingModel>>(equipDetail);
                ;
                bookingModel[0].RenterName = renterDetails.FirstName + " " + renterDetails.LastName;
                bookingModel[0].OwnerName = ownerDetails.FirstName + " " + ownerDetails.LastName;
                if (bookingModel[0].CancelledBy == bookingModel[0].UserId)
                {
                    bookingModel[0].Cancel = renterDetails.FirstName + " " + renterDetails.LastName;
                }
                else if (bookingModel[0].CancelledBy == bookingModel[0].OwnerId)
                {
                    bookingModel[0].Cancel = ownerDetails.FirstName + " " + ownerDetails.LastName;
                }

                double total = Convert.ToDouble(bookingModel[0].TotalPrice);
                total = Math.Round(total, 2);
                bookingModel[0].TotalPrice = "£" + total;
                return bookingModel;
            }

            return null;

        }
        /// <summary>
        /// RefundToUser
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public string RefundToUser(BookingModel booking)
        {
            string refundId = null;
            string host = ConfigurationManager.AppSettings["Host"];
            string from = ConfigurationManager.AppSettings["FromEmail"];            
            string password = ConfigurationManager.AppSettings["Password"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            List<PushNotificationModel> pushlist = new List<PushNotificationModel>();
            if (booking.RefundType == "Full")
            {
                refundId = PaymentGateway.RefundToUser(booking.ChargeId, true, 0);
                if (refundId != null)
                {
                    var renterDevice = _dbEntity.tblUserDeviceMappings.Where(x => x.UserId == booking.UserId).ToList();
                    if (renterDevice != null && renterDevice.Count > 0)
                    {
                        foreach (var items in renterDevice)
                        {
                            PushNotificationModel pushModel = new PushNotificationModel();
                            pushModel.DeviceToken = items.DeviceToken;
                            pushModel.Message = "Full Refund is initiated regarding your Dispute(" + booking.DisputeId + ") for booking " + booking.BookingId;
                            pushlist.Add(pushModel);
                        }
                    }
                }
            }
            else if (booking.RefundType == "Specific")
            {
                refundId = PaymentGateway.RefundToUser(booking.ChargeId, false,Convert.ToInt32(booking.SpecificAmount));
                if (refundId != null)
                {
                    var renterDevice = _dbEntity.tblUserDeviceMappings.Where(x => x.UserId == booking.UserId).ToList();
                    if (renterDevice != null && renterDevice.Count > 0) 
                    {
                        foreach (var items in renterDevice)
                        {
                            PushNotificationModel pushModel = new PushNotificationModel();
                            pushModel.DeviceToken = items.DeviceToken;
                            pushModel.Message = "Refund of Amount: £" + booking.SpecificAmount + " is initiated regarding your Dispute("+booking.DisputeId+") for booking " + booking.BookingId;
                            pushlist.Add(pushModel);
                        }
                    }
                }
            }
            else
            {
                var OwnerCredit = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == booking.OwnerId);
                if (OwnerCredit != null)
                {
                    if (OwnerCredit.CreditAmount != null)
                    {
                        OwnerCredit.CreditAmount = OwnerCredit.CreditAmount + Convert.ToInt32(booking.AmountToOwner);
                    }
                    else
                    {
                        OwnerCredit.CreditAmount = Convert.ToInt32(booking.AmountToOwner);
                    }
                }

                PaymentGateway.TranferPayment(Convert.ToInt32(booking.AmountToOwner), "GBP", OwnerCredit.AccountId);

                var ownerDevice = _dbEntity.tblUserDeviceMappings.Where(x => x.UserId == booking.OwnerId).ToList();
                if (ownerDevice != null && ownerDevice.Count > 0)
                {

                    foreach (var item in ownerDevice)
                    {
                        PushNotificationModel pushModel = new PushNotificationModel();
                        pushModel.DeviceToken = item.DeviceToken;
                        pushModel.Message = "You have been Credited by amount £" + booking.AmountToOwner + " is initiated regarding your Dispute(" + booking.DisputeId + ") for booking " + booking.BookingId;
                        pushlist.Add(pushModel);
                    }
                }
                refundId = string.Empty;
            }
            PushNotifications.PushToIphone(pushlist);
            if (!string.IsNullOrEmpty(refundId))
            {
                var bookingDetails = _dbEntity.tblBookings.FirstOrDefault(x => x.BookingId == booking.BookingId);
                if (bookingDetails != null)
                {
                    bookingDetails.IsRefund = true;
                    _dbEntity.SaveChanges();
                }
            }
               
            
      

            return refundId;
        }
        /// <summary>
        /// FineToUser
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public string FineToUser(BookingModel booking)
        {
            try
            {
                string host = ConfigurationManager.AppSettings["Host"];
                string from = ConfigurationManager.AppSettings["FromEmail"];
                
                string password = ConfigurationManager.AppSettings["Password"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                List<PushNotificationModel> pushList = new List<PushNotificationModel>();
                if (Convert.ToInt32(booking.FineToOwner) > 0)
                {
                    var ownerFine = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == booking.OwnerId);
                    var OwnerId = booking.OwnerId;
                    if (ownerFine.FineAmount != null)
                    {
                        ownerFine.FineAmount = ownerFine.FineAmount +Convert.ToInt32(booking.FineToOwner);
                    }
                    else
                    {
                        ownerFine.FineAmount = Convert.ToInt32(booking.FineToOwner);
                    }
                    var ownerDevice = _dbEntity.tblUserDeviceMappings.Where(x => x.UserId == OwnerId).ToList();
                    if (ownerDevice.Count > 0)
                    {
                        
                        foreach(var item in ownerDevice)
                        {
                            PushNotificationModel pushModel = new PushNotificationModel();
                            pushModel.DeviceToken = item.DeviceToken;
                            pushModel.Message = "You have been fined by amount £" + booking.FineToOwner+ " for Dispute(" + booking.DisputeId + ") for booking " + booking.BookingId;
                            pushList.Add(pushModel);
                        }
                    }

                    var ownerEmail = _dbEntity.tblLogins.FirstOrDefault(x => x.Id == ownerFine.LoginId);
          
                }
                if (Convert.ToInt32( booking.FineToRenter) > 0)
                {
                    var renterFine = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == booking.UserId);
                    var renterId = booking.UserId;
                    if (renterFine.FineAmount != null)
                    {
                        renterFine.FineAmount = renterFine.FineAmount + Convert.ToInt32(booking.FineToRenter);
                    }
                    else
                    {
                        renterFine.FineAmount = Convert.ToInt32(booking.FineToRenter);
                    }
                    var renterDevice = _dbEntity.tblUserDeviceMappings.Where(x => x.UserId == renterId).ToList();
                    if (renterDevice.Count > 0)
                    {

                        foreach (var item in renterDevice)
                        {
                            PushNotificationModel pushmodel = new PushNotificationModel();
                            pushmodel.DeviceToken = item.DeviceToken;
                            pushmodel.Message = "You have been fined by amount £" + booking.FineToRenter + " for Dispute(" + booking.DisputeId + ") for booking " + booking.BookingId;
                            pushList.Add(pushmodel);
                        }
                    }

                  
                  
                 
                }
                PushNotifications.PushToIphone(pushList);
                _dbEntity.SaveChanges();
                return "FineSuccess";
            }
            catch(Exception ex)
            {
                Common.ExcepLog(ex);
                return null;
            }
        }
        /// <summary>
        /// DeleteDisputeById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteDisputeById(long id)
        {
            try
            {
                var tblDispute = _dbEntity.tblDisputes.FirstOrDefault(x => x.Id == id);
                tblDispute.IsResolved = true;
                _dbEntity.SaveChanges();
                return "Deleted"; 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
