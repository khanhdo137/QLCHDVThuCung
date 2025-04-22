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
    public class CTPhieuNhan_DichVu1Controller : Controller
    {
        private DACSEntities db = new DACSEntities();

        // GET: Admin/CTPhieuNhan_DichVu1
        public ActionResult Index()
        {
            var cTPhieuNhan_DichVu = db.CTPhieuNhan_DichVu.Include(c => c.DichVu).Include(c => c.PhieuNhan);
            return View(cTPhieuNhan_DichVu.ToList());
        }

        // GET: Admin/CTPhieuNhan_DichVu1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTPhieuNhan_DichVu cTPhieuNhan_DichVu = db.CTPhieuNhan_DichVu.Find(id);
            if (cTPhieuNhan_DichVu == null)
            {
                return HttpNotFound();
            }
            return View(cTPhieuNhan_DichVu);
        }

        // GET: Admin/CTPhieuNhan_DichVu1/Create
        public ActionResult Create()
        {
            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDichVu");
            ViewBag.MaPhieu = new SelectList(db.PhieuNhans, "MaPhieu", "TinhTrangTruocTiepNhan");
            return View();
        }

        // POST: Admin/CTPhieuNhan_DichVu1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCT,MaPhieu,MaDV,DichVuSuDung,NguoiThucHien,ThoiGian,SoLuong,DonGia,GhiChu")] CTPhieuNhan_DichVu cTPhieuNhan_DichVu)
        {
            if (ModelState.IsValid)
            {
                db.CTPhieuNhan_DichVu.Add(cTPhieuNhan_DichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDichVu", cTPhieuNhan_DichVu.MaDV);
            ViewBag.MaPhieu = new SelectList(db.PhieuNhans, "MaPhieu", "TinhTrangTruocTiepNhan", cTPhieuNhan_DichVu.MaPhieu);
            return View(cTPhieuNhan_DichVu);
        }

        // GET: Admin/CTPhieuNhan_DichVu1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTPhieuNhan_DichVu cTPhieuNhan_DichVu = db.CTPhieuNhan_DichVu.Find(id);
            if (cTPhieuNhan_DichVu == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDichVu", cTPhieuNhan_DichVu.MaDV);
            ViewBag.MaPhieu = new SelectList(db.PhieuNhans, "MaPhieu", "TinhTrangTruocTiepNhan", cTPhieuNhan_DichVu.MaPhieu);
            return View(cTPhieuNhan_DichVu);
        }

        // POST: Admin/CTPhieuNhan_DichVu1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCT,MaPhieu,MaDV,DichVuSuDung,NguoiThucHien,ThoiGian,SoLuong,DonGia,GhiChu")] CTPhieuNhan_DichVu cTPhieuNhan_DichVu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTPhieuNhan_DichVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDV = new SelectList(db.DichVus, "MaDV", "TenDichVu", cTPhieuNhan_DichVu.MaDV);
            ViewBag.MaPhieu = new SelectList(db.PhieuNhans, "MaPhieu", "TinhTrangTruocTiepNhan", cTPhieuNhan_DichVu.MaPhieu);
            return View(cTPhieuNhan_DichVu);
        }

        // GET: Admin/CTPhieuNhan_DichVu1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTPhieuNhan_DichVu cTPhieuNhan_DichVu = db.CTPhieuNhan_DichVu.Find(id);
            if (cTPhieuNhan_DichVu == null)
            {
                return HttpNotFound();
            }
            return View(cTPhieuNhan_DichVu);
        }

        // POST: Admin/CTPhieuNhan_DichVu1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CTPhieuNhan_DichVu cTPhieuNhan_DichVu = db.CTPhieuNhan_DichVu.Find(id);
            db.CTPhieuNhan_DichVu.Remove(cTPhieuNhan_DichVu);
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
    }
}
