using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DichVuThuCungKVH.Model;
using System.Linq;

namespace DichVuThuCungKVH.Controllers
{
    public class AccountController : Controller
    {
        private readonly DACSEntities db = new DACSEntities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Email, string Password, bool RememberMe = false)
        {
            try
            {
                var user = db.TaiKhoans.FirstOrDefault(u => u.Email == Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.MatKhau))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, RememberMe);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình đăng nhập." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string Email, string Password, string ConfirmPassword)
        {
            try
            {
                if (Password != ConfirmPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu xác nhận không khớp." });
                }

                if (db.TaiKhoans.Any(u => u.Email == Email))
                {
                    return Json(new { success = false, message = "Email này đã được sử dụng." });
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                var newUser = new TaiKhoan
                {
                    Email = Email,
                    MatKhau = hashedPassword,
                    NgayTao = DateTime.Now,
                    TrangThai = true
                };

                db.TaiKhoans.Add(newUser);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình đăng ký." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Profile()
        {
            var user = db.TaiKhoans.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
    }
} 