using BiblioVelo.Data.Repository;
using BiblioVelo.Services.Repository;
using BiblioVelo.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.EquipmentDetailEntity;

namespace BiblioVelo.BusinessLogic.Services
{
    public class EquipmentService : IEquipmentService
    {
        IEquipmentRepo _repo;

        public IEnumerable<EquipmentDetail> GetAllEquipment()
        {
            try
            {
                EquipmentDetail objOutPut = new EquipmentDetail();
                _repo = new EquipmentRepo();
                var lst = _repo.GetAllEquipment();
                if (lst != null)
                {
                    return lst.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EquipmentStatus(int EquipmentId, bool Status)
        {
            try
            {
                _repo = new EquipmentRepo();
                _repo.EquipmentStatus(EquipmentId, Status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
