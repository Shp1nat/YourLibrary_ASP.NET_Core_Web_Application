using Backend.Api.Contract;
using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace Backend.Api.Services
{
    public class ExampleService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public ExampleService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? CreateExample(NewExample newExample)
        {
            var book = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Book>().SingleOrDefault(x => x.UidBook == newExample.UidBook);
            var publisher = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Publisher>().SingleOrDefault(x => x.UidPublisher == newExample.UidPublisher);
            if (book == null || publisher == null) return null;
            var example = new DatabaseAccessLayer.Entities.Example
            {
                UidExample = Guid.NewGuid(),
                Book = book,
                Publisher = publisher,
                YearOfCreation = newExample.YearOfCreation,
                IsTaken = false
            };
            _libraryWebDbContex.Add(example);
            _libraryWebDbContex.SaveChanges();
            return example.UidExample;
        }
        public List<Contract.Example>? GetExamples()
        {
            var examples = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Example>()
                .Include(x => x.Book)
                .Include(x => x.Publisher)
                .Include(x => x.Book.AuthorsOfBook)
                .Include(x => x.Book.GenresOfBook)
                .Include(x => x.Book.TypesBkOfBook)
                .ToList();
            if (examples.Count == 0) return null; 

            return examples.Select(example => new Contract.Example
            {
                UidExample = example.UidExample,
                Book = new Contract.Book
                {
                    UidBook = example.Book.UidBook,
                    Shifr = example.Book.Shifr,
                    NameOfBook = example.Book.NameOfBook,
                    Authors = example.Book.AuthorsOfBook.Select(x => x.NameOfAuthor).ToList(),
                    Genres = example.Book.GenresOfBook.Select(x => x.NameOfGenre).ToList(),
                    TypesBk = example.Book.TypesBkOfBook.Select(x => x.NameOfTypeBk).ToList()
                },
                Publisher = new Contract.Publisher
                { 
                    UidPublisher= example.Publisher.UidPublisher,
                    NameOfPublisher= example.Publisher.NameOfPublisher
                },
                YearOfCreation = example.YearOfCreation,
                IsTaken = example.IsTaken
            }).ToList();
        }     
        public bool EditExample(Guid uIdExample, NewExample newExample)
        {
            Guid uIdBook = newExample.UidBook;
            Guid uIdPublisher = newExample.UidPublisher;
            var example = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Example>().SingleOrDefault(x => x.UidExample == uIdExample);
            var book = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Book>().SingleOrDefault(x => x.UidBook == uIdBook);
            var publisher = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Publisher>().SingleOrDefault(x => x.UidPublisher == uIdPublisher);
            if (example == null || book == null || publisher == null) return false;
            example.Book= book;
            example.Publisher= publisher;
            example.YearOfCreation= newExample.YearOfCreation;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool SetExampleStatus(Guid uIdExample, bool isTaken)
        {
            var example = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Example>().SingleOrDefault(x => x.UidExample == uIdExample);
            if (example == null) return false;
            example.IsTaken = isTaken;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteExample(Guid uIdExample)
        {
            var example = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Example>()
                .Include(x => x.Book)
                .Include(x => x.Publisher)
                .SingleOrDefault(x => x.UidExample == uIdExample);
            var order = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Order>()
                .Include(x => x.Example)
                .Where(x => x.Example.UidExample == uIdExample)
                .ToList();
            if (example == null) return false;
            try
            {
                foreach(var el in order)
                {
                    _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Order>().Remove(el);
                }
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Example>().Remove(example);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}