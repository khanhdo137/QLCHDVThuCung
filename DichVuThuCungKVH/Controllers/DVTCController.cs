using DichVuThuCungKVH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCung.Controllers
{
    public class DVTCController : Controller
    {
        private LTWEntities db = new LTWEntities();
        // GET: DVTC
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DichVu()
        {
            return View();
        }
        public ActionResult CacDichVu()
        {
            return PartialView();
        }
        public ActionResult SanPhamPartial()
        {
            var listSanPham = (from cd in db.SanPhams select cd).ToList();
            return PartialView(listSanPham);
        }
        public ActionResult NavPartial()
        {
            return PartialView();
        }
        public ActionResult Slider()
        {
            return View();
        }
        public ActionResult FormDatLich()
        {
            return View();
        }
        public ActionResult ChiTietSanPham(int id)
        {
            var sanpham = from sp in db.SanPhams
                          where sp.MaSP == id
                          select sp;
            return View(sanpham.Single());
        }
    }
}