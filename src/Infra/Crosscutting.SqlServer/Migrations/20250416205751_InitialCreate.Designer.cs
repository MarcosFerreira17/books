﻿// <auto-generated />
using Infra.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infra.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250416205751_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Assunto", b =>
                {
                    b.Property<int>("CodAs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodAs"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("CodAs");

                    b.ToTable("Assunto");
                });

            modelBuilder.Entity("Domain.Entities.Autor", b =>
                {
                    b.Property<int>("CodAu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodAu"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("CodAu");

                    b.ToTable("Autor");
                });

            modelBuilder.Entity("Domain.Entities.Livro", b =>
                {
                    b.Property<int>("Codl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Codl"));

                    b.Property<string>("AnoPublicacao")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<int>("Edicao")
                        .HasColumnType("int");

                    b.Property<string>("Editora")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Codl");

                    b.ToTable("Livro");
                });

            modelBuilder.Entity("Domain.Entities.LivroAssunto", b =>
                {
                    b.Property<int>("LivroCodl")
                        .HasColumnType("int")
                        .HasColumnName("Livro_Codl");

                    b.Property<int>("AssuntoCodAs")
                        .HasColumnType("int")
                        .HasColumnName("Assunto_CodAs");

                    b.HasKey("LivroCodl", "AssuntoCodAs");

                    b.HasIndex("AssuntoCodAs");

                    b.ToTable("Livro_Assunto");
                });

            modelBuilder.Entity("Domain.Entities.LivroAutor", b =>
                {
                    b.Property<int>("LivroCodl")
                        .HasColumnType("int")
                        .HasColumnName("Livro_Codl");

                    b.Property<int>("AutorCodAu")
                        .HasColumnType("int")
                        .HasColumnName("Autor_CodAu");

                    b.HasKey("LivroCodl", "AutorCodAu");

                    b.HasIndex("AutorCodAu");

                    b.ToTable("Livro_Autor");
                });

            modelBuilder.Entity("Domain.Entities.LivroPreco", b =>
                {
                    b.Property<int>("Codp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Codp"));

                    b.Property<int>("LivroCodl")
                        .HasColumnType("int")
                        .HasColumnName("Livro_Codl");

                    b.Property<string>("TipoCompra")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Codp");

                    b.HasIndex("LivroCodl");

                    b.ToTable("LivroPreco");
                });

            modelBuilder.Entity("Domain.Views.RelatorioAutoresLivros", b =>
                {
                    b.Property<string>("Assuntos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Autor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Livro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalLivros")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("RelatorioAutoresLivros", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.LivroAssunto", b =>
                {
                    b.HasOne("Domain.Entities.Assunto", "Assunto")
                        .WithMany("Livros")
                        .HasForeignKey("AssuntoCodAs")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Livro", "Livro")
                        .WithMany("Assuntos")
                        .HasForeignKey("LivroCodl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assunto");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("Domain.Entities.LivroAutor", b =>
                {
                    b.HasOne("Domain.Entities.Autor", "Autor")
                        .WithMany("Livros")
                        .HasForeignKey("AutorCodAu")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Livro", "Livro")
                        .WithMany("Autores")
                        .HasForeignKey("LivroCodl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("Domain.Entities.LivroPreco", b =>
                {
                    b.HasOne("Domain.Entities.Livro", "Livro")
                        .WithMany("Precos")
                        .HasForeignKey("LivroCodl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("Domain.Entities.Assunto", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("Domain.Entities.Autor", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("Domain.Entities.Livro", b =>
                {
                    b.Navigation("Assuntos");

                    b.Navigation("Autores");

                    b.Navigation("Precos");
                });
#pragma warning restore 612, 618
        }
    }
}
