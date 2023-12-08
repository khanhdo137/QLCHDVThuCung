using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DichVuThuCungKVH.Model
{
    public class GioHang
    {
        private LTWEntities db = new LTWEntities();
        public int iMaSanPham { get; set; }
        public string sṬenSP { get; set; }
        public string sTenSP { get; }
        public string sAnh { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public GioHang(int ms)

        {
            iMaSanPham = ms;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSanPham);
            sTenSP = sp.TenSP;
            sAnh = sp.Anh;
            dDonGia = double.Parse(sp.GiaBan.ToString());
            iSoLuong = 1;

        }
    }
}