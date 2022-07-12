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
    public class CustomersController : Controller
    {
        private TruckLoaderEntities4 db = new TruckLoaderEntities4();
        
        public ActionResult Logins()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logins(Login login)
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
            return View("Logins", login);
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
                return RedirectToAction("Logins");
            }

            return View(login);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transporter transporter = db.Transporters.Find(id);
            if (transporter == null)
            {
                return HttpNotFound();
            }
            return View(transporter);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logins");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,GoodsType,Area,City")] Customer customer)
        {
            List<Transporter> transporterlist = new List<Transporter>();
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                
                    var nearbytruck = (from t in db.Transporters
                                       where t.City == customer.City
                                       select new
                                       {
                                           t.TransporterId,
                                           t.TypeOfTruck,
                                           t.Capacity,
                                           t.City,
                                           t.OffLoading,
                                           t.Refrigerated,
                                           t.ContactNumber,
                                           t.Charges,
                                           t.AdditionalCharge,
                                           t.DriverCharge
                                       }).ToList();
                if (!nearbytruck.Any())
                {
                    return View("NotFound");
                }
                else
                {
                    foreach (var Result in nearbytruck)
                    {
                        transporterlist.Add(new Transporter
                        {
                            TransporterId = Result.TransporterId,
                            TypeOfTruck = Result.TypeOfTruck,
                            Capacity = Result.Capacity,
                            City = Result.City,
                            OffLoading = Result.OffLoading,
                            Refrigerated = Result.Refrigerated,
                            ContactNumber = Result.ContactNumber,
                            Charges = Result.Charges,
                            AdditionalCharge = Result.AdditionalCharge,
                            DriverCharge = Result.DriverCharge

                        });
                    }
                    return View("Results", transporterlist);

                }
            
            }
            return View(customer);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }

       
    }
}
