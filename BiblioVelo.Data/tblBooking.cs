//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BiblioVelo.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBooking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBooking()
        {
            this.tblDisputes = new HashSet<tblDispute>();
        }
    
        public long BookingId { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<long> EquipmentId { get; set; }
        public Nullable<long> OwnerId { get; set; }
        public Nullable<bool> IsVerifiedAccount { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
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
        public Nullable<long> CancelledBy { get; set; }
        public Nullable<System.DateTime> RentalPeriodStartTime { get; set; }
        public Nullable<System.DateTime> RentalPeriodEndTime { get; set; }
        public Nullable<bool> IsRented { get; set; }
        public Nullable<bool> IsNotificationSent { get; set; }
        public Nullable<bool> IsPickupNotificationSent { get; set; }
        public Nullable<bool> IsBookingEnded { get; set; }
        public Nullable<bool> IsPaymentDone { get; set; }
        public string ChargeId { get; set; }
        public string TransferId { get; set; }
        public Nullable<bool> IsRefund { get; set; }
        public Nullable<bool> IsOwnerRaisedDispute { get; set; }
        public Nullable<bool> IsRenterRaisedDispute { get; set; }
        public Nullable<bool> IsTheftClaimRaised { get; set; }
        public Nullable<bool> IsAccidentalClaimRaised { get; set; }
        public Nullable<bool> IsOwnerRated { get; set; }
        public Nullable<bool> IsRenterRated { get; set; }
        public string RentalFee { get; set; }
        public string BookingFee { get; set; }
        public string Insurance { get; set; }
        public Nullable<System.DateTime> LastSentNotificationTime { get; set; }
        public Nullable<bool> IsMailForTheftSent { get; set; }
        public Nullable<bool> IsNotificationForSecuritySent { get; set; }
        public Nullable<bool> IsSecurityAmountPaid { get; set; }
        public Nullable<bool> IsSecurityByPaypal { get; set; }
        public string SecurityId { get; set; }
        public Nullable<bool> IsSecurityCaptureOrRefund { get; set; }
        public Nullable<System.DateTime> LastSentReminderAt { get; set; }
        public Nullable<System.DateTime> LastSentNotificationTimeForReturn { get; set; }
        public string MessageForOwner { get; set; }
        public Nullable<bool> IsEmailChecklistSent { get; set; }
        public Nullable<bool> IsOwnerEmailSent { get; set; }
        public string YellowJerseyFee { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDispute> tblDisputes { get; set; }
        public virtual tblBooking tblBooking1 { get; set; }
        public virtual tblBooking tblBooking2 { get; set; }
    }
}
