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
    public class SuDungDichVusController : Controller
    {
        private DACSEntities db = new DACSEntities();

        // GET: Admin/SuDungDichVus
        public ActionResult Index()
        {
            var suDungDichVus = db.SuDungDichVus.Include(s => s.KhachHang);
            return View(suDungDichVus.ToList());
        }

        // GET: Admin/SuDungDichVus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuDungDichVu suDungDichVu = db.SuDungDichVus.Find(id);
            if (suDungDichVu == null)
            {
                return HttpNotFound();
            }
            return View(suDungDichVu);
        }

        // GET: Admin/SuDungDichVus/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");
            return View();
        }

        // POST: Admin/SuDungDichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSDDV,MaKH,Ngay,GhiChu")] SuDungDichVu suDungDichVu)
        {
            if (ModelState.IsValid)
            {
                db.SuDungDichVus.Add(suDungDichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", suDungDichVu.MaKH);
            return View(suDungDichVu);
        }

        // GET: Admin/SuDungDichVus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuDungDichVu suDungDichVu = db.SuDungDichVus.Find(id);
            if (suDungDichVu == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", suDungDichVu.MaKH);
            return View(suDungDichVu);
        }

        // POST: Admin/SuDungDichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSDDV,MaKH,Ngay,GhiChu")] SuDungDichVu suDungDichVu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suDungDichVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", suDungDichVu.MaKH);
            return View(suDungDichVu);
        }

        // GET: Admin/SuDungDichVus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuDungDichVu suDungDichVu = db.SuDungDichVus.Find(id);
            if (suDungDichVu == null)
            {
                return HttpNotFound();
            }
            return View(suDungDichVu);
        }

        // POST: Admin/SuDungDichVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SuDungDichVu suDungDichVu = db.SuDungDichVus.Find(id);
            db.SuDungDichVus.Remove(suDungDichVu);
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
