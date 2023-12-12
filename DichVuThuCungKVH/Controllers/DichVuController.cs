using DichVuThuCungKVH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Controllers
{
    public class DichVuController : Controller
    {
        private LTWEntities db = new LTWEntities();

        // GET: DichVu
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult DangKyDichVu()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult DangKyDichVu(KhachHang khachHang)
        {
            khachHang.TinhTrangLienHe = false;
            if (ModelState.IsValid)
            {
                //luu db
                var existingTaiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.Email == khachHang.Email);

                if (existingTaiKhoan != null)
                {
                    khachHang.MaTK = existingTaiKhoan.MaTK;
                }
                khachHang.ThoiGianDangKy = DateTime.Now;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();

                return RedirectToAction("DichVu", "DVTC");
            }

            return View(khachHang);
        }
    }
}