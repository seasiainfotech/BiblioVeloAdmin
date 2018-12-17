using BiblioVelo.Data.Repository;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using BiblioVelo.Services.Services;
using System.Collections.Generic;
using static BiblioVelo.Services.Models.DisputeModelEntity;
namespace BiblioVelo.BusinessLogic.Services
{
    public class DisputesService : IDisputesService
    {
        IDisputesRepository _disputeRepo = new DisputesRepository();

        /// <summary>
        /// GetDisputes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DisputeModel> GetDisputes()
        {
            
            var listdisputes = _disputeRepo.GetDisputes();
            return listdisputes;
        }
        /// <summary>
        /// GetRefundDetails
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="disputeId"></param>
        /// <returns></returns>
        public BookingModel GetRefundDetails(long Id,long disputeId)
        {
           
           var booking = _disputeRepo.GetRefundDetails(Id,disputeId);
            return booking;
        }
        /// <summary>
        /// GetBookingDetails
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<BookingModel> GetBookingDetails(long Id)
        {
            var booking = _disputeRepo.GetBookingDetails(Id);
            return booking;
        }
        /// <summary>
        /// RefundToUser
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public string RefundToUser(BookingModel booking)
        {
           return _disputeRepo.RefundToUser(booking);
        }
        /// <summary>
        /// FineToUser
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public string FineToUser(BookingModel booking)
        {
            return _disputeRepo.FineToUser(booking);
        }
        /// <summary>
        /// DeleteDisputeById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteDisputeById(long id)
        {
            return _disputeRepo.DeleteDisputeById(id);
        }
    }
}
