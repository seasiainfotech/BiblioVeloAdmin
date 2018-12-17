using BiblioVelo.Data.Repository;
using BiblioVelo.Services.Repository;
using BiblioVelo.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.BookingDetailModel;

namespace BiblioVelo.BusinessLogic.Services
{
    public class BookingDetailServices : IBookingDetailServices
    {
        IBookingDetailRepo _repo;
        public IEnumerable<BookingDetail> GetAllBookings()
        {
            try
            {
                BookingDetail objOutPut = new BookingDetail();
                _repo = new BookingDetailRepo();
                return _repo.GetAllBookings();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
