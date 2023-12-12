// HomeController.cs
using DichVuThuCungKVH.Model;
using System.Linq;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly DACSEntities db = new DACSEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            TaiKhoan user = db.TaiKhoans.SingleOrDefault(n => n.TenTK == UserName && n.MatKhau == Password);
            if (user != null)
            {
                Session["UserType"] = user.LoaiTK;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }
    }
}