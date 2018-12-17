using AutoMapper;
using AutoMapper.Configuration;
using BiblioVelo.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.EquipmentDetailEntity;

namespace BiblioVelo.Data.Repository
{
    public class EquipmentRepo : IEquipmentRepo
    {
        BiblioveloEntities1 db = new BiblioveloEntities1();
        public IEnumerable<EquipmentDetail> GetAllEquipment()
        {
            try
            {
                var equipDetail = (from equipment in db.tblEquipments
                                   join user in db.tblUsers
                                   on equipment.UserId equals user.Id
                                   where equipment.IsActive == true
                                   select new EquipmentDetail()
                                   {
                                       Id = equipment.Id,
                                       Name = user.FirstName + " " + user.LastName,
                                       PhoneNumber = user.PhoneNumber,
                                       IsBlock = equipment.IsBlock,
                                       Model = equipment.Model,
                                       Price =equipment.Price,
                                       Size = equipment.Size
                                   }).ToList();


                if (equipDetail.Any())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblEquipment, EquipmentDetail>();
                    });
                    Mapper.Initialize(new MapperConfigurationExpression() { AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true });
                    var equipModel = Mapper.Instance.Map<List<EquipmentDetail>>(equipDetail); ;
                    return equipModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public void EquipmentStatus(int EquipmentId, bool Status)
        {
            try
            {
                var equipmentStatus = db.tblEquipments.Where(x => x.Id == EquipmentId).FirstOrDefault();
                equipmentStatus.IsBlock = Status;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
