using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DichVuThuCungKVH.Model;

namespace DichVuThuCungKVH.Areas.Admin.Controllers
{
    public class PhieuNhansController : Controller
    {
        private readonly DACSEntities db = new DACSEntities();

        // GET: Admin/PhieuNhans
        public ActionResult Index()
        {
            var phieuNhans = db.PhieuNhans.Include(p => p.SuDungDichVu).Include(p => p.ThuCung);
            return View(phieuNhans.ToList());
        }

        public ActionResult Create(int? maSDDV)
        {
            if (maSDDV == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy thông tin lượt sử dụng dịch vụ
            var suDungDichVu = db.SuDungDichVus.Find(maSDDV);
            if (suDungDichVu == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách thú cưng của khách hàng
            var thuCungs = db.ThuCungs.Where(t => t.MaKH == suDungDichVu.MaKH).ToList();

            // Lấy danh sách nhân viên
            var nhanViens = db.NhanViens.ToList();

            ViewBag.MaTC = new SelectList(thuCungs, "MaTC", "TenTC");
            ViewBag.MaSDDV = maSDDV;
            ViewBag.NguoiGiao = new SelectList(nhanViens, "MaNV", "TenNV");
            ViewBag.NguoiNhan = new SelectList(nhanViens, "MaNV", "TenNV");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTC,MaSDDV,TinhTrangTruocTiepNhan,TinhTrangSauTiepNhan,NguoiGiao,NguoiNhan,NgayNhan,TinhTrangDichVu,NgayTra,NguoiTra,GhiChu")] PhieuNhan phieuNhan, HttpPostedFileBase fileTruoc, HttpPostedFileBase fileSau)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý upload ảnh tình trạng trước tiếp nhận
                    if (fileTruoc != null && fileTruoc.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fileTruoc.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/TinhTrangTruocTiepNhan"), fileName);
                        fileTruoc.SaveAs(path);
                        phieuNhan.TinhTrangTruocTiepNhan = fileName;
                    }

                    // Xử lý upload ảnh tình trạng sau tiếp nhận
                    if (fileSau != null && fileSau.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fileSau.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/TinhTrangSauTiepNhan"), fileName);
                        fileSau.SaveAs(path);
                        phieuNhan.TinhTrangSauTiepNhan = fileName;
                    }

                    // Set default values
                    phieuNhan.NgayNhan = DateTime.Now;
                    phieuNhan.TinhTrangDichVu = "Chưa hoàn thành";

                    db.PhieuNhans.Add(phieuNhan);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Tạo phiếu nhận thành công!";
                    return RedirectToAction("PhieuCuaLuotSDDV", new { id = phieuNhan.MaSDDV });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo phiếu nhận: " + ex.Message);
                }
            }

            // Lấy lại dữ liệu cho dropdown
            var suDungDichVu = db.SuDungDichVus.Find(phieuNhan.MaSDDV);
            var thuCungs = db.ThuCungs.Where(t => t.MaKH == suDungDichVu.MaKH).ToList();
            var nhanViens = db.NhanViens.ToList();

            ViewBag.MaTC = new SelectList(thuCungs, "MaTC", "TenTC", phieuNhan.MaTC);
            ViewBag.MaSDDV = phieuNhan.MaSDDV;
            ViewBag.NguoiGiao = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiGiao);
            ViewBag.NguoiNhan = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiNhan);

            return View(phieuNhan);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PhieuNhan phieuNhan = db.PhieuNhans.Find(id);
            if (phieuNhan == null)
            {
                return HttpNotFound();
            }

