using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    public class SahPodaci : DbContext, IDisposable
    {
        public SahPodaci()
            : base("DefaultConnection")
        {
        }

        public DbSet<Osoba> Osobe { get; set; }
        public DbSet<DioIgre> Pozicije { get; set; }
        public DbSet<Komentar> Komentari { get; set; }
        public DbSet<PokusajRjesenje> PokusajiRjesenja { get; set; }
        public DbSet<TipIgre> Tipovi { get; set; }
        public DbSet<Potez> Potezi { get; set; }
        public DbSet<Partija> Partije { get; set; }
        //public DbSet<StanjePloca> Stanja { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Potez>()
        //    //.HasMany(e => e.poteziDjeca)
        //    //.WithMany()
        //    //.Map(m =>
        //    //{
        //    //    m.MapLeftKey("Potez_Id");
        //    //    m.MapRightKey("PotezDijete_Id");
        //    //    m.ToTable("EntryRelations");
        //    //});

        //    modelBuilder.Entity<DioIgre>()
        //        .HasRequired(s => s.potezi)
        //        .WithMany()
        //        .WillCascadeOnDelete(true);
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}