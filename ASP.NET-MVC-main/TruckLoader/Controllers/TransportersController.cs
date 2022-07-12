using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TruckLoader.Models;

namespace TruckLoader.Controllers
{
    public class TransportersController : Controller
    {
        TruckLoaderEntities4 db = new TruckLoaderEntities4();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            ModelState.Clear();
            if (login.Name != null)
            {
                if (login.Password != null)
                {
                    if (ModelState.IsValid)
                    {
                        var det = (from Userlist in db.Logins
                                   where Userlist.Name == login.Name && Userlist.Password == login.Password
                                   select new
                                   {
                                       Userlist.Id,
                                       Userlist.Name
                                   }).ToList();
                        if (det.FirstOrDefault() != null)
                        {
                            Session["UserId"] = det.FirstOrDefault().Id;
                            Session["UserName"] = det.FirstOrDefault().Name;
                            return RedirectToAction("Create");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Credentials");
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Password is required");
                }
            }
            else
            {
                ModelState.AddModelError("Name", "Name is required");
            }
            return View("Login", login);
        }

      
        public ActionResult Register()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Name,Email,Password,MobileNumber")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Add(login);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(login);
        }

        public ActionResult Create()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransporterId,TypeOfTruck,Capacity,City,OffLoading,Refrigerated,ContactNumber,Charges,AdditionalCharge,DriverCharge")] Transporter transporter)
        {
            if (ModelState.IsValid)
            {
                db.Transporters.Add(transporter);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(transporter);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
