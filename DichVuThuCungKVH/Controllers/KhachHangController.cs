using DichVuThuCungKVH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang

        private LTWEntities db = new LTWEntities();
        public ActionResult KhachHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            var maTaiKhoan = Convert.ToInt32(Session["MaTaiKhoan"].ToString());
            var kh = db.KhachHangs.SingleOrDefault(n => n.MaTK == maTaiKhoan);
            return View(kh);

        }
        public ActionResult DonHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            var maTaiKhoan = Convert.ToInt32(Session["MaTaiKhoan"].ToString());
            var dh = db.CTDonHangs.SingleOrDefault(n => n.MaDH == maTaiKhoan);
            return View(dh);
        }
        public ActionResult DanhSachThuCung()
        {
            if (Session["TaiKhoan"] == null || string.IsNullOrEmpty(Session["TaiKhoan"].ToString()))
            {
                return RedirectToAction("DangNhap", "User");
            }

            var maTaiKhoanObject = Session["MaTaiKhoan"];
            if (maTaiKhoanObject != null && int.TryParse(maTaiKhoanObject.ToString(), out var maTaiKhoan))
            {
                var khachHang = db.KhachHangs.SingleOrDefault(n => n.MaTK == maTaiKhoan);

                if (khachHang != null)
                {
                    var maKhachHang = khachHang.MaKH;

                    var danhSachThuCung = db.ThuCungs.Where(n => n.MaKH == maKhachHang).ToList();

                    ViewBag.MaKhachHang = maKhachHang;

                    ViewBag.DanhSachThuCung = danhSachThuCung;

                    return View();
                }
                else
                {
                    return RedirectToAction("ThongBaoKhongTimThayKhachHang", "User");
                }
            }
            else
            {
                return RedirectToAction("DangNhap", "User");
            }
        }

    }
}