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
    
    public partial class tblAdditionalSecurityMeasure
    {
        public long Id { get; set; }
        public Nullable<long> EquipmentId { get; set; }
        public Nullable<bool> HaveSecureLock { get; set; }
        public string ProofImage { get; set; }
        public Nullable<bool> GPSTracker { get; set; }
        public string BicycleFrameNumber { get; set; }
        public string BicycleRegisterNumber { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual tblEquipment tblEquipment { get; set; }
    }
}
