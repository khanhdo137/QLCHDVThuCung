using DichVuThuCungKVH.Model;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Areas.Admin.Controllers
{
    public class ThuCungsController : Controller
    {
        private readonly DACSEntities db = new DACSEntities();

        // GET: Admin/ThuCungs/Create
        public ActionResult Create(int? maKH)
        {
            if (maKH == null)
            {
                // Nếu maKH là null, có thể thực hiện xử lý nếu cần
                // Ví dụ: Redirect hoặc hiển thị thông báo lỗi
                return RedirectToAction("Error", "Home");
            }

            ViewBag.MaKH = maKH.Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThuCung thuCung, HttpPostedFileBase PetFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (PetFile != null && PetFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(PetFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/ThuCung"), fileName);

                        if (!Directory.Exists(Server.MapPath("~/Images/ThuCung")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Images/ThuCung"));
                        }

                        PetFile.SaveAs(path);
                        thuCung.HinhAnh = fileName;
                    }

                    db.ThuCungs.Add(thuCung);
                    db.SaveChanges();

                    return RedirectToAction("Index", "KhachHangs");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Lỗi: " + ex.Message;
                }
            }

            return View(thuCung);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ThuCung thuCung = db.ThuCungs.Find(id);
            if (thuCung == null)
            {
                return HttpNotFound();
            }

            return View(thuCung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThuCung thuCung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thuCung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ThuCungCuaKhachHang", new { id = thuCung.MaKH });
            }

            return View(thuCung);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ThuCung thuCung = db.ThuCungs.Find(id);
            if (thuCung == null)
            {
                return HttpNotFound();
            }

            return View(thuCung);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThuCung thuCung = db.ThuCungs.Find(id);
            db.ThuCungs.Remove(thuCung);
            db.SaveChanges();
            return RedirectToAction("ThuCungCuaKhachHang", new { id = thuCung.MaKH });
        }

        public ActionResult ThuCungCuaKhachHang(int? id)
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
            var thuCungCuaKhachHang = db.ThuCungs.Where(tc => tc.MaKH == id).ToList();
            return View(thuCungCuaKhachHang);
        }
    }
}