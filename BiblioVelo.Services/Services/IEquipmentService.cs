using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.EquipmentDetailEntity;

namespace BiblioVelo.Services.Services
{
    public interface IEquipmentService
    {
        IEnumerable<EquipmentDetail> GetAllEquipment();

        void EquipmentStatus(int EquipmentId, bool Status);
    }
}
