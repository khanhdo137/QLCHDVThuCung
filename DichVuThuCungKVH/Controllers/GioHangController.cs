using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DichVuThuCungKVH.Model;

namespace DichVuThuCungKVH.Controllers
{
    public class GioHangController : Controller
    {
        private DACSEntities db = new DACSEntities();
        // GET: GioHang

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
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
            //Lay gio hang hien tai
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaSanPham == ms);
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
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>; if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }

            return dTongTien;
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return PartialView();
        }

        public ActionResult XoaSPKhoiGioHang(int iMaSP)
        {
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSanPham == iMaSP);
            if (sp != null)
            {
                // Xoa sp
                lstGioHang.Remove(sp);

                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "DVTC");
                }
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSanPham == iMaSP); //Nếu tồn tại thì cho sửa số lượng
            if (sp != null)
            {
            }
            sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "DVTC");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiểm tra đăng nhập chưa
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            //Kiểm tra có hàng trong giỏ chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "GioHang");
            }
            List<GioHang> lstGioHang = LayGioHang();

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        public ActionResult DatHangPartial()
        {
            var maTaiKhoan = Convert.ToInt32(Session["MaTaiKhoan"].ToString());
            var kh = db.KhachHangs.SingleOrDefault(n => n.MaTK == maTaiKhoan);
            return PartialView(kh);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DonHang ddh = new DonHang();
            var maTaiKhoan = Convert.ToInt32(Session["MaTaiKhoan"].ToString());
            var kh = db.KhachHangs.SingleOrDefault(n => n.MaTK == maTaiKhoan);
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = f["NgayGiao"];
            ddh.NgayGiao = DateTime.Parse(NgayGiao);

            //ddh.TrangThaiThanhToan = 1;
            ddh.TrangThaiThanhToan = false;
            db.DonHangs.Add(ddh);
            //db.SaveChanges();
            // Thêm chi tiết đơn hàng
            foreach (var item in lstGioHang)
            {
                CTDonHang ctdh = new CTDonHang();
                ctdh.MaDH = ddh.MaDH;
                ctdh.MaSP = item.iMaSanPham;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CTDonHangs.Add(ctdh);
            }
            //db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}