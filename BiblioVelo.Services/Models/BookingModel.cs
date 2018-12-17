using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BiblioVelo.Services.Models
{


    public class BookingModelEntity
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<BookingModel> lstBookings { get; set; }
    }

    public class BookingModel
    {
        public long BookingId { get; set; }
        public long DisputeId { get; set; }
        public string RenterName { get; set; }
        public string OwnerName { get; set; }

        [DisplayName("Fine To Owner")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must Be a number")]
        [StringLength(5, ErrorMessage = "Amount can't be that big.")]
        public string FineToOwner { get; set; }

        [DisplayName("Fine To Renter")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must Be a number")]
        [StringLength(5, ErrorMessage = "Amount can't be that big.")]
        public string FineToRenter { get; set; }
        public bool IsPaymentDone { get; set; }
        public string ChargeId { get; set; }
        [Required]
        [DisplayName("Refund Type")]
        public string RefundType { get; set; }

        [DisplayName("Credit To Owner")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must Be a number")]
        [StringLength(5, ErrorMessage = "Amount can't be that big.")]
        public string AmountToOwner { get; set; }

        [DisplayName("Refund To Renter")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must Be a number")]
        [StringLength(5, ErrorMessage = "Amount can't be that big.")]
       
        public string SpecificAmount { get; set; } 
        public bool IsRefund { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<long> EquipmentId { get; set; }
        public Nullable<long> OwnerId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<bool> IsEvening { get; set; }
        public Nullable<System.DateTime> PickUpTime { get; set; }
        public Nullable<System.DateTime> DropTime { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsCancel { get; set; }
        public string CancelReason { get; set; }
        public Nullable<bool> IsConfirmed { get; set; }
        public string BookingCode { get; set; }
        public Nullable<bool> IsEmailSent { get; set; }
        public Nullable<int> NoOfDays { get; set; }
        public string TotalPrice { get; set; }
        public long? CancelledBy { get; set; }
        [DisplayName("Cancelled By")]
        public string Cancel { get; set; }
        public string RentalPeriodStartTime { get; set; }
        public string RentalPeriodEndTime { get; set; }
        public string RentalFee { get; set; }
        public string BookingFee { get; set; }
        public string Insurance { get; set; }

    }

        public enum TypeOfRefund
        {
        [Display(Name = "No Refund")]
        NoRefund,
            Full,
            Specific
        }
    }

