using Backend.Api.Contract;
using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Backend.Api.Services
{
    public class BookService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public BookService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? CreateBook(BookUpdate bookUpdate)
        {
            var book = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Book>()
                .SingleOrDefault(x => x.NameOfBook == bookUpdate.NameOfBook
            && x.Shifr == bookUpdate.Shifr);
            if (book != null) return null;

            book = new DatabaseAccessLayer.Entities.Book
            {
                UidBook = Guid.NewGuid(),
                NameOfBook = bookUpdate.NameOfBook,
                Shifr = bookUpdate.Shifr,
            };

            var authors = new List<DatabaseAccessLayer.Entities.Author>();
            foreach(var authorOfBook in bookUpdate.AuthorsOfBook)
            {
                var author = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Author>()
                    .SingleOrDefault(x => x.NameOfAuthor == authorOfBook);
                if (author == null)
                {
                    author = new DatabaseAccessLayer.Entities.Author
                    {
                        UidAuthor = Guid.NewGuid(),
                        NameOfAuthor = authorOfBook
                    };
                    _libraryWebDbContex.Add(author);
                }
                authors.Add(author);
            }
            var genres = new List<DatabaseAccessLayer.Entities.Genre>();
            foreach (var genreOfBook in bookUpdate.GenresOfBook)
            {
                var genre = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Genre>()
                    .SingleOrDefault(x => x.NameOfGenre == genreOfBook);
                if (genre == null)
                {
                    genre = new DatabaseAccessLayer.Entities.Genre
                    {
                        UidGenre = Guid.NewGuid(),
                        NameOfGenre = genreOfBook
                    };
                    _libraryWebDbContex.Add(genre);
                }
                genres.Add(genre);
            }
            var typesBk = new List<DatabaseAccessLayer.Entities.TypeBk>();
            foreach (var typeBkOfBook in bookUpdate.TypesBkOfBook)
            {
                var typeBk = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.TypeBk>()
                    .SingleOrDefault(x => x.NameOfTypeBk == typeBkOfBook);
                if (typeBk == null)
                {
                    typeBk = new DatabaseAccessLayer.Entities.TypeBk
                    {
                        UidTypeBk = Guid.NewGuid(),
                        NameOfTypeBk = typeBkOfBook
                    };
                    _libraryWebDbContex.Add(typeBk);
                }
                typesBk.Add(typeBk);
            }
            book.AuthorsOfBook = authors;
            book.GenresOfBook = genres;
            book.TypesBkOfBook= typesBk;
            _libraryWebDbContex.Add(book);
            _libraryWebDbContex.SaveChanges();
            return book.UidBook;
        }
        public List<Contract.Book>? GetBooks()
        {
            var books = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Book>()
                 .Include(x => x.AuthorsOfBook)
                 .Include(x => x.GenresOfBook)
                 .Include(x => x.TypesBkOfBook)
                 .ToList();
            if (books.Count == 0) return null;

            return books.Select(book => new Contract.Book
            {
                UidBook = book.UidBook,
                Shifr = book.Shifr,
                NameOfBook = book.NameOfBook,
                Authors = book.AuthorsOfBook.Select(x => x.NameOfAuthor).ToList(),
                Genres = book.GenresOfBook.Select(x => x.NameOfGenre).ToList(),
                TypesBk = book.TypesBkOfBook.Select(x => x.NameOfTypeBk).ToList()
            }).ToList();
        }     
        public bool EditBook(Guid uIdBook, BookUpdate bookUpdate)
        {
            var book = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Book>()
                 .Include(x => x.AuthorsOfBook)
                 .Include(x => x.GenresOfBook)
                 .Include(x => x.TypesBkOfBook)
                 .SingleOrDefault(x => x.UidBook == uIdBook);
            if (book == null) return false;
            book.Shifr = bookUpdate.Shifr;
            book.NameOfBook= bookUpdate.NameOfBook;
            book.AuthorsOfBook.Clear();
            var authors = new List<DatabaseAccessLayer.Entities.Author>();
            foreach (var authorOfBook in bookUpdate.AuthorsOfBook)
            {
                var author = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Author>()
                    .SingleOrDefault(x => x.NameOfAuthor == authorOfBook);
                if (author == null)
                {
                    author = new DatabaseAccessLayer.Entities.Author
                    {
                        UidAuthor = Guid.NewGuid(),
                        NameOfAuthor = authorOfBook
                    };
                    _libraryWebDbContex.Add(author);
                }
                authors.Add(author);
            }
            book.GenresOfBook.Clear();
            var genres = new List<DatabaseAccessLayer.Entities.Genre>();
            foreach (var genreOfBook in bookUpdate.GenresOfBook)
            {
                var genre = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Genre>()
                    .SingleOrDefault(x => x.NameOfGenre == genreOfBook);
                if (genre == null)
                {
                    genre = new DatabaseAccessLayer.Entities.Genre
                    {
                        UidGenre = Guid.NewGuid(),
                        NameOfGenre = genreOfBook
                    };
                    _libraryWebDbContex.Add(genre);
                }
                genres.Add(genre);
            }
            book.TypesBkOfBook.Clear();
            var typesBk = new List<DatabaseAccessLayer.Entities.TypeBk>();
            foreach (var typeBkOfBook in bookUpdate.TypesBkOfBook)
            {
                var typeBk = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.TypeBk>()
                    .SingleOrDefault(x => x.NameOfTypeBk == typeBkOfBook);
                if (typeBk == null)
                {
                    typeBk = new DatabaseAccessLayer.Entities.TypeBk
                    {
                        UidTypeBk = Guid.NewGuid(),
                        NameOfTypeBk = typeBkOfBook
                    };
                    _libraryWebDbContex.Add(typeBk);
                }
                typesBk.Add(typeBk);
            }
            book.AuthorsOfBook = authors;
            book.GenresOfBook = genres;
            book.TypesBkOfBook = typesBk;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteBook(Guid uIdBook)
        {
            var book = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Book>()
                .Include(x => x.AuthorsOfBook)
                .Include(x => x.GenresOfBook)
                .Include(x => x.TypesBkOfBook)
                .Include(x => x.ExamplesWithBook)
                .SingleOrDefault(x => x.UidBook == uIdBook);
            if (book == null) return false;
            try
            {
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Book>().Remove(book);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}