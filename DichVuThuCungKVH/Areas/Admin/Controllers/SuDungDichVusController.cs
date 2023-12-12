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
        private readonly DACSEntities db = new DACSEntities();

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
        public ActionResult Create(int? maKH)
        {
            if (maKH == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = maKH;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SuDungDichVu suDungDichVu, int maKH)
        {
            if (ModelState.IsValid)
            {
                // Set mã khách hàng cho lượt sử dụng dịch vụ
                suDungDichVu.MaKH = maKH;

                db.SuDungDichVus.Add(suDungDichVu);
                db.SaveChanges();
                return RedirectToAction("LuotSDDV", new { id = maKH });
            }

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
            return View(suDungDichVu);
        }

        // POST: Admin/SuDungDichVus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SuDungDichVu suDungDichVu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suDungDichVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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

        public ActionResult LuotSDDV(int? id)
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

            ViewBag.MaKH = id;

            var LuotSDDV = db.SuDungDichVus.Where(dv => dv.MaKH == id).ToList();
            return View(LuotSDDV);
        }
    }
}