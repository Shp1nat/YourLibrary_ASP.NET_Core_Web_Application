using Backend.Api.Contract;
using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Backend.Api.Services
{
    public class PublisherService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public PublisherService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex = libraryWebDbContex;
        }
        public Guid? CreatePublisher(string nameOfPublisher)
        {
            var publisher = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Publisher>().SingleOrDefault(x => x.NameOfPublisher == nameOfPublisher);
            if (publisher != null) return null;
            publisher = new DatabaseAccessLayer.Entities.Publisher
            {
                UidPublisher = Guid.NewGuid(),
                NameOfPublisher = nameOfPublisher
            };
            _libraryWebDbContex.Add(publisher);
            _libraryWebDbContex.SaveChanges();
            return publisher.UidPublisher;
        }
        public List<Contract.Publisher>? GetPublishers()
        {
            var publishersDAL = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Publisher>().ToList();
            List<Contract.Publisher> publishersCntx = new List<Contract.Publisher>();
            if (publishersDAL.Any())
            {
                foreach (var el in publishersDAL)
                {
                    publishersCntx.Add(new Contract.Publisher { UidPublisher = el.UidPublisher, NameOfPublisher = el.NameOfPublisher });
                }
                return publishersCntx;
            }
            return null;
        }
        public bool EditPublisher(Guid uIdPublisher, string nameOfPublisher)
        {
            var publisher = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Publisher>().SingleOrDefault(x => x.UidPublisher == uIdPublisher);
            if (publisher == null) return false;
            publisher.NameOfPublisher = nameOfPublisher;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeletePublisher(Guid uIdPublisher)
        {
            var publisher = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Publisher>().SingleOrDefault(x => x.UidPublisher == uIdPublisher);
            if (publisher == null) return false;
            try
            {
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Publisher>().Remove(publisher);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}