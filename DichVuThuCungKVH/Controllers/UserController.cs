using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DichVuThuCungKVH.Controllers
{
    public class UserController : Controller
    {
        // GET: User
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
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]); // Không cần định dạng ngày

            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenDN))
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
            else if (db.KHACHHANGs.Any(n => n.TaiKhoan == sTenDN))
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.Any(n => n.Email == sEmail))
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN;
                kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DiaChi = sDiaChi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);

                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
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
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    // Đăng nhập thành công, lưu thông tin người dùng vào Session
                    Session["TaiKhoan"] = kh;

                    // Kiểm tra xem người dùng có từ trang giỏ hàng không
                    string returnUrl = Session["ReturnUrl"] as string;
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Session["ReturnUrl"] = null;
                        return Redirect(returnUrl);
                    }

                    // Nếu không, chuyển hướng về trang chính
                    return RedirectToAction("Index", "SachOnline");
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