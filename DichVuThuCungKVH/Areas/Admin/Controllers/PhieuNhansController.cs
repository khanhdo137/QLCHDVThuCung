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

        public ActionResult Create()
        {
            try
            {
                ViewBag.MaTC = new SelectList(db.ThuCungs, "MaTC", "TenTC");
                ViewBag.MaSDDV = new SelectList(db.SuDungDichVus, "MaSDDV", "MaSDDV");
                
                var nhanViens = db.NhanViens.ToList();
                if (nhanViens.Any())
                {
                    ViewBag.NhanVienList = new SelectList(nhanViens, "MaNV", "TenNV");
                }
                else
                {
                    ViewBag.NhanVienList = new SelectList(new List<NhanVien>(), "MaNV", "TenNV");
                }
                
                return View();
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Error in Create action: " + ex.Message);
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải danh sách nhân viên: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieu,MaLuotSDDV,TinhTrangTruocTiepNhan,TinhTrangSauTiepNhan,MaNhanVien")] PhieuNhan phieuNhan, HttpPostedFileBase TinhTrangTruocTiepNhan)
        {
            if (ModelState.IsValid)
            {
                if (TinhTrangTruocTiepNhan != null && TinhTrangTruocTiepNhan.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(TinhTrangTruocTiepNhan.FileName);
                    var uniqueFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{fileName}";
                    var uploadPath = "/Images/PhieuNhan";
                    var serverPath = Server.MapPath(uploadPath);
                    
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }
                    
                    var filePath = Path.Combine(serverPath, uniqueFileName);
                    TinhTrangTruocTiepNhan.SaveAs(filePath);
                    
                    phieuNhan.TinhTrangTruocTiepNhan = $"{uploadPath}/{uniqueFileName}";
                }

                db.PhieuNhans.Add(phieuNhan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLuotSDDV = new SelectList(db.LuotSuDungDichVus, "MaLuotSDDV", "MaLuotSDDV", phieuNhan.MaLuotSDDV);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "HoTen", phieuNhan.MaNhanVien);
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

            ViewBag.MaLuotSDDV = new SelectList(db.LuotSuDungDichVus, "MaLuotSDDV", "MaLuotSDDV", phieuNhan.MaLuotSDDV);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNV", "TenNV", phieuNhan.MaNhanVien);
            return View(phieuNhan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieu,MaLuotSDDV,TinhTrangTruocTiepNhan,TinhTrangSauTiepNhan,MaNhanVien")] PhieuNhan phieuNhan, HttpPostedFileBase TinhTrangTruocTiepNhan)
        {
            if (ModelState.IsValid)
            {
                if (TinhTrangTruocTiepNhan != null && TinhTrangTruocTiepNhan.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(TinhTrangTruocTiepNhan.FileName);
                    var uniqueFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{fileName}";
                    var uploadPath = "/Images/PhieuNhan";
                    var serverPath = Server.MapPath(uploadPath);
                    
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }
                    
                    var filePath = Path.Combine(serverPath, uniqueFileName);
                    TinhTrangTruocTiepNhan.SaveAs(filePath);
                    
                    phieuNhan.TinhTrangTruocTiepNhan = $"{uploadPath}/{uniqueFileName}";
                }

                db.Entry(phieuNhan).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("PhieuCuaLuotSDDV", new { id = phieuNhan.MaLuotSDDV });
            }

            ViewBag.MaLuotSDDV = new SelectList(db.LuotSuDungDichVus, "MaLuotSDDV", "MaLuotSDDV", phieuNhan.MaLuotSDDV);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNV", "TenNV", phieuNhan.MaNhanVien);
            return View(phieuNhan);
        }

        public ActionResult Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuNhan phieuNhan = db.PhieuNhans.Find(id);
            db.PhieuNhans.Remove(phieuNhan);
            db.SaveChanges();

            return RedirectToAction("PhieuCuaLuotSDDV", new { id = phieuNhan.MaSDDV });
        }

        public ActionResult PhieuCuaLuotSDDV(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.MaSDDV = id;

            var phieuNhans = db.PhieuNhans.Where(pn => pn.MaSDDV == id).ToList();

            return View(phieuNhans);
        }
    }
}