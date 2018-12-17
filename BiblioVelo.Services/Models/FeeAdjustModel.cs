using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Models
{
    public class FeeAdjustModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("No. of days = 1")]
        [Range(0.1, 99.0, ErrorMessage = "Invalid Input - must be between 0.1 and 99")]
        [RegularExpression("\\d+(\\.\\d{1,2})?", ErrorMessage = "Please enter valid input and maximum upto 2 digits after decimal")]
        public double InsuranceFor1Day { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("No. of days 2-3")]
        [Range(0.1, 99.0, ErrorMessage = "Invalid Input - must be between 0.1 and 99")]
        [RegularExpression("\\d+(\\.\\d{1,2})?", ErrorMessage = "Please enter valid input and maximum upto 2 digits after decimal")]
        public double InsuranceFor2to3Days { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("No. of days 4-6")]
        [Range(0.1, 99.0, ErrorMessage = "Invalid Input - must be between 0.1 and 99")]
        [RegularExpression("\\d+(\\.\\d{1,2})?", ErrorMessage = "Please enter valid input and maximum upto 2 digits after decimal")]
        public double InsuranceFor4to6Days { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("No. of days 7-10")]
        [Range(0.1, 99.0, ErrorMessage = "Invalid Input - must be between 0.1 and 99")]
        [RegularExpression("\\d+(\\.\\d{1,2})?", ErrorMessage = "Please enter valid input and maximum upto 2 digits after decimal")]
        public double InsuranceFor7to10Days { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("No.of days more than 11")]
        [Range(0.1, 99.0, ErrorMessage = "Invalid Input - must be between 0.1 and 99")]
        [RegularExpression("\\d+(\\.\\d{1,2})?", ErrorMessage = "Please enter valid input and maximum upto 2 digits after decimal")]
        public double InsuranceForMoreThan10Days { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Booking fee to Renter")]
        [Range(0.1, 99.0, ErrorMessage = "Invalid Input - must be between 0.1 and 99")]
        [RegularExpression("\\d+(\\.\\d{1,2})?", ErrorMessage = "Please enter valid input and maximum upto 2 digits after decimal")]
        public double BookingFeeRenter { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Booking fee to Owner")]
        [Range(0.1, 99.0, ErrorMessage = "Invalid Input - must be between 0.1 and 99")]
        [RegularExpression("\\d+(\\.\\d{1,2})?", ErrorMessage = "Please enter valid input and maximum upto 2 digits after decimal")]
        public double BookingFeeOwner { get; set; }

        public string ModifiedDate { get; set; }
    }
}
