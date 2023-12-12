using DichVuThuCungKVH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Controllers
{
    public class DichVuController : Controller
    {
        private DACSEntities db = new DACSEntities();

        // GET: DichVu
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DVTrongGiu()
        {
            ViewBag.MaDV = new SelectList(db.DichVus.ToList().OrderBy(n => n.TenDichVu), "MaDV", "TenDV");
            return View();
        }

        [HttpPost]
        public ActionResult DvTrongGiu(FormCollection collection, KhachHang kh, CTPhieuNhan_DichVu ct)
        {
            ViewBag.MaDV = new SelectList(db.DichVus.ToList().OrderBy(n => n.TenDichVu), "MaDV", "TenDV");

            // ViewBag.ThongBao = "Không được để trống";
            ViewBag.TenKH = collection["sTenKH"];
            ViewBag.SDT = collection["sSDT"];
            ViewBag.Email = collection["sEmail"];
            ViewBag.MaDV = new SelectList(db.DichVus.ToList().OrderBy(n => n.TenDichVu), "MaDV", "TenDV", int.Parse(collection["MaDV"]));

            //      var sHoTen = collection["HoTen"];
            //    var sDienThoai = collection["DienThoai"];
            //  var sEmail = collection["Email"];
            //var sYeuCau = db.DichVus.ToList();
            //var selectList = new SelectList(sYeuCau, "MaDV", "TenDV");
            //ViewBag.MaDV = selectList;
            //var sYeuCau = collection["YeuCau"];
            //     var selectList = new SelectList(db.DichVus.ToList().OrderBy(n => n.TenDichVu), "MaDV", "TenDV");
            // ViewBag.YeuCau = selectList;
            //ViewBag.MaDV = new SelectList(db.DichVus.ToList().OrderBy(n => n.TenDichVu), "MaDV", "TenDV", int.Parse(collection["MaDV"]));
            /*       if (String.IsNullOrEmpty(sHoTen))
                   {
                       ViewData["err1"] = "Họ tên không được rỗng";
                   }
                   else if (String.IsNullOrEmpty(sDienThoai))
                   {
                       ViewData["err2"] = "Điện thoại không được rỗng";
                   }
                   else if (String.IsNullOrEmpty(sEmail))
                   {
                       ViewData["err3"] = "Email không được rỗng";
                   }
                   else if (String.IsNullOrEmpty(s))
                   {
                    ViewData["err4"] = "Yêu cầu không được rỗng";
                   }
                   else
                   {
                       KhachHang kh = new KhachHang
                       {
                           TenKH = sHoTen,
                           SDT = sDienThoai,
                           Email = sEmail,
                       };
                       CTPhieuNhan_DichVu ct = new CTPhieuNhan_DichVu
                       {
                          DichVuSuDung = ,
                       };
            */
            kh.TenKH = collection["sTenKH"];
            kh.SDT = collection["sDienThoai"];
            kh.Email = collection["sEmail"];
            ct.DichVuSuDung = (collection["TenDV"]);
            // ViewBag.MaDV = new SelectList(db.DichVus.ToList().OrderBy(n => n.TenDichVu), "MaDV", "TenDV", int.Parse(collection["MaDV"]));
            db.KhachHangs.Add(kh);
            db.CTPhieuNhan_DichVu.Add(ct);
            db.SaveChanges();
            return RedirectToAction("DVTrongGiu");

            // return View();
        }

        [HttpGet]
        public ActionResult DangKyDichVu()
        {
            // Populate dropdowns or perform any other necessary setup here
            return PartialView();
        }

        [HttpPost]
        public ActionResult DangKyDichVu(KhachHang khachHang)
        {
            // Validate and save the registration data to the database
            khachHang.TinhTrangLienHe = false;
            if (ModelState.IsValid)
            {
                // Save to the database
                var existingTaiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.Email == khachHang.Email);

                if (existingTaiKhoan != null)
                {
                    // If a matching TaiKhoan is found, link it to the KhachHang
                    khachHang.MaTK = existingTaiKhoan.MaTK;
                }
                khachHang.ThoiGianDangKy = DateTime.Now;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();

                // Redirect to a success page or another action
                return RedirectToAction("DichVu","DVTC");
            }

            // If validation fails, return to the registration form with errors
            return View(khachHang);
        }
    }
}