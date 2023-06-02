namespace Consegna_Biblioteca.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    using System.Linq;

    [Table("Tipo")]
    public partial class Tipo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tipo()
        {
            Documento = new HashSet<Documento>();
        }

        [Key]
        public int Id_Tipo { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tipologia")]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tipo Di Lunghezza")]
        public string Display_Lunghezza { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }

        public static List<SelectListItem> ListaTipi
        {
            get
            {
                ModelDBContext dbContext = new ModelDBContext();
                List<SelectListItem> _tempList = new List<SelectListItem>();
                foreach (Tipo t in dbContext.Tipo.ToList())
                {
                    SelectListItem selectListItem = new SelectListItem
                    {
                        Text = t.Nome,
                        Value = t.Id_Tipo.ToString()
                    };
                    _tempList.Add(selectListItem);
                }
                return _tempList;
            }
        }
    }
}
