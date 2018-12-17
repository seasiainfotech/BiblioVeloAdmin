using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.DisputeModelEntity;
using static BiblioVelo.Services.Models.BookingModelEntity;
namespace BiblioVelo.Services.Repository
{
    public interface IDisputesRepository
    {
        IEnumerable<DisputeModel> GetDisputes();
        BookingModel GetRefundDetails(long Id,long disputeId);
        IEnumerable<BookingModel> GetBookingDetails(long Id);
        string RefundToUser(BookingModel booking);
        string FineToUser(BookingModel booking);
        string DeleteDisputeById(long id);
    }
}
