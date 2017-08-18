using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wedding_planner.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace wedding_planner.Controllers
{
    public class LoginController : Controller
    {
        private WeddingContext _context;
 
        public LoginController(WeddingContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel user)
        {
            if (ModelState.IsValid) {
                User newuser = new User {
                    FirstName = user.FirstName,
                    LastName= user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    CreatedAt = DateTime.Now,
                    UploadedAt = DateTime.Now,
                };
                User getuser = _context.Users.SingleOrDefault(get => get.Email == user.Email);
                if (getuser == null) {
                    _context.Users.Add(newuser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("userid", newuser.UserId);
                    return RedirectToAction("Index", "Wedding");
                } else {
                    ViewBag.Error = "Email already in database.";
                }
            }
            return View("Index");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            ViewBag.Error = "false";
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {
            User getuser = _context.Users.SingleOrDefault(get => get.Email == Email);
            if (getuser != null && Password != null) {
                if (getuser.Password == Password) {
                    HttpContext.Session.SetInt32("userid", getuser.UserId);
                    return RedirectToAction("Index", "Wedding");
                }
                ViewBag.Error = "Email/Password Invalid.";
            } else {
                ViewBag.Error = "Email/Password Invalid. May need to register.";
            }
            return View("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }
    }
}