using DichVuThuCungKVH.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Areas.Admin.Controllers
{
    public class DoanhThuController : Controller
    {

        private DACSEntities db = new DACSEntities();

        // GET: Admin/DoanhThu
        public ActionResult Index()
        {
            var totalRevenue = db.CTDonHangs.Sum(od => od.DonGia * od.SoLuong);

            ViewBag.DoanhThu = totalRevenue;

            var TienDichVu = db.CTPhieuNhan_DichVu.Sum(n => n.DonGia * n.SoLuong);
            ViewBag.TienDichVu = TienDichVu;
            ///////
            var Thang1 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 1)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang1 = Thang1;
            ////////
            var Thang2 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 2)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang2 = Thang2;


            /////////
            var Thang3 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 3)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang3 = Thang3;


            ////////////
            var Thang4 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 4)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang4 = Thang4;



            ///////////
            var Thang5 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 5)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang5 = Thang5;


            ////////////
            var Thang6 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 6)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang6 = Thang6;

            ////////
            var Thang7 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 7)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang7 = Thang7;


            ///////
            var Thang8 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 8)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang8 = Thang8;

            ////////
            var Thang9 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 9)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang9 = Thang9;


            /////////
            var Thang10 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 10)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang10 = Thang10;

            ////////
            var Thang11 = db.CTPhieuNhan_DichVu
             .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 11)
             .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang11 = Thang11;


            /////////
            var Thang12 = db.CTPhieuNhan_DichVu
            .Where(dh => DbFunctions.TruncateTime(dh.ThoiGian).Value.Month == 12)
            .Sum(od => od.DonGia * od.SoLuong);
            ViewBag.thang12 = Thang12;
            ////////
            ///
            ///
            ///

            ///

            var Thang8hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 8)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang8hd = Thang8hd;
            ////////
            var Thang7hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 7)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang7hd = Thang7hd;
            //////

            var Thang1hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 1)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang1hd = Thang1hd;
            ////////
            var Thang2hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 2)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang2hd = Thang2hd;
            /////////
            var Thang3hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 3)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang3hd = Thang3hd;
            ///////////
            var Thang4hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 4)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang4hd = Thang4hd;
            ///////////
            var Thang5hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 5)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang5hd = Thang5hd;

            ////////
            var Thang6hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 6)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang6hd = Thang6hd;
            //////////

            var Thang9hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 9)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang9hd = Thang9hd;

            /////////

            var Thang10hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 10)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang10hd = Thang10hd;

            ////
            var Thang11hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 11)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang11hd = Thang11hd;

            /////
            var Thang12hd = db.CTDonHangs
            .Join(db.DonHangs, ct => ct.MaDH, dh => dh.MaDH, (ct, dh) => new { ct, dh })
            .Where(x => (x.dh.NgayGiao).Value.Month == 12)
            .Sum(x => x.ct.DonGia * x.ct.SoLuong);

            ViewBag.Thang12hd = Thang12hd;

            return View();
        }
    }
}