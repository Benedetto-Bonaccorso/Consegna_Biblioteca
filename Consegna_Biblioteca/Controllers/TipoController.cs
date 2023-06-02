using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consegna_Biblioteca.Models;

namespace Consegna_Biblioteca.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TipoController : Controller
    {
        // GET: Tipo
        ModelDBContext DBConnection = new ModelDBContext();
        [HttpGet]
        public ActionResult Index()
        {
            List<Tipo> listaTipi = DBConnection.Tipo.ToList();
            // Per Gli Utenti .Where(x => x.Stato_Disponibilità == true)
            return View(listaTipi);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Tipo t)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DBConnection.Tipo.Add(t);
                    DBConnection.SaveChanges();

                    TempData["SuccessoAggiuntaTipo"] = "Tipo aggiunto con successo";
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

            return View(DBConnection.Tipo.Find(id));

        }
        [HttpPost]
        public ActionResult Edit(Tipo t)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DBConnection.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    DBConnection.SaveChanges();
                    TempData["SuccessoModificaTipo"] = "Tipo modificato con successo";
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