﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dmart.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBDMartEntities : DbContext
    {
        public DBDMartEntities()
            : base("name=DBDMartEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_admin> tbl_admin { get; set; }
        public virtual DbSet<tbl_categories> tbl_categories { get; set; }
        public virtual DbSet<tbl_product> tbl_product { get; set; }
        public virtual DbSet<tbl_user> tbl_user { get; set; }
        public virtual DbSet<tbl_cart> tbl_cart { get; set; }
        public virtual DbSet<tbl_invoice> tbl_invoice { get; set; }
        public virtual DbSet<tbl_invoiceDetail> tbl_invoiceDetail { get; set; }
    }
}
