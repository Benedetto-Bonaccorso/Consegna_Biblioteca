using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Consegna_Biblioteca.Models
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext")
        {
        }

        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Prestito_Pivot> Prestito_Pivot { get; set; }
        public virtual DbSet<Settore> Settore { get; set; }
        public virtual DbSet<Tipo> Tipo { get; set; }
        public virtual DbSet<Utente> Utente { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Documento>()
                .HasMany(e => e.Prestito_Pivot)
                .WithRequired(e => e.Documento)
                .HasForeignKey(e => e.Id_Documento_Foreign)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Settore>()
                .HasMany(e => e.Documento)
                .WithOptional(e => e.Settore)
                .HasForeignKey(e => e.Id_Settore_Foreign);

            modelBuilder.Entity<Tipo>()
                .HasMany(e => e.Documento)
                .WithOptional(e => e.Tipo)
                .HasForeignKey(e => e.Id_Tipo_Foreign);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Prestito_Pivot)
                .WithRequired(e => e.Utente)
                .HasForeignKey(e => e.Id_Utente_Foreign)
                .WillCascadeOnDelete(false);
        }
    }
}
