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

            ViewBag.MaSDDV = maSDDV;

            // Use SelectListItems to populate dropdown lists
            ViewBag.MaTC = new SelectList(db.ThuCungs, "MaTC", "TenTC");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhieuNhan phieuNhan, HttpPostedFileBase fileTinhTrangTruocTiepNhan, HttpPostedFileBase fileTinhTrangSauTiepNhan)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file ảnh TinhTrangTruocTiepNhan
                if (fileTinhTrangTruocTiepNhan != null && fileTinhTrangTruocTiepNhan.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileTinhTrangTruocTiepNhan.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/TinhTrangTruocTiepNhan/"), fileName);
                    fileTinhTrangTruocTiepNhan.SaveAs(path);
                    phieuNhan.TinhTrangTruocTiepNhan = fileName;
                }

                // Xử lý file ảnh TinhTrangSauTiepNhan
                if (fileTinhTrangSauTiepNhan != null && fileTinhTrangSauTiepNhan.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileTinhTrangSauTiepNhan.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/TinhTrangSauTiepNhan/"), fileName);
                    fileTinhTrangSauTiepNhan.SaveAs(path);
                    phieuNhan.TinhTrangSauTiepNhan = fileName;
                }

                db.PhieuNhans.Add(phieuNhan);
                db.SaveChanges();

                return RedirectToAction("PhieuCuaLuotSDDV", new { id = phieuNhan.MaSDDV });
            }

            ViewBag.MaSDDV = phieuNhan.MaSDDV;
            ViewBag.MaTC = new SelectList(db.ThuCungs, "MaTC", "TenTC", phieuNhan.MaTC);
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

            ViewBag.MaTC = new SelectList(db.ThuCungs, "MaTC", "TenTC", phieuNhan.MaTC);
            return View(phieuNhan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhieuNhan phieuNhan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuNhan).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("PhieuCuaLuotSDDV", new { id = phieuNhan.MaSDDV });
            }

            ViewBag.MaTC = new SelectList(db.ThuCungs, "MaTC", "TenTC", phieuNhan.MaTC);
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