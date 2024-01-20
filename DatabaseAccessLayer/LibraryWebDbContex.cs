using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer
{
    public class LibraryWebDbContex: DbContext
    {
        public LibraryWebDbContex(DbContextOptions<LibraryWebDbContex> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(x => x.idUser);
            builder.Entity<User>().HasMany(x => x.OrdersOfUser).WithOne(x => x.User).HasForeignKey("idUser");
            builder.Entity<User>().HasMany(x => x.Vacancies).WithOne(x => x.User).HasForeignKey("idUser");

            builder.Entity<Vacancy>().HasKey(x => x.idVacancy);
            builder.Entity<Vacancy>().HasOne(x => x.User).WithMany(x => x.Vacancies).HasForeignKey("idUser");

            builder.Entity<Order>().HasKey(x => x.idOrder);
            builder.Entity<Order>().HasOne(x => x.Example).WithMany(x => x.OrdersWithExample).HasForeignKey("idExample");
            builder.Entity<Order>().HasOne(x => x.User).WithMany(x => x.OrdersOfUser).HasForeignKey("idUser");

            builder.Entity<Example>().HasKey(x => x.idExample);
            builder.Entity<Example>().HasOne(x => x.Book).WithMany(x => x.ExamplesWithBook).HasForeignKey("idBook");
            builder.Entity<Example>().HasOne(x => x.Publisher).WithMany(x => x.ExamplesPublished).HasForeignKey("idPublisher");
            builder.Entity<Example>().HasMany(x => x.OrdersWithExample).WithOne(x => x.Example).HasForeignKey("idExample");

            builder.Entity<Publisher>().HasKey(x => x.idPublisher);
            builder.Entity<Publisher>().HasMany(x => x.ExamplesPublished).WithOne(x => x.Publisher).HasForeignKey("idPublisher");

            builder.Entity<Book>().HasKey(x => x.idBook);
            builder.Entity<Book>().HasMany(x => x.ExamplesWithBook).WithOne(x => x.Book).HasForeignKey("idBook");

            builder.Entity<Author>().HasKey(x => x.idAuthor);
            builder.Entity<Author>().HasMany(x => x.BooksWithAuthor).WithMany(x => x.AuthorsOfBook).UsingEntity("AuthorOfBook",
                x => x.HasOne(typeof(Book)).WithMany().HasForeignKey("idBook").HasPrincipalKey(nameof(Book.idBook)),
                y => y.HasOne(typeof(Author)).WithMany().HasForeignKey("idAuthor").HasPrincipalKey(nameof(Author.idAuthor)),
                z => z.HasKey("idAuthor", "idBook"));

            builder.Entity<TypeBk>().HasKey(x => x.idTypeBk);
            builder.Entity<TypeBk>().HasMany(x => x.BooksWithTypeBk).WithMany(x => x.TypesBkOfBook).UsingEntity("TypeBkOfBook",
                x => x.HasOne(typeof(Book)).WithMany().HasForeignKey("idBook").HasPrincipalKey(nameof(Book.idBook)),
                y => y.HasOne(typeof(TypeBk)).WithMany().HasForeignKey("idTypeBk").HasPrincipalKey(nameof(TypeBk.idTypeBk)),
                z => z.HasKey("idTypeBk", "idBook"));

            builder.Entity<Genre>().HasKey(x => x.idGenre);
            builder.Entity<Genre>().HasMany(x => x.BooksWithGenre).WithMany(x => x.GenresOfBook).UsingEntity("GenreOfBook",
                x => x.HasOne(typeof(Book)).WithMany().HasForeignKey("idBook").HasPrincipalKey(nameof(Book.idBook)),
                y => y.HasOne(typeof(Genre)).WithMany().HasForeignKey("idGenre").HasPrincipalKey(nameof(Genre.idGenre)),
                z => z.HasKey("idGenre", "idBook"));
        }
    }
}