            return View(phieuNhan);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PhieuNhan phieuNhan = db.PhieuNhans.Find(id);
            if (phieuNhan == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách thú cưng của khách hàng
            var suDungDichVu = db.SuDungDichVus.Find(phieuNhan.MaSDDV);
            var thuCungs = db.ThuCungs.Where(t => t.MaKH == suDungDichVu.MaKH).ToList();

            // Lấy danh sách nhân viên
            var nhanViens = db.NhanViens.ToList();

            ViewBag.MaTC = new SelectList(thuCungs, "MaTC", "TenTC", phieuNhan.MaTC);
            ViewBag.NguoiGiao = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiGiao);
            ViewBag.NguoiNhan = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiNhan);
            ViewBag.NguoiTra = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiTra);
            
            return View(phieuNhan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhieuNhan phieuNhan, HttpPostedFileBase fileTruoc, HttpPostedFileBase fileSau)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin phiếu nhận hiện tại từ cơ sở dữ liệu
                    var phieuNhanHienTai = db.PhieuNhans.Find(phieuNhan.MaPhieu);
                    if (phieuNhanHienTai == null)
                    {
                        return HttpNotFound();
                    }

                    // Cập nhật thông tin từ form vào đối tượng hiện tại
                    phieuNhanHienTai.MaTC = phieuNhan.MaTC;
                    phieuNhanHienTai.MaSDDV = phieuNhan.MaSDDV;
                    phieuNhanHienTai.NguoiGiao = phieuNhan.NguoiGiao;
                    phieuNhanHienTai.NguoiNhan = phieuNhan.NguoiNhan;
                    phieuNhanHienTai.NgayNhan = phieuNhan.NgayNhan;
                    phieuNhanHienTai.TinhTrangDichVu = phieuNhan.TinhTrangDichVu;
                    phieuNhanHienTai.NgayTra = phieuNhan.NgayTra;
                    phieuNhanHienTai.NguoiTra = phieuNhan.NguoiTra;
                    phieuNhanHienTai.GhiChu = phieuNhan.GhiChu;

                    // Xử lý upload ảnh tình trạng trước tiếp nhận
                    if (fileTruoc != null && fileTruoc.ContentLength > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(phieuNhanHienTai.TinhTrangTruocTiepNhan))
                        {
                            string oldPath = Path.Combine(Server.MapPath("~/Images/TinhTrangTruocTiepNhan"), phieuNhanHienTai.TinhTrangTruocTiepNhan);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        // Lưu ảnh mới
                        string fileName = Path.GetFileName(fileTruoc.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/TinhTrangTruocTiepNhan"), fileName);
                        fileTruoc.SaveAs(path);
                        phieuNhanHienTai.TinhTrangTruocTiepNhan = fileName;
                    }

                    // Xử lý upload ảnh tình trạng sau tiếp nhận
                    if (fileSau != null && fileSau.ContentLength > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(phieuNhanHienTai.TinhTrangSauTiepNhan))
                        {
                            string oldPath = Path.Combine(Server.MapPath("~/Images/TinhTrangSauTiepNhan"), phieuNhanHienTai.TinhTrangSauTiepNhan);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        // Lưu ảnh mới
                        string fileName = Path.GetFileName(fileSau.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/TinhTrangSauTiepNhan"), fileName);
                        fileSau.SaveAs(path);
                        phieuNhanHienTai.TinhTrangSauTiepNhan = fileName;
                    }

                    db.Entry(phieuNhanHienTai).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật phiếu nhận thành công!";
                    return RedirectToAction("PhieuCuaLuotSDDV", new { id = phieuNhanHienTai.MaSDDV });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật phiếu nhận: " + ex.Message);
                }
            }

            // Lấy danh sách thú cưng của khách hàng
            var suDungDichVu = db.SuDungDichVus.Find(phieuNhan.MaSDDV);
            var thuCungs = db.ThuCungs.Where(t => t.MaKH == suDungDichVu.MaKH).ToList();

            // Lấy danh sách nhân viên
            var nhanViens = db.NhanViens.ToList();

            ViewBag.MaTC = new SelectList(thuCungs, "MaTC", "TenTC", phieuNhan.MaTC);
            ViewBag.NguoiGiao = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiGiao);
            ViewBag.NguoiNhan = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiNhan);
            ViewBag.NguoiTra = new SelectList(nhanViens, "MaNV", "TenNV", phieuNhan.NguoiTra);
            
            return View(phieuNhan);
        }

        public ActionResult Delete(int? id, int? maSDDV)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PhieuNhan phieuNhan = db.PhieuNhans.Find(id);
            if (phieuNhan == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaSDDV = maSDDV;
            return View(phieuNhan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? maSDDV)
        {
            try
            {
                PhieuNhan phieuNhan = db.PhieuNhans.Find(id);
                if (phieuNhan == null)
                {
                    return HttpNotFound();
                }

                // Xóa ảnh tình trạng trước tiếp nhận
                if (!string.IsNullOrEmpty(phieuNhan.TinhTrangTruocTiepNhan))
                {
                    string pathTruoc = Path.Combine(Server.MapPath("~/Images/TinhTrangTruocTiepNhan"), phieuNhan.TinhTrangTruocTiepNhan);
                    if (System.IO.File.Exists(pathTruoc))
                    {
                        System.IO.File.Delete(pathTruoc);
                    }
                }

                // Xóa ảnh tình trạng sau tiếp nhận
                if (!string.IsNullOrEmpty(phieuNhan.TinhTrangSauTiepNhan))
                {
                    string pathSau = Path.Combine(Server.MapPath("~/Images/TinhTrangSauTiepNhan"), phieuNhan.TinhTrangSauTiepNhan);
                    if (System.IO.File.Exists(pathSau))
                    {
                        System.IO.File.Delete(pathSau);
                    }
                }

                // Xóa các chi tiết phiếu nhận dịch vụ liên quan
                var ctPhieuNhans = db.CTPhieuNhan_DichVu.Where(ct => ct.MaPhieu == id).ToList();
                foreach (var ct in ctPhieuNhans)
                {
                    db.CTPhieuNhan_DichVu.Remove(ct);
                }

                // Xóa phiếu nhận
                db.PhieuNhans.Remove(phieuNhan);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Xóa phiếu nhận thành công!";
                return RedirectToAction("PhieuCuaLuotSDDV", new { id = maSDDV ?? phieuNhan.MaSDDV });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa phiếu nhận: " + ex.Message;
                return RedirectToAction("PhieuCuaLuotSDDV", new { id = maSDDV });
            }
        }

        public ActionResult PhieuCuaLuotSDDV(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.MaSDDV = id;

            // Lấy danh sách phiếu nhận và thông tin nhân viên
            var phieuNhans = db.PhieuNhans
                .Include(p => p.SuDungDichVu)
                .Include(p => p.ThuCung)
                .Where(pn => pn.MaSDDV == id)
                .ToList();

            // Lấy thông tin nhân viên
            var nhanViens = db.NhanViens.ToDictionary(nv => nv.MaNV.ToString(), nv => nv.TenNV);
            ViewBag.NhanViens = nhanViens;

            return View(phieuNhans);
        }
    }
}