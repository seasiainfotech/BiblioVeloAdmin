using AutoMapper;
using AutoMapper.Configuration;
using BiblioVelo.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiblioVelo.Services.Models.BookingDetailModel;

namespace BiblioVelo.Data.Repository
{
    public class BookingDetailRepo : IBookingDetailRepo
    {
        BiblioveloEntities1 db = new BiblioveloEntities1();
        public IEnumerable<BookingDetail> GetAllBookings()
        {
            try
            {
                var bookingDetail = db.tblBookings.Where(x => x.IsCancel != true).ToList();

                foreach (var item in bookingDetail)
                {
                    DateTime startdate = Convert.ToDateTime(item.StartDate);
                    DateTime pickDate = Convert.ToDateTime(item.PickUpTime);
                    item.PickUpTime = new DateTime(startdate.Year, startdate.Month, startdate.Day, pickDate.Hour, pickDate.Minute, pickDate.Second);

                    DateTime endate = Convert.ToDateTime(item.EndDate);
                    DateTime dropdate = Convert.ToDateTime(item.DropTime);
                    item.DropTime = new DateTime(endate.Year, endate.Month, endate.Day, dropdate.Hour, dropdate.Minute, dropdate.Second);

                }
                if (bookingDetail.Any())
                {

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblBooking, BookingDetail>();
                    });


                    Mapper.Initialize(new MapperConfigurationExpression() { AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true });
                    var bookingModel = Mapper.Instance.Map<List<BookingDetail>>(bookingDetail);
                    foreach (var items in bookingModel)
                    {
                        var ownerDetails = db.tblUsers.FirstOrDefault(x => x.Id == items.OwnerId);
                        items.Owner = ownerDetails.FirstName + " " + ownerDetails.LastName;
                        var renterDetails = db.tblUsers.FirstOrDefault(x => x.Id == items.UserId);
                        items.Renter = renterDetails.FirstName + " " + renterDetails.LastName;

                        double totalPrice = Convert.ToDouble(items.TotalPrice);
                        totalPrice = Math.Round(totalPrice, 2);
                        items.TotalPrice = totalPrice.ToString();

                        double bookingPrice = Convert.ToDouble(items.BookingFee);
                        bookingPrice = Math.Round(bookingPrice, 2);
                        items.BookingFee = bookingPrice.ToString();

                        double amount = Convert.ToDouble(items.RentalFee);

                        double taxOnOwner = 15.00;

                        amount = Math.Round(amount, 2);

                        double deductedamount = ((taxOnOwner / 100) * amount);
                        deductedamount = Math.Round(deductedamount, 2);
                        double payableAmount = amount - deductedamount;

                        items.OwnerIncome = payableAmount.ToString();
                        double yellowJerseyFee = 0;
                        if (items.YellowJerseyFee != null)
                        {
                            yellowJerseyFee = Convert.ToDouble(items.YellowJerseyFee);
                            yellowJerseyFee = Math.Round(yellowJerseyFee, 2);
                        }


                        double biblioveloIncome = (bookingPrice + deductedamount) - yellowJerseyFee;
                        items.YellowJerseyFee = yellowJerseyFee.ToString();
                        items.BiblioveloIncome = biblioveloIncome.ToString();


                    }
                    return bookingModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
