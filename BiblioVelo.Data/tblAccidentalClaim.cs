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
    
    public partial class tblAccidentalClaim
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string BookingId { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }
        public string DateOfIncident { get; set; }
        public string TimeOfIncident { get; set; }
        public string InCharge { get; set; }
        public string IncidentDescription { get; set; }
        public string Circumstances { get; set; }
        public string WitnessDetails { get; set; }
        public string ThirdPartyDetails { get; set; }
        public Nullable<bool> IsReportedToPolice { get; set; }
        public string ItemsClaimedFor { get; set; }
        public Nullable<bool> IsAnotherInsurer { get; set; }
        public string PreviousInsurerName { get; set; }
        public string ExpiryDate { get; set; }
        public string PastClaimDetails { get; set; }
        public Nullable<bool> IsAnyCriminalConviction { get; set; }
        public Nullable<bool> IsPolicyCancelled { get; set; }
        public Nullable<bool> IsRefusedRenewal { get; set; }
        public string ProvidedDetails { get; set; }
        public string PDFName { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}