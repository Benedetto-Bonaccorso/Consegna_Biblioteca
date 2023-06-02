using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Consegna_Biblioteca.Models;

namespace Consegna_Biblioteca.Controllers
{

    public class ValuesController : ApiController
    {
        ModelDBContext DBContext = new ModelDBContext();

        // GET api/values
        [Route("api/prestiti")]
        [HttpGet]
        public IEnumerable<Prestito_Pivot> prestiti()
        {
            List<Prestito_Pivot> listaPrestitiApi = new List<Prestito_Pivot>();
            foreach (Prestito_Pivot p in DBContext.Prestito_Pivot.Where(x=>x.In_Essere == true))
            {
                Prestito_Pivot pre = new Prestito_Pivot();
                pre.Id_Prestito = p.Id_Prestito;
                pre.In_Essere = p.In_Essere;
                pre.Id_Utente_Foreign = p.Id_Utente_Foreign;
                pre.Id_Documento_Foreign = p.Id_Documento_Foreign;
                listaPrestitiApi.Add(pre);
            }
            return listaPrestitiApi;
        }

        [Route("api/utentiSenzaPrestiti")]
        [HttpGet]
        public IEnumerable<Utente> utentiSenzaPrestiti()
        {
            List<Utente> listaUtentiSenzaPrestito = DBContext.Utente.ToList();
            foreach (Utente u in DBContext.Utente.ToList())
            {
                foreach (Prestito_Pivot p in DBContext.Prestito_Pivot.ToList())
                {
                    if (u.Id_Utente == p.Id_Utente_Foreign)
                    {
                        listaUtentiSenzaPrestito.Remove(u);
                        break;
                    }
                }
            }

            List<Utente> listaUtentiSenzaPrestitoDaRestituire = new List<Utente>();
                foreach (Utente u in listaUtentiSenzaPrestito)
                {
                    Utente user = new Utente();
                    user.Id_Utente = u.Id_Utente;
                    user.Nome = u.Nome;
                    user.Cognome = u.Cognome;
                    user.Email = u.Email;
                    user.Password = u.Password;
                    user.Telefono = u.Telefono;
                    user.IsAdmin = u.IsAdmin;
                    listaUtentiSenzaPrestitoDaRestituire.Add(user);
                }

            
            return listaUtentiSenzaPrestitoDaRestituire;
            }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
