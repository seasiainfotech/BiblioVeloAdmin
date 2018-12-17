using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class ReportedReviewsEntity
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<ReportedReviews> lstReviews { get; set; }
    }

    public class ReportedReviews
    {
        public long Id { get; set; }
        [DisplayName("Review By")]
        public string RatedBy { get; set; }
        [DisplayName("Review For")]
        public string RatedTo { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public string BookingId { get; set; }
        public bool IsOwnerRating { get; set; }
        public string CreatedDate { get; set; }
        public bool IsReported { get; set; }
        [DisplayName("Report by")]
        public string ReportedBy { get; set; }
        [DisplayName("Reason For Report")]
        public string Reason { get; set; }
        public long RatingId { get; set; }
       

    }
}
