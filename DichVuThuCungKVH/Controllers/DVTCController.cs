using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCung.Controllers
{
    public class DVTCController : Controller
    {
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
            return PartialView();
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
    }
}