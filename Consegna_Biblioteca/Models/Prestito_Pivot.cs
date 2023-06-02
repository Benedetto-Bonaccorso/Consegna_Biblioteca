namespace Consegna_Biblioteca.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    using System.Linq;


    public partial class Prestito_Pivot
    {
        [Key]
        public int Id_Prestito { get; set; }

        public int Id_Utente_Foreign { get; set; }

        public int Id_Documento_Foreign { get; set; }

        public bool? In_Essere { get; set; }

        public virtual Documento Documento { get; set; }

        public virtual Utente Utente { get; set; }
    }
    public class PrestitoJson
    {
        public int Id_Prestito { get; set; }
        public int Id_Documento { get; set; }
        public string Titolo_Documento { get; set; }
        public int Id_Utente { get; set; }
        public string Cognome_Utente { get; set; }

        public static List<PrestitoJson> ListaPrestitiEssere()
        {
            ModelDBContext DBContext = new ModelDBContext();

            List<PrestitoJson> listaTuttiPrestiti = new List<PrestitoJson>();
            foreach (Prestito_Pivot pre in DBContext.Prestito_Pivot.Where(x=>x.In_Essere == true).ToList())
            {
                PrestitoJson prestito = new PrestitoJson();

                prestito.Id_Prestito = pre.Id_Prestito;
                prestito.Id_Documento = pre.Id_Documento_Foreign;
                prestito.Titolo_Documento = pre.Documento.Titolo;
                prestito.Id_Utente = pre.Id_Utente_Foreign;
                prestito.Cognome_Utente = pre.Utente.Cognome;

                listaTuttiPrestiti.Add(prestito);
            }
            return listaTuttiPrestiti;
        }
    }
    public class ContaPrestitoJson
    {
        public string NomeSettore { get; set; }
        public int ConteggioPrestiti { get; set; }

        public static List<ContaPrestitoJson> ListaPrestitiSettore()
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

            return contaPrestitiSettore;
        }

    }
}
