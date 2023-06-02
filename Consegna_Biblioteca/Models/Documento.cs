namespace Consegna_Biblioteca.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web;

    [Table("Documento")]
    public partial class Documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documento()
        {
            Prestito_Pivot = new HashSet<Prestito_Pivot>();
        }

        [Key]
        public int Id_Documento { get; set; }

        [Required]
        [StringLength(50)]
        public string Titolo { get; set; }

        public int Anno { get; set; }

        [Display(Name = "Settore")]
        public int? Id_Settore_Foreign { get; set; }
        [Display(Name = "Disponibile")]
        public bool Stato_Disponibilità { get; set; }

        [StringLength(50)]
        public string Scaffale { get; set; }

        [Display(Name = "Immagine")]
        public string img_Url { get; set; }

        [NotMapped]
        [Display(Name = "Immagine")]
        public HttpPostedFileBase Image { get; set; }

        public int Lunghezza { get; set; }
        [Display(Name = "Tipo")]
        public int? Id_Tipo_Foreign { get; set; }

        public virtual Settore Settore { get; set; }

        public virtual Tipo Tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prestito_Pivot> Prestito_Pivot { get; set; }

    }

    public class DocumentoJson
    {
        public int Id_Documento { get; set; }
        public string Titolo { get; set; }
        public int Lunghezza { get; set; }
        public string Scaffale { get; set; }

        public static List<DocumentoJson> ListaDocumentiJson()
        {
                ModelDBContext dbContext = new ModelDBContext();
                List<DocumentoJson> listaDocumenti = new List<DocumentoJson>();
                foreach (Documento d in dbContext.Documento.ToList())
                {
                    DocumentoJson dj = new DocumentoJson();
                    dj.Id_Documento = d.Id_Documento;
                    dj.Titolo = d.Titolo;
                    dj.Lunghezza = d.Lunghezza;
                    dj.Scaffale = d.Scaffale;
                    listaDocumenti.Add(dj);
                }
                return listaDocumenti;
        }
    }
}
