using DichVuThuCungKVH.Model;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Areas.Admin.Controllers
{
    public class KhachHangsController : Controller
    {
        private readonly DACSEntities db = new DACSEntities();

        // GET: Admin/KhachHangs
        public ActionResult Index()
        {
            var khachHangs = db.KhachHangs.Include(k => k.TaiKhoan);
            return View(khachHangs.ToList());
        }

        // GET: Admin/KhachHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK");
            return View();
        }

        // POST: Admin/KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,NgaySinh,SDT,DiaChi,GioiTinh,Email,MaTK,ThoiGianDangKy,TinhTrangLienHe,GhiChu")] KhachHang khachHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra các trường bắt buộc
                    if (string.IsNullOrEmpty(khachHang.TenKH))
                    {
                        ModelState.AddModelError("TenKH", "Tên khách hàng không được để trống");
                    }
                    if (string.IsNullOrEmpty(khachHang.SDT))
                    {
                        ModelState.AddModelError("SDT", "Số điện thoại không được để trống");
                    }
                    if (string.IsNullOrEmpty(khachHang.DiaChi))
                    {
                        ModelState.AddModelError("DiaChi", "Địa chỉ không được để trống");
                    }

                    if (ModelState.IsValid)
                    {
                        // Thiết lập giá trị mặc định cho các trường không bắt buộc
                        if (khachHang.ThoiGianDangKy == null)
                        {
                            khachHang.ThoiGianDangKy = DateTime.Now;
                        }
                        if (khachHang.TinhTrangLienHe == null)
                        {
                            khachHang.TinhTrangLienHe = true; // true = "Đang hoạt động"
                        }

                        db.KhachHangs.Add(khachHang);
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm khách hàng: " + ex.Message);
            }

            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", khachHang.MaTK);
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", khachHang.MaTK);
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,TenKH,NgaySinh,SDT,DiaChi,GioiTinh,Email,MaTK,ThoiGianDangKy,TinhTrangLienHe,GhiChu")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", khachHang.MaTK);
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // 1. Kiểm tra khách hàng tồn tại
                var khachHang = db.KhachHangs.Find(id);
                if (khachHang == null)
                {
                    return HttpNotFound();
                }

                // 2. Xóa tất cả chi tiết đơn hàng liên quan
                var donHangs = db.DonHangs.Where(d => d.MaKH == id).ToList();
                foreach (var donHang in donHangs)
                {
                    var ctDonHangs = db.CTDonHangs.Where(ct => ct.MaDH == donHang.MaDH).ToList();
                    foreach (var ctDonHang in ctDonHangs)
                    {
                        db.CTDonHangs.Remove(ctDonHang);
                    }
                }
                db.SaveChanges();

                // 3. Xóa tất cả đơn hàng
                foreach (var donHang in donHangs)
                {
                    db.DonHangs.Remove(donHang);
                }
                db.SaveChanges();

                // 4. Xóa tất cả chi tiết phiếu nhận dịch vụ liên quan
                var thuCungs = db.ThuCungs.Where(t => t.MaKH == id).ToList();
                foreach (var thuCung in thuCungs)
                {
                    var phieuNhans = db.PhieuNhans.Where(p => p.MaTC == thuCung.MaTC).ToList();
                    foreach (var phieuNhan in phieuNhans)
                    {
                        var ctPhieuNhans = db.CTPhieuNhan_DichVu.Where(ct => ct.MaPhieu == phieuNhan.MaPhieu).ToList();
                        foreach (var ctPhieuNhan in ctPhieuNhans)
                        {
                            db.CTPhieuNhan_DichVu.Remove(ctPhieuNhan);
                        }
                    }
                }
                db.SaveChanges();

                // 5. Xóa tất cả phiếu nhận liên quan
                foreach (var thuCung in thuCungs)
                {
                    var phieuNhans = db.PhieuNhans.Where(p => p.MaTC == thuCung.MaTC).ToList();
                    foreach (var phieuNhan in phieuNhans)
                    {
                        db.PhieuNhans.Remove(phieuNhan);
                    }
                }
                db.SaveChanges();

                // 6. Xóa tất cả thú cưng
                foreach (var thuCung in thuCungs)
                {
                    db.ThuCungs.Remove(thuCung);
                }
                db.SaveChanges();

                // 7. Xóa tất cả lượt sử dụng dịch vụ
                var suDungDichVus = db.SuDungDichVus.Where(s => s.MaKH == id).ToList();
                foreach (var suDungDichVu in suDungDichVus)
                {
                    db.SuDungDichVus.Remove(suDungDichVu);
                }
                db.SaveChanges();

                // 8. Cuối cùng xóa khách hàng
                db.KhachHangs.Remove(khachHang);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã xóa khách hàng thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi khi xóa khách hàng: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                // Kiểm tra xem có phải là lỗi ràng buộc khóa ngoại không
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    TempData["ErrorMessage"] = "Không thể xóa khách hàng vì vẫn còn dữ liệu liên quan. Vui lòng kiểm tra lại các đơn hàng, thú cưng và dịch vụ của khách hàng.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa khách hàng. Vui lòng thử lại sau.";
                }

                return RedirectToAction("Delete", new { id = id });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ThuCung(int id)
        {
            // Lấy thông tin khách hàng và thú cưng của khách hàng
            var khachHang = db.KhachHangs.Include(kh => kh.ThuCungs).SingleOrDefault(kh => kh.MaKH == id);

            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }
    }
}