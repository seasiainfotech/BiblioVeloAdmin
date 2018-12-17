using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class TheftClaimModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [CheckDateRange]
        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "This field is required")]
      
        public string BookingId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a Valid Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string ListOfStolenItems { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string ApproxValue { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Circumstances { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string WhoWasResponsible { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HowLongUnattended { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [CheckDateRange(ErrorMessage = "Invalid Date and Time")]
        [Display(Name = "Time and date")]
        public DateTime TimeAndDateLastSeen { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [CheckDateRange(ErrorMessage = "Invalid Date and Time")]
        [Display(Name = "Time and date")]
        public DateTime TimeAndDateTheftDiscovered { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public bool? IsAnyWitness { get; set; }

        public string WitnessDetails { get; set; } = string.Empty;

        public string PoliceContactNumber { get; set; } = string.Empty;

        public string CrimeIncidentNumber { get; set; } = string.Empty;

        public bool IsPoliceAttend { get; set; }

        public bool IsReportedToPoliceImmediately { get; set; }

        public string ReasonForNotReportedImmediately { get; set; } = string.Empty;

        public string HowBicycleWasSecured { get; set; } = string.Empty;

        public string DetailOfSecuredLock { get; set; } = string.Empty;

        public string HowAccessGained { get; set; } = string.Empty;

        public string AlternativeSecurityMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required")]
        public bool? IsAnotherInsurer { get; set; }

        public string PreviousInsurerName { get; set; } = string.Empty;

        public string ExpiryDate { get; set; } = string.Empty;

        public string PastClaimDetails { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required")]
        public bool? IsAnyCriminalConviction { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public bool? IsPolicyCancelled { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public bool? IsRefusedRenewal { get; set; }

        public string ProvidedDetails { get; set; } = string.Empty;

        public string PDFName { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Please tick the box to proceed")]
        public bool IsAgreed { get; set; }

        public string CreatedDate { get; set; }
    }
}
