using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DichVuThuCungKVH.Model;

namespace DichVuThuCungKVH.Controllers
{
    public class GioHangController : Controller
    {
        private DACSEntities db = new DACSEntities();

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();

            if (!lstGioHang.Any())
            {
                ViewBag.GioHangTrong = "Giỏ hàng bạn đang trống!";
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSanPham == ms);

            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }

            return Redirect(url);
        }

        private int TongSoLuong()
        {
            var lstGioHang = Session["GioHang"] as List<GioHang>;
            return lstGioHang?.Sum(n => n.iSoLuong) ?? 0;
        }

        private double TongTien()
        {
            var lstGioHang = Session["GioHang"] as List<GioHang>;
            return lstGioHang?.Sum(n => n.dThanhTien) ?? 0;
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaSPKhoiGioHang(int iMaSP)
        {
            var lstGioHang = LayGioHang();
            var sp = lstGioHang.SingleOrDefault(n => n.iMaSanPham == iMaSP);

            if (sp != null)
            {
                lstGioHang.Remove(sp);
            }

            if (!lstGioHang.Any())
            {
                return RedirectToAction("Index", "DVTC");
            }

            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            var lstGioHang = LayGioHang();
            var sp = lstGioHang.SingleOrDefault(n => n.iMaSanPham == iMaSP);

            if (sp != null && int.TryParse(f["txtSoLuong"], out int soLuong))
            {
                sp.iSoLuong = soLuong;
            }

            return RedirectToAction("GioHang");
        }

        public ActionResult XoaGioHang()
        {
            LayGioHang().Clear();
            return RedirectToAction("Index", "DVTC");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            var lstGioHang = LayGioHang();
            if (!lstGioHang.Any())
            {
                return RedirectToAction("Index", "DVTC");
            }

            return View(lstGioHang); // truyền danh sách giỏ hàng vào View
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            try
            {
                // Kiểm tra đăng nhập
                if (Session["MaTaiKhoan"] == null)
                    return RedirectToAction("DangNhap", "User");

                var lstGioHang = LayGioHang();
                if (!lstGioHang.Any())
                    return RedirectToAction("Index", "GioHang");

                int maTaiKhoan = Convert.ToInt32(Session["MaTaiKhoan"]);
                var kh = db.KhachHangs.SingleOrDefault(n => n.MaTK == maTaiKhoan);
                if (kh == null)
                    return RedirectToAction("DangNhap", "User");

                // Xử lý ngày giao
                if (!DateTime.TryParse(f["NgayGiao"], out DateTime ngayGiao))
                {
                    ModelState.AddModelError("NgayGiao", "Ngày giao không hợp lệ");
                    return View("DatHang", lstGioHang);
                }

                // Tạo đơn hàng
                var ddh = new DonHang
                {
                    MaKH = kh.MaKH,
                    NgayDat = DateTime.Now,
                    NgayGiao = ngayGiao,
                    TrangThaiThanhToan = false
                };
                db.DonHangs.Add(ddh);
                db.SaveChanges(); // Để lấy MaDH

                // Thêm chi tiết đơn hàng
                foreach (var item in lstGioHang)
                {
                    var ctdh = new CTDonHang
                    {
                        MaDH = ddh.MaDH,
                        MaSP = item.iMaSanPham,
                        SoLuong = item.iSoLuong,
                        DonGia = (decimal)item.dDonGia
                    };
                    db.CTDonHangs.Add(ctdh);
                }

                db.SaveChanges();

                // Xoá giỏ hàng
                Session["GioHang"] = null;

                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            catch (Exception ex)
            {
                ViewBag.Loi = "Đặt hàng thất bại: " + ex.Message;
                return View("DatHang", LayGioHang());
            }
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}
