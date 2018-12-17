using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class BookingDetailModel
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<BookingDetail> lstBooking { get; set; }

        public class BookingDetail
        {
            public long BookingId { get; set; }
            public long OwnerId { get; set; }
            public string Owner { get; set; }
            public long UserId { get; set; }
            public string Renter { get; set; }
            public string PickUpTime { get; set; }
            public string DropTime { get; set; }
            public int NoOfDays { get; set; }
            public string TotalPrice { get; set; }
            public string RentalFee { get; set; }
            public string BookingFee { get; set; }
            public string Insurance { get; set; }
            public string BiblioveloIncome { get; set; }
            public string OwnerIncome { get; set; }
            public string YellowJerseyFee { get; set; }
        }
    }
}
