using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.BookingDetailModel;

namespace BiblioVelo.Services.Services
{
    public interface IBookingDetailServices
    {
        IEnumerable<BookingDetail> GetAllBookings();
    }
}
