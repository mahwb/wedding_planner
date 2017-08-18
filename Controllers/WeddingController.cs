using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wedding_planner.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace wedding_planner.Controllers
{
    public class WeddingController : Controller
    {
        private WeddingContext _context;

        public WeddingController(WeddingContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            //delete expired weddings and guests attached to event
            List<Wedding> weddings = _context.Weddings.Include(wedding => wedding.Guests).ThenInclude(guest => guest.User).ToList();
            foreach (var wedding in weddings)
            {
                if (wedding.Date < DateTime.Now)
                {
                    _context.Weddings.Remove(wedding);
                    foreach (var guest in wedding.Guests)
                    {
                        if (guest.WeddingId == wedding.WeddingId)
                        {
                            _context.Guests.Remove(guest);
                        }
                    }
                }
            }
            _context.SaveChanges();
            ViewBag.Weddings = _context.Weddings.Include(wedding => wedding.Guests).ThenInclude(guest => guest.User).ToList(); ;
            ViewBag.UserId = HttpContext.Session.GetInt32("userid");
            return View();
        }

        [HttpGet]
        [Route("newwedding")]
        public IActionResult NewPage()
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }

        [HttpPost]
        [Route("new")]
        public IActionResult New(WeddingCheck weddinginfo)
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            if (ModelState.IsValid)
            {
                Wedding newwedding = new Wedding
                {
                    WedderOne = weddinginfo.WedderOne,
                    WedderTwo = weddinginfo.WedderTwo,
                    Date = weddinginfo.Date,
                    Address = weddinginfo.Address,
                    CreatedAt = DateTime.Now,
                    UploadedAt = DateTime.Now,
                    UserId = (int)HttpContext.Session.GetInt32("userid"),
                };
                _context.Weddings.Add(newwedding);
                _context.SaveChanges();
                return RedirectToAction("InfoPage", new { id = newwedding.WeddingId });
            }
            return View("NewPage");
        }

        [HttpGet]
        [Route("info/{id}")]
        public IActionResult InfoPage(int id)
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            ViewBag.Wedding = _context.Weddings.Include(wedding => wedding.Guests).ThenInclude(guest => guest.User).SingleOrDefault(wedding => wedding.WeddingId == id);
            return View();
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            Wedding delwedding = _context.Weddings.SingleOrDefault(wedding => wedding.WeddingId == id);
            if (delwedding.UserId == HttpContext.Session.GetInt32("userid"))
            {
                List<Guest> delguests = _context.Guests.Where(guest => guest.WeddingId == id).ToList();
                _context.Weddings.Remove(delwedding);
                //delete guest from the event
                foreach (var guest in delguests)
                {
                    _context.Guests.Remove(guest);
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("rsvp/{id}")]
        public IActionResult Rsvp(int id)
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            Guest newguest = new Guest
            {
                UserId = (int)HttpContext.Session.GetInt32("userid"),
                WeddingId = id,
                CreatedAt = DateTime.Now,
                UploadedAt = DateTime.Now,
            };
            _context.Guests.Add(newguest);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("unrsvp/{id}")]
        public IActionResult Unrsvp(int id)
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            Guest unrsvp = _context.Guests.SingleOrDefault(guest => guest.WeddingId == id && guest.UserId == (int)HttpContext.Session.GetInt32("userid"));
            if (unrsvp.UserId == (int)HttpContext.Session.GetInt32("userid"))
            {
                _context.Guests.Remove(unrsvp);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}