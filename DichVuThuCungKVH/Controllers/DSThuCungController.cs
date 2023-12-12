using DichVuThuCungKVH.Model;
using DichVuThuCungKVH.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Controllers
{
    public class DSThuCungController : Controller
    {
        private DACSEntities db = new DACSEntities();

        // GET: DSThuCung
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
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            var maTaiKhoan = Convert.ToInt32(Session["MaTaiKhoan"].ToString());
            var maKhachHang = Convert.ToInt32(Session["MaKhachHang"].ToString());
            var kh = db.KhachHangs.SingleOrDefault(n => n.MaTK == maTaiKhoan);
            var ds = db.KhachHangs.SingleOrDefault(n => n.MaKH == maKhachHang );
            return View(ds);
        }

    }
}