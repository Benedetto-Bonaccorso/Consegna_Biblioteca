using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Consegna_Biblioteca.Models;

namespace Consegna_Biblioteca.Controllers
{
    public class UtenteController : Controller
    {
        ModelDBContext DBConnection = new ModelDBContext();
        // GET: Utente

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }

        [AllowAnonymous]
        //REGISTRAZIONE
        public ActionResult Registrazione()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Registrazione([Bind(Exclude = "IsAdmin")] Utente u)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    u.IsAdmin = false;
                    DBConnection.Utente.Add(u);
                    DBConnection.SaveChanges();
                    return Redirect(FormsAuthentication.LoginUrl);
                }
                catch (Exception ex)
                {
                    ViewBag.Errore = ex.Message;
                    return View();
                }

            }
            else
            {
                ViewBag.Errore = "Username o Password errati.";
                return View();
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogInUtente()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogInUtente(string email, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Utente u = DBConnection.Utente.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
                    if (u != null)
                    {
                        FormsAuthentication.SetAuthCookie(u.Email, false);
                        return Redirect(FormsAuthentication.DefaultUrl);
                    }
                    else
                    {
                        ViewBag.Errore = "Email o password errati.";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Errore = ex.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.Errore = "Email o password errati.";
                return View();
            }
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}