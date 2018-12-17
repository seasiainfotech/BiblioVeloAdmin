using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{

    public class DisputeModelEntity
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<DisputeModel> lstDisputes { get; set; }

        public class DisputeModel
        {
            public long Id { get; set; }
            public long RaisedById { get; set; }
            public long RaisedForId { get; set; }
            [DisplayName("Dispute Raised By")]
            public string RaisedBy { get; set; }
            [DisplayName("Dispute Raised Against")]
            public string RaisedFor { get; set; }
            public string BookingId { get; set; }
            [DisplayName("Reason for Dispute")]
            public string Reason { get; set; }
            public string CreatedDate { get; set; }


        }
    }
}
