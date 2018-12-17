using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class EquipmentDetailEntity
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<EquipmentDetail> lstEquipment { get; set; }


        public class EquipmentDetail
        {
            public Int64 Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Model { get; set; }
            public decimal? Price { get; set; }
            public string Size { get; set; }
            public bool? IsBlock { get; set; }

        }
    }
}
