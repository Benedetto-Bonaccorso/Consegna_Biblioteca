using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consegna_Biblioteca.Models;

namespace Consegna_Biblioteca.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettoreController : Controller
    {
        // GET: Settore
        ModelDBContext DBConnection = new ModelDBContext();
        [HttpGet]
        public ActionResult Index()
        {
            List<Settore> listaSettori = DBConnection.Settore.ToList();
            // Per Gli Utenti .Where(x => x.Stato_Disponibilità == true)
            return View(listaSettori);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Settore s)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DBConnection.Settore.Add(s);
                    DBConnection.SaveChanges();

                    TempData["SuccessoAggiuntaSettore"] = "Settore aggiunto con successo";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Errore = ex.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.Errore = "Attenzione al riempire correttamente i campi";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            return View(DBConnection.Settore.Find(id));

        }
        [HttpPost]
        public ActionResult Edit(Settore s)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DBConnection.Entry(s).State = System.Data.Entity.EntityState.Modified;
                    DBConnection.SaveChanges();
                    TempData["SuccessoModificaSettore"] = "Settore modificato con successo";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Errore = ex.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.Errore = "Attenzione al riempire correttamente i campi";
                return View();
            }
        }
    }
}