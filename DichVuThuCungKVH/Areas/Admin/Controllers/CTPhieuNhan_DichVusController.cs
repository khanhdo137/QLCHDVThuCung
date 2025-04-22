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
    public class CTPhieuNhan_DichVusController : Controller
    {
        private QLCHDVThuCungEntities db = new QLCHDVThuCungEntities();

        // GET: Admin/CTPhieuNhan_DichVus/Create
        public ActionResult Create(int? maPhieu)
        {
            if (maPhieu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var phieuNhan = db.PhieuNhans.Find(maPhieu);
            if (phieuNhan == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaPhieu = maPhieu;
            ViewBag.ThongTinPhieu = phieuNhan;
            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDV");
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "TenNV");

            return View();
        }

        // POST: Admin/CTPhieuNhan_DichVus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieu,MaDV,MaNV,ThoiGian,SoLuong,Gia")] CT_PhieuNhan_DichVu cT_PhieuNhan_DichVu)
        {
            if (ModelState.IsValid)
            {
                db.CT_PhieuNhan_DichVu.Add(cT_PhieuNhan_DichVu);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Thêm dịch vụ thành công!";
                return RedirectToAction("PhieuCuaLuotSDDV", "PhieuNhans", new { id = cT_PhieuNhan_DichVu.PhieuNhan.MaSDDV });
            }

            ViewBag.MaPhieu = cT_PhieuNhan_DichVu.MaPhieu;
            ViewBag.ThongTinPhieu = db.PhieuNhans.Find(cT_PhieuNhan_DichVu.MaPhieu);
            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDV", cT_PhieuNhan_DichVu.MaDV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "TenNV", cT_PhieuNhan_DichVu.MaNV);
            return View(cT_PhieuNhan_DichVu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}