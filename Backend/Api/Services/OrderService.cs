using Backend.Api.Contract;
using Backend.Api.Controllers;
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
    public class OrderService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public OrderService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? CreateOrder(Guid uIdExample, Guid uIdUser)
        {
            var example = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Example>()
                .SingleOrDefault(x => x.UidExample == uIdExample);
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>()
                .SingleOrDefault(x => x.UidUser == uIdUser);
            if (example == null || example.IsTaken || user == null) return null;
            var order = new DatabaseAccessLayer.Entities.Order
            {
                UidOrder = Guid.NewGuid(),
                Example = example,
                User = user,
                IsBack = false
            };
            example.IsTaken = true;
            _libraryWebDbContex.Add(order);
            _libraryWebDbContex.SaveChanges();
            return order.UidOrder;
        }
        public List<Contract.Order>? GetOrders()
        {
            var orders = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Order>()
                .Include(x => x.User)
                .Include(x => x.Example)
                .Include(x => x.Example.Book)
                .Include(x => x.Example.Publisher)
                .Include(x => x.Example.Book.AuthorsOfBook)
                .Include(x => x.Example.Book.GenresOfBook)
                .Include(x => x.Example.Book.TypesBkOfBook)
                .ToList();
            if (orders.Count == 0) return null; 
            return orders.Select(order => new Contract.Order
            {
                UidOrder = order.UidOrder,
                Example = new Contract.Example
                {
                    UidExample = order.Example.UidExample,
                    Book = new Contract.Book
                    {
                        UidBook = order.Example.Book.UidBook,
                        Shifr = order.Example.Book.Shifr,
                        NameOfBook = order.Example.Book.NameOfBook,
                        Authors = order.Example.Book.AuthorsOfBook.Select(x => x.NameOfAuthor).ToList(),
                        Genres = order.Example.Book.GenresOfBook.Select(x => x.NameOfGenre).ToList(),
                        TypesBk = order.Example.Book.TypesBkOfBook.Select(x => x.NameOfTypeBk).ToList()
                    },
                    Publisher = new Contract.Publisher
                    {
                        UidPublisher = order.Example.Publisher.UidPublisher,
                        NameOfPublisher = order.Example.Publisher.NameOfPublisher
                    },
                    YearOfCreation = order.Example.YearOfCreation,
                    IsTaken = order.Example.IsTaken
                },
                User = new Contract.User
                {
                    UidUser = order.User.UidUser,
                    Email = order.User.Email,
                    Name = order.User.Name,
                    Surname = order.User.Surname,
                    Otchestvo = order.User.Otchestvo,
                    Login = order.User.Login,
                    Address = order.User.Address,
                    PhoneNumber = order.User.PhoneNumber,
                    Age = order.User.Age,
                    IsAdmin = order.User.IsAdmin
                },
                IsBack = order.IsBack
            }).ToList();
        }    
        public List<Contract.Order>? GetOrders(Guid uIdUser)
        {
            var orders = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Order>()
               .Include(x => x.User)
               .Include(x => x.Example)
               .Include(x => x.Example.Book)
               .Include(x => x.Example.Publisher)
               .Include(x => x.Example.Book.AuthorsOfBook)
               .Include(x => x.Example.Book.GenresOfBook)
               .Include(x => x.Example.Book.TypesBkOfBook)
               .Where(x => x.User.UidUser == uIdUser)
               .ToList();
            if (orders.Count == 0) return null;
            return orders.Select(order => new Contract.Order
            {
                UidOrder = order.UidOrder,
                Example = new Contract.Example
                {
                    UidExample = order.Example.UidExample,
                    Book = new Contract.Book
                    {
                        UidBook = order.Example.Book.UidBook,
                        Shifr = order.Example.Book.Shifr,
                        NameOfBook = order.Example.Book.NameOfBook,
                        Authors = order.Example.Book.AuthorsOfBook.Select(x => x.NameOfAuthor).ToList(),
                        Genres = order.Example.Book.GenresOfBook.Select(x => x.NameOfGenre).ToList(),
                        TypesBk = order.Example.Book.TypesBkOfBook.Select(x => x.NameOfTypeBk).ToList()
                    },
                    Publisher = new Contract.Publisher
                    {
                        UidPublisher = order.Example.Publisher.UidPublisher,
                        NameOfPublisher = order.Example.Publisher.NameOfPublisher
                    },
                    YearOfCreation = order.Example.YearOfCreation,
                    IsTaken = order.Example.IsTaken
                },
                User = new Contract.User
                {
                    UidUser = order.User.UidUser,
                    Email = order.User.Email,
                    Name = order.User.Name,
                    Surname = order.User.Surname,
                    Otchestvo = order.User.Otchestvo,
                    Login = order.User.Login,
                    Address = order.User.Address,
                    PhoneNumber = order.User.PhoneNumber,
                    Age = order.User.Age,
                    IsAdmin = order.User.IsAdmin
                },
                IsBack = order.IsBack 
            }).ToList();
        }
        public bool CloseOrder(Guid uIdOrder)
        {
            var order = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Order>()
                .Include(x => x.Example)
                .SingleOrDefault(x => x.UidOrder == uIdOrder);
            if (order == null) return false;
            order.Example.IsTaken = false;
            order.IsBack= true;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteOrder(Guid uIdOrder)
        {
            var order = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Order>()
                .Include(x => x.User)
                .Include(x => x.Example)
                .SingleOrDefault(x => x.UidOrder == uIdOrder);
            if (order == null) return false;
            try
            {
                order.Example.IsTaken = false;
                order.IsBack = true;
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Order>().Remove(order);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}