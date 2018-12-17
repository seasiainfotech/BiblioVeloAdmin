using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class AccidentalClaimModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        //validate:Must be greater than current date
        //[Range(typeof(DateTime), "1/1/1966", "1/1/2020", ErrorMessage = "Invalid Date of birth")]
        [CheckDateRange(ErrorMessage = "Invalid Date of birth")]
        public string DOB { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string BookingId { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [CheckValidDateTime(ErrorMessage ="Invalid Date")]
        public string DateOfIncident { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [CheckValidDateTime(ErrorMessage = "Invalid Time")]
        public string TimeOfIncident { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string InCharge { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string IncidentDescription { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string Circumstances { get; set; }
        public string WitnessDetails { get; set; }
        public string ThirdPartyDetails { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required!")]
        public bool? IsReportedToPolice { get; set; } 
        public string ItemsClaimedFor { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public bool? IsAnotherInsurer { get; set; } = false;
        public string PreviousInsurerName { get; set; } = string.Empty;

        public string ExpiryDate { get; set; } = string.Empty;
        public string PastClaimDetails { get; set; } = string.Empty;
        [Required(ErrorMessage = "This field is required!")]
        public bool? IsAnyCriminalConviction { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public bool? IsPolicyCancelled { get; set; } 
        [Required(ErrorMessage = "This field is required!")]
        public bool? IsRefusedRenewal { get; set; }
        public string ProvidedDetails { get; set; } = string.Empty;
        public string PDFName { get; set; }
        public bool IsApproved { get; set; } = false;
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Please tick the box to proceed")]
        public bool IsAgreed { get; set; }
        public string CreatedDate { get; set; }
    }
    public class CheckDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(value);

                return ValidationResult.Success;
            }
            catch(Exception ex)
            {
                return new ValidationResult(ErrorMessage ?? "Invalid DOB");
            }
        }

    }
    public class CheckValidDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {

                DateTime dt = Convert.ToDateTime(value);

                return ValidationResult.Success;

            }


            catch (Exception ex)
            {
                return new ValidationResult(ErrorMessage ?? "Invalid DateTime");
            }
        }

    }
}
