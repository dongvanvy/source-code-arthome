﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WEBREPORTMOBILEAPP.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBMOBILEAPPWALLEntities : DbContext
    {
        public DBMOBILEAPPWALLEntities()
            : base("name=DBMOBILEAPPWALLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_BaoCaoHinhAnh> tbl_BaoCaoHinhAnh { get; set; }
        public virtual DbSet<tbl_bigplan> tbl_bigplan { get; set; }
        public virtual DbSet<tbl_bill> tbl_bill { get; set; }
        public virtual DbSet<Tbl_BillCNKDetail> Tbl_BillCNKDetail { get; set; }
        public virtual DbSet<Tbl_BillChuyenNhanKho> Tbl_BillChuyenNhanKho { get; set; }
        public virtual DbSet<tbl_billDetail> tbl_billDetail { get; set; }
        public virtual DbSet<tbl_CaLamViecDetail> tbl_CaLamViecDetail { get; set; }
        public virtual DbSet<tbl_chitietDondathang> tbl_chitietDondathang { get; set; }
        public virtual DbSet<Tbl_ChitietDonNhapHang> Tbl_ChitietDonNhapHang { get; set; }
        public virtual DbSet<tbl_ChitietSupChamDiem> tbl_ChitietSupChamDiem { get; set; }
        public virtual DbSet<tbl_chucnangphu> tbl_chucnangphu { get; set; }
        public virtual DbSet<tbl_ChuongTrinhDoiQua> tbl_ChuongTrinhDoiQua { get; set; }
        public virtual DbSet<tbl_DanhMucQuaTang> tbl_DanhMucQuaTang { get; set; }
        public virtual DbSet<tbl_DetailBaoCaoHinhAnh> tbl_DetailBaoCaoHinhAnh { get; set; }
        public virtual DbSet<tbl_DMQT_CTDQ> tbl_DMQT_CTDQ { get; set; }
        public virtual DbSet<tbl_DoiQua> tbl_DoiQua { get; set; }
        public virtual DbSet<tbl_doithucanhtranh> tbl_doithucanhtranh { get; set; }
        public virtual DbSet<Tbl_DoithucanhtranhIMAGE> Tbl_DoithucanhtranhIMAGE { get; set; }
        public virtual DbSet<Tbl_DoithucanhtranhIMAGEDetail> Tbl_DoithucanhtranhIMAGEDetail { get; set; }
        public virtual DbSet<tbl_Dondathang> tbl_Dondathang { get; set; }
        public virtual DbSet<Tbl_DonNhapHang> Tbl_DonNhapHang { get; set; }
        public virtual DbSet<tbl_DTCTBrand> tbl_DTCTBrand { get; set; }
        public virtual DbSet<tbl_DTCTcrmrange> tbl_DTCTcrmrange { get; set; }
        public virtual DbSet<tbl_DTCTCty> tbl_DTCTCty { get; set; }
        public virtual DbSet<tbl_DTCTffrange> tbl_DTCTffrange { get; set; }
        public virtual DbSet<tbl_DTCTformat> tbl_DTCTformat { get; set; }
        public virtual DbSet<tbl_DTCTltmrange> tbl_DTCTltmrange { get; set; }
        public virtual DbSet<tbl_DTCTonpostoffpost> tbl_DTCTonpostoffpost { get; set; }
        public virtual DbSet<tbl_DTCTtype> tbl_DTCTtype { get; set; }
        public virtual DbSet<tbl_DTCTVARIANT> tbl_DTCTVARIANT { get; set; }
        public virtual DbSet<Tbl_HsSms> Tbl_HsSms { get; set; }
        public virtual DbSet<Tbl_ImageDonNhapHang> Tbl_ImageDonNhapHang { get; set; }
        public virtual DbSet<tbl_Kho> tbl_Kho { get; set; }
        public virtual DbSet<Tbl_KhoChiNhanh> Tbl_KhoChiNhanh { get; set; }
        public virtual DbSet<Tbl_KhoChuyenNhan> Tbl_KhoChuyenNhan { get; set; }
        public virtual DbSet<Tbl_KhoDoi> Tbl_KhoDoi { get; set; }
        public virtual DbSet<Tbl_KHOQUA> Tbl_KHOQUA { get; set; }
        public virtual DbSet<Tbl_KhoXe> Tbl_KhoXe { get; set; }
        public virtual DbSet<tbl_menu> tbl_menu { get; set; }
        public virtual DbSet<tbl_NganhHang> tbl_NganhHang { get; set; }
        public virtual DbSet<tbl_NhanHang> tbl_NhanHang { get; set; }
        public virtual DbSet<tbl_NhomNganhHang> tbl_NhomNganhHang { get; set; }
        public virtual DbSet<tbl_Price> tbl_Price { get; set; }
        public virtual DbSet<tbl_RulesChuongTrinhDoiQua> tbl_RulesChuongTrinhDoiQua { get; set; }
        public virtual DbSet<tbl_shop> tbl_shop { get; set; }
        public virtual DbSet<tbl_sku> tbl_sku { get; set; }
        public virtual DbSet<tbl_SupBigPlan> tbl_SupBigPlan { get; set; }
        public virtual DbSet<tbl_SupChamDiem> tbl_SupChamDiem { get; set; }
        public virtual DbSet<tbl_SupCheckIn> tbl_SupCheckIn { get; set; }
        public virtual DbSet<Tbl_SupName> Tbl_SupName { get; set; }
        public virtual DbSet<tbl_Target> tbl_Target { get; set; }
        public virtual DbSet<tbl_TieuChiBaoCaoHinhAnh> tbl_TieuChiBaoCaoHinhAnh { get; set; }
        public virtual DbSet<tbl_TieuChiChamDiem> tbl_TieuChiChamDiem { get; set; }
        public virtual DbSet<tbl_user> tbl_user { get; set; }
        public virtual DbSet<tbl_UserCheckInOut> tbl_UserCheckInOut { get; set; }
        public virtual DbSet<VW_banhang> VW_banhang { get; set; }
        public virtual DbSet<VW_BCHINHANH> VW_BCHINHANH { get; set; }
        public virtual DbSet<VW_Bigplan> VW_Bigplan { get; set; }
        public virtual DbSet<VW_DatHang> VW_DatHang { get; set; }
        public virtual DbSet<VW_DeXuatNhapHang> VW_DeXuatNhapHang { get; set; }
        public virtual DbSet<VW_DOIQUA> VW_DOIQUA { get; set; }
        public virtual DbSet<vw_doithucanhtranh> vw_doithucanhtranh { get; set; }
        public virtual DbSet<VW_DoithucanhtranhImage> VW_DoithucanhtranhImage { get; set; }
        public virtual DbSet<VW_KHO> VW_KHO { get; set; }
        public virtual DbSet<VW_NHAPHANG> VW_NHAPHANG { get; set; }
        public virtual DbSet<VW_Supchamdiem> VW_Supchamdiem { get; set; }
        public virtual DbSet<VW_SupCheckin> VW_SupCheckin { get; set; }
        public virtual DbSet<VW_SUPCHECKINOUT> VW_SUPCHECKINOUT { get; set; }
        public virtual DbSet<VW_UsercheckInout> VW_UsercheckInout { get; set; }
    }
}
