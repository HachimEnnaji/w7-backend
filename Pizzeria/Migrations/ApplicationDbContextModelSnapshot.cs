﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pizzeria.data;

#nullable disable

namespace Pizzeria.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Pizzeria.Models.Articolo", b =>
                {
                    b.Property<int>("IdArticolo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdArticolo"));

                    b.Property<string>("Immagine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ingredienti")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Prezzo")
                        .HasColumnType("float");

                    b.Property<int>("TempoConsegna")
                        .HasColumnType("int");

                    b.HasKey("IdArticolo");

                    b.ToTable("Articoli");
                });

            modelBuilder.Entity("Pizzeria.Models.DettagliOrdine", b =>
                {
                    b.Property<int>("IdDettagliOrdine")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDettagliOrdine"));

                    b.Property<int>("IdArticolo")
                        .HasColumnType("int");

                    b.Property<int>("IdOrdine")
                        .HasColumnType("int");

                    b.Property<double>("PrezzoUnitario")
                        .HasColumnType("float");

                    b.Property<int>("Quantita")
                        .HasColumnType("int");

                    b.HasKey("IdDettagliOrdine");

                    b.HasIndex("IdArticolo");

                    b.HasIndex("IdOrdine");

                    b.ToTable("DettagliOrdini");
                });

            modelBuilder.Entity("Pizzeria.Models.Ordine", b =>
                {
                    b.Property<int>("IdOrdine")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdOrdine"));

                    b.Property<int>("IdUtente")
                        .HasColumnType("int");

                    b.Property<string>("Indirizzo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsConsegnato")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("PrezzoTotale")
                        .HasColumnType("float");

                    b.HasKey("IdOrdine");

                    b.HasIndex("IdUtente");

                    b.ToTable("Ordini");
                });

            modelBuilder.Entity("Pizzeria.Models.Utente", b =>
                {
                    b.Property<int>("IdUtente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUtente"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ruolo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUtente");

                    b.ToTable("Utenti");
                });

            modelBuilder.Entity("Pizzeria.Models.DettagliOrdine", b =>
                {
                    b.HasOne("Pizzeria.Models.Articolo", "Articoli")
                        .WithMany("DettagliOrdini")
                        .HasForeignKey("IdArticolo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pizzeria.Models.Ordine", "Ordine")
                        .WithMany("DettagliOrdini")
                        .HasForeignKey("IdOrdine")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articoli");

                    b.Navigation("Ordine");
                });

            modelBuilder.Entity("Pizzeria.Models.Ordine", b =>
                {
                    b.HasOne("Pizzeria.Models.Utente", "Utente")
                        .WithMany("Ordini")
                        .HasForeignKey("IdUtente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Utente");
                });

            modelBuilder.Entity("Pizzeria.Models.Articolo", b =>
                {
                    b.Navigation("DettagliOrdini");
                });

            modelBuilder.Entity("Pizzeria.Models.Ordine", b =>
                {
                    b.Navigation("DettagliOrdini");
                });

            modelBuilder.Entity("Pizzeria.Models.Utente", b =>
                {
                    b.Navigation("Ordini");
                });
#pragma warning restore 612, 618
        }
    }
}
