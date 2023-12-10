using DichVuThuCungKVH.Model;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private LTWEntities db = new LTWEntities();

        // GET: Admin/SanPhams
        public ActionResult Index()
        {
            return View(db.SanPhams.ToList());
        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,Anh,LoaiSP,GiaBan,MoTaSP,SoLuongConLai")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sanPham);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPhams.Find(id);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,Anh,LoaiSP,GiaBan,MoTaSP,SoLuongConLai")] SanPham sanPham, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Save the uploaded image and update the 'Anh' property
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/SanPham/"), fileName);
                    imageFile.SaveAs(path);

                    sanPham.Anh = fileName; // Update the 'Anh' property with the uploaded file name
                }

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sanPham);
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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