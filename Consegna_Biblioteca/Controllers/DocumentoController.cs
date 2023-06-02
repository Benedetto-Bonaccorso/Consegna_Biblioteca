using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consegna_Biblioteca.Models;

namespace Consegna_Biblioteca.Controllers
{
    
    public class DocumentoController : Controller
    {
        // GET: Documento
        ModelDBContext DBContext = new ModelDBContext();

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Documento> listaDocumenti = DBContext.Documento.ToList();
            return View(listaDocumenti);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Documento d)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    if (d.Image != null)
                    {
                        if (d.Image.ContentLength > 0)
                        {
                            string fileName = d.Image.FileName;
                            string path = Server.MapPath("~/Content/img");

                            d.Image.SaveAs($"{path}/{fileName}");
                        }
                        d.img_Url = d.Image.FileName;
                    }
                    d.Stato_Disponibilità = true;
                    DBContext.Documento.Add(d);
                    DBContext.SaveChanges();

                    TempData["SuccessoAggiuntaDocumento"] = "Documento aggiunto con successo";
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {

            return View(DBContext.Documento.Find(id));

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Documento d)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (d.Image != null)
                    {
                        if (d.Image.ContentLength > 0)
                        {
                            string fileName = d.Image.FileName;
                            string path = Server.MapPath("~/Content/img");

                            d.Image.SaveAs($"{path}/{fileName}");
                        }
                        d.img_Url = d.Image.FileName;
                    }
                    
                    DBContext.Entry(d).State = System.Data.Entity.EntityState.Modified;
                    DBContext.SaveChanges();
                    TempData["SuccessoModificaDocumento"] = "Documento modificato con successo";
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
        [Authorize(Roles = "Admin")]
        public ActionResult InformazioniPrestiti()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Galleria()
        {
            List<Documento> listaDocumenti = DBContext.Documento.Where(x => x.Stato_Disponibilità == true).ToList();
            return View(listaDocumenti);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult Prestito(int id)
        {
            return View(DBContext.Documento.Find(id));
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Prestito(Documento d)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Prestito_Pivot prestito = new Prestito_Pivot();
                    Utente u = DBContext.Utente.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                    prestito.Id_Documento_Foreign = d.Id_Documento;
                    prestito.Id_Utente_Foreign = u.Id_Utente;
                    prestito.In_Essere = true;

                    d.Stato_Disponibilità = false;

                    DBContext.Prestito_Pivot.Add(prestito);
                    DBContext.Entry(d).State = System.Data.Entity.EntityState.Modified;
                    DBContext.SaveChanges();
                    TempData["SuccessoPrestitoDocumento"] = "Documento preso in prestito con successo";
                    return RedirectToAction("Galleria");
                }
                catch (Exception ex)
                {
                    ViewBag.Errore = ex.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.Errore = "Sembra ci sia stato un errore inaspettato";
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Prestiti()
        {
            List<Prestito_Pivot> listaPrestiti = DBContext.Prestito_Pivot.Where(x => x.In_Essere == true).ToList();
            return View(listaPrestiti);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ArchiviaPrestito(int id)
        {
            return View(DBContext.Prestito_Pivot.Find(id));
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ArchiviaPrestito(Prestito_Pivot p)
        {
            try
            {
                Prestito_Pivot pre = DBContext.Prestito_Pivot.Where(x => x.Id_Prestito == p.Id_Prestito).FirstOrDefault();
                pre.In_Essere = false;
                DBContext.Entry(pre).State = System.Data.Entity.EntityState.Modified;

                Documento doc = DBContext.Documento.Where(x => x.Id_Documento == p.Id_Documento_Foreign).FirstOrDefault();
                doc.Stato_Disponibilità = true;
                DBContext.Entry(doc).State = System.Data.Entity.EntityState.Modified;

                DBContext.SaveChanges();
                return RedirectToAction("Prestiti");
            }
            catch (Exception ex)
            {
                ViewBag.Errore = ex.Message;
                return RedirectToAction("Prestiti");
            }
        }

        public JsonResult ListaPrestitiSettore()
        {

            ModelDBContext DBContext = new ModelDBContext();

            List<ContaPrestitoJson> contaPrestitiSettore = new List<ContaPrestitoJson>();

            foreach (Settore s in DBContext.Settore)
            {
                ContaPrestitoJson pres = new ContaPrestitoJson();
                pres.NomeSettore = s.Nome;
                pres.ConteggioPrestiti = 0;

                foreach (Prestito_Pivot p in DBContext.Prestito_Pivot)
                {
                    if (p.Documento.Settore == s && p.In_Essere == true)
                    {
                        pres.ConteggioPrestiti++;
                    }
                }
                contaPrestitiSettore.Add(pres);
            }

            return Json (contaPrestitiSettore, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaPrestitiInEssere()
        {
            ModelDBContext DBContext = new ModelDBContext();

            List<PrestitoJson> listaTuttiPrestiti = new List<PrestitoJson>();
            foreach (Prestito_Pivot pre in DBContext.Prestito_Pivot.Where(x => x.In_Essere == true).ToList())
            {
                PrestitoJson prestito = new PrestitoJson();

                prestito.Id_Prestito = pre.Id_Prestito;
                prestito.Id_Documento = pre.Id_Documento_Foreign;
                prestito.Titolo_Documento = pre.Documento.Titolo;
                prestito.Id_Utente = pre.Id_Utente_Foreign;
                prestito.Cognome_Utente = pre.Utente.Cognome;

                listaTuttiPrestiti.Add(prestito);
            }
            return Json (listaTuttiPrestiti, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CercaDocumentoNome(string id)
        {
            Documento d = new Documento();
            d = DBContext.Documento.Where(x => x.Titolo == id).FirstOrDefault();
            Documento doc = new Documento();
            if (d != null && d.Stato_Disponibilità == true)
            {
                doc.Id_Documento = d.Id_Documento;
                doc.Id_Settore_Foreign = d.Id_Settore_Foreign;
                doc.Id_Tipo_Foreign = d.Id_Tipo_Foreign;
                doc.Titolo = d.Titolo;
                doc.Anno = d.Anno;
                doc.Scaffale = d.Scaffale;
                doc.Stato_Disponibilità = d.Stato_Disponibilità;
                doc.Lunghezza = d.Lunghezza;
                doc.img_Url = d.img_Url;
            }

            return Json(doc, JsonRequestBehavior.AllowGet);
        }

        

        [HttpGet]
        public JsonResult CercaDocumentoId(int id)
        {
            Documento d = new Documento();
            d = DBContext.Documento.Find(id);
                Documento doc = new Documento();
            if(d != null && d.Stato_Disponibilità == true)
            {
                doc.Id_Documento = d.Id_Documento;
                doc.Id_Settore_Foreign = d.Id_Settore_Foreign;
                doc.Id_Tipo_Foreign = d.Id_Tipo_Foreign;
                doc.Titolo = d.Titolo;
                doc.Anno = d.Anno;
                doc.Scaffale = d.Scaffale;
                doc.Stato_Disponibilità = d.Stato_Disponibilità;
                doc.Lunghezza = d.Lunghezza;
                doc.img_Url = d.img_Url;
            }

                return Json(doc, JsonRequestBehavior.AllowGet);
        }
    }
}
