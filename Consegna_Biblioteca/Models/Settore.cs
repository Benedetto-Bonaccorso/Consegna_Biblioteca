namespace Consegna_Biblioteca.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    using System.Linq;

    [Table("Settore")]
    public partial class Settore
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Settore()
        {
            Documento = new HashSet<Documento>();
        }

        [Key]
        public int Id_Settore { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Settore")]
        public string Nome { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }

        public static List<SelectListItem> ListaSettori
        {
            get
            {
                ModelDBContext dbContext = new ModelDBContext();
                List<SelectListItem> _tempList = new List<SelectListItem>();
                foreach (Settore s in dbContext.Settore.ToList())
                {
                    SelectListItem selectListItem = new SelectListItem
                    {
                        Text = s.Nome,
                        Value = s.Id_Settore.ToString()
                    };
                    _tempList.Add(selectListItem);
                }
                return _tempList;
            }
        }
    }
}
