using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DichVuThuCungKVH.Model;

namespace DichVuThuCungKVH.Areas.Admin.Controllers
{
    public class CTPhieuNhan_DichVuController : Controller
    {
        private DACSEntities db = new DACSEntities();

        public ActionResult ChiTietPhieu(int maPhieu)
        {
            var chiTietPhieuList = db.CTPhieuNhan_DichVu.Where(ct => ct.MaPhieu == maPhieu).ToList();
            ViewBag.MaPhieu = maPhieu;
            return View(chiTietPhieuList);
        }

        public ActionResult Create(int maPhieu)
        {
            ViewBag.MaPhieu = maPhieu;
            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDichVu");
            ViewBag.NguoiThucHienList = new SelectList(db.NhanViens, "MaNV", "TenNV");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCT,MaPhieu,MaDV,DichVuSuDung,NguoiThucHien,ThoiGian,SoLuong,DonGia,GhiChu")] CTPhieuNhan_DichVu cTPhieuNhan_DichVu)
        {
            if (ModelState.IsValid)
            {
                // Lấy tên của Dịch vụ dựa trên MaDV
                cTPhieuNhan_DichVu.DichVuSuDung = db.DichVus
                                                    .Where(dv => dv.MaDV == cTPhieuNhan_DichVu.MaDV)
                                                    .Select(dv => dv.TenDichVu)
                                                    .FirstOrDefault();

                // Tiếp tục lưu dữ liệu vào CSDL
                db.CTPhieuNhan_DichVu.Add(cTPhieuNhan_DichVu);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDichVu", cTPhieuNhan_DichVu.MaDV);
            ViewBag.MaPhieu = new SelectList(db.PhieuNhans, "MaPhieu", "TinhTrangTruocTiepNhan", cTPhieuNhan_DichVu.MaPhieu);

            // Thêm SelectList cho danh sách người thực hiện
            ViewBag.NguoiThucHienList = new SelectList(db.NhanViens, "MaNV", "TenNV");

            return View(cTPhieuNhan_DichVu);
        }

        public JsonResult GetTenDichVu(int maDV)
        {
            var tenDichVu = db.DichVus.Where(dv => dv.MaDV == maDV).Select(dv => dv.TenDichVu).FirstOrDefault();
            return Json(tenDichVu, JsonRequestBehavior.AllowGet);
        }
    }
}