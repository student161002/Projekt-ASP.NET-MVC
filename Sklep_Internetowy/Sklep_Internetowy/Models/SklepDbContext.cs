using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Sklep_Internetowy.Models;

public partial class SklepDbContext : IdentityDbContext
{
    public SklepDbContext()
    {
    }

    public SklepDbContext(DbContextOptions<SklepDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kategorie> Kategories { get; set; }

    public virtual DbSet<PozycjeZamowienium> PozycjeZamowienia { get; set; }

    public virtual DbSet<Produkty> Produkties { get; set; }

    public virtual DbSet<Zamowienium> Zamowienia { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Kategorie>(entity =>
        {
            entity.HasKey(e => e.KategoriaId).HasName("PK__Kategori__37D210ECA5AB7EC6");

            entity.ToTable("Kategorie");

            entity.Property(e => e.KategoriaId).HasColumnName("KategoriaID");
            entity.Property(e => e.Nazwa).HasMaxLength(50);
            entity.Property(e => e.Opis).HasMaxLength(200);
        });

        modelBuilder.Entity<PozycjeZamowienium>(entity =>
        {
            entity.HasKey(e => e.PozycjaId).HasName("PK__PozycjeZ__A67D6D74BCBB1E69");

            entity.Property(e => e.PozycjaId).HasColumnName("PozycjaID");
            entity.Property(e => e.CenaJednostkowa).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProduktId).HasColumnName("ProduktID");
            entity.Property(e => e.ZamowienieId).HasColumnName("ZamowienieID");

            entity.HasOne(d => d.Produkt).WithMany(p => p.PozycjeZamowienia)
                .HasForeignKey(d => d.ProduktId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pozycje_Produkty");

            entity.HasOne(d => d.Zamowienie).WithMany(p => p.PozycjeZamowienia)
                .HasForeignKey(d => d.ZamowienieId)
                .HasConstraintName("FK_Pozycje_Zamowienia");
        });

        modelBuilder.Entity<Produkty>(entity =>
        {
            entity.HasKey(e => e.ProduktId).HasName("PK__Produkty__F1FF3022834FF6F4");

            entity.ToTable("Produkty");

            entity.Property(e => e.ProduktId).HasColumnName("ProduktID");
            entity.Property(e => e.Cena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.KategoriaId).HasColumnName("KategoriaID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);

            entity.HasOne(d => d.Kategoria).WithMany(p => p.Produkties)
                .HasForeignKey(d => d.KategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Produkty_Kategorie");
        });

        modelBuilder.Entity<Zamowienium>(entity =>
        {
            entity.HasKey(e => e.ZamowienieId).HasName("PK__Zamowien__7BB9EB606F7B650C");

            entity.Property(e => e.ZamowienieId).HasColumnName("ZamowienieID");
            entity.Property(e => e.AdresWysylki).HasMaxLength(200);
            entity.Property(e => e.DataZamowienia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImieNazwiskoKlienta).HasMaxLength(100);
            entity.Property(e => e.WartoscCalkowita)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
