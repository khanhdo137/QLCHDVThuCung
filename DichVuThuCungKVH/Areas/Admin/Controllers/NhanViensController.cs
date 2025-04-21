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
    public class NhanViensController : Controller
    {
        private readonly DACSEntities db = new DACSEntities();

        // GET: Admin/NhanViens
        public ActionResult Index()
        {
            var nhanViens = db.NhanViens.Include(n => n.TaiKhoan);
            return View(nhanViens.ToList());
        }

        // GET: Admin/NhanViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK");
            return View();
        }

        // POST: Admin/NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,TenNV,ChucVu,ChiNhanh,NgaySinh,SDT,DiaChi,GioiTinh,MaTK,Email")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", nhanVien.MaTK);
            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", nhanVien.MaTK);
            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,TenNV,ChucVu,ChiNhanh,NgaySinh,SDT,DiaChi,GioiTinh,MaTK,Email")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", nhanVien.MaTK);
            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult DonHangDaDat()
        {

            var danhSachDonHang = db.DonHangs.ToList();
            return View(danhSachDonHang);
        }
        public ActionResult SuaDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            return View(donHang);
        }

        [HttpPost]
        public ActionResult SuaDonHang(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin đơn hàng trong cơ sở dữ liệu
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DonHangDaDat");
            }

            return View(donHang);
        }
        public ActionResult XemChiTietDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            return View(donHang);
        }
        public ActionResult XoaDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            return View(donHang);
        }

        [HttpPost, ActionName("XoaDonHang")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();

            return RedirectToAction("DonHangDaDat");
        }
        public ActionResult DanhSachSuDungDichVu()
        {
            // Lấy danh sách phiếu nhận từ cơ sở dữ liệu
            var danhSachPhieuNhan = db.PhieuNhans.ToList();

            return View(danhSachPhieuNhan);
        }
        public ActionResult ChinhSuaPhieuNhan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy thông tin phiếu nhận từ cơ sở dữ liệu theo id
            PhieuNhan phieuNhan = db.PhieuNhans.Find(id);

            if (phieuNhan == null)
            {
                return HttpNotFound();
            }

            return View(phieuNhan);
        }

        [HttpPost]
        public ActionResult CapNhatPhieuNhan(PhieuNhan phieuNhan)
        {
            if (ModelState.IsValid)
            {
                // Xử lý cập nhật thông tin phiếu nhận trong cơ sở dữ liệu
                db.Entry(phieuNhan).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DanhSachSuDungDichVu");
            }

            // Nếu ModelState.IsValid là false, có thể in các lỗi ModelState để xem lý do
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(phieuNhan);
        }
        public ActionResult XoaPhieuNhan(int id)
        {
            try
            {
                var phieuNhan = db.PhieuNhans.Find(id);
                if (phieuNhan == null)
                {
                    return Json(new { success = false, message = "Phiếu nhận không tồn tại." });
                }

                db.PhieuNhans.Remove(phieuNhan);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}