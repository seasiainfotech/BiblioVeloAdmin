﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BiblioveloEntities1 : DbContext
    {
        public BiblioveloEntities1()
            : base("name=BiblioveloEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblEquipment> tblEquipments { get; set; }
        public virtual DbSet<tblRating> tblRatings { get; set; }
        public virtual DbSet<tblReportedReview> tblReportedReviews { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblUserDeviceMapping> tblUserDeviceMappings { get; set; }
        public virtual DbSet<tblDispute> tblDisputes { get; set; }
        public virtual DbSet<tblLogin> tblLogins { get; set; }
        public virtual DbSet<tblImage> tblImages { get; set; }
        public virtual DbSet<tblAccidentalClaim> tblAccidentalClaims { get; set; }
        public virtual DbSet<tblAdminFee> tblAdminFees { get; set; }
        public virtual DbSet<tblTheftClaim> tblTheftClaims { get; set; }
        public virtual DbSet<tblBooking> tblBookings { get; set; }
    }
}
