using DichVuThuCungKVH.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Controllers
{
    public class UserController : Controller
    {
        private DACSEntities db = new DACSEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection)
        {
            var sHoTen = collection["HoTen"];
            var sTenTK = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenTK))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Mật khẩu không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Nhập lại mật khẩu không được rỗng";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sDiaChi))
            {
                ViewData["err5"] = "Địa chỉ không được rỗng";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err6"] = "Email không được rỗng";
            }
            else if (db.TaiKhoans.Any(n => n.TenTK == sTenTK))
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KhachHangs.Any(n => n.Email == sEmail))
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                KhachHang kh = new KhachHang
                {
                    TenKH = sHoTen,
                    Email = sEmail,
                    DiaChi = sDiaChi,
                    SDT = sDienThoai,
                    NgaySinh = DateTime.Parse(dNgaySinh)
                };

                TaiKhoan tk = new TaiKhoan
                {
                    TenTK = sTenTK,
                    MatKhau = sMatKhau
                };

                db.KhachHangs.Add(kh);
                db.TaiKhoans.Add(tk);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {


            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {

                TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.TenTK == sTenDN && n.MatKhau == sMatKhau);
                if (tk != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = tk;
                    Session["MaTaiKhoan"] = tk.MaTK;
                    return RedirectToAction("Index", "DVTC");
                }

                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }

            }
            return View();


        }
    }
}